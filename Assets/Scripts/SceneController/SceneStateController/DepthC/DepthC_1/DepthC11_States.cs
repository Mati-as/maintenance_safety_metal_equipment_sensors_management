

public class DepthC11_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_0(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }
        
    public override void OnEnter()
    {
        _depthC1SceneController.DepthC11Init();
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

public class DepthC11_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_1(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
       
    }
    

    public override void OnEnter()
    {
   
        CurrentScene.contentController.ShutTrainingIntroAnim();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
     
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



    public override void OnEnter()
    {
        base.OnEnter();

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC11_State_4 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;
    public DepthC11_State_4(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}

    
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC1_GameObj.LimitSwitchLookAt);
        
    }


    public override void OnEnter()
    {
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = true;
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
       
        base.OnEnter();
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.Lever_Handle);
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,false);
        CurrentScene.cameraController.isControllable = false;
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
       
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,true);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.Lever_Handle);
        
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
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,false);
        _depthC1SceneController.ClearTool();
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


    public override void OnEnter()
    {
        _depthC1SceneController.InitTransform(DepthC1_GameObj.LS_Cover);
        base.OnEnter();
    }
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


    public override void OnEnter()
    {
        _depthC1SceneController.InitTransform(DepthC1_GameObj.LS_Cover);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LS_Cover);
        base.OnEnter();
        
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }
}

public class DepthC11_State_10 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_10(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
      //  CurrentScene.cameraController.isControllable = true;
      _depthC1SceneController.animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].SetBool("Click", false);
      _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewA,"접속나사 확인");
      _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA, false);
      _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewA);
        
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
   //     CurrentScene.cameraController.isControllable = false;
   _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA);
        base.OnExit();
    }
}

public class DepthC11_State_11 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_11(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }



    public override void OnEnter()
    {
       
        base.OnEnter();
    }
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
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
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
        _depthC1SceneController.TakeDefaultMultimeter();
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        //depthC1SceneController.ClearTool();
        base.OnExit();
    }
}


public class DepthC11_State_15 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;

    public DepthC11_State_15(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.SetToResistantMode();
        _depthC1SceneController.multimeterController.isConductive = false;
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConductiveCheckModeBtn,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConductiveCheckModeBtn);
       
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConductiveCheckModeBtn);
        base.OnExit();
    }
}


public class DepthC11_State_16 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;
    public DepthC11_State_16(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.SetToResistantMode();
        _depthC1SceneController.multimeterController.isConductive = true; 

        
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewA);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewB);
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA,false);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB,false);
        
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewA,"접속단자 A");
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewB,"접속단자 B");
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB);
        _depthC1SceneController.multimeterController.isConductive = false; 
        _depthC1SceneController.isAnodePut = false;
        _depthC1SceneController.ClearTool();
        base.OnExit();
    }
}
public class DepthC11_State_17 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;
    public DepthC11_State_17(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.InitProbe();
        _depthC1SceneController.SetToResistantMode();
        _depthC1SceneController.multimeterController.isConductive = true;
        _depthC1SceneController.isAnodePut = false;
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewC,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewC);
        
        
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewC,"접속단자 C");
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewD,"접속단자 D");
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewC);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD);
        _depthC1SceneController.ClearTool();
        base.OnExit();
        
    }
}


public class DepthC11_State_18 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;
    public DepthC11_State_18(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.ClearTool();    
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 2;
        Managers.ContentInfo.PlayData.Count = 0;

        
        _depthC1SceneController.contentController.Refresh();
        _depthC1SceneController.OnDepth3Clicked();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}