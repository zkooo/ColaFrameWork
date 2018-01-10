using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

/// <summary>
/// UI基类
/// </summary>
public class UIBase : IEventHandler
{
    /// <summary>
    /// 当前的UI界面GameObject
    /// </summary>
    public GameObject Panel { get; protected set; }

    /// <summary>
    /// 当前的UI界面GameObject对应的唯一资源ID
    /// </summary>
    public int ResId { get; protected set; }

    /// <summary>
    /// UI界面的名字
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// UI界面的深度
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// UI界面的层级
    /// </summary>
    public int Layer
    {
        get { return this.Panel.layer; }
        set { this.Panel.layer = value; }
    }

    /// <summary>
    ///  UI的显隐状态
    /// </summary>
    public bool IsShow { get { return Panel.activeSelf; } }

    /// <summary>
    /// UI的类型
    /// </summary>
    protected UIType uiType;

    /// <summary>
    /// UI的创建方法
    /// </summary>
    protected UICreateType uiCreateType = UICreateType.Res;

    /// <summary>
    /// 消息-回调函数字典，接收到消息后调用字典中的回调方法
    /// </summary>
    protected Dictionary<string, MsgHandler> msgHanderDic;

    /// <summary>
    /// 消息传递过来的数据
    /// </summary>
    protected EventData eventData;

    protected UIBase(int resId, UIType uiType)
    {
        this.uiType = uiType;
        ResId = resId;
        this.uiCreateType = UICreateType.Res;

        InitRegisterHandler();
    }

    protected UIBase(GameObject panel, GameObject parent, UIType uiType)
    {
        this.uiType = uiType;
        this.ResId = 0;
        this.Panel = panel;
        this.Panel.transform.SetParent(parent.transform);
        this.Name = panel.name;
        this.uiCreateType = UICreateType.Go;

        InitRegisterHandler();
    }

    /// <summary>
    /// 打开一个UI界面
    /// </summary>
    public virtual void Open()
    {
        this.Create();
    }

    /// <summary>
    /// 关闭一个UI界面
    /// </summary>
    public virtual void Close()
    {
        this.Destroy();
    }

    /// <summary>
    /// UI界面显隐的时候会调用该方法
    /// </summary>
    /// <param name="isShow"></param>UI的是否显示
    public virtual void OnShow(bool isShow)
    {

    }

    /// <summary>
    /// 创建UI界面
    /// </summary>
    public virtual void Create()
    {
        if (UICreateType.Res == uiCreateType)
        {
            if (null != this.Panel)
            {
                GameObject.Destroy(Panel);
            }

        }
        else if (UICreateType.Go == uiCreateType)
        {

        }

        this.OnCreate();
    }

    /// <summary>
    /// UI创建过程中会调用该方法
    /// </summary>
    public virtual void OnCreate()
    {
        this.OnShow(IsShow);
    }

    /// <summary>
    /// 销毁一个UI界面
    /// </summary>
    public virtual void Destroy()
    {
        this.UnRegisterHandler();
        if (null != Panel)
        {
            GameObject.Destroy(Panel);
        }
        Panel = null;
        this.OnDestroy();
    }

    /// <summary>
    /// 销毁UI界面的过程中调用该方法
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// 设置一个UI界面的显隐
    /// </summary>
    /// <param name="isActive"></param>UI界面是否显示
    public void SetActive(bool isActive)
    {
        this.Panel.SetActive(isActive);
        this.OnShow(isActive);
    }

    /// <summary>
    /// 刷新UI界面
    /// </summary>
    /// <param name="eventData"></param>消息数据
    public virtual void UpdateUI(EventData eventData)
    {
        if (null == eventData) return;
        this.eventData = eventData;
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
    /// 初始化注册消息监听
    /// </summary>
    protected void InitRegisterHandler()
    {
        msgHanderDic = null;
        GameEventMgr.GetInstance().RegisterHandler(this, EventType.UIMsg);
        msgHanderDic = new Dictionary<string, MsgHandler>()
        {
            {Name+"Open",data=>Open()},
            {Name+"Close",data=>Close()},
            {Name+"UpdateUI",data=>UpdateUI(data)},
        };
    }

    /// <summary>
    /// 取消注册该UI监听的所有消息
    /// </summary>
    protected void UnRegisterHandler()
    {
        GameEventMgr.GetInstance().UnRegisterHandler(this);

        if (null != msgHanderDic)
        {
            msgHanderDic.Clear();
            msgHanderDic = null;
        }
    }

    /// <summary>
    /// 注册一个UI界面上的消息
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="msgHandler"></param>
    public void RegisterEvent(string evt, MsgHandler msgHandler)
    {
        if (null != msgHandler && null != msgHanderDic)
        {
            if (!msgHanderDic.ContainsKey(evt))
            {
                msgHanderDic.Add(Name + evt, msgHandler);
            }
            else
            {
                Debug.LogWarning(string.Format("消息{0}重复注册！",evt));
            }
        }
    }

    /// <summary>
    /// 取消注册一个UI界面上的消息
    /// </summary>
    /// <param name="evt"></param>
    public void UnRegisterEvent(string evt)
    {
        if (null != msgHanderDic)
        {
            msgHanderDic.Remove(Name + evt);
        }
    }
}
