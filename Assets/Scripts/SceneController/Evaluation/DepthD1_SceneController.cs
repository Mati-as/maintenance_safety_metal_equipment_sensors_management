using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
///     1. 검출스위차 --------------------------------기본적으로 센서 로직 상속
///     2. 애니메이션 별도 구성
///     3. BasesceneController 기본적으로 상속
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
        if (UIEvaluation == null) UIEvaluation = Managers.UI.ShowPopupUI<UI_Evaluation>();

        InitializeD11States();


        DepthD11Init();

         // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.


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

        defaultRotationMap.TryAdd((int)DepthC1_GameObj.Probe_Cathode,
            GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC1_GameObj.Probe_Anode,
            GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation);
        controlPanel = GetObject((int)DepthC1_GameObj.PowerHandle).GetComponent<ControlPanelController>();

        C1_PreCommonObjInit();


        BindInteractionEvent();
        foreach (DepthC1_GameObj obj in Enum.GetValues(typeof(DepthC1_GameObj)))
        {
            if (GetObject((int)obj) == null ||
                //정답오답 여부에 포함되지 않는 오브젝트들을 할당합니다. 
                obj == DepthC1_GameObj.ConnectionScrewA ||
                obj == DepthC1_GameObj.ConnectionScrewB ||
                obj == DepthC1_GameObj.ConnectionScrewC ||
                obj == DepthC1_GameObj.ConnectionScrewD ||
                obj == DepthC1_GameObj.LeverHandleReadjustTargetPos ||
                obj == DepthC1_GameObj.MultimeterHandleHighlight ||
                obj == DepthC1_GameObj.LimitSwitch ||
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

        // GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        // {
        //     if (Managers.ContentInfo.PlayData.Count == 2)
        //     {
        //      //리밋스위치 버튼 클릭시 (UIToolBox)   
        //     }
        // });

        GetObject((int)DepthC1_GameObj.PowerHandle).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 3) OnStepMissionComplete(animationNumber: 3);
        });

        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 4) OnStepMissionComplete(animationNumber: 4);
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 5)
            {
                //전동드라이버를 통한 해체 5
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 6)
            {
                //레버위치 조절 6
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 7)
            {
                // 멀티미터 버튼 클릭
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.ConductiveCheckModeBtn).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
                Managers.Sound.Play(SoundManager.Sound.Effect, "Audio/Object/MultermeterConductiveModeClick");
                Managers.Sound.Play(SoundManager.Sound.Effect, "Audio/Object/beep_01");

                if (Managers.ContentInfo.PlayData.Count == 9 && !multimeterController.isConductive &&
                    multimeterController.isResistanceMode)
                {
                    Logger.Log("통전버튼 전환미션 완료");
                    multimeterController.isConductive = true;
                    OnStepMissionComplete(animationNumber: 9);
                }
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 10)
            {
            }
        });

        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 11)
            {
            }
        });


        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 12)
            {
            }
        });


        SetScrewDriverSection();
        SetMultimeterSection();
        GetScrewColliders();
        InitScrews();
        InitProbePos();

        BindHLForAllClickableObj();
        limitSwitchPivotController.InitLimitSwitch();
        InitScrewForConductiveCheck();
        SetDepthNum();
        ChangeState(1);
        PlayAnimation(1);
        
        contentController.OnDepth2Init((int)Define.DepthC_Sensor.LimitSwitch);
    }

    protected override void InitScrewForConductiveCheck()
    {
        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation && Managers.ContentInfo.PlayData.Count == 10))
               
            {
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_A, true);

                ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewB, "접속단자 B");

                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA);
                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB, false);

                BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewB);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
        });

        GetObject((int)DepthC1_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation && Managers.ContentInfo.PlayData.Count == 10))
            {
                if (!isAnodePut) return;

                Logger.Log("Probe Set == 16");
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_B, true);

                Action action = multimeterController.OnAllProbeSetOnConductiveCheckMode;
                if (Managers.ContentInfo.PlayData.Count == 10)
                    OnStepMissionComplete(animationNumber: 10, delayTimeAmount: new WaitForSeconds(4f),
                        ActionBeforeDelay: action);
  
            }
        }, Define.UIEvent.PointerDown);


        GetObject((int)DepthC1_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation && Managers.ContentInfo.PlayData.Count == 11))
            {
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_C, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() =>
                {
                    ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewD, "접속단자 D");
                    SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD, false);
                    BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewD);

                    isAnodePut = true;
                });
            }
        });


        GetObject((int)DepthC1_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation && Managers.ContentInfo.PlayData.Count == 11))
            {
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, true);
                Action action = multimeterController.OnAllProbeSetOnConductiveCheckModeNoSound;
                Action noCurrentAction = multimeterController.OnAllProbeSetOnConductiveCheckMode;


                if (Managers.ContentInfo.PlayData.Count == 11)
                    OnStepMissionComplete(animationNumber: 11, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay: action);
             


                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
        });
    }

    protected override void UnBindInteractionEvent()
    {
        base.UnBindInteractionEvent();

        UI_Evaluation.OnRestartBtnOnEvalClicked -= OnDepth3Clicked;
        UI_Evaluation.OnRestartBtnOnEvalClicked -= DepthD11Init;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        UnBindInteractionEvent();
    }

    protected override void OnTargetPosArrive()
    {
        if (Managers.ContentInfo.PlayData.Count == 6)
            OnStepMissionComplete(animationNumber: 6, delayTimeAmount: new WaitForSeconds(3f));
    }


    protected override void OnConductiveModeSet()
    {
        if (Managers.ContentInfo.PlayData.Count == 8)
            OnStepMissionComplete(animationNumber: 8, delayTimeAmount: new WaitForSeconds(3f));
    }


    protected override void OnToolBoxLimitSwitchBtnOnUIClicked()
    {
        if (Managers.ContentInfo.PlayData.Count == 2) OnStepMissionComplete(animationNumber: 2);
    }


    public override void OnElectricScrewdriverBtnClicked()
    {
        if (!Managers.EvaluationManager.CheckIfAnswerIsCorrect(UI_ToolBox.Btns.Btn_ElectricScrewdriver)) return;
        base.OnElectricScrewdriverBtnClicked();
    }


    protected override void OnToolBoxClicked()
    {
//        Logger.Log("");
    }

    protected override void OnPowerOnOff(bool isOn)
    {
        if (Managers.ContentInfo.PlayData.Count == 3)
            if (!isOn)
                OnStepMissionComplete(animationNumber: 3);

        if (Managers.ContentInfo.PlayData.Count == 12)
            if (isOn)
                OnStepMissionComplete(animationNumber: 12);
    }


    protected override void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
            Input.mousePosition.y + _toolPosYOffset,
            distanceFromCamera));


        if (isDriverOn && currentActiveTool == (int)DepthC1_GameObj.ElectricScrewdriver)
        {
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).transform.position = mousePosition;
        }

        else if (isMultimeterOn && currentActiveTool == (int)DepthC1_GameObj.Multimeter &&
                 multimeterController.isResistanceMode && multimeterController.isConductive)
        {
            GetObject((int)DepthC1_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC1_GameObj.Probe_Anode).SetActive(isMultimeterOn);

            if ((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation &&
                 Managers.ContentInfo.PlayData.Count == 10 &&
                 !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation &&
                    Managers.ContentInfo.PlayData.Count == 11 &&
                    !isAnodePut)
               )
            {
                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC1_GameObj.Probe_Anode];

                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if
            (
                (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation &&
                 Managers.ContentInfo.PlayData.Count == 10 &&
                 isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation &&
                    Managers.ContentInfo.PlayData.Count == 11 &&
                    isAnodePut)
            )
            {
                GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation =
                    defaultRotationMap[(int)DepthC1_GameObj.Probe_Cathode];

                GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.position = mousePosition;
            }
        }
    }

    private void InitializeD11States()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 4111, new StateD11_1(this) },
            { 4112, new StateD11_2(this) },
            { 4113, new StateD11_3(this) },
            { 4114, new StateD11_4(this) },
            { 4115, new StateD11_5(this) },
            { 4116, new StateD11_6(this) },
            { 4117, new StateD11_7(this) },
            { 4118, new StateD11_8(this) },
            { 4119, new StateD11_9(this) },
            { 41110, new StateD11_10(this) },
            { 41111, new StateD11_11(this) },
            { 41112, new StateD11_12(this) },
            { 41113, new StateD11_13(this) }
        };
    }
}