using System;

using UnityEngine;

public class UI_ToolBox : UI_Popup
{
    private readonly int UI_ON = Animator.StringToHash("On");
    public enum Btns
    {
        Btn_Close,
        Btn_ElectricScrewdriver,
        Btn_Multimeter,
        Btn_TemperatureSensor
    }

    public enum Obj
    {
        ElectricScrewdriver
    }

    private bool _isToolBoxOn=false;
    private Animator _animator; 
    private Depth1C_SceneController _depthCsceneController;
    
    public static event Action ToolBoxOnEvent; // 도구함을 클릭해주세요 완수 시  
    public static event Action ScrewDriverClickedEvent; // 도구함을 클릭해주세요 완수 시  
    public static event Action MultimeterClickedEvent; // 도구함을 클릭해주세요 완수 시  \
    
    public static event Action TemperatureSensorClickedEvent; // 센서 교체 클릭해주세요 완수 시  
    public override bool Init()
    {
        if (!base.Init())
            return false;

        _depthCsceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Depth1C_SceneController>();
        _animator = GetComponent<Animator>();
        BindButton(typeof(Btns));
       // BindObject(typeof(Obj));
        
   
             
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            
            ToolBoxOnEvent?.Invoke();
            SetToolBox(false);
            //Managers.UI.ClosePopupUI(this);
        });

        
        
        GetButton((int)Btns.Btn_ElectricScrewdriver).gameObject.BindEvent(() =>
        {
            //_depthCsceneController.OnScrewDriverBtnClicked();
            ScrewDriverClickedEvent?.Invoke();
            SetToolBox(false);
        });
        
        GetButton((int)Btns.Btn_Multimeter).gameObject.BindEvent(() =>
        {
            //_depthCsceneController.OnMultimeterBtnClicked();
            MultimeterClickedEvent?.Invoke();
            SetToolBox(false);
          
        });

        GetButton((int)Btns.Btn_TemperatureSensor).gameObject.BindEvent(() =>
        {
            
            TemperatureSensorClickedEvent?.Invoke();
            SetToolBox(false);

        });
        

        SetToolBox(false);
        return true;
        
    }

  


    public void SetToolBox()
    {
        _isToolBoxOn = !_isToolBoxOn;
        _animator.SetBool(UI_ON,_isToolBoxOn);

        if (_isToolBoxOn)
        {
            //Logger.Log("툴박스 클릭 이벤트 ON---------------------------");
            ToolBoxOnEvent?.Invoke();
        }
    }
    public void SetToolBox(bool isOn)
    {
     
        _animator.SetBool(UI_ON,isOn);
    }
 


}
