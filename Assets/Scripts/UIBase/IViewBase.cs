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
/// UI管理器接口
/// </summary>
public interface IViewManager
{
    /// <summary>
    /// UI管理器，打开某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Open<T>();

    /// <summary>
    /// UI管理器，关闭某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Close<T>();

    /// <summary>
    /// UI管理器，销毁某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Destroy<T>();

    /// <summary>
    /// UI管理器，刷新某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventData"></param>
    void UpdateUI<T>(EventData eventData);

    /// <summary>
    /// UI管理器，获取某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetViewByType<T>() where T:UIBase;

    /// <summary>
    /// UI管理器，将一个UI加入到UI管理器中参与管理
    /// </summary>
    /// <param name="ui"></param>
    void AddView(UIBase ui);

    /// <summary>
    /// UI管理器，从UI管理器中移除某个UI，使其不再参与管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void RemoveViewByType<T>() where T : UIBase;
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