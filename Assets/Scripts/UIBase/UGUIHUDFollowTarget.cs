using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUIHUDFollowTarget : MonoBehaviour {

    /// <summary>
    /// 3D target that this object will be positioned above.
    /// </summary>

    public Transform target;

    /// <summary>
    /// Game camera to use.
    /// </summary>

    public static Camera gameCamera;

    /// <summary>
    /// UI camera to use.
    /// </summary>

    public static Camera uiCamera;

    Canvas canvas;

    /// <summary>
    /// Whether the children will be disabled when this object is no longer visible.
    /// </summary>

    public bool disableIfInvisible = true;

    public bool enableAll = true;
    public static float cullDistance = 0f;

    public Vector3 offset = Vector3.zero;

    Transform mTrans;
    bool mIsVisible = false;

    static int frameCount = 0;
    static int maxCountActivateInOnFrame = 2;
    static int currentCountActivateInOnFrame = 0;

    /// <summary>
    /// Cache the transform;
    /// </summary>

    void Awake()
    {
        mTrans = transform;

        canvas = GUIHelper.GetUIRoot();

    }

    /// <summary>
    /// Find both the UI camera and the game camera so they can be used for the position calculations
    /// </summary>

    void Start()
    {
        if (target != null)
        {
            SetVisible(false);
        }
        else
        {
            Debug.LogError("Expected to have 'target' set to a valid transform", this);
            enabled = false;
        }
    }

    /// <summary>
    /// Enable or disable child objects.
    /// </summary>

    public void SetVisible(bool val)
    {
        mIsVisible = val;

        for (int i = 0, imax = mTrans.childCount; i < imax; ++i)
        {
            mTrans.GetChild(i).gameObject.SetActive(val);
        }
    }

    /// <summary>
    /// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
    /// </summary>

    void LateUpdate()
    {
        if (target == null)
        {
            if( this.gameObject.activeSelf )
            {
                this.gameObject.SetActive(false);
            }
            return;
        }
            

        Vector3 tpos = target.position;
        tpos += offset;
        //float x = Mathf.Floor(tpos.x * 1000) * 0.001f;
        //float y = Mathf.Floor(tpos.y * 1000) * 0.001f;
        //float z = Mathf.Floor(tpos.z * 1000) * 0.001f;
        //Vector3 tempPos = new Vector3(x, y, z);

        if(gameCamera == null)
        {
            return;
        }

        Vector3 pos = gameCamera.WorldToViewportPoint(tpos);

        // Determine the visibility and the target alpha
        bool isVisible = (gameCamera.orthographic || pos.z > 0) && (!disableIfInvisible || (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f));
        isVisible = enableAll && isVisible;

        if (isVisible && cullDistance > 0f)
        {
            Vector3 dist = gameCamera.transform.position - target.position;
            if (dist.sqrMagnitude > cullDistance * cullDistance)
                isVisible = false;
        }

        // Update the visibility flag
        if (mIsVisible != isVisible && isVisible == false) SetVisible(isVisible);

        if (isVisible)
        {
            if (frameCount != Time.frameCount)
            {
                frameCount = Time.frameCount;
                currentCountActivateInOnFrame = 0;
            }

            if (!mIsVisible && ++currentCountActivateInOnFrame <= maxCountActivateInOnFrame)
            {
                SetVisible(true);
            }

            if (mIsVisible)
            {
                var viewPoint = gameCamera.WorldToViewportPoint(tpos);
                var uiWordPoint = uiCamera.ViewportToWorldPoint(viewPoint);
                uiWordPoint.Set(uiWordPoint.x, uiWordPoint.y, 0);
                mTrans.transform.position = uiWordPoint;
                //(mTrans as RectTransform).anchoredPosition = GUITools.WorldToUIPoint(gameCamera, canvas, tpos);
                mTrans.localScale = Vector3.one;
            }
        }


        OnUpdate(isVisible);
    }

    /// <summary>
    /// Custom update function.
    /// </summary>

    protected virtual void OnUpdate(bool isVisible) { }
}
