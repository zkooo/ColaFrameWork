using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UGUIDragEventListenner : UGUIEventListener,IBeginDragHandler,IDragHandler,
                                  IEndDragHandler,IDropHandler,IScrollHandler,IUpdateSelectedHandler,IInitializePotentialDragHandler {

	// Use this for initialization
	void Start () {
		
	}
	
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onDrag != null)
        {
            this.onDrag(gameObject, eventData.delta,eventData.position);
        }

        if (null != onEvent)
        {
            this.onEvent("onDrag");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onDrop != null)
        {
            this.onDrop(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("OnDrop");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onBeginDrag != null)
        {
            this.onBeginDrag(gameObject, eventData.delta, eventData.position);
        }

        if (null != onEvent)
        {
            this.onEvent("onBeginDrag");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }
        if (this.onEndDrag != null)
        {
            this.onEndDrag(gameObject, eventData.delta, eventData.position);
        }

        if (null != onEvent)
        {
            this.onEvent("onEndDrag");
        }
    }

    public virtual void OnScroll(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onScroll != null)
        {
            this.onScroll(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onScroll");
        }
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onUpdateSelected != null)
        {
            this.onUpdateSelected(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onUpdateSelected");
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (CheckNeedHideEvent())
        {
            return;
        }

        if (this.onInitializePotentialDrag != null)
        {
            this.onInitializePotentialDrag(gameObject);
        }

        if (null != onEvent)
        {
            this.onEvent("onInitializePotentialDrag");
        }
    }
                
}
