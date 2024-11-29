
public class Base_SceneState : ISceneState
{
    protected Base_SceneController CurrentScene;

    
  
    protected float _animationDelay=0;
 
  
    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Base_SceneController currentScene)
    {
        CurrentScene = currentScene;
        //Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
     
        // Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        // Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
        
        //평가하기가 아닌경우
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation)
        {
            Managers.evaluationManager. InitPerState();
        }

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.SensorOverview)
        {
            CurrentScene.contentController.HideToolBoxBtn();
        }
        else
        {
            CurrentScene.contentController.ShowToolBoxAndGuideBookBtn();
        }
        
        //튜토리얼이 아닌경우
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.MultimeterHandleHighlight,false);
          
        CurrentScene.contentController.SetNextPrevBtnsActiveStatus();
        CurrentScene.contentController.isStepMissionComplete = false;
        CurrentScene.ChangeInstructionTextWithAnim();
        
        //항상 클릭 가능해야하는 것들 ----
       
        CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,isReverse:CurrentScene.isReverseAnim);
    
        
        Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
        

    }


    public virtual void OnStep()
    {
    }

    public virtual void OnExit()
    {
        //CurrentScene.contentController.ShutTrainingInfroAnim();
       // _animationDelay = 0;

       
       /* Depth4 훈련하기에서만 진행할 수 있도록 다음과 같이 구성하며, UI클릭 미션인경우 다른 오브젝트도
      오답쳐리가 될 수 있도록 다음과 같이 센티널 값을 넣어 구성합니다.
     */
       if (Managers.ContentInfo.PlayData.Depth1 == 4)
       {
           Managers.evaluationManager.EvalmodeOnStateExit();
          
       }
       
          
           CurrentScene.contentController.ShutTrainingIntroAnim();
           CurrentScene.TurnOffAllRegisteredHighlights();
           CurrentScene.contentController.StopBtnUIBlink();
           CurrentScene.contentController.uiToolBox.Refresh();

           Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        
           CurrentScene.cameraController.isControllable = false;

    }
    

    public void SetLookAt(int objToActivate)
    {
        CurrentScene.cameraController.SetRotationDefault(
            CurrentScene.GetObject(objToActivate).transform
        );
    }
    
    
    /// <summary>
    /// OnAnimatrionCompele 사용시
    /// 자동으로 1.카메사용 가능, 2. 초기화 버튼 하이라이트 동작
    /// </summary>
    /// <param name="_"></param>
    protected virtual void OnAnimationCompleteHandler(int _)
    {
        
        CurrentScene.cameraController.isControllable = true;
        CurrentScene.cameraController.isDragging = false;
        if (CurrentScene.contentController != null)
        {
            CurrentScene.contentController.ShowCamInitBtn();
        }
        else Logger.Log("content Controller is Null.. it must be tutorial state.");
    }
    

}