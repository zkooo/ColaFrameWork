using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI统一管理器
/// </summary>
public class UIMgr : IViewManager
{
    private static UIMgr instance;

    /// <summary>
    /// 用于存储所有的UI的字典
    /// </summary>
    private Dictionary<string, UIBase> uiList;

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
        uiList = new Dictionary<string, UIBase>();
        outTouchList = new List<UIBase>();
        removeList = new List<UIBase>();

        /*---------------UI界面控制脚本添加-------------------*/
        UIBase ui = new UILogin(100, UIType.Common);
        uiList.Add("UILogin", ui);
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

    public bool Open(string uiType)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            uiList[uiType].Open();
            return uiList[uiType].IsShow;
        }
        return false;
    }

    public void Close(string uiType)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            uiList[uiType].Close();
        }
    }

    public void Destroy(string uiType)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            uiList[uiType].Destroy();
        }
    }

    public void UpdateUI(string uiType, EventData eventData)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            uiList[uiType].UpdateUI(eventData);
        }
    }

    public UIBase GetViewByType(string uiType)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            return uiList[uiType];
        }
        Debug.LogWarning(string.Format("UIMgr中不包含类型为{0}的UI", uiType));
        return null;
    }

    public void AddView(string uiType, UIBase ui)
    {
        if (null != uiList && !uiList.ContainsKey(uiType))
        {
            uiList.Add(uiType, ui);
        }
        Debug.LogWarning(string.Format("添加类型为{0}的UI到UIMgr中失败！", uiType));
    }

    public void RemoveViewByType(string uiType)
    {
        if (null != uiList && uiList.ContainsKey(uiType))
        {
            uiList.Remove(uiType);
        }
    }

    public bool OpenUIWithReturn(string uiType)
    {
        CheckFuncResult result = CommonHelper.CheckFuncOpen(uiType, true);
        if (CheckFuncResult.True == result)
        {
            return Open(uiType);
        }
        return false;
    }
}