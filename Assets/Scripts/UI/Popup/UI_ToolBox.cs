using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class UI_ToolBox : UI_Popup
{
    private readonly int UI_ON = Animator.StringToHash("On");
    public enum Btns
    {
        Btn_Close,
        Btn_ElectricScrewdriver
    }

    public enum Obj
    {
        ElectricScrewdriver
    }


    private Animator _animator; 
    private Depth1C_SceneController _sceneController;
    public override bool Init()
    {
        if (!base.Init())
            return false;

        _sceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Depth1C_SceneController>();
 
        BindButton(typeof(Btns));
        BindObject(typeof(Obj));
        
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
        
        
        GetButton((int)Btns.Btn_ElectricScrewdriver).gameObject.BindEvent(() =>
        {
            _sceneController.OnScrewDriverBtnClicked();
        });
        return true;
    }

    public void SetToolBox(bool show)
    {
        _animator.SetBool(UI_ON,show);
    }
    
 


}
