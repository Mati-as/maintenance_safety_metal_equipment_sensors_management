
/// <summary>
/// 3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
///
///
///
///  
public class Depth1C21_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public Depth1C21_State_1(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_2 : Base_SceneState
{
    public Depth1C21_State_2(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.ShutUIAndSetDefault();
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

public class Depth1C21_State_3 : Base_SceneState
{
    public Depth1C21_State_3(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_4 : Base_SceneState
{
    public Depth1C21_State_4(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_5 : Base_SceneState
{
    public Depth1C21_State_5(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_LockingScrew);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew, false);
      
        CurrentScene.contentController.isActionPerformable = true;

    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.contentController.isActionPerformable = false;
    }
}

public class Depth1C21_State_6 : Base_SceneState
{
    public Depth1C21_State_6(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.isActionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_ConnectionPiping, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_ConnectionPiping);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.contentController.isActionPerformable = false;
    }
}

public class Depth1C21_State_7 : Base_SceneState
{
    public Depth1C21_State_7(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_8 : Base_SceneState
{
    public Depth1C21_State_8(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.isActionPerformable = true;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC_GameObj.TS_Cover);
        
      
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.contentController.isActionPerformable = false;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover);
        
    }}


public class Depth1C21_State_9 : Base_SceneState
{
    public Depth1C21_State_9(Depth1C_SceneController currentScene) : base(currentScene)
    {
       
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC);
        
        CurrentScene.BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewB, "보상도선 점검");
        
        CurrentScene.contentController.isActionPerformable = true;
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
    
        CurrentScene.contentController.isActionPerformable = false;
    }
}

public class Depth1C21_State_10 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public Depth1C21_State_10(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
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

public class Depth1C21_State_11 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public Depth1C21_State_11(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        CurrentScene.BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        CurrentScene.contentController.isActionPerformable = true;
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);
        
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.ON, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.ON, false);
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


public class Depth1C21_State_12 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    
    public Depth1C21_State_12(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.ON, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.ON, false);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play($"ON", 0, 0);
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewB, "+ 프로브 접지");
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

public class Depth1C21_State_13 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public Depth1C21_State_13(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Depth1C_sceneController.isAnodePut = false;
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        
        CurrentScene.BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewB, "- 프로브 접지");
        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
       
        base.OnExit();
    }
}

public class Depth1C21_State_14 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public Depth1C21_State_14(Depth1C_SceneController currentScene) : base(currentScene)
    {
        Depth1C_sceneController = currentScene;
    }


    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB);
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(Depth1C_SceneController.ON, true);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(Depth1C_SceneController.ON, true);
        
        
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 1);
        Depth1C_sceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 1);
        
     
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


public class Depth1C21_State_15 : Base_SceneState
{
    Depth1C_SceneController Depth1C_sceneController;
    public Depth1C21_State_15(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C21_State_16 : Base_SceneState
{
    public Depth1C21_State_16(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C21_State_17 : Base_SceneState
{
    public Depth1C21_State_17(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C21_State_18 : Base_SceneState
    {
        public Depth1C21_State_18(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_19 : Base_SceneState
    {
        public Depth1C21_State_19(Depth1C_SceneController currentScene) : base(currentScene)
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

    public class Depth1C21_State_20 : Base_SceneState
    {
        public Depth1C21_State_20(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_21 : Base_SceneState
    {
        public Depth1C21_State_21(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_22 : Base_SceneState
    {
        public Depth1C21_State_22(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_23 : Base_SceneState
    {
        public Depth1C21_State_23(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_24 : Base_SceneState
    {
        public Depth1C21_State_24(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C21_State_25 : Base_SceneState
    {
        public Depth1C21_State_25(Depth1C_SceneController currentScene) : base(currentScene)
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
