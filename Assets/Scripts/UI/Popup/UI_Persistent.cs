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
        
        //DontDestroyOnLoad(gameObject);
        
        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));

        _activationAnimator = GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.GetComponent<Animator>();
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.BindEvent(OnMouseEnterActivationBtn,Define.UIEvent.PointerEnter);
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.BindEvent(OnMouseExitActivationBtn,Define.UIEvent.PointerExit);
        
        GetButton((int)Btns.Btn_Main).gameObject.BindEvent(OnMainBtnClicked,Define.UIEvent.PointerUp);
        GetButton((int)Btns.Btn_Help).gameObject.BindEvent(OnHelpBtnClicked,Define.UIEvent.PointerUp);
        GetButton((int)Btns.Btn_FullMenu).gameObject.BindEvent(OnFullMenuBtnClicked,Define.UIEvent.PointerUp);
        GetButton((int)Btns.Btn_Setting).gameObject.BindEvent(OnSettingBtnClicked,Define.UIEvent.PointerUp);
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(OnloseBtnClicked,Define.UIEvent.PointerUp);
        return true;
    }


    #region Mouse Hover Logic 

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
        DeactivatePersistentUI();
    }



    #endregion


    #region Btn Methods

    private void OnMainBtnClicked()
    {

        if (Managers.UI.FindPopup<UI_Main>() == null)
        {
            Managers.UI.ShowPopupUI<UI_Main>();
        }
    }

    private void OnHelpBtnClicked()
    {
        Managers.UI.ShowPopupUI<UI_Help>();
    }

    private void OnFullMenuBtnClicked()
    {
        Managers.UI.ShowPopupUI<UI_FullMenu>();
    }

    private void OnSettingBtnClicked()
    {
        Managers.UI.ShowPopupUI<UI_Setting>();
    }


    private void OnloseBtnClicked()
    {
        DeactivatePersistentUI();
    }

    #endregion
    
    
    
    
    

    private void DeactivatePersistentUI()
    {
#if UNITY_EDITOR
        Debug.Log("Deactivate PersistentUI");
#endif
        _activationAnimator.SetBool(UI_OFF,true);    
        _activationAnimator.SetBool(UI_ON,false);    
    }
    
    
}
