using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 头顶字缓存池和管理的相关接口
/// </summary>
public interface IEPateCache
{
    T CreateFromCache<T>(GameObject targetObj, GameObject prefab, float offsetH = 0f) where T : EPateBase;
}

/// <summary>
/// 头顶字的缓存池和管理器
/// </summary>
public class EPateCacheMgr : IEPateCache
{

}
