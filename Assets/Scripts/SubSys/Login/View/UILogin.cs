using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.LogWarning(this.Name);
        GameObject root = GameObject.Find("Canvas");
       // Text debugText = CommonHelper.GetComponentByName<Text>(root, "Text");
        Text debugText = CommonHelper.GetComponentByPath<Text>(root,"Text");
        debugText.text = this.Name;
    }

    public override void UpdateUI(EventData eventData)
    {
        base.UpdateUI(eventData);
    }
}
