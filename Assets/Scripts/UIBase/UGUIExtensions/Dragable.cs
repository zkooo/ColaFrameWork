using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 可拖动组件，适用于UI
/// </summary>
[RequireComponent(typeof(Image))]
public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;

    private Camera uiCamera;

    public delegate void OnBeginDragInLua(GameObject pointerDrag);
    public OnBeginDragInLua onBeginDragInLua;

    public delegate void OnDragingInLua(GameObject pointerDrag);
    public OnDragingInLua onDragingInLua;

    public delegate void OnDragEndInLua(GameObject pointerDrag, GameObject targetGo);
    public OnDragEndInLua onDragEndInLua;

    void Awake()
    {
        uiCamera = GUIHelper.GetUICamera();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 curTouchedWorldPos = uiCamera.ScreenToWorldPoint(eventData.position);
        offset = transform.position - curTouchedWorldPos;
        offset.z = 0;
        GetComponent<Image>().raycastTarget = false;
        SetDraggedPosition(eventData);
        if (onBeginDragInLua != null)
            onBeginDragInLua(eventData.pointerDrag);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        if (onDragingInLua != null)
            onDragingInLua(eventData.pointerDrag);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = true;
        SetDraggedPosition(eventData);
        if (onDragEndInLua != null)
            onDragEndInLua(eventData.pointerDrag, eventData.pointerEnter);
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        Vector3 worldpos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, uiCamera, out worldpos))
        {
            transform.position = worldpos + offset;
        }
    }
}
