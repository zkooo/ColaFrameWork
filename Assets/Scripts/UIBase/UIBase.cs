using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool IsShow { get { return Panel != null && Panel.activeSelf; } }

    /// <summary>
    /// UI的类型
    /// </summary>
    protected UILevel uiLevel;

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

    public UIBase(int resId, UILevel uiLevel)
    {
        this.uiLevel = uiLevel;
        ResId = resId;
        this.uiCreateType = UICreateType.Res;
        this.Name = CommonHelper.GetResourceMgr().GetResNameById(resId);
        InitRegisterHandler();
    }

    public UIBase(GameObject panel, GameObject parent, UILevel uiLevel)
    {
        this.uiLevel = uiLevel;
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
            this.Panel = CommonHelper.InstantiateGoByID(ResId, GUIHelper.GetUIRootObj());
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
                Debug.LogWarning(string.Format("消息{0}重复注册！", evt));
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

    /// <summary>
    /// 显示UI背景模糊
    /// </summary>
    public void ShowUIBlur()
    {
        CommonHelper.GetUIMgr().ShowUIBlur(this);
    }

    #region UI事件回调处理逻辑

    public void Touch(GameObject obj)
    {

        ScrollRect[] rects = obj.GetComponentsInChildren<ScrollRect>(true);
        for (int i = 0; i < rects.Length; i++)
        {
            AddOtherEventListenner(rects[i]);
        }
        Selectable[] selectable = obj.GetComponentsInChildren<Selectable>(true);

        foreach (Selectable st in selectable)
        {
            AddEventHandler(st);
        }
    }

    void AddEventHandler(Selectable st)
    {
        UGUIEventListener listener = st.gameObject.GetComponent<UGUIEventListener>();

        if (listener == null) //防止多次Touch
        {
            if ((st is Scrollbar) || (st is InputField) || (st is Slider))
            {
                listener = st.gameObject.AddComponent<UGUIDragEventListenner>();
            }
            else
            {
                //此处正常button是可以响应拖拽事件但有ScrollRect作为父组件的情况下会存在冲突
                bool useDrag = false;
                if (st is Button)
                {
                    ScrollRect[] rect = st.gameObject.GetComponentsInParent<ScrollRect>(true);
                    useDrag = (rect == null || rect.Length == 0);
                }
                //else if (st is ToggleExt)
                //{
                //    useDrag = ((ToggleExt)st).useDrag;
                //}

                if (useDrag)
                {
                    listener = st.gameObject.AddComponent<UGUIDragEventListenner>();
                }
                else
                {
                    listener = st.gameObject.AddComponent<UGUIEventListener>();
                    //UGUIEventListenner.GetEventListenner(obj);
                }

            }
        }
        else
        {
            if (this == listener.uiHandler) //如果当前的和原来的一样 就不用再touch一次
            {
                listener.CurSelectable = st;
                return;
            }
            else             //如果想touch一个新的对象 先清除掉原来的
            {
                UIBase prevHandler = listener.uiHandler;
                if (null != prevHandler) prevHandler.RemoveEventHandler(listener.gameObject);
                listener.uiHandler = this;
            }
        }
        //在listenner上面记录Selectable组件
        listener.CurSelectable = st;
        AddEventHandlerEx(listener);
    }

    void AddEventHandlerEx(UGUIEventListener listener)
    {
        listener.onClick += onClick;
        listener.onDown += onDown;
        listener.onUp += onUp;
        listener.onDownDetail += this.onDownDetail;
        listener.onUpDetail += this.onUpDetail;
        listener.onEnter += onEnter;
        listener.onExit += onExit;
        listener.onDrop += onDrop;
        listener.onBeginDrag += onBeginDrag;
        listener.onDrag += onDrag;
        listener.onEndDrag += onEndDrag;
        listener.onSelect += onSelect;
        listener.onDeSelect += onDeSelect;
        listener.onScroll += onScroll;
        listener.onCancel += onCancel;
        listener.onSubmit += onSubmit;
        listener.onMove += onMove;
        listener.onUpdateSelected += onUpdateSelected;
        listener.onInitializePotentialDrag += this.onInitializePotentialDrag;
        AddOtherEventHandler(listener.gameObject);
    }

    private void onInitializePotentialDrag(GameObject obj)
    {
        throw new NotImplementedException();
    }

    private void onUpDetail(GameObject obj, Vector2 deltapos, Vector2 curtoucposition)
    {
        throw new NotImplementedException();
    }

    private void onDownDetail(GameObject obj, Vector2 deltaPos, Vector2 curToucPosition)
    {
        throw new NotImplementedException();
    }

    void AddOtherEventHandler(GameObject go)
    {
        OtherEventListenner otherlistenner = go.GetComponent<OtherEventListenner>();
        if (otherlistenner == null)
            otherlistenner = go.AddComponent<OtherEventListenner>();
        otherlistenner.inputvalueChangeAction += onStrValueChange;
        otherlistenner.inputeditEndAction += onEditEnd;
        otherlistenner.togglevalueChangeAction += onBoolValueChange;
        otherlistenner.slidervalueChangeAction += onFloatValueChange;
        otherlistenner.scrollbarvalueChangeAction += onFloatValueChange;
        otherlistenner.scrollrectvalueChangeAction += onRectValueChange;
        otherlistenner.dropdownvalueChangeAction += onIntValueChange;
    }

    void AddOtherEventListenner(ScrollRect rect)
    {

        OtherEventListenner otherlistenner = rect.gameObject.GetComponent<OtherEventListenner>();
        if (otherlistenner == null)
            otherlistenner = rect.gameObject.AddComponent<OtherEventListenner>();
        rect.onValueChanged.AddListener(otherlistenner.scrollrectValueChangeHandler());
        otherlistenner.scrollrectvalueChangeAction += onRectValueChange;
    }

    public void UnTouch(GameObject obj)
    {
        Selectable[] selectable = obj.GetComponentsInChildren<Selectable>(true);

        foreach (Selectable st in selectable)
        {
            RemoveEventHandler(st.gameObject);
        }
    }

    void RemoveEventHandler(GameObject obj)
    {
        UGUIEventListener listener = obj.GetComponent<UGUIEventListener>();
        if (listener == null) return;
        if (listener.uiHandler == null || listener.uiHandler != this)        //必须在touch过同一个 MsgHandler的情况下才能用这个MsgHandler进行untouch
            return;

        listener.onClick -= onClick;
        listener.onDown -= onDown;
        listener.onUp -= onUp;
        listener.onEnter -= onEnter;
        listener.onExit -= onExit;
        listener.onDrop -= onDrop;
        listener.onBeginDrag -= onBeginDrag;
        listener.onDrag -= onDrag;
        listener.onEndDrag -= onEndDrag;
        listener.onSelect -= onSelect;
        listener.onDeSelect -= onDeSelect;
        listener.onScroll -= onScroll;
        listener.onCancel -= onCancel;
        listener.onSubmit -= onSubmit;
        listener.onMove -= onMove;
        listener.onUpdateSelected -= onUpdateSelected;
        listener.onInitializePotentialDrag -= onInitializePotentialDragHandle;

        OtherEventListenner otherlistenner = listener.gameObject.GetComponent<OtherEventListenner>();
        if (otherlistenner != null)
        {
            otherlistenner.inputvalueChangeAction -= onStrValueChange;
            otherlistenner.inputeditEndAction -= onEditEnd;
            otherlistenner.togglevalueChangeAction -= onBoolValueChange;
            otherlistenner.slidervalueChangeAction -= onFloatValueChange;
            otherlistenner.scrollbarvalueChangeAction -= onFloatValueChange;
            otherlistenner.scrollrectvalueChangeAction -= onRectValueChange;
            otherlistenner.dropdownvalueChangeAction -= onIntValueChange;
        }
    }
    #endregion

    #region UI回调事件

    protected virtual void onClick(GameObject obj)
    {
    }

    protected virtual void onDown(GameObject obj)
    {
    }

    protected virtual void onUp(GameObject obj)
    {
    }

    protected virtual void onEnter(GameObject obj)
    {
    }

    protected virtual void onInitializePotentialDragHandle(GameObject obj)
    {
    }

    protected virtual void onUpdateSelected(GameObject obj)
    {
    }

    protected virtual void onMove(GameObject obj)
    {
    }

    protected virtual void onSubmit(GameObject obj)
    {
    }

    protected virtual void onCancel(GameObject obj)
    {
    }

    protected virtual void onScroll(GameObject obj)
    {
    }

    protected virtual void onDeSelect(GameObject obj)
    {
    }

    protected virtual void onSelect(GameObject obj)
    {
    }

    protected virtual void onEndDrag(GameObject obj, Vector2 deltaPos, Vector2 curToucPosition)
    {
    }

    protected virtual void onDrag(GameObject obj, Vector2 deltaPos, Vector2 curToucPosition)
    {
    }

    protected virtual void onBeginDrag(GameObject obj, Vector2 deltaPos, Vector2 curToucPosition)
    {
    }

    protected virtual void onDrop(GameObject obj)
    {
    }

    protected virtual void onExit(GameObject obj)
    {
    }

    protected virtual void onStrValueChange(GameObject obj, string para)
    {
    }

    protected virtual void onIntValueChange(GameObject obj, int para)
    {
    }

    protected virtual void onRectValueChange(GameObject obj, Vector2 para)
    {
    }

    protected virtual void onFloatValueChange(GameObject obj, float para)
    {
    }

    protected virtual void onBoolValueChange(GameObject obj, bool para)
    {
    }

    protected virtual void onEditEnd(GameObject obj, string para)
    {
    }
    #endregion
}
