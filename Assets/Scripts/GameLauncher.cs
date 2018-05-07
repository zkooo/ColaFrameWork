using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 游戏入口:游戏启动器类
/// </summary>
public class GameLauncher : MonoBehaviour
{
    /// <summary>
    /// 存放资源的可读写路径
    /// </summary>
    public string AssetPath;

    private static GameLauncher instance;
    private GameManager gameManager;
    private GameObject fpsHelperObj;
    private FPSHelper fpsHelper;
    private LogHelper logHelper;
    private InputMgr inputMgr;

    public static GameLauncher Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        InitPath();
        gameManager = GameManager.GetInstance();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        DontDestroyOnLoad(gameObject);
        //加入输入输出管理器
        inputMgr = gameObject.AddComponent<InputMgr>();

#if SHOW_FPS
        fpsHelperObj = new GameObject("FpsHelperObj");
        fpsHelper = fpsHelperObj.AddComponent<FPSHelper>();
        GameObject.DontDestroyOnLoad(fpsHelperObj);
#endif

#if BUILD_DEBUG_LOG || UNITY_EDITOR
#if UNITY_2017
        Debug.unityLogger.logEnabled = true;
#else
        Debug.logger.logEnabled = true;
#endif
#else
#if UNITY_2017
        Debug.unityLogger.logEnabled = false;
#else
        Debug.logger.logEnabled = false;
#endif
#endif

#if WETEST_SDK
        gameObject.AddComponent<WeTest.U3DAutomation.U3DAutomationBehaviour>();
#endif

#if OUTPUT_LOG
        GameObject logHelperObj = new GameObject("LogHelperObj");
        logHelper = logHelperObj.AddComponent<LogHelper>();
        GameObject.DontDestroyOnLoad(logHelperObj);

        Application.logMessageReceived += logHelper.LogCallback;
#endif
        StreamingAssetHelper.SetAssetPathDir(AssetPath);
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(InitGameCore());
    }

    void Update()
    {
        gameManager.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        gameManager.LateUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        gameManager.FixedUpdate(Time.fixedDeltaTime);
    }


    private void OnApplicationQuit()
    {
        gameManager.OnApplicationQuit();
    }

    private void OnApplicationPause(bool pause)
    {
        gameManager.OnApplicationPause(pause);
    }

    private void OnApplicationFocus(bool focus)
    {
        gameManager.OnApplicationFocus(focus);
    }

    public void ApplicationQuit(string exitCode = "0")
    {
        gameManager.ApplicationQuit();
    }

    IEnumerator InitGameCore()
    {
        yield return new WaitForEndOfFrame();
        gameManager.InitGameCore(gameObject);
    }

    /// <summary>
    /// 初始化一些路径
    /// </summary>
    private void InitPath()
    {
        if (string.IsNullOrEmpty(AssetPath))
        {
#if UNITY_IPHONE
            AssetPath = Application.temporaryCachePath;
#else
            AssetPath = Application.persistentDataPath;
#endif
        }
        else if (AssetPath.StartsWith("./") || AssetPath.StartsWith("../"))
        {
            AssetPath = Application.dataPath + "/" + AssetPath;
        }
    }
}
