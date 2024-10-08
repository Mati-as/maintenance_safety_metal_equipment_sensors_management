public class Depth1C_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public Depth1C_State_1(Depth1C_SceneController currentScene) : base(currentScene)
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
        CurrentScene.contentController.ShutTrainingInfroAnim();
        base.OnExit();
    }
}

public class Depth1C_State_2 : Base_SceneState
{
    public Depth1C_State_2(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.ShutUIAndSetDefault();
        CurrentScene.contentController.ShowScriptUI();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class Depth1C_State_3 : Base_SceneState
{
    public Depth1C_State_3(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_4 : Base_SceneState
{
    public Depth1C_State_4(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_5 : Base_SceneState
{
    public Depth1C_State_5(Depth1C_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.cameraController.isControllable = true;
        // CurrentScene.cameraController.
        //     SetLookAtAndFollow(CurrentScene.GetObject((int)GameObj.LooAt_plumbingSystemOrPipework).transform);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.cameraController.isControllable = false;
        //CurrentScene.cameraController.SetLookAtAndFollow(null);
    }
}

public class Depth1C_State_6 : Base_SceneState
{
    public Depth1C_State_6(Depth1C_SceneController currentScene) : base(currentScene)
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
      
        // CurrentScene.cameraController.
        //     SetLookAtAndFollow(null);
    }
}

public class Depth1C_State_7 : Base_SceneState
{
    public Depth1C_State_7(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_8 : Base_SceneState
{
    public Depth1C_State_8(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_9 : Base_SceneState
{
    public Depth1C_State_9(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_10 : Base_SceneState
{
    public Depth1C_State_10(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_11 : Base_SceneState
{
    public Depth1C_State_11(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_12 : Base_SceneState
{
    public Depth1C_State_12(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_13 : Base_SceneState
{
    public Depth1C_State_13(Depth1C_SceneController currentScene) : base(currentScene)
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

public class Depth1C_State_14 : Base_SceneState
{
    public Depth1C_State_14(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C_State_15 : Base_SceneState
{
    public Depth1C_State_15(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C_State_16 : Base_SceneState
{
    public Depth1C_State_16(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C_State_17 : Base_SceneState
{
    public Depth1C_State_17(Depth1C_SceneController currentScene) : base(currentScene)
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


public class Depth1C_State_18 : Base_SceneState
    {
        public Depth1C_State_18(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_19 : Base_SceneState
    {
        public Depth1C_State_19(Depth1C_SceneController currentScene) : base(currentScene)
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

    public class Depth1C_State_20 : Base_SceneState
    {
        public Depth1C_State_20(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_21 : Base_SceneState
    {
        public Depth1C_State_21(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_22 : Base_SceneState
    {
        public Depth1C_State_22(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_23 : Base_SceneState
    {
        public Depth1C_State_23(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_24 : Base_SceneState
    {
        public Depth1C_State_24(Depth1C_SceneController currentScene) : base(currentScene)
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


    public class Depth1C_State_25 : Base_SceneState
    {
        public Depth1C_State_25(Depth1C_SceneController currentScene) : base(currentScene)
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
