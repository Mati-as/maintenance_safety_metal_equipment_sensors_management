
using System.Linq;

/// <summary>
/// 3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
///
///
///
///  
public class DepthC23_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_1(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    public override void OnEnter()
    {
        Depth1C_sceneController.DepthC23Init();
        Depth1C_sceneController.GetObject((int)DepthC_GameObj.Indicator)
            .GetComponent<IndicatorController>().ShowNothing();
        base.OnEnter();
    
        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        
        base.OnExit();
    }
}

public class DepthC23_State_2 : Base_SceneState
{
    public DepthC23_State_2(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class DepthC23_State_3 : Base_SceneState
{
    public DepthC23_State_3(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle,false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.PowerHandle);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class DepthC23_State_4 : Base_SceneState
{
    public DepthC23_State_4(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TankValve, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TankValve);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TankValve);
        base.OnExit();
    }
}

public class DepthC23_State_5 : Base_SceneState
{
    public DepthC23_State_5(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        base.OnEnter();

    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover);
        base.OnExit();
       
    }
}

public class DepthC23_State_6 : Base_SceneState
{
  Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_6(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    { 
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);

        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        //나사 위치 초기화
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,false);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(0);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].StopPlayback();
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].StopPlayback();
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].StopPlayback();
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);
      
        //
        
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        CurrentScene.contentController.isStepMissionPerformable = true;
        
        
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        base.OnEnter();
    }


    public override void OnStep()
    {
    }

    public override void OnExit()
    {     
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
        
        Depth1C_sceneController.CurrentActiveTool =  -1;
        Depth1C_sceneController._isDriverOn= false;
        Depth1C_sceneController.isMultimeterOn = false;
        
        Depth1C_sceneController.ToggleActiveState(Depth1C_sceneController.
            GetObject((int)DepthC_GameObj.ElectricScrewdriver), Depth1C_sceneController.isDriverOn);
        base.OnExit();
       
    }
}

public class DepthC23_State_7 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC23_State_7(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {

        Depth1C_sceneController.ClearTool();
        
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TemperatureSensor, false);
        //나사 위치 초기화
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 1f;
        }
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,true);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB,false);
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
       
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }


    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Depth1C_sceneController.ClearTool();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TemperatureSensor);
        base.OnExit();
       
    }
}

public class DepthC23_State_8 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_8(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
     
    }

    public override void OnEnter()
    {
        Depth1C_sceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        
        Depth1C_sceneController.SetScrewDriverSection(isWind:false);
     //나사 위치 초기화1
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 1f;
        }
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,true);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB,false);
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        
        base.OnEnter();
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        
        
    }
    
}


public class DepthC23_State_9 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_9(Depth1C_SceneController currentScene) : base(currentScene)
    {
       
    }

    public override void OnEnter()
    {
        base.OnEnter();


    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
       
    }
}

public class DepthC23_State_10 : Base_SceneState
{
    
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_10(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
      

        Depth1C_sceneController.multimeterController.OnGroundNothing();
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB);

        
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "저항 측정");
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);

        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;

        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;


        Depth1C_sceneController.isAnodePut = false;

        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
   
    }
}

public class DepthC23_State_11 : Base_SceneState
{
   
  
 Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_11(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    

    public override void OnEnter()
    {
        CurrentScene.contentController.isStepMissionPerformable = true;
        
        Depth1C_sceneController.isAnodePut = false;
        
        Depth1C_sceneController.multimeterController.OnGroundNothing();

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalA,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB,false);
        
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalA);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalB);
        

        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
     
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalA);
        
        
        base.OnEnter();
    }
    public override void OnStep()
    {
    }

    public override void OnExit()
    {
     
   
        base.OnExit();
    }
}


public class DepthC23_State_12 : Base_SceneState
{
  
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_12(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        
        
        Depth1C_sceneController.SetScrewDriverSection(isWind:true);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        
        Depth1C_sceneController.CurrentActiveTool =  (int)DepthC_GameObj.ElectricScrewdriver;
        Depth1C_sceneController._isDriverOn= true;
        Depth1C_sceneController.isMultimeterOn = false;
        Depth1C_sceneController.ToggleActiveState(Depth1C_sceneController.
            GetObject((int)DepthC_GameObj.ElectricScrewdriver), Depth1C_sceneController.isDriverOn);
        
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        //나사 위치 초기화
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,false);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);
        
        base.OnEnter();
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        
        Depth1C_sceneController.ClearTool();
        
        Depth1C_sceneController.ToggleActiveState(Depth1C_sceneController.
            GetObject((int)DepthC_GameObj.ElectricScrewdriver), Depth1C_sceneController.isDriverOn);
        base.OnExit();
    }
}

public class DepthC23_State_13 : Base_SceneState
{
   
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC23_State_13(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    
    public override void OnEnter()
    {
        
       
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 1f;
        }
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(Depth1C_SceneController.UNWIND,true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(Depth1C_SceneController.UNWIND,true);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        

        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Depth1C_sceneController.isMultimeterOn = false;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
        base.OnExit();
    }
}

public class DepthC23_State_14 : Base_SceneState
{
    
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC23_State_14(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    

    public override void OnEnter()
    {
      
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Depth1C_sceneController.ClearTool();
        
        Depth1C_sceneController.ToggleActiveState(Depth1C_sceneController.
            GetObject((int)DepthC_GameObj.ElectricScrewdriver), Depth1C_sceneController.isDriverOn);
        
        base.OnExit();
    }
}


public class DepthC23_State_15 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC23_State_15(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
      
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthC23_State_16 : Base_SceneState
{
    public DepthC23_State_16(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
     
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthC23_State_17 : Base_SceneState
{
    public DepthC23_State_17(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthC23_State_18 : Base_SceneState
    {
        public DepthC23_State_18(Depth1C_SceneController currentScene) : base(currentScene)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
         
            
            
        }

        public override void OnStep()
        {
           
        }

        public override void OnExit()
        {
         
            
        }


    }


    public class DepthC23_State_19 : Base_SceneState
    {
        public DepthC23_State_19(Depth1C_SceneController currentScene) : base(currentScene)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnStep()
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        
        }
    
    }
