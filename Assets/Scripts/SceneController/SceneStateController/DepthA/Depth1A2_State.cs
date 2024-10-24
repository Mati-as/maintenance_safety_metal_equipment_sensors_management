
public class DepthA2_State_1 : Base_SceneState
{
    public DepthA2_State_1(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_2 : Base_SceneState
{
    public DepthA2_State_2(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_3 : Base_SceneState
{
    public DepthA2_State_3(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_4 : Base_SceneState
{
    public DepthA2_State_4(Depth1A_SceneController currentScene) : base(currentScene)
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


public class DepthA2_State_5 : Base_SceneState
{
    public DepthA2_State_5(Depth1A_SceneController currentScene) : base(currentScene)
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


public class DepthA2_State_6 : Base_SceneState
{
    public DepthA2_State_6(Depth1A_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.GetObject((int)GameObj.TemperatureSensor).SetActive(false);
        CurrentScene.GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(true);
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthA2_State_7 : Base_SceneState
{
    public DepthA2_State_7(Depth1A_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthA2_State_8 : Base_SceneState
    {
        public DepthA2_State_8(Depth1A_SceneController currentScene) : base(currentScene)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
            Base_SceneController.OnAnimationCompelete += OnAnimationComplete;
            
            CurrentScene.GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(false);
            CurrentScene.GetObject((int)GameObj.TemperatureSensor).SetActive(true);
            
        }

        public override void OnStep()
        {
           
        }

        public override void OnExit()
        {
         
            
        }


    }


    public class DepthA2_State_9 : Base_SceneState
    {
        public DepthA2_State_9(Depth1A_SceneController currentScene) : base(currentScene)
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
            CurrentScene.cameraController.isControllable = false;
            Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
        }
    }

    public class DepthA2_State_10 : Base_SceneState
    {
        public DepthA2_State_10(Depth1A_SceneController currentScene) : base(currentScene)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrentScene.cameraController.isControllable = false;
            Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
        }

        public override void OnStep()
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }


public class DepthA2_State_11 : Base_SceneState
{
    public DepthA2_State_11(Depth1A_SceneController currentScene) : base(currentScene)
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




public class DepthA2_State_12 : Base_SceneState
{
    public DepthA2_State_12(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_13 : Base_SceneState
{
    public DepthA2_State_13(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_14 : Base_SceneState
{
    public DepthA2_State_14(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_15 : Base_SceneState
{
    public DepthA2_State_15(Depth1A_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_16 : Base_SceneState
{
    public DepthA2_State_16(Depth1A_SceneController currentScene) : base(currentScene)
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