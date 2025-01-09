using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateB21_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthB_SceneController _depthBceneController;
    public StateB21_1(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}

    public override void OnEnter()
    {
        _depthBceneController.DepthB21Init();
        _depthBceneController.TurnOnAllEquiment();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class StateB21_2 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_2(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}

    public override void OnEnter()
    {
        
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.ElectronicDriver);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Multimeter);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Wrench);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Stripper);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.PressureCalibrator);
        base.OnEnter();
    }

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}

public class StateB21_3 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_3(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}

    public override void OnEnter()
    {
        _depthBceneController.InitEquipmentSelectionPart();
        _depthBceneController.TurnOnAllEquiment();
        
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.ElectronicDriver,false);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Multimeter,false);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Wrench,false);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.Stripper,false);
        _depthBceneController.SetHighlightIgnore((int)DepthB_SceneController.DepthB_Objects.PressureCalibrator,false);
        
        
        
        _depthBceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.ElectronicDriver);
        _depthBceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.Multimeter);
        _depthBceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.Wrench);
        _depthBceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.Stripper);
        _depthBceneController.BlinkHighlight((int)DepthB_SceneController.DepthB_Objects.PressureCalibrator);
        
        
        base.OnEnter();
    }

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_4 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_4(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_5 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_5(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_6 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_6(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_7 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_7(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_8 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_8(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_9 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_9(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}




public class StateB21_10 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_10(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_11 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_11(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_12 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_12(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}


public class StateB21_13 : Base_SceneState
{
    DepthB_SceneController _depthBceneController;
    public StateB21_13(DepthB_SceneController currentScene) : base(currentScene)
    {_depthBceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}

    public override void OnStep(){base.OnStep();}

    public override void OnExit(){base.OnExit();}
}



