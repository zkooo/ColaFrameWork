using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含某个图集内的精灵资源的信息类(方便程序取用)
/// </summary>
[System.Serializable]
public class SpriteAsset : MonoBehaviour
{
    /// <summary>
    /// 存储某个图集内的所有精灵信息
    /// </summary>
    public List<SpriteAssetInfo> SpriteAssetInfos;

    /// <summary>
    /// 根据Sprite的name获取对应的Sprite
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetSpriteByName(string name)
    {
        for (int i = 0; i < SpriteAssetInfos.Count; i++)
        {
            if (SpriteAssetInfos[i].name.Equals(name))
            {
                return SpriteAssetInfos[i].sprite;
            }
        }
        Debug.LogWarning(string.Format("没有找到Name为:{0}对应的Sprite!", name));
        return null;
    }
}


/// <summary>
/// 单张图片/精灵的资源信息类
/// </summary>
[System.Serializable]
public class SpriteAssetInfo
{
    /// <summary>
    /// id
    /// </summary>
    public int id;

    /// <summary>
    /// 名称
    /// </summary>
    public string name;

    /// <summary>
    /// 精灵
    /// </summary>
    public Sprite sprite;
}
