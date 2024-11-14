//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class PathFinderNode // Ÿ�� �ϳ��ϳ��� �ǹ��ϴ� Ŭ����
//{
//    public bool isWall; // ������
//    public PathFinderNode ParentNode; // ��� ������ ���� ���� ��� ����
//    public int x, y, G, H; // �����ǥ(x, y), ���۳��~������(g), ������~��ǥ���(h)
//    public int F // �� ���� ���
//    {
//        get { return G + H; }
//    }

//    // ������
//    public PathFinderNode(bool isWall, int x, int y)
//    {
//        this.isWall = isWall;
//        this.x = x;
//        this.y = y;
//    }
//}

//public class GameManager_A_StartTest : MonoBehaviour
//{
//    public Vector2Int bottomLeft, topRight; // ����, ����
//    public Vector2Int startPos, targetPos; // ����, ��ǥ
//    public List<PathFinderNode> FinalNodeList; // �ִ� ��θ� ��� ����Ʈ
//    public bool allowDiagonal, dontCrossCorner; // �밢�� ���, �밢�� ����ϵ� �𼭸� ��� ����

//    int sizeX, sizeY;
//    PathFinderNode[,] NodeArray;
//    PathFinderNode StartNode, TargetNode, CurNode;
//    List<PathFinderNode> OpenList, ClosedList; // �� �� �ִ� ��, �� �� ���� ���� ��� ����Ʈ

//    // �� ũ�⸦ ����ϰ� �� ��ġ�� Note ��ü ����
//    // �������� �浹ü�� ���� �� ��� �ĺ�
//    // ���� ����Ʈ���� ���� ����� ���� ��� ����(�ִܰŸ�) �� ��ǥ ������ ������ ��� �˻�
//    // ��ǥ ��忡 �����ϸ� ��θ� �����ؼ� FinalNodeList�� ����
//    public void PathFinding()
//    {
//        // �� ũ�� ���
//        sizeX = topRight.x - bottomLeft.x + 1;
//        sizeY = topRight.y - bottomLeft.y + 1;
//        NodeArray = new PathFinderNode[sizeX, sizeY]; // �� ũ�⸦ ���� 2���� �迭

//        // �� �ʱ�ȭ
//        // �� �� �ִ��� ������ üũ
//        for (int i = 0; i < sizeX; i++)
//        {
//            for (int j = 0; j < sizeY; j++)
//            {
//                bool isWall = false;
//                // ���� ��ǥ�� i�� bottomLeft.x�� �����ָ� ���� ���� ��ǥ���� ����
//                // 0.4f���� ���� �����ؼ� �� �ȿ� ���Ե� �ݶ��̴��� ���̾ Wall�̸� ���� �� ���� �������� ����
//                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
//                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

//                // ���� �� �ִ��� ������ ���θ� NodeArray�� ����
//                NodeArray[i, j] = new PathFinderNode(isWall, i + bottomLeft.x, j + bottomLeft.y);
//            }
//        }

//        // ���� ��ǥ to �迭 : ���� - ���� ��ǥ
//        // �迭 to ���� ��ǥ : ���� + ������ǥ

//        // ����, ��ǥ���� �迭 ���� ����
//        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
//        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

//        OpenList = new List<PathFinderNode>() { StartNode }; // Ž�� ��ǥ
//        ClosedList = new List<PathFinderNode>(); // Ž�� ���� ��ǥ
//        FinalNodeList = new List<PathFinderNode>(); // ���� ��ǥ

//        // �̵� ������ ��ǥ�� 1���� ������ ��� �ݺ��ϸ鼭 ���� ã��
//        while (OpenList.Count > 0)
//        {
//            // ���� ó������ ���
//            CurNode = OpenList[0];
//            for (int i = 1; i < OpenList.Count; i++)
//                // OpenList �� �̵� ���� ���� �̵� ���ų� �۰ų� OpenList�� ���� ~ ������ �Ÿ��� ���� ���õ� ���� ~ ������ �Ÿ����� ������
//                // ���� ���õ� ��忡 ���� ��� ������ ������(�� ������ ã�Ҵٴ� �ǹ�)
//                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

//            // ó�� �Ϸ� ������ OpenList���� ���� �� ClosedList�� �߰�
//            OpenList.Remove(CurNode);
//            ClosedList.Add(CurNode);

//            // ���� ��ǥ ��忡 �����ϸ�?
//            if (CurNode == TargetNode)
//            {
//                // Ÿ�� ��� ����
//                PathFinderNode TargetCurNode = TargetNode;

//                // Ÿ�� ������ ��� ������ �޷�� FinalNodeList�� ���
//                while (TargetCurNode != StartNode)
//                {
//                    // ���� ��θ� FinalNodeList�� ���
//                    FinalNodeList.Add(TargetCurNode);
//                    // ParentNode�� ���� ���. ���� ��带 ��� �����ϸ鼭 ����
//                    TargetCurNode = TargetCurNode.ParentNode;
//                }
//                // ������ ���� ������ �߰��ϰ�, �������ָ� �ϼ�!
//                FinalNodeList.Add(StartNode);
//                FinalNodeList.Reverse();

//                // ����Ʈ�� ��� ������� ���
//                for (int i = 0; i < FinalNodeList.Count; i++) print(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
//                return;
//            }

//            // ���� ���ߴٸ�?
//            // �밢�� ����
//            if (allowDiagonal)
//            {
//                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
//                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
//                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
//                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
//            }

//            // �밢�� ������
//            OpenListAdd(CurNode.x, CurNode.y + 1);
//            OpenListAdd(CurNode.x + 1, CurNode.y);
//            OpenListAdd(CurNode.x, CurNode.y - 1);
//            OpenListAdd(CurNode.x - 1, CurNode.y);
//        }
//    }
    
//    // Ư�� ��ǥ�� ��带 ���� ����Ʈ�� �߰�
//    // ���� ����Ʈ�� ������ ���� �θ� ��带 ������Ʈ
//    void OpenListAdd(int checkX, int checkY)
//    {
//        // Ž�� �� ����� x, y��ǥ�� �� ������ �ִ��� Ȯ��
//        // ��尡 �̵� �����ϰ� ClosedList�� ���Ե��� �ʾƾ� ����
//        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
//        {
//            // �밢���� ���Ǹ�? �밢������ �̵��ϴ� ��ο� ���� �ִ��� Ȯ��
//            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

//            // �ڳʸ� ���������� �ʵ��� ������ ����
//            // ����, �������� �̵��ϴ� ��ο� ���� �������� �̵�
//            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

//            // �̿� ���
//            // ���� �̵��� 10
//            // �밢�� �̵��� 14 ���
//            PathFinderNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
//            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

//            // �̿� ��带 OpenList�� �߰�
//            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
//            {
//                NeighborNode.G = MoveCost;
//                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
//                NeighborNode.ParentNode = CurNode;

//                OpenList.Add(NeighborNode);
//            }
//        }
//    }

//    // ����� ���� ��θ� �ð�ȭ
//    // FinalNodeList�� ���� ���� �׸�
//    void OnDrawGizmos()
//    {
//        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
//                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
//    }
//}