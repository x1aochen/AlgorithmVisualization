using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TransformHelper
{

    public static Transform FindChild(Transform trans, string name)
    {
        Transform child = trans.Find(name);

        if (child != null)
        {
            return child;
        }

        Transform go;

        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, name);

            if (go != null)
            {
                return go;
            }
        }

        return null;
    }
}

