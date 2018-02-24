using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 预制体组件丢失清理助手类
/// </summary>
public class MissComponentsCleaner
{
    [MenuItem("ColaFramework/Cleaner/清除丢失组件")]
    public static void ClearMissComonents()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep);
        for (int i = 0; i < transforms.Length; i++)
        {
            EditorUtility.DisplayProgressBar("清理组件中...", "清理重复组件:" + transforms[i].name, i / (float)transforms.Length);
            ClearMissComponents(transforms[i].gameObject);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void ClearMissComponents(GameObject obj)
    {
        if (null == obj)
        {
            return;
        }
        var components = obj.GetComponents<Component>();
        SerializedObject serializedObject = new SerializedObject(obj);
    }

}
