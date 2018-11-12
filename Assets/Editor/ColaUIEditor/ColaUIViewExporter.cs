using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEditor;

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

    /// <summary>
    /// 导出UI接口
    /// </summary>
    /// <param name="type"></param>
    public static void ExportUIView(ExportUIViewType type = ExportUIViewType.CSharp)
    {
        GameObject uguiRoot = CreateColaUIEditor.GetOrCreateUGUIRoot();

        AssetDatabase.StartAssetEditing();
        //处理uguiRoot下面的各UIView

        for (int i = 0; i < uguiRoot.ChildCount(); i++)
        {
            Transform child = uguiRoot.GetChild(i).transform;
            //过滤
            if (!child.gameObject.activeSelf || !child.CompareTag(GloablDefine.UIViewTag))
            {
                continue;
            }
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    private static void AnalyzeUIView()
    {

    }

}

/// <summary>
/// 导出脚本的类型
/// </summary>
public enum ExportUIViewType : byte
{
    CSharp = 1,
    Lua = 2,
}
