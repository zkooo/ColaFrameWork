using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机触摸控制脚本
/// </summary>
public class CameraTouchController : MonoBehaviour
{
    public readonly float SPEED_SCALE = 5.0f;
    public readonly float DEGREE_PER_UNIT = 400.0f;
    public readonly float X_SCALE = 1.0f;
    public readonly float Y_SCALE = 0.25f;

    private Vector2 preMousePos = new Vector2(0, 0);
    private bool isMouseDrag = false;
    private bool isTouchDrag = false;

    private HashSet<int> validFingers = new HashSet<int>();
    private HashSet<int> cache = new HashSet<int>();
    private Dictionary<int, Touch> fingerToTouches = new Dictionary<int, Touch>();
    private float lastZoomDistance = 0f;
    private CameraTouchController instance;

    public CameraTouchController Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// 位置归一化
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    public static Vector2 MouseToNormal(Vector2 mousePos)
    {
        mousePos.Set(mousePos.x / Screen.width, mousePos.y / Screen.height);
        return mousePos;
    }

    public bool isDrag
    {
        get { return isMouseDrag || isTouchDrag; }
    }

    void Awake()
    {
        instance = this;
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
