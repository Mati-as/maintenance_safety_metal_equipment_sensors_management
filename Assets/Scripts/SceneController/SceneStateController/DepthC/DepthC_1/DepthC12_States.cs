using System.Linq;

/// <summary>
///     3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
public class DepthC12_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
private readonly DepthC1_SceneController _depthC1SceneController;

public DepthC12_State_0(DepthC1_SceneController currentScene) : base(currentScene)
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
public class DepthC12_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_1(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
       
    }


    public override void OnEnter()
    {
        _depthC1SceneController.DepthC12Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
       
        base.OnExit();
    }
}

public class DepthC12_State_2 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_2(DepthC1_SceneController currentScene) : base(currentScene)
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

public class DepthC12_State_3 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_3(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}

    public override void OnEnter()
    {
        
      
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LS_Cover);
        
        base.OnEnter();
        
        _depthC1SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover);
        base.OnExit();
    }
}

public class DepthC12_State_4 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;
    public DepthC12_State_4(DepthC1_SceneController currentScene) : base(currentScene) {_depthC1SceneController = currentScene;}


    public override void OnEnter()
    {

        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
       
        base.OnEnter();
        
        _depthC1SceneController.cameraController.isControllable = false;
        

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_5 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_5(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    public override void OnEnter()
    {
       
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(false);
        _depthC1SceneController.SetScrewStatus(false);
      
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        
        _depthC1SceneController.TurnOffAllRegisteredHighlights();
     
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LeverScrew);

        _depthC1SceneController.cameraController.isControllable = false;
        
        _depthC1SceneController.CurrentActiveTool = (int)DepthC1_GameObj.ElectricScrewdriver;
        _depthC1SceneController._isDriverOn = true;
        CurrentScene.cameraController.isControllable = false;
        base.OnEnter();
    
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.ClearTool();
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,false);
        base.OnExit();
    }}

public class DepthC12_State_6 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_6(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC1SceneController.ClearTool();
        _depthC1SceneController.limitSwitchPivotController.isTargetPosEventInvoked = false;
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(true);
        
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = false;

        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle,false); 
        _depthC1SceneController.SetCollider(DepthC1_GameObj.Lever_Handle,false);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos);
 
        _depthC1SceneController.contentController.isStepMissionComplete = false;

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.Lever_Handle);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LeverHandleReadjustTargetPos);
        _depthC1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(false);
        _depthC1SceneController.ClearTool();
        base.OnExit();
        _depthC1SceneController.SetCollider(DepthC1_GameObj.LimitSwitch,false);
    }
}

public class DepthC12_State_7 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_7(DepthC1_SceneController currentScene) : base(currentScene)
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

public class DepthC12_State_8 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_8(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
       
        base.OnEnter();
        _depthC1SceneController.InitTransform(DepthC1_GameObj.LS_Cover);
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LS_Cover);
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = false;
    
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover);
        base.OnExit();
    }
}


public class DepthC12_State_9 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_9(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }

    

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_LimitSwitch);
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = false;
        
      
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }
}


public class DepthC12_State_10 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_10(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 1;
        Logger.Log($"CurrentStatus on State 14 -----> {Managers.ContentInfo.PlayData.CurrentDepthStatus} ");
        _depthC1SceneController.StopAllCoroutines();

        _depthC1SceneController.PlayAnimation(1);
        _depthC1SceneController.contentController.OnDepth3BtnClicked(3);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}





//----------------------------------------------------------------------------
public class DepthC12_State_11 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;

    public DepthC12_State_11(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }



    public override void OnEnter()
    {
        _depthC1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        _depthC1SceneController.BlinkHighlight((int)DepthC1_GameObj.LS_Cover);
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC12_State_12 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC1SceneController;
    protected override void OnAnimationCompleteHandler(int _)
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 0;
        
        _depthC1SceneController.OnDepth3Clicked();
        _depthC1SceneController.contentController.Refresh();
        
        Logger.Log($"애니메이션 Compelete 이벤트 Current Status in State13 {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        
    }
    public DepthC12_State_12(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC1SceneController = currentScene;
    }


    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_LimitSwitch);
        base.OnEnter();
        _depthC1SceneController.cameraController.isControllable = false;
        
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class DepthC12_State_13 : Base_SceneState
{
    public DepthC12_State_13(DepthC1_SceneController currentScene) :base(currentScene){_depthC1SceneController = currentScene;}
  
    
    private readonly DepthC1_SceneController _depthC1SceneController;



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




public class DepthC12_State_14 : Base_SceneState
{
    public DepthC12_State_14(DepthC1_SceneController currentScene) :base(currentScene){_depthC1SceneController = currentScene;}
    private readonly DepthC1_SceneController _depthC1SceneController;




    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 0;
        Logger.Log($"CurrentStatus on State 14 -----> {Managers.ContentInfo.PlayData.CurrentDepthStatus} ");
        _depthC1SceneController.StopAllCoroutines();
  

        _depthC1SceneController.PlayAnimation(1);
       _depthC1SceneController.contentController.OnDepth3BtnClicked(3);
       

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

//
// public class DepthC12_State_15 : Base_SceneState
// {
//     private DepthC1_SceneController _depthC1SceneController;
//
//     public DepthC12_State_15(DepthC1_SceneController currentScene) : base(currentScene)
//     {
//         _depthC1SceneController = currentScene;
//     }
//
//     
//     public override void OnEnter(){base.OnEnter();}
//     public override void OnStep(){base.OnStep();}
//     public override void OnExit(){base.OnExit();}
// }
//
//
// public class DepthC12_State_16 : Base_SceneState
// {
//     public DepthC12_State_16(DepthC1_SceneController currentScene) : base(currentScene)
//     {
//     }
//
//   
//     public override void OnEnter(){base.OnEnter();}
//     public override void OnStep(){base.OnStep();}
//     public override void OnExit(){base.OnExit();}
// }
//
//
