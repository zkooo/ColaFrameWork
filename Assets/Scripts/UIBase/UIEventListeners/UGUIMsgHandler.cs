using System;
using UnityEngine;
using UnityEngine.UI;

public class UGUIMsgHandler : MonoBehaviour, IUGUIEventHandler
{
    /// <summary>
    /// 注册UIEventListener
    /// </summary>
    /// <param name="obj"></param>
    public void AttachListener(GameObject obj)
    {
        ScrollRect[] rects = obj.GetComponentsInChildren<ScrollRect>(true);
        for (int i = 0; i < rects.Length; i++)
        {
            AddOtherEventListenner(rects[i]);
        }
        Selectable[] selectable = obj.GetComponentsInChildren<Selectable>(true);

        foreach (Selectable st in selectable)
        {
            AddEventaHandler(st);
        }
    }

    void AddEventaHandler(Selectable st)
    {
        UGUIEventListener listener = st.gameObject.GetComponent<UGUIEventListener>();

        if (listener == null) //防止多次AttachListener
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

                if (useDrag)
                {
                    listener = st.gameObject.AddComponent<UGUIDragEventListenner>();
                }
                else
                {
                    listener = st.gameObject.AddComponent<UGUIEventListener>();
                }

            }
            listener.uiHandler = this;
        }
        else
        {
            if (this == listener.uiHandler) //如果当前的和原来的一样 就不用再Attach一次
            {
                listener.CurSelectable = st;
                return;
            }
            else             //如果想Attach一个新的对象 先清除掉原来的
            {
                IUGUIEventHandler prevHandler = listener.uiHandler;
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
        listener.onEvent += onEvent;
        AddOtherEventHandler(listener.gameObject);
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
        otherlistenner.onEvent += onEvent;
    }

    void AddOtherEventListenner(ScrollRect rect)
    {

        OtherEventListenner otherlistenner = rect.gameObject.GetComponent<OtherEventListenner>();
        if (otherlistenner == null)
            otherlistenner = rect.gameObject.AddComponent<OtherEventListenner>();
        rect.onValueChanged.AddListener(otherlistenner.scrollrectValueChangeHandler());
        otherlistenner.scrollrectvalueChangeAction += onRectValueChange;
        otherlistenner.onEvent += onEvent;
    }

    /// <summary>
    /// 反注册UIEventListener
    /// </summary>
    /// <param name="obj"></param>
    public void UnAttachListener(GameObject obj)
    {
        Selectable[] selectable = obj.GetComponentsInChildren<Selectable>(true);

        foreach (Selectable st in selectable)
        {
            RemoveEventHandler(st.gameObject);
        }
    }

    public void RemoveEventHandler(GameObject obj)
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
        listener.onEvent -= onEvent;

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
            otherlistenner.onEvent -= onEvent;
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

    protected virtual void onStrValueChange(GameObject obj, string text)
    {
    }

    protected virtual void onIntValueChange(GameObject obj, int value)
    {
    }

    public Action<GameObject, Vector2> onRectValueChange;
    public Action<GameObject, float> onFloatValueChange;
    public Action<GameObject, bool> onBoolValueChange;
    public StrValueChangeAction onEditEnd;
    public UIEventHandler onInitializePotentialDrag;
    public Action<GameObject, Vector2, Vector2> onUpDetail;
    public Action<GameObject, Vector2, Vector2> onDownDetail;

    /// <summary>
    /// 触发UI事件时会触发onEvent方法(在需要的事件里面添加即可)
    /// </summary>
    public Action<string> onEvent;

    #endregion
}
