using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Def = GloablDefine;

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
    public static T AddSingleComponent<T>(this GameObject go) where T : Component
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
    public static T GetComponentByName<T>(this GameObject go, string name) where T : Component
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
    public static T[] GetComponentsByName<T>(this GameObject go) where T : Component
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
    /// 获取某个物体下对应的名字的所有子物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static List<GameObject> GetGameObjectsByName(this GameObject go, string childName)
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
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="childPath"></param>
    /// <returns></returns>
    public static T GetComponentByPath<T>(this GameObject obj, string childPath) where T : Component
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
    [Obsolete("已弃用,建议使用FindChildByPath")]
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

    /// <summary>
    /// 设置一个Image组件的Sprite为某个图集中的图片
    /// </summary>
    /// <param name="atlasID"></param>图集的资源ID
    /// <param name="image"></param>要设置的Image组件
    /// <param name="spriteName"></param>图集中对应的Sprite的名称
    /// <param name="keepNativeSize"></param>是否保持原始尺寸
    public static void SetImageSpriteFromAtlas(int atlasID, Image image, string spriteName, bool keepNativeSize)
    {
        if (null == image)
        {
            Debug.LogWarning("需要指定一个image");
            return;
        }
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogWarning("需要指定一个SpriteName");
            return;
        }

        if (image.overrideSprite && image.overrideSprite.name == spriteName)
        {
            return;
        }
        GameObject atlasObj = ResourceMgr.GetInstance().GetResourceById<GameObject>(atlasID);
        if (null != atlasObj)
        {
            SpriteAsset spriteAsset = atlasObj.GetComponent<SpriteAsset>();
            if (null != spriteAsset)
            {
                Sprite sprite = spriteAsset.GetSpriteByName(spriteName);
                if (null != sprite)
                {
                    image.overrideSprite = sprite;
                    if (keepNativeSize)
                    {
                        image.SetNativeSize();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置一个RawImage的Texture
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="resID"></param>
    /// <param name="keepNativeSize"></param>
    public static void SetRawImage(RawImage rawImage, int resID, bool keepNativeSize)
    {
        if (null == rawImage)
        {
            Debug.LogWarning("需要指定RawImage");
            return;
        }

        Texture2D texture2D = ResourceMgr.GetInstance().GetResourceById<Texture2D>(resID);
        if (null != texture2D)
        {
            rawImage.texture = texture2D;
            if (keepNativeSize)
            {
                rawImage.SetNativeSize();
            }
        }
    }

    /// <summary>
    /// 设置一个Image是否变灰
    /// </summary>
    /// <param name="image"></param>
    /// <param name="isGray"></param>
    public static void SetImageGray(Image image, bool isGray)
    {
        if (null == image)
        {
            Debug.LogWarning("需要指定一个Image");
            return;
        }
        image.color = isGray ? Def.ColorGray : Def.ColorWhite;
    }

    /// <summary>
    /// RawImage置灰
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="isGray"></param>
    public static void SetRawImageGray(RawImage rawImage, bool isGray)
    {
        if (null == rawImage)
        {
            Debug.LogWarning("需要指定一个RawImage");
            return;
        }
        if (isGray)
        {
            Material grayMat = ResourceMgr.GetInstance().GetResourceById<Material>(300001);
            rawImage.material = grayMat;
            rawImage.color = Def.ColorBlack;
        }
        else
        {
            rawImage.material = null;
            rawImage.color = Def.ColorWhite;
        }
    }
    /// <summary>
    /// 获取当前运行的设备平台信息
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Device And Sysinfo:\r\n");
        stringBuilder.Append(string.Format("DeviceModel: {0} \r\n", SystemInfo.deviceModel));
        stringBuilder.Append(string.Format("DeviceName: {0} \r\n", SystemInfo.deviceName));
        stringBuilder.Append(string.Format("DeviceType: {0} \r\n", SystemInfo.deviceType));
        stringBuilder.Append(string.Format("BatteryLevel: {0} \r\n", SystemInfo.batteryLevel));
        stringBuilder.Append(string.Format("DeviceUniqueIdentifier: {0} \r\n", SystemInfo.deviceUniqueIdentifier));
        stringBuilder.Append(string.Format("SystemMemorySize: {0} \r\n", SystemInfo.systemMemorySize));
        stringBuilder.Append(string.Format("OperatingSystem: {0} \r\n", SystemInfo.operatingSystem));
        stringBuilder.Append(string.Format("GraphicsDeviceID: {0} \r\n", SystemInfo.graphicsDeviceID));
        stringBuilder.Append(string.Format("GraphicsDeviceName: {0} \r\n", SystemInfo.graphicsDeviceName));
        stringBuilder.Append(string.Format("GraphicsDeviceType: {0} \r\n", SystemInfo.graphicsDeviceType));
        stringBuilder.Append(string.Format("GraphicsDeviceVendor: {0} \r\n", SystemInfo.graphicsDeviceVendor));
        stringBuilder.Append(string.Format("GraphicsDeviceVendorID: {0} \r\n", SystemInfo.graphicsDeviceVendorID));
        stringBuilder.Append(string.Format("GraphicsDeviceVersion: {0} \r\n", SystemInfo.graphicsDeviceVersion));
        stringBuilder.Append(string.Format("GraphicsMemorySize: {0} \r\n", SystemInfo.graphicsMemorySize));
        stringBuilder.Append(string.Format("GraphicsMultiThreaded: {0} \r\n", SystemInfo.graphicsMultiThreaded));
        stringBuilder.Append(string.Format("SupportedRenderTargetCount: {0} \r\n", SystemInfo.supportedRenderTargetCount));
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 检测一个功能模块是否开启
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="isTips"></param>
    /// <returns></returns>
    public static CheckFuncResult CheckFuncOpen(string funcName, bool isTips)
    {
        //todo:做些检查工作
        CheckFuncResult result = CheckFuncResult.None;
        if (CheckFuncResult.True != result && isTips)
        {
            switch (result)
            {
                case CheckFuncResult.False:
                    break;
                case CheckFuncResult.LevelLimit:
                    break;
                case CheckFuncResult.TimeLimit:
                    break;
                case CheckFuncResult.None:
                    break;
                default:
                    break;
            }
        }
        return result;
    }

    /// <summary>
    /// 将世界坐标转化UGUI坐标
    /// </summary>
    /// <param name="gameCamera"></param>
    /// <param name="canvas"></param>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector2 WorldToUIPoint(Camera gameCamera, Canvas canvas, Vector3 worldPos)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            gameCamera.WorldToScreenPoint(worldPos), canvas.worldCamera, out pos);
        return pos;
    }

    /// <summary>
    /// 获取一个Transform组建下所有处于Active状态的子物体的数量
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
        return GameLauncher.Instance.AssetPath;
    }
}