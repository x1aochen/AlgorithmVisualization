using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
{
    //静态私有字段
    protected static T m_Instance = null;

    public static T instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("Singleton of" + 
                        typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    m_Instance.Init();
                }
            }
        
            return m_Instance;
        }
    }

    //unity系统把挂载物体上的脚本实例化，变成对象
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        Init();
    }
    //提供初始化的一种选择
    public virtual void Init() { }
    //应用程序退出做清理工作
    private void OnApplicationQuit()
    {
        m_Instance = null;    
    }
}
