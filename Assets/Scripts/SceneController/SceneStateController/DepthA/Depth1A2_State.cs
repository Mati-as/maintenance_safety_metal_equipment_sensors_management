public class DepthA2_State_1 : Base_SceneState
{
    public DepthA2_State_1(Depth1A_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {

        CurrentScene.contentController.ShutTrainingInfroAnim();
        
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.LimitSwitch,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.ProximitySwitch,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor,false);
        CurrentScene.SetHighlightIgnore((int)DepthAGameObj.TemperatureSensor_Whole,false);
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
        base.OnExit();
    }
}

public class DepthA2_State_2 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능

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
        CurrentScene.HighlightBlink((int)DepthAGameObj.LimitSwitch);
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
    {  CurrentScene.HighlightBlink((int)DepthAGameObj.ProximitySwitch);
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
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor).SetActive(false);
        CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor_Whole).SetActive(true);
        
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
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
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
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
    public DepthA2_State_8(Depth1A_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_9(Depth1A_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
        Base_SceneController.OnAnimationCompelete += OnAnimationComplete;


        CurrentScene.cameraController.isControllable = true;
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
        base.OnExit();
    }
}

public class DepthA2_State_10 : Base_SceneState
{
    public DepthA2_State_10(Depth1A_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
       
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
        
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        CurrentScene.contentController.OnDepth2Clicked(2);
        
        Managers.Scene.LoadScene(SceneType.Depth1C);
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
    public DepthA2_State_13(Depth1A_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_14(Depth1A_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_15(Depth1A_SceneController currentScene) : base(currentScene)
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
    public DepthA2_State_16(Depth1A_SceneController currentScene) : base(currentScene)
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