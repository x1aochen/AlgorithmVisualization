using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 重写Dropdown
/// </summary>
public class MyDropdown : TMP_Dropdown, IPointerClickHandler
{
    public bool call = true;
    protected override void Awake()
    {
        base.Awake();
        template = transform.Find("Template").GetComponent<RectTransform>();
        captionText = transform.Find("Label").GetComponent<TMP_Text>();
        itemText = transform.Find("Template/Viewport/Content/Item/Item Label").GetComponent<TMP_Text>();
    }

    public void MyShow()
    {
        Show();

        Toggle[] toggles = transform.Find("Dropdown List/Viewport/Content").GetComponentsInChildren<Toggle>(false);
        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle tmp = toggles[i];
            tmp.onValueChanged.RemoveAllListeners();
            tmp.isOn = false;
            int k = i;
            toggles[i].onValueChanged.AddListener((isOn) => OnSelectToggle(k,tmp));
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        MyShow();
    }

    public void OnSelectToggle(int i,Toggle toggle)
    {
        if (!toggle.isOn)
        {
            if (value == i)
            {
                onValueChanged.Invoke(value);
                toggle.isOn = true;
            }
            return;
        }

        if (value == i)
        {
            onValueChanged.Invoke(value);
        } else
        {
            value = i;
        }

        Hide();
    }
}



