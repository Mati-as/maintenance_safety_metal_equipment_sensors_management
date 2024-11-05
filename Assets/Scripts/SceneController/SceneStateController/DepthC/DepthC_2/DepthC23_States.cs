using System.Linq;

/// <summary>
///     3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
public class DepthC23_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_1(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.DepthC23Init();
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowNothing();
        base.OnEnter();
    }

  
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC23_State_2 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_2(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_TemperatureSensor);
        _depthC2SceneController.controlPanel.SetPowerHandleOn();

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC23_State_3 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_3(DepthC2_SceneController currentScene) : base(currentScene) {_depthC2SceneController = currentScene;}
    public override void OnEnter()
    {
        _depthC2SceneController.controlPanel.SetPowerHandleOn();
        _depthC2SceneController.GetObject((int)DepthC_GameObj.NewTemperatureSensor).SetActive(true);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.PowerHandle);
        base.OnEnter();
    }

  
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC23_State_4 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_4(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Pipe_WaterEffect).SetActive(true);
        _depthC2SceneController.controlPanel.SetPowerHandleOff();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TankValve, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TankValve);
        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TankValve);
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Pipe_WaterEffect).SetActive(false);
        base.OnExit();
    }
}

public class DepthC23_State_5 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_5(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Pipe_WaterEffect).SetActive(false);
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        _depthC2SceneController.SetUnscrewStatus(false);
        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover);
        base.OnExit();
    }
}

public class DepthC23_State_6 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_6(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        Logger.Log("State 6 Enter  ---------------------------------------");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "나사");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewC, "나사");
        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);

        //나사 위치 초기화
        _depthC2SceneController.SetUnscrewStatus(false);
        _depthC2SceneController.TurnOnCollidersAndInit();

        _depthC2SceneController.isWindSession = false;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC,false);
        
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewC);

      
        CurrentScene.contentController.isStepMissionPerformable = true;


        base.OnEnter();
    }


     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);

        _depthC2SceneController.CurrentActiveTool = -1;
        _depthC2SceneController._isDriverOn = false;
        _depthC2SceneController.isMultimeterOn = false;

        _depthC2SceneController.ToggleActiveState(
            _depthC2SceneController.GetObject((int)DepthC_GameObj.ElectricScrewdriver),
            _depthC2SceneController.isDriverOn);
        base.OnExit();
    }
}

public class DepthC23_State_7 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_7(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.ClearTool();
        _depthC2SceneController.TurnOnCollidersAndInit();
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TemperatureSensor, false);
        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;

        foreach (var key in _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC2SceneController.currentScrewGaugeStatus[key] = 1f;

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play("UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play("UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play("UnScrew", 0, 1);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA]
            .SetBool(DepthC2_SceneController.UNWIND, true);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB]
            .SetBool(DepthC2_SceneController.UNWIND, true);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC]
            .SetBool(DepthC2_SceneController.UNWIND, true);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;


        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);


        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;


        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);

        
        base.OnEnter();
    }


     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);


        _depthC2SceneController.TurnOnCollidersAndInit();
        _depthC2SceneController.ClearTool();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TemperatureSensor);
        base.OnExit();
    }
}

public class DepthC23_State_8 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_8(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_TemperatureSensor);
        _depthC2SceneController.TurnOnCollidersAndInit();

        _depthC2SceneController.SetScrewDriverSection(false);

        //나사 위치 초기화1
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;

        foreach (var key in _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC2SceneController.currentScrewGaugeStatus[key] = 1f;
        //
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play("UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play("UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play("UnScrew", 0, 1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA]
            .SetBool(DepthC2_SceneController.UNWIND, true);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB]
            .SetBool(DepthC2_SceneController.UNWIND, true);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC]
            .SetBool(DepthC2_SceneController.UNWIND, true);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;


        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);


        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;


        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);

        base.OnEnter();
    }

   
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC23_State_9 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_9(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(true);
        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
        _depthC2SceneController.multimeterController.TurnOffResistantMode();
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Probe_Cathode).SetActive(false);
        _depthC2SceneController.GetObject((int)DepthC_GameObj.Probe_Anode).SetActive(false);

        _depthC2SceneController.HighlightBlink((int)DepthC_GameObj.MultimeterHandleHighlight);
        base.OnEnter();
    }


    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC23_State_10 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_10(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.TurnOnCollidersAndInit();
        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(true);
        _depthC2SceneController.CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
        _depthC2SceneController.multimeterController.SetToResistanceModeAndRotation();


        _depthC2SceneController.multimeterController.OnGroundNothing();

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB);


        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "측정단자 A");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "측정단자 B");
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewA);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;


        _depthC2SceneController.isAnodePut = false;

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
    }
}

public class DepthC23_State_11 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_11(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }


    public override void OnEnter()
    {
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "측정단자 A");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "접지");
        
        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
        
        CurrentScene.contentController.isStepMissionPerformable = true;

        _depthC2SceneController.isAnodePut = false;
        _depthC2SceneController.multimeterController.OnGroundNothing();

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB, false);
    
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewA);
        
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;

        //CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalA);


        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB);
        base.OnExit();
    }
}


public class DepthC23_State_12 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_12(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "나사");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewC, "나사");

        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh();
        
        _depthC2SceneController.isWindSession = true;
        _depthC2SceneController.isMultimeterOn = false;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(false);

        // CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        // CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);


        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewC);

        _depthC2SceneController.CurrentActiveTool = (int)DepthC_GameObj.ElectricScrewdriver;
        _depthC2SceneController._isDriverOn = true;
        _depthC2SceneController.ToggleActiveState(_depthC2SceneController.GetObject((int)DepthC_GameObj.ElectricScrewdriver),
            _depthC2SceneController.isDriverOn);


        _depthC2SceneController.TurnOnCollidersAndInit();

        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC2SceneController.isScrewWindMap.Keys.ToList())
            _depthC2SceneController.isScrewWindMap[key] = false;


        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND, false);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND, false);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play("Screw", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play("Screw", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play("Screw", 0, 0);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(0);

        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);

        
        
        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC2SceneController.controlPanel.SetPowerHandleOn();
        _depthC2SceneController.ClearTool();
        _depthC2SceneController.ToggleActiveState(
            _depthC2SceneController.GetObject((int)DepthC_GameObj.ElectricScrewdriver),
            _depthC2SceneController.isDriverOn);
        base.OnExit();
    }
}

public class DepthC23_State_13 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_13(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.PowerHandle, "전원 켜기");
        _depthC2SceneController.controlPanel.SetPowerHandleOff();

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.PowerHandle);

        
        foreach (var key in _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC2SceneController.currentScrewGaugeStatus[key] = 1f;

        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"Screw", 0, 1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"Screw", 0, 1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"Screw", 0, 1);

        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        //
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,false);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,false);
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,false);
        //
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;


        base.OnEnter();
    }

    public override void OnStep()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle);
    }

    public override void OnExit()
    {
        _depthC2SceneController.isMultimeterOn = false;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
        base.OnExit();
    }
}

public class DepthC23_State_14 : Base_SceneState
{
    private readonly DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_14(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
    }

     public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC2SceneController.ClearTool();

        _depthC2SceneController.ToggleActiveState(
            _depthC2SceneController.GetObject((int)DepthC_GameObj.ElectricScrewdriver),
            _depthC2SceneController.isDriverOn);

        base.OnExit();
    }
}


public class DepthC23_State_15 : Base_SceneState
{
    private DepthC2_SceneController _depthC2SceneController;

    public DepthC23_State_15(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
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


public class DepthC23_State_16 : Base_SceneState
{
    public DepthC23_State_16(DepthC2_SceneController currentScene) : base(currentScene)
    {
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


