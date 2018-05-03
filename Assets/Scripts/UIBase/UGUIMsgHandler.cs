using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UGUIMsgHandler : MonoBehaviour
{

    private Dictionary<GameObject, Dictionary<string, string>> m_luaOprFuncDic = new Dictionary<GameObject, Dictionary<string, string>>();

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
                else if (st is ToggleExt)
                {
                    useDrag = ((ToggleExt)st).useDrag;
                }

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

            listener.MsgHandler = this;
        }
        else
        {
            if (this == listener.MsgHandler) //如果当前的和原来的一样 就不用再touch一次
            {
                listener.CurSelectable = st;
                return;
            }
            else             //如果想touch一个新的对象 先清除掉原来的
            {
                UGUIMsgHandler prevHandler = listener.MsgHandler;
                if (prevHandler) prevHandler.RemoveEventHandler(listener.gameObject);
                listener.MsgHandler = this;
            }
        }
        //在listenner上面记录Selectable组件
        listener.CurSelectable = st;
        AddEventHandlerEx(listener);
    }

    void AddEventHandlerEx(UGUIEventListener listener)
    {
        listener.onClick += this.onClickhandle;
        listener.onDown += this.onDownhandle;
        listener.onUp += this.onUphandle;
        listener.onDownDetail += this.onDownDetailhandle;
        listener.onUpDetail += this.onUpDetailhandle;
        listener.onEnter += this.onEnterhandle;
        listener.onExit += this.onExithandle;
        listener.onDrop += this.onDrophandle;
        listener.onBeginDrag += this.onBeginDraghandle;
        listener.onDrag += this.onDraghandle;
        listener.onEndDrag += this.onEndDraghandle;
        listener.onSelect += this.onSelecthandle;
        listener.onDeSelect += this.onDeSelecthandle;
        listener.onScroll += this.onScrollhandle;
        listener.onCancel += this.onCancelhandle;
        listener.onSubmit += this.onSubmithandle;
        listener.onMove += this.onMovehandle;
        listener.onUpdateSelected += this.onUpdateSelectedhandle;
        listener.onInitializePotentialDrag += this.onInitializePotentialDragHandle;
        AddOtherEventHandler(listener.gameObject);
    }

    void AddOtherEventHandler(GameObject go)
    {
        OtherEventListenner otherlistenner = go.GetComponent<OtherEventListenner>();
        if (otherlistenner == null)
            otherlistenner = go.AddComponent<OtherEventListenner>();
        otherlistenner.inputvalueChangeAction += onStrValueChangeHandle;
        otherlistenner.inputeditEndAction += onEditEndHandle;
        otherlistenner.togglevalueChangeAction += onBoolValueChangeHandle;
        otherlistenner.slidervalueChangeAction += onFloatValueChangeHandle;
        otherlistenner.scrollbarvalueChangeAction += onFloatValueChangeHandle;
        otherlistenner.scrollrectvalueChangeAction += onRectValueChangeHandle;
        otherlistenner.dropdownvalueChangeAction += onIntValueChangeHandle;
        otherlistenner.OnPlayTweenHandle += OnPlayTweenFinishHandle;
    }
    void AddOtherEventListenner(ScrollRect rect)
    {

        OtherEventListenner otherlistenner = rect.gameObject.GetComponent<OtherEventListenner>();
        if (otherlistenner == null)
            otherlistenner = rect.gameObject.AddComponent<OtherEventListenner>();
        rect.onValueChanged.AddListener(otherlistenner.scrollrectValueChangeHandler());
        otherlistenner.scrollrectvalueChangeAction += onRectValueChangeHandle;
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
        if (listener.MsgHandler == null || listener.MsgHandler != this)        //必须在touch过同一个 MsgHandler的情况下才能用这个MsgHandler进行untouch
            return;

        listener.onClick -= this.onClickhandle;
        listener.onDown -= this.onDownhandle;
        listener.onUp -= this.onUphandle;
        listener.onEnter -= this.onEnterhandle;
        listener.onExit -= this.onExithandle;
        listener.onDrop -= this.onDrophandle;
        listener.onBeginDrag -= this.onBeginDraghandle;
        listener.onDrag -= this.onDraghandle;
        listener.onEndDrag -= this.onEndDraghandle;
        listener.onSelect -= this.onSelecthandle;
        listener.onDeSelect -= this.onDeSelecthandle;
        listener.onScroll -= this.onScrollhandle;
        listener.onCancel -= this.onCancelhandle;
        listener.onSubmit -= this.onSubmithandle;
        listener.onMove -= this.onMovehandle;
        listener.onUpdateSelected -= this.onUpdateSelectedhandle;
        listener.onInitializePotentialDrag -= this.onInitializePotentialDragHandle;

        OtherEventListenner otherlistenner = listener.gameObject.GetComponent<OtherEventListenner>();
        if (otherlistenner != null)
        {
            otherlistenner.inputvalueChangeAction -= onStrValueChangeHandle;
            otherlistenner.inputeditEndAction -= onEditEndHandle;
            otherlistenner.togglevalueChangeAction -= onBoolValueChangeHandle;
            otherlistenner.slidervalueChangeAction -= onFloatValueChangeHandle;
            otherlistenner.scrollbarvalueChangeAction -= onFloatValueChangeHandle;
            otherlistenner.scrollrectvalueChangeAction -= onRectValueChangeHandle;
            otherlistenner.dropdownvalueChangeAction -= onIntValueChangeHandle;
            otherlistenner.OnPlayTweenHandle -= OnPlayTweenFinishHandle;
        }
        if (m_luaOprFuncDic.ContainsKey(obj))
        {
            m_luaOprFuncDic.Remove(obj);
        }
        listener.MsgHandler = null;  //清除掉MsgHandler
    }
}
