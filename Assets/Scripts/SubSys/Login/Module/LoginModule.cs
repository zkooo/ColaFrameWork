using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFrame.EventType;

public class LoginModule : ModuleBase {
    public LoginModule() : base(ModuleType.Login)
    {

    }

    public override void Init()
    {
        base.Init();
        GameEventMgr.GetInstance().DispatchEvent("OpenUIWithReturn", EventType.UIMsg, "UILogin");
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void RegisterHander()
    {
        base.RegisterHander();
    }
}
