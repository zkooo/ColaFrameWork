using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

/// <summary>
/// ColaFramework框架UI预制件导出工具
/// </summary>
public class ColaUIViewExporter 
{
    //强制导出的基本类型
    private static List<Type> necessaryTypes = new List<Type>()
    {
        typeof(IControl),typeof(Button),typeof(DropdownExtension),typeof(Dropdown),typeof(InputField),typeof(Scrollbar),typeof(ScrollRect),
        typeof(Slider),typeof(Toggle),typeof(IrregulaButton)
    };

    //自定义导出类型，根据一定规则判断是否需要导出
    private static List<Type> customTypes = new List<Type>()
    {
        typeof(IComponent),typeof(Image),typeof(RawImage),typeof(Text),typeof(LayoutGroup),typeof(RectTransform),typeof(Transform),typeof(UGUISpriteAnimation),
        typeof(UGUITweenAlpha),typeof(UGUITweenPosition),typeof(UGUITweenRotation),typeof(UGUITweenScale)
    };
}
