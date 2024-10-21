
using System.Linq;

/// <summary>
/// 3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
///
///
///
///  
public class DepthC22_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC22_State_1(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    public override void OnEnter()
    {
        Logger.Log("C22 초기화 진행 -----------------------");
        Depth1C_sceneController.DepthC22Init();
        Depth1C_sceneController.GetObject((int)DepthC_GameObj.Indicator).GetComponent<IndicatorController>().ShowErrorMessage();
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

public class DepthC22_State_2 : Base_SceneState
{
    public DepthC22_State_2(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_3 : Base_SceneState
{
    public DepthC22_State_3(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_4 : Base_SceneState
{
    public DepthC22_State_4(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        //초기화의 BindEvent에서 다룹니다.
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_SensingElement);
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew, false);
        
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_LockingScrew);
        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_SensingElement);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew);
        base.OnExit();
    }
}

public class DepthC22_State_5 : Base_SceneState
{
    public DepthC22_State_5(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC22_State_6(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    public override void OnEnter()
    {
        Depth1C_sceneController.GetObject((int)DepthC_GameObj.Indicator).GetComponent<IndicatorController>().ShowErrorMessage();
        base.OnEnter();
       
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
       
    }
}

public class DepthC22_State_7 : Base_SceneState
{
    public DepthC22_State_7(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_8 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC22_State_8(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        
        
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
        base.OnExit();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover);
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
    }
    
}


public class DepthC22_State_9 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC22_State_9(Depth1C_SceneController currentScene) : base(currentScene)
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
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 0f;
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

public class DepthC22_State_10 : Base_SceneState
{Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC22_State_10(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }
    
    public override void OnEnter()
    {
        
     

        foreach (var key in  Depth1C_sceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            Depth1C_sceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        foreach (var key in  Depth1C_sceneController._isScrewWindMap.Keys.ToList())
        {
            Depth1C_sceneController._isScrewWindMap[key] = false;
        }
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB,false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC,false);
        base.OnEnter();

        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
      
    }
}

public class DepthC22_State_11 : Base_SceneState
{
   
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC22_State_11(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 1;
        
        Depth1C_sceneController.PlayAnimationAndNarration(0);
        Depth1C_sceneController.contentController.Refresh();
        Depth1C_sceneController.DepthC23Init();
        
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


public class DepthC22_State_12 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    
    public DepthC22_State_12(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_13 : Base_SceneState
{
   
    public DepthC22_State_13(Depth1C_SceneController currentScene) : base(currentScene)
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

public class DepthC22_State_14 : Base_SceneState
{
    
    public DepthC22_State_14(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC22_State_15 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public DepthC22_State_15(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC22_State_16 : Base_SceneState
{
    public DepthC22_State_16(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC22_State_17 : Base_SceneState
{
    public DepthC22_State_17(Depth1C_SceneController currentScene) : base(currentScene)
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


public class DepthC22_State_18 : Base_SceneState
    {
        public DepthC22_State_18(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class DepthC22_State_19 : Base_SceneState
    {
        public DepthC22_State_19(Depth1C_SceneController currentScene) : base(currentScene)
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
