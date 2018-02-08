using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectUIBlur : MonoBehaviour
{
    /// <summary>
    /// 渲染效果所用到的Shader
    /// </summary>
    private Shader effectShader;

    public bool EnableUIBlur = false;

    private bool isOpen = false;

    /// <summary>
    /// 最终渲染出来的临时Texture效果图
    /// </summary>
    private RenderTexture tempRtLowB = null;

    /// <summary>
    /// 主相机
    /// </summary>
    private Camera mainCamera;

    //最终渲染出来的临时Texture效果图
    public RenderTexture TempRtLowB
    {
        get
        {
            bool state = false;
            if (false == EnableUIBlur)
            {

            }
            EnableUIBlur = true;
            isOpen = true;
            state = true;
            return tempRtLowB;
        }
        set
        {
            if (null == value)
            {
                mainCamera.enabled = true;
                EnableUIBlur = false;
                isOpen = false;
                //RenderTexture.ReleaseTemporary(tempRtLowB);
                //tempRtLowB = null;
            }
        }
    }

    /// <summary>
    /// 渲染效果所用到的Shader
    /// </summary>
    public Shader EffecShader
    {
        get
        {
            if (null == effectShader)
            {
                effectShader = Shader.Find("ColaFrameWork/MobileUIBlur");
            }
            return effectShader;
        }
    }

    void Awake()
    {
        mainCamera = GUIHelper.GetMainCamera();
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
