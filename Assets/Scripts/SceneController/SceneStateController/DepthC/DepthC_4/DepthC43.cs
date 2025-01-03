

using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>
/// 
public class DepthC43_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_0(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }
        
    public override void OnEnter()
    {

        base.OnEnter();
     
    }

    public override void OnStep()
    {
        base.OnStep();
    }

    public override void OnExit()
    {
       
    }
}

public class DepthC43_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_1(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
    
        
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        _depthC4SceneController.DepthC43Init();
        CurrentScene.cameraController.isControllable = false;
        CurrentScene.contentController.ShutTrainingIntroAnim();
    
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        CurrentScene.cameraController.isControllable = true;
        base.OnExit();
    }
}

public class DepthC43_State_2 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_2(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_FlowSensor);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC43_State_3 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_3(DepthC4_SceneController currentScene) : base(currentScene) {_depthC4SceneController = currentScene;}

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.controlPanel.SetPowerHandleOn();
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.PowerHandle, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.PowerHandle);
        _depthC4SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC43_State_4 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_4(DepthC4_SceneController currentScene) : base(currentScene) {_depthC4SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.controlPanel.SetPowerHandleOff();
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowerSensor_Valve, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.FlowerSensor_Valve);
        _depthC4SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowerSensor_Valve);
        base.OnExit();
    }
}

public class DepthC43_State_5 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_5(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {


        _depthC4SceneController.isWindSession = false;
        
        foreach (var key in _depthC4SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC4SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC4SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC4SceneController.isScrewUnwindMap[key] = false;


    
        _depthC4SceneController.TurnOnCollidersAndInit();
        
        
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = true;
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Update(0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = false;
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ConnectionScrewB,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.ConnectionScrewB);


        
        
        base.OnEnter();
        _depthC4SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC4SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC43_State_6 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_6(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.controlPanel.SetPowerHandleOff();
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.NewFlowSensor, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.NewFlowSensor);
        _depthC4SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.NewFlowSensor);
        base.OnExit();
    }
}

public class DepthC43_State_7 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_7(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;
    }

    public override void OnStep()
    {
        base.OnStep();
    }

    public override void OnExit()
    {

        base.OnExit();
    }
}

public class DepthC43_State_8 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_8(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_PressureCalibrator);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC43_State_9 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_9(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
        _depthC4SceneController.pressureCalibratorController.BootPressureCalibrator();
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Vent, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Vent);
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Vent);
        base.OnExit();
    }
}

public class DepthC43_State_10 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_10(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.ChangeTooltipText((int)DepthC4_GameObj.Btn_F3,"0으로 압력 초기화 [ZERO PRESSURE]");
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F3, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_F3);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
  
   _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F3);
   _depthC4SceneController.ChangeTooltipText((int)DepthC4_GameObj.Btn_F3,"");
        base.OnExit();
    }
}

public class DepthC43_State_11 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_11(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }



    public override void OnEnter()
    {
        base.OnEnter();
       
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Tasks, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Tasks);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Tasks, false);
        base.OnExit();
    }
}


public class DepthC43_State_12 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_12(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Tasks);
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_One, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Number_One);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_One, false);
        base.OnExit();
    }
}

public class DepthC43_State_13 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_13(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC43_State_14 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_14(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }



    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;
        
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Arrow_Down, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Arrow_Down);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Arrow_Down, false);
        base.OnExit();
    }
}


public class DepthC43_State_15 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;

    public DepthC43_State_15(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC4SceneController.pressureCalibratorController.CursorOnMaxPressureSetting();//커서옮긴상황
        
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_One, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Number_One);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_Zero, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Number_Zero);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Enter);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_Zero);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Number_One);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter);
        base.OnExit();
    }
}


public class DepthC43_State_16 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_16(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
 
      
        _depthC4SceneController.ChangeTooltipText((int)DepthC4_GameObj.Btn_F3, "F3 : Turn Loop Power Up");
        _depthC4SceneController.ChangeTooltipText((int)DepthC4_GameObj.Btn_F4, "F4 : Continue");
        
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F3, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_F3);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_F4);
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F3);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4);
        base.OnExit();
    }
}
public class DepthC43_State_17 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_17(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}




public class DepthC43_State_18 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_18(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
       // _depthC4SceneController.pressureCalibratorController.CursorOnTestStrategySetting();
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Arrow_Down, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Arrow_Down);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Enter);
        base.OnEnter();
        _depthC4SceneController.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Arrow_Down);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter);
        base.OnExit();
    }
}


public class DepthC43_State_19 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_19(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.TestStrategy);
        _depthC4SceneController.cameraController.isControllable = false;
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_Enter);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_Enter);
        base.OnExit();
    }


}



public class DepthC43_State_20 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_20(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
        _depthC4SceneController.pressureCalibratorController.CursorOnTestStrategySetting();
        _depthC4SceneController.pressureCalibratorController.SetTestStrategyModeString("3↑");
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_F4);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4);
        base.OnExit();
    }

}


public class DepthC43_State_21 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_21(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Calibrating);
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.ChangeTooltipText((int)DepthC4_GameObj.Btn_F4, "F4: Auto Test");
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.Btn_F4);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.Btn_F4);
        base.OnExit();
    }
}



public class DepthC43_State_22 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_22(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Calibrating);
        _depthC4SceneController.cameraController.isControllable = false;
        _depthC4SceneController.pressureCalibratorController.CalibratePressure();
        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.pressureCalibratorController.KillCalibratePressureSeq();
        base.OnExit();
    }
}


public class DepthC43_State_23 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_23(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.pressureCalibratorController.KillCalibratePressureSeq();
        _depthC4SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationFinish);
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}



public class DepthC43_State_24 : Base_SceneState
{
    private DepthC4_SceneController _depthC4SceneController;
    public DepthC43_State_24(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC4SceneController.controlPanel.SetPowerHandleOff();
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.PowerHandle, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.PowerHandle);
        _depthC4SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.PowerHandle);
        base.OnExit();
    }
}