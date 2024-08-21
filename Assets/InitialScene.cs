using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Dev;
        Managers.UI.ShowPopupUI<UI_Setting>();
        Debug.Log("Init");
        return true;
    }
}
