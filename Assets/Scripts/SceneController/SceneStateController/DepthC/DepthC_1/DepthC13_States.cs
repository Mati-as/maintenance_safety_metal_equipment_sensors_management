using System.Linq;

/// <summary>
///     3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
 public class DepthC13_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
private readonly DepthC1_SceneController _depthC1SceneController;

public DepthC13_State_0(DepthC1_SceneController currentScene) : base(currentScene)
{
    _depthC1SceneController = currentScene;
       
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


public class DepthC13_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_1(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
       
    }


    public override void OnEnter()
    {
        _depthC1SceneController.DepthC13Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
       
        base.OnExit();
    }
}

public class DepthC13_State_2 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_2(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_LimitSwitch);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC13_State_3 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_3(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}

    public override void OnEnter()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.PowerHandle,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.PowerHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.PowerHandle);
        base.OnExit();
    }
}

public class DepthC13_State_4 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;
    public DepthC13_State_4(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}


    public override void OnEnter()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LS_Cover);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover);
        base.OnExit();
    }
}

public class DepthC13_State_5 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_5(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC1SceneController.isWindSession = false;
        _depthC1SceneController.SetScrewStatus(false);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.ClearTool();
        base.OnExit();
    }}

public class DepthC13_State_6 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_6(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(true);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LimitSwitch,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LimitSwitch);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    { 
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LimitSwitch);
        base.OnExit();
    }
}

public class DepthC13_State_7 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_7(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC1SceneController.ClearTool();
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(false);

        _depthC1SceneController.isLeverScrewUnwound = false;
        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC1SceneController.isWindSession = false;
        
        _depthC1SceneController.InitTransform(DepthC1_GameObj.Lever_Handle);
     
        _depthC1SceneController.isScrewUnwindMap[(int)DepthC1_GameObj.LeverScrew] = false;
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LeverScrew,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LeverScrew);

  
        
        base.OnEnter();
        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LeverHandleReadjustTargetPos);
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(false);
        _depthC1SceneController.ClearTool();
        base.OnExit();
    }
  
}

public class DepthC13_State_8 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_8(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC1SceneController.isWindSession = true;
        _depthC1SceneController.SetScrewStatus(true);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.ClearTool();
        base.OnExit();
    }
}


public class DepthC13_State_9 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_9(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
         _depthC1SceneController.ClearTool();
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC13_State_10 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_10(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC1SceneController.TakeDefaultMultimeter();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC13_State_11 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_11(DepthC1_SceneController currentScene) : base(currentScene)
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
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC13_State_12 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_12(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC1SceneController.SetToResistantMode();
        _depthC1SceneController.multimeterController.isConductive = true; 
        _depthC1SceneController.isAnodePut = false;

        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewA);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewB);
        
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA,false);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB,false);
        
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewA,"접속단자 A");
        _depthC1SceneController.ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewB,"접속단자 B");
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC13_State_13 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_13(DepthC1_SceneController currentScene) : base(currentScene)
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
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
       
        base.OnExit();
    }
}

public class DepthC13_State_14 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_14(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }



    public override void OnEnter()
    {

        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewC);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD);
        _depthC1SceneController.ClearTool();
        CurrentScene.PlayAnimation(0);

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}


public class DepthC13_State_15 : Base_SceneState
{
    private DepthC1_SceneController _depthC1SceneController;

    public DepthC13_State_15(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC13_State_16 : Base_SceneState
{
    public DepthC13_State_16(DepthC1_SceneController currentScene) : base(currentScene)
    {
    }

  
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


