using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHintTest : UIBase
{
    public UIHintTest(int resId, UILevel uiLevel) : base(resId, uiLevel)
    {
    }

    public UIHintTest(GameObject panel, GameObject parent, UILevel uiLevel) : base(panel, parent, uiLevel)
    {
    }

    protected override void onClick(GameObject obj)
    {
        if (obj.name == "bg")
        {
            Debug.LogWarning("点击了bg按钮");
        }
    }
}
