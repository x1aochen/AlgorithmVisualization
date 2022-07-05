using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择排序
/// </summary>
public class SelectionSort : BaseAlgorithm
{
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        for (int i = 0;i < array.Length; i++)
        {
            int min = i;
            for (int j = i + 1;j < array.Length; j++)
            {
                if (sortDic[array[j]] < sortDic[array[min]])
                    min = j;
            }
            SortHelper.instance.Swap<Transform>(array, i, min);

        }
        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    protected override IEnumerator Sort()
    {
        InitData();
        for (int i = 0;i < array.Length; i++)
        {
            int min = i;
            yield return StartCoroutine(SortHelper.instance.Out(min));
            //找出最小值
            for (int j = i + 1;j < array.Length; j++)
            {
                yield return StartCoroutine(SortHelper.instance.Out(j));
                if (sortDic[array[j]] < sortDic[array[min]])
                {
                    yield return StartCoroutine(SortHelper.instance.Back(min));
                    min = j;
                } else
                {
                    yield return StartCoroutine(SortHelper.instance.Back(j));
                }
            }
            if (i != min)
            {
                yield return StartCoroutine(SortHelper.instance.Out(i));
                yield return StartCoroutine(SortHelper.instance.Swap(array, i, min));
                yield return StartCoroutine(SortHelper.instance.Back(i, min));
            } else
            {
                yield return StartCoroutine(SortHelper.instance.Back(min));
            }
        }
        SortDone(0);
    }
}

