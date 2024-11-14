//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class PathFinderNode // 타일 하나하나를 의미하는 클래스
//{
//    public bool isWall; // 벽인지
//    public PathFinderNode ParentNode; // 경로 추적을 위해 이전 노드 저장
//    public int x, y, G, H; // 노드좌표(x, y), 시작노드~현재노드(g), 현재노드~목표노드(h)
//    public int F // 총 추적 노드
//    {
//        get { return G + H; }
//    }

//    // 생성자
//    public PathFinderNode(bool isWall, int x, int y)
//    {
//        this.isWall = isWall;
//        this.x = x;
//        this.y = y;
//    }
//}

//public class GameManager_A_StartTest : MonoBehaviour
//{
//    public Vector2Int bottomLeft, topRight; // 가로, 세로
//    public Vector2Int startPos, targetPos; // 시작, 목표
//    public List<PathFinderNode> FinalNodeList; // 최단 경로를 담는 리스트
//    public bool allowDiagonal, dontCrossCorner; // 대각선 허용, 대각선 허용하되 모서리 닿기 여부

//    int sizeX, sizeY;
//    PathFinderNode[,] NodeArray;
//    PathFinderNode StartNode, TargetNode, CurNode;
//    List<PathFinderNode> OpenList, ClosedList; // 갈 수 있는 곳, 갈 수 없는 곳을 담는 리스트

//    // 맵 크기를 계산하고 각 위치에 Note 객체 생성
//    // 물리적인 충돌체를 통해 벽 노드 식별
//    // 열린 리스트에서 가장 비용이 적은 노드 선택(최단거리) 및 목표 노드까지 인접한 노드 검사
//    // 목표 노드에 도달하면 경로를 추적해서 FinalNodeList에 저장
//    public void PathFinding()
//    {
//        // 맵 크기 계산
//        sizeX = topRight.x - bottomLeft.x + 1;
//        sizeY = topRight.y - bottomLeft.y + 1;
//        NodeArray = new PathFinderNode[sizeX, sizeY]; // 맵 크기를 담은 2차원 배열

//        // 맵 초기화
//        // 갈 수 있는지 없는지 체크
//        for (int i = 0; i < sizeX; i++)
//        {
//            for (int j = 0; j < sizeY; j++)
//            {
//                bool isWall = false;
//                // 현재 좌표인 i에 bottomLeft.x를 더해주면 실제 월드 좌표값이 나옴
//                // 0.4f만한 원을 생성해서 그 안에 포함된 콜라이더의 레이어가 Wall이면 걸을 수 없는 구역으로 설정
//                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
//                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

//                // 걸을 수 있는지 없는지 여부를 NodeArray에 저장
//                NodeArray[i, j] = new PathFinderNode(isWall, i + bottomLeft.x, j + bottomLeft.y);
//            }
//        }

//        // 월드 좌표 to 배열 : 지점 - 월드 좌표
//        // 배열 to 월드 좌표 : 지점 + 월드좌표

//        // 시작, 목표지점 배열 값을 얻음
//        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
//        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

//        OpenList = new List<PathFinderNode>() { StartNode }; // 탐색 좌표
//        ClosedList = new List<PathFinderNode>(); // 탐색 끝난 좌표
//        FinalNodeList = new List<PathFinderNode>(); // 최종 좌표

//        // 이동 가능한 좌표에 1개라도 있으면 계속 반복하면서 값을 찾음
//        while (OpenList.Count > 0)
//        {
//            // 현재 처리중인 노드
//            CurNode = OpenList[0];
//            for (int i = 1; i < OpenList.Count; i++)
//                // OpenList 총 이동 값이 현재 이동 값거나 작거나 OpenList의 현재 ~ 목적지 거리가 현재 선택된 현재 ~ 목적지 거리보다 작으면
//                // 현재 선택된 노드에 다음 노드 값으로 변경함(더 좋은거 찾았다는 의미)
//                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

//            // 처리 완료 했으니 OpenList에서 삭제 및 ClosedList에 추가
//            OpenList.Remove(CurNode);
//            ClosedList.Add(CurNode);

//            // 만약 목표 노드에 도착하면?
//            if (CurNode == TargetNode)
//            {
//                // 타겟 노드 저장
//                PathFinderNode TargetCurNode = TargetNode;

//                // 타겟 노드부터 출발 노드까지 쭈루룩 FinalNodeList에 담기
//                while (TargetCurNode != StartNode)
//                {
//                    // 최종 경로를 FinalNodeList에 담기
//                    FinalNodeList.Add(TargetCurNode);
//                    // ParentNode는 이전 노드. 이전 노드를 계속 추적하면서 담음
//                    TargetCurNode = TargetCurNode.ParentNode;
//                }
//                // 마지막 시작 노드까지 추가하고, 뒤집어주면 완성!
//                FinalNodeList.Add(StartNode);
//                FinalNodeList.Reverse();

//                // 리스트에 담긴 순서대로 출력
//                for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
//                return;
//            }

//            // 도착 안했다면?
//            // 대각성 허용시
//            if (allowDiagonal)
//            {
//                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
//                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
//                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
//                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
//            }

//            // 대각선 비허용시
//            OpenListAdd(CurNode.x, CurNode.y + 1);
//            OpenListAdd(CurNode.x + 1, CurNode.y);
//            OpenListAdd(CurNode.x, CurNode.y - 1);
//            OpenListAdd(CurNode.x - 1, CurNode.y);
//        }
//    }
    
//    // 특정 좌표의 노드를 열린 리스트에 추가
//    // 열린 리스트에 없으면 비용과 부모 노드를 업데이트
//    void OpenListAdd(int checkX, int checkY)
//    {
//        // 탐색 할 노드의 x, y좌표가 맵 범위에 있는지 확인
//        // 노드가 이동 가능하고 ClosedList에 포함되지 않아야 진행
//        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
//        {
//            // 대각선이 허용되면? 대각선으로 이동하는 경로에 벽이 있는지 확인
//            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

//            // 코너를 가로지르지 않도록 설정된 경우는
//            // 수직, 수평으로 이동하는 경로에 벽이 없을때만 이동
//            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

//            // 이웃 노드
//            // 직선 이동은 10
//            // 대각선 이동은 14 비용
//            PathFinderNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
//            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

//            // 이웃 노드를 OpenList에 추가
//            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
//            {
//                NeighborNode.G = MoveCost;
//                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
//                NeighborNode.ParentNode = CurNode;

//                OpenList.Add(NeighborNode);
//            }
//        }
//    }

//    // 기즈모를 통해 경로를 시각화
//    // FinalNodeList를 따라 선을 그림
//    void OnDrawGizmos()
//    {
//        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
//                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
//    }
//}