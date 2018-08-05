using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

/// <summary>
/// 系统管理控制类
/// </summary>
public class SubSysMgr : IEventHandler
{
    /// <summary>
    /// 存储系统的字典
    /// </summary>
    private Dictionary<int, ModuleBase> subSysList;

    /// <summary>
    /// 当前打开的系统
    /// </summary>
    private ModuleBase _curModule;

    public SubSysMgr()
    {
        subSysList = new Dictionary<int, ModuleBase>();
        /*------------------注册子系统---------------------*/
        ModuleBase module = new LoginModule();
        subSysList.Add((int)module.subSysType,module);
        module.EnterSys();
        GameEventMgr.GetInstance().RegisterHandler(this,EventType.ChangeSys,EventType.ServerMsg,EventType.SystemMsg);
    }

    /// <summary>
    /// 获取对应类型的系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSubSys<T>() where T : ModuleBase
    {
        foreach (var subSys in subSysList)
        {
            if (typeof(T) == subSys.Value.GetType())
            {
                return (T) subSys.Value;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据系统类型，返回对应的系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="subSysType"></param>
    /// <returns></returns>
    public T GetSubSys<T>(SubSysType subSysType) where T : ModuleBase
    {
        return (T)subSysList[(int)subSysType];
    }

    public bool HandleMessage(GameEvent evt)
    {
        throw new System.NotImplementedException();
    }

    public bool IsHasHandler(GameEvent evt)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 更新系统
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        using (var enumerator = subSysList.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.UpdateSys(deltaTime);
            }
        }
    }
}
