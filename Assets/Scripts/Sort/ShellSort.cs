using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShellSort : BaseAlgorithm
{

    /// <summary>
    /// 获取Sedgewick增量序列
    /// </summary>
    /// <returns></returns>
    public int[] GetSedgewickStepArr(int len)
    {
        int i = 0;
        List<int> arr = new List<int>();
        while (true)
        {
            int tmp = 9 * ((1 << 2 * i) - (1 << i)) + 1;
            if (tmp <= len)
            {
                arr.Add(tmp);
            }
            tmp = (1 << 2 * i + 4) - 3 * (1 << i + 2) + 1;
            if (tmp <= len)
            {
                arr.Add(tmp);
            }
            else
                break;
            i += 1;
        }

        arr.Reverse();
        return arr.ToArray();
    }


    /// <summary>
    /// 获取Hibbard增量序列
    /// </summary>
    /// <param name="len"></param>
    /// <returns></returns>
    public int[] GetHibbardStepArr(int len)
    {
        int i = 1;
        List<int> arr = new List<int>();
        while (true)
        {
            int tmp = (1 << i) - 1;
            if (tmp <= len)
            {
                arr.Add(tmp);
            }
            else
                break;
            i += 1;
        }
        arr.Reverse();
        return arr.ToArray();
    }
    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        int[] stepArr = GetSedgewickStepArr(array.Length);

        foreach (int gap in stepArr)
        {
            for (int i = gap; i < array.Length; i++)
            {
                int j = i;
                while (j - gap >= 0 && sortDic[array[j]] < sortDic[array[j - gap]])
                {
                    SortHelper.instance.Swap<Transform>(array, j, j - gap);
                    j -= gap;
                }
            }
        }
        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    protected override IEnumerator Sort()
    {
        InitData();
        int[] stepArr = GetHibbardStepArr(array.Length);

        foreach (int gap in stepArr)
        {
            for (int i = gap; i < array.Length; i++)
            {
                int j = i;
                while (j - gap >= 0)
                {
                    yield return StartCoroutine(SortHelper.instance.Out(j, j - gap));
                    if (sortDic[array[j]] < sortDic[array[j - gap]]) 
                    {
                        yield return StartCoroutine(SortHelper.instance.Swap(array, j, j - gap));
                    }
                    yield return StartCoroutine(SortHelper.instance.Back(j, j - gap));
                    j -= gap;
                }
            }
        }
        SortDone(0);
    }
}

