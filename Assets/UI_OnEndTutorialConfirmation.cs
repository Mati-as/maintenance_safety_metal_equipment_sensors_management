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
            Managers.isTutorialAlreadyPlayed = true;
            Managers.Scene.LoadScene(SceneType.DepthA);
        });


        GetButton((int)Btns.Btn_Confirmation_No).gameObject.BindEvent(() =>
        {
            Managers.isTutorialAlreadyPlayed = true;
            Managers.Scene.LoadScene(SceneType.Main);
        });
        
              
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });

        return true;
    }
}
