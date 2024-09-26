public class Base_SceneState : ISceneState
{
    protected Depth1A_SceneController CurrentScene;

    
    protected float _animationDelay=0;
  
    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Depth1A_SceneController currentScene)
    {
        CurrentScene = currentScene;
        Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
        if (_animationDelay == 0)
        {
            Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
            CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount);
            
        }
        else
        {
            Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
            CurrentScene.PlayAnimationAndNarration(CurrentScene.currentCount,_animationDelay);
        }
     
    }

    public virtual void OnStep()
    {
    }

    public virtual void OnExit()
    {
        _animationDelay = 0;
    }
}