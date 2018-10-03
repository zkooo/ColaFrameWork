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

    [MenuItem("Assets/Create/C#/Templates(UIView&Module)", false, 92)]
    public static void CreateTemplates()
    {
        string basePath = GetSelectedPath();
        //获取最后一级文件夹名，即选中的文件夹的名称
        string dirName = basePath.Substring(basePath.LastIndexOf(@"/") + 1);
        //创建对应的View和Module子路径
        string uiviewPath = Path.Combine(GetSelectedPath(), "View");
        //拷贝模板文件并创建新的文件
        CreateCSharpScriptEndAction.CreateScriptAssetFromTemplate()
    }

    #endregion

    #region 创建Lua模版
    #endregion
}

#endif
