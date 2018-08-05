using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

public class LoginModule : ModuleBase {
    public LoginModule() : base(SubSysType.Login)
    {
    }

    public override void EnterSys()
    {
        base.EnterSys();
        GameEventMgr.GetInstance().DispatchEvent("OpenUIWithReturn", EventType.UIMsg, "UILogin");
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
