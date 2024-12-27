using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateB11_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_1(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
       
    }

    public override void OnEnter()
    {
        _depthBSceneController.contentController.ShutTrainingIntroAnim();
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
public class StateB11_2 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_2(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
       
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
