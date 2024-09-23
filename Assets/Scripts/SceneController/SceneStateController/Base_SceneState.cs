public class Base_SceneState : ISceneState
{
    protected Depth1A_SceneController CurrentScene;

    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Depth1A_SceneController currentScene)
    {
        CurrentScene = currentScene;
        Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
        Managers.Sound.Play(SoundManager.Sound.Narration, Managers.ContentInfo.PlayData.CurrentDepthStatus);
    }

    public virtual void OnStep()
    {
    }

    public virtual void OnExit()
    {
    }
}