using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

public class LoginSys : SubSysBase {
    public LoginSys() : base(SubSysType.Login)
    {
        Debug.Log("执行Login系统构造函数");
    }

    public override void EnterSys()
    {
        base.EnterSys();
        GameEventMgr.GetInstance().DispatchEvent("uiLoginPanelOpen", EventType.UIMsg);
        Debug.LogWarning("进入了Login系统");
    }

    public override void ExitSys()
    {
        base.ExitSys();
    }

    public override void UpdateSys(double deltaTime)
    {
        base.UpdateSys(deltaTime);
    }

    protected override void RegisterHander()
    {
        base.RegisterHander();
    }
}
