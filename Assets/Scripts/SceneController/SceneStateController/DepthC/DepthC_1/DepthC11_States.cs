using System.Linq;

/// <summary>
///     3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
public class DepthC11_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_1(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC1_GameObj.LimitSwitch);
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        _depthC1SceneController.DepthC11Init();
        CurrentScene.contentController.ShutTrainingIntroAnim();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        CurrentScene.cameraController.isControllable = true;
        base.OnExit();
    }
}

public class DepthC11_State_2 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_2(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.SetScriptUI();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC11_State_3 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_3(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_4 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;
    public DepthC11_State_4(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_5 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_5(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC11_State_6 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_6(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        Logger.Log($"highlight{DepthC1_GameObj.Lever_Handle}");
        _depthC1SceneController.HighlightBlink((int)DepthC1_GameObj.Lever_Handle);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,false);
     
        base.OnEnter();
    }

    public override void OnStep()
    {
        _depthC1SceneController.contentController.HideCamInitBtn();
        CurrentScene.cameraController.isControllable = false;
        base.OnStep();
       
    }

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,true);
        base.OnExit();
    }
}

public class DepthC11_State_7 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_7(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
       
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,false);
        _depthC1SceneController.HighlightBlink((int)DepthC1_GameObj.Lever_Handle);
        base.OnEnter();
    }

    public override void OnStep()
    {
        _depthC1SceneController.contentController.HideCamInitBtn();
        CurrentScene.cameraController.isControllable = false;
        _depthC1SceneController.limitSwitchPivotController.InitLeverRotation();
        base.OnStep();
    }

    public override void OnExit()
    {
        
        _depthC1SceneController.limitSwitchPivotController.InitLeverRotation();
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,true);
        base.OnExit();
    }
}

public class DepthC11_State_8 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_8(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

 
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC11_State_9 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_9(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

  
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_10 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_10(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_11 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_11(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC11_State_12 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_12(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_13 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_13(DepthC1_SceneController currentScene) : base(currentScene)
    {
      
    }


    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_14 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_14(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }



    public override void OnEnter()
    {

        base.OnEnter();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC11_State_15 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_15(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC11_State_16 : Base_SceneState
{
    public DepthC11_State_16(DepthC1_SceneController currentScene) : base(currentScene)
    {
    }

  
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


