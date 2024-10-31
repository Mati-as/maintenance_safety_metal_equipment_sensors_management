public class Depth1A_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public Depth1A_State_1(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_2 : Base_SceneState
{
    private DepthA_SceneController CurrentScene;
    public Depth1A_State_2(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_3 : Base_SceneState
{
    public Depth1A_State_3(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_4 : Base_SceneState
{
    public Depth1A_State_4(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_5 : Base_SceneState
{
    public Depth1A_State_5(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_6 : Base_SceneState
{
    public Depth1A_State_6(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.cameraController.isControllable = true;
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

public class Depth1A_State_7 : Base_SceneState
{
    public Depth1A_State_7(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_8 : Base_SceneState
{
    public Depth1A_State_8(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_9 : Base_SceneState
{
    public Depth1A_State_9(DepthA_SceneController currentScene) : base(currentScene)
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

public class Depth1A_State_10 : Base_SceneState
{
    public Depth1A_State_10(DepthA_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        CurrentScene.HighlightBlink((int)DepthAGameObj.LevelSensor);
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

public class Depth1A_State_11 : Base_SceneState
{
    public Depth1A_State_11(DepthA_SceneController currentScene) : base(currentScene)
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


// public class Depth1A_State_12 : Base_SceneState
// {
//     public Depth1A_State_12(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_13 : Base_SceneState
// {
//     public Depth1A_State_13(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_14 : Base_SceneState
// {
//     public Depth1A_State_14(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_15 : Base_SceneState
// {
//     public Depth1A_State_15(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_16 : Base_SceneState
// {
//     public Depth1A_State_16(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_17 : Base_SceneState
// {
//     public Depth1A_State_17(Depth1A_SceneController currentScene) : base(currentScene)
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
// public class Depth1A_State_18 : Base_SceneState
//     {
//         public Depth1A_State_18(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_19 : Base_SceneState
//     {
//         public Depth1A_State_19(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_20 : Base_SceneState
//     {
//         public Depth1A_State_20(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_21 : Base_SceneState
//     {
//         public Depth1A_State_21(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_22 : Base_SceneState
//     {
//         public Depth1A_State_22(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_23 : Base_SceneState
//     {
//         public Depth1A_State_23(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_24 : Base_SceneState
//     {
//         public Depth1A_State_24(Depth1A_SceneController currentScene) : base(currentScene)
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
//     public class Depth1A_State_25 : Base_SceneState
//     {
//         public Depth1A_State_25(Depth1A_SceneController currentScene) : base(currentScene)
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
