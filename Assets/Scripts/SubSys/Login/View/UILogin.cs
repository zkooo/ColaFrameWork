using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : UIBase
{
    protected UILogin(int resId, UIType uiType) : base(resId, uiType)
    {
    }

    protected UILogin(GameObject panel, GameObject parent, UIType uiType) : base(panel, parent, uiType)
    {
    }
}
