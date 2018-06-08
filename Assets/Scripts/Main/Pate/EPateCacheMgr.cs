using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 头顶字缓存池和管理的相关接口
/// </summary>
public interface IEPateCache
{
    /// <summary>
    /// 通过管理器创建一个指定类型的头顶字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetObj"></param>
    /// <param name="prefab"></param>
    /// <param name="offsetH"></param>
    /// <returns></returns>
    T CreateFromCache<T>(GameObject targetObj, GameObject prefab, float offsetH = 0f) where T : EPateBase;

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

    }

    public T CreateFromCache<T>(GameObject targetObj, GameObject prefab, float offsetH = 0) where T : EPateBase
    {
        throw new System.NotImplementedException();
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
}
