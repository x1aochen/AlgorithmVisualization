using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 插入排序
/// </summary>
public class InsertionSort : BaseAlgorithm
{
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        for (int i = 1; i < array.Length; i++)
        {
            int tmp = sortDic[array[i]];
            for (int j = i;j > 0; j--)
            {
                if (sortDic[array[j - 1]] > tmp)
                {
                    SortHelper.instance.Swap<Transform>(array, j, j - 1);
                }
            }
        }
        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    protected override IEnumerator Sort()
    {
        InitData();
        for (int i = 1;i < array.Length; i++)
        {
            int k = i; //用来记录当前主角位置，以便最后回到原位置
            yield return StartCoroutine(SortHelper.instance.Out(i));
            int tmp = sortDic[array[i]];

            //逐个依次往前比
            for (int j = i; j > 0; j--)
            {
                yield return StartCoroutine(SortHelper.instance.Out(j - 1));
                //有比主角大的，就做交换
                if (sortDic[array[j - 1]] > tmp)
                {
                    yield return StartCoroutine(SortHelper.instance.Swap(array,j,j - 1));
                    yield return StartCoroutine(SortHelper.instance.Back(j));
                    k--;
                }
                else
                {
                    yield return StartCoroutine(SortHelper.instance.Back(j - 1));
                }        
            }

            yield return StartCoroutine(SortHelper.instance.Back(k));
        }
        SortDone(0);
    }
}
