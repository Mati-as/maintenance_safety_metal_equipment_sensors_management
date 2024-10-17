public class Base_SceneState : ISceneState
{
    protected Base_SceneController CurrentScene;

    
    protected float _animationDelay=0;
    protected float _instructionDelay = 0;
  
    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Base_SceneController currentScene)
    {
        CurrentScene = currentScene;
        Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
        CurrentScene.contentController.isStepMissionPerformable = false;
        
        CurrentScene.ChangeInstructionTextWithAnim();
        if (_animationDelay == 0)
        {
            Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
            CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,isReverse:CurrentScene.isReverseAnim);
            
        }
        else
        {
            Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
            CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,delay:_animationDelay,isReverse:CurrentScene.isReverseAnim);
        }
     
    }

    public virtual void OnStep()
    {
    }

    public virtual void OnExit()
    {
        //CurrentScene.contentController.ShutTrainingInfroAnim();
     
        _animationDelay = 0;
    }
    
    public void OnAnimationComplete(int currentAnimationNumber)
    {
        if (currentAnimationNumber == 18)
        {   
            Logger.Log("Camera Control Available ------------------------------");
            CurrentScene.cameraController.isControllable = true;
            CurrentScene.cameraController.SetCurrentMainAngleAndPos
                (CurrentScene.GetObject((int)GameObj.TS_Stabilizer).transform);
                
        }
    }
}