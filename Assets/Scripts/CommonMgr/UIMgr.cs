using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    private static UIMgr instance;

    /// <summary>
    /// 用于存储所有的UI的字典
    /// </summary>
    private Dictionary<Type, UIBase> uiList;

    /// <summary>
    /// 用于存储参与点击其他地方关闭面板管理的UI的列表
    /// </summary>
    private List<UIBase> outTouchList;

    /// <summary>
    /// 存储要进行统一关闭面板的列表
    /// </summary>
    private List<UIBase> removeList;

    public UIMgr()
    {
        uiList = new Dictionary<Type, UIBase>();
        outTouchList = new List<UIBase>();
        removeList = new List<UIBase>();

        /*---------------UI界面控制脚本添加-------------------*/
        UIBase ui = new UILogin(100, UIType.Common);
        uiList.Add(typeof(UILogin), ui);
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

    /// <summary>
    /// 设置一个UI界面参与点击其他地方关闭面板管理
    /// </summary>
    /// <param name="ui"></param>
    public void SetOutTouchDisappear(UIBase ui)
    {
        outTouchList.Add(ui);
    }

    /// <summary>
    /// 分发处理点击其他地方关闭面板的
    /// </summary>
    /// <param name="uiName"></param>
    public void NotifyDisappear(string panelName)
    {
        removeList.Clear();
        for (int i = 0; i < outTouchList.Count; i++)
        {
            if (null != outTouchList[i] && outTouchList[i].Name != panelName)
            {
                outTouchList[i].Close();
                removeList.Add(outTouchList[i]);
                break; //每次只关闭一个界面
            }
        }

        //从outTouch列表中移除已经关闭的UI界面
        for (int i = 0; i < removeList.Count; i++)
        {
            outTouchList.Remove(removeList[i]);
        }
    }
}