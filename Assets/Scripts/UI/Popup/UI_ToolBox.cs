using System;

using UnityEngine;

public class UI_ToolBox : UI_Popup
{
    private readonly int UI_ON = Animator.StringToHash("On");
    public enum Btns
    {
        Btn_ElectricScrewdriver,
        Btn_Multimeter,
        Btn_TemperatureSensor,
        Btn_PressureSensor,
        Btn_LimitSwitch,
        Btn_PressureCalibrator,
        Btn_PSNewAdaptor,
        Btn_FlowSensor,
        Btn_LevelSensor
    }
    

    private bool _isToolBoxOn=false;
    private Animator _animator; 
    private DepthC2_SceneController _depthCsceneController;
    
    public static event Action ToolBoxOnEvent; // 도구함을 클릭해주세요 완수 시  
    public static event Action ToolBox_ElectronicScrewDriverClickedEvent; // 도구함을 클릭해주세요 완수 시  
    public static event Action ToolBox_MultimeterClickedEvent; // 도구함을 클릭해주세요 완수 시  
    public static event Action ToolBox_TemperatureSensorClickedEvent; // 온도센서 클릭해주세요 완수 시  
    public static event Action ToolBox_LimitSwitchSensorClickedEvent; // 리밋스위치 클릭해주세요 완수 시  
    public static event Action ToolBox_PressureCalibratorClickedEvent; // 리밋스위치 클릭해주세요 완수 시  
    public static event Action ToolBox_PressureSensorClicked; // 리밋스위치 클릭해주세요 완수 시  
    public static event Action ToolBox_PS_NewAdaptorCliked; // 리밋스위치 클릭해주세요 완수 시  
    public static event Action ToolBox_FlowSensorClicked; // 리밋스위치 클릭해주세요 완수 시  
    public static event Action ToolBox_LevelSensorBtnClicked; // 리밋스위치 클릭해주세요 완수 시  +
    public override bool Init()
    {
        if (!base.Init())
            return false;

        _depthCsceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<DepthC2_SceneController>();
        _animator = GetComponent<Animator>();
        BindButton(typeof(Btns));

        
        
        GetButton((int)Btns.Btn_ElectricScrewdriver).gameObject.BindEvent(() =>
        {
            //_depthCsceneController.OnScrewDriverBtnClicked();
            ToolBox_ElectronicScrewDriverClickedEvent?.Invoke();
            SetToolBox(false);
        });
        
        GetButton((int)Btns.Btn_Multimeter).gameObject.BindEvent(() =>
        {
            //_depthCsceneController.OnMultimeterBtnClicked();
            ToolBox_MultimeterClickedEvent?.Invoke();
            SetToolBox(false);
          
        });

        GetButton((int)Btns.Btn_TemperatureSensor).gameObject.BindEvent(() =>
        {
            
            ToolBox_TemperatureSensorClickedEvent?.Invoke();
            SetToolBox(false);

        });
        
        
        GetButton((int)Btns.Btn_LimitSwitch).gameObject.BindEvent(() =>
        {
            
            ToolBox_LimitSwitchSensorClickedEvent?.Invoke();
            SetToolBox(false);

        });

        GetButton((int)Btns.Btn_PressureCalibrator).gameObject.BindEvent(() =>
        {
            
            ToolBox_PressureCalibratorClickedEvent?.Invoke();
            SetToolBox(false);

        });
        
        GetButton((int)Btns.Btn_PressureSensor).gameObject.BindEvent(() =>
        {
            
            ToolBox_PressureSensorClicked?.Invoke();
            SetToolBox(false);

        });

            
        GetButton((int)Btns.Btn_PSNewAdaptor).gameObject.BindEvent(() =>
        {
            
            ToolBox_PS_NewAdaptorCliked?.Invoke();
            SetToolBox(false);

        });

        GetButton((int)Btns.Btn_FlowSensor).gameObject.BindEvent(() =>
        {

            ToolBox_FlowSensorClicked?.Invoke();
            SetToolBox(false);

        });
        
        GetButton((int)Btns.Btn_LevelSensor).gameObject.BindEvent(() =>
        {

            ToolBox_LevelSensorBtnClicked?.Invoke();
            SetToolBox(false);

        });
        
        SetToolBox(false);
        Refresh();
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
        _isToolBoxOn = isOn;
        _animator.SetBool(UI_ON,_isToolBoxOn);
    }


    ///현재는 한개의 UI만 Refresh하면서 킬 수 있게 동작,
    /// 파라미터가 없는경우 비활성화만 진행
    public void Refresh(Btns currentActiveTool = (Btns)987654321)
    {

        //평가하기의 경우 도구를 모두 보여주기
        if (Managers.ContentInfo.PlayData.Depth1 ==4)
        {
            
            for (int i = (int)Btns.Btn_ElectricScrewdriver; i < Enum.GetValues(typeof(Btns)).Length ; i++)
            {
                GetButton(i).gameObject.SetActive(true);
            }

            return;
        }
        
        // if (Managers.ContentInfo.PlayData.Depth1 ==5)
        // {
        //  
        //     GetButton( (int)Btns.Btn_Multimeter).gameObject.SetActive(true);
        //     return;
        // }
        
        for (int i = (int)Btns.Btn_ElectricScrewdriver; i < Enum.GetValues(typeof(Btns)).Length ; i++)
        {
            GetButton(i).gameObject.SetActive(false);
        }

        if ((int)currentActiveTool < Enum.GetValues(typeof(Btns)).Length)
        {
            GetButton((int)currentActiveTool).gameObject.SetActive(true);
        }
    }
    
    

}
