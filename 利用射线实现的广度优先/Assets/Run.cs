// ReSharper disable All

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{

    public GameObject start;

    public GameObject end;

    private bool isfind;
    /// <summary>
    /// 存放已经探索过的坐标
    /// </summary>
    private List<Vector3> Waylist;
    /// <summary>
    /// 存放标记的坐标点
    /// </summary>
    private List<Vector3> WayQueue;

    Transform root;

    // Breadth First Search, 广度优先搜索
    int[,] bfs_search = null;

    private int[][] map;
    void Start () {
        Waylist = new List<Vector3>();
        WayQueue = new List<Vector3>();
        WayQueue.Add(this.start.transform.position);
        Rayfind(this.start.transform.position);
        //StartCoroutine(BFS());
    }
    void Update ()
	{
	    
        //rangeFind(transform.position);
        //rangeFind(transform.position);
        //Rayfind(transform.position);
    }


    IEnumerator BFS()
    {

        while (!isfind)
        {
            Debug.Log(this.isfind);
            if (isfind)
            {
                break;
            }
            for (int i = 0; i < WayQueue.Count; i++)
            {
                yield return new WaitForSeconds(0f);
                Rayfind(WayQueue[i]);
            }
            WayQueue.Clear();
        }
    }

    IEnumerator DrawPath(Vector3 vec)
    {
        yield return null;
        ispath(vec);
    }

    void ispath(Vector3 vec)
    {
        if (this.start.GetComponent<Collider>().bounds.Contains(vec+Vector3.up))
        {
            return;
        }


        if (this.Waylist.Contains(vec + Vector3.left))
        {
            Instantiate(this.end, vec + Vector3.up, Quaternion.identity);
            ispath(vec + Vector3.left);
            return;
        }
        if (this.Waylist.Contains(vec + Vector3.right))
        {
            Instantiate(this.end, vec + Vector3.up, Quaternion.identity);
            ispath(vec + Vector3.right);
            return;
        }
        if (this.Waylist.Contains(vec + Vector3.forward))
        {
            Instantiate(this.end, vec + Vector3.up, Quaternion.identity);
            ispath(vec + Vector3.forward);
            return;
        }
        if (this.Waylist.Contains(vec + Vector3.down))
        {
            Instantiate(this.end, vec + Vector3.up, Quaternion.identity);
            ispath(vec + Vector3.down);
            return;
        }
    }

    void Rayfind(Vector3 vec)
    {
        
        for (int i = 0; i < 4; i++)
        {
            Vector3 RayEnd = Quaternion.AngleAxis(90.0f *i, Vector3.up) * Vector3.forward;
            Debug.DrawRay(vec + Vector3.down*0.5f, RayEnd, Color.red);
            //Debug.Log(RayEnd);
            RaycastHit hit;
            if (Physics.Raycast(vec + Vector3.down * 0.5f, RayEnd,out hit,1f))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.bounds.Contains(this.end.transform.position));
                    if (hit.collider.bounds.Intersects(this.end.GetComponent<Collider>().bounds))
                    {

                        isfind = true;
                        this.StopAllCoroutines();
                        StartCoroutine(DrawPath(hit.collider.transform.position));
                    }

                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.grey;

                        if (!this.Waylist.Contains(hit.collider.transform.position))
                        {
                            this.Waylist.Add(hit.collider.transform.position);
                            WayQueue.Add(hit.collider.transform.position);
                        }
                }
            }
        }
    }
}
