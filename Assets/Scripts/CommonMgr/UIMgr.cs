using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventType = ColaFrame.EventType;

/// <summary>
/// UI统一管理器
/// </summary>
public class UIMgr : IViewManager, IEventHandler
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

    /// <summary>
    /// 存储统一隐藏/恢复显示的UI列表
    /// </summary>
    private List<UIBase> recordList;

    /// <summary>
    /// 消息-回调函数字典，接收到消息后调用字典中的回调方法
    /// </summary>
    protected Dictionary<string, MsgHandler> msgHanderDic;

    public UIMgr()
    {
        uiList = new Dictionary<string, UIBase>();
        outTouchList = new List<UIBase>();
        removeList = new List<UIBase>();
        recordList = new List<UIBase>();
        InitRegisterHandler();

        /*---------------UI界面控制脚本添加-------------------*/
        UIBase ui = new UILogin(100, UILevel.Common);
        uiList.Add("UILogin", ui);
    }

    /// <summary>
    /// 初始化注册消息监听
    /// </summary>
    protected void InitRegisterHandler()
    {
        msgHanderDic = null;
        GameEventMgr.GetInstance().RegisterHandler(this, EventType.UIMsg);
        msgHanderDic = new Dictionary<string, MsgHandler>()
        {
            {"OpenUIWithReturn",data=> { OpenUIWithReturn(data.ParaList[0] as string); }},
            {"CloseUI",data=>{Close(data.ParaList[0] as string);}},
        };
    }

    /// <summary>
    /// 处理消息的函数的实现
    /// </summary>
    /// <param name="gameEvent"></param>事件
    /// <returns></returns>是否处理成功
    public bool HandleMessage(GameEvent evt)
    {
        bool handled = false;
        if (EventType.UIMsg == evt.EventType)
        {
            if (null != msgHanderDic)
            {
                EventData eventData = evt.Para as EventData;
                if (null != eventData && msgHanderDic.ContainsKey(eventData.Cmd))
                {
                    msgHanderDic[eventData.Cmd](eventData);
                    handled = true;
                }
            }
        }
        return handled;
    }

    /// <summary>
    /// 是否处理了该消息的函数的实现
    /// </summary>
    /// <returns></returns>是否处理
    public bool IsHasHandler(GameEvent evt)
    {
        bool handled = false;
        if (EventType.UIMsg == evt.EventType)
        {
            if (null != msgHanderDic)
            {
                EventData eventData = evt.Para as EventData;
                if (null != eventData && msgHanderDic.ContainsKey(eventData.Cmd))
                {
                    handled = true;
                }
            }
        }
        return handled;
    }

    /// <summary>
    /// 设置一个UI界面参与点击其他地方关闭面板管理
    /// </summary>
    /// <param name="ui"></param>
    public void SetOutTouchDisappear(UIBase ui)
    {
        if (!outTouchList.Contains(ui))
        {
            outTouchList.Add(ui);
        }
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

    /// <summary>
    /// 记录并隐藏除了指定类型的当前显示的所有UI；
    /// </summary>
    public void StashAndHideAllUI(string uiType, params string[] extUITypes)
    {
        recordList.Clear();
        using (var enumator = uiList.GetEnumerator())
        {
            while (enumator.MoveNext())
            {
                if (enumator.Current.Value.IsShow &&
                    !CommonHelper.IsArrayContainString(enumator.Current.Key, extUITypes))
                {
                    recordList.Add(enumator.Current.Value);
                    enumator.Current.Value.Show(false);
                }
            }
        }
    }

    /// <summary>
    /// 恢复显示之前记录下来的隐藏UI
    /// </summary>
    public void PopAndShowAllUI()
    {
        if (null == recordList || recordList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < recordList.Count; i++)
        {
            recordList[i].Show(true);
        }
        recordList.Clear();
    }

    /// <summary>
    /// 统一关闭属于某一UI层
    /// </summary>
    /// <param name="level"></param>
    public void CloseUIByLevel(UILevel level)
    {
        if (null != uiList)
        {
            using (var enumator = uiList.GetEnumerator())
            {
                while (enumator.MoveNext())
                {
                    var ui = enumator.Current.Value;
                    if (level == ui.UILevel)
                    {
                        ui.Close();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 显示UI背景模糊
    /// </summary>
    /// <param name="ui"></param>
    public void ShowUIBlur(UIBase ui)
    {
        string uiBlurName = string.Format("blur_{0}", ui.Name);
        GameObject uiBlurObj = CommonHelper.FindChildByPath(ui.Panel, uiBlurName);
        if (null != uiBlurObj)
        {
            RawImage rawImage = uiBlurObj.GetComponent<RawImage>();
            SetBlurRawImage(rawImage);
        }
        else
        {
            CreateUIBlur(ui, uiBlurName);
        }
    }

    /// <summary>
    /// 创建UI背景模糊
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="blurName"></param>
    private void CreateUIBlur(UIBase ui, string blurName)
    {
        GameObject uiBlurObj = new GameObject(blurName);
        uiBlurObj.transform.SetParent(ui.Panel.transform, false);
        uiBlurObj.layer = ui.Layer;
        RawImage rawImage = uiBlurObj.AddComponent<RawImage>();
        Button button = uiBlurObj.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        RectTransform rectTransform = uiBlurObj.GetComponent<RectTransform>();
        if (null == rectTransform)
        {
            rectTransform = uiBlurObj.AddComponent<RectTransform>();
        }
        rectTransform.anchorMin = new Vector2(-0.1f, -0.1f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.SetAsFirstSibling();
        SetBlurRawImage(rawImage);
    }

    /// <summary>
    /// 设置背景模糊RawImage
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="blurName"></param>
    /// <returns></returns>
    private void SetBlurRawImage(RawImage rawImage)
    {
        if (null != rawImage)
        {
            rawImage.gameObject.SetActive(false);
            RenderTexture texture = GUIHelper.GetModelOutlineCameraObj().GetComponent<ImageEffectUIBlur>().FinalTexture;
            if (texture)
            {
                rawImage.texture = texture;
            }
            rawImage.gameObject.SetActive(true);
        }
    }


}