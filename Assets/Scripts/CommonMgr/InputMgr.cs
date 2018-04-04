using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColaFramework框架输入输出管理器
/// </summary>
public class InputMgr : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            ConfirmQuit();
        }
    }

    /// <summary>
    /// 退出游戏前确认对话框
    /// </summary>
    public void ConfirmQuit()
    {

#if UNITY_ANDROID
        //var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //jo.Call("ShowConfirmDialog");
        var jc = new AndroidJavaClass("com.msxher.ColaFrameWork.ColaMainActivity");
        var jo = jc.GetStatic<AndroidJavaObject>("GetInstance");
        jo.Call("ShowConfirmDialog","");
#endif
    }


}

