using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 下载视频的工具类
/// </summary>
public class DownloadMovHelper
{
    /// <summary>
    /// 是否正在下载的标志位
    /// </summary>
    private static bool isLoading = false;

    /// <summary>
    /// 本地文件的路径
    /// </summary>
    private static string localFilePath;

    /// <summary>
    /// 下载的网络资源路径
    /// </summary>
    private static string downloadURL;

    private static Action _onLoading;
    private static Action _onCompleted;
    private static Action<DownLoadMovError> _onFailed;
    private static Action<int> _onProgress;

    /// <summary>
    /// 对外提供的开始下载接口
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="url"></param>
    /// <param name="onLoading"></param>
    /// <param name="onComplete"></param>
    /// <param name="onFailed"></param>
    /// <param name="onProgress"></param>
    public static void Begin(string filePath, string url, Action onLoading, Action onComplete, Action<DownLoadMovError> onFailed, Action<int> onProgress)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(url))
        {
            Debug.LogWarning("视频本地路径或者网络资源路径为空！");
            if (null != onFailed)
            {
                onFailed(DownLoadMovError.PathError);
            }
            return;
        }
        
    }

    /// <summary>
    /// 停止下载
    /// </summary>
    public static void Stop()
    {

    }

    /// <summary>
    /// 释放清理之前的回调和下载进程，本地缓存不会被清理掉
    /// </summary>
    public static void Release()
    {

    }

    /// <summary>
    /// 删除本地的视频资源缓存
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="onComplete"></param>
    /// <param name="onFailed"></param>
    public static void DeleteVideo(string filePath, Action onComplete, Action onFailed)
    {

    }



    /// <summary>
    /// 检查本地文件是否存在,如果目录不存在则创建目录
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool CheckLocalFileExist(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }
        string dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
            return false;
        }

        if (File.Exists(filePath))
        {
            return true;
        }
        return false;
    }
}

/// <summary>
/// 下载视频错误的枚举码
/// </summary>
public enum DownLoadMovError : byte
{
    /// <summary>
    /// 路径错误
    /// </summary>
    PathError,

}