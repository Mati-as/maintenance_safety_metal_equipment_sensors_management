using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Exit : UI_Popup
{
    public enum Btns
    {   
        
        Btn_Confirmation_Yes,
        Btn_Confirmation_No,
         Btn_Close
    }

    public enum TMPs
    {
        TMP_ConfirmationHead,
        TMP_ConfirmationBody,
        TMP_Yes,
        TMP_No,
    }
    
    public enum UI
    {
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
        BindTMP(typeof(TMPs));
        
        RefreshText();
        
        GetButton((int)Btns.Btn_Confirmation_Yes).gameObject.BindEvent(() =>
        {
            Application.Quit();
        });


        GetButton((int)Btns.Btn_Confirmation_No).gameObject.BindEvent(() =>
        {
           Managers.UI.ClosePopupUI(this);
        });
        
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
       
        return true;

    }

    public override void RefreshText()
    {
        GetTMP((int)TMPs.TMP_ConfirmationHead).text = Managers.Data.IsEngMode() ? "Quit" : "컨텐츠 종료";
        GetTMP((int)TMPs.TMP_ConfirmationBody).text = Managers.Data.IsEngMode() ? "Would you like to quit the content?" : "컨텐츠를 종료하시겠습니까?";
        GetTMP((int)TMPs.TMP_Yes).text = Managers.Data.IsEngMode() ? "YES" : "네";
        GetTMP((int)TMPs.TMP_No).text = Managers.Data.IsEngMode() ? "NO" : "아니오";
    }

}
