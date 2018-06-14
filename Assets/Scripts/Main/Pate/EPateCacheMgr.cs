using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Def = GloablDefine;

/// <summary>
/// 头顶字缓存池和管理的相关接口
/// </summary>
public interface IEPateCache
{

    /// <summary>
    /// 通过管理器创建一个指定类型的头顶字预制
    /// </summary>
    /// <param name="targetObj"></param>
    /// <param name="prefab"></param>
    /// <param name="offsetH"></param>
    /// <returns></returns>
    GameObject CreateFromCache<T>(GameObject targetObj, GameObject prefab, float offsetH = 0f, bool isVisible = true) where T : EPateBase;

    /// <summary>
    /// 获取头顶字的根节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    GameObject GetCacheRoot(Type type);

    /// <summary>
    /// 释放所有的缓存Pate
    /// </summary>
    void ReleaseAll();

    /// <summary>
    /// 释放某一种类型的缓存Pate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void ReleaseByType<T>() where T : EPateBase;

    /// <summary>
    /// 销毁PateMgr
    /// </summary>
    void Destroy();

    void AttachTarget(GameObject targetObj, float offsetH);
}

/// <summary>
/// 头顶字的缓存池和管理器
/// </summary>
public class EPateCacheMgr : IEPateCache
{
    private Dictionary<Type, GameObject> cachesDic;
    private static EPateCacheMgr instance;

    public static EPateCacheMgr Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new EPateCacheMgr();
            }
            return instance;
        }
    }

    private EPateCacheMgr()
    {
        cachesDic = new Dictionary<Type, GameObject>();
        GameObject playerTopPateObj = new GameObject("PlayerTopPateCache");
        cachesDic.Add(typeof(EPlayerTopPate), playerTopPateObj);
    }

    public GameObject CreateFromCache<T>(GameObject targetObj, GameObject prefab, float offsetH = 0, bool isVisible = true) where T : EPateBase
    {
        var cacheRoot = GetCacheRoot(typeof(T));
        GameObject pate = null;
        UGUIHUDFollowTarget hudFollow = null;
        if (null != cacheRoot)
        {
            if (cacheRoot.ChildCount() > 0)
            {
                var hudObj = cacheRoot.GetChild(0); //每次取栈顶
                hudFollow = hudObj.GetComponent<UGUIHUDFollowTarget>();
                hudFollow.target = targetObj.transform;
                hudFollow.offset = new Vector3(0, offsetH, 0);
                var root = CommonHelper.GetUIMgr().GetHUDTopBoardRoot();
                hudObj.transform.SetParent(root.transform, false);
                pate = hudObj.FindChildByPath("pate");
            }
            else
            {
                pate = CommonHelper.InstantiateGoByPrefab(prefab, null);
                AttachTarget(targetObj, offsetH,pate);
                hudFollow = pate.GetComponent<UGUIHUDFollowTarget>();
            }
            pate.SetActive(true);
            hudFollow.SetVisible(isVisible);
        }

        return pate;
    }

    public GameObject GetCacheRoot(Type type)
    {
        if (null != cachesDic && cachesDic.ContainsKey(type))
        {
            return cachesDic[type];
        }

        return null;
    }

    public void ReleaseAll()
    {
        throw new NotImplementedException();
    }

    public void ReleaseByType<T>() where T : EPateBase
    {
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    public void AttachTarget(GameObject targetObj, float offsetH,GameObject pate)
    {
        var hud = CommonHelper.GetUIMgr().CreateHUD(targetObj.name);
        var follow = hud.AddSingleComponent<UGUIHUDFollowTarget>();
        var transform = targetObj.transform;
        follow.target = transform;
        follow.offset = new Vector3(0,offsetH,0);
        pate.transform.SetParent(hud.transform,false);
        pate.transform.localPosition = Vector3.zero;
        pate.transform.localScale = Vector3.one;
        hud.layer = LayerMask.NameToLayer("PateText");
    }
}
