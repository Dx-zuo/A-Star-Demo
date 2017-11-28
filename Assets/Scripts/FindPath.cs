using UnityEngine;
using System.Collections.Generic;
// ReSharper disable All

/// <summary>
/// 路径查找类
/// </summary>
public class FindPath : MonoBehaviour
{
    public Transform Player, EndPoint;
    private Vector3 playerPos, endPos;

    private Grid grid;
	
	void Start ()
	{
	    grid = GetComponent<Grid>();
    }
	

	void Update () {
        // 当玩家或者结束点位置变动时，查找路径
	    if (playerPos != Player.position || endPos != EndPoint.position){
            playerPos = Player.position;
            endPos = EndPoint.position;
            FindingPath(playerPos, endPos);
	    }
	}

    /// <summary>
    /// 查找路径
    /// </summary>
    private void FindingPath(Vector3 _startPos, Vector3 _endPos)
    {
        Node startNode = grid.GetFromPostion(_startPos);
        Node endNode = grid.GetFromPostion(_endPos);

        // 开启列表
        List<Node> openSet = new List<Node>();
        // 关闭列表
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0){
            Node currentNode = openSet[0];

            // 找到F值最小的，为当前节点
            foreach (Node node in openSet) {
                if (node.F_Cost < currentNode.F_Cost ||
                    (node.F_Cost == currentNode.F_Cost && node.H_Cost < currentNode.H_Cost)){
                    currentNode = node;
                }
            }

            // 从关闭列表移除当前节点，并加入到关闭列表
            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            // 如果查询到了结束节点，则生成路径并退出
            if (currentNode == endNode){
                GeneratePath(startNode, endNode);
                return;
            }

            // 遍历当前节点的 相邻节点
            foreach (Node node in grid.GetNeibourNodes(currentNode)){
                // 排除不能通过的，和已经在关闭列表中的节点
                if (!node.CanWalk || closeSet.Contains(node)) { continue; }

                // 计算新的G Cost
                int newGCost = currentNode.G_Cost + GetNodesDistance(currentNode, node);
                // 如果节点不在开启列表，或者新的G更小，则更新
                if (!openSet.Contains(node) || newGCost < node.G_Cost){
                    node.G_Cost = newGCost;
                    node.H_Cost = GetNodesDistance(node, endNode);
                    node.Parent = currentNode;

                    if (!openSet.Contains(node)){
                        openSet.Add(node);
                    }
                }
            }

        }

    }

    /// <summary>
    /// 生成最终路径
    /// </summary>
    private void GeneratePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node temp = end;

        // 从end开始遍历每一个节点的parent，得到的就是逆向的路径节点
        while (temp != start){
            path.Add(temp);
            temp = temp.Parent;
        }
        path.Reverse();
        grid.FinalPath = path;
    }

    /// <summary>
    /// 计算两个节点间距离
    /// </summary>
    private int GetNodesDistance(Node a, Node b)
    {
        int cntX = Mathf.Abs(a.Grid_X - b.Grid_X);
        int cntY = Mathf.Abs(a.Grid_Y - b.Grid_Y);

        // 14代表斜方向长度，10代表平移长度
        if (cntX > cntY){
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else{
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }


}
