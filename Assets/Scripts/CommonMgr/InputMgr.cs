using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColaFramework框架输入输出管理器
/// </summary>
public class InputMgr : MonoBehaviour
{
    private bool isQuit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKey(KeyCode.Escape))
	    {
	        isQuit = true;
	    }
	}

    private void OnGUI()
    {
        if (isQuit)
        {
            GUI.Box(new Rect(0,0,200,100),"确定要退出游戏？");

            if (GUI.Button(new Rect(20, 50, 70, 30), "确定"))
            {
                Application.Quit();
            }

            if (GUI.Button(new Rect(110, 50, 70, 30), "取消"))
            {
                isQuit = false;
            }
        }
    }


}

