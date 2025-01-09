using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// 1. 검출스위차 --------------------------------기본적으로 센서 로직 상속
/// 2. 애니메이션 별도 구성
/// 3. BasesceneController 기본적으로 상속
/// </summary>
public class DepthD1_SceneController : DepthC1_SceneController
{
    public UI_Evaluation UIEvaluation { get; private set; }

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void Init()
    {
     //   if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000")
        SetDepthNum(); 
      
        base.Init();
        
        BindObject(typeof(DepthC1_GameObj));
        if(UIEvaluation==null) UIEvaluation = Managers.UI.ShowPopupUI<UI_Evaluation>();
        GetScrewColliders();
        InitializeD2States();
        DepthD11Init();
        contentController.OnDepth2Init(2); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
      
        BindAllClickableObj();
        
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD11Init;
        UI_Evaluation.OnRestartBtnOnEvalClicked += DepthD11Init;
    }

    public void DepthD11Init()
    {
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();


        UnBindEventAttatchedObj();
        UnBindInteractionEvent();
        SetScrewDriverSection();
        
        InitProbePos();
        
        
        SetDepthNum();
        ChangeState(1);
        PlayAnimation(1);
        
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
        currentActiveTool = (int)DepthC2_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false)
        {
            currentActiveTool = -1;
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


     
     protected override void OnUIBtnToolBoxTemperatureSensorClicked()
     {
         if (!Managers.evaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_TemperatureSensor)) return;
         
         if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5 )
         {
             OnStepMissionComplete( animationNumber:5,delayTimeAmount:new WaitForSeconds(12.9f));
             GetObject((int)DepthC2_GameObj.Indicator).GetComponent<IndicatorController>().ShowTemperature(7.5f);
             return;
         }


         if (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 2 )
         {
             Logger.Log($"4.2.1.2 온도센서 버튼 클릭 및 꺼내기 미션 수행");
             OnStepMissionComplete( animationNumber:2,delayTimeAmount:new WaitForSeconds(3.8f));
          //   GetObject((int)DepthC_GameObj.Indicator).GetComponent<IndicatorController>().ShowTemperature(7.5f);
             return;
         }
        
     }
     

     
     protected override void SetToolPos()
     {
         var distanceFromCamera = 0.09f;
         var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
             Input.mousePosition.y + _toolPosYOffset,
             distanceFromCamera));


         if (isDriverOn && currentActiveTool == (int)DepthC2_GameObj.ElectricScrewdriver)
         {

             GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
             GetObject((int)DepthC2_GameObj.ElectricScrewdriver).transform.position = mousePosition;
         }
         else if (isMultimeterOn && currentActiveTool == (int)DepthC2_GameObj.Multimeter && multimeterController.isResistanceMode)
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
             { 4111, new  StateD11_1(this) },
             { 4112, new  StateD11_2(this) },
             { 4113, new  StateD11_3(this) },
             { 4114, new  StateD11_4(this) },
             { 4115, new  StateD11_5(this) },
             { 4116, new  StateD11_6(this) },
             { 4117, new  StateD11_7(this) },
             { 4118, new  StateD11_8(this) },
             { 4119, new  StateD11_9(this) },
             { 41110, new StateD11_10(this) },


       
         };
     }
}
