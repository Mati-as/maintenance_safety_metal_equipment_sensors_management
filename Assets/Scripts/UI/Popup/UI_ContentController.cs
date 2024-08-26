using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UI_ContentController : UI_Popup
{

    public enum UI
    {
        Depth1
    }
    public enum Toggles
    {
        Toggle_Depth2_A,
        Toggle_Depth2_B,
        Toggle_Depth2_C
    }
    public enum Btns
    {
        Btn_Prev,
       Btn_Next,
       Btn_Depth1_Title,
       Btn_TopMenu_Hide,
       Btn_Script_Hide,
       Btn_ThirdDepth_Hide,
       Btn_GuideBook,
       Btn_Evaluation
    }


    private Animator _DepthOnetextAnimator;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


     
        BindButton(typeof(Btns));
        BindToggle(typeof(Toggles));

        GetToggle((int)Toggles.Toggle_Depth2_A).gameObject.BindEvent(()=>{OnToggleClicked(Toggles.Toggle_Depth2_A);}, Define.UIEvent.PointerUp);
        GetToggle((int)Toggles.Toggle_Depth2_B).gameObject.BindEvent(()=>{OnToggleClicked(Toggles.Toggle_Depth2_B);}, Define.UIEvent.PointerUp);
        GetToggle((int)Toggles.Toggle_Depth2_C).gameObject.BindEvent(()=>{OnToggleClicked(Toggles.Toggle_Depth2_C);}, Define.UIEvent.PointerUp);
      
        GetButton((int)Btns.Btn_Depth1_Title).gameObject.BindEvent(OnDepthOneTitleHover,Define.UIEvent.PointerEnter);
        GetButton((int)Btns.Btn_Depth1_Title).gameObject.BindEvent(OnDepthOneTitleHoverExit,Define.UIEvent.PointerExit);
      
        _DepthOnetextAnimator = GetButton((int)Btns.Btn_Depth1_Title).gameObject.GetComponent<Animator>();
        _DepthOnetextAnimator.enabled = false;
        
        return true;
    }

    private void OnToggleClicked(Toggles currentToggle)
    {
        
    }

    private void OnDepthOneTitleHover()
    {
        _DepthOnetextAnimator.enabled = true;
    }
    private void OnDepthOneTitleHoverExit()
    {
        _DepthOnetextAnimator.Rebind(); 
        _DepthOnetextAnimator.enabled = false;
    }
}
