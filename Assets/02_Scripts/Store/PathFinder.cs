using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[System.Serializable]
public class PathFinderNode
{
    // 생성자
    public PathFinderNode(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public PathFinderNode ParentNode; // inspector에 노출 X

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public float F { get { return G + H; } } // set이 없기 때문에 inspector엔 노출 X
}


public class PathFinder : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos; // int 값 vector
    [SerializeField] private List<Transform> targetTr = new List<Transform>();
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private float speed;
    public List<PathFinderNode> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner; // 대각선 허용 여부

    int sizeX, sizeY;
    PathFinderNode[,] NodeArray;
    PathFinderNode StartNode, TargetNode, CurNode;
    List<PathFinderNode> OpenList, ClosedList;

    private bool _isNpcMoving;
    private int _loadIndex = 0;

    private bool _isShopping;
    private Table _tabedTable;

    private void Start()
    {
        PathFinding();
    }
    private void FixedUpdate()
    {
        if (_isNpcMoving)
        {
            // 현재 위치, 목표 위치 사이 거리 계산
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = new Vector2(targetPos.x, targetPos.y);

            if (Vector2.Distance(currentPosition, targetPosition) > 0.1f) // 임계값 설정
            {
                // NPC 위치 업데이트
                transform.position = Vector2.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                // 목표에 도달한 경우 멈춤
                transform.position = targetPosition; // 정확한 목표 위치로 설정
                _isNpcMoving = false; // 이동 상태를 false로 변경
                _loadIndex++;
                Debug.Log("Finish moving");

                Invoke("PathFinding", 3f);
            }
        }
    }

    private void Update()
    {
        if(_isShopping)
        {
            Invoke("PickBread", 1.8f);
            _isShopping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Table"))
        {
            _isShopping = true;
            _tabedTable = collision.transform.GetComponent<Table>();
        }

        if(collision.CompareTag("Counter"))
        {
            Debug.Log("buy bread");
        }
    }

    private void PickBread()
    {
        Debug.Log("pick bread");
        // tabed table에 count를 수정
    }

    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        // 배열을 생성을 해야하기 때문에 +1 해줌
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new PathFinderNode[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.1f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new PathFinderNode(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        Transform tr = targetTr[_loadIndex];

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];

        targetPos = new Vector2Int((int) tr.position.x, (int) tr.position.y);
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<PathFinderNode>() { StartNode }; // open에 시작 노드 담아주기
        ClosedList = new List<PathFinderNode>();
        FinalNodeList = new List<PathFinderNode>();

        while (OpenList.Count > 0)
        {
            // G H F
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            if (CurNode == TargetNode)
            {
                PathFinderNode TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++) Debug.Log(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                break;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }

        _isNpcMoving = true;

    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            PathFinderNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    // 기즈모로 이동 거리 표시
    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }

}