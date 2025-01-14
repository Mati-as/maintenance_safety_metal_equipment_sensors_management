

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
       
        base.OnEnter();
       
        _depthBSceneController.DepthB11Init();
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
        _depthBSceneController.TurnOffObjectOnCharacter();
    }
    public override void OnStep()
    {base.OnStep();}
    public override void OnExit()
    {base.OnExit();}
}

public class StateB11_3 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_3(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_Helmet,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_Helmet);

        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_Helmet);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnCharacter_Helmet);
        base.OnExit();
    }
}



public class StateB11_4 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_4(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedGloves,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedGloves);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_InsulatedGloves);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedGloves);
        base.OnExit();
    }
}


public class StateB11_5 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_5(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_Earplugs,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_Earplugs);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_Earplugs);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_Earplugs);
        base.OnExit();
    }
}


public class StateB11_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_6(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

  
    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_Mask,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_Mask);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_Mask);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_Mask);
        base.OnExit();
    }
}


public class StateB11_7 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_7(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

    
    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_ProtectiveGoggles,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_ProtectiveGoggles);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_ProtectiveGoggles);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_ProtectiveGoggles);
        base.OnExit();
    }
}


public class StateB11_8 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_8(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedShoes,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedShoes);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_InsulatedShoes);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_InsulatedShoes);
        base.OnExit();
    }
}


public class StateB11_9 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_9(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }


    public override void OnEnter()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_FlameResistantClothing,false);
        _depthBSceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.OnStorage_FlameResistantClothing);
        _depthBSceneController.TurnOffObjectOnCharacter(DepthB_SceneController.DepthB_Objects.OnCharacter_FlameResistantClothing);
        _depthBSceneController.GetObject((int)DepthB_SceneController.DepthB_Objects.Character_NoFlameSuit).SetActive(true);
        base.OnEnter();
    }
    public override void OnStep()
    {base.OnStep();}

    public override void OnExit()
    {
        _depthBSceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.OnStorage_FlameResistantClothing);
        base.OnExit();
    }
}


public class StateB11_10 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_10(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 2;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;

        
        _depthBSceneController.contentController.Refresh();
        _depthBSceneController.DepthB21Init();
        
        base.OnEnter();
        _depthBSceneController.PlayAnimation(1);
    }
    public override void OnStep()
    {base.OnStep();}
    public override void OnExit()
    {base.OnExit();}
}


// public class StateB11_11 : Base_SceneState
// {
//     // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
//     private readonly DepthB_SceneController _depthBSceneController;
//
//     public StateB11_11(DepthB_SceneController currentScene) : base(currentScene)
//     {
//         _depthBSceneController = currentScene;
//     }
//
//     public override void OnEnter()
//     {base.OnEnter();}
//     public override void OnStep()
//     {base.OnStep();}
//     public override void OnExit()
//     {base.OnExit();}
// }
//

public class StateB11_12 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    private readonly DepthB_SceneController _depthBSceneController;

    public StateB11_12(DepthB_SceneController currentScene) : base(currentScene)
    {
        _depthBSceneController = currentScene;
    }

    public override void OnEnter()
    {base.OnEnter();}
    public override void OnStep()
    {base.OnStep();}
    public override void OnExit()
    {base.OnExit();}
}
