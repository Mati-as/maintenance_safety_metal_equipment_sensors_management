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

    public override void OnEnter()
    {
        depthD2SceneController.contentController.ShutTrainingIntroAnim();
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
       
        //depthD2SceneController.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_3 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_3(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        
        Logger.Log("전원 On Off 하이라이트 동작 중 --------------------");
        depthD2SceneController.SetHighlightIgnore((int)DepthC_GameObj.PowerHandle,false);
        //depthD2SceneController.HighlightBlink((int)DepthC_GameObj.PowerHandle);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class StateD21_4 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_4(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC_GameObj.TankValve,false);
        //depthD2SceneController.HighlightBlink((int)DepthC_GameObj.TankValve);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_5 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_5(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
      
        depthD2SceneController.SetHighlightIgnore((int)DepthC_GameObj.TS_Cover, false);
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in  depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        //나사 위치 초기화
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,false);
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(0);
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);
        
        base.OnEnter();
       
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_6(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}
    public override void OnEnter()
    {
        
       Logger.Log("덮개열림 파트-------------------------");
     
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_7 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_7(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        depthD2SceneController.ClearTool();
        
        depthD2SceneController.GetObject((int)DepthC_GameObj.Probe_Cathode).SetActive(true);
        depthD2SceneController.GetObject((int)DepthC_GameObj.Probe_Anode).SetActive(true);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(1);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
        
   
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
        depthD2SceneController.multimeterController.OnGroundNothing();

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalA);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "저항 측정");
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);


        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;


        depthD2SceneController.isAnodePut = false;

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_9 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;

    public StateD21_9(DepthD2_SceneController currentScene) : base(currentScene)
    {
        depthD2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        depthD2SceneController.isMultimeterOn = true;
        depthD2SceneController.CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
        depthD2SceneController.isAnodePut = false;
        depthD2SceneController.multimeterController.OnGroundNothing();
        
        depthD2SceneController.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA,false);
        depthD2SceneController.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB, false);base.OnEnter();
        
        
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode]
            .SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Play("ON", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;
        
        
    }
    public override void OnStep(){base.OnStep();}

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA);
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
        CurrentScene.BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        
        depthD2SceneController.isWindSession = true;
        depthD2SceneController.isMultimeterOn = false;
        depthD2SceneController.multimeterController.SetMeasureGuideStatus(false);
        depthD2SceneController.TurnOnCollidersAndInit();
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in depthD2SceneController.currentScrewGaugeStatus.Keys.ToList())
            depthD2SceneController.currentScrewGaugeStatus[key] = 0f;

        foreach (var key in depthD2SceneController.isScrewWindMap.Keys.ToList())
            depthD2SceneController.isScrewWindMap[key] = false;


        //나사 위치 초기화
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = true;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = true;

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND, false);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND, false);

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Play("Screw", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Play("Screw", 0, 0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Play("Screw", 0, 0);

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].Update(0);
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].Update(0);

        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
        depthD2SceneController.animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;

        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewC, false);
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_11 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_11(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}

    public override void OnEnter()
    {
        depthD2SceneController.ClearTool();
        base.OnEnter();
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}


public class StateD21_12 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthD2_SceneController depthD2SceneController;
    public StateD21_12(DepthD2_SceneController currentScene) : base(currentScene)
    { depthD2SceneController = currentScene;}
    public override void OnEnter() {base.OnEnter();}
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


