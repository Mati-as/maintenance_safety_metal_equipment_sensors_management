
public class Base_SceneState : ISceneState
{
    protected Base_SceneController CurrentScene;

    
  
    protected float _animationDelay=0;
    protected float _narrationStartDelay = 1.55f;
  
    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Base_SceneController currentScene)
    {
        CurrentScene = currentScene;
        //Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
        //항상 클릭 가능해야하는 것들 ----
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.MultimeterHandleHighlight,false);
        
        CurrentScene.contentController.isStepMissionComplete = false;
        CurrentScene.contentController.isStepMissionPerformable = true;
       // CurrentScene.isSceneAnimationPlayingToProtectDoublePlaying = false;
         
        CurrentScene.ChangeInstructionTextWithAnim();
        CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,isReverse:CurrentScene.isReverseAnim);
     
        Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
        

        Managers.Sound.PlayNarration(_narrationStartDelay);

    }


    public virtual void OnStep()
    {
    }

    public virtual void OnExit()
    {
        //CurrentScene.contentController.ShutTrainingInfroAnim();
       // _animationDelay = 0;
        CurrentScene.TurnOffAllRegisteredHighlights();
        CurrentScene.contentController.StopBtnUIBlink();
    }
    
    public void OnAnimationComplete(int currentAnimationNumber)
    {
        
            Logger.Log("Camera Control Available ------------------------------");
            CurrentScene.cameraController.isControllable = true;
            CurrentScene.cameraController.SetCurrentMainAngleAndPos
                (CurrentScene.GetObject((int)DepthAGameObj.TS_Stabilizer).transform);
                
        
    }
}