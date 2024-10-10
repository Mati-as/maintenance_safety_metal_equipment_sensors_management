public class Depth1C21_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public Depth1C21_State_1(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.contentController.ShutTrainingInfroAnim();
        
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
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew, false);
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
        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        
    }
}

public class Depth1C21_State_7 : Base_SceneState
{
    public Depth1C21_State_7(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
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
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class Depth1C21_State_9 : Base_SceneState
{
    public Depth1C21_State_9(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_10 : Base_SceneState
{
    public Depth1C21_State_10(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_11 : Base_SceneState
{
    public Depth1C21_State_11(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_12 : Base_SceneState
{
    public Depth1C21_State_12(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_13 : Base_SceneState
{
    public Depth1C21_State_13(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C21_State_14 : Base_SceneState
{
    public Depth1C21_State_14(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C21_State_15 : Base_SceneState
{
    public Depth1C21_State_15(Depth1C_SceneController currentScene) : base(currentScene)
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
