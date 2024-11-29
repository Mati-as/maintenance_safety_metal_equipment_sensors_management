using System;

public class DepthA1_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public DepthA1_State_1(DepthA_SceneController currentScene) : base(currentScene)
    {
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

public class DepthA1_State_2 : Base_SceneState
{
    private DepthA_SceneController CurrentScene;
    public DepthA1_State_2(DepthA_SceneController currentScene) : base(currentScene)
    {
        CurrentScene = currentScene;
    }

    public override void OnEnter()
    {
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

public class DepthA1_State_3 : Base_SceneState
{
    public DepthA1_State_3(DepthA_SceneController currentScene) : base(currentScene)
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

public class DepthA1_State_4 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA1_State_4(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
        SetLookAt((int)DepthAGameObj.ProximitySwitch);
        base.OnAnimationCompleteHandler(_);
        
    }

    public override void OnEnter()
    {      
        
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;

      
        



        
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

public class DepthA1_State_5 : Base_SceneState
{
    private DepthA_SceneController _currentAScene;
    public DepthA1_State_5(DepthA_SceneController currentScene) : base(currentScene)
    {
        _currentAScene = currentScene;
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.FlowMeter);

    }


    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;

        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor_Whole);
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.HighlightBlink((int)DepthAGameObj.LevelSensor);
        CurrentScene.HighlightBlink((int)DepthAGameObj.FlowMeter);
        CurrentScene.HighlightBlink((int)DepthAGameObj.PressureSensor);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.cameraController.isControllable = false;
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class DepthA1_State_6 : Base_SceneState
{
    public DepthA1_State_6(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
       // CurrentScene.cameraController.SetCurrentMainAngleAndPos(CurrentScene.GetObject((int)DepthAGameObj.TemperatureSensor).transform);
      
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor_Whole);
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.HighlightBlink((int)DepthAGameObj.LevelSensor);
        CurrentScene.HighlightBlink((int)DepthAGameObj.FlowMeter);
        CurrentScene.HighlightBlink((int)DepthAGameObj.PressureSensor);
        // CurrentScene.cameraController.
        //     SetLookAtAndFollow(CurrentScene.GetObject((int)GameObj.LooAt_plumbingSystemOrPipework).transform);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        CurrentScene.cameraController.isControllable = false;
        base.OnExit();
        // CurrentScene.cameraController.
        //     SetLookAtAndFollow(null);
    }
}

public class DepthA1_State_7 : Base_SceneState
{
    public DepthA1_State_7(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor_Whole);
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

public class DepthA1_State_8 : Base_SceneState
{
    public DepthA1_State_8(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.HighlightBlink((int)DepthAGameObj.PressureSensor);
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

public class DepthA1_State_9 : Base_SceneState
{
    public DepthA1_State_9(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.HighlightBlink((int)DepthAGameObj.FlowMeter);
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

public class DepthA1_State_10 : Base_SceneState
{
    
    public DepthA1_State_10(DepthA_SceneController currentScene) : base(currentScene)
    {
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthAGameObj.LevelSensor);

    }
    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        CurrentScene.HighlightBlink((int)DepthAGameObj.LevelSensor);
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

public class DepthA1_State_11 : Base_SceneState
{
    public DepthA1_State_11(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        Logger.Log("뎁스전환 -> 1.2.1");
        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        CurrentScene.contentController.OnDepth2Clicked(2);
        CurrentScene.PlayAnimation(count:1);
        
        
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


// public class DepthA1_State_12 : Base_SceneState
// {
//     public DepthA1_State_12(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
// public class DepthA1_State_13 : Base_SceneState
// {
//     public DepthA1_State_13(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
// public class DepthA1_State_14 : Base_SceneState
// {
//     public DepthA1_State_14(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
//
// public class DepthA1_State_15 : Base_SceneState
// {
//     public DepthA1_State_15(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
//
// public class DepthA1_State_16 : Base_SceneState
// {
//     public DepthA1_State_16(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//         CurrentScene.GetObject((int)GameObj.TemperatureSensor).SetActive(false);
//         CurrentScene.GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(true);
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
//
// public class DepthA1_State_17 : Base_SceneState
// {
//     public DepthA1_State_17(DepthA1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//     public override void OnEnter()
//     {
//         base.OnEnter();
//         CurrentScene.cameraController.isControllable = false;
//         Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
//     }
//
//     public override void OnStep()
//     {
//     }
//
//     public override void OnExit()
//     {
//         base.OnExit();
//     }
// }
//
//
// public class DepthA1_State_18 : Base_SceneState
//     {
//         public DepthA1_State_18(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//             Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
//             Base_SceneController.OnAnimationCompelete += OnAnimationComplete;
//             
//             CurrentScene.GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(false);
//             CurrentScene.GetObject((int)GameObj.TemperatureSensor).SetActive(true);
//             
//         }
//
//         public override void OnStep()
//         {
//            
//         }
//
//         public override void OnExit()
//         {
//          
//             
//         }
//
//
//     }
//
//
//     public class DepthA1_State_19 : Base_SceneState
//     {
//         public DepthA1_State_19(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//             CurrentScene.cameraController.isControllable = false;
//             Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
//         }
//     }
//
//     public class DepthA1_State_20 : Base_SceneState
//     {
//         public DepthA1_State_20(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//             CurrentScene.cameraController.isControllable = false;
//             Base_SceneController.OnAnimationCompelete -= OnAnimationComplete;
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
//
//
//     public class DepthA1_State_21 : Base_SceneState
//     {
//         public DepthA1_State_21(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
//
//
//     public class DepthA1_State_22 : Base_SceneState
//     {
//         public DepthA1_State_22(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
//
//
//     public class DepthA1_State_23 : Base_SceneState
//     {
//         public DepthA1_State_23(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
//
//
//     public class DepthA1_State_24 : Base_SceneState
//     {
//         public DepthA1_State_24(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
//
//
//     public class DepthA1_State_25 : Base_SceneState
//     {
//         public DepthA1_State_25(DepthA1_SceneController currentScene) : base(currentScene)
//         {
//         }
//
//         public override void OnEnter()
//         {
//             base.OnEnter();
//         }
//
//         public override void OnStep()
//         {
//         }
//
//         public override void OnExit()
//         {
//             base.OnExit();
//         }
//     }
