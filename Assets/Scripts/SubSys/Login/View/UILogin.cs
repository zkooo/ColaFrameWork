using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using EventType = ColaFrame.EventType;

public class UILogin : UIBase
{
    public UILogin(int resId, UILevel uiLevel) : base(resId, uiLevel)
    {
    }

    public UILogin(GameObject panel, GameObject parent, UILevel uiLevel) : base(panel, parent, uiLevel)
    {
    }

    public override void Close()
    {
        base.Close();
    }

    public override void Create()
    {
        base.Create();
        ShowUIBlur();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void OnCreate()
    {
        base.OnCreate();
        GameObject okBtn = Panel.FindChildByPath("bottom/okBtn");
        Image titleImage = Panel.GetComponentByPath<Image>("logo");
        CommonHelper.AddBtnMsg(okBtn, (obj) =>
        {
            if (obj.name == "okBtn")
            {
                CommonHelper.SetImageSpriteFromAtlas(2001, titleImage, "airfightSheet_3", false);
            }
        });
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
    }

    public override void UpdateUI(EventData eventData)
    {
        base.UpdateUI(eventData);
    }

    protected override void onClick(GameObject obj)
    {
        if ("cancelBtn" == obj.name)
        {
            GameEventMgr.GetInstance().DispatchEvent("CloseUI", EventType.UIMsg, "UILogin");
        }
    }

    protected override void onEditEnd(GameObject obj, string text)
    {
        if (obj.name == "usernameInput")
        {
            Debug.LogWarning("输入用户名："+text);
        }
        else if (obj.name == "passwordInput")
        {
            Debug.LogWarning("输入密码：" + text);
        }
    }
}
