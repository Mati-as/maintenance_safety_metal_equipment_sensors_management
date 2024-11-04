using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// 1. 기본적으로 센서 로직 상속
/// 2. 애니메이션 별도 구성
/// 3. BasesceneController 기본적으로 상속
/// </summary>
public class DepthD2_SceneController : DepthC2_SceneController
{

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum(); //개발용
      
        base.Init();
        
        BindObject(typeof(DepthC_GameObj));
        GetScrewColliders();
        InitializeD2States();
        DepthD21Init();
        contentController.OnDepth2Clicked(2); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
    }

    public void DepthD21Init()
    {
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();
       
        UnBindEventAttatchedObj();
    
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Cover, "덮개");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_LockingScrew, "고정나사 체결확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "나사A");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "나사B");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewC, "나사C");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalA, "접지");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "접지");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.PowerHandle, "전원");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.NewTemperatureSensor, "새 온도센서");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TankValve, "밸브");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TemperatureSensor, "교체 할 센서");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.MultimeterHandleHighlight, "측정모드 설정");
        
        SetScrewDriverSection();
        
        InitProbePos();
        defaultRotationMap.TryAdd((int)DepthC_GameObj.Probe_Cathode,GetObject((int)DepthC_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC_GameObj.Probe_Anode,GetObject((int)DepthC_GameObj.Probe_Cathode).transform.rotation);
       
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;

        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;
        
        UI_ToolBox.ScrewDriverClickedEvent -= OnScrewDriverBtnClicked;
        UI_ToolBox.ScrewDriverClickedEvent += OnScrewDriverBtnClicked;

        UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
        UI_ToolBox.TemperatureSensorClickedEvent += OnUI_Btn_TemperatureSensorClicked;
        
        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;
        
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
        MultimeterController.OnResistanceMeasureReadyAction += OnResistanceReady;
        
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        ControlPanelController.PowerOnOffActionWithBool += PowerOnOff;
        
        
        GetObject((int)DepthC_GameObj.TankValve).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 4)
            {
                OnStepMissionComplete(animationNumber: 4,delayAmount: new WaitForSeconds(4.5f));
                Logger.Log("벨브 잠금 및 유체 차단 애니메이션 재생 -----------------");
            }
        });
        
        GetObject((int)DepthC_GameObj.TS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 5)
            {
                OnStepMissionComplete(animationNumber: 5,delayAmount: new WaitForSeconds(4f));
                Logger.Log("커버 애니메이션 재생");
            }
        });
        
        
        
        
        
     
        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 4) return;
            
            
            //108옴 저항측정
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
                
                BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "저항 측정");
                SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
                //HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
                
                animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
            
            //접지
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
                BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "저항 측정");
                SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB, false);
                //HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalB);
            
                animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(TO_GROUNDING_TERMINAL, true);
                
                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });

            }

        
         
            
            

        }, Define.UIEvent.PointerDown);
       
        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            //108옴 저항측정
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
                if (!isAnodePut) return;
                BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "저항 측정");
               // HighlightBlink((int)DepthC_GameObj.TS_GroundingTerminalB);
                SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalB, false);
                
                animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(PROBE_TO_SCREWB, true);

                Action action = multimeterController.OnAllProbeSet;
                OnStepMissionComplete(animationNumber:8, delayAmount: new WaitForSeconds(6f),
                    delayedAction: action);
            }
        });

        GetObject((int)DepthC_GameObj.TS_GroundingTerminalA).BindEvent(() =>
        {
          
        });


        GetObject((int)DepthC_GameObj.TS_GroundingTerminalB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 4) return;
            if (!isAnodePut) return;
       
          
            if (Managers.ContentInfo.PlayData.Count == 9)
            {  
                
                Logger.Log("접지미션 수행 완료");
                
                animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(TO_GROUNDING_TERMINAL, true);
                multimeterController.OnAllProbeSetToGroundingTerminal();
         
                OnStepMissionComplete(animationNumber:9, delayAmount: new WaitForSeconds(6f));
                
            }

      
        });
        
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.ScrewDriverClickedEvent -= OnScrewDriverBtnClicked;
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
        UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
    }
    
    
    protected override void OnUI_MultimeterBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != 4) return;
    
        
        InitializeTool();
        CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
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
        Logger.Log("");
 
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


     
     protected override void OnUI_Btn_TemperatureSensorClicked()
     {
         if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5 )
         {
             OnStepMissionComplete( animationNumber:5,delayAmount:new WaitForSeconds(12.9f));
             GetObject((int)DepthC_GameObj.Indicator).GetComponent<IndicatorController>().ShowTemperature(7.5f);
             return;
         }


         if (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 2 )
         {
             Logger.Log($"4.2.1.2 온도센서 버튼 클릭 및 꺼내기 미션 수행");
             OnStepMissionComplete( animationNumber:2,delayAmount:new WaitForSeconds(3.8f));
          //   GetObject((int)DepthC_GameObj.Indicator).GetComponent<IndicatorController>().ShowTemperature(7.5f);
             return;
         }
        
     }
     
     protected override void OnResistanceReady()
     {
        
         
     }
     
     protected override void SetToolPos()
     {
         var distanceFromCamera = 0.09f;
         var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
             Input.mousePosition.y + _toolPosYOffset,
             distanceFromCamera));


         if (isDriverOn && CurrentActiveTool == (int)DepthC_GameObj.ElectricScrewdriver)
         {

             GetObject((int)DepthC_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
             GetObject((int)DepthC_GameObj.ElectricScrewdriver).transform.position = mousePosition;
         }
         else if (isMultimeterOn && CurrentActiveTool == (int)DepthC_GameObj.Multimeter && multimeterController.isResistanceMode)
         {
             GetObject((int)DepthC_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
             GetObject((int)DepthC_GameObj.Probe_Anode).SetActive(isMultimeterOn);
        
             if ((Managers.ContentInfo.PlayData.Count >= 8 && !isAnodePut)||(Managers.ContentInfo.PlayData.Depth1 ==4 && Managers.ContentInfo.PlayData.Count>=8 &&!isAnodePut))
             {
                 GetObject((int)DepthC_GameObj.Probe_Anode).transform.rotation =
                     defaultRotationMap[(int)DepthC_GameObj.Probe_Anode];
                
                 GetObject((int)DepthC_GameObj.Probe_Anode).transform.position = mousePosition;
             }

             if ((Managers.ContentInfo.PlayData.Count >= 8 && isAnodePut)||(Managers.ContentInfo.PlayData.Depth1 ==4  && Managers.ContentInfo.PlayData.Count>=98 &&isAnodePut))
             {
                 GetObject((int)DepthC_GameObj.Probe_Cathode).transform.rotation =
                     defaultRotationMap[(int)DepthC_GameObj.Probe_Cathode];
                
                 GetObject((int)DepthC_GameObj.Probe_Cathode).transform.position = mousePosition;
             }

         }
      





     }
     
     private void InitializeD2States()
     {

         _sceneStates = new Dictionary<int, ISceneState>
         { 
             { 4211, new  StateD21_1(this) },
             { 4212, new  StateD21_2(this) },
             { 4213, new  StateD21_3(this) },
             { 4214, new  StateD21_4(this) },
             { 4215, new  StateD21_5(this) },
             { 4216, new  StateD21_6(this) },
             { 4217, new  StateD21_7(this) },
             { 4218, new  StateD21_8(this) },
             { 4219, new  StateD21_9(this) },
             { 42110, new StateD21_10(this) },
             { 42111, new StateD21_11(this) },
             { 42112, new StateD21_12(this) },
             { 42113, new StateD21_13(this) },

       
         };
     }
}
