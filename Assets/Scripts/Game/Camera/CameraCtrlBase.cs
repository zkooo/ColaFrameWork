using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机控制脚本的基类
/// </summary>
public class CameraCtrlBase
{
    private Transform transform;
    private float fov = 45;

    public CameraCtrlBase()
    {
        GameObject go = new GameObject();
        go.name = this.GetType().ToString();
        this.transform = go.transform;
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public float Fov
    {
        get { return fov; }
        set { fov = value; }
    }

    public virtual bool enable { get; set; }

    /// <summary>
    /// 强制就位
    /// </summary>
    public virtual void ForcePosition() { }

    public virtual void OnSetTouchDeltaX(float value) { }
    public virtual void OnSetTouchDeltaY(float value) { }

    public virtual void OnLateUpdate(float deltaTime, Vector3 shakeOffset) { }
}
