public class DepthA2_State_1 : Base_SceneState
{
    public DepthA2_State_1(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {

       
        
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.LimitSwitch,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.ProximitySwitch,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor_Whole);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.LevelSensor,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.FlowMeter,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.PressureSensor,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TS_CompensatingWire,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TS_Stabilizer,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TS_SensingElement,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TS_Cover,false);
        
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.contentController.ShutTrainingIntroAnim();
        base.OnExit();
    }
}

public class DepthA2_State_2 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_2(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
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
    public DepthA2_State_3(DepthA_SceneController currentScene) : base(currentScene)
    {
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LimitSwitch);
    }
    
    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        
        CurrentScene.BlinkHighlight((int)DepthAGameObj.LimitSwitch);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class DepthA2_State_4 : Base_SceneState
{
    public DepthA2_State_4(DepthA_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_5(DepthA_SceneController currentScene) : base(currentScene)
    {
    }


    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.ProximitySwitch);
    }
    
    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        CurrentScene.BlinkHighlight((int)DepthAGameObj.ProximitySwitch);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}


public class DepthA2_State_6 : Base_SceneState
{
    public DepthA2_State_6(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor).SetActive(false);
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor_Whole).SetActive(true);
        
        CurrentScene.BlinkHighlight((int)DepthAGameObj.TemperatureSensor);
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
    public DepthA2_State_7(DepthA_SceneController currentScene) : base(currentScene)
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


public class DepthA2_State_8 : Base_SceneState
{
    public DepthA2_State_8(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor_Whole).SetActive(false);
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor).SetActive(true);
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


public class DepthA2_State_9 : Base_SceneState
{
    public DepthA2_State_9(DepthA_SceneController currentScene) : base(currentScene)
    {
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.TemperatureSensor);
    }
    
    public override void OnEnter()
    {
       // CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor_Whole);
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        base.OnEnter();
    
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor_Whole);
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class DepthA2_State_10 : Base_SceneState
{
    public DepthA2_State_10(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
       
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        
        // Managers.ContentInfo.PlayData.Depth1 = 3;
        // Managers.ContentInfo.PlayData.Depth2 = 2;
        // Managers.ContentInfo.PlayData.Depth3 = 1;
        // Managers.ContentInfo.PlayData.Count = 1;
        // CurrentScene.contentController.OnDepth2Clicked(2);
        
        // Managers.Scene.LoadScene(SceneType.DepthC2);
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


public class DepthA2_State_11 : Base_SceneState
{
    public DepthA2_State_11(DepthA_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_12(DepthA_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_13 : Base_SceneState
{
    public DepthA2_State_13(DepthA_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_14 : Base_SceneState
{
    public DepthA2_State_14(DepthA_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_15 : Base_SceneState
{
    public DepthA2_State_15(DepthA_SceneController currentScene) : base(currentScene)
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

public class DepthA2_State_16 : Base_SceneState
{
    public DepthA2_State_16(DepthA_SceneController currentScene) : base(currentScene)
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