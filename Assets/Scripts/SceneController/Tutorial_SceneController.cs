using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;

public class Tutorial_SceneController : DepthC2_SceneController
{
    

    
    public override void Init()
    {
        Logger.Log($"튜토리얼 초기화 완료 --------------------------");
        BindEvent();
        BindObject(typeof(DepthC2_GameObj));
        GetScrewColliders();
        SetDepthNum(); //튜토리얼에서는 반드시 Depth를 Set해주고 들어가야합니다
        SetMainProperties(); 
        InitTutorialStates();
      
        
        TutorialInit();
        contentController.SetInstructionShowOrHideStatus(true);
    }

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 5;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }


     public void TutorialInit()
     {
         UnBindEventAttatchedObj();
         
        
        _mainAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();
        if(Managers.UI.SceneUI ==null) Managers.UI.ShowSceneUI<UI_Persistent>();
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();

        // UI_Evaluation.OnRestartBtnOnEvalClicked -= TutorialInit;
        // UI_Evaluation.OnRestartBtnOnEvalClicked += TutorialInit;
        
    
        
        multimeterController = GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<MultimeterController>();
        animatorMap.TryAdd((int)DepthC2_GameObj.Multimeter,
            GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<Animator>());
        
        SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        BindHighlight((int)DepthC2_GameObj.TemperatureSensor, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_Stabilizer, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_ConnectionPiping, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "클릭");
        BindHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight, "클릭");
        
       // SetScrewDriverSection();
        
       animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,
           GetObject((int)DepthC2_GameObj.Probe_Anode).GetComponent<Animator>());
       animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,
           GetObject((int)DepthC2_GameObj.Probe_Cathode).GetComponent<Animator>());

        InitProbePos();
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
       
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;

        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;
        
        
        

        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            
            Logger.Log("튜토리얼 클릭이벤트 실행 ---------------------------");
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);
            
            OnStepMissionComplete(animationNumber: 7, delayAmount: new WaitForSeconds(2f));
            
        }, Define.UIEvent.PointerDown);


        SetDepthNum();
        ChangeState(1);
        PlayAnimation(1);
        
    }
     

     private new void OnDestroy()
     {
         base.OnDestroy();
         
        // UI_Evaluation.OnRestartBtnOnEvalClicked -= TutorialInit;
         UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
         UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
         UI_ToolBox.ScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
         MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
         UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
     }

     protected override void OnUI_MultimeterBtnClicked()
     {
                 
         InitializeTool();
         OnStepMissionComplete(animationNumber: 6);
         CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
         isMultimeterOn = !isMultimeterOn;
         multimeterController.SetToResistanceModeAndRotation();

         if (isMultimeterOn == false) CurrentActiveTool = -1;
     }
     

     
       private void InitTutorialStates()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 5111, new Tutorial_State_1(this) },
            { 5112, new Tutorial_State_2(this) },
            { 5113, new Tutorial_State_3(this) },
            { 5114, new Tutorial_State_4(this) },
            { 5115, new Tutorial_State_5(this) },
            { 5116, new Tutorial_State_6(this) },
            { 5117, new Tutorial_State_7(this) },
            { 5118, new Tutorial_State_8(this) },
            { 5119, new Tutorial_State_9(this) },
            { 51110, new Tutorial_State_10(this) },
            { 51111, new Tutorial_State_11(this) },

            // { 32118, new DepthC21_State_18(this) },
            // { 32119, new DepthC21_State_19(this) },


       
        };
    }
}
