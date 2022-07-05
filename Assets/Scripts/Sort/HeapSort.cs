using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 堆排序,小根堆
/// </summary>
public class HeapSort : BaseAlgorithm,ITreeDiagram
{
    private int heapSize;

    public override void ActualSort()
    {
        InitData();
        sw.Reset();
        sw.Start();
        heapSize = array.Length;
        for (int i = 0; i < array.Length; i++)
        {
            ABuildMinHeap(i);
        }
        //最小值与堆末尾元素交换，并保持小根堆
        SortHelper.instance.ANodeSwap(0, --heapSize);
        while (heapSize > 0)
        {
            AHeapify(0);
            SortHelper.instance.ANodeSwap(0, --heapSize);
        }

        sw.Stop();
        SortDone(sw.Elapsed.TotalMilliseconds);
    }

    private void ABuildMinHeap(int index)
    {
        while (sortDic[array[index]] > sortDic[array[(index - 1) / 2]])
        {
            int parent = (index - 1) / 2;
            SortHelper.instance.ANodeSwap(index, parent);
            index = parent;
        }
    }

    private void AHeapify(int index)
    {
        int left = 2 * index + 1;
        while (left < heapSize)
        {
            //选出左右较大者
            int largest = left + 1 < heapSize && sortDic[array[left + 1]] > sortDic[array[left]] ? left + 1 : left;

            largest = sortDic[array[largest]] > sortDic[array[index]] ? largest : index;

            //如果孩子中没有比自己大的
            if (largest == index)
                return;
            SortHelper.instance.ANodeSwap(index, largest);
            //继续向下比对
            index = largest;
            left = index * 2 + 1;
        }
    }



    protected override IEnumerator Sort()
    {
        InitData();
        heapSize = array.Length;
        for (int i = 0; i < array.Length; i++)
        {
            yield return StartCoroutine(BuildMinHeap(i));
        }
        Debug.Log("开始排序");
        //最小值与堆末尾元素交换，并保持小根堆
        SortHelper.instance.SetColor(Color.blue, 0, --heapSize);
        yield return StartCoroutine(SortHelper.instance.NodeSwap(0, heapSize));
        SortHelper.instance.ResetColor(0);
        while (heapSize > 0)
        {
            yield return StartCoroutine(Heapify(0));
            SortHelper.instance.SetColor(Color.blue, 0, --heapSize);
            yield return StartCoroutine(SortHelper.instance.NodeSwap(0, heapSize));
            SortHelper.instance.ResetColor(0);
        }
        SortHelper.instance.SetColor(Color.blue, 0);

        SortDone(0);
    }

    /// <summary>
    /// 构建大根堆
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator BuildMinHeap(int index)
    {
        //依次与父节点作比较，形成大根堆
        while (sortDic[array[index]] > sortDic[array[(index - 1) / 2]])
        {
            int parent = (index - 1) / 2;
            yield return StartCoroutine(SortHelper.instance.ISetColor(Color.yellow, index, parent));
            yield return StartCoroutine(SortHelper.instance.NodeSwap(index, parent));
            SortHelper.instance.ResetColor(index, parent);
            index = parent;
        }
    }
    /// <summary>
    /// 堆化
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator Heapify(int index)
    {
        int left = 2 * index + 1;
        while (left < heapSize)
        {
            //选出左右较大者
            int largest;
            if (left + 1 < heapSize)
            {
                yield return StartCoroutine(SortHelper.instance.ISetColor(Color.yellow, left, left + 1));
                largest = sortDic[array[left + 1]] > sortDic[array[left]] ? left + 1 : left;
                SortHelper.instance.ResetColor(left, left + 1);
            }
            else
            {
                largest = left;
            }
            //较大者与父亲作比较，谁大谁做父亲
            yield return StartCoroutine(SortHelper.instance.ISetColor(Color.yellow, largest, index));
            //这边重新定义了个Parent用来存储 孩子与父亲之间的最大者，是为了后面的颜色更改，与算法无关，否则正常直接用largest来存就行了
            int parent = sortDic[array[largest]] > sortDic[array[index]] ? largest : index;

            //如果孩子中没有比自己大的
            if (parent == index)
            {
                SortHelper.instance.ResetColor(index, largest);
                yield break;
            }

            yield return StartCoroutine(SortHelper.instance.NodeSwap(index, parent));
            SortHelper.instance.ResetColor(index, parent);
            //继续向下比对
            index = parent;
            left = index * 2 + 1;
        }
    }
}

