using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainView :MonoBehaviour
{
    private TMP_Text timer;
    private TMP_Text log;
    private int excuteCount; //实际时间的执行次数
    private void Awake()
    {
        timer = TransformHelper.FindChild(transform,"Timer").GetComponentInChildren<TMP_Text>();
        EventManager.instance.AddListener(UIEvent.UpdateTimer, UpdateTime);
        
    }

    private void UpdateTime(object time)
    {
        timer.text = time.ToString();
    }
    


}

