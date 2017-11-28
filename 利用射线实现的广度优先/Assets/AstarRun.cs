// ReSharper disable All
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AstarRun : MonoBehaviour
{
    public GameObject _intMapCube;

    private int[,] Map;

    public GameObject start;

    public GameObject end;

    //是否有障碍物
    private bool IsWall;
    //位置
    private Vector3 pos;
    //格子坐标
    public int x, y;

    private int GCost;
    private int HCaost;

    private int FCaost
    {
        get
        {
            return this.GCost + this.HCaost;
        }
    }

    public NodeItem parendt;
    public NodeItem(bool isWall, Vector3 pos, int x, int y)
    {
        this.IsWall = IsWall;
        this.pos = pos;
        this.x = x;
        this.y = y;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
