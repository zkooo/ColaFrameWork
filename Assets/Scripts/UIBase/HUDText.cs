using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UGUI版的HUDText
/// </summary>
public class HUDText : MonoBehaviour {



    List<Entry> mList = new List<Entry>();
    List<Entry> mUnused = new List<Entry>();

    int counter = 0;

    public bool isVisible { get { return mList.Count != 0; } }

    private int bmfontSize = 32;

    private static Dictionary<string, Font> FontsMap = new Dictionary<string, Font>();
    //private static GameObject FontsRoot = null;
    private static Dictionary<string, Sprite> effectsMap = new Dictionary<string, Sprite>();


    public static void AddEffectSpriteObj(string spriteName, GameObject resObj)
    {
        if (effectsMap.ContainsKey(spriteName))
        {
            return;
        }
        if (resObj == null)
        {
            return;
        }

        SpriteAsset sa = resObj.GetComponent<SpriteAsset>();
        if (sa == null)
        {
            return;
        }

        Sprite sprite = sa.GetSpriteByName(spriteName);
        if (sprite == null)
            Debug.LogWarning("The sprite asset " + spriteName + " not in Atlas");
        else
        {
            effectsMap.Add(spriteName, sprite);
        }
    }

    public static void CreateUGUIFontObj(string fontName, GameObject prefabObj)
    {
        if (FontsMap.ContainsKey(fontName))
        {
            return;
        }

        if (prefabObj == null)
        {
            return;
        }

        //CustomFont cf = prefabObj.GetComponent<CustomFont>();
        //if (cf == null)
        //{
        //    return;
        //}
        //Font font = cf.font;
        //if (font == null)
        //    Debug.LogWarning("The font asset " + fontName + " is not font asset");
        //else
        //{
        //    FontsMap.Add(fontName, font);
        //}
    }

    public static void AddEffectSprite(string spriteName, string path)
    {
        Action<UnityEngine.Object> callback = (asset) =>
        {
            if (asset != null)
            {
                GameObject go = asset as GameObject;
                SpriteAsset sa = go.GetComponent<SpriteAsset>();
                Sprite sprite = sa.GetSpriteByName(spriteName);
                if (sprite == null)
                    Debug.LogWarning("The sprite asset " + spriteName + " not in Atlas");
                else
                {
                    if (!effectsMap.ContainsKey(spriteName))
                        effectsMap.Add(spriteName, sprite);
                }
            }
            else
            {
                Debug.LogWarning("Failed to load Font " + spriteName);
            }
        };
        //AssetUtility.ResourceManager.AsyncLoadResourceAndCleanUp(path, false, false, callback);
    }

    public static void CreateUGUIFont(string fontName, string prefabPath)
    {
        {
            //Action<UnityEngine.Object> callback = (asset) =>
            //{
            //    if (asset != null)
            //    {
            //        GameObject go = asset as GameObject;
            //        CustomFont cf = go.GetComponent<CustomFont>();
            //        Font font = cf.font;
            //        if (font == null)
            //            Debug.LogWarning("The font asset " + prefabPath + " is not font asset");
            //        else
            //        {
            //            if (!FontsMap.ContainsKey(fontName))
            //                FontsMap.Add(fontName, font);
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Failed to load Font " + fontName);
            //    }
            //};
            //AssetUtility.ResourceManager.AsyncLoadResourceAndCleanUp(prefabPath, false, false, callback);
        }

    }

    Entry Create(string fontName, int fontSize, float lifeTime, Vector3 velocity, bool useGravity, string effectName, bool isCrit, Vector3 offset, AnimationCurve alphaCurve, AnimationCurve scaleCurve)
    {
        if (mUnused.Count > 0)
        {
            Entry ent = mUnused[mUnused.Count - 1];
            mUnused.RemoveAt(mUnused.Count - 1);

            UpdateUI(ent, fontName, fontSize, lifeTime, effectName, isCrit, velocity, useGravity, offset);

            if (alphaCurve != null)
                ent.alphaCurve = alphaCurve;
            if (scaleCurve != null)
                ent.scaleCurve = scaleCurve;
            mList.Add(ent);
            return ent;
        }

        Entry ne = new Entry();
        //ne.root = GameObject.Instantiate(UGUIPateTextComponent.hudTextPrefab) as GameObject;
        ne.root.name = "HUD";
        GameObject label = ne.root.transform.Find("canvasgroup/textroot/text").gameObject;
        ne.label = label.GetComponent<Text>();

        ne.effectImage = ne.root.transform.Find("canvasgroup/effect").GetComponent<Image>();
        ne.critImage = ne.root.transform.Find("canvasgroup/crit").GetComponent<Image>();

        if (ne.animRoot == null)
        {
            ne.animRoot = ne.root.transform.Find("canvasgroup").gameObject;
            ne.canvasGroup = ne.animRoot.GetComponent<CanvasGroup>();
            ne.animTransform = ne.animRoot.GetComponent<RectTransform>();
        }

        UpdateUI(ne, fontName, fontSize, lifeTime, effectName, isCrit, velocity, useGravity, offset);
        mList.Add(ne);
        ++counter;
        return ne;
    }

    void UpdateUI(Entry ne, string fontName, int fontSize, float lifeTime, string effectName, bool isCrit, Vector3 velocity, bool useGravity, Vector3 offset)
    {
        ne.bornTime = Time.realtimeSinceStartup;
        ne.label.fontSize = fontSize;
        ne.liftTime = lifeTime;
        ne.effectName = effectName;
        ne.isCrit = isCrit;
        ne.moveSpeed = velocity;
        ne.useGravity = useGravity;
        ne.offset = Vector3.zero;

        ne.root.transform.SetParent(transform, false);
        ne.root.transform.localPosition = offset;

        ne.animRoot.SetActive(true);

        transform.localScale = Vector3.one;

        //修改动画时间
        for (int i = 0; i < ne.alphaCurve.keys.Length; i++)
        {
            Keyframe key = new Keyframe(lifeTime * ne.alphaTemplateCurve.keys[i].time, ne.alphaTemplateCurve.keys[i].value);
            ne.alphaCurve.MoveKey(i, key);
            //ne.alphaCurve.keys[i].time = lifeTime * ne.alphaCurve.keys[i].time;
        }

        for (int i = 0; i < ne.scaleCurve.keys.Length; i++)
        {
            Keyframe key = new Keyframe(lifeTime * ne.scaleTemplateCurve.keys[i].time, ne.scaleTemplateCurve.keys[i].value);
            ne.scaleCurve.MoveKey(i, key);
            //ne.scaleCurve.keys[i].time = lifeTime * ne.scaleCurve.keys[i].time;
        }

        //for (int i = 0; i < ne.moveCurve.keys.Length; i++)
        //{
        //    ne.moveCurve.keys[i].time = lifeTime * ne.moveCurve.keys[i].time;
        //}

        //根据参数里的字体大小决定缩放最大值
        float scaleMax = 1.0f * ne.label.fontSize / bmfontSize;
        Keyframe frame = ne.scaleCurve.keys[1];
        frame.value = scaleMax;

        ne.root.name = counter.ToString();
        Sprite sprite;
        if (isCrit)
        {
            ne.effectImage.gameObject.SetActive(false);
            ne.critImage.gameObject.SetActive(true);
            if (effectsMap.TryGetValue(effectName, out sprite))
            {
                ne.critImage.overrideSprite = sprite;
            }
            ne.critImage.SetNativeSize();
            //ne.label.transform.localRotation = Quaternion.Euler(0, 0, 8.38f);
        }
        else
        {
            ne.critImage.gameObject.SetActive(false);
            if (effectsMap.TryGetValue(effectName, out sprite))
            {
                ne.effectImage.gameObject.SetActive(true);
                ne.effectImage.overrideSprite = sprite;
                ne.effectImage.SetNativeSize();
                //ne.label.transform.localRotation = Quaternion.Euler(0, 0, 8.38f);
            }
            else
            {
                ne.effectImage.gameObject.SetActive(false);
                //ne.label.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

        }

        if (ne.fontName != fontName)
        {
            Font font;
            FontsMap.TryGetValue(fontName, out font);
            if (font != null)
            {
                ne.label.font = font;
            }
        }
        ne.canvasGroup.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        //Font nf;
        //FontsMap.TryGetValue(fontName, out nf);
        //if (nf != null)
        //    ne.label.font = nf;


    }

    void Delete(Entry ent)
    {
        mList.Remove(ent);
        mUnused.Add(ent);
        ent.animRoot.SetActive(false);
    }

    public void Clear()
    {
        foreach (Entry ent in mList)
        {
            mUnused.Add(ent);
            if (ent.animRoot)
                ent.animRoot.SetActive(false);
        }

        mList.Clear();
    }

    public void Add(string text, string fontName, int fontSize, float lifeTime, Vector3 velocity, bool useGravity, string effectName, bool isCrit, Vector3 offset)
    {
        if (!enabled) return;

        Entry ne = Create(fontName, fontSize, lifeTime, velocity, useGravity, effectName, isCrit, offset, null, null);
        ne.cb = null;
        ne.canvasGroup.alpha = 0;
        ne.label.text = text;

        //Debug.Log("add2 " + text);
    }

    void OnDisable()
    {
        for (int i = mList.Count; i > 0;)
        {
            Entry ent = mList[--i];
            if (ent.animRoot != null)
            {
                ent.animRoot.SetActive(false);
            }
            else
            {
                mList.RemoveAt(i);
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        float time = Time.unscaledTime;

        for (int i = mList.Count; i > 0;)
        {
            Entry ent = mList[--i];

            float totalEnd = ent.liftTime;

            float currentTime = time - ent.bornTime;
            float x = ent.moveSpeed.x * currentTime;
            float y = ent.moveSpeed.y * currentTime;

            if (ent.useGravity)
            {
                y = y - ent.moveSpeed.y * currentTime * currentTime * 0.7f;
                // y = ent.moveCurve.Evaluate(currentTime);
            }
            ent.offset = new Vector3(x, y, 0);

            //Debug.Log(ent.offset.ToString() + " " + ent.moveSpeed.ToString() + " " + currentTime);

            float a = ent.alphaCurve.Evaluate(currentTime);
            ent.canvasGroup.alpha = a;

            float scaleMax = 1.0f * ent.label.fontSize / bmfontSize;
            float s = ent.scaleCurve.Evaluate(currentTime);
            if (s < 0.001f) s = 0.001f;
            if (s < scaleMax)
                ent.canvasGroup.transform.localScale = new Vector3(s, s, s);

            if (currentTime > totalEnd)
            {
                if (ent.cb != null) ent.cb(ent.label.rectTransform.localPosition);
                Delete(ent);
            }
            else
            {
                ent.animRoot.SetActive(true);
            }
        }

        float offsetY = 0f;

        for (int i = mList.Count; i > 0;)
        {
            Entry ent = mList[--i];
            if (ent.canOverlapped)
                ent.canvasGroup.transform.localPosition = ent.offset;
            else
            {
                ent.animTransform.anchoredPosition = new Vector2(ent.offset.x, offsetY + ent.offset.y); //-offsetY
                //offsetY += Mathf.Round(ent.label.cachedTransform.localScale.y * ent.label.fontSize);
                float y = ent.animRoot.transform.localScale.y;
                float fsize = ent.label.fontSize;
                offsetY += Mathf.Round(y * fsize);
            }

        }
    }

    static string GetString(string s)
    {
        int idx = s.IndexOf('=');
        return (idx == -1) ? "" : s.Substring(idx + 1);
    }

    static int GetInt(string s)
    {
        int val = 0;
        string text = GetString(s);
        int.TryParse(text, out val);
        return val;
    }
}

public delegate void OnEntryDisappear2(Vector3 pos);

class Entry
{
    public float bornTime;
    public Vector3 offset;

    public Image effectImage;
    public Image critImage;
    public Text label;

    public GameObject root = null;
    public GameObject animRoot = null;
    public CanvasGroup canvasGroup = null;
    public RectTransform animTransform = null;

    public string fontName;
    public int fontSize = 20;

    public float liftTime = 0;
    public Vector3 moveSpeed;
    public bool useGravity = false;
    public bool canOverlapped = true;

    public string effectName = "";
    public bool isCrit = false;

    public AnimationCurve alphaTemplateCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.3f, 1f), new Keyframe(0.31f, 1f), new Keyframe(0.7f, 0f) });
    public AnimationCurve scaleTemplateCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.2f), new Keyframe(0.1f, 2.5f), new Keyframe(0.15f, 1f), new Keyframe(1f, 0.5f) });

    public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.3f, 1f), new Keyframe(0.31f, 1f), new Keyframe(0.7f, 0f) });
    public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0.2f), new Keyframe(0.1f, 2.5f), new Keyframe(0.15f, 1f), new Keyframe(1f, 0.5f) });
    //public AnimationCurve moveCurve = new AnimationCurve(new Keyframe[]                          { new Keyframe(0.25f, 100f), new Keyframe(0.35f, 50f), new Keyframe(1f, 30f) });

    public OnEntryDisappear2 cb = null;
}