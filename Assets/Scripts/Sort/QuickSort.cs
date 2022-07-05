using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 快速排序
/// </summary>
public class QuickSort : BaseAlgorithm
{
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        AQuickSort(0, array.Length - 1);

        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    private void AQuickSort(int lo,int hi)
    {
        if (lo >= hi)
            return;

        int pivot = sortDic[array[lo]];
        int left = lo + 1;
        int right = hi;

        while (left <= right)
        {
            //左部分找比轴点大的值
            while (left <= right && sortDic[array[left]] < pivot)
            {
                left++;
            }
            //右部分找比轴点小的值
            while (left <= right && sortDic[array[right]] > pivot)
            {
                right--;
            }

            if (left <= right)
            {
                SortHelper.instance.Swap<Transform>(array, left, right);
                left++;
                right--;
            }
        }

        SortHelper.instance.Swap<Transform>(array, lo, right);

        AQuickSort(lo, right - 1);
        AQuickSort(right + 1, hi);
    }

    protected override IEnumerator Sort()
    {
        InitData();
        yield return StartCoroutine(DoQuickSort(0, array.Length - 1));
        SortDone(0);
    }

    private IEnumerator DoQuickSort(int lo,int hi)
    {
        if (lo >= hi)
            yield break;
        //int rand = Random.Range(lo, hi + 1);
        //yield return StartCoroutine(SortHelper.instance.Out(array[lo], array[rand]));
        //yield return StartCoroutine(SortHelper.instance.Swap(array, lo, rand));
        //yield return StartCoroutine(SortHelper.instance.Back(array[rand], array[lo]));
        int pivot = sortDic[array[lo]];
        int left = lo + 1;
        int right = hi;

        while (left <= right)
        {
            while (left <= right && sortDic[array[left]] < pivot)
            {
                left++;
            }

            while (left <= right && sortDic[array[right]] > pivot)
            {
                right--;
            }

            if (left <= right)
            {
                yield return StartCoroutine(SortHelper.instance.Out(left,right));
                yield return StartCoroutine(SortHelper.instance.Swap(array, left, right));
                yield return StartCoroutine(SortHelper.instance.Back(left, right));
                left++;
                right--;
            }
        }

        yield return StartCoroutine(SortHelper.instance.Out(lo,right));
        yield return StartCoroutine(SortHelper.instance.Swap(array, lo, right));
        yield return StartCoroutine(SortHelper.instance.Back(lo, right));

        yield return StartCoroutine(DoQuickSort(lo, right - 1));
        yield return StartCoroutine(DoQuickSort(right + 1, hi));
    }

}
