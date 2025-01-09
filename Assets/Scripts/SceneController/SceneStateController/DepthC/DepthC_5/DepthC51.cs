
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC51_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_0(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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

public class DepthC51_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_1(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
       
    }


    public override void OnEnter()
    {

        _depthC5SceneController.DepthC51Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
     
        base.OnExit();
    }
}

public class DepthC51_State_2 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_2(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC51_State_3 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;
    protected override void OnAnimationCompleteHandler(int _)
    {
        
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC5_GameObj.LookAtPoint_LevelSensor);
        isCurrentStateCameraControllable = true;

    }

    public DepthC51_State_3(DepthC5_SceneController currentScene) : base(currentScene) {_depthC5SceneController = currentScene;}

    public override void OnEnter()
    {
        base.OnEnter();
    
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC51_State_4 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_4(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
        isCurrentStateCameraControllable = true;
    }



    public override void OnEnter()
    {
        base.OnEnter();
       
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC51_State_5 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_5(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensorConnectingScrew,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.LevelSensorConnectingScrew);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.LevelSensorConnectingScrew);
        base.OnExit();
    }
    
    
}

public class DepthC51_State_6 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_6(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ContaminatedRod,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ContaminatedRod);
      
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ContaminatedRod);
        base.OnExit();
    }

}




public class DepthC51_State_7 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_7(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
        
    }

    public override void OnEnter()
    {
        base.OnEnter();



        _depthC5SceneController.isWindSession = false;

        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC5SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC5SceneController.isScrewUnwindMap[key] = false;

        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB, false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);


        _depthC5SceneController.TurnOnCollidersAndInit();


        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB]
            .SetBool(DepthC2_SceneController.UNWIND, false);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
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


public class DepthC51_State_8 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_8(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
 
        
        _depthC5SceneController.isWindSession = false;
        
        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in _depthC5SceneController.isScrewUnwindMap.Keys.ToList())
            _depthC5SceneController.isScrewUnwindMap[key] = false;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);

    
        _depthC5SceneController.TurnOnCollidersAndInit();
        
        
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"UnScrew", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;

        base.OnEnter();
              
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC5SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);


        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB);
        base.OnExit();
    }
}


public class DepthC51_State_9 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC4SceneController;

    public DepthC51_State_9(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC51_State_10 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC4SceneController;

    public DepthC51_State_10(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }



    public override void OnEnter()
    {
     
        
        _depthC4SceneController.InitProbePos();
        _depthC4SceneController.isMultimeterOn = true;
        _depthC4SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC4SceneController.multimeterController.PS_SetToDefaultMode();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC5_GameObj.Multimeter;
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC5_GameObj.MultimeterHandleHighlight,false);
        _depthC4SceneController.BlinkHighlight((int)DepthC5_GameObj.MultimeterHandleHighlight);
      
        
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC4SceneController.SetHighlightIgnore((int)DepthC5_GameObj.MultimeterHandleHighlight);
        base.OnExit();
    }
}


public class DepthC51_State_11 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC4SceneController;

    public DepthC51_State_11(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC4SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC4SceneController.multimeterController.SetToCurrentModeAndRotation();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC5_GameObj.Multimeter;
        _depthC4SceneController.SetHighlightIgnore((int)DepthC5_GameObj.AnodeSensorOutput, false);
        _depthC4SceneController.BlinkHighlight((int)DepthC5_GameObj.AnodeSensorOutput);
        _depthC4SceneController.SetHighlightIgnore((int)DepthC5_GameObj.CathodeSensorInput, false);
        
        //Anode 입력시 Cathode Blink effect
       // _depthC4SceneController.BlinkHighlight((int)DepthC5_GameObj.CathodeSensorInput);
        
        _depthC4SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB);
        base.OnEnter();
        _depthC4SceneController.InitProbePos();
        _depthC4SceneController.isMultimeterOn = true;
        _depthC4SceneController.multimeterController.SetMeasureGuideStatus();
        _depthC4SceneController.multimeterController.SetToCurrentModeAndRotation();
        _depthC4SceneController.CurrentActiveTool = (int)DepthC5_GameObj.Multimeter;
        
        _depthC4SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}
public class DepthC51_State_12 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_12(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 5;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthC5SceneController.contentController.Refresh();
        _depthC5SceneController.DepthC52Init();
        
        base.OnEnter();
        _depthC5SceneController.PlayAnimation(1);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC51_State_13 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_13(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC51_State_14 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_14(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }



    public override void OnEnter()
    {
      
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        //depthC5SceneController.ClearTool();
        base.OnExit();
    }
}


public class DepthC51_State_15 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;

    public DepthC51_State_15(DepthC5_SceneController currentScene) : base(currentScene)
    {
      
    }


    public override void OnEnter()
    {

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}


public class DepthC51_State_16 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC51_State_16(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
;
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}
public class DepthC51_State_17 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC51_State_17(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
    
        base.OnExit();
    }
}


public class DepthC51_State_18 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC51_State_18(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.ClearTool();    
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 0;

        
        _depthC5SceneController.contentController.Refresh();
        _depthC5SceneController.OnDepth3Clicked();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}