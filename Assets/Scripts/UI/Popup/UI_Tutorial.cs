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
        Btn_Prev,
        Btn_Next,
        Btn_ToolBox,
        Btn_CameraInit,
       // Btn_Close
    }

    public enum TMPs
    {
        Text_Instruction
    }
    
    public enum UI
    {
        UI_Bottom,
        UI_ToolBox,
        UI_Confirmation,
        UI_ToolTip
        
    }

    private bool _isTutorialEndedOrSkipped;
   
    public bool isTutorialEndedOrSkipped
    {
        get { return _isTutorialEndedOrSkipped; }
        set { _isTutorialEndedOrSkipped = value; }
    }
    
    public override bool Init()
    {
        
        BindTMP(typeof(TMPs));
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
        
        return true;

    }




}
