
using System.Linq;

/// <summary>
/// 압력센서 관련 States 입니다 ---------------------------------------------------
/// </summary>

public class DepthC52_State_0 : Base_SceneState
{
// 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_0(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_1(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
       
    }

    protected override void OnAnimationCompleteHandler(int _)
    {
        
        base.OnAnimationCompleteHandler(_);
        
    }

    public override void OnEnter()
    {
        
        CurrentScene.contentController.ShutTrainingIntroAnim();
        _depthC5SceneController.DepthC52Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {

        base.OnExit();
    }
}

public class DepthC52_State_2 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_2(DepthC5_SceneController currentScene) : base(currentScene)
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

public class DepthC52_State_3 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_3(DepthC5_SceneController currentScene) : base(currentScene) {_depthC5SceneController = currentScene;}

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC52_State_4 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;
    public DepthC52_State_4(DepthC5_SceneController currentScene) : base(currentScene) {_depthC5SceneController = currentScene;}


    public override void OnEnter()
    {

        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.BlockingPipePart,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.BlockingPipePart);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.BlockingPipePart);
        base.OnExit();
    }

}

public class DepthC52_State_5 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_5(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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

public class DepthC52_State_6 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_6(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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



public class DepthC52_State_7 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_7(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }

    public override void OnEnter()
    {

        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PanelDoorHandle,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.PanelDoorHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.PanelDoorHandle);
        base.OnExit();
    }
}

public class DepthC52_State_8 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_8(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        
        _depthC5SceneController.isWindSession = true;
        
        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;
        foreach (var key in _depthC5SceneController.isScrewWindMap.Keys.ToList())
            _depthC5SceneController.isScrewWindMap[key] = false;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);
        _depthC5SceneController.TurnOnCollidersAndInit();
        
        
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"Screw", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
     

        base.OnEnter();
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC5SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);


    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class DepthC52_State_9 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_9(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }
    public override void OnEnter()
    {
        
        
        _depthC5SceneController.isWindSession = true;
        
        foreach (var key in _depthC5SceneController.currentScrewGaugeStatus.Keys.ToList())
            _depthC5SceneController.currentScrewGaugeStatus[key] = 0f;
        foreach (var key in _depthC5SceneController.isScrewWindMap.Keys.ToList())
            _depthC5SceneController.isScrewWindMap[key] = false;
        
        _depthC5SceneController.SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB,false);
        _depthC5SceneController.BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);
        _depthC5SceneController.TurnOnCollidersAndInit();
        
        
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play($"Screw", 0, 0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        _depthC5SceneController.animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
     

        base.OnEnter();
        _depthC5SceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        _depthC5SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);


    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        
        base.OnExit();
    }
}

public class DepthC52_State_10 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_10(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthC5SceneController.ClearTool();    
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 5;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthC5SceneController.contentController.Refresh();
        _depthC5SceneController.DepthC53Init();
        base.OnEnter();
        _depthC5SceneController.PlayAnimation(1);
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
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_11(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_12(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


 
    public override void OnEnter()
    {

    }
}

public class DepthC52_State_13 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_13(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
    }


    public override void OnEnter()
    {

        base.OnEnter();
        
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC52_State_14 : Base_SceneState
{
    private readonly DepthC5_SceneController _depthC5SceneController;

    public DepthC52_State_14(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_15 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;

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
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC52_State_16(DepthC5_SceneController currentScene) : base(currentScene)
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
public class DepthC52_State_17 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC52_State_17(DepthC5_SceneController currentScene) : base(currentScene)
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


public class DepthC52_State_18 : Base_SceneState
{
    private DepthC5_SceneController _depthC5SceneController;
    public DepthC52_State_18(DepthC5_SceneController currentScene) : base(currentScene)
    {
        _depthC5SceneController = currentScene;
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