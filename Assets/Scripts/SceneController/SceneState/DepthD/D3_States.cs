

using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>
/// 

public class DepthD31_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_1(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
    }

    public override void OnEnter()
    {
   
        _depthD3SceneController.DepthD31Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        

        base.OnExit();
    }
}

public class DepthD31_State_2 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_2(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_PressureSensor);
        
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_PressureSensor);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(1,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }}

public class DepthD31_State_3 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_3(DepthD3_SceneController currentScene) : base(currentScene) {_depthD3SceneController = currentScene;}

    public override void OnEnter()
    {
        base.OnEnter();
        _depthD3SceneController.controlPanel.SetPowerHandleOn();
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.PowerHandle);
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(2,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}

public class DepthD31_State_4 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;
    public DepthD31_State_4(DepthD3_SceneController currentScene) : base(currentScene) {_depthD3SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
        _depthD3SceneController.controlPanel.SetPowerHandleOff();
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.PressureSensorWaterPipeValve);

     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
 
        base.OnExit();
    }
}

public class DepthD31_State_5 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_5(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }

    public override void OnEnter()
    {

      
        
        _depthD3SceneController.isWindSession = false;
        
        foreach (var key in _depthD3SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthD3SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthD3SceneController.isScrewUnwindMap.Keys.ToList())
            _depthD3SceneController.isScrewWindMap[key] = false;


    
        _depthD3SceneController.TurnOnCollidersAndInit();
        
        
        _depthD3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = true;
        _depthD3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthD3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthD3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Update(0);
        _depthD3SceneController.animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = false;
        
        _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB,false);
        _depthD3SceneController.BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewB);


        
        
        base.OnEnter();
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.ConnectionScrewB);

        _depthD3SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        //CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthD31_State_6 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_6(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.NewPressureSensor);
    }
    public override void OnStep()
    {
        
        base.OnStep();
       
    }

    public override void OnExit()
    {
       ;
        base.OnExit();
    }
}

public class DepthD31_State_7 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_7(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthD3SceneController.ClearTool();
        _depthD3SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_Multimeter);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        //_depthD3SceneController.cameraController.isControllable = false;
    }

    public override void OnStep()
    {
      
        //CurrentScene.cameraController.isControllable = false;
        base.OnStep();
    }

    public override void OnExit()
    {
       // _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB);
        base.OnExit();
    }
}

public class DepthD31_State_8 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_8(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthD3SceneController.InitProbePos();
        _depthD3SceneController.isMultimeterOn = true;
        _depthD3SceneController.multimeterController.SetMeasureGuideStatus();
        _depthD3SceneController.multimeterController.PS_SetToDefaultMode();
        _depthD3SceneController.currentActiveTool = (int)DepthC3_GameObj.Multimeter;
        
        _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.MultimeterHandleHighlight,false);
        _depthD3SceneController.BlinkHighlight((int)DepthC3_GameObj.MultimeterHandleHighlight);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.MultimeterHandleHighlight);
        
        //Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.Multimeter);
        base.OnEnter();
        
        
       
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthD31_State_9 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_9(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.CathodeSensorInput);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC3_GameObj.AnodeSensorOutput);
        _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.CathodeSensorInput,false);
        _depthD3SceneController.BlinkHighlight((int)DepthC3_GameObj.CathodeSensorInput);
      
     
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Vent);
        base.OnExit();
    }
}
public class DepthD31_State_10 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthD3SceneController;

    public DepthD31_State_10(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthD3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthD3SceneController.ClearTool();
        _depthD3SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        _depthD3SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
               
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_PressureCalibrator);
    }

    public override void OnStep()
    {
        base.OnStep();
    }

    public override void OnExit()
    {
        _depthD3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3);
        base.OnExit();
    }
}

public class DepthD31_State_11 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthC3SceneController;

    public DepthD31_State_11(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


      public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
        _depthC3SceneController.pressureCalibratorController.BootPressureCalibrator();
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

public class DepthD31_State_12 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthC3SceneController;

    public DepthD31_State_12(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Default);
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

public class DepthD31_State_13 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthC3SceneController;

    public DepthD31_State_13(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }



    public override void OnEnter()
    {
        base.OnEnter();
       
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Tasks, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Tasks);
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.Tasks);
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_One);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Tasks, false);
        base.OnExit();
    }
}



public class DepthD31_State_14 : Base_SceneState
{
    private readonly DepthD3_SceneController _depthC3SceneController;

    public DepthD31_State_14(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Arrow_Down);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down, false);
        base.OnExit();
    }
}


public class DepthD31_State_15 : Base_SceneState
{
    private DepthD3_SceneController _depthC3SceneController;

    public DepthD31_State_15(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.PressureAndMeasureSetting);
        _depthC3SceneController.pressureCalibratorController.CursorOnMaxPressureSetting();//커서옮긴상황
        
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_One, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_One);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Number_Zero, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Number_Zero);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_Enter);
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


public class DepthD31_State_16 : Base_SceneState
{
    private DepthD3_SceneController _depthC3SceneController;
    public DepthD31_State_16(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F3);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4, false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.Btn_F4);
        base.OnEnter();

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F3);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_F4);
        base.OnExit();
    }
}




public class DepthD31_State_17 : Base_SceneState
{
    private DepthD3_SceneController _depthC3SceneController;
    public DepthD31_State_17(DepthD3_SceneController currentScene) : base(currentScene)
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

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Arrow_Down);
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.Btn_Enter);
        base.OnExit();
    }
}




public class DepthD31_State_18 : Base_SceneState
{
    private DepthD3_SceneController _depthC3SceneController;
    public DepthD31_State_18(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationModeSetting);
        _depthC3SceneController.pressureCalibratorController.CursorOnTestStrategySetting();
        _depthC3SceneController.pressureCalibratorController.SetTestStrategyModeString("3↑");
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





public class DepthD31_State_19 : Base_SceneState
{
    private DepthD3_SceneController _depthC3SceneController;
    public DepthD31_State_19(DepthD3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC3SceneController.pressureCalibratorController.KillCalibratePressureSeq();
        _depthC3SceneController.pressureCalibratorController.TurnOnUI(PressureCalibratorController.UI.CalibrationFinish);
        
        _depthC3SceneController.controlPanel.SetPowerHandleOff();
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PowerHandle, false);
      
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}
