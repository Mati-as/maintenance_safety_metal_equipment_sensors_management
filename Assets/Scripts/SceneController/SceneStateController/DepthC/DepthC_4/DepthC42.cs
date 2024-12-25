
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC42_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_0(DepthC4_SceneController currentScene) : base(currentScene)
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

public class DepthC42_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_1(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
       
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        SetLookAt((int)DepthC4_GameObj.FlowSensor);
        base.OnAnimationCompleteHandler(_);
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        SetLookAt((int)DepthC4_GameObj.FlowSensor);
        
        
        _depthC3SceneController.DepthC42Init();
        CurrentScene.contentController.ShutTrainingIntroAnim();
    
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        CurrentScene.cameraController.isControllable = false;
        base.OnExit();
    }
}

public class DepthC42_State_2 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_2(DepthC4_SceneController currentScene) : base(currentScene)
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

public class DepthC42_State_3 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_3(DepthC4_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC42_State_4 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;
    public DepthC42_State_4(DepthC4_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}

    public override void OnEnter()
    {
        
        _depthC3SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ResidueBlockingPiping,false);
        _depthC3SceneController.BlinkHighlight((int)DepthC4_GameObj.ResidueBlockingPiping);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC4_GameObj.ResidueBlockingPiping);
        base.OnExit();
    }
    
}


public class DepthC42_State_5 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_5(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }}

public class DepthC42_State_6 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_6(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }
}



public class DepthC42_State_7 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_7(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowerSensor_Valve,false);
        _depthC3SceneController.BlinkHighlight((int)DepthC4_GameObj.FlowerSensor_Valve);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC4_GameObj.FlowerSensor_Valve);
        base.OnExit();
    }
}

public class DepthC42_State_8 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_8(DepthC4_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 4;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthC3SceneController.contentController.Refresh();
        _depthC3SceneController.DepthC42Init();
        
        base.OnEnter();
        _depthC3SceneController.PlayAnimation(1);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC42_State_9 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_9(DepthC4_SceneController currentScene) : base(currentScene)
    {

    }


    public override void OnEnter()
    {
 

    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }
}

public class DepthC42_State_10 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_10(DepthC4_SceneController currentScene) : base(currentScene)
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

public class DepthC42_State_11 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_11(DepthC4_SceneController currentScene) : base(currentScene)
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


public class DepthC42_State_12 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_12(DepthC4_SceneController currentScene) : base(currentScene)
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

public class DepthC42_State_13 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_13(DepthC4_SceneController currentScene) : base(currentScene)
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

public class DepthC42_State_14 : Base_SceneState
{
    private readonly DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_14(DepthC4_SceneController currentScene) : base(currentScene)
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


public class DepthC42_State_15 : Base_SceneState
{
    private DepthC4_SceneController _depthC3SceneController;

    public DepthC42_State_15(DepthC4_SceneController currentScene) : base(currentScene)
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


public class DepthC42_State_16 : Base_SceneState
{
    private DepthC4_SceneController _depthC3SceneController;
    public DepthC42_State_16(DepthC4_SceneController currentScene) : base(currentScene)
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
public class DepthC42_State_17 : Base_SceneState
{
    private DepthC4_SceneController _depthC3SceneController;
    public DepthC42_State_17(DepthC4_SceneController currentScene) : base(currentScene)
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


public class DepthC42_State_18 : Base_SceneState
{
    private DepthC4_SceneController _depthC3SceneController;
    public DepthC42_State_18(DepthC4_SceneController currentScene) : base(currentScene)
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