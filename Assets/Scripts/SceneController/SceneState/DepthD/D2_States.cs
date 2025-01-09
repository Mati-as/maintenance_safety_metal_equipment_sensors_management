using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class StateD21_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_1(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnEnter()
    {
      
       
        depthD2SceneController.indicator.ShowErrorMessage();
       
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_CompensatingWire);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_Stabilizer);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_SensingElement);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.OnTempSensor_Pipe);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_ConnectionPiping);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalA,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.PowerHandle,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.NewTemperatureSensor,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TankValve,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TemperatureSensor);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.MultimeterHandleHighlight,false);
     
      
        depthD2SceneController.contentController.SetInstructionShowOrHideStatus(true);
        depthD2SceneController.UIEvaluation.Init();
        depthD2SceneController.UIEvaluation.SetChecklistAnimStatus(false);

        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_2 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_2(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
            
        base.OnEnter();
        depthD2SceneController.UIEvaluation.UI_OnEvalStart();
  
        Managers.evaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_TemperatureSensor);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(1,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD21_3 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_3(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
       
//        Logger.Log("전원 On Off 하이라이트 동작 중 --------------------");
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(1);
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.PowerHandle,false);
      //  depthD2SceneController.HighlightBlink((int)DepthC_GameObj.PowerHandle);
        base.OnEnter();
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.PowerHandle);
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(2,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        
        base.OnExit();
    }
}

public class StateD21_4 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_4(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(2);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TankValve);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TankValve,false);
        //depthD2SceneController.HighlightBlink((int)DepthC_GameObj.TankValve);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(3,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD21_5 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_5(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(3);
        
        Managers.evaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_Cover);
        Managers.evaluationManager.isScoringState = false;
        
      
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover, false);
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in  depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        //나사 위치 초기화
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,false);
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, false);
        
        base.OnEnter();
       
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.isScoringState = false;
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(4,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
      
    }
}


public class StateD21_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_6(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}
    public override void OnEnter()
    {
        
        Managers.evaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
    
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewA);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewB);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewC);
        
       Logger.Log("드라이버 파트-------------------------");
     
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
      
        base.OnExit();
    }
}


public class StateD21_7 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_7(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(4);
        depthD2SceneController.ClearTool();
        
        depthD2SceneController.GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(true);
        depthD2SceneController.GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(true);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        Managers.evaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_Multimeter);
        
     
       
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        
        foreach (var key in  depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
        
   
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_8 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_8(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
       
        
        depthD2SceneController.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "측정 단자 A");
        depthD2SceneController.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "측정 단자 B");
        
        
        depthD2SceneController.multimeterController.OnGroundNothing();
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.Multimeter);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.MultimeterHandleHighlight);
        

        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewA);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewB);
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalA);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC,false);
        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;


        depthD2SceneController.isAnodePut = false;

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_9 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 depthD2SceneController에 접근 가능
    DepthD2_SceneController depthD2SceneController;

    public StateD21_9(DepthD2_SceneController currentScene) : base(currentScene)
    {
        depthD2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        depthD2SceneController.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "측정 단자 A");
        depthD2SceneController.BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
        
        depthD2SceneController.isMultimeterOn = true;
        depthD2SceneController.CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        depthD2SceneController.isAnodePut = false;
        depthD2SceneController.multimeterController.OnGroundNothing();
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC,false);
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
        
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewA);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_GroundingTerminalB);

        
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;
        base.OnEnter();
        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(7,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD21_10 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_10(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        
        CurrentScene.BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "나사");
      
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewA);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewB);
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.TS_InnerScrewC);
        
        Managers.evaluationManager.UIanswerToClick.Add((int)UI_ToolBox.Btns.Btn_ElectricScrewdriver);
        
        depthD2SceneController.isWindSession = true;
        depthD2SceneController.ClearTool();
        depthD2SceneController.multimeterController.SetMeasureGuideStatus(false);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        depthD2SceneController.contentController.isStepMissionPerformable = true;
        foreach (var key in depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in depthD2SceneController.isScrewWindMap.Keys.ToList())
            depthD2SceneController.isScrewWindMap[key] = false;


        //나사 위치 초기화
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND, false);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND, false);

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play("Screw", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play("Screw", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play("Screw", 0, 0);

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);

        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;

        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, false);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(5,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD21_11 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_11(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(5);
      
        depthD2SceneController.controlPanel.SetPowerHandleOff();
        Managers.evaluationManager.objAnswerToClick.Add((int)DepthC2_GameObj.PowerHandle);
        depthD2SceneController.ClearTool();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        Managers.evaluationManager.SaveIsCorrectStatusPerItems(6,
            Managers.evaluationManager.isAlreadyWrongAnswerChecked);
        base.OnExit();
    }
}


public class StateD21_12 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;

    public StateD21_12(DepthD2_SceneController currentScene) : base(currentScene)
    {
        depthD2SceneController = currentScene;
      
    }

    public override void OnEnter()
    {
        depthD2SceneController.UIEvaluation.RefreshCheckListImage(6);
        depthD2SceneController.contentController.SetInstructionShowOrHideStatus(false);
        depthD2SceneController.UIEvaluation.UI_OnEvalFinish();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_13 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_13(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


