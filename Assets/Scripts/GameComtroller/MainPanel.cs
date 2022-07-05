using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainPanel:MonoBehaviour
{
    private string play = "Play";
    private string pause = "Pause";

    private TMP_Dropdown dropdown;
    private TMP_InputField numberText;
    private Button create;
    private Button shuffle;
    private Toggle gameControl;
    private Slider speedSlider;
    private TMP_Text speedText;
    private TMP_Text playOrPauseText;
    private Button actualTime;

    private void Awake()
    {
        dropdown = TransformHelper.FindChild(transform, "SortType").GetComponent<MyDropdown>();
        numberText = TransformHelper.FindChild(transform, "Number").GetComponent<TMP_InputField>();
        create = TransformHelper.FindChild(transform, "Create").GetComponent<Button>();
        shuffle = TransformHelper.FindChild(transform, "Shuffle").GetComponent<Button>();
        gameControl = TransformHelper.FindChild(transform, "Play").GetComponent<Toggle>();
        playOrPauseText = gameControl.GetComponentInChildren<TMP_Text>();
        speedSlider = TransformHelper.FindChild(transform, "Slider").GetComponent<Slider>();
        speedText = speedSlider.GetComponentInChildren<TMP_Text>();
        actualTime = TransformHelper.FindChild(transform, "ActualTime").GetComponent<Button>();
        InitEvent();
    }

    private void InitEvent()
    {
        dropdown.ClearOptions();

        foreach (SORTTYPE sort in Enum.GetValues(typeof(SORTTYPE)))
        {
            TMP_Dropdown.OptionData op = new TMP_Dropdown.OptionData();
            op.text = sort.ToString();
            dropdown.options.Add(op);
        }
        dropdown.onValueChanged.AddListener((int index) => ChangeSortType(index));
        create.onClick.AddListener(GenerateScene);
        shuffle.onClick.AddListener(() => EventManager.instance.Call(UIEvent.RandomItem));
        gameControl.onValueChanged.AddListener((bool isOn) => PlayOrPauseGame(isOn));
        EventManager.instance.AddListener(UIEvent.ChangePlayToggle, ChangePlayToggle);
        speedSlider.onValueChanged.AddListener((value) => SetSpeed(value));
        speedSlider.value = 2; //初始速度
        actualTime.onClick.AddListener(DoActualSort);
    }
    /// <summary>
    /// 做真实运算
    /// </summary>
    private void DoActualSort()
    {
        if (unlock)
        {
            EventManager.instance.Call(UIEvent.DoActualSort);
        }
    }

    /// <summary>
    /// 生成可视化
    /// </summary>
    private void GenerateScene()
    {
        if (numberText.text == "" || numberText.text == "0")
            return;
        EventManager.instance.Call(UIEvent.GenerateScene, numberText.text);
        unlock = true;
    }
    private bool unlock;

    private void SetSpeed(float value)
    {
        float speed = 0;
        switch (value)
        {
            case 0:
                speed = 0.5f;
                break;
            case 1:
                speed = 0.75f;
                break;
            case 2:
                speed = 1f;
                break;
            case 3:
                speed = 1.25f;
                break;
            case 4:
                speed = 1.5f;
                break;
            case 5:
                speed = 2f;
                break;
        }

        EventManager.instance.Call(UIEvent.SetSpeedMultiply, speed);
        speedText.text = speed.ToString() + "x";
    }
    /// <summary>
    /// 游戏是否执行
    /// </summary>
    /// <param name="isOn"></param>
    private void PlayOrPauseGame(bool isOn)
    {
        if (!unlock)
            return;
        EventManager.instance.Call(UIEvent.PlayOrPause, isOn);
        if (isOn)
        {
            playOrPauseText.text = pause;
        } else
        {
            playOrPauseText.text = play;
        }
    }
    /// <summary>
    /// 键盘也可以改变游戏状态
    /// </summary>
    /// <param name="isOn"></param>
    private void ChangePlayToggle(object isOn)
    {
        gameControl.isOn = Convert.ToBoolean(isOn);
    }

    private void ChangeSortType(int index)
    {
        if (Enum.IsDefined(typeof(SORTTYPE), index))
        {
            EventManager.instance.Call(UIEvent.SetSortType, (SORTTYPE)index);
        }
        else
        {
            Debug.LogError($"{index}下标不存在于枚举SORTTYPE中");
        }
    }



   

}