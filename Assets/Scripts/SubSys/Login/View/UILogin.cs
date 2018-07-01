using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using EventType = ColaFrame.EventType;

public class UILogin : UIBase
{
    private UIHintTest uiHintTest;
    public UILogin(int resId, UILevel uiLevel) : base(resId, uiLevel)
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
        uiHintTest = null;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        uiHintTest = new UIHintTest();
        AttachSubPanel("center/uiTextHint", uiHintTest, UILevel.Level2);
        GameObject okBtn = Panel.FindChildByPath("bottom/okBtn");
        Image titleImage = Panel.GetComponentByPath<Image>("logo");
        CommonHelper.AddBtnMsg(okBtn, (obj) =>
        {
            if (obj.name == "okBtn")
            {
                CommonHelper.SetImageSpriteFromAtlas(2001, titleImage, "airfightSheet_3", false);
            }
        });

        //TODO:测试视频下载
//        var path = Path.Combine(CommonHelper.GetAssetPath(), "Videos.mp4");
//        //var testUrl = @"http://yun.it7090.com/video/XHLaunchAd/video01.mp4";
//        var testUrl = @"http://yun.it7090.com/video/XHLaunchAd/video03.mp4" + "?" + DateTime.Now;
//        Debug.LogWarning("-------------->视频网络资源地址" + testUrl);
//        DownloadMovHelper.Begin(path, testUrl, () =>
//          {
//              Debug.LogWarning("开始下载。");
//          }
//        ,
//        () =>
//        {
//            Debug.LogWarning("下载完成。");
//        },
//        (reason) =>
//        {
//            Debug.LogWarning("下载失败原因" + reason);
//        },
//        (progress) =>
//        {
//            Debug.LogWarning("下载进度" + progress);
//        });

        //todo:测试UTF8字符串功能函数
        var str1 = "abc";
        var str2 = "中国人";
        var str3 = "a中b国c人";
        var str4 = "中ab国人c";
        Debug.LogWarning("UTF8字符串功能测试1:"+CommonHelper.GetUTF8StringCount(str1));
        Debug.LogWarning("UTF8字符串功能测试2:"+CommonHelper.GetUTF8StringCount(str2));
        Debug.LogWarning("UTF8字符串功能测试3:"+CommonHelper.GetUTF8StringCount(str3));
        Debug.LogWarning("UTF8字符串功能测试4:"+CommonHelper.GetUTF8StringCount(str4));
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        uiHintTest = null;
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
            //GameEventMgr.GetInstance().DispatchEvent("CloseUI", EventType.UIMsg, "UILogin");
        }

        if (obj.name == "okBtn")
        {
            Debug.LogWarning("点击了OK按钮！");
            uiHintTest.Open();
            DownloadMovHelper.Stop();
        }
        if (obj.name == "bg")
        {
            Debug.LogWarning("点击了bg按钮");
        }
    }

    protected override void onEditEnd(GameObject obj, string text)
    {
        if (obj.name == "usernameInput")
        {
            Debug.LogWarning("输入用户名：" + text);
        }
        else if (obj.name == "passwordInput")
        {
            Debug.LogWarning("输入密码：" + text);
        }
    }
}
