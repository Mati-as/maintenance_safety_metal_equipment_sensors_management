

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>
public class DepthC32_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_0(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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

public class DepthC32_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_1(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC3_GameObj.PressureSensor);
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        _depthC3SceneController.DepthC32Init();
    
        
        CurrentScene.cameraController.isControllable = true;
        CurrentScene.contentController.ShutTrainingIntroAnim();
    
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        CurrentScene.cameraController.isControllable = false;
        base.OnExit();
    }
}

public class DepthC32_State_2 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_2(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC32_State_3 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_3(DepthC3_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC32_State_4 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;
    public DepthC32_State_4(DepthC3_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC32_State_5 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_5(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorDamagedPart,false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PressureSensorDamagedPart);
        _depthC3SceneController.cameraController.isControllable = true;
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorDamagedPart);
        base.OnExit();
    }}

public class DepthC32_State_6 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_6(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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

public class DepthC32_State_7 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_7(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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

public class DepthC32_State_8 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_8(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC32_State_9 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_9(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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

public class DepthC32_State_10 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_10(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
      
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
   //     CurrentScene.cameraController.isControllable = false;
        base.OnExit();
    }
}

public class DepthC32_State_11 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_11(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }



    public override void OnEnter()
    {
       
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC32_State_12 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_12(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC32_State_13 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_13(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC3SceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC32_State_14 : Base_SceneState
{
    private readonly DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_14(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }



    public override void OnEnter()
    {
      
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        //depthC3SceneController.ClearTool();
        base.OnExit();
    }
}


public class DepthC32_State_15 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;

    public DepthC32_State_15(DepthC3_SceneController currentScene) : base(currentScene)
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


public class DepthC32_State_16 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC32_State_16(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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
public class DepthC32_State_17 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC32_State_17(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
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


public class DepthC32_State_18 : Base_SceneState
{
    private DepthC3_SceneController _depthC3SceneController;
    public DepthC32_State_18(DepthC3_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC3SceneController.ClearTool();    
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 0;

        
        _depthC3SceneController.contentController.Refresh();
        _depthC3SceneController.OnDepth3Clicked();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}