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

    public enum TMPs
    {
        TMP_ConfirmationHead,
        TMP_ConfirmationBody,
        TMP_Yes,
        TMP_No,
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
        BindTMP(typeof(TMPs));

        RefreshText();
        
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


    public override void RefreshText()
    {
        GetTMP((int)TMPs.TMP_ConfirmationHead).text = Managers.Data.IsEngMode() ? "Operation Guide" : "조작법 안내";
        GetTMP((int)TMPs.TMP_ConfirmationBody).text = Managers.Data.IsEngMode() ? "Would you like to start the operation guide?" : "조작법 안내를 시작 하시겠습니까?";
        GetTMP((int)TMPs.TMP_Yes).text = Managers.Data.IsEngMode() ? "YES" : "네";
        GetTMP((int)TMPs.TMP_No).text = Managers.Data.IsEngMode() ? "NO" : "아니오";
    }

}
