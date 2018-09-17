using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 文件\目录操作助手类
/// </summary>
public static class FileHelper
{

    /// <summary>
    /// 创建一个目录
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isContainFileName"></param>
    public static void MkDir(string path, bool isContainFileName = true)
    {
        if (isContainFileName)
        {
            path = Path.GetDirectoryName(path);
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 以二进制的形式读取对应路径的文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] ReadBytes(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }
        return File.ReadAllBytes(filePath);
    }

    /// <summary>
    /// 以文本的形式读取对应路径的文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string ReadString(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }
        return File.ReadAllText(filePath);
    }

    public static void WriteBytes(string filePath, byte[] content)
    {

    }
}
