using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Persistent : UI_Popup
{
    private enum Btns
    {
        Btn_Logo_MenuActivation, // 산기대 대학교 로고
        Btn_Main,
        Btn_Help,
        Btn_FullMenu,
        Btn_Setting,
        Btn_Close
        
    }

    private Animator _activationAnimator;
    private int UI_ON = Animator.StringToHash("UI_On");
    private int UI_OFF = Animator.StringToHash("UI_Off");
   
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        
        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));

        _activationAnimator = GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.GetComponent<Animator>();
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.BindEvent(OnMouseEnterActivationBtn,Define.UIEvent.PointerEnter);
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.BindEvent(OnMouseExitActivationBtn,Define.UIEvent.PointerExit);
        return true;
    }

    private void OnMouseEnterActivationBtn()
    {
        
#if UNITY_EDITOR
        Debug.Log("Hover Animation Activating");
#endif
        _activationAnimator.SetBool(UI_ON,true);    
        _activationAnimator.SetBool(UI_OFF,false);    
    }
    private void OnMouseExitActivationBtn()
    {
#if UNITY_EDITOR
        Debug.Log("Deactivate PersistentUI");
#endif
        _activationAnimator.SetBool(UI_OFF,true);    
        _activationAnimator.SetBool(UI_ON,false);    
    }
    
    
}
