
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC41_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_0(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
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
       
    }
}

public class DepthC41_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_1(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC4_GameObj.FlowSensor);
    }



    public override void OnEnter()
    {
        //카메라 시점변경On
       
        _depthC4SceneController.DepthC41Init();
        CurrentScene.contentController.ShutTrainingIntroAnim();
    
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

public class DepthC41_State_2 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;


    public DepthC41_State_2(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC4_GameObj.FlowSensor);
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
        isCurrentStateCameraControllable = true;
;
    }

    public override void OnStep()
    {
        base.OnStep();
    }
    public override void OnExit(){base.OnExit();}}

public class DepthC41_State_3 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_3(DepthC4_SceneController currentScene) : base(currentScene) {_depthC4SceneController = currentScene;}

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC4_GameObj.FlowSensor);
    }
    
    public override void OnEnter()
    { 
        base.OnEnter();
        isCurrentStateCameraControllable = true;

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC41_State_4 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;
    public DepthC41_State_4(DepthC4_SceneController currentScene) : base(currentScene) {_depthC4SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
        isCurrentStateCameraControllable = true;
    }

    public override void OnStep()
    {
        base.OnStep();
       
    }
    public override void OnExit(){base.OnExit();}
}

public class DepthC41_State_5 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_5(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowSensorConnectingScrew,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.FlowSensorConnectingScrew);
        
     
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowSensorConnectingScrew);
        base.OnExit();
    }}

public class DepthC41_State_6 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_6(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowSensorConnectingPipe,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.FlowSensorConnectingPipe);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowSensorConnectingPipe);
        base.OnExit();
    }
    
}



public class DepthC41_State_7 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_7(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
               
        _depthC4SceneController.isWindSession = false;
        
        foreach (var key in _depthC4SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC4SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC4SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC4SceneController.isScrewUnwindMap[key] = false;
        
    
        _depthC4SceneController.TurnOnCollidersAndInit();
        
        
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = true;
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Update(0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = false;
        _depthC4SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        _depthC4SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);

        
        CurrentScene.cameraController.isControllable = false;
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


public class DepthC41_State_8 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_8(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
 
        
        _depthC4SceneController.isWindSession = false;
        
        foreach (var key in _depthC4SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC4SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC4SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC4SceneController.isScrewUnwindMap[key] = false;
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ConnectionScrewB,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.ConnectionScrewB);

    
        _depthC4SceneController.TurnOnCollidersAndInit();
        
        
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = true;
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].Update(0);
        _depthC4SceneController.animatorMap[(int)DepthC4_GameObj.ConnectionScrewB].enabled = false;
        



        
        
        base.OnEnter();
        _depthC4SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ConnectionScrewB);
        base.OnExit();
    }
}

public class DepthC41_State_9 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_9(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
      
        base.OnEnter();
        _depthC4SceneController.ClearTool();
        _depthC4SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
   //     CurrentScene.cameraController.isControllable = false;
        base.OnExit();
    }
}

public class DepthC41_State_10 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_10(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }



    public override void OnEnter()
    {
     
        
        _depthC4SceneController.InitProbePos();
        _depthC4SceneController.isMultimeterOn = true;
        _depthC4SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC4SceneController.multimeterController.PS_SetToDefaultMode();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC4_GameObj.Multimeter;
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.MultimeterHandleHighlight,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.MultimeterHandleHighlight);
      
        
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.MultimeterHandleHighlight);
        base.OnExit();
    }
}


public class DepthC41_State_11 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_11(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC4SceneController.multimeterController.SetToCurrentModeAndRotation();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC4_GameObj.Multimeter;
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.AnodeSensorOutput, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.AnodeSensorOutput);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.CathodeSensorInput, false);
        
        //Anode 입력시 Cathode Blink effect
       // _depthC4SceneController.BlinkHighlight((int)DepthC4_GameObj.CathodeSensorInput);
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ConnectionScrewB);
        base.OnEnter();
        _depthC4SceneController.InitProbePos();
        _depthC4SceneController.isMultimeterOn = true;
        _depthC4SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC4SceneController.multimeterController.SetToCurrentModeAndRotation();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC4_GameObj.Multimeter;
        
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC41_State_12 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC4SceneController;

    public DepthC41_State_12(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 4;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthC4SceneController.contentController.Refresh();
        _depthC4SceneController.DepthC42Init();
        
        base.OnEnter();
        _depthC4SceneController.PlayAnimation(1);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}



// public class DepthC41_State_13 : Base_SceneState
// {
//     private readonly DepthC4_SceneController _depthC4SceneController;
//
//     public DepthC41_State_13(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//         _depthC4SceneController = currentScene;
//     }
//
//
//
//     public override void OnEnter()
//     {
//       
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//         //depthC4SceneController.ClearTool();
//         base.OnExit();
//     }
// }

// public class DepthC41_State_14 : Base_SceneState
// {
//     private readonly DepthC4_SceneController _depthC4SceneController;
//
//     public DepthC41_State_14(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//         _depthC4SceneController = currentScene;
//     }
//
//
//
//     public override void OnEnter()
//     {
//       
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//         //depthC4SceneController.ClearTool();
//         base.OnExit();
//     }
// }
//
//
// public class DepthC41_State_15 : Base_SceneState
// {
//     private DepthC4_SceneController _depthC4SceneController;
//
//     public DepthC41_State_15(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//       
//     }
//
//
//     public override void OnEnter()
//     {
//
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//
//         base.OnExit();
//     }
// }
//
//
// public class DepthC41_State_16 : Base_SceneState
// {
//     private DepthC4_SceneController _depthC4SceneController;
//     public DepthC41_State_16(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//         _depthC4SceneController = currentScene;
//     }
//
//
//     public override void OnEnter()
//     {
// ;
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//
//         base.OnExit();
//     }
// }
// public class DepthC41_State_17 : Base_SceneState
// {
//     private DepthC4_SceneController _depthC4SceneController;
//     public DepthC41_State_17(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//         _depthC4SceneController = currentScene;
//     }
//
//
//     public override void OnEnter()
//     {
//
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//     
//         base.OnExit();
//     }
// }
//
//
// public class DepthC41_State_18 : Base_SceneState
// {
//     private DepthC4_SceneController _depthC4SceneController;
//     public DepthC41_State_18(DepthC4_SceneController currentScene) : base(currentScene)
//     {
//         _depthC4SceneController = currentScene;
//     }
//
//
//     public override void OnEnter()
//     {
//         _depthC4SceneController.ClearTool();    
//         Managers.ContentInfo.PlayData.Depth1 = 3;
//         Managers.ContentInfo.PlayData.Depth2 = 1;
//         Managers.ContentInfo.PlayData.Depth3 = 2;
//         Managers.ContentInfo.PlayData.Count = 0;
//
//         
//         _depthC4SceneController.contentController.Refresh();
//         _depthC4SceneController.OnDepth3Clicked();
//         base.OnEnter();
//     }
//     public override void OnStep(){base.OnStep();}
//
//     public override void OnExit()
//     {
//
//         base.OnExit();
//     }
// }