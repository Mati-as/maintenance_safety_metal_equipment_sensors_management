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
        SetDepthNum(); //튜토리얼에서는 반드시 Depth를 Set해주고 들어가야합니다
        SetMainProperties(); 
        InitTutorialStates();
      
        
        TutorialInit();
        contentController.Init();
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
         
        
        multimeterController = GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<MultimeterController>();
        animatorMap.TryAdd((int)DepthC2_GameObj.Multimeter,
            GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<Animator>());
        
        SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA,false);
        BindHighlight((int)DepthC2_GameObj.TemperatureSensor, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_Stabilizer, "클릭");
        BindHighlight((int)DepthC2_GameObj.TS_ConnectionPiping, "클릭");
        
        
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewC, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_CoverScrew, "나사");
        
        
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "클릭");
        BindHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight, "클릭");
         
        
       animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,
           GetObject((int)DepthC2_GameObj.Probe_Anode).GetComponent<Animator>());
       animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,
           GetObject((int)DepthC2_GameObj.Probe_Cathode).GetComponent<Animator>());

        InitProbePos();
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
       
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;
        
        MultimeterController.OnResistanceMeasureReadyAction  -= OnResistantReadyOnTutorial;
        MultimeterController.OnResistanceMeasureReadyAction  += OnResistantReadyOnTutorial;

        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        


        
        GetObject((int)DepthC2_GameObj.TS_CoverScrew).BindEvent(() =>
        {
           // if(!contentController.isStepMissionComplete)Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            if (Managers.ContentInfo.PlayData.Count == 2) OnStepMissionComplete(animationNumber:2);
        });
        
        
        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
          // if(!contentController.isStepMissionComplete)Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            if (Managers.ContentInfo.PlayData.Count == 3) OnStepMissionComplete(animationNumber:3);
        });
        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
           //if(!contentController.isStepMissionComplete)Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            if (Managers.ContentInfo.PlayData.Count == 3) OnStepMissionComplete(animationNumber:3);
        });
        GetObject((int)DepthC2_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
         //  if(!contentController.isStepMissionComplete)Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            if (Managers.ContentInfo.PlayData.Count == 3) OnStepMissionComplete(animationNumber:3);
        });

        
        SetDepthNum();
        
        ChangeState(1);
        PlayAnimation(1);
        
        contentController.PlayTrainingGoalAnim();
    }
     

     private new void OnDestroy()
     {
         base.OnDestroy();

         Managers.Resource.Destroy(Managers.UI.FindPopup<UI_ContentController>().gameObject);
     
         

         UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
         UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
         UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
         MultimeterController.OnResistanceMeasureReadyAction -= OnResistantReadyOnTutorial;
         UI_ToolBox.ToolBox_TemperatureSensorClickedEvent -= OnUIBtnToolBoxTemperatureSensorClicked;
     }

     private void OnResistantReadyOnTutorial()
     {
         OnStepMissionComplete(animationNumber: 7, delayTimeAmount: new WaitForSeconds(1f));
     }

     protected override void OnUIToolBoxMultimeterBtnClicked()
     {
                 
         InitializeTool();
         OnStepMissionComplete(animationNumber: 6);
         CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
         isMultimeterOn = !isMultimeterOn;
         if (isMultimeterOn == false) CurrentActiveTool = -1;
     }
     

     protected override void OnToolBoxClicked()
     {
         if (Managers.ContentInfo.PlayData.Count == 5 )
         {
             Logger.Log("Toolbox Click event : 툴박스 클릭 이벤트 5 ------------------");
             OnStepMissionComplete(animationNumber: 5);
         }
     }


       private void InitTutorialStates()
    {
        if (_sceneStates == null)
        {
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

       
            };
        }

      
    }
}
