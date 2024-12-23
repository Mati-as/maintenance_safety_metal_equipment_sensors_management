
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC52_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_0(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_1(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        
        base.OnAnimationCompleteHandler(_);
        
    }

    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        SetLookAt((int)DepthC3_GameObj.PressureSensor);
        
        CurrentScene.cameraController.isControllable = true;
       // _depthC3SceneController.DepthC52Init();
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

public class DepthC52_State_2 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_2(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_3 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_3(DepthC5_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC52_State_4 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;
    public DepthC52_State_4(DepthC5_SceneController currentScene) : base(currentScene) {_depthC3SceneController = currentScene;}


    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC52_State_5 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_5(DepthC5_SceneController currentScene) : base(currentScene)
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
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorConnectingScrew);
        base.OnExit();
    }}

public class DepthC52_State_6 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_6(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorConnectingPipe,false);
        _depthC3SceneController.BlinkHighlight((int)DepthC3_GameObj.PressureSensorConnectingPipe);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC3SceneController.SetHighlightIgnore((int)DepthC3_GameObj.PressureSensorConnectingPipe);
        base.OnExit();
    }
    
}



public class DepthC52_State_7 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_7(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }

    public override void OnEnter()
    {
        base.OnEnter();
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

public class DepthC52_State_8 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_8(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC3SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC3SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
        CurrentScene.cameraController.isControllable = false;
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC52_State_9 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_9(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_10 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_10(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_11 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_11(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_12 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_12(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_13 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_13(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_14 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_14(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_15 : Base_SceneState
{
    private DepthC5_SceneController _depthC3SceneController;

    public DepthC52_State_15(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_16 : Base_SceneState
{
    private DepthC5_SceneController _depthC3SceneController;
    public DepthC52_State_16(DepthC5_SceneController currentScene) : base(currentScene)
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
public class DepthC52_State_17 : Base_SceneState
{
    private DepthC5_SceneController _depthC3SceneController;
    public DepthC52_State_17(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_18 : Base_SceneState
{
    private DepthC5_SceneController _depthC3SceneController;
    public DepthC52_State_18(DepthC5_SceneController currentScene) : base(currentScene)
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