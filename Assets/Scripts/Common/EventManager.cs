using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIEvent
{
    SetSortType,
    GenerateScene,
    RandomItem,
    PlayOrPause,
    SetSpeedMultiply,
    ChangePlayToggle,
    DoActualSort,
    UpdateTimer,
    UpdateLog,
    UnLockButton
}

public class EventManager
{
    private static EventManager m_instance = null;
    public static EventManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new EventManager();
            }
            return m_instance;
        }
    }
    private EventManager() { }

    private Dictionary<UIEvent, Action<object>> events = new Dictionary<UIEvent, Action<object>>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="func"></param>
    public void AddListener(UIEvent ui,Action<object> func)
    {
        if (events.ContainsKey(ui))
        {
            events[ui] += func;
        }
        else
        {
            events.Add(ui, func);
        }
    }
    /// <summary>
    /// 调用事件
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="objs"></param>
    public void Call(UIEvent ui,object objs = null)
    {
        if (events.ContainsKey(ui))
        {
            events[ui](objs);
        }
        else
        {
            Debug.LogError(ui + "事件不存在");
        }
    }

}

