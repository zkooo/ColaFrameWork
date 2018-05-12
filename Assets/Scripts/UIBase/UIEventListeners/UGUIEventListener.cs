using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UGUIEventListener : MonoBehaviour,
                                  IMoveHandler,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler, IPointerClickHandler,
        ISubmitHandler, ICancelHandler
{
    void Start()
    {

    }

    protected Selectable mCurSelectableComponent;

    public Selectable CurSelectable
    {
        get
        {
            if (mCurSelectableComponent != null)
                return mCurSelectableComponent;
            else
                return null;
        }
        set
        {
            mCurSelectableComponent = value;
        }
    }

    /// <summary>
    /// EventListener有一个统一的UIBase类型的uihandler来接收处理回调
    /// </summary>
    public UIBase uiHandler;

    public delegate void UIEventHandler(GameObject obj);
    public delegate void UIDragEventHandlerDetail(GameObject obj, Vector2 deltaPos, Vector2 curToucPosition);
    public UIEventHandler onClick;
    public UIEventHandler onDown;
    public UIEventHandler onUp;
    public UIDragEventHandlerDetail onDownDetail;
    public UIDragEventHandlerDetail onUpDetail;
    public UIDragEventHandlerDetail onDrag;
    public UIEventHandler onExit;
    public UIEventHandler onDrop;
    public UIEventHandler onSelect;
    public UIEventHandler onDeSelect;
    public UIEventHandler onMove;
    public UIDragEventHandlerDetail onBeginDrag;
    public UIDragEventHandlerDetail onEndDrag;
    public UIEventHandler onEnter;
    public UIEventHandler onSubmit;
    public UIEventHandler onScroll;
    public UIEventHandler onCancel;
    public UIEventHandler onUpdateSelected;
    public UIEventHandler onInitializePotentialDrag;
    /// <summary>
    /// 新增触发事件回调，参数为触发的UI事件名称，比如onClick,onBoolValueChange,onSubmit等等
    /// </summary>
    public Action<string> onEvent;

    //统一检查是否需要屏蔽点击事件
    protected bool CheckNeedHideEvent()
    {
        if (this.mCurSelectableComponent == null || !this.mCurSelectableComponent.interactable
            || !this.mCurSelectableComponent.enabled)
        {
            return true;
        }
        return false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onClick != null)
        {
            this.onClick(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onClick");
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onDown != null)
        {
            this.onDown(gameObject);
        }
        if (this.onDownDetail != null)
        {
            this.onDownDetail(gameObject, eventData.delta, eventData.position);
        }

        if (null != onEvent)
        {
            this.onEvent("onDown");
            this.onEvent("onDownDetail");
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onUp != null)
        {
            this.onUp(gameObject);
        }
        if (this.onUpDetail != null)
        {
            this.onUpDetail(gameObject, eventData.delta, eventData.position);
        }

        if (null != onEvent)
        {
            this.onEvent("onUp");
            this.onEvent("onUpDetail");
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onExit != null)
        {
            this.onExit(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onExit");
        }
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onSelect != null)
        {
            this.onSelect(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onSelect");
        }
    }



    public virtual void OnMove(AxisEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onMove != null)
        {
            this.onMove(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onMove");
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }
        if (this.onEnter != null)
        {
            this.onEnter(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onEnter");
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onSubmit != null)
        {
            this.onSubmit(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onSubmit");
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onCancel != null)
        {
            this.onCancel(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onCancel");
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }
        if (this.onDeSelect != null)
        {
            this.onDeSelect(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onDeSelect");
        }
    }


    //public static UGUIEventListener GetEventListenner(GameObject obj)
    //{
    //    UGUIEventListener listenner = obj.GetComponent<UGUIEventListener>();
    //    if (listenner == null)
    //    {
    //        listenner = obj.AddComponent<UGUIEventListener>();
    //    }
    //    return listenner;
    //}
}
