
public class DepthB2_Base_1 : Base_StateB_State
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthB_SceneController _depthC2SceneController;
    public DepthB22_State_1(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
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
        
        base.OnExit();
    }
}
