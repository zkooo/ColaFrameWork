using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用工具类，为导出lua接口调用准备
/// </summary>
public static class Common_Utils
{
    /// <summary>
    /// 给按钮添加点击事件(以后可以往这里添加点击声音)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    public static void AddBtnMsg(GameObject go, Action<GameObject> callback)
    {
        CommonHelper.AddBtnMsg(go, callback);
    }

    /// <summary>
    /// 删除一个按钮的点击事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    public static void RemoveBtnMsg(GameObject go, Action<GameObject> callback)
    {
        CommonHelper.RemoveBtnMsg(go, callback);
    }

    /// <summary>
    /// 根据路径实例化一个Prefab
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject InstantiateGoByPath(string path, GameObject parent)
    {
        return null;
    }

    /// <summary>
    /// 根据一个预制实例化一个Prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject InstantiateGoByPrefab(GameObject prefab, GameObject parent)
    {
        return CommonHelper.InstantiateGoByPrefab(prefab, parent);
    }

    /// <summary>
    /// 给物体添加一个单一组件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Component AddSingleComponent(this GameObject go,Type type)
    {
        if (null != go)
        {
            Component component = go.GetComponent(type);
            if (null == component)
            {
                component = go.AddComponent(type);
            }
            return component;
        }
        Debug.LogWarning("要添加组件的物体为空！");
        return null;
    }
}
