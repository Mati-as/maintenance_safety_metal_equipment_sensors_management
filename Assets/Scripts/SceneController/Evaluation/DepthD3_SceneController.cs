using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     1. 온도센서 --------------------------------기본적으로 센서 로직 상속
///     2. 애니메이션 별도 구성
///     3. BasesceneController 기본적으로 상속
/// </summary>
public class DepthD3_SceneController : DepthC3_SceneController
{
    public UI_Evaluation UIEvaluation { get; private set; }

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 3;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void Init()
    {
        //   if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000")
        SetDepthNum();

        base.Init();
        BindObject(typeof(DepthC3_GameObj));
        if (UIEvaluation == null) UIEvaluation = Managers.UI.ShowPopupUI<UI_Evaluation>();
        
        
        GetScrewColliders();
        InitializeD2States();
       // DepthD31Init();
        contentController.OnDepth2Init((int)Define.DepthC_Sensor.PressureSensor); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
        
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD31Init;
        UI_Evaluation.OnRestartBtnOnEvalClicked += DepthD31Init;
    }
    

    public void DepthD31Init()
    {
        UnBindEventAttatchedObj();


        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();

        PreCommonInit();

        SetPressureSensorCurrentCheckMultimeterSection();
        SetScrewDriverSection();
        InitScrews();

        InitScrewForConductiveCheck();
        InitProbePos();
        SetDefaultTransform();
        BindInteractionEvent();


        foreach (DepthC3_GameObj obj in Enum.GetValues(typeof(DepthC3_GameObj)))
        {
            if (GetObject((int)obj) == null ||
                obj == DepthC3_GameObj.MultimeterHandleHighlight||
                obj == DepthC3_GameObj.Multimeter)
            {
                Logger.Log($"클릭해도 평가대상 오브젝트가 아닙니다 : {(DepthC3_GameObj)obj}");
                continue;
            }

            GetObject((int)obj).BindEvent(() =>
            {
                Managers.EvaluationManager.CheckIfAnswerIsCorrect((int)obj);
                Logger.Log($"Evaluation Event Bound : {obj}");
            });
        }


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 1)
            {
            }
        });

        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 2)
            {
            }
        });

        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 3)
            {
            }
        });

        GetObject((int)DepthC3_GameObj.PressureSensorWaterPipeValve).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 4)
            {
                OnStepMissionComplete(animationNumber:4);
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 5)
            {
            }
        });

        GetObject((int)DepthC3_GameObj.NewPressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 6) 
            {
        
                OnStepMissionComplete(animationNumber:6);
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 7)
            {
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 10)
            {
            }
        });


        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 11)
            {
            }
        });

        BindEventForPsCalibrator();
        LateCommonInit();
        SetDepthNum();
       // ChangeState(1);
        PlayAnimation(1);
        BindHLForAllClickableObj();
    }
    
    protected override void OnCurrentModeSet()
    {
         if(Managers.ContentInfo.PlayData.Count == 8) OnStepMissionComplete(animationNumber:8);
        
    }

    protected override void BindEventForPsCalibrator()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != (int)Define.Depth.Evaluation)
            Logger.Break("Current Depth has to be Evalutation");

        GetObject((int)DepthC3_GameObj.Btn_F3).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 12)
            {
                pressureCalibratorController.OnBtn_F3Clicked_ZPressureOrLoopPower();
                OnStepMissionComplete(animationNumber: 12);
            }

            if (Managers.ContentInfo.PlayData.Count == 16)
            {
                pressureCalibratorController.OnBtn_F3Clicked_ZPressureOrLoopPower();
                ChangeTooltipText((int)DepthC3_GameObj.Btn_F4, "F4 : Continue");
            }
        });

        //BindHighlight((int)DepthC3_GameObj.Btn_F4,"F4");
        GetObject((int)DepthC3_GameObj.Btn_F4).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 16)
            {
                pressureCalibratorController.OnBtn_F4Clicked_ATContinue();
                OnStepMissionComplete(animationNumber: 16);
            }


            if (Managers.ContentInfo.PlayData.Count == 20)
            {
                pressureCalibratorController.OnBtn_F4Clicked_ATContinue();
                OnStepMissionComplete(animationNumber: 20);
            }

            //AutoTest
            if (Managers.ContentInfo.PlayData.Count == 19)
                OnStepMissionComplete(animationNumber: 19);
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Vent,"VENT");
        GetObject((int)DepthC3_GameObj.Btn_Vent).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 11)
            {
                pressureCalibratorController.OnVentClicked();
                OnStepMissionComplete(animationNumber: 11);
            }
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Tasks,"TASKS");
        GetObject((int)DepthC3_GameObj.Btn_Tasks).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 11)
            {
                pressureCalibratorController.OnTasksBtnClicked();
                OnStepMissionComplete(animationNumber: 11);
            }
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Arrow_Down,"DOWN");
        GetObject((int)DepthC3_GameObj.Btn_Arrow_Down).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 15)
            {
                pressureCalibratorController.OnDownBtnClicked();
                OnStepMissionComplete(animationNumber: 15);
            }

            if (Managers.ContentInfo.PlayData.Count == 18)
                pressureCalibratorController.OnDownBtnClicked();
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Arrow_Up,"UP");


        //BindHighlight((int)DepthC3_GameObj.Btn_Enter,"ENTER");
        GetObject((int)DepthC3_GameObj.Btn_Enter).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 16)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                if (pressureCalibratorController.is100PsiSet) OnStepMissionComplete(animationNumber: 16);
            }

            if (Managers.ContentInfo.PlayData.Count == 18)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                OnStepMissionComplete(animationNumber: 18);
            }


            if (Managers.ContentInfo.PlayData.Count == 18)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                OnStepMissionComplete(animationNumber: 18);
            }
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Number_One,"1");
        GetObject((int)DepthC3_GameObj.Btn_Number_One).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 13)
            {
                pressureCalibratorController.OnBtnNumberOneClicked();
                OnStepMissionComplete(animationNumber: 13);
            }

            if (Managers.ContentInfo.PlayData.Count == 16)
                pressureCalibratorController.OnBtnNumberOneClicked();
        });


        //BindHighlight((int)DepthC3_GameObj.Btn_Number_Zero,"0");
        GetObject((int)DepthC3_GameObj.Btn_Number_Zero).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 16)
                pressureCalibratorController.OnBtnNumberZeroClicked();
        });
    }


    private new void OnDestroy()
    {
        base.OnDestroy();
        UnBindInteractionEvent();
    }

    protected override void UnBindInteractionEvent()
    {
        base.UnBindInteractionEvent();
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD31Init;
    }

    public override void OnElectricScrewdriverBtnClicked()
    {
        if (!Managers.EvaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_ElectricScrewdriver)) return;
        base.OnElectricScrewdriverBtnClicked();
    }
    
    protected override void OnPressureCalibratorClicked()
    {
        if (Managers.ContentInfo.PlayData.Count == 10) 
        {
            pressureCalibratorController.BootPressureCalibrator();
            OnStepMissionComplete(animationNumber:10);
        }
    }

    protected override void OnUIToolBoxMultimeterBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != 4) return;

        if (!Managers.EvaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_Multimeter)) return;


        InitializeTool();
        currentActiveTool = (int)DepthC2_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false) currentActiveTool = -1;

        if (Managers.ContentInfo.PlayData.Count == 7) OnStepMissionComplete(animationNumber: 7);
        ;
    }

    protected override void OnToolBoxClicked()
    {
//        Logger.Log("");
    }

    protected override void PowerOnOff(bool isOn)
    {
        if (Managers.ContentInfo.PlayData.Count == 3)
            if (!isOn)
                OnStepMissionComplete(animationNumber: 3);

        if (Managers.ContentInfo.PlayData.Count == 11)
            if (isOn)
                OnStepMissionComplete(animationNumber: 11);
    }


    // protected override void SetToolPos()
    // {
    //     var distanceFromCamera = 0.09f;
    //     var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
    //         Input.mousePosition.y + _toolPosYOffset,
    //         distanceFromCamera));
    //
    //
    //     if (isDriverOn && currentActiveTool == (int)DepthC2_GameObj.ElectricScrewdriver)
    //     {
    //         GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
    //         GetObject((int)DepthC2_GameObj.ElectricScrewdriver).transform.position = mousePosition;
    //     }
    //     else if (isMultimeterOn && currentActiveTool == (int)DepthC2_GameObj.Multimeter &&
    //              multimeterController.isCurrentCheckMode)
    //     {
    //         GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
    //         GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(isMultimeterOn);
    //
    //         if ((Managers.ContentInfo.PlayData.Count >= 8 && !isAnodePut) ||
    //             (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count >= 8 && !isAnodePut))
    //         {
    //             GetObject((int)DepthC2_GameObj.Probe_Anode).transform.rotation =
    //                 defaultRotationMap[(int)DepthC2_GameObj.Probe_Anode];
    //
    //             GetObject((int)DepthC2_GameObj.Probe_Anode).transform.position = mousePosition;
    //         }
    //
    //         if ((Managers.ContentInfo.PlayData.Count >= 8 && isAnodePut) ||
    //             (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count >= 98 && isAnodePut))
    //         {
    //             GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation =
    //                 defaultRotationMap[(int)DepthC2_GameObj.Probe_Cathode];
    //
    //             GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.position = mousePosition;
    //         }
    //     }
    // }

    private void InitializeD2States()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 4311, new DepthD31_State_1(this) },
            { 4312, new DepthD31_State_2(this) },
            { 4313, new DepthD31_State_3(this) },
            { 4314, new DepthD31_State_4(this) },
            { 4315, new DepthD31_State_5(this) },
            { 4316, new DepthD31_State_6(this) },
            { 4317, new DepthD31_State_7(this) },
            { 4318, new DepthD31_State_8(this) },
            { 4319, new DepthD31_State_9(this) },
            { 43110, new DepthD31_State_10(this) },
            { 43111, new DepthD31_State_11(this) },
            { 43112, new DepthD31_State_12(this) },
            { 43113, new DepthD31_State_13(this) },
            { 43114, new DepthD31_State_14(this) },
            { 43115, new DepthD31_State_15(this) },
            { 43116, new DepthD31_State_16(this) },
            { 43117, new DepthD31_State_17(this) },
            { 43118, new DepthD31_State_18(this) },
            { 43119, new DepthD31_State_19(this) },
            // { 43120, new DepthD31_State_20(this) },
            // { 43121, new DepthD31_State_21(this) }
        };
    }
}