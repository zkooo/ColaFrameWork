using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI基类
/// </summary>
public class UIBase
{
    /// <summary>
    /// 当前的UI界面GameObject
    /// </summary>
    public GameObject Panel { get; protected set; }

    /// <summary>
    /// 当前的UI界面GameObject对应的唯一资源ID
    /// </summary>
    public int ResId { get; protected set; }

    /// <summary>
    /// UI界面的名字
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// UI界面的深度
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// UI界面的层级
    /// </summary>
    public int Layer
    {
        get { return this.Panel.layer; }
        set { this.Panel.layer = value; }
    }

    /// <summary>
    ///  UI的显隐状态
    /// </summary>
    public bool IsShow { get { return Panel.activeSelf; } }

    /// <summary>
    /// UI的类型
    /// </summary>
    protected UIType uiType;

    /// <summary>
    /// UI的创建方法
    /// </summary>
    protected UICreateType uiCreateType = UICreateType.Res;

    protected UIBase(int resId, UIType uiType)
    {
        this.uiType = uiType;
        ResId = resId;
        this.uiCreateType = UICreateType.Res;
    }

    protected UIBase(GameObject panel, GameObject parent, UIType uiType)
    {
        this.uiType = uiType;
        this.ResId = 0;
        this.Panel = panel;
        this.Panel.transform.SetParent(parent.transform);
        this.Name = panel.name;
        this.uiCreateType = UICreateType.Go;
    }

    /// <summary>
    /// 打开一个UI界面
    /// </summary>
    public virtual void Open()
    {
        this.Create();
    }

    /// <summary>
    /// 关闭一个UI界面
    /// </summary>
    public virtual void Close()
    {
        this.Destroy();
    }

    /// <summary>
    /// UI界面显隐的时候会调用该方法
    /// </summary>
    /// <param name="isShow"></param>UI的是否显示
    public virtual void OnShow(bool isShow)
    {

    }

    /// <summary>
    /// 创建UI界面
    /// </summary>
    public virtual void Create()
    {
        if (UICreateType.Res == uiCreateType)
        {
            if (null != this.Panel)
            {
                GameObject.Destroy(Panel);
            }

        }
        else if (UICreateType.Go == uiCreateType)
        {

        }

        this.OnCreate();
    }

    /// <summary>
    /// UI创建过程中会调用该方法
    /// </summary>
    public virtual void OnCreate()
    {
        this.OnShow(IsShow);
    }

    /// <summary>
    /// 销毁一个UI界面
    /// </summary>
    public virtual void Destroy()
    {
        this.OnDestroy();
    }

    /// <summary>
    /// 销毁UI界面的过程中调用该方法
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// 设置一个UI界面的显隐
    /// </summary>
    /// <param name="isActive"></param>UI界面是否显示
    public void SetActive(bool isActive)
    {
        this.Panel.SetActive(isActive);
        this.OnShow(isActive);
    }
}
