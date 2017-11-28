using UnityEngine;
using System.Collections.Generic;
// ReSharper disable All

/// <summary>
/// 地图网格类
/// </summary>
public class Grid : MonoBehaviour
{
    public LayerMask UnWalkableLayer; // 障碍物层

    private Node[,] grid; // 网格数组
    
    public Vector2 GridSize = new Vector2(10, 10); // 网格大小
    public float NodeRadius = 0.2f; //节点半径
    private float nodeDiameter; //节点直径
    private int gridCntX, gridCntY; // 在x，y方向上格子的数量
    private Vector3 startPos; // 网格起始位置

    public Transform Player;

    // 最终路径
    public List<Node> FinalPath;

	void Start ()
	{
	    nodeDiameter = NodeRadius * 2;
	    gridCntX = Mathf.RoundToInt(GridSize.x / nodeDiameter);
        gridCntY = Mathf.RoundToInt(GridSize.y / nodeDiameter);

        grid = new Node[gridCntX, gridCntY];
	    CreateGrid();
	}

    /// <summary>
    /// 生成网格
    /// </summary>
    private void CreateGrid()
    {
        //起始位置，左下角
        startPos = new Vector3(transform.position.x - GridSize.x / 2, 0, transform.position.z - GridSize.y / 2);

        for (int i = 0; i < gridCntX; i++){
            float xPos = startPos.x + i * nodeDiameter + NodeRadius;

            for (int j = 0; j < gridCntY; j++){
                float zPos = startPos.z + j * nodeDiameter + NodeRadius;
                Vector3 nodePos = new Vector3(xPos, 0, zPos);

                // 通过在节点位置发射球射线，检测是否可以通过。
                bool walkable = !Physics.CheckSphere(nodePos, NodeRadius, UnWalkableLayer);

                // 初始化每个节点
                grid[i, j] = new Node(walkable, nodePos, i, j);
            }
        }
    }

    /// <summary>
    /// 返回当前结点的周围八个节点
    /// </summary>
    public List<Node> GetNeibourNodes(Node node)
    {
        List<Node> neibourList = new List<Node>();

        // 遍历九个节点
        for (int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++){
                //排除中心的节点
                if (i == 0 && j == 0){ continue; }

                int tempX = node.Grid_X + i;
                int tempY = node.Grid_Y + j;

                // 检测该节点没有超出范围
                if (tempX < gridCntX && tempX > 0 && 
                    tempY < gridCntY && tempY > 0){
                    neibourList.Add(grid[tempX, tempY]);
                }
            }
        }

        return neibourList;
    }


    /// <summary>
    /// 根据位置获得节点
    /// </summary>
    public Node GetFromPostion(Vector3 position)
    {
        // 所在位置相对起始位置的百分比
        float percentX = (position.x - startPos.x) / GridSize.x;
        float percentY = (position.z - startPos.z) / GridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // 求出所在位置的节点 序号
        int x = Mathf.RoundToInt((gridCntX - 1) * percentX);
        int y = Mathf.RoundToInt((gridCntY - 1) * percentY);
        return grid[x, y];
    }


    private void OnDrawGizmos()
    {
        // 绘制网格边框
        Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, 1, GridSize.y));

        if (grid == null || grid.Length == 0){
            return;
        }

        // 求出玩家所在节点
        Node playerNode = GetFromPostion(Player.position);

        // 绘制每个节点
        foreach (Node node in grid){

            // 根据是否可以通过设置不同颜色
            Gizmos.color = node.CanWalk ? Color.white : Color.red;

            // 设置路径颜色
            if (FinalPath != null && FinalPath.Count > 0){
                if (FinalPath.Contains(node)){
                    Gizmos.color = Color.black;
                }
            }
            
            // 设置玩家节点的颜色
            if (playerNode != null && playerNode.CanWalk && playerNode == node){
                Gizmos.color = Color.cyan;
            }

            // 绘制立方体，并留出0.1的缝隙
            Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - 0.1f));
        }
    }

}
