using System;
using System.Collections;
using UnityEngine;

public class MergeSort : BaseAlgorithm
{
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        ASort(0, array.Length - 1);

        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    private void ASort(int lo,int hi)
    {
        if (lo == hi)
            return;
        int mi = ((hi - lo) >> 1) + lo;
        ASort(lo, mi);
        ASort(mi + 1, hi);
        AMerge(lo, mi, hi);
    }

    private void AMerge(int lo,int mi ,int hi)
    {
        int len = hi - lo + 1;
        Transform[] arr = new Transform[len];
        int index = 0; //临时数组arr下标
        int left = lo; //原数组左指针
        int right = mi + 1; //原数组右指针
        int[] tmp = new int[len];
        for (int i = 0; i < len; i++)
        {
            tmp[i] = lo + i;
        }

        //左右互相比较，符合条件进入临时数组
        while (left <= mi && right <= hi)
        {
            arr[index++] = sortDic[array[left]] < sortDic[array[right]] ? array[left++] : array[right++];
        }

        while (left <= mi)
        {
            arr[index++] = array[left++];
        }

        while (right <= hi)
        {
            arr[index++] = array[right++];
        }

        for (int i = 0; i < hi - lo + 1; i++)
        {
            array[lo + i] = arr[i];
        }
    }

    protected override IEnumerator Sort()
    {
        InitData();

        yield return StartCoroutine(Sort(0, array.Length - 1));

        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    private IEnumerator Sort(int lo,int hi)
    {
        if (lo == hi)
            yield break;
        int mi = ((hi - lo) >> 1) + lo;
        yield return StartCoroutine(Sort(lo, mi));
        yield return StartCoroutine(Sort(mi + 1, hi));
        yield return StartCoroutine(Merge(lo, mi, hi));
    }

    private IEnumerator Merge(int lo,int mi, int hi)
    {
        int len = hi - lo + 1;
        Transform [] arr = new Transform[len];
        int index = 0; //临时数组arr下标
        int left = lo; //原数组左指针
        int right = mi + 1; //原数组右指针
        //该次参与比较的值出列
        int[] tmp = new int[len];
        for (int i = 0;i < len; i++)
        {
            tmp[i] = lo + i;
        }
        yield return StartCoroutine(SortHelper.instance.Out(tmp));

        //左右互相比较，符合条件进入临时数组
        while (left <= mi && right <= hi)
        {
            arr[index++] = sortDic[array[left]] < sortDic[array[right]] ? array[left++] : array[right++];
        }

        while (left <= mi)
        {
            arr[index++] = array[left++];
        }

        while (right <= hi)
        {
            arr[index++] = array[right++];
        }

        for (int i = 0;i < hi - lo + 1; i++)
        {
            array[lo + i] = arr[i];
            yield return StartCoroutine(SortHelper.instance.SetCubePos(lo + i, lo + i));
        }
    }

}


