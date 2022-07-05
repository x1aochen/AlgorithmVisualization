using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class GameObjectPool
{
    private static GameObjectPool m_instance;
    public static GameObjectPool instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObjectPool();
            }
            return m_instance;
        }
    }
    private GameObjectPool() { }
    
    private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

    public Transform CreateObject(string key, GameObject prefab)
    {
        GameObject go = null;
        if (pool.ContainsKey(key))
        {
            go = pool[key].Find((a) => !a.activeSelf);
        }

        if (go)
        {
            go.SetActive(true);
        } else
        {
            go = GameObject.Instantiate(prefab);
            Add(key, go);
        }
        return go.transform;
    }

    private void Add(string key,GameObject go)
    {
        if (!pool.ContainsKey(key))
        {
            pool.Add(key, new List<GameObject>());
        }
        pool[key].Add(go);
    }
}

