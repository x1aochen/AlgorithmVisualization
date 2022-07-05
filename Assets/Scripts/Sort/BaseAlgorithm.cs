using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAlgorithm:MonoBehaviour
{
    protected Dictionary<Transform, int> sortDic;
    protected Transform[] array;
    public delegate void Complete(double time);
    public event Complete sortComplete;

    protected Stopwatch sw = new Stopwatch();

    public void Begin()
    {
        StartCoroutine(Sort());
    }

    protected abstract IEnumerator Sort();

    public abstract void ActualSort();
    
    protected void InitData()
    {
        sortDic = SortHelper.instance.sortDic;
        array = SortHelper.instance.array;
    }
    protected void SortDone(double time)
    {
        if (sortComplete != null)
        {
            UnityEngine.Debug.Log("Sort Completed");
            sortComplete(time);
        }
    }

    public void Finish()
    {
        StopAllCoroutines();
    }

}

