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
    /// UI管理器，打开某种类型的UI,并返回界面是否打开成功
    /// </summary>
    /// <typeparam name="T"></typeparam>
    bool Open(string uiType);

    /// <summary>
    /// UI管理器，关闭某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Close(string uiType);

    /// <summary>
    /// UI管理器，销毁某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Destroy(string uiType);

    /// <summary>
    /// UI管理器，刷新某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventData"></param>
    void UpdateUI(string uiType,EventData eventData);

    /// <summary>
    /// UI管理器，获取某种类型的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    UIBase GetViewByType(string uiType);

    /// <summary>
    /// UI管理器，将一个UI加入到UI管理器中参与管理
    /// </summary>
    /// <param name="ui"></param>
    void AddView(string uiType,UIBase ui);

    /// <summary>
    /// UI管理器，从UI管理器中移除某个UI，使其不再参与管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void RemoveViewByType(string uiType) ;

    /// <summary>
    /// UI管理器，打开某种类型的UI,并返回界面是否打开成功,带有功能开启检查
    /// </summary>
    /// <param name="uiType"></param>
    /// <returns></returns>
    bool OpenUIWithReturn(string uiType);
}

/// <summary>
/// UI界面等级枚举
/// </summary>
public enum UILevel : byte
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