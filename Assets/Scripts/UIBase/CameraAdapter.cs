using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI相机适配器,多出屏幕部分给黑边
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraAdapter : MonoBehaviour
{
    public float Aspect
    {
        get
        {
            return aspect;
        }
        set
        {
            if (value > 0f)
            {
                aspect = value;
            }
            RefreshCameraRect();
        }
    }

    private float aspect = 16 / (float)9;

    private bool defaultExcute = false;

    private int m_defaultMask = 0;
    private Color m_defaultColor;
    public bool NeedUpdateRect = false;
    public Camera m_camera;
    public bool bIsMainCamera = false;
    [Header("勾选此选项宽度不加黑边")]
    public bool WidthAdapter = true;

    void Awake()
    {
        Debug.Log("******************1" + gameObject.name);
        defaultExcute = false;
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("******************2" + gameObject.name);
        SetRefreshCameraRect();
    }

    private void OnPostRender()
    {
        //if (NeedUpdateRect && !bIsMainCamera)
        //{
        //    UpdateRect();
        //    NeedUpdateRect = false;
        //}
    }

    void OnApplicationFocus(bool hasFocus)
    {
        //if (hasFocus)
        //{
        //    RefreshCameraRect();
        //}
    }

    private void SetRefreshCameraRect()
    {
        //if (!defaultExcute)
        //{
        //    RefreshCameraRect();
        //    defaultExcute = true;
        //}
    }

    public void RefreshCameraRect()
    {
        //if (m_camera == null)
        //{
        //    m_camera = GetComponent<Camera>();
        //}
        //float aspectNow = Screen.width / (float)Screen.height;
        //if (aspect > 0f && Mathf.Abs(aspectNow - aspect)>0.01)
        //{
        //    m_defaultMask = m_camera.cullingMask;
        //    m_camera.cullingMask = LayerMask.GetMask("Nothing");
        //    if (!defaultExcute)
        //    {
        //        m_defaultColor = m_camera.backgroundColor;
        //    }
        //    m_camera.backgroundColor = new Color(0, 0, 0, 1);
        //    if (gameObject.activeInHierarchy)
        //    {
        //        NeedUpdateRect = true;
        //        m_camera.rect = new Rect(0, 0, 1, 1);
        //        m_camera.Render();
        //        //m_camera.RenderDontRestore();
        //    }

        //}
        //else
        //{
        //    m_camera.rect = new Rect(0, 0, 1, 1);
        //}
    }


    public void UpdateRect()
    {
        //int defaultScreenWith = Screen.width;
        //int defaultScreenHeight = Screen.height;
        //float aspectNow = defaultScreenWith / (float)defaultScreenHeight;
        //float targetH = 1f;
        //float targetV = 1f;
        //if (aspectNow > aspect)
        //{
        //    if (!WidthAdapter)
        //    {
        //        targetV = (defaultScreenHeight * aspect) / defaultScreenWith;
        //    }

        //}
        //else
        //{
        //    targetH = defaultScreenWith / (defaultScreenHeight * aspect);
        //}

        //m_camera.backgroundColor = m_defaultColor;
        //m_camera.cullingMask = m_defaultMask;
        //if (targetH < 1f || targetV < 1f)//上下左右都切黑边
        ////if (targetH < 1f)//只有上下切黑边，去掉左右切黑边
        //{
        //    Rect rect = new Rect((1f - targetV) / 2f, (1f - targetH) / 2f, targetV, targetH);
        //    m_camera.rect = rect;
        //}
        //else
        //{
        //    m_camera.pixelRect = new Rect(m_camera.pixelRect.x,
        //    m_camera.pixelRect.y, Screen.width, Screen.height);
        //    m_camera.rect = new Rect(0, 0, 1, 1);
        //}
    }
}
