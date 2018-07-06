using UnityEngine;
using System.Collections;
using UnityEditor;

public class ColaQuickWindowEditor : EditorWindow
{
    [MenuItem("ColaFramework/Open Quick Window %Q")]
    static void Popup()
    {
        ColaQuickWindowEditor window = EditorWindow.GetWindow<ColaQuickWindowEditor>();
        window.titleContent = new GUIContent("快捷工具窗");
        window.position = new Rect(400, 100, 640, 480);
        window.Show();
    }

    public void OnGUI()
    {
        DrawColaFrameworkUI();
        GUILayout.Space(20);
        DrawAssetBundleUI();
    }


    public void DrawColaFrameworkUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== UI相关辅助 ==");
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
    }

    public void DrawAssetBundleUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("== Assetbundle相关 ==");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打包Assetbundle（增量）", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BundleBuildHelper.BuildAssetBundlesAuto();
        }
        if (GUILayout.Button("重新打包Assetbundle（先删除再重打）", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("为所有资源设置Assetbundle name", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BundleBuildHelper.SetBundleNameAuto();
        }
        if (GUILayout.Button("清除所有资源的Assetbundle name", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(30)))
        {
            BundleBuildHelper.ClearAssetBundlesName();
        }
        GUILayout.EndHorizontal();
    }
}
