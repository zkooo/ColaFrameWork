using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打bundle助手
/// </summary>
public class BundleBuildHelper
{
    /// <summary>
    /// 需要打bundle的资源路径
    /// </summary>
    private static readonly List<string> BundlePathList = new List<string>()
    {
        "/Resources/Arts/Gui",
        "/Scenes",
    };

    /// <summary>
    /// bundle的输出路径
    /// </summary>
    private static readonly string abOutputPath = Application.streamingAssetsPath + "/Output";

    /// <summary>
    /// 资源的存放目录
    /// </summary>
    private static readonly string assetDir = Application.dataPath;

    /// <summary>
    /// 资源的前缀目录
    /// </summary>
    private static string frontDirName = "Assets/Resources/";

    /// <summary>
    /// 给所有的设置了bundleName标签的资源打bundle
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/Build All_BundlesManual", false, 5)]
    public static void BuildAllAssetBundlesManual()
    {
        if (!Directory.Exists(abOutputPath))
        {
            Directory.CreateDirectory(abOutputPath);
        }
        BuildPipeline.BuildAssetBundles(abOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 使用脚本给资源自动设置bundle标签
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/SetBundleNameAuto", false, 2)]
    public static void SetBundleNameAuto()
    {
        ClearAssetBundlesName();
        for (int i = 0; i < BundlePathList.Count; i++)
        {
            SetAssetBundleNameByPath(assetDir + BundlePathList[i]);
        }
    }

    /// <summary>
    /// 自动给指定目录中的资源设置bundlename并打出bundle，然后拷贝到指定目录
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/BuildAssetBundlesAutoCopy", false, 3)]
    public static void BuildAssetBundlesAutoCopy()
    {
        SetBundleNameAuto();
        BuildAllAssetBundlesManual();
        ClearAssetBundlesName();
        //拷贝bundle到外面的目录下
        CopyFolder(abOutputPath, assetDir + "/../Output");
    }

    /// <summary>
    /// 自动给指定目录中的资源设置bundlename并打出bundle（增量更新）
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/BuildAssetBundlesAuto", false, 4)]
    public static void BuildAssetBundlesAuto()
    {
        SetBundleNameAuto();
        BuildAllAssetBundlesManual();
        ClearAssetBundlesName();
    }

    /// <summary>
    /// 强制删除资源重新打Bundle
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/ReBuildAssetBundlesAuto", false, 4)]
    public static void ReBuildAssetBundlesAuto()
    {
        if (Directory.Exists(abOutputPath))
        {
            Directory.Delete(abOutputPath, true);
        }
        SetBundleNameAuto();
        BuildAllAssetBundlesManual();
        ClearAssetBundlesName();
    }

    /// <summary>
    /// 对选中的资源分别打bundle
    /// </summary>
    [MenuItem("Assets/NewBundleTools/BuildBundleFromSelection")]
    private static void BuildBundleFromSelection()
    {
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
            Object[] targets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            for (int i = 0; i < targets.Length; i++)
            {
                if (null == targets[i])
                {
                    continue;
                }
                string assetPath = AssetDatabase.GetAssetPath(targets[i]);
                if (null != assetPath)
                {
                    //跳过文件夹
                    if (!File.Exists(assetPath)) continue;
                    //跳过脚本文件和meta文件
                    if (assetPath.EndsWith(".cs") || assetPath.EndsWith(".meta")) continue;
                    AssetBundleBuild abb = new AssetBundleBuild();
                    string fullPath = assetPath.Replace(frontDirName, "");  //包含文件名、拓展名的全路径
                    abb.assetBundleName = fullPath + GloablDefine.extenName;
                    //abb.assetBundleVariant = "hd";
                    abb.assetNames = new[] { assetPath };
                    BuildPipeline.BuildAssetBundles(path, new AssetBundleBuild[1] { abb },
                        BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
                }
            }

            // Selection.objects = targets;
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
    /// </summary>
    [MenuItem("ColaFramework/AssetBundle/ClearAssetBundlesName", false, 1)]
    public static void ClearAssetBundlesName()
    {
        //获取所有的AssetBundle名称
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();

        //强制删除所有的AssetBundle名称
        for (int i = 0; i < abNames.Length; i++)
        {
            EditorUtility.DisplayProgressBar("AssetBundle", string.Format("Clear AssetBundle Name: {0}", abNames[i]), .5f);
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 设置所有在指定路径下的AssetBundleName 
    /// </summary>
    /// <param name="assetPath"></param>
    private static void SetAssetBundleNameByPath(string assetPath)
    {
        var fileInfos = Directory.GetFiles(assetPath, "*.*", SearchOption.AllDirectories);

        for (int i = 0; i < fileInfos.Length; i++)
        {
            EditorUtility.DisplayProgressBar("AssetBundle", string.Format("Set AssetBundle Name: {0}", assetPath), .5f);
            if (!fileInfos[i].EndsWith(".meta") && !fileInfos[i].EndsWith(".cs"))//如果是文件的话，则设置AssetBundleName，并排除掉.meta文件和cs文件
            {
                //逐个设置AssetBundleName  
                SetABName(fileInfos[i]);
            }
        }
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 设置单独的某个AssetBundle的Name
    /// </summary>
    /// <param name="assetPath"></param>
    private static void SetABName(string assetPath)
    {
        string subDir = assetPath.Substring(assetDir.Length);
        string importPath = string.Format("Assets{0}", subDir);
        string bundleName = importPath.Replace(frontDirName, "");
        AssetImporter assetImporter = AssetImporter.GetAtPath(importPath);
        assetImporter.assetBundleName = bundleName + GloablDefine.extenName;
    }

    /// <summary>
    /// 拷贝资源
    /// </summary>
    /// <param name="sPath"></param>
    /// <param name="dPath"></param>
    public static void CopyFolder(string sPath, string dPath)
    {
        if (!Directory.Exists(dPath))
        {
            Directory.CreateDirectory(dPath);
        }

        DirectoryInfo sDir = new DirectoryInfo(sPath);
        FileInfo[] fileArray = sDir.GetFiles();
        foreach (FileInfo file in fileArray)
        {
            if (file.Extension != ".meta" && file.Extension != ".manifest")
                file.CopyTo(dPath + "/" + file.Name, true);
        }
        //递归复制子文件夹
        DirectoryInfo[] subDirArray = sDir.GetDirectories();
        foreach (DirectoryInfo subDir in subDirArray)
        {
            CopyFolder(subDir.FullName, dPath + "/" + subDir.Name);
        }
    }
}

