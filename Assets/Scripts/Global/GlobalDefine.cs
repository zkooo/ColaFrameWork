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

/// <summary>
/// 公共定义数据
/// </summary>
public static class GloablDefine
{
    public static Color ColorWhite = Color.white;
    public static Color ColorRed = Color.red;
    public static Color ColorGreen = Color.green;
    public static Color ColorBlue = Color.blue;
    public static Color ColorBlack = Color.black;
    public static Color ColorYellow = Color.yellow;
    public static Color ColorGray = Color.gray;

    public static EPateColor PateBlue = new EPateColor(new Color(39 / 255f, 193 / 255f, 255 / 255f, 1), new Color(10 / 255f, 16 / 255f, 39 / 255f, 1));
    public static EPateColor PateRed = new EPateColor(new Color(230 / 255f, 46 / 255f, 46 / 255f, 1), new Color(20 / 255f, 2 / 255f, 2 / 255f, 1));
    public static EPateColor PateGreen = new EPateColor(new Color(0 / 255f, 255 / 255f, 0 / 255f, 1), new Color(20 / 255f, 2 / 255f, 2 / 255f, 1));

    /// <summary>
    /// Bundle的资源后缀名
    /// </summary>
    public static readonly string extenName = ".u3d";

    /// <summary>
    /// StreamingAsset下的预装资源目录
    /// </summary>
    public static List<string> resbasePathList = new List<string>()
    {
        "config",
        "res_base"
    };

    public static readonly string UIViewTag = "UIView";
    public static readonly string UIIgneroTag = "UIIgnero";
    public static readonly string UIPropertyTag = "UIProperty";

    public static readonly string UIExportPrefabPath = "Assets/Resources/Arts/Gui/Prefabs";
    public static readonly string UIExportCSScriptPath = "Assets/Scripts/_UIViews";
}