using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : UIBase
{
    public UILogin(int resId, UIType uiType) : base(resId, uiType)
    {
    }

    public UILogin(GameObject panel, GameObject parent, UIType uiType) : base(panel, parent, uiType)
    {
    }

    public override void Close()
    {
        base.Close();
    }

    public override void Create()
    {
        base.Create();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnShow(bool isShow)
    {
        base.OnShow(isShow);
    }

    public override void Open()
    {
        base.Open();
        Debug.LogWarning("UI登录系统打开");
    }

    public override void UpdateUI(EventData eventData)
    {
        base.UpdateUI(eventData);
    }
}
