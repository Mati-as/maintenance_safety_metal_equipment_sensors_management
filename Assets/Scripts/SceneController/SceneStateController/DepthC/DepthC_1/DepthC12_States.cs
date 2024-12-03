using System.Linq;

/// <summary>
///     3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
public class DepthC12_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_1(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
       
    }


    public override void OnEnter()
    {
        _depthC2SceneController.DepthC12Init();
        
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
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_2(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
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
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_3(DepthC1_SceneController currentScene) : base(currentScene) {_depthC2SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_4 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;
    public DepthC12_State_4(DepthC1_SceneController currentScene) : base(currentScene) {_depthC2SceneController = currentScene;}


    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_5 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_5(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}}

public class DepthC12_State_6 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_6(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

   
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_7 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_7(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_8 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_8(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

 
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC12_State_9 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_9(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

  
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_10 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_10(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_11 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_11(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }


    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC12_State_12 : Base_SceneState
{
    private readonly DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_12(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_13 : Base_SceneState
{
    public DepthC12_State_13(DepthC1_SceneController currentScene) : base(currentScene){}
  
    
    private readonly DepthC1_SceneController _depthC2SceneController;


    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC12_State_14 : Base_SceneState
{
    public DepthC12_State_14(DepthC1_SceneController currentScene) : base(currentScene){_depthC2SceneController = currentScene;}
    private readonly DepthC1_SceneController _depthC2SceneController;



 
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC12_State_15 : Base_SceneState
{
    private DepthC1_SceneController _depthC2SceneController;

    public DepthC12_State_15(DepthC1_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC12_State_16 : Base_SceneState
{
    public DepthC12_State_16(DepthC1_SceneController currentScene) : base(currentScene)
    {
    }

  
    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


