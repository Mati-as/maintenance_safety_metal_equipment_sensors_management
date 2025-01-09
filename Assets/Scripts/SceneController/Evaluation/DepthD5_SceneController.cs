using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// 1. 온도센서 --------------------------------기본적으로 센서 로직 상속
/// 2. 애니메이션 별도 구성
/// 3. BasesceneController 기본적으로 상속
/// </summary>
public class DepthD5_SceneController : DepthC5_SceneController
{
    public UI_Evaluation UIEvaluation { get; private set; }

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 5;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void Init()
    {
     //   if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000")
        SetDepthNum(); 
      
        base.Init();
        
        BindObject(typeof(DepthC2_GameObj));
        if(UIEvaluation==null) UIEvaluation = Managers.UI.ShowPopupUI<UI_Evaluation>();
        GetScrewColliders();
        InitializeD2States();
        DepthD21Init();
        contentController.OnDepth2Init(2); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
      
        
        
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD21Init;
        UI_Evaluation.OnRestartBtnOnEvalClicked += DepthD21Init;
    }

    public void DepthD21Init()
    {
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();

        
        UnBindEventAttatchedObj();


        var isKorMode = Managers.UI.languageSetting == (int)Define.LanguageMode.Kor ;
        BindHighlight((int)DepthC5_GameObj.LevelSensor,isKorMode ?"클릭": "Eng");
        BindHighlight((int)DepthC5_GameObj.NewLevelSensor,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.LookAtPoint_LevelSensor,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ElectricScrewdriver,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.Multimeter,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.MultimeterHandleHighlight,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.Probe_Anode,isKorMode ?"클릭" : "Eng"); // negative
        BindHighlight((int)DepthC5_GameObj.Probe_Cathode,isKorMode ?"클릭" : "Eng"); // positive,
        BindHighlight((int)DepthC5_GameObj.CathodeSensorInput,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.Wrench,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.LevelSensor_PipeValve,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewA,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewB,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewC,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewD,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.PowerHandle,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.AnodeSensorOutput,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.LevelSensorConnectingPipe,isKorMode ?"클릭" : "Eng"); //연결 배관
        BindHighlight((int)DepthC5_GameObj.LevelSensorConnectingScrew,isKorMode ?"클릭" : "Eng"); // 연결 나사 (어댑터)
        BindHighlight((int)DepthC5_GameObj.ContaminatedRod,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.PanelDoorHandle,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.BlockingPipePart,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.LevelSensor_TankWaterFluidEffect,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.LevelSensor_ResidueTankWaterFluidEffect,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.ModeOrEnterBtn,isKorMode ?"클릭" : "Eng");
        BindHighlight((int)DepthC5_GameObj.SetBtn,"세팅버튼");
    
 
        SetScrewDriverSection();
        BindInteractionEvent();
        
        InitProbePos();
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);

        
        
        foreach (DepthC2_GameObj obj in Enum.GetValues(typeof(DepthC2_GameObj)))
        {
            if (GetObject((int)obj) == null||
                obj == DepthC2_GameObj.OnTempSensor_Pipe||
                obj == DepthC2_GameObj.MultimeterHandleHighlight||
            obj == DepthC2_GameObj.TemperatureSensor
                )
            {
                Logger.Log($"no object is set: {obj}");
                continue;
            }

            GetObject((int)obj).BindEvent(() =>
            {
                Managers.evaluationManager.CheckIfAnswerIsCorrect((int)obj);
                Logger.Log($"Evaluation Event Bound : {obj}");
            });
        }
        
        
        
        
        GetObject((int)DepthC2_GameObj.TankValve).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 4)
            {
                OnStepMissionComplete(animationNumber: 4,delayTimeAmount: new WaitForSeconds(4.5f));
                Logger.Log("벨브 잠금 및 유체 차단 애니메이션 재생 -----------------");
            }
        });
        
        GetObject((int)DepthC2_GameObj.TS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 5)
            {
                OnStepMissionComplete(animationNumber: 5,delayTimeAmount: new WaitForSeconds(4f));
                Logger.Log("커버 애니메이션 재생");
            }
        });
        
        
        
        
        
     
        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 4) return;
            
            
            //108옴 저항측정
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
                isAnodePut = true; 
                BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "측정 단자 B");
                SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
                //HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
                
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
            

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
            
            //접지
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
                isAnodePut = true; 
                BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
                SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
                //HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalB);
            
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(TO_GROUNDING_TERMINAL, true);
                
                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });

            }

        
         
            
            

        }, Define.UIEvent.PointerDown);
       
        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            //108옴 저항측정
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
                if (!isAnodePut) return;
                Logger.Log("평가하기 - count 8 - 저항측정(108옴) 완료");
                BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
               // HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalB);
                SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
                
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
            

                multimeterController.OnAllProbeSetOnResistanceMode();
                OnStepMissionComplete(animationNumber: 8, delayTimeAmount: new WaitForSeconds(6f));
            }
        });

        GetObject((int)DepthC2_GameObj.TS_GroundingTerminalA).BindEvent(() =>
        {
          
        });


        GetObject((int)DepthC2_GameObj.TS_GroundingTerminalB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 4) return;
            if (!isAnodePut) return;
       
          
            if (Managers.ContentInfo.PlayData.Count == 9)
            {  
                
                Logger.Log("접지미션 수행 완료");
                
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(TO_GROUNDING_TERMINAL, true);
                multimeterController.OnAllProbeSetToGroundingTerminal();
         
                OnStepMissionComplete(animationNumber:9, delayTimeAmount: new WaitForSeconds(6f));
                
            }

      
        });
 
               
        SetDepthNum();
        ChangeState(1);
        PlayAnimation(1);
        
    }
    
    protected override void UnBindInteractionEvent()
    {
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD21Init;
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
     
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;

    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        UnBindInteractionEvent();
    }


    public override void OnElectricScrewdriverBtnClicked()
    {
        if (!Managers.evaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_ElectricScrewdriver)) return;
        base.OnElectricScrewdriverBtnClicked();
        
    }

    protected override void OnUIToolBoxMultimeterBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != 4) return;

        if (!Managers.evaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_Multimeter)) return;
       
        
        InitializeTool();
        CurrentActiveTool = (int)DepthC2_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false)
        {
            CurrentActiveTool = -1;
        }

        if (Managers.ContentInfo.PlayData.Count == 7)
        {
            OnStepMissionComplete(animationNumber: 7);
        };    
    }
    
    protected override void OnToolBoxClicked()
    {
//        Logger.Log("");
 
    }
    
    protected override void PowerOnOff(bool isOn)
    {
        if (Managers.ContentInfo.PlayData.Count == 3)
        {
            if(!isOn) OnStepMissionComplete(animationNumber:3);
        }
       
        if (Managers.ContentInfo.PlayData.Count == 11)
        {
            if(isOn) OnStepMissionComplete(animationNumber:11);
        }
    }


     

     
     protected override void SetToolPos()
     {
         var distanceFromCamera = 0.09f;
         var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
             Input.mousePosition.y + _toolPosYOffset,
             distanceFromCamera));


         if (isDriverOn && CurrentActiveTool == (int)DepthC2_GameObj.ElectricScrewdriver)
         {

             GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
             GetObject((int)DepthC2_GameObj.ElectricScrewdriver).transform.position = mousePosition;
         }
         else if (isMultimeterOn && CurrentActiveTool == (int)DepthC2_GameObj.Multimeter && multimeterController.isCurrentCheckMode)
         {
             GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
             GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(isMultimeterOn);
        
             if ((Managers.ContentInfo.PlayData.Count >= 8 && !isAnodePut)||(Managers.ContentInfo.PlayData.Depth1 ==4 && Managers.ContentInfo.PlayData.Count>=8 &&!isAnodePut))
             {
                 GetObject((int)DepthC2_GameObj.Probe_Anode).transform.rotation =
                     defaultRotationMap[(int)DepthC2_GameObj.Probe_Anode];
                
                 GetObject((int)DepthC2_GameObj.Probe_Anode).transform.position = mousePosition;
             }

             if ((Managers.ContentInfo.PlayData.Count >= 8 && isAnodePut)||(Managers.ContentInfo.PlayData.Depth1 ==4  && Managers.ContentInfo.PlayData.Count>=98 &&isAnodePut))
             {
                 GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation =
                     defaultRotationMap[(int)DepthC2_GameObj.Probe_Cathode];
                
                 GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.position = mousePosition;
             }

         }
      





     }
     
     private void InitializeD2States()
     {

         _sceneStates = new Dictionary<int, ISceneState>
         { 
             { 4211, new  StateD51_1(this) },
             { 4212, new  StateD51_2(this) },
             { 4213, new  StateD51_3(this) },
             { 4214, new  StateD51_4(this) },
             { 4215, new  StateD51_5(this) },
             { 4216, new  StateD51_6(this) },
             { 4217, new  StateD51_7(this) },
             { 4218, new  StateD51_8(this) },
             { 4219, new  StateD51_9(this) },
             { 42110, new StateD51_10(this) },
        
       
         };
     }
}
