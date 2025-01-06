
using System.Linq;

/// <summary>
/// 3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
///
///
///
///  
public class DepthC21_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_1(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC2SceneController.DepthC21Init();
        CurrentScene.contentController.ShutTrainingIntroAnim();
        
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowNothing();
        base.OnEnter();
      
    }

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}

public class DepthC21_State_2 : Base_SceneState
{
    public DepthC21_State_2(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC2_GameObj.TemperatureSensor);
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();

    }
    
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC21_State_3 : Base_SceneState
{
    public DepthC21_State_3(DepthC2_SceneController currentScene) : base(currentScene){}
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC2_GameObj.TemperatureSensor);
    }
    public override void OnEnter(){  
        base.OnEnter();
        CurrentScene.cameraController.isControllable = true;
        
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC21_State_4 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_4(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Wrench).SetActive(false);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = true;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC21_State_5 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_5(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew, false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_LockingScrew);
      
     
        base.OnEnter();

    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {

        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Wrench).SetActive(false);
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew);
        CurrentScene.contentController.isStepMissionPerformable = false;
        base.OnExit();
    }
}

public class DepthC21_State_6 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_6(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Wrench).SetActive(false);
        _depthC2SceneController.SetWaterDecalStatus(true);
        _depthC2SceneController.FadeInDecal();
        _depthC2SceneController.SetParticleStatus(true);
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Wrench).SetActive(false);
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_ConnectionPiping, false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_ConnectionPiping);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {

        _depthC2SceneController.SetWaterMatAlpha();
        _depthC2SceneController.SetParticleStatus(false);
        
        CurrentScene.contentController.isStepMissionPerformable = false;
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_ConnectionPiping);
        base.OnExit();
    }
}

public class DepthC21_State_7 : Base_SceneState
{
    public DepthC21_State_7(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.OnTempSensor_Pipe);
        base.OnEnter();
    }

    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC21_State_8 : Base_SceneState
{
    public DepthC21_State_8(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.isStepMissionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover, false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_Cover);
        base.OnEnter();
        
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.contentController.isStepMissionPerformable = false;
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover);
        
        base.OnExit();
    }}


public class DepthC21_State_9 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_9(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC);
        
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "보상도선 점검");
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        _depthC2SceneController.isWindSession = false;
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "나사");
        CurrentScene.contentController.isStepMissionPerformable = false;
        
               
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
             
        foreach (var key in  _depthC2SceneController.isScrewUnwindMap.Keys.ToList())
        {
            _depthC2SceneController.isScrewUnwindMap[key] = false;
        }
        
        

        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
    
        
        
        //나사 위치 초기
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
       
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);


    }
}

public class DepthC21_State_10 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_10(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.TurnOnCollidersAndInit();
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        CurrentScene.contentController.isStepMissionPerformable = true;

       
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
   
        
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowNothing();
        base.OnExit();
     
    }
}


public class DepthC21_State_11 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_11(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowErrorMessage();
        
        _depthC2SceneController.isWindSession = false;

        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        base.OnEnter();
    }


    
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowNothing();
        base.OnExit();
    }
}

public class DepthC21_State_12 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_12(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC2SceneController.TurnOnCollidersAndInit();
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        CurrentScene.contentController.isStepMissionPerformable = true;
       
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
             
        foreach (var key in  _depthC2SceneController.isScrewUnwindMap.Keys.ToList())
        {
            _depthC2SceneController.isScrewUnwindMap[key] = false;
        }
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, false);

        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewC);
     
        
        
        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].StopPlayback();
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].StopPlayback();
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].StopPlayback();
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
     
        CurrentScene.contentController.isStepMissionPerformable = true;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
       
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        _depthC2SceneController.TurnOnCollidersAndInit();
     
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC);
        
        _depthC2SceneController.ClearTool();
        
        
        base.OnExit();
    }
}


public class DepthC21_State_13 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    
    public DepthC21_State_13(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        _depthC2SceneController.TurnOnCollidersAndInit();
        _depthC2SceneController.ClearTool();
        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 1f;
        }
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);

        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
        
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        // Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB,false);
        
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;
        
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        _depthC2SceneController.isMultimeterOn = false;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(false);
        _depthC2SceneController.ClearTool();
        base.OnExit();
    }
}

public class DepthC21_State_14 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;

    public DepthC21_State_14(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;

    }

    public override void OnEnter()
    {

        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC2SceneController.CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        _depthC2SceneController.multimeterController.SetToDefaultMode();
        
        _depthC2SceneController.BlinkHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight);
     
        base.OnEnter();



    }

    public override void OnExit()
    {
        
        _depthC2SceneController.isMultimeterOn = false;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus(false);
    }
}

public class DepthC21_State_15 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_15(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
        
    }

    public override void OnEnter()
    {
        _depthC2SceneController.InitProbePos();
        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC2SceneController.CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        _depthC2SceneController.multimeterController.SetToResistanceModeAndRotation();
        
        //나사 위치 초기화
        //초기화로직이 포함되어있음으로 순서변경 X
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "측정단자 A");
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "측정단자 B");
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewA);
        
        _depthC2SceneController.multimeterController.OnGroundNothing();
        
      
       

        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;
        
        
        _depthC2SceneController.isAnodePut = false;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        
        base.OnEnter();

        
        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        _depthC2SceneController.ClearTool();
        base.OnExit();
    }
}

public class DepthC21_State_16 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_16(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    

    public override void OnEnter()
    {

        _depthC2SceneController.InitProbePos();
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "측정단자 A");
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB);
        
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewA);

        
        
        _depthC2SceneController.isMultimeterOn = true;
        _depthC2SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC2SceneController.CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        _depthC2SceneController.multimeterController.SetToResistanceModeAndRotation();
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        
        _depthC2SceneController.isAnodePut = false;
        
        _depthC2SceneController.multimeterController.OnGroundNothing();


        
        //CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalA);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB);
        

        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;
     
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_GroundingTerminalA);
        
        
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        _depthC2SceneController.ClearTool();
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        base.OnExit();
    }
}


public class DepthC21_State_17 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC21_State_17(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthC2SceneController.contentController.Refresh();
        _depthC2SceneController.DepthC22Init();
        
        base.OnEnter();
        _depthC2SceneController.PlayAnimation(1);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


