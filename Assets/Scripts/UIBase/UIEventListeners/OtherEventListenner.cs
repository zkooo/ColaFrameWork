using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

//用来处理一些控件独有 不能统一处理的事件
public class OtherEventListenner : MonoBehaviour
{
    public delegate void StrValueChangeAction(GameObject obj, string text);
    public delegate void FloatValueChangeAction(GameObject obj, float value);
    public delegate void IntValueChangeAction(GameObject obj, int value);
    public delegate void BoolValueChangeAction(GameObject obj, bool isSelect);
    public delegate void RectValueChangeAction(GameObject obj, Vector2 rect);
    //InputField
    public StrValueChangeAction inputvalueChangeAction;
    public StrValueChangeAction inputeditEndAction;

    //Toggle
    public BoolValueChangeAction togglevalueChangeAction;
    //ScrollBar
    public FloatValueChangeAction scrollbarvalueChangeAction;
    //slider
    public FloatValueChangeAction slidervalueChangeAction;
    //dropdown
    public IntValueChangeAction dropdownvalueChangeAction;
    //scrollrect
    public RectValueChangeAction scrollrectvalueChangeAction;

    /// <summary>
    /// 新增触发事件回调，参数为触发的UI事件名称，比如onClick,onBoolValueChange,onSubmit等等
    /// </summary>
    public Action<string> onEvent;

    public void Awake()
    {
        //inputEditEndAction += delegate { };
        InputField input = gameObject.GetComponent<InputField>();
        if (input != null)
        {
            input.onValueChanged.AddListener(inputValueChangeHandler);
            input.onEndEdit.AddListener(inputEditEndHanler);
        }

        Toggle toggle = gameObject.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(toggleValueChangeHandler);
        }

        Scrollbar scrollbar = gameObject.GetComponent<Scrollbar>();
        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(scrollbarValueChangeHandler);
        }

        Slider slider = gameObject.GetComponent<Slider>();
        if (slider != null)
        {
            slider.onValueChanged.AddListener(sliderValueChangeHandler);
        }

        Dropdown dropdown = gameObject.GetComponent<Dropdown>();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(dropdownValueChangeHandler);
        }

        ScrollRect scrollrect = gameObject.GetComponent<ScrollRect>();
        if (scrollrect != null)
        {
            scrollrect.onValueChanged.AddListener(scrollrectValueChangeHandler);
        }
    }

    private void inputValueChangeHandler(string text)
    {
        if (inputvalueChangeAction != null)
        {
            inputvalueChangeAction(gameObject, text);
        }

        if (null != onEvent)
        {
            this.onEvent("onStrValueChange");
        }
    }

    private void inputEditEndHanler(string text)
    {
        if (inputeditEndAction != null)
        {
            inputeditEndAction(gameObject, text);
        }

        if (null != onEvent)
        {
            this.onEvent("onEditEnd");
        }
    }

    private void toggleValueChangeHandler(bool select)
    {
        if (togglevalueChangeAction != null)
        {
            togglevalueChangeAction(gameObject, select);
        }

        if (null != onEvent)
        {
            this.onEvent("onBoolValueChange");
        }
    }

    private void sliderValueChangeHandler(float value)
    {
        if (slidervalueChangeAction != null)
        {
            slidervalueChangeAction(gameObject, value);
        }

        if (null != onEvent)
        {
            this.onEvent("onFloatValueChange");
        }
    }

    private void scrollbarValueChangeHandler(float value)
    {
        if (scrollbarvalueChangeAction != null)
        {
            scrollbarvalueChangeAction(gameObject, value);
        }

        if (null != onEvent)
        {
            this.onEvent("onFloatValueChange");
        }
    }

    private void dropdownValueChangeHandler(int value)
    {
        if (dropdownvalueChangeAction != null)
        {
            dropdownvalueChangeAction(gameObject, value);
        }

        if (null != onEvent)
        {
            this.onEvent("onIntValueChange");
        }
    }

    private void scrollrectValueChangeHandler(Vector2 rect)
    {
        if (scrollrectvalueChangeAction != null)
        {
            scrollrectvalueChangeAction(gameObject, rect);
        }

        if (null != onEvent)
        {
            this.onEvent("onRectValueChange");
        }
    }

    public UnityAction<Vector2> scrollrectValueChangeHandler()
    {
        return delegate (Vector2 rect)
        {
            if (scrollrectvalueChangeAction != null)
            {
                scrollrectvalueChangeAction(gameObject, rect);
            }

            if (null != onEvent)
            {
                this.onEvent("onRectValueChange");
            }
        };
    }
}

