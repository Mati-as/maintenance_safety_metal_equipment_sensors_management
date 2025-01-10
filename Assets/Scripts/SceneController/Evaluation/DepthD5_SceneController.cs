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
        DepthD51Init();
        contentController.OnDepth2Init((int)Define.DepthC_Sensor.LevelSensor); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
      
        
        
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD51Init;
        UI_Evaluation.OnRestartBtnOnEvalClicked += DepthD51Init;
    }

      public void DepthD51Init()
    {
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();


        foreach (DepthC1_GameObj obj in Enum.GetValues(typeof(DepthC1_GameObj)))
        {
            if (GetObject((int)obj) == null ||
                //정답오답 여부에 포함되지 않는 오브젝트들을 할당합니다. 
                obj == DepthC1_GameObj.LeverHandleReadjustTargetPos ||
                obj == DepthC1_GameObj.MultimeterHandleHighlight ||
                obj == DepthC1_GameObj.LimitSwitch
               )
            {
                Logger.Log($"no object is set: {obj}");
                continue;
            }

            GetObject((int)obj).BindEvent(() =>
            {
                Managers.EvaluationManager.CheckIfAnswerIsCorrect((int)obj);
                Logger.Log($"Evaluation Event Bound : {obj}");
            });
        }


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 1)
            {
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 2)
            {
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 3)
            {
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 4)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 5)
            {
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 6)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 7)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 8)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 9)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 10)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 11)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 12)
            {
            }
        });


        UnBindEventAttatchedObj();
        UnBindInteractionEvent();
        SetScrewDriverSection();
        InitProbePos();

        SetDepthNum();
        ChangeState(1);
        PlayAnimation(1);
    }

    protected override void UnBindInteractionEvent()
    {
        base.UnBindInteractionEvent();
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD51Init;
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
        UnBindInteractionEvent();
    }



    public override void OnElectricScrewdriverBtnClicked()
    {
        if (!Managers.EvaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_ElectricScrewdriver)) return;
        base.OnElectricScrewdriverBtnClicked();
        
    }

    protected override void OnUIToolBoxMultimeterBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != 4) return;

        if (!Managers.EvaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_Multimeter)) return;
       
        
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
         else if (isMultimeterOn && currentActiveTool == (int)DepthC2_GameObj.Multimeter && multimeterController.isCurrentCheckMode)
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
