public class DepthA2_State_1 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_1(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }

    public override void OnEnter()
    {

        
        
        base.OnEnter();
        _currentAScene.DepthA2Init();
        
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.TS_CompensatingWire, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.TS_Stabilizer, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.TS_SensingElement, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.TS_FixingScrew, false);
        
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.TS_Cover, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.LS_Roller, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.LS_Head, false);
        
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.PS_SensorParts, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.PS_AnaloguePart,  false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.PS_Housing, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.PS_Adapter, false);
        
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.FS_Adapter, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.FS_Housing, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.FS_SensorParts, false);
        
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.LvS_Housing, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.LvS_Display, false);
        _currentAScene.SetHighlightIgnore((int)DepthAGameObj.Lvs_SensorProbe, false);
        
        
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
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_2(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }
   

    public override void OnEnter()
    {
        CurrentScene.contentController.ShutTrainingIntroAnim();
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
        SetLookAt((int)DepthAGameObj.LookAt_LimitSwitch);
        isCurrentStateCameraControllable = true;
    }
    
    public override void OnEnter()
    {
        
        CurrentScene.BlinkHighlight((int)DepthAGameObj.LimitSwitch);
        base.OnEnter();
    }

    public override void OnStep()
    {
        base.OnStep();
       
    }

    public override void OnExit()
    {
      
        base.OnExit();
    }
}

public class DepthA2_State_4 : Base_SceneState
{
    
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_4(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LookAt_LimitSwitch);
        isCurrentStateCameraControllable = true;
    }
    
    public override void OnEnter()
    {
        _currentAScene.BlinkHighlight((int)DepthAGameObj.LS_Cover);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.LS_Roller);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.LS_Head);
        

        base.OnEnter();
    }

    public override void OnStep()
    {
        base.OnStep();
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}


public class DepthA2_State_5 : Base_SceneState
{
    
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_5(DepthA_SceneController currentScene) : base(currentScene)
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


public class DepthA2_State_6 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_6(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
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
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_7(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.TS_Stabilizer);
        isCurrentStateCameraControllable = true;
    }
    public override void OnEnter()
    {
     
        _currentAScene.BlinkHighlight((int)DepthAGameObj.TS_Cover);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.TS_CompensatingWire);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.TS_Stabilizer);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.TS_SensingElement);

       
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
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_9(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
        isCurrentStateCameraControllable = true;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LookAt_PressureSensor);
    }
    
    public override void OnEnter()
    {
        _currentAScene.BlinkHighlight((int)DepthAGameObj.PS_SensorParts);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.PS_AnaloguePart);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.PS_Housing);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.PS_Adapter);
        
   
        base.OnEnter();
    
    }

    public override void OnStep()
    {
        base.OnStep();
        isCurrentStateCameraControllable = true;
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor_Whole);
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
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_11(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.FlowSensor);
        isCurrentStateCameraControllable = true;
    }

    public override void OnEnter()

    {
        _currentAScene.BlinkHighlight((int)DepthAGameObj.FS_Adapter);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.FS_Housing);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.FS_SensorParts);
        
  
        base.OnEnter();
    }

    public override void OnStep()
    {
        base.OnStep();
        
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

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.FlowSensor);
    }
    public override void OnEnter()

    {
        base.OnEnter();
       
    }

    public override void OnStep()
    {
        base.OnStep();

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class DepthA2_State_13 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA2_State_13(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
        isCurrentStateCameraControllable = true;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LookAtPoint_LevelSensor);
    }


    public override void OnEnter()

    {
        _currentAScene.BlinkHighlight((int)DepthAGameObj.LvS_Housing);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.LvS_Display);
        _currentAScene.BlinkHighlight((int)DepthAGameObj.Lvs_SensorProbe);
        base.OnEnter();
    }

    public override void OnStep()
    {
        base.OnStep();
     
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

    protected override void OnAnimationCompleteHandler(int _)
    {
       
    }
    public override void OnEnter()

    {
        Managers.Scene.LoadScene(SceneType.DepthB);
       
    }

    public override void OnStep()
    {
        base.OnStep();

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

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LookAtPoint_LevelSensor);
    }
    public override void OnEnter()

    {
        base.OnEnter();
       
    }

    public override void OnStep()
    {
        base.OnStep();

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