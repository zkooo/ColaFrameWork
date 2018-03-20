using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : CameraCtrlBase
{

    private float _minPitch = -10;                          //最小仰角(相机朝向目标的最小角度)
    private float _maxPitch = 75;                           //最大仰角
    private float _maxFollowDistance = 10;                  //最大跟随距离
    private float _minFollowistance = 2;                    //最小跟随距离
    private float _defaultFollowDistance = 6;               //默认跟随距离
    private Vector3 _raycastOffset = new Vector3(0, -1, 0); //射线目标的偏移量（防止相机进入地面）
    private RaycastHit _racasthitInfo;
    private float _distance = 1;                            //相机到角色的当前距离
    private float _zoomSpeed = 6;                           //镜头推拉的速度（米/秒）
    private float _rotateSpeed = 10;                        //旋转速度
    private float _followSpeed = 15;                        //相机的跟随速度
    private float _followAutoRotYawSpeed = 2.5f;            //相机自动旋转时的速度

    private Transform _target;                              //相机跟踪的目标
    private Vector3 _targetOffset = new Vector3(0, 1.5f, 0);//目标偏移
    private int _raycastLayer;                              //接受射线检测层的掩码
    private float _yawDegree;                               //当前偏航角
    private float _pitchDegree;                             //当前仰角

    private float _targetYaw;
    private float _targetPitch;
    private bool _lockYaw = false;
    private bool _lockPitch = false;
    private bool _lockZoom = false;
    private Vector3 _currentTargetPosition = new Vector3();
    private Vector3 _prePos = new Vector3();                    //上一帧target所在的位置
    private bool _enable = true;

    public MainCameraController()
    {
        defaultFollowDistance = defaultFollowDistance;
        Fov = 45;
    }

    private VirtualJoystick _joystick;
    public VirtualJoystick joystick
    {
        get
        {
            if (!_joystick)
            {


                GameObject go = CommonHelper.FindChildByPath(GUIHelper.GetUIRootObj(), "ui_set_main_v1/Nested_Main_Joystick/Landscape/ui_main_joystick/Joystick");
                _joystick = go ? go.GetComponent<VirtualJoystick>() : null;
            }
            return _joystick;
        }
    }

    public override bool enable
    {
        get { return _enable; }
        set { _enable = value; }
    }

    private float CheckPitch(float val)
    {
        val = Math.Min(val, 89);
        val = Math.Max(val, -89);
        return val;
    }

    //锁定偏航角，外部修改无效，但是内部修改仍有效
    public bool lockYaw
    {
        get { return _lockYaw; }
        set { _lockYaw = value; }
    }

    //锁定俯仰角，外部修改无效，但是内部修改仍有效
    public bool lockPitch
    {
        get { return _lockPitch; }
        set { _lockPitch = value; }
    }

    //锁定俯Zoom，外部修改无效，但是内部修改仍有效
    public bool lockZoom
    {
        get { return _lockZoom; }
        set { _lockZoom = value; }
    }

    public float followSpeed
    {
        get { return _followSpeed; }
        set { _followSpeed = value; }
    }

    public float followAutoRotYawSpeed
    {
        get { return _followAutoRotYawSpeed; }
        set { _followAutoRotYawSpeed = value; }
    }

    //相机朝向目标的最小俯仰角
    public float minPitch
    {
        get { return _minPitch; }
        set
        {
            value = CheckPitch(value);
            _minPitch = value > _maxPitch ? _maxPitch : value;
        }
    }

    //相机朝向目标的最大俯仰角
    public float maxPitch
    {
        get { return _maxPitch; }
        set
        {
            value = CheckPitch(value);
            _maxPitch = value < _minPitch ? _minPitch : value;
        }
    }

    //相机推拉速度
    public float zoomSpeed
    {
        get { return _zoomSpeed; }
        set { _zoomSpeed = value; }
    }

    //旋转速度，俯仰角和偏航角
    public float rotateSpeed
    {
        get { return _rotateSpeed; }
        set { _rotateSpeed = value; }
    }

    //最大跟随距离
    public float maxFollowDistance
    {
        get { return _maxFollowDistance; }
        set
        {
            //_maxFollowDistance = value < _minFollowistance ? _minFollowistance : value;
            _maxFollowDistance = Math.Max(0, value);
            _maxFollowDistance = Math.Max(_minFollowistance, _maxFollowDistance);
            defaultFollowDistance = defaultFollowDistance;
        }
    }

    //最小跟随距离
    public float minFollowistance
    {
        get { return _minFollowistance; }
        set
        {
            //_minFollowistance = value > _maxFollowDistance ? _maxFollowDistance : value;
            _minFollowistance = Math.Max(0, value);
            _minFollowistance = Math.Min(_maxFollowDistance, _minFollowistance);
            defaultFollowDistance = defaultFollowDistance;
        }
    }

    public float defaultFollowDistance
    {
        get { return _defaultFollowDistance; }
        set
        {
            _defaultFollowDistance = Math.Max(0, value);
            _defaultFollowDistance = Math.Min(_maxFollowDistance, _defaultFollowDistance);
            _defaultFollowDistance = Math.Max(_minFollowistance, _defaultFollowDistance);
        }
    }

    //相机当前偏航角
    public float yawDegree
    {
        get { return _yawDegree; }
    }

    //相机当前俯仰角
    public float pitchDegree
    {
        get { return _pitchDegree; }
    }

    //跟踪的目标
    public Transform target
    {
        get { return _target; }
        set { _target = value; }
    }

    //跟踪目标位置的偏移值，例如跟踪目标位置向上2个单位的地方
    public Vector3 targetOffset
    {
        get { return _targetOffset; }
        set { _targetOffset = value; }
    }

    //接受碰撞检测的层
    public int raycastLayer
    {
        get { return _raycastLayer; }
        set { _raycastLayer = value; }
    }

    //当前相机到目标的距离
    public float distance
    {
        get { return _distance; }
    }

    //public ECCameraShake cameraShaker
    //{
    //    get { return ECGame.Instance.MainCamera.cameraShaker; }
    //    set { ECGame.Instance.MainCamera.cameraShaker = value; }
    //}

    //public ECCameraPush cameraPush
    //{
    //    get { return ECGame.Instance.MainCamera.cameraPush; }
    //    set { ECGame.Instance.MainCamera.cameraPush = value; }
    //}

    public override void OnSetTouchDeltaX(float val)
    {
        targetYaw -= val;
    }

    public override void OnSetTouchDeltaY(float val)
    {
        targetPitch -= val;
    }

    //获取向量水平方向角度（逆时针绕x轴正方向）
    public static float GetDegree(Vector3 val)
    {
        val.y = 0;
        val.Normalize();
        float angle = Mathf.Asin(val.z) * Mathf.Rad2Deg;
        angle = Mathf.Abs(angle);
        if (val.x < 0 && val.z > 0)
            angle = 90 + 90 - angle;
        else if (val.x < 0 && val.z < 0)
            angle = 180 + angle;
        else if (val.x > 0 && val.z < 0)
            angle = 270 + 90 - angle;
        return angle;
    }

    //获取向量的俯仰角
    public static float CalPitch(Vector3 val)
    {
        float angle = Vector3.Angle(val, new Vector3(0, 1, 0));
        return 90 - angle;
    }

    //将角度转换到0~360
    public static float GetNormalDegree(float val)
    {
        val = val % 360;
        val += 360;
        val = val % 360;
        return val;
    }

    //旋转到目标角度需要的旋转量（最短旋转量，小于180）
    public float GetDeltaDegRotTo(float degFrom, float degTo)
    {
        degTo = GetNormalDegree(degTo);
        degFrom = GetNormalDegree(degFrom);
        //当旋转量大于180度则选择反向旋转
        if (degTo - degFrom > 180)
        {
            degTo -= 360;
        }
        else if (degTo - degFrom < -180)
        {
            degFrom -= 360;
        }
        float delta = degTo - degFrom;
        return delta;
    }

    //旋转到指定角度
    public void RotateTo(float tarYawDeg, float tarPitchDegree)
    {
        float deltaYaw = GetDeltaDegRotTo(_yawDegree, tarYawDeg);
        float deltaPitch = GetDeltaDegRotTo(_pitchDegree, tarPitchDegree);
        targetYaw = _yawDegree + deltaYaw;
        targetPitch = _pitchDegree + deltaPitch;
    }

    //相机跟随一个方向
    public void FollowDirection(Vector3 dir, float pitchDeg)
    {
        float targetAngle = GetDegree(dir);
        RotateTo(targetAngle, pitchDeg);
    }

    //停止yaw方向的插值
    public void StopYawLerp()
    {
        if (!_target)
            return;
        _prePos = _target.position;
        _targetYaw = _yawDegree;
    }

    #region 设置跟随目标的快捷方法
    //跟随目标forward , backward , left , right
    public void FollowTargetForward(float pitchDeg)
    {
        if (!_target)
            return;
        FollowDirection(_target.forward, pitchDeg);
    }
    //backward
    public void FollowTargetBackward(float pitchDeg)
    {
        if (!_target)
            return;
        FollowDirection(-_target.forward, pitchDeg);
    }
    //left
    public void FollowTargetLeft(float pitchDeg)
    {
        if (!_target)
            return;
        FollowDirection(-_target.right, pitchDeg);
    }
    //right
    public void FollowTargetRight(float pitchDeg)
    {
        if (!_target)
            return;
        FollowDirection(_target.right, pitchDeg);
    }
    #endregion

    //目标偏航角
    public float targetYaw
    {
        get { return _targetYaw; }
        set
        {
            if (!_lockYaw)
                _targetYaw = value;
        }
    }

    //目标俯仰角
    public float targetPitch
    {
        get { return _targetPitch; }
        set
        {
            if (!_lockPitch)
            {
                value = CheckPitch(value);
                _targetPitch = value;
                _targetPitch = Mathf.Min(_targetPitch, _maxPitch);
                _targetPitch = Mathf.Max(_targetPitch, _minPitch);
            }
        }
    }

    /// <summary>
    /// 强制就位相机
    /// </summary>
    public override void ForcePosition()
    {
        base.ForcePosition();
        if (_target)
        {
            _prePos = _target.position;
        }
        OnLateUpdate(1000, Vector3.zero);
    }

    /// <summary>
    /// 计算相机与目标碰撞之后的距离
    /// </summary>
    public float CalDisAfterCollision(Vector3 camPos, Vector3 tarPos, bool lockZoom)
    {
        float colliderDis = _maxFollowDistance;
        if (!lockZoom)
        {
            Vector3 raycastOrgin = tarPos + _targetOffset;
            Vector3 raycastDir = (camPos + _raycastOffset - raycastOrgin).normalized;
            _racasthitInfo = new RaycastHit();
            //检测到碰撞则拉近相机
            if (Physics.Raycast(raycastOrgin, raycastDir, out _racasthitInfo, _maxFollowDistance, _raycastLayer))
            {
                colliderDis = _racasthitInfo.distance;
            }
        }
        return colliderDis;
    }

    public override void OnLateUpdate(float deltaTime, Vector3 shakeOffset)
    {
        if (!_enable)
        {
            return;
        }
        //开始正式更新相机逻辑
        if (!Transform || !_target) return;
        //如果目标位置在更新则相机自动尾随目标
        bool isJoyPressed = joystick ? joystick.IsPressed : false;
        bool isTouchDrag = CameraTouchController.Instance ? CameraTouchController.Instance.isDrag : false;
        if (!isJoyPressed && !isTouchDrag)
        {
            Vector3 tf = target.forward;
            tf.y = 0;
            tf.Normalize();
            Vector3 cf = Transform.forward;
            cf.y = 0;
            cf.Normalize();
            float angle = Vector3.Angle(tf, cf);
            //目标朝向与相机朝向在阈值内才会启用尾随
            if (angle < 165)
            {
                //如果目标对象移动了则尾随目标
                float sDis = (_target.position - _prePos).sqrMagnitude;
                if (sDis > 0.0001f)
                {
                    //计算相机尾随的各个角度
                    float tarYawDeg = GetDegree(-_target.forward);
                    float deltaYaw = GetDeltaDegRotTo(_yawDegree, tarYawDeg) * Time.deltaTime * _followAutoRotYawSpeed;
                    //float deltaPitch = GetDeltaDegRotTo(_pitchDegree, _pitchDegree);
                    //targetYaw = _yawDegree + deltaYaw;
                    _targetYaw = _yawDegree + deltaYaw;
                    //targetPitch = _pitchDegree + deltaPitch;
                    //记录当前位置，用做下一帧的比较
                    _prePos = _target.position;
                }
            }
        }
        _currentTargetPosition = Vector3.Lerp(_currentTargetPosition, _target.position, deltaTime * _followSpeed);
        //碰撞检测
        float colliderDis = _defaultFollowDistance;
        if (!_lockZoom)
        {
            Vector3 raycastOrgin = _currentTargetPosition + _targetOffset;
            Vector3 raycastDir = (Transform.position + _raycastOffset - raycastOrgin).normalized;
            _racasthitInfo = new RaycastHit();
            //检测到碰撞则拉近相机
            if (Physics.Raycast(raycastOrgin, raycastDir, out _racasthitInfo, _defaultFollowDistance, _raycastLayer))
            {
                colliderDis = Math.Max(_racasthitInfo.distance, _minFollowistance);
            }
        }
        //偏航角,俯仰角，距离插值
        _distance = Mathf.Lerp(_distance, colliderDis, deltaTime * _zoomSpeed);
        _pitchDegree = Mathf.Lerp(_pitchDegree, _targetPitch, deltaTime * _rotateSpeed);
        _yawDegree = Mathf.Lerp(_yawDegree, _targetYaw, deltaTime * _rotateSpeed);
        //计算垂直的单位方向
        float y = Mathf.Sin(_pitchDegree * Mathf.Deg2Rad);
        //计算水平的单位方向
        float x = Mathf.Cos(_yawDegree * Mathf.Deg2Rad);
        float z = Mathf.Sin(_yawDegree * Mathf.Deg2Rad);
        //单位方向映射到世界
        float tempRadius = Mathf.Cos(_pitchDegree * Mathf.Deg2Rad) * _distance;
        Vector3 raltionPos = new Vector3(x * tempRadius, y * _distance, z * tempRadius);
        Transform.position = _currentTargetPosition + _targetOffset + raltionPos;
        //计算世界左边系中的相对偏移(相机震动)
        Vector3 wordOffset = Transform.localToWorldMatrix.MultiplyPoint(shakeOffset);
        wordOffset = wordOffset - Transform.position;
        //目标位置与相机当前位置同时加上偏移值
        Transform.position += wordOffset;
        Vector3 forward = _currentTargetPosition + _targetOffset + wordOffset - Transform.position;
        if (forward.sqrMagnitude >= 0)
        {
            Transform.forward = forward;
        }
    }
}
