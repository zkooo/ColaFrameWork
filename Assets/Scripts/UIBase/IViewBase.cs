using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI的接口（废弃）
/// </summary>
public interface IViewBase
{

}


/// <summary>
/// UI的类型枚举
/// </summary>
public enum UIType : byte
{
    Top = 0,
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Common = 4,
}

/// <summary>
/// UI的创建形式
/// </summary>
public enum UICreateType : byte
{
    /// <summary>
    /// 根据一个资源ID创建
    /// </summary>
    Res = 0,
    /// <summary>
    /// 根据一个传入的现有gameobjec
    /// </summary>
    Go = 1,
}