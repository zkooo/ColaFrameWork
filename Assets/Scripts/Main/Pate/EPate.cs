using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各种头顶字的基类
/// </summary>
public class EPateBase
{
    /// <summary>
    /// 头顶字颜色
    /// </summary>
    public EPateColor pateColor;

    /// <summary>
    /// 头顶字的跟随组件
    /// </summary>
    public UGUIHUDFollowTarget HudComponent { get; private set; }

    /// <summary>
    /// 该头顶字是否被释放
    /// </summary>
    public bool release = false;
    /// <summary>
    /// 该头顶字是否逻辑上可见
    /// </summary>
    private bool visible = true;
    /// <summary>
    /// 头顶字绑定的跟随的物体
    /// </summary>
    private EObject followObj;
    /// <summary>
    /// 实例化出来的头顶字预制
    /// </summary>
    private GameObject pateObj;

    /// <summary>
    /// 该头顶字是否逻辑上可见
    /// </summary>
    public bool Visible
    {
        get { return visible; }
        set
        {
            visible = value;
            if (null != HudComponent)
            {
                HudComponent.enableAll = value;
            }
        }
    }

    /// <summary>
    /// 实例化出来的头顶字预制
    /// </summary>
    public GameObject PateObj
    {
        get { return pateObj; }
    }

    public void Create(GameObject pateObj)
    {
        this.pateObj = pateObj;
        OnCreate();
    }

    public virtual void OnCreate()
    {

    }

    public void SetActive(bool isActive)
    {
        if (HudComponent != null)
        {
            HudComponent.gameObject.SetActive(isActive);
        }
    }

    public virtual void Release()
    {
        this.release = true;
        if (null != pateObj)
        {
            var hud = this.pateObj.transform.parent;
            var root = EPateCacheMgr.Instance.GetCacheRoot(this.GetType());
            hud.transform.SetParent(root.transform,false);
            hud.gameObject.SetActive(false);
            pateObj = null;
        }

        if (null != HudComponent)
        {
            HudComponent.target = null;
            HudComponent = null;
        }
    }

}

/// <summary>
/// 头顶字的颜色类
/// </summary>
public class EPateColor
{
    public Color TextColor;
    public Color OutlineColor;

    public EPateColor(Color textColor, Color outlineColor)
    {
        this.TextColor = textColor;
        this.OutlineColor = outlineColor;
    }

}