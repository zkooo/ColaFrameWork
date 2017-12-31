using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI的接口
/// </summary>
public interface IViewBase
{
    /// <summary>
    /// 打开一个UI界面
    /// </summary>
    void Open();

    /// <summary>
    /// 关闭一个UI界面
    /// </summary>
    void Close();

    /// <summary>
    /// UI界面显隐的时候会调用该方法
    /// </summary>
    /// <param name="isShow"></param>UI的是否显示
    void OnShow(bool isShow);

    /// <summary>
    /// 创建UI界面
    /// </summary>
    void Create();

    /// <summary>
    /// UI创建过程中会调用该方法
    /// </summary>
    void OnCreate();

    /// <summary>
    /// 销毁一个UI界面
    /// </summary>
    void Destroy();

    /// <summary>
    /// 销毁UI界面的过程中调用该方法
    /// </summary>
    void OnDestroy();

    /// <summary>
    /// 设置一个UI界面的显隐
    /// </summary>
    /// <param name="isActive"></param>UI界面是否显示
    void SetActive(bool isActive);
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