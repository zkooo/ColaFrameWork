using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Threading;

/// <summary>
/// StreamingAsset助手类
/// </summary>
public static class StreamingAssetHelper
{
    /// <summary>
    /// 读取写资源路径
    /// </summary>
    private static String assetPath;

    public static String AssetPathDir
    {
        get { return assetPath; }
    }

    /// <summary>
    /// 在可读写路径下设置一个streamingassets路径
    /// </summary>
    /// <param name="debugDir"></param>
    public static void SetAssetPathDir(String debugDir)
    {
        assetPath = Path.Combine(debugDir, "streamingassets");
    }

    /// <summary>
    /// 拼接StreamingAsset的子路径
    /// </summary>
    /// <param name="subPath"></param>
    /// <returns></returns>
    public static String CombinePath(String subPath)
    {
        return Path.Combine(Application.streamingAssetsPath, subPath);
    }

    /// <summary>
    /// 读取文件(从可读写资源路径或者StreamingAsset中)
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static String ReadFileText(String filePath)
    {
        String debugPath = assetPath != null ? Path.Combine(assetPath, filePath) : null;
        //先尝试从debupath中读取文件，如果没有再去StreamingAsset中读取
        if (debugPath != null && File.Exists(debugPath))
        {
            return File.ReadAllText(debugPath);
        }
        else
        {
            string fullPath = CombinePath(filePath);
#if UNITY_ANDROID && !UNITY_EDITOR
			WWW www = new WWW(fullPath); 
			while(!www.isDone){}
			return www.text;
#else
            return File.ReadAllText(fullPath);
#endif
        }
    }

#if UNITY_ANDROID && ( ! UNITY_EDITOR)
	/// <summary>
	/// 在Android平台上，从(apk包中的) Assets目录复制文件至指定目录
	/// 注意：只能在子线程中调用
	/// </summary>
	/// <param name="sourceDir">源目录，是以 assets 为根目录的相对路径</param>
	/// <param name="destDir">目标目录</param>
	/// <param name="title">进度对话框标题</param>
	/// <returns></returns>
	public static Boolean CopyAssetDirectoryInThread(String sourceDir, String destDir, String title)
	{
		Boolean ret = false;
		MainThreadTask.RunUntilFinish(()=>
			{
				ret = CopyAssetDirectory(sourceDir, destDir, title);
			});
		return ret;
	}

	/// <summary>
	/// 在Android平台上，从(apk包中的) Assets目录复制文件至指定目录
	/// 注意：只能在主线程中调用
	/// </summary>
	/// <param name="sourceDir">源目录，是以 assets 为根目录的相对路径</param>
	/// <param name="destDir">目标目录</param>
	/// <param name="title">进度对话框标题</param>
	/// <returns></returns>
	public static Boolean CopyAssetDirectory(String sourceDir, String destDir, String title)
	{
		Debug.Log("CopyAssetDirectory, sourceDir = " + sourceDir + ", destDir = " + destDir);

		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActitivy = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		
		AndroidJavaClass CopyAssetDirectory = new AndroidJavaClass("com.pwrd.copyassetdirectory.CopyAssetDirectory");

		Debug.Log("CopyAssetDirectory before execute");

		CopyAssetDirectory.CallStatic("execute",
			currentActitivy, sourceDir, destDir, title);

		Debug.Log("CopyAssetDirectory finished");
		return true;
  }
#endif 
}
