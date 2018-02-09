using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检查功能开启结果枚举
/// </summary>
public enum CheckFuncResult : byte
{
    False = 0,
    True = 1,
    LevelLimit = 2,
    TimeLimit = 3,
    None = 4,
}