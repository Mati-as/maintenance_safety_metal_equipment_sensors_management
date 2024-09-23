using UnityEngine;


public class Base_SceneState : ISceneState
{
    protected Depth1A_SceneController CurrentScene;

    // BaseScene 참조를 생성자에서 주입받음
    protected Base_SceneState(Depth1A_SceneController currentScene)
    {
        CurrentScene = currentScene;
        Logger.Log($"state logic Uploaded : current scene controller: {currentScene}");
    }

    public virtual void OnEnter(){} 
    public virtual void OnStep(){}
    public virtual void OnExit(){}
}


public class Depth1A_State_1 : Base_SceneState
{
   // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    public Depth1A_State_1(Depth1A_SceneController currentScene) : base(currentScene) {}

    public override void OnEnter()
    {
        if(!CurrentScene.contentController.isInstructAnimOn) CurrentScene.IntroUIAnim();
    } 
    public override void OnStep(){}

    public override void OnExit()
    {
        CurrentScene.ShutUIAndSetDefault();
    }
}

public class Depth1A_State_2 : Base_SceneState
{
     public Depth1A_State_2(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep()  {}
    public override void OnExit() {}
}

public class Depth1A_State_3 : Base_SceneState
{
     public Depth1A_State_3(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_4 : Base_SceneState
{
     public Depth1A_State_4(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_5 : Base_SceneState
{
     public Depth1A_State_5(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_6 : Base_SceneState
{
     public Depth1A_State_6(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_7 : Base_SceneState
{
     public Depth1A_State_7(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_8 : Base_SceneState
{
     public Depth1A_State_8(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_9 : Base_SceneState
{
     public Depth1A_State_9(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}
public class Depth1A_State_10 : Base_SceneState
{
     public Depth1A_State_10(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_11 : Base_SceneState
{
     public Depth1A_State_11(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_12 : Base_SceneState
{
     public Depth1A_State_12(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_13 : Base_SceneState
{
     public Depth1A_State_13(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_14 : Base_SceneState
{
     public Depth1A_State_14(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}


public class Depth1A_State_15 : Base_SceneState
{
     public Depth1A_State_15(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}


public class Depth1A_State_16 : Base_SceneState
{
     public Depth1A_State_16(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}


public class Depth1A_State_17 : Base_SceneState
{
     public Depth1A_State_17(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}


public class Depth1A_State_18 : Base_SceneState
{
     public Depth1A_State_18(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}


public class Depth1A_State_19 : Base_SceneState
{
     public Depth1A_State_19(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}

public class Depth1A_State_20 : Base_SceneState
{
     public Depth1A_State_20(Depth1A_SceneController currentScene) : base(currentScene) {}
    public override void OnEnter() {}
    public override void OnStep(){}
    public override void OnExit() {}
}
