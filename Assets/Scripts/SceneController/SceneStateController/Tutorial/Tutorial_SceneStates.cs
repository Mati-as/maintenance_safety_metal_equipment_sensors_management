using System;
using DG.Tweening;
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
      
        base.OnEnter();
        tutorialSceneController.contentController.HideToolBoxBtn();
       // Managers.isTutorialAlreadyPlayed = true;
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
        isCurrentStateCameraControllable = true;
    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC2_GameObj.TS_Stabilizer);
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        tutorialSceneController.contentController.HideToolBoxBtn();
   
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_CoverScrew,false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_CoverScrew);

    }

    public override void OnStep()
    {
        base.OnStep();
        
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_CoverScrew);
        base.OnExit();
    }
}

public class Tutorial_State_3 : Base_SceneState
{
    Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_3(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
        isCurrentStateCameraControllable = true;

    }
    protected override void OnAnimationCompleteHandler(int _)
    {
        base.OnAnimationCompleteHandler(_);
        SetLookAt((int)DepthC2_GameObj.TS_Stabilizer);
      
    }

   
    public override void OnEnter()
    {
        tutorialSceneController.contentController.isStepMissionComplete = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB,false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC,false);
        CurrentScene.BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewC);
        tutorialSceneController.contentController.HideToolBoxBtn();
        base.OnEnter();
        DOVirtual.Float(0,0,5f, _ =>
        {
            isCurrentStateCameraControllable = true;
            CurrentScene.contentController.SetCamInitBtnStatus(true);
        });

    }

    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
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
    


    public override void OnEnter()
    {
       
        tutorialSceneController.contentController.ShowToolBoxAndGuideBookBtn();
     
        base.OnEnter();
        CurrentScene.contentController.HideToolBoxBtn();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit()
    {
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        CurrentScene.contentController.HideToolBoxBtn();
        base.OnExit();
    }
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
        
        CurrentScene.contentController.ShowToolBoxAndGuideBookBtn();
        tutorialSceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
        CurrentScene.contentController.uiToolBox.SetToolBox(false);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        CurrentScene.contentController.HideToolBoxBtn();
        base.OnExit();
    }
}

public class Tutorial_State_6 : Base_SceneState
{
    private Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_6(Tutorial_SceneController currentScene) : base(currentScene)
    {
        tutorialSceneController = currentScene;
    }


    public override void OnEnter()
    {
        CurrentScene.contentController.ShowToolBoxAndGuideBookBtn();
        CurrentScene.contentController.uiToolBox.SetToolBox(true);
        DOVirtual.DelayedCall(3, () => { CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_Next); });
        base.OnEnter();
        tutorialSceneController.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        tutorialSceneController.contentController.HideToolBoxBtn();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_7 : Base_SceneState
{
    
    private Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_7(Tutorial_SceneController currentScene) : base(currentScene)
    {
       
        tutorialSceneController = currentScene;
    }



    public override void OnEnter()
    {
        tutorialSceneController.contentController.uiToolBox.SetToolBox(false);
        tutorialSceneController.multimeterController.SetMeasureGuideStatus(false);
        tutorialSceneController.isMultimeterOn = true;
        tutorialSceneController.CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        
        tutorialSceneController.multimeterController.SetToDefaultMode();

    

    
        base.OnEnter();
        tutorialSceneController.contentController.HideToolBoxBtn();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        tutorialSceneController.ClearTool();
        
        CurrentScene.GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(true);
        CurrentScene.GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(true);
        base.OnExit();
    }
}

public class Tutorial_State_8 : Base_SceneState
{
    public Tutorial_State_8(Tutorial_SceneController currentScene) : base(currentScene)
    {
    }


    public override void OnEnter()
    {
        base.OnEnter();
           
        DOVirtual.DelayedCall(2.5f,()=>
        {
            Managers.UI.ShowPopupUI<UI_OnEndTutorialConfirmation>();
        });
        //CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_Next);
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit() {base.OnExit();}
}

public class Tutorial_State_9 : Base_SceneState
{
    private Tutorial_SceneController tutorialSceneController;
    public Tutorial_State_9(Tutorial_SceneController currentScene) : base(currentScene)
    {
       
        tutorialSceneController = currentScene;
    }

    public override void OnEnter()
    {
        
    
       
  
    }
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
        // Logger.Log("뎁스전환 -> 1.2.1");
        // Managers.ContentInfo.PlayData.Depth1 = 1;
        // Managers.ContentInfo.PlayData.Depth2 = 2;
        // Managers.ContentInfo.PlayData.Depth3 = 1;
        // Managers.ContentInfo.PlayData.Count = 1;
        // CurrentScene.contentController.OnDepth2Clicked(2);
        // CurrentScene.PlayAnimationAndNarration(count:1);
        
        
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
