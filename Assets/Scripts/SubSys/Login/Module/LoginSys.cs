using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : SubSysBase {
    public LoginSys() : base(SubSysType.Login)
    {
    }

    public override void UpdateSys(double deltaTime)
    {
        base.UpdateSys(deltaTime);
        Debug.LogWarning("登录系统刷新"+deltaTime);
    }
}
