using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    private static UIMgr instance;

    /// <summary>
    /// 存储UI控制脚本的字典
    /// </summary>
    private Dictionary<Type, UIBase> uiList;

    private UIMgr()
    {
        uiList = new Dictionary<Type, UIBase>()
        {
            
        };
    }

    public static UIMgr GetInstance()
    {
        if (null == instance)
        {
            instance = new UIMgr();
        }
        return instance;
    }
}