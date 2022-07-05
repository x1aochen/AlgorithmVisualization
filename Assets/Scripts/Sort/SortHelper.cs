using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SortHelper
{
    #region singleton
    private static SortHelper m_instance;
    public static SortHelper instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new SortHelper();
            }
            return m_instance;
        }
    }

    private SortHelper()
    {
        wait = new WaitForSeconds(0.2f);
        until = new WaitUntil(() => play == true);
        sortDic = new Dictionary<Transform, int>();
        array = new Transform[0];
        nodeArray = new Transform[0];
    }
    #endregion

    #region Data

    /// <summary>
    /// 添加元素
    /// </summary>
    /// <param name="i"></param>
    /// <param name="cube"></param>
    public void AddItem(int i, Transform cube)
    {

        if (sortDic.ContainsKey(cube))
        {
            return;
        }

        sortDic.Add(cube, i);
        array[i - 1] = cube;
    }

    public void SetData()
    {
        sortDic.Clear();
        Array.Clear(nodeArray, 0, nodeArray.Length);
        Array.Clear(array,0, nodeArray.Length);
        Array.Resize(ref nodeArray, num);
        Array.Resize(ref array, num);
    }
    public Dictionary<Transform, int> sortDic; //用于比较大小的字典，也可以直接比对transfrom的Y轴缩放
    public Transform[] array; //Cube存储数组
    public Transform[] nodeArray; //树节点存储数组
    private float refCubeSwap; //SmoothDamp传入参数
    private Vector3 refNodeSwap; ////SmoothDamp传入参数
    private WaitForSeconds wait; //树节点比对时颜色的显示时间
    private WaitUntil until; //暂停直道play为true才向下执行
    private bool play = true;
    private Color originColor; //树节点原始颜色
    private Transform treeStartPos; //树节点生成起始点
    private int treeHeight; //树高
    private Vector3 offset; //Cube相对父物体偏移量
    private float battleLerp; //Cube出队比较大小的速度
    private float swapLerp; //Cube交换时的速度
    private int num; //总大小
    public float nodeDistanceY;
    public Color OriginColor
    {
        set => originColor = value;
    }
    public Transform TreeStartPos
    {
        set => treeStartPos = value;
    }
    public int TreeHeight
    {
        set => treeHeight = value;
    }
    public float BattleLerp
    {
        set => battleLerp = value;
    }
    public float SwapLerp
    {
        set => swapLerp = value;
    }
    public int Num
    {
        set => num = value;
    }
    public Vector3 Offset
    {
        set => offset = value;
    }
    public float NodeDistanceY
    {
        set => nodeDistanceY = value;
    }

//----------------------------------------------------------------------------------
    public void Play()
    {
        play = true;
    }

    public void Pause()
    {
        play = false;
    }
    #endregion

    #region Battle
    /// <summary>
    /// 出队比较
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public IEnumerator Out(params int[] index)
    {
        if (index.Length <= 0)
            yield break;
        Vector3[] pos = new Vector3[index.Length];
        for (int i = 0; i < index.Length; i++)
        {
            pos[i] = array[index[i]].localPosition - Vector3.forward;
        }

        while (array[index[0]].localPosition.z >= -0.95)
        {
            for (int i = 0; i < index.Length; i++)
            {
                array[index[i]].localPosition = Vector3.Lerp(array[index[i]].localPosition, pos[i], battleLerp);
            }
            yield return null;
        }
        for (int i = 0; i < index.Length; i++)
        {
            array[index[i]].localPosition = pos[i];
        }

    }
    /// <summary>
    /// 回归原位
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public IEnumerator Back(params int[] index)
    {
        if (index.Length > 0)
        {
            Vector3[] pos = new Vector3[index.Length];
            for (int i = 0; i < index.Length; i++)
            {
                pos[i] = array[index[i]].localPosition + Vector3.forward;
            }
            while (array[index[0]].localPosition.z <= -0.05)
            {
                for (int i = 0; i < index.Length; i++)
                {
                    array[index[i]].localPosition = Vector3.Lerp(array[index[i]].localPosition, pos[i], battleLerp);;
                }
                yield return until;
            }

            for (int i = 0; i < index.Length; i++)
            {
                array[index[i]].localPosition = pos[i];
            }
        }
    }

    #endregion


    #region Swap


    /// <summary>
    /// 交换位置
    /// </summary>
    /// <param name="array"></param>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public IEnumerator Swap(Transform[] array, int t1, int t2)
    {
        if (t1 == t2)
            yield break;

        Swap<Transform>(array, t1, t2);

        float v1 = array[t1].localPosition.x;
        float v2 = array[t2].localPosition.x;
        Vector3 vv1 = new Vector3();
        Vector3 vv2 = new Vector3();
        while (Mathf.Abs(v2 - array[t1].localPosition.x) > 0.05)
        {
            vv1.Set(Mathf.SmoothDamp(array[t1].localPosition.x, v2, ref refCubeSwap, swapLerp), array[t1].localPosition.y, array[t1].localPosition.z);
            array[t1].localPosition = vv1;
            vv2.Set(Mathf.SmoothDamp(array[t2].localPosition.x, v1, ref refCubeSwap, swapLerp), array[t2].localPosition.y, array[t2].localPosition.z);
            array[t2].localPosition = vv2;
            yield return until;
        }
        vv1.Set(v2, array[t1].localPosition.y, array[t1].localPosition.z);
        vv2.Set(v1, array[t2].localPosition.y, array[t2].localPosition.z);
        array[t1].localPosition = vv1;
        array[t2].localPosition = vv2;
    }

    /// <summary>
    /// 节点的交换
    /// </summary>
    /// <param name="nodeArray"></param>
    /// <param name="array"></param>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public IEnumerator NodeSwap(int t1, int t2)
    {
        if (t1 == t2)
            yield break;
        Swap<Transform>(array, t1, t2);

        float x1 = array[t1].localPosition.x;
        float x2 = array[t2].localPosition.x;
        Vector3 vv1 = new Vector3();
        Vector3 vv2 = new Vector3();
        Swap<Transform>(nodeArray, t1, t2);

        Vector3 v1 = nodeArray[t1].position;
        Vector3 v2 = nodeArray[t2].position;

        while (Vector3.Distance(nodeArray[t1].position, v2) > 0.05)
        {
            nodeArray[t1].position = Vector3.SmoothDamp(nodeArray[t1].position, v2, ref refNodeSwap, swapLerp);
            nodeArray[t2].position = Vector3.SmoothDamp(nodeArray[t2].position, v1, ref refNodeSwap, swapLerp);
            vv1.Set(Mathf.SmoothDamp(array[t1].localPosition.x, x2, ref refCubeSwap, swapLerp), array[t1].localPosition.y, array[t1].localPosition.z);
            array[t1].localPosition = vv1;
            vv2.Set(Mathf.SmoothDamp(array[t2].localPosition.x, x1, ref refCubeSwap, swapLerp), array[t2].localPosition.y, array[t2].localPosition.z);
            array[t2].localPosition = vv2;
            yield return until;
        }

        nodeArray[t1].position = v2;
        nodeArray[t2].position = v1;
        vv1.Set(x2, array[t1].localPosition.y, array[t1].localPosition.z);
        vv2.Set(x1, array[t2].localPosition.y, array[t2].localPosition.z);
        array[t1].localPosition = vv1;
        array[t2].localPosition = vv2;
    }

    public void ANodeSwap(int t1,int t2)
    {
        Swap<Transform>(array, t1, t2);
        Swap<Transform>(nodeArray, t1, t2);
    }
    public void Swap<T>(T[] array, int t1, int t2)
    {
        T tmp = array[t1];
        array[t1] = array[t2];
        array[t2] = tmp;
    }
    #endregion


    #region Shuffle
    /// <summary>
    /// 洗牌
    /// </summary>
    public void ShuffleSort()
    {
        for (int i = 0; i < num; i++)
        {
            int n = UnityEngine.Random.Range(i, num);
            ShuffleCube(i, n);
            if (nodeArray[i] != null)
            {
                Swap<Transform>(nodeArray, i, n);
                ResetColor(i, n);
            }
        }
        if (nodeArray[0] != null)
        {
            nodeArray[0].position = treeStartPos.position;
            ResetNodePos(0, treeHeight, nodeArray[0].position);
        }
    }
    /// <summary>
    /// 重置树节点位置
    /// </summary>
    /// <param name="i"></param>
    /// <param name="floor"></param>
    /// <param name="parent"></param>
    public void ResetNodePos(int i, int floor,Vector3 parent)
    {
        int k = 2 * i + 1;
        float nodeOffset; //节点相对父的偏移量
        if (floor == 1)
            nodeOffset = 1f; //最底层为1
        else
            nodeOffset = (1 << floor) / 2; //每层树节点之间的间距
        if (k < num)
        {
            nodeArray[k].position = parent + new Vector3(-nodeOffset, 0, nodeDistanceY);
            ResetColor(k);
            ResetNodePos(k, floor - 1, nodeArray[k].position);
        }
        
        if (k + 1 < num)
        {
            nodeArray[k + 1].position = parent + new Vector3(nodeOffset, 0, nodeDistanceY);
            ResetNodePos(k + 1, floor - 1, nodeArray[k + 1].position);
            ResetColor(k + 1);
        }
    }

    private void ShuffleCube(int i, int n)
    {
        Swap<Transform>(array, i, n);
        //重置位置，如果直接使用交换位置，在暂停时候洗牌的话位置就不对了
        //{
        //    Vector3 tmp = array[i].position;
        //    array[i].position = array[n].position;
        //    array[n].position = tmp;
        //}SS
        array[i].localPosition = new Vector3((offset * i).x, array[i].localPosition.y, 0);
        array[n].localPosition = new Vector3((offset * n).x, array[n].localPosition.y, 0);
    }
    private void ResetCubePos()
    {
        for (int i = 0; i < num; i++)
        {
            array[i].localPosition = new Vector3((offset * i).x, array[i].localPosition.y, 0);
        }
    }

    public void ResetPos()
    {
        ResetCubePos();
        if (treeStartPos.childCount > 0)
        {
            nodeArray[0].position = treeStartPos.position;
            ResetNodePos(0, treeHeight, nodeArray[0].position);
        }
    }
    #endregion

    #region Color
    /// <summary>
    /// 比对时，等待
    /// </summary>
    /// <param name="color"></param>
    /// <param name="arr"></param>
    /// <returns></returns>
    public IEnumerator ISetColor(Color color, params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            nodeArray[arr[i]].GetComponent<Renderer>().material.color = nodeArray[arr[i]].GetComponent<Renderer>().material.color = color;
        }
        yield return wait;
    }

    /// <summary>
    /// 交换时，直接改变
    /// </summary>
    /// <param name="color"></param>
    /// <param name="arr"></param>
    public void SetColor(Color color, params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            nodeArray[arr[i]].GetComponent<Renderer>().material.color = color;
        }
    }

    public void ResetColor(params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (nodeArray[i] != null)
                nodeArray[arr[i]].GetComponent<Renderer>().material.color = originColor;

        }
    }
    #endregion

    #region  SetCubePosition
    /// <summary>
    /// 单Cube移动，
    /// </summary>
    /// <param name="t">偏移量 offset * t</param>
    /// <param name="i">索引</param>
    /// <returns></returns>
    public IEnumerator SetCubePos(int t, int i)
    {
        Vector3 pos = new Vector3();
        Vector2 target = new Vector2((offset * t).x, (offset * t).z);
        Vector2 self = new Vector2(array[i].localPosition.x, array[i].localPosition.z);
        while (Vector2.Distance(self, target) > 0.05)
        {
            pos.Set(Mathf.SmoothDamp(array[i].localPosition.x, target.x, ref refCubeSwap, swapLerp),
                array[i].localPosition.y,
                Mathf.SmoothDamp(array[i].localPosition.z, target.y, ref refCubeSwap, swapLerp)
                );
            array[i].localPosition = pos;

            self.Set(array[i].localPosition.x, array[i].localPosition.z);
            yield return until;
        }
        pos.Set(Mathf.SmoothDamp(array[i].localPosition.x, target.x, ref refCubeSwap, swapLerp),
                array[i].localPosition.y,
                Mathf.SmoothDamp(array[i].localPosition.z, target.y, ref refCubeSwap, swapLerp)
                );
        array[i].localPosition = pos;
        
    }

    #endregion

    
}

