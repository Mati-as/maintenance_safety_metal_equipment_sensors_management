using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class StateD11_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_1(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.Init();
        depthD1SceneController.UIEvaluation.SetChecklistAnimStatus(false);

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}



public class StateD11_2 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_2(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        
        depthD1SceneController.UIEvaluation.UI_OnEvalStart();
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_TemperatureSensor);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(1,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}



public class StateD11_3 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_3(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(1);
        depthD1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.PowerHandle,false);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC1_GameObj.PowerHandle);
        
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(2,
            Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}



public class StateD11_4 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_4(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(2);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC1_GameObj.LS_Cover);
        depthD1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.LS_Cover,false);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(3,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_5 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_5(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(2);
        depthD1SceneController.isWindSession = false;
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.LeverScrew);
        base.OnEnter();
        depthD1SceneController.SetScrewStatus(false);
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(4,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}

public class StateD11_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_6(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(3);
        depthD1SceneController.TurnOnCollidersAndInit();
        depthD1SceneController.isLeverScrewUnwound = false;
        
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.ConnectionScrewA);
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.ConnectionScrewB);
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.ConnectionScrewC);
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.ConnectionScrewD);
        base.OnEnter();
        depthD1SceneController.limitSwitchPivotController.SetLimitSwitchControllableOrClickable(false);
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(5,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_7 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_7(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.TurnOffCollider((int)DepthC1_GameObj.LeverScrew);
        
        depthD1SceneController.isWindSession = true;
        depthD1SceneController.SetScrewStatus(true);
        base.OnEnter();
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(6,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_8 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_8(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        base.OnEnter();
        Managers.EvaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_Multimeter);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(7,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_9 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_9(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.TakeDefaultMultimeter();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(8,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_10 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_10(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.SetToResistantMode();
        depthD1SceneController.multimeterController.isConductive = true; 
        depthD1SceneController.isAnodePut = false;
        
        depthD1SceneController.multimeterController.SetMeasureGuideStatus(true);
        depthD1SceneController.TurnOnCollidersAndInit();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD11_11 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_11(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.InitProbe();
        depthD1SceneController.SetToResistantMode();
        depthD1SceneController.multimeterController.isConductive = true; 
        depthD1SceneController.isAnodePut = false;
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(9,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_12 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_12(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(4);
        depthD1SceneController.ClearTool();
        depthD1SceneController.controlPanel.SetPowerHandleOff();
        depthD1SceneController.SetHighlightIgnore((int)DepthC1_GameObj.PowerHandle,false);
        Managers.EvaluationManager.ObjAnswerToClick.Add((int)DepthC1_GameObj.PowerHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.EvaluationManager.SaveIsCorrectStatusPerItems(10,Managers.EvaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD11_13 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_13(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
        depthD1SceneController.UIEvaluation.RefreshCheckListImage(5);
        depthD1SceneController.contentController.SetInstructionShowOrHideStatus(false);
        depthD1SceneController.UIEvaluation.UI_OnEvalFinish();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class StateD11_14 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD1_SceneController depthD1SceneController;
    public StateD11_14(DepthD1_SceneController currentScene) : base(currentScene)
    { depthD1SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
      
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}
