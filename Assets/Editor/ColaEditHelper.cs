#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ColaFramework 编辑器助手类
/// </summary>
public class ColaEditHelper
{

    /// <summary>
    /// 打开指定文件夹(编辑器模式下)
    /// </summary>
    /// <param name="path"></param>
    public static void OpenDirectory(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        path = path.Replace("/", "\\");
        if (!Directory.Exists(path))
        {
            Debug.LogError("No Directory: " + path);
            return;
        }

        System.Diagnostics.Process.Start("explorer.exe", path);
    }

    /// <summary>
    /// 合并Lua代码，并复制到StreamingAsset目录中准备打包
    /// </summary>
    public static void BuildLuaToStreamingAsset()
    {
        RmDir(LuaConst.streamingAssetLua);
        CopyDir(LuaConst.toluaDir, LuaConst.streamingAssetLua);
        CopyDir(LuaConst.luaDir, LuaConst.streamingAssetLua);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path"></param> 路径
    /// <param name="isOverride"></param> 是否覆盖原有同名目录
    public static void Mkdir(string path, bool isOverride = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        else if(Directory.Exists(dir) && isOverride)
        {
            Directory.Delete(dir,true);
            Directory.CreateDirectory(dir);
        }
    }

    /// <summary>
    /// 删除目录
    /// </summary>
    /// <param name="path"></param>
    public static void RmDir(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        string dir = Path.GetDirectoryName(path);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir,true);
        }
    }

    public static void CopyDir(string srcPath, string destPath)
    {
        if(string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(destPath))
        {
            return;
        }
        Mkdir(destPath);
        DirectoryInfo dir = new DirectoryInfo(srcPath);
        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            if (i is DirectoryInfo)     //判断是否文件夹
            {
                if (!Directory.Exists(destPath + "\\" + i.Name))
                {
                    Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                }
                CopyDir(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
            }
            else
            {
                File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
            }
        }
    }
}

#endif
