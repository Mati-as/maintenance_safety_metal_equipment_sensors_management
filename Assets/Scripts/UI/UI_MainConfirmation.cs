using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainConfirmation : UI_Popup
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
            Managers.UI.CloseAllPopupUI();
            Managers.Scene.LoadScene(SceneType.Main);
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
        GetTMP((int)TMPs.TMP_ConfirmationHead).text = Managers.Data.IsEngMode() ? "Return to Main Screen" : "메인화면 이동";
        GetTMP((int)TMPs.TMP_ConfirmationBody).text = Managers.Data.IsEngMode() ? "Would you like to return to the main screen?" : "메인화면으로 이동하시겠습니까?";
        GetTMP((int)TMPs.TMP_Yes).text = Managers.Data.IsEngMode() ? "Yes" : "네";
        GetTMP((int)TMPs.TMP_No).text = Managers.Data.IsEngMode() ? "No" : "아니오";
    }
}
