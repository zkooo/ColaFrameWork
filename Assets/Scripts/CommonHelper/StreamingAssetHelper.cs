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
    private static String m_debugDir;
    public static String DebugDir
    {
        get { return m_debugDir; }
    }

    public static void SetDebugBaseDir(String debugDir)
    {
        m_debugDir = Path.Combine(debugDir, "streamingassets");
    }

    public static String MakePath(String subPath)
    {
        return Path.Combine(Application.streamingAssetsPath, subPath);
    }

    public static String ReadFileText(String filePath)
    {
        String debugPath = m_debugDir != null ? Path.Combine(m_debugDir, filePath) : null;
        if (debugPath != null && File.Exists(debugPath))        //先尝试调试用目录：assetpath/streamingassets/...
        {
            return File.ReadAllText(debugPath);
        }
        else
        {
            string fullPath = MakePath(filePath);
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
