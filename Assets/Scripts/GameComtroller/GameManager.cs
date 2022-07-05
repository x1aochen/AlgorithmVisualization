using System;
using System.Collections;
using UnityEngine;

public enum SORTTYPE
{
    BubbleSort,
    InsertionSort,
    SelectionSort,
    QuickSort,
    MergeSort,
    HeapSort
}

public class GameManager : MonoBehaviour
{
    private bool isWork;
    private bool isPause = true;
    public BaseAlgorithm currentSort;
    private GenerateScene generater;
    private float originSwapLerp = 0.018f;
    private float originBattleLerp = 0.2f;
    private int num;
    private double sortTime;

    public void Awake()
    {
        generater = gameObject.AddComponent<GenerateScene>();
        InitSortData();
        InitUIEvent();
    }

    private void InitUIEvent()
    {
        EventManager.instance.AddListener(UIEvent.SetSortType, ChangeSortType);
        EventManager.instance.AddListener(UIEvent.GenerateScene, GeneateObj);
        EventManager.instance.AddListener(UIEvent.RandomItem, ExcuteShuffle);
        EventManager.instance.AddListener(UIEvent.PlayOrPause, GameControll);
        EventManager.instance.AddListener(UIEvent.SetSpeedMultiply, SetSpeed);
        EventManager.instance.AddListener(UIEvent.DoActualSort, DoActualSort);          
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.instance.Call(UIEvent.ChangePlayToggle, isPause == true);
        }
    }

    private void InitSortData()
    {
        currentSort = gameObject.AddComponent<BubbleSort>();
        SortHelper.instance.SwapLerp = originSwapLerp;
        SortHelper.instance.BattleLerp = originBattleLerp;
    }

    private void DoActualSort(object obj)
    {
        if (isWork || currentSort == null || num == 0)
            return;
        isWork = true;
        currentSort.sortComplete += ActualSortDone;
        currentSort.ActualSort();
    }

    private void ActualSortDone(double time)
    {
        currentSort.sortComplete -= ActualSortDone;
        isWork = false;
        SortHelper.instance.ResetPos();
        EventManager.instance.Call(UIEvent.UpdateTimer, time + "ms");
        EventManager.instance.Call(UIEvent.ChangePlayToggle, false);
        StopAllCoroutines();
    }
    /// <summary>
    /// 开始排序
    /// </summary>
    private void BeginSort()
    {
        if (currentSort == null || num == 0)
            return;
        currentSort.sortComplete += SortDone;
        isWork = true;
        currentSort.Begin();
        StartCoroutine(Timer());
    }

    /// <summary>
    /// 排序完成调用
    /// </summary>
    private void SortDone(double time)
    {
        currentSort.sortComplete -= SortDone;
        currentSort.Finish();
        isWork = false;
        EventManager.instance.Call(UIEvent.ChangePlayToggle, false);
        StopAllCoroutines();
    }
    /// <summary>
    /// 改变当前算法
    /// </summary>
    /// <param name="name"></param>
    private void ChangeSortType(object name)
    {
        Type type = Type.GetType(name.ToString());
        currentSort = GetComponent(type) as BaseAlgorithm;
        if (currentSort == null)
        {
            currentSort = gameObject.AddComponent(type) as BaseAlgorithm;
        }
        if (currentSort is ITreeDiagram)
        {
            generater.ShowTree();
        }
        else
        {
            generater.HideTree();
        }
    }
    /// <summary>
    /// 设置执行速度
    /// </summary>
    /// <param name="multiply"></param>
    private void SetSpeed(object multiply)
    {
        float speed = Mathf.Round((float)multiply * 1000) / 1000;
        SortHelper.instance.SwapLerp = originSwapLerp / speed;
        SortHelper.instance.BattleLerp = originBattleLerp * speed;
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    /// <param name="obj"></param>
    private void ExcuteShuffle(object obj)
    {
        if (isWork)
        {
            if (!isPause)
            {
                return;
            }

            SortDone(0);
        }
        EventManager.instance.Call(UIEvent.UpdateTimer, "0:0:0");
        SortHelper.instance.ShuffleSort();
    }
    /// <summary>
    /// 生成场景可视化
    /// </summary>
    /// <param name="number"></param>
    private void GeneateObj(object number)
    {
        if (isWork)
            return;

        int newNum = int.Parse(number.ToString());
        if (num == newNum) //是否输入相同数量
            return;
    
        generater.HideAll();
        num = newNum;
        generater.Num = SortHelper.instance.Num = num;
        SortHelper.instance.SetData();
        generater.GenerateHistogram();
        if (currentSort is ITreeDiagram)
            generater.ShowTree();
        else
            generater.HideTree();
    }
    /// <summary>
    /// 控制排序开始 暂停 继续
    /// </summary>
    /// <param name="isPlay"></param>
    private void GameControll(object isPlay)
    {
        if (Convert.ToBoolean(isPlay))
        {
            PlayGame();
        } else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// 开始或继续 
    /// </summary>
    private void PlayGame()
    {
        SortHelper.instance.Play();
        if (!isWork)
        {
            BeginSort();
        }
        //正在排序，显示的是pause
        isPause = false;
    }
    /// <summary>
    /// 暂停
    /// </summary>
    private void PauseGame()
    {
        SortHelper.instance.Pause();

        //暂停，显示的是play
        isPause = true;
    }
    int hour;
    int minute;
    int sencond;
    int millisecond;

    private IEnumerator Timer()
    {
        sortTime = 0;
        while (true)
        {
            
            sortTime += Time.deltaTime;
            hour = (int)sortTime / 3600;
            minute = ((int)sortTime - hour * 3600) / 60;
            sencond = (int)sortTime - hour * 3600 - minute * 60;
            millisecond = (int)((sortTime - (int)sortTime) * 1000);

            if (millisecond < 100)
            {
                EventManager.instance.Call(UIEvent.UpdateTimer, string.Format(minute + ":" + sencond + ":0" + millisecond));
            } else
            {
                EventManager.instance.Call(UIEvent.UpdateTimer, string.Format(minute + ":" + sencond + ":" + millisecond));
            }
            yield return new WaitUntil(() => isPause == false);
        }
    }
}

