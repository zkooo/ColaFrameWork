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

    public UIMgr()
    {
        uiList = new Dictionary<Type, UIBase>();

        /*---------------UI界面控制脚本添加-------------------*/
        UIBase ui = new UILogin(100,UIType.Common);
        uiList.Add(typeof(UILogin),ui);
    }
}