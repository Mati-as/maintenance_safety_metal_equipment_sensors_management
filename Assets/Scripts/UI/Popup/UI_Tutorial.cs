using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_Popup
{
    public enum Btns
    {   
        
        Btn_Confirmation_Yes,
        Btn_Confirmation_No,
        Btn_Close
    }
    
    
    public enum UI
    {
        UI_Confirmation,
        
    }

    private bool _isTutorialEndedOrSkipped;
   
    public bool isTutorialEndedOrSkipped
    {
        get { return _isTutorialEndedOrSkipped; }
        set { _isTutorialEndedOrSkipped = value; }
    }
    
    public override bool Init()
    {
        
        BindButton(typeof(Btns));
        BindObject(typeof(UI));
      
        
        GetButton((int)Btns.Btn_Confirmation_Yes).gameObject.BindEvent(() =>
        {
            Managers.Scene.LoadScene(SceneType.Tutorial);
            isTutorialEndedOrSkipped = true;
        });


        GetButton((int)Btns.Btn_Confirmation_No).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
            Managers.UI.ShowPopupUI<UI_DepthSelection>();
            GetObject((int)UI.UI_Confirmation).gameObject.SetActive(false);
            isTutorialEndedOrSkipped = true;
        });
        
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
            Managers.UI.ShowPopupUI<UI_Main>();
        });

        return true;

    }




}
