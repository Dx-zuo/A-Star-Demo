using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

/// <summary>
/// 寻路节点
/// </summary>

    public class NodeItem
    {
        // 是否是障碍物
        public bool isWall;
        // 位置
        public Vector3 pos;
        // 格子坐标
        public int x, y;

        // 与起点的长度
        public int gCost;
        // 与目标点的长度
        public int hCost;

        // 总的路径长度
        public int fCost
        {
            get { return gCost + hCost; }
        }

        // 父节点
        public NodeItem parent;

        public NodeItem(bool isWall, Vector3 pos, int x, int y)
        {
            this.isWall = isWall;
            this.pos = pos;
            this.x = x;
            this.y = y;
        }

    }

