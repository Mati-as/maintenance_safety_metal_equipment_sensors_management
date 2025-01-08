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
        BindButton(typeof(Btns));
        
        
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

}
