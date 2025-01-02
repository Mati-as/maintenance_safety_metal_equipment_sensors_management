

using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>
/// 
public class DepthC33_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_0(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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

public class DepthC33_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_1(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
    
        
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        _depthC3SceneController.DepthC33Init();
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

public class DepthC33_State_2 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_2(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_PressureSensor);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC33_State_3 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_3(DepthC3_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.controlPanel.SetPowerHandleOn();
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PowerHandle, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PowerHandle);
        _depthC3SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC33_State_4 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_4(DepthC3_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.controlPanel.SetPowerHandleOff();
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorWaterPipeValve, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PressureSensorWaterPipeValve);
        _depthC3SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorWaterPipeValve);
        base.OnExit();
    }
}

public class DepthC33_State_5 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_5(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {


        _depthC3SceneController.isWindSession = false;
        
        foreach (var key in _depthC3SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC3SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC3SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC3SceneController.isScrewWindMap[key] = false;


    
        _depthC3SceneController.TurnOnCollidersAndInit();
        
        
        _depthC3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = true;
        _depthC3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Update(0);
        _depthC3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = false;
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB,false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewB);


        
        
        base.OnEnter();
        _depthC3SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC33_State_6 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_6(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensor, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PressureSensor);
        base.OnStep();
       
    }

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensor);
        base.OnExit();
    }
}

public class DepthC33_State_7 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_7(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;
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

public class DepthC33_State_8 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_8(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_PressureCalibrator);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC33_State_9 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_9(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
        _depthC3SceneController.pressureCalibratorController.BootPressureCalibrator();
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Vent, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Vent);
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Vent);
        base.OnExit();
    }
}

public class DepthC33_State_10 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_10(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.ChangeTooltipText((int)DepthC3_GameObj.Btn_F3,"0으로 압력 초기화 [ZERO PRESSURE]");
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F3);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
  
   _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3);
   _depthC3SceneController.ChangeTooltipText((int)DepthC3_GameObj.Btn_F3,"");
        base.OnExit();
    }
}

public class DepthC33_State_11 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_11(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }



    public override void OnEnter()
    {
        base.OnEnter();
       
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Tasks, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Tasks);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Tasks, false);
        base.OnExit();
    }
}


public class DepthC33_State_12 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_12(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Tasks);
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_One);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        base.OnExit();
    }
}

public class DepthC33_State_13 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_13(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC33_State_14 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_14(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }



    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Arrow_Down);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down, false);
        base.OnExit();
    }
}


public class DepthC33_State_15 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;

    public DepthC33_State_15(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC3SceneController.pressureCalibratorController.CursorOnMaxPressureSetting();//커서옮긴상황
        
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_One);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_Zero, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_Zero);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Enter);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_Zero);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter);
        base.OnExit();
    }
}


public class DepthC33_State_16 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_16(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
 
      
        _depthC3SceneController.ChangeTooltipText((int)DepthC3_GameObj.Btn_F3, "F3 : Turn Loop Power Up");
        _depthC3SceneController.ChangeTooltipText((int)DepthC3_GameObj.Btn_F4, "F4 : Continue");
        
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F3);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F4);
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4);
        base.OnExit();
    }
}
public class DepthC33_State_17 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_17(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}




public class DepthC33_State_18 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_18(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
       // _depthC3SceneController.pressureCalibratorController.CursorOnTestStrategySetting();
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Arrow_Down);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Enter);
        base.OnEnter();
        _depthC3SceneController.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter);
        base.OnExit();
    }
}


public class DepthC33_State_19 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_19(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.TestStrategy);
        _depthC3SceneController.cameraController.isControllable = false;
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Enter);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter);
        base.OnExit();
    }


}



public class DepthC33_State_20 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_20(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
        _depthC3SceneController.pressureCalibratorController.CursorOnTestStrategySetting();
        _depthC3SceneController.pressureCalibratorController.SetTestStrategyModeString("3↑");
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F4);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4);
        base.OnExit();
    }

}


public class DepthC33_State_21 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_21(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Calibrating);
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.ChangeTooltipText((int)DepthC3_GameObj.Btn_F4, "F4: Auto Test");
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F4);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4);
        base.OnExit();
    }
}



public class DepthC33_State_22 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_22(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Calibrating);
        _depthC3SceneController.cameraController.isControllable = false;
        _depthC3SceneController.pressureCalibratorController.CalibratePressure();
        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.pressureCalibratorController.KillCalibratePressureSeq();
        base.OnExit();
    }
}


public class DepthC33_State_23 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_23(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.KillCalibratePressureSeq();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationFinish);
        _depthC3SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}



public class DepthC33_State_24 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC33_State_24(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.controlPanel.SetPowerHandleOff();
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PowerHandle, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PowerHandle);
        _depthC3SceneController.cameraController.isControllable = false;
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PowerHandle);
        base.OnExit();
    }
}