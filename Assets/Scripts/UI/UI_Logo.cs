using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_Logo : UI_Popup
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        DOVirtual.DelayedCall(InitialScene._logoShowTimeDuration, ()=>
        {
            Managers.UI.ClosePopupUI(this);
        });
        return true;
    }

}
