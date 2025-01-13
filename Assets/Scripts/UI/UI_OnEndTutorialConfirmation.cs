using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnEndTutorialConfirmation : UI_Popup
{
    public enum Btns
    {   
        
        Btn_Confirmation_Yes,
        Btn_Confirmation_No,
        Btn_Close,
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

        GetButton((int)Btns.Btn_Confirmation_Yes).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
         //   Managers.isTutorialAlreadyPlayed = true;
            Managers.Scene.LoadScene(SceneType.DepthA);
        });


        GetButton((int)Btns.Btn_Confirmation_No).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
          //  Managers.isTutorialAlreadyPlayed = true;
            Managers.Scene.LoadScene(SceneType.Main);
        });
        
              
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
        RefreshText();
        return true;
    }
    
    public override void RefreshText()
    {
        GetTMP((int)TMPs.TMP_ConfirmationHead).text = Managers.Data.IsEngMode() ? "Notification" : "알림";
        GetTMP((int)TMPs.TMP_ConfirmationBody).text = Managers.Data.IsEngMode() ? "Would you like to move on to the next lesson?\n<color=orange>[Basic Training- Checking Metal Manufacturing Equipment]</color>" : "다음 학습으로 이동하시겠습니까?\n<color=orange>[기본학습 - 금속제조설비 구성확인]</color>";
        GetTMP((int)TMPs.TMP_Yes).text = Managers.Data.IsEngMode() ? "YES" : "네";
        GetTMP((int)TMPs.TMP_No).text = Managers.Data.IsEngMode() ? "NO" : "아니오";
    }

}
