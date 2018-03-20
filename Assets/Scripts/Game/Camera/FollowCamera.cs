using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机控制
/// 当检测到相机与目标之间存在碰撞物体的时候，相机将来拉近到角色以避开碰撞物
/// 勿随意修改参数以及暴露接口
/// </summary>
public class FollowCamera
{
    private float _minPitch = -10;              //最小仰角(相机朝向目标的最小角度)
    private float _maxPitch = 75;               //最大仰角
    private float _defaultFollowDistance = 8;   //默认跟随距离
    private float _minZoomDistance = 2;         //最zoom距离
    private float _raycastDistance = 8;         //射线检测的发射长度

    private Vector3 _raycastOrgin;                          //射线检测的发射源
    private Vector3 _raycastDir;                            //射线检测的发射方向
    private Vector3 _raycastOffset = new Vector3(0, -1, 0); //射线目标的偏移量（防止相机进入地面）
    private RaycastHit _racasthitInfo;
    private float _targetRadius = 1;        //相机到角色的期望距离
    private float _distance = 1;            //相机到角色的当前距离
    private float _zoomSpeed = 6;           //镜头推拉的速度（米/秒）
    private float _rotateSpeedFade = 10;    //旋转速度衰减值(角度/秒)
    private float _targetSpeedFade = 10;    //目标旋转速度衰减值(角度/秒)
    private float _startSpeedScale = 0.1f;  //初始速度缩放

    private Transform _cameraTransform; //相机
    private Transform _target;          //相机跟踪的目标
    private Vector3 _targetOffset;      //目标偏移
    private int _raycastLayer;      //接受射线检测的掩码
    private float _yawDegree;       //当前偏航角
    private float _yawSpeed;        //当前偏航角速度
    private float _pitchDegree;     //当前仰角
    private float _pitchSpeed;      //当前仰角速度

    private float _targetYawSpeed;      //当前设定的偏航角速度（degree/秒）
    private float _targetPitchSpeed;    //当前设定的仰角速度（degree/秒）

    public FollowCamera()
    {
        _targetRadius = _defaultFollowDistance;
        _distance = _targetRadius;
    }

    /// <summary>
    /// 最小仰角(相机朝向目标的最小角度)
    /// </summary>
    public float minPitch
    {
        get { return _minPitch; }
        set { _minPitch = value; }
    }

    /// <summary>
    /// 最大仰角(相机朝向目标的最大角度)
    /// </summary>
    public float maxPitch
    {
        get { return _maxPitch; }
        set { _maxPitch = value; }
    }

    public float targetSpeedFade
    {
        get { return _targetSpeedFade; }
        set { _targetSpeedFade = value; }
    }

    /// <summary>
    /// 初始速度缩放比例
    /// </summary>
    public float startSpeedScale
    {
        get { return _startSpeedScale; }
        set { _startSpeedScale = value; }
    }

    /// <summary>
    /// 相机到目标的默认距离
    /// </summary>
    public float defaultFollowDistance
    {
        get { return _defaultFollowDistance; }
        set { _defaultFollowDistance = value; }
    }

    /// <summary>
    /// 镜头推拉速度
    /// </summary>
    public float zoomSpeed
    {
        get { return _zoomSpeed; }
        set { _zoomSpeed = value; }
    }

    public float rotateSpeedFade
    {
        get { return _rotateSpeedFade; }
        set { _rotateSpeedFade = value; }
    }

    /// <summary>
    /// 相机到目标的最近距离
    /// </summary>
    public float minZoomDistance
    {
        get { return _minZoomDistance; }
        set { _minZoomDistance = value; }
    }

    /// <summary>
    /// 射线检测的发射长度
    /// </summary>
    public float raycastDistance
    {
        get { return _raycastDistance; }
        set { _raycastDistance = value; }
    }

    /// <summary>
    /// 相机当前偏航角
    /// </summary>
    public float yawDegree
    {
        get { return _yawDegree; }
    }

    /// <summary>
    /// 相机当前仰角
    /// </summary>
    public float pitchDegree
    {
        get { return _pitchDegree; }
    }

    /// <summary>
    /// 正在操作的相机
    /// </summary>
    public Transform cameraTransform
    {
        get { return _cameraTransform; }
        set { _cameraTransform = value; }
    }

    /// <summary>
    /// 跟踪的目标
    /// </summary>
    public Transform target
    {
        get { return _target; }
        set { _target = value; }
    }

    /// <summary>
    /// 跟踪目标位置的偏移值，例如跟踪目标位置向上2个单位的地方
    /// </summary>
    public Vector3 targetOffset
    {
        get { return _targetOffset; }
        set { _targetOffset = value; }
    }

    /// <summary>
    /// 接受碰撞检测的层
    /// </summary>
    public int raycastLayer
    {
        get { return _raycastLayer; }
        set { _raycastLayer = value; }
    }

    /// <summary>
    /// 当前相机到目标的距离
    /// </summary>
    public float distance
    {
        get { return _distance; }
    }

    /// <summary>
    /// 期望旋转到指定角度
    /// </summary>
    public void DesireRotateTo(float yaw, float pitch)
    {
        float deltaYaw = yaw - yawDegree;
        float deltaPitch = pitch - yawDegree;
        SetRotSpeed(deltaYaw, deltaPitch);
    }

    /// <summary>
    /// 旋转到跟随目标的forward方向
    /// </summary>
    public void RotateToTargetForward()
    {
        Vector3 temp = _target.forward;
        temp.y = 0;
        temp.Normalize();
        float angle = Mathf.Asin(temp.z) * Mathf.Rad2Deg;
        angle = Mathf.Abs(angle);
        if (temp.x < 0 && temp.z > 0)
            angle = 90 + 90 - angle;
        else if (temp.x < 0 && temp.z < 0)
            angle = 180 + angle;
        else if (temp.x > 0 && temp.z < 0)
            angle = 270 + 90 - angle;
        DesireRotateTo(angle, 45);
    }

    /// <summary>
    /// 设置旋转初速度，使用角度值（degree）不是弧度值
    /// </summary>
    /// <param name="yaw">仰角</param>
    /// <param name="pitch">偏航角</param>
    public void SetRotSpeed(float yaw, float pitch)
    {
        _targetYawSpeed = yaw * _startSpeedScale;
        _targetPitchSpeed = pitch * _startSpeedScale;
    }

    public void Update(float deltaTime)
    {
        if (!_cameraTransform || !_target) return;
        //碰撞检测
        _raycastOrgin = _target.position + _targetOffset;
        _raycastDir = (cameraTransform.position + _raycastOffset - _raycastOrgin).normalized;
        _racasthitInfo = new RaycastHit();
        _targetRadius = _defaultFollowDistance;
        if (Physics.Raycast(_raycastOrgin, _raycastDir, out _racasthitInfo, _raycastDistance, _raycastLayer))
        {
            _targetRadius = Mathf.Min(_racasthitInfo.distance, _defaultFollowDistance);
        }
        //限制最近距离
        _targetRadius = Mathf.Max(_targetRadius, _minZoomDistance);
        //镜头拉近
        _distance = Mathf.Lerp(_distance, _targetRadius, deltaTime * _zoomSpeed);
        //目标值衰减，防止突然设置角速度造成的抖动&不平稳
        _yawSpeed = Mathf.Lerp(_yawSpeed, _targetYawSpeed, deltaTime * _rotateSpeedFade);
        _pitchSpeed = Mathf.Lerp(_pitchSpeed, _targetPitchSpeed, deltaTime * _rotateSpeedFade);
        _targetYawSpeed = Mathf.Lerp(_targetYawSpeed, 0, deltaTime * _targetSpeedFade);
        _targetPitchSpeed = Mathf.Lerp(_targetPitchSpeed, 0, deltaTime * _targetSpeedFade);
        //相机变换
        _pitchDegree += _pitchSpeed;
        _yawDegree += _yawSpeed;
        _pitchDegree = _pitchDegree % 360;
        _yawDegree = _yawDegree % 360;
        _pitchDegree = Mathf.Max(_pitchDegree, _minPitch);
        _pitchDegree = Mathf.Min(_pitchDegree, _maxPitch);
        float y = Mathf.Sin(_pitchDegree * Mathf.Deg2Rad);
        float x = Mathf.Cos(_yawDegree * Mathf.Deg2Rad);
        float z = Mathf.Sin(_yawDegree * Mathf.Deg2Rad);
        float tempRadius = Mathf.Cos(_pitchDegree * Mathf.Deg2Rad) * _distance;
        Vector3 raltionPos = new Vector3(x * tempRadius, y * _distance, z * tempRadius);
        _cameraTransform.position = _target.position + _targetOffset + raltionPos;
        Vector3 forward = _target.position + _targetOffset - _cameraTransform.position;
        if (forward.sqrMagnitude >= 0)
        {
            _cameraTransform.forward = _target.position + _targetOffset - _cameraTransform.position;
        }
    }
}
