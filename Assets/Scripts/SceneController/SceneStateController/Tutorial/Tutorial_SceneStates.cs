using System;
using UnityEngine;

public class Tutorial_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능


    Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_1(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }
    public override void OnEnter()
    {
        tutorialSceneController.Init();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_2 : Base_SceneState
{
    Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_2(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC_GameObj.TemperatureSensor);
    }
    
    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        
//        CurrentScene.HighlightBlink((int)DepthAGameObj.TemperatureSensor);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class Tutorial_State_3 : Base_SceneState
{
    Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_3(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC_GameObj.TemperatureSensor);
    }
    
    public override void OnEnter()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        tutorialSceneController.UITutorial = GameObject.Find("UI_Tutorial").GetComponent<UI_Tutorial>();
        tutorialSceneController.UITutorial.BlinkBtnUI((int)UI_Tutorial.Btns.Btn_CameraInit);
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        base.OnExit();
    }
}

public class Tutorial_State_4 : Base_SceneState
{
    private Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_4(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {
    }


    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_5 : Base_SceneState
{
    private Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_5(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }

    protected override void OnAnimationCompleteHandler(int _)
    {

    }



    public override void OnEnter()
    {
        tutorialSceneController.UITutorial.BlinkBtnUI((int)UI_Tutorial.Btns.Btn_ToolBox);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_6 : Base_SceneState
{
    public Tutorial_State_6(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }


    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_7 : Base_SceneState
{
    public Tutorial_State_7(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }


    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_8 : Base_SceneState
{
    public Tutorial_State_8(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }


    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_9 : Base_SceneState
{
    public Tutorial_State_9(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_10 : Base_SceneState
{
    
    public Tutorial_State_10(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }
    
    protected override void OnAnimationCompleteHandler(int _)
    {


    }

    public override void OnEnter(){base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_11 : Base_SceneState
{
    public Tutorial_State_11(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        Logger.Log("뎁스전환 -> 1.2.1");
        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        CurrentScene.contentController.OnDepth2Clicked(2);
        CurrentScene.PlayAnimationAndNarration(count:1);
        
        
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
