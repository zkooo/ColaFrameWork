using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

public class LoginSys : SubSysBase {
    public LoginSys() : base(SubSysType.Login)
    {
    }

    public override void EnterSys()
    {
        base.EnterSys();
        GameEventMgr.GetInstance().DispatchEvent("uiLoginPanelOpen",EventType.UIMsg,new List<string>(){"测试数据"});
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
