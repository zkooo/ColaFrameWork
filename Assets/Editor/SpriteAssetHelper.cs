using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SpriteAssetHelper
{
    [MenuItem("Asset/Create Or Update Sprite Asset")]
    public static void Execute()
    {
        Object obj = Selection.activeObject;
        CreateOrUpdateSpriteAsset(obj, null);
    }

    /// <summary>
    /// 创建或者更新一个图集的资源信息
    /// </summary>
    /// <param name="targetObj"></param>
    /// <param name="fullFileName"></param>
    public static void CreateOrUpdateSpriteAsset(Object targetObj, string fullFileName)
    {
        if (null == targetObj || targetObj.GetType() != typeof(Texture2D))
        {
            return;
        }

    }

    public static List<SpriteAssetInfo> GetSpriteAssetInfos(Texture2D texture2D)
    {

    }
}
