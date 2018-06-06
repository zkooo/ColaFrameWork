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
    private UGUIHUDFollowTarget HUDComponent;
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
        set { visible = value; }
    }

    public EPateBase(GameObject prefab, GameObject attachTarget, EPateColor color,float offsetH)
    {
        pateObj = CommonHelper.InstantiateGoByPrefab(prefab, null);
        this.pateColor = color;
        AttachTarget(attachTarget,offsetH);
        HUDComponent = this.pateObj.GetComponent<UGUIHUDFollowTarget>();
    }

    public void AttachTarget(GameObject targetObj, float offsetH)
    {

    }

    public void SetActive(bool isActive)
    {

    }

    public GameObject GetAttachObj()
    {

    }

    public float GetAttachOffsetH()
    {

    }

    public void Release()
    {
        this.release = true;
        if (null != pateObj)
        {

        }

        if (null != HUDComponent)
        {

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