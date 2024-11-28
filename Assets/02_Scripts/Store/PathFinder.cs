using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEngine.GraphicsBuffer;


[System.Serializable]
public class PathFinderNode
{
    // ������
    public PathFinderNode(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public PathFinderNode ParentNode; // inspector�� ���� X

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, y, G, H;
    public float F { get { return G + H; } } // set�� ���� ������ inspector�� ���� X
}


public class PathFinder : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos; // int �� vector
    [SerializeField] private List<Transform> targetTr = new List<Transform>();
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private float speed;
    public List<PathFinderNode> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner; // �밢�� ��� ����

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
            // ���� ��ġ, ��ǥ ��ġ ���� �Ÿ� ���
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = new Vector2(targetPos.x, targetPos.y);

            if (Vector2.Distance(currentPosition, targetPosition) > 0.1f) // �Ӱ谪 ����
            {
                // NPC ��ġ ������Ʈ
                transform.position = Vector2.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                // ��ǥ�� ������ ��� ����
                transform.position = targetPosition; // ��Ȯ�� ��ǥ ��ġ�� ����
                _isNpcMoving = false; // �̵� ���¸� false�� ����
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
    }

    public void PathFinding()
    {
        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����
        // �迭�� ������ �ؾ��ϱ� ������ +1 ����
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

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];

        targetPos = new Vector2Int((int) tr.position.x, (int) tr.position.y);
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<PathFinderNode>() { StartNode }; // open�� ���� ��� ����ֱ�
        ClosedList = new List<PathFinderNode>();
        FinalNodeList = new List<PathFinderNode>();

        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // ������
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

                for (int i = 0; i < FinalNodeList.Count; i++) Debug.Log(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                break;
            }


            // �֢آע�
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }

        _isNpcMoving = true;

    }

    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // �밢�� ����, �� ���̷� ��� �ȵ�
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            PathFinderNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    // ������ �̵� �Ÿ� ǥ��
    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }

}