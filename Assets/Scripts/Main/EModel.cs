using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColaFrame的Model模型基类
/// </summary>
public class EModel
{
    /// <summary>
    /// 对应的原始资源
    /// </summary>
    private GameObject asset;
    /// <summary>
    /// 实例化出来的模型
    /// </summary>
    private GameObject model;
    /// <summary>
    /// 模型上面的Animation组件
    /// </summary>
    private Animation animation;
    /// <summary>
    /// 模型上面的Animator组件
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 资源名称
    /// </summary>
    private string resName;
    /// <summary>
    /// Pate挂载点的位置
    /// </summary>
    private Vector3 HUDPos;
    /// <summary>
    /// 是否可见
    /// </summary>
    private bool visible;

    public EModel()
    {

    }

    public void Reset()
    {

    }

    public bool IsLoaded()
    {
        return model != null;
    }

    public void Load(int resId, Action callback)
    {

    }

    public void OnModelLoaded(GameObject asset, GameObject model, Action callback)
    {

    }

    public EModel Clone()
    {
        //TODO:Clone
        return this;
    }

    public EModel CloneFromObj(GameObject srcObj)
    {
        //TODO:从一个Gameobecjt Clone
        return this;
    }

    public void SetActive(bool isActive)
    {
        if (this.model)
        {
            this.model.SetActive(isActive);
        }
    }

    public void SetClickEnable(bool isOK)
    {
        if (this.model)
        {
            var collider = this.model.GetComponent<BoxCollider>();
            if (null != collider)
            {
                collider.enabled = isOK;
            }
        }
    }

    public void Destroy()
    {

    }
}
