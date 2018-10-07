using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;

#if UNITY_EDITOR
/// <summary>
/// 用于创建代码模版的编辑器类(UI\Model等类)
/// </summary>
public static class CreateScriptsEditor
{
    #region 共有变量
    private static readonly string CSharpTemplateUIViewPath = "Assets/Editor/CreateScriptEditor/Templates/CSharp/UIViewTemplate.txt";
    private static readonly string CSharpTemplateModulePath = "Assets/Editor/CreateScriptEditor/Templates/CSharp/ModuleTemplate.txt";
    #endregion

    #region 共用方法
    /// <summary>
    /// 获取在编辑器中选择的路径
    /// </summary>
    /// <returns></returns>
    static string GetSelectedPath()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
    #endregion

    #region 创建C#模版
    [MenuItem("Assets/Create/C#/UIView", false, 90)]
    public static void CreateCSharpUIView()
    {
        string basePath = GetSelectedPath();
        string templateFullPath = CSharpTemplateUIViewPath;

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateCSharpScriptEndAction>(),
            basePath + "/NewUIView.cs",
            null,
            templateFullPath);
    }

    [MenuItem("Assets/Create/C#/Module", false, 91)]
    public static void CreateCSharpModule()
    {
        string basePath = GetSelectedPath();
        string templateFullPath = CSharpTemplateModulePath;

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateCSharpScriptEndAction>(),
            basePath + "/NewModule.cs",
            null,
            templateFullPath);
    }

    [MenuItem("Assets/Create/C#/Templates(UIView和Module)", false, 92)]
    public static void CreateTemplates()
    {
        string basePath = GetSelectedPath();
        //获取最后一级文件夹名，即选中的文件夹的名称
        string dirName = basePath.Substring(basePath.LastIndexOf(@"/") + 1);
        //创建对应的View和Module子路径
        string uiviewPath = Path.Combine(GetSelectedPath(), "View/");
        string modulePath = Path.Combine(GetSelectedPath(), "Module/");
        string dataPath = Path.Combine(GetSelectedPath(), "Data/");
        CommonHelper.CheckLocalFileExist(uiviewPath);
        CommonHelper.CheckLocalFileExist(modulePath);
        CommonHelper.CheckLocalFileExist(dataPath);

        //拷贝模板文件并创建新的文件
        string uiviewFileName = uiviewPath + dirName + "_UIView.cs";
        string moduleFileName = modulePath + dirName + "_Module.cs";
        CreateCSharpScriptEndAction.CreateScriptAssetFromTemplate(uiviewFileName, CSharpTemplateUIViewPath);
        CreateCSharpScriptEndAction.CreateScriptAssetFromTemplate(moduleFileName, CSharpTemplateModulePath);

        //刷新资源
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 快速创建UI模版
    /// </summary>
    [MenuItem("GameObject/UI/ColaUI/UIView", false, 1)]
    public static void CreateColaUIView()
    {
        GameObject uguiRoot = GetOrCreateUGUIRoot();

        //创建新的UI Prefab
        GameObject view = new GameObject("NewUIView", typeof(RectTransform));
        view.layer = LayerMask.NameToLayer("UI");
        view.tag = "UIView";
        string uniqueName = GameObjectUtility.GetUniqueNameForSibling(uguiRoot.transform, view.name);
        view.name = uniqueName;
        Undo.RegisterCreatedObjectUndo(view, "Create" + view.name);
        Undo.SetTransformParent(view.transform, uguiRoot.transform, "Parent" + view.name);
        GameObjectUtility.SetParentAndAlign(view, uguiRoot);

        //设置RectTransform属性
        RectTransform rect = view.GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);

        //设置新建的UIView被选中
        Selection.activeGameObject = view;
    }

    /// <summary>
    /// 获取或者创建UGUIRoot（编辑器状态下）
    /// </summary>
    /// <returns></returns>
    private static GameObject GetOrCreateUGUIRoot()
    {
        GameObject selectObj = Selection.activeGameObject;
        //先查找选中的物体的父节点是否是uUGIRoot
        Canvas canvas = (null != selectObj) ? selectObj.GetComponentInParent<Canvas>() : null;
        if (null != canvas && canvas.gameObject.activeInHierarchy)
        {
            return canvas.gameObject;
        }
        //再查找整个面板中是否存在UGIRoot
        canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        if (null != canvas && canvas.gameObject.activeInHierarchy)
        {
            return canvas.gameObject;
        }

        //如果以上步骤都没有找到，那就从Resource里面加载并实例化一个
        var uguiRootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Arts/Gui/Prefabs/UGUIRoot.prefab");
        GameObject uguiRoot = CommonHelper.InstantiateGoByPrefab(uguiRootPrefab, null);
        GameObject canvasRoot = uguiRoot.GetComponentInChildren<Canvas>().gameObject;
        return canvasRoot;
    }

    #endregion

    #region 创建Lua模版
    #endregion
}

#endif
