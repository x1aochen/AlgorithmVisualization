using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 冒泡排序
/// </summary>
public class BubbleSort : BaseAlgorithm
{
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        int len = array.Length - 1;
        while (len > 0)
        {
            int lo = -1;
            int last = lo;
            while (++lo < len)
            {
                if (sortDic[array[lo]] > sortDic[array[lo + 1]])
                {
                    SortHelper.instance.Swap<Transform>(array, lo, lo + 1);
                    last = lo + 1;
                }
            }
            len = last - 1;
        }
        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    protected override IEnumerator Sort()
    {
        InitData();
        int len = array.Length - 1;
        while (len > 0)
        {
            int lo = -1;
            int last = lo;
            while (++lo < len)
            {
                yield return StartCoroutine(SortHelper.instance.Out(lo, lo + 1));
                if (sortDic[array[lo]] > sortDic[array[lo + 1]])
                {
                    yield return StartCoroutine(SortHelper.instance.Swap(array,lo, lo + 1));
                    last = lo + 1;
                }
                yield return StartCoroutine(SortHelper.instance.Back(lo,lo + 1));
            }

            len = last - 1;
        }
        SortDone(0);
    }
}

