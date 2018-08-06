using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏核心管理中心
/// </summary>
public class GameManager
{

    private static GameManager instance;

    /// <summary>
    /// Launcher的Obj
    /// </summary>
    private GameObject gameLauncherObj;

    /// <summary>
    /// 系统管理器
    /// </summary>
    private SubSysMgr subSysMgr;

    /// <summary>
    /// 场景/关卡管理器
    /// </summary>
    private LevelMgr levelMgr;

    /// <summary>
    /// 资源管理器
    /// </summary>
    private ResourceMgr resourceMgr;

    /// <summary>
    /// UI管理器
    /// </summary>
    private UIMgr uiMgr;

    private GameManager()
    {

    }

    public static GameManager GetInstance()
    {
        if (null == instance)
        {
            instance = new GameManager();
        }
        return instance;
    }

    /// <summary>
    /// 初始化游戏核心
    /// </summary>
    public void InitGameCore(GameObject gameObject)
    {
        //初始化各种管理器
        resourceMgr = ResourceMgr.GetInstance();
        gameLauncherObj = gameObject;
        LocalDataMgr.GetInstance().LoadStartConfig(() =>
        {
            resourceMgr.Init();
        });

        uiMgr = new UIMgr();
        subSysMgr = new SubSysMgr();
        levelMgr = gameObject.AddComponent<LevelMgr>();
    }


    /// <summary>
    /// 模拟 Update
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        if (null != resourceMgr)
        {
            resourceMgr.Update(deltaTime);
        }
    }

    /// <summary>
    /// 模拟 LateUpdate
    /// </summary>
    /// <param name="deltaTime"></param>
    public void LateUpdate(float deltaTime)
    {

    }

    /// <summary>
    /// 模拟 FixedUpdate
    /// </summary>
    /// <param name="fixedDeltaTime"></param>
    public void FixedUpdate(float fixedDeltaTime)
    {

    }

    public void OnApplicationQuit()
    {

    }

    public void OnApplicationPause(bool pause)
    {

    }

    public void OnApplicationFocus(bool focus)
    {

    }

    /// <summary>
    /// 获取系统管理器
    /// </summary>
    /// <returns></returns>
    public SubSysMgr GetSubSysMgr()
    {
        if (null != subSysMgr)
        {
            return subSysMgr;
        }
        Debug.LogWarning("subSysMgr构造异常");
        return null;
    }

    /// <summary>
    /// 获取UI管理器
    /// </summary>
    /// <returns></returns>
    public UIMgr GetUIMgr()
    {
        if (null != uiMgr)
        {
            return uiMgr;
        }
        Debug.LogWarning("uiMgr构造异常");
        return null;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ApplicationQuit()
    {
        Application.Quit();
    }

}
