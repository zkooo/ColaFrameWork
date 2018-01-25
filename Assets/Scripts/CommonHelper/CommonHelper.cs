using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用工具类
/// 提供一些常用功能的接口
/// </summary>
public static class CommonHelper
{

    /// <summary>
    /// 通过ID获取国际化文字
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetText(int id)
    {
        return I18NHelper.GetInstance().GetI18NText(id);
    }

    /// <summary>
    /// 给按钮添加点击事件(以后可以往这里添加点击声音)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    public static void AddBtnMsg(GameObject go, Action<GameObject> callback)
    {
        if (null != go)
        {
            Button button = go.GetComponent<Button>();
            if (null != button)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    callback(go);
                });
            }
            else
            {
                Debug.LogWarning("该按钮没有挂载button组件！");
            }
        }
        else
        {
            Debug.LogWarning("ButtonObj为空！");
        }
    }

    /// <summary>
    /// 删除一个按钮的点击事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    public static void RemoveBtnMsg(GameObject go, Action<GameObject> callback)
    {
        if (null != go)
        {
            Button button = go.GetComponent<Button>();
            if (null != button)
            {
                button.onClick.RemoveAllListeners();
            }
            else
            {
                Debug.LogWarning("该按钮没有挂载button组件！");
            }
        }
        else
        {
            Debug.LogWarning("ButtonObj为空！");
        }
    }

    /// <summary>
    /// 根据id实例化一个Prefab
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject InstantiateGoByID(int id, GameObject parent)
    {
        ResourceMgr resourceMgr = ResourceMgr.GetInstance();
        GameObject prefab = null;

        if (null != resourceMgr)
        {
            prefab = resourceMgr.GetResourceById<GameObject>(id);
            if (null == prefab) return null;
            GameObject go = GameObject.Instantiate(prefab);
            if (null == go) return null;
            go.name = prefab.name;
            if (null != parent)
            {
                go.transform.SetParent(parent.transform, false);
            }
            go.transform.localScale = prefab.transform.localScale;
            go.transform.localPosition = prefab.transform.localPosition;
            go.transform.localRotation = prefab.transform.localRotation;
            return go;
        }
        else
        {
            Debug.LogWarning("检查资源管理器！");
            return null;
        }
    }

    /// <summary>
    /// 根据一个预制实例化一个Prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject InstantiateGoByPrefab(GameObject prefab, GameObject parent)
    {
        if (null == prefab) return null;
        GameObject obj = GameObject.Instantiate(prefab);
        if (null == obj) return null;
        obj.name = prefab.name;
        if (null != parent)
        {
            obj.transform.SetParent(parent.transform, false);
        }
        obj.transform.localPosition = prefab.transform.localPosition;
        obj.transform.localRotation = prefab.transform.localRotation;
        obj.transform.localScale = prefab.transform.localScale;
        return obj;
    }

    /// <summary>
    /// 给物体添加一个单一组件
    /// </summary>
    /// <typeparam name="T"></typeparam>组件的类型
    /// <param name="go"></param>要添加组件的物体
    /// <returns></returns>
    public static T AddSingleComponent<T>(GameObject go) where T : Component
    {
        if (null != go)
        {
            T component = go.GetComponent<T>();
            if (null == component)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }
        Debug.LogWarning("要添加组件的物体为空！");
        return null;
    }

    /// <summary>
    /// 获取某个物体下对应名字的子物体上的某个类型的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>组件的类型
    /// <param name="go"></param>父物体
    /// <param name="name"></param>子物体的名称
    /// <returns></returns>
    public static T GetComponentByName<T>(GameObject go, string name) where T : Component
    {
        T[] components = go.GetComponentsInChildren<T>(true);
        if (components != null)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != null && components[i].name == name)
                {
                    return components[i];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 获取某个物体下子物体上某种类型的所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T[] GetComponentsByName<T>(GameObject go) where T : Component
    {
        T[] components = go.GetComponentsInChildren<T>(true);

        return components;
    }

    /// <summary>
    /// 获取某个物体下对应名字的子物体(如果有重名的，就返回第一个符合的)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static GameObject GetGameObjectByName(GameObject go, string childName)
    {
        GameObject ret = null;
        if (go != null)
        {
            Transform[] childrenObj = go.GetComponentsInChildren<Transform>(true);
            if (childrenObj != null)
            {
                for (int i = 0; i < childrenObj.Length; ++i)
                {
                    if ((childrenObj[i].name == childName))
                    {
                        ret = childrenObj[i].gameObject;
                        break;
                    }
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// 获取某个物体下对应的名字的所有子物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static List<GameObject> GetGameObjectsByName(GameObject go, string childName)
    {
        List<GameObject> list = new List<GameObject>();
        Transform[] objChildren = go.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < objChildren.Length; ++i)
        {
            if ((objChildren[i].name.Contains(childName)))
            {
                list.Add(objChildren[i].gameObject);
            }
        }
        return list;
    }

    /// <summary>
    /// 根据路径查找物体(可以是自己本身/子物体/父节点)
    /// </summary>
    /// <param name="obj"></param>父物体节点
    /// <param name="childPath"></param>子物体路径+子物体名称
    /// <returns></returns>
    public static GameObject FindChildByPath(GameObject obj, string childPath)
    {
        if (null == obj)
        {
            Debug.LogWarning(string.Format("FindChildByPath方法传入的根节点为空！"));
            return null;
        }
        if ("." == childPath) return obj;
        if (".." == childPath)
        {
            Transform parentTransform = obj.transform.parent;
            return parentTransform == null ? null : parentTransform.gameObject;
        }
        return obj.transform.Find(childPath).gameObject;
    }

    /// <summary>
    /// 根据路径查找物体上的某个类型的组件(可以是自己本身/子物体/父节点)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="childPath"></param>
    /// <returns></returns>
    public static T GetComponentByPath<T>(GameObject obj, string childPath) where T : Component
    {
        GameObject childObj = FindChildByPath(obj, childPath);
        if (null == childObj)
        {
            return null;
        }
        T component = childObj.GetComponent<T>();
        if (null == component)
        {
            Debug.LogWarning(String.Format("没有在路径:{0}上找到组件:{1}!", childPath, typeof(T)));
            return null;
        }
        return component;
    }

    /// <summary>
    /// 根据名称查找子物体
    /// </summary>
    /// <param name="obj"></param>父物体节点
    /// <param name="childName"></param>子物体名称
    /// <param name="isRecursice"></param>是否递归查找
    /// <returns></returns>
    public static GameObject FindChildDirect(GameObject obj, string childName, bool isRecursice)
    {
        if ("." == childName) return obj;
        if (".." == childName)
        {
            Transform parentTransform = obj.transform.parent;
            return parentTransform == null ? null : parentTransform.gameObject;
        }
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform trans = obj.transform.GetChild(i);
            GameObject childObj = trans.gameObject;
            if (childName == childObj.name)
            {
                return childObj;
            }
            if (isRecursice)
            {
                GameObject result = FindChildDirect(childObj, childName, isRecursice);
                if (null != result)
                {
                    return result;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 获取资源管理器
    /// </summary>
    /// <returns></returns>
    public static ResourceMgr GetResourceMgr()
    {
        return ResourceMgr.GetInstance();
    }

    /// <summary>
    /// 获取系统管理器
    /// </summary>
    /// <returns></returns>
    public static SubSysMgr GetSubSysMgr()
    {
        return GameManager.GetInstance().GetSubSysMgr();
    }

    /// <summary>
    /// 获取UI管理器
    /// </summary>
    /// <returns></returns>
    public static UIMgr GetUIMgr()
    {
        return GameManager.GetInstance().GetUIMgr();
    }
}