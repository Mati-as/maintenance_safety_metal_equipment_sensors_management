
using System.Linq;

/// <summary>
/// 3.2.1 온도센서 상태 클래스입니다 ------------------------------------------------------------
/// </summary>
///
///
///
///  
public class DepthC22_State_1 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthC2_SceneController _depthC2SceneController;
    public DepthC22_State_1(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    public override void OnEnter()
    {
        Logger.Log("C22 초기화 진행 -----------------------");
        _depthC2SceneController.DepthC22Init();
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator).GetComponent<IndicatorController>().ShowErrorMessage();
        _depthC2SceneController.isWindSession = false;
        
        base.OnEnter();
     

    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
        
        base.OnExit();
    }
}

public class DepthC22_State_2 : Base_SceneState
{
    public DepthC22_State_2(DepthC2_SceneController currentScene) : base(currentScene)
    {}
    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_3 : Base_SceneState
{
    public DepthC22_State_3(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_4 : Base_SceneState
{
    public DepthC22_State_4(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        //초기화의 BindEvent에서 다룹니다.
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_SensingElement);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew, false);
        CurrentScene.HighlightBlink((int)DepthC2_GameObj.TS_LockingScrew);
        
       // CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        base.OnEnter();
    }

    public override void OnExit()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_SensingElement);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew);
        base.OnExit();
    }
}

public class DepthC22_State_5 : Base_SceneState
{
    public DepthC22_State_5(DepthC2_SceneController currentScene) : base(currentScene)
    {
    }

    public override void OnEnter()
    {
        
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_TemperatureSensor);
        base.OnEnter();

    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_6 : Base_SceneState
{
    // 부모 클래스 생성자를 호출하여 CurrentScene에 접근 가능
    DepthC2_SceneController _depthC2SceneController;
    public DepthC22_State_6(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    public override void OnEnter()
    {
        _depthC2SceneController.GetObject((int)DepthC2_GameObj.Indicator).GetComponent<IndicatorController>().ShowErrorMessage();
        base.OnEnter();
       
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_7 : Base_SceneState
{
    public DepthC22_State_7(DepthC2_SceneController currentScene) : base(currentScene){}
    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_8 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    public DepthC22_State_8(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover, false);
        CurrentScene.HighlightBlink((int)DepthC2_GameObj.TS_Cover);
        
        
        CurrentScene.contentController.isStepMissionPerformable = true;
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,false);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, false);
        
        base.OnEnter();
      
    }

 
    public override void OnExit()
    {
        base.OnExit();
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_Cover);
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC);
    }
    
}


public class DepthC22_State_9 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    
    public DepthC22_State_9(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    
    public override void OnEnter()
    {
        CurrentScene.contentController.BlinkBtnUI((int)Btns.Btn_ToolBox);
        CurrentScene.contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_ElectricScrewdriver);
       
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, 1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(1);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
        
        base.OnEnter();
        
    }
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_10 : Base_SceneState
{DepthC2_SceneController _depthC2SceneController;
    
    public DepthC22_State_10(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }
    
    public override void OnEnter()
    {   
        
        foreach (var key in  _depthC2SceneController.currentScrewGaugeStatus.Keys.ToList())
        {
            _depthC2SceneController.currentScrewGaugeStatus[key] = 0f;
        }
        
        foreach (var key in  _depthC2SceneController.isScrewWindMap.Keys.ToList())
        {
            _depthC2SceneController.isScrewWindMap[key] = false;
        }

        
        _depthC2SceneController.CurrentActiveTool =  (int)DepthC2_GameObj.ElectricScrewdriver;
        _depthC2SceneController._isDriverOn= true;
        _depthC2SceneController.ToggleActiveState(_depthC2SceneController.
            GetObject((int)DepthC2_GameObj.ElectricScrewdriver), _depthC2SceneController.isDriverOn);
        
        CurrentScene.HighlightAndTooltipInit((int)DepthC2_GameObj.TS_InnerScrewB, "나사");
        _depthC2SceneController.TurnOnCollidersAndInit();
        
        
        _depthC2SceneController.isWindSession = true;
        _depthC2SceneController.isMultimeterOn = false;
        
        
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, false);
        
        CurrentScene.HighlightBlink((int)DepthC2_GameObj.TS_InnerScrewA);
        CurrentScene.HighlightBlink((int)DepthC2_GameObj.TS_InnerScrewB);
        CurrentScene.HighlightBlink((int)DepthC2_GameObj.TS_InnerScrewC);
        
        //나사 위치 초기화
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,false);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,false);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"Screw", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"Screw", 0, 0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"Screw", 0, 0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);
        
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
        _depthC2SceneController.animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
        

     
        
        
        base.OnEnter();

        
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
       // _depthC2SceneController.controlPanel.SetPowerHandleOn();
        _depthC2SceneController.ClearTool();
        _depthC2SceneController.ToggleActiveState(_depthC2SceneController.
            GetObject((int)DepthC2_GameObj.ElectricScrewdriver), _depthC2SceneController.isDriverOn);
    }
}

public class DepthC22_State_11 : Base_SceneState
{
   
    DepthC2_SceneController _depthC2SceneController;
    public DepthC22_State_11(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 3;
        Managers.ContentInfo.PlayData.Count = 1;
        
        _depthC2SceneController.contentController.Refresh();
        _depthC2SceneController.DepthC23Init();
        _depthC2SceneController.PlayAnimationAndNarration(1);
        
        base.OnEnter();
    }

    public override void OnStep()
    {
    }

    public override void OnExit()
    {
     
   
        base.OnExit();
    }
}


public class DepthC22_State_12 : Base_SceneState
{
    DepthC2_SceneController _depthC2SceneController;
    
    public DepthC22_State_12(DepthC2_SceneController currentScene) : base(currentScene)
    {
        _depthC2SceneController = currentScene;
    }

    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

public class DepthC22_State_13 : Base_SceneState
{
   
    public DepthC22_State_13(DepthC2_SceneController currentScene) : base(currentScene)
    {
     
    }

    public override void OnEnter() {base.OnEnter();}
    public override void OnStep(){base.OnStep();}
    public override void OnExit(){base.OnExit();}
}

