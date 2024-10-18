
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
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_1(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        Depth1C_sceneController.DepthC21Init();
        CurrentScene.contentController.ShutTrainingInfroAnim();
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

public class DepthC21_State_2 : Base_SceneState
{
    public DepthC21_State_2(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.SetScriptUI();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class DepthC21_State_3 : Base_SceneState
{
    public DepthC21_State_3(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC21_State_4 : Base_SceneState
{
    public DepthC21_State_4(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC21_State_5 : Base_SceneState
{
    public DepthC21_State_5(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_LockingScrew);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew, false);
      
        CurrentScene.contentController.isStepMissionPerformable = true;

    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.contentController.isStepMissionPerformable = false;
    }
}

public class DepthC21_State_6 : Base_SceneState
{
    public DepthC21_State_6(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.isStepMissionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_ConnectionPiping, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_ConnectionPiping);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.contentController.isStepMissionPerformable = false;
    }
}

public class DepthC21_State_7 : Base_SceneState
{
    public DepthC21_State_7(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.OnTempSensor_Pipe);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
       
    }
}

public class DepthC21_State_8 : Base_SceneState
{
    public DepthC21_State_8(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.isStepMissionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.contentController.isStepMissionPerformable = false;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover);
        
    }}


public class DepthC21_State_9 : Base_SceneState
{
    public DepthC21_State_9(Depth1C_SceneController currentScene) : base(currentScene)
    {
       
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
        
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "보상도선 점검");
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
    
        CurrentScene.contentController.isStepMissionPerformable = false;
    }
}

public class DepthC21_State_10 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_10(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
       
       
        CurrentScene.contentController.isStepMissionPerformable = true;
        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.contentController.isStepMissionPerformable = false;
    }
}

public class DepthC21_State_11 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_11(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {

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
   
        base.OnExit();
    }
}


public class DepthC21_State_12 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC21_State_12(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        
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

public class DepthC21_State_13 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_13(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        //나사 위치 초기화
        //초기화로직이 포함되어있음으로 순서변경 X
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "저항 측정");
        Depth1C_sceneController.multimeterController.OnGroundNothing();
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB,false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);

        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.PROBE_TO_SCREWB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        
        
        Depth1C_sceneController.isAnodePut = false;
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        
    

        
        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
       
        base.OnExit();
    }
}

public class DepthC21_State_14 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_14(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC21_State_15 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC21_State_15(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 1;

        Depth1C_sceneController.PlayAnimationAndNarration(0);
        Depth1C_sceneController.contentController.Refresh();
        Depth1C_sceneController.DepthC22Init();
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


public class DepthC21_State_16 : Base_SceneState
{
    public DepthC21_State_16(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC21_State_17 : Base_SceneState
{
    public DepthC21_State_17(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC21_State_18 : Base_SceneState
    {
        public DepthC21_State_18(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class DepthC21_State_19 : Base_SceneState
    {
        public DepthC21_State_19(Depth1C_SceneController currentScene) : base(currentScene)
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
