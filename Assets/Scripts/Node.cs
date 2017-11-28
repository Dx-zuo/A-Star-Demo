// ReSharper disable All
using UnityEngine;

/// <summary>
/// 节点类
/// </summary>
public class Node
{

    public readonly bool CanWalk; // 是否可以通过
    public readonly int Grid_X, Grid_Y; // 序号
    public Vector3 Position; // 位置

    public int G_Cost; //与起点的距离 
    public int H_Cost; //与终点的距离 

    // 总消耗
    public int F_Cost
    {
        get { return G_Cost + H_Cost; }
    }

    public Node Parent; //父节点

    public Node(bool canWalk, Vector3 position, int x, int y)
    {
        CanWalk = canWalk;
        Position = position;
        Grid_X = x;
        Grid_Y = y;
    }
}
