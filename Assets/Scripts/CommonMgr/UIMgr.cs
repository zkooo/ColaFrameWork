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

    /// <summary>
    /// 根据UI的Type，打开一个UI界面，并返回界面是否打开成功
    /// </summary>
    /// <param name="uiType"></param>
    /// <returns></returns>
    public bool OpenUIWithReturn(Type uiType)
    {
        if (null != uiList)
        {
            if (uiList.ContainsKey(uiType))
            {
                uiList[uiType].Open();
                return uiList[uiType].IsShow;
            }
        }
        return false;
    }
}