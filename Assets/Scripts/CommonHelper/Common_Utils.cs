using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GameObject prefab = LuaResourceMgr.GetInstance().GetResourceByPath(path, typeof(GameObject), 0) as GameObject;
        return CommonHelper.InstantiateGoByPrefab(prefab, parent);
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
    public static Component AddSingleComponent(this GameObject go, Type type)
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

    /// <summary>
    /// 获取某个物体下对应名字的子物体(如果有重名的，就返回第一个符合的)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static GameObject GetGameObjectByName(this GameObject go, string childName)
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
    /// 根据路径查找物体(可以是自己本身/子物体/父节点)
    /// </summary>
    /// <param name="obj"></param>父物体节点
    /// <param name="childPath"></param>子物体路径+子物体名称
    /// <returns></returns>
    public static GameObject FindChildByPath(this GameObject obj, string childPath)
    {
        if (null == obj)
        {
            Debug.LogWarning("FindChildByPath方法传入的根节点为空！");
            return null;
        }
        if ("." == childPath) return obj;
        if (".." == childPath)
        {
            Transform parentTransform = obj.transform.parent;
            return parentTransform == null ? null : parentTransform.gameObject;
        }
        Transform transform = obj.transform.Find(childPath);
        return null != transform ? transform.gameObject : null;
    }


    /// <summary>
    /// 根据路径查找物体上的某个类型的组件(可以是自己本身/子物体/父节点)
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="childPath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Component GetComponentByPath(this GameObject obj, string childPath, Type type)
    {
        GameObject childObj = FindChildByPath(obj, childPath);
        if (null == childObj)
        {
            return null;
        }
        Component component = childObj.GetComponent(type);
        if (null == component)
        {
            Debug.LogWarning(String.Format("没有在路径:{0}上找到组件:{1}!", childPath, type));
            return null;
        }
        return component;
    }

    /// <summary>
    /// 获取当前运行的设备平台信息
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceInfo()
    {
        return CommonHelper.GetDeviceInfo();
    }

    /// <summary>
    /// 返回UI画布的根节点
    /// </summary>
    /// <returns></returns>
    public static GameObject GetUIRootObj()
    {
        return GUIHelper.GetUIRootObj();
    }

    /// <summary>
    /// 返回UI相机节点
    /// </summary>
    /// <returns></returns>
    public static GameObject GetUICameraObj()
    {
        return GUIHelper.GetUICameraObj();
    }

    /// <summary>
    /// 返回UI画布
    /// </summary>
    /// <returns></returns>
    public static Canvas GetUIRoot()
    {
        return GUIHelper.GetUIRoot();
    }

    /// <summary>
    /// 返回UI相机
    /// </summary>
    /// <returns></returns>
    public static Camera GetUICamera()
    {
        return GUIHelper.GetUICamera();
    }

    /// <summary>
    /// 获取主相机
    /// </summary>
    /// <returns></returns>
    public static Camera GetMainCamera()
    {
        return GUIHelper.GetMainCamera();
    }

    /// <summary>
    /// 获取主相机节点
    /// </summary>
    /// <returns></returns>
    public static GameObject GetMainGameObj()
    {
        return GUIHelper.GetMainGameObj();
    }

    /// <summary>
    /// 获取模型描边相机节点
    /// </summary>
    /// <returns></returns>
    public static GameObject GetModelOutlineCameraObj()
    {
        return GUIHelper.GetModelOutlineCameraObj();
    }

    /// <summary>
    /// 获取设备的电量
    /// </summary>
    /// <returns></returns>
    public static float GetBatteryLevel()
    {
        return CommonHelper.GetBatteryLevel();
    }

    /// <summary>
    /// 获取设备的电池状态
    /// </summary>
    /// <returns></returns>
    public static int GetBatteryStatus()
    {
        return (int)CommonHelper.GetBatteryStatus();
    }

    /// <summary>
    /// 获取设备网络的状况
    /// </summary>
    /// <returns></returns>
    public static int GetNetworkStatus()
    {
        return (int)CommonHelper.GetNetworkStatus();
    }

    /// <summary>
    /// 将世界坐标转化UGUI坐标
    /// </summary>
    /// <param name="gameCamera"></param>
    /// <param name="canvas"></param>
    /// <param name="worldPos"></param>
    public static Vector2 WorldToUIPoint(Camera gameCamera, Canvas canvas, Vector3 worldPos)
    {
        return CommonHelper.WorldToUIPoint(gameCamera, canvas, worldPos);
    }

    /// <summary>
    /// 获取一个Transform组件下所有处于Active状态的子物体的数量
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static int ActivedChildCount(this Transform transform)
    {
        int childCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                childCount++;
        }
        return childCount;
    }

    /// <summary>
    /// 获取资源路径(可读写)
    /// </summary>
    /// <returns></returns>
    public static string GetAssetPath()
    {
        return CommonHelper.GetAssetPath();
    }

    /// <summary>
    /// 获取主机IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetHostIp()
    {
        return CommonHelper.GetHostIp();
    }

    /// <summary>
    /// 获取一个GameObject下所有子物体的数量
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int ChildCount(this GameObject obj)
    {
        if (null != obj)
        {
            return obj.transform.childCount;
        }

        return 0;
    }

    /// <summary>
    /// 根据索引获取一个GameOject的子物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static GameObject GetChild(this GameObject obj, int index)
    {
        if (null != obj)
        {
            return obj.transform.GetChild(index).gameObject;
        }

        return null;
    }

    /// <summary>
    /// 获取导航点距离地面的高度
    /// </summary>
    /// <param name="vPos"></param>
    /// <returns></returns>
    public static float GetNavMeshHeight(Vector3 vPos)
    {
        return CommonHelper.GetNavMeshHeight(vPos);
    }

    /// <summary>
    /// 检查本地文件是否存在,如果目录不存在则创建目录
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool CheckLocalFileExist(string filePath)
    {
        return CommonHelper.CheckLocalFileExist(filePath);
    }

    public static Component[] GetComponentsInChildren(this GameObject obj, string type, bool includeInactive = false)
    {
        List<Component> components = new List<Component>();
        Component component = null;
        Transform[] objChildren = obj.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < objChildren.Length; ++i)
        {
            component = objChildren[i].GetComponent(type);
            components.Add(component);
        }
        return components.ToArray();
    }

    public static void SetLogHelperText(Text text)
    {
        GameLauncher.Instance.LogHelper.SetTextComponent(text);
    }

    public static void ClearLogHelperText()
    {
        GameLauncher.Instance.LogHelper.ClearText();
    }
}
