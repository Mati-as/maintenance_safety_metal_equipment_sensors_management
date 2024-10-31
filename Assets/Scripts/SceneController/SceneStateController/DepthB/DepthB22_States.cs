using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_StateB_State : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public DepthB_SceneController _depthC2SceneController;
    public Base_StateB_State(DepthB_SceneController currentScene) : base(currentScene)
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

