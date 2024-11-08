
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
      
        if (Managers.ContentInfo.PlayData.Depth1 == 4)
        {
            Managers.evaluationManager. InitPerState();
        }
        
        //항상 클릭 가능해야하는 것들 ----
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.MultimeterHandleHighlight,false);
        CurrentScene.ChangeInstructionTextWithAnim();
        CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,isReverse:CurrentScene.isReverseAnim);
        CurrentScene.contentController.SetNextPrevBtnsActiveStatus();
        
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
           Managers.evaluationManager.OnStateExit();
       }
       
     

        CurrentScene.TurnOffAllRegisteredHighlights();
        CurrentScene.contentController.StopBtnUIBlink();
        CurrentScene.contentController.uiToolBox.Refresh();
    }
    
    public void OnAnimationComplete(int currentAnimationNumber)
    {
        
            Logger.Log("Camera Control Available ------------------------------");
            CurrentScene.cameraController.isControllable = true;
            CurrentScene.cameraController.SetCurrentMainAngleAndPos
                (CurrentScene.GetObject((int)DepthAGameObj.TS_Stabilizer).transform);
                
        
    }
}