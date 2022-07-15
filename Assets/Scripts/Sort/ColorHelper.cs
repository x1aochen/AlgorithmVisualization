using System;
using System.Collections;
using UnityEngine;

public class ColorHelper : MonoSingleton<ColorHelper>
{
    private Color originColor; //树节点原始颜色
    private WaitForSeconds wait;
    public override void Init()
    {
        base.Init();
        wait = new WaitForSeconds(0.2f);
    }

    public Color OriginColor
    {
        set => originColor = value;
    }
    #region Color
    /// <summary>
    /// 比对时，等待
    /// </summary>
    /// <param name="color"></param>
    /// <param name="arr"></param>
    /// <returns></returns>
    public IEnumerator ISetColor(Transform[] array, Color color, params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            array[arr[i]].GetComponent<Renderer>().material.color = array[arr[i]].GetComponent<Renderer>().material.color = color;
        }
        yield return wait;
    }

    /// <summary>
    /// 交换时，直接改变
    /// </summary>
    /// <param name="color"></param>
    /// <param name="arr"></param>
    public void SetColor(Transform[] array,Color color, params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            array[arr[i]].GetComponent<Renderer>().material.color = color;
        }
    }

    public void ResetColor(Transform[] array,params int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (array[i] != null)
                array[arr[i]].GetComponent<Renderer>().material.color = originColor;

        }
    }
    #endregion
}

