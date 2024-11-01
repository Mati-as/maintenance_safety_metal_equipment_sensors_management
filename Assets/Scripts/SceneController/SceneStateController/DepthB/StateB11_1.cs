using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateB11_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthB_SceneController _depthC2SceneController;
    public StateB11_1(DepthB_SceneController currentScene) : base(currentScene)
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
