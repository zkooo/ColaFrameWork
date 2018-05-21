using UnityEngine;
using UnityEngine.UI;

public class UICopyingAssetHelper : UIBase
{
    /// <summary>
    /// 拷贝资源进度框比较特殊，路径是固定的，并且不参与uiMgr的管理，不走资源管理器
    /// </summary>
    private static string resPath = "Arts/Gui/Prefabs/uicopying";
    private static UICopyingAssetHelper instance = null;
    private static RawImage background;
    private static Text progressText;

    private UICopyingAssetHelper():base()
    {
        var uiGameobject =   Resources.Load<GameObject>(resPath);
        CreateWithGO(uiGameobject,UILevel.Top);
    }


    public UICopyingAssetHelper Instance()
    {
        if (null == instance)
        {
            instance = new UICopyingAssetHelper();
        }

        return instance;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        background = Panel.GetComponentByPath<RawImage>("bg");
        progressText = Panel.GetComponentByPath<Text>("progress_text");
    }

    public void UpdateUI(int progress,int totalCount,string HintText)
    {
        progressText.text = string.Format("{0}:({1}/{2})", HintText, progress, totalCount);
    }


    public override void Destroy()
    {
        uiCreateType = UICreateType.Res;
        base.Destroy();
    }
}
