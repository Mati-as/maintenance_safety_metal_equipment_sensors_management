
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC53_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_0(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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

public class DepthC53_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_1(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
       
    }

    public override void OnEnter()
    {
        
        CurrentScene.contentController.ShutTrainingIntroAnim();
        _depthC5SceneController.DepthC53Init();
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class DepthC53_State_2 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_2(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_LevelSensor);
        _depthC5SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC53_State_3 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_3(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {

        _depthC5SceneController.controlPanel.SetPowerHandleOn();
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PowerHandle,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.PowerHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC53_State_4 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_4(DepthC5_SceneController currentScene) : base(currentScene) {_depthC5SceneController = currentScene;}


    public override void OnEnter()
    {

        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensor_PipeValve,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.LevelSensor_PipeValve);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensor_PipeValve);
        base.OnExit();
    }
}

public class DepthC53_State_5 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_5(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
                    
        _depthC5SceneController.isWindSession = false;
        
        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;
        foreach (var key in _depthC5SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC5SceneController.isScrewUnwindMap[key] = false;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);
        _depthC5SceneController.TurnOnCollidersAndInit();
        
        
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
       
  
        base.OnEnter();
        
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
  

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
       
        base.OnExit();
    }}

public class DepthC53_State_6 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_6(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensor,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.LevelSensor);
        _depthC5SceneController.levelSensorDisplayController.PowerOffSensor();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensor);
        base.OnExit();
    }

    
}



public class DepthC53_State_7 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_7(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _depthC5SceneController.cameraController.isControllable = false;
        _depthC5SceneController.levelSensorDisplayController.BootLevelSensorDisplay();
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

public class DepthC53_State_8 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_8(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
  
        base.OnEnter();

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC53_State_9 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_9(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isClickable = true;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(modeBtn:true);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ModeOrEnterBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        base.OnExit();
    }
}

public class DepthC53_State_10 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_10(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {

        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.SP1);
        
        _depthC5SceneController.levelSensorDisplayController.isClickable = true;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(setBtn:true);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.SetBtn);
        base.OnEnter();

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        base.OnExit();
    }
}

public class DepthC53_State_11 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_11(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }



    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isClickable = true;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.SP1_DEFAULT_VALUE.ToString("F1");
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC53_State_12 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_12(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isClickable = true;
        _depthC5SceneController.levelSensorDisplayController.SetClickable(modeBtn:true);
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ModeOrEnterBtn);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        base.OnExit();
    }
}

public class DepthC53_State_13 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_13(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isClickable = true;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.rP1);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(setBtn:true);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.SetBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(false,false);
        base.OnExit();
    }
}

public class DepthC53_State_14 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_14(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isClickable = false;
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.RP1_VALUE.ToString("F1");
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(false,false);
        base.OnExit();
        
    }
}



public class DepthC53_State_15 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;

    public DepthC53_State_15(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.SP1);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(modeBtn:true);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ModeOrEnterBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(false,false);
        base.OnExit();
    }
}


public class DepthC53_State_16 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_16(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isValueChangeModeUsable = false;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.SP1);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(setBtn:true);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.SetBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(false,false);
        base.OnExit();
        
    }
}
public class DepthC53_State_17 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_17(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {

        _depthC5SceneController.levelSensorDisplayController.OnValueSettingModeAccessableInit();
        
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.SP1_DEFAULT_VALUE.ToString("F1");
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn,false);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(setBtn:true);
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.SetBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(false,false);
        _depthC5SceneController.levelSensorDisplayController.isValueChangeModeUsable = false;
        _depthC5SceneController.levelSensorDisplayController.clickCountForChangingValue = -1;
        base.OnExit();
        
    }
}
public class DepthC53_State_18 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_18(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isValueChangeModeUsable = false;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.ValueSetting);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = 12.0f.ToString("F1");
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn,false);
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        _depthC5SceneController.levelSensorDisplayController.SetClickable(modeBtn:true);
        
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ModeOrEnterBtn);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.OnModeOff();
        base.OnExit();
        
    }
}
public class DepthC53_State_19 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_19(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.SP1_DEFAULT_VALUE.ToString("F1");
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.SP1);

        _depthC5SceneController.OnModeBtnAvailable();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.OnModeOff();
        base.OnExit();
        
    }
}
public class DepthC53_State_20 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_20(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isValueChangeModeUsable = false;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.EF);

        _depthC5SceneController.OnSetBtnAvailable();
        
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.OnModeOff();
        base.OnExit();
        
    }
}
public class DepthC53_State_21 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_21(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.isValueChangeModeUsable = false;
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.rES);

        _depthC5SceneController.OnModeBtnAvailable();
        
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.OnModeOff();
        base.OnExit();
        
    }
}
public class DepthC53_State_22 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_22(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.OutputSelection);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.OUTPUT_ONE;
           
        _depthC5SceneController.OnModeBtnAvailable();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}
public class DepthC53_State_23 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_23(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.OutputSelection);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.NORMAL_CLOSE;
        
        _depthC5SceneController.OnSetBtnAvailable();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}
public class DepthC53_State_24 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_24(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.OutputSelection);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue = LevelSensorDisplayController.NORMAL_OPEN;
        _depthC5SceneController.OnModeBtnAvailable();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}
public class DepthC53_State_25 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_25(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.levelSensorDisplayController.SetMode(LevelSensorDisplayController.Mode.Default_ValueCheck);
        _depthC5SceneController.levelSensorDisplayController.currentDisplayValue =LevelSensorDisplayController.SP1_DEFAULT_VALUE.ToString("F1");
     
        
        _depthC5SceneController.isWindSession = true;
        
        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;
        foreach (var key in _depthC5SceneController.isScrewWindMap.Keys.ToList())
            _depthC5SceneController.isScrewWindMap[key] = false;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);
        _depthC5SceneController.TurnOnCollidersAndInit();
        
        
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"Screw", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
     

        base.OnEnter();
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}


public class DepthC53_State_26 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_26(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.ClearTool();
        _depthC5SceneController.controlPanel.SetPowerHandleOff();
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PowerHandle,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.PowerHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC53_State_27 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC53_State_27(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}