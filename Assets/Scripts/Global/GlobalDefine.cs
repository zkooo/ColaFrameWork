using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检查功能开启结果枚举
/// </summary>
public enum CheckFuncResult : byte
{
    /// <summary>
    /// 功能未开启
    /// </summary>
    False = 0,
    /// <summary>
    /// 功能开启
    /// </summary>
    True = 1,
    /// <summary>
    /// 功能未开启，时间限制
    /// </summary>
    LevelLimit = 2,
    /// <summary>
    /// 功能未开启,等级限制
    /// </summary>
    TimeLimit = 3,
    /// <summary>
    /// 未知原因，备用
    /// </summary>
    None = 4,
}