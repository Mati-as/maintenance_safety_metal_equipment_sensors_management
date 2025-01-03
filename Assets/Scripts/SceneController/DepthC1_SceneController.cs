using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;


/// <summary>
///     주의사항
///     1.초기화 및 이벤트성 함수만 담도록 구성합니다.
///     2.상태에 따른 애니메이션 수행은 최대한 DepthCState에 구성합니다.
/// </summary>

/// <summary>
/// 검출스위치
/// </summary>

public enum DepthC1_GameObj
{
    // Common------------------
    LimitSwitchLookAt,
    ElectricScrewdriver,
    Multimeter,
    MultimeterHandleHighlight,
    ConductiveCheckModeBtn,
    Indicator,
    Probe_Anode, // negative
    Probe_Cathode, // positive,
    LimitSwitch,
    Lever_Handle,
    LeverHandleReadjustTargetPos,
    Limitswitch_ArmPivot,
    LS_Cover,
    LeverScrew,
    ConnectionScrewA,
    ConnectionScrewB,
    ConnectionScrewC,
    ConnectionScrewD,
    ConveyorCube,
    PowerHandle,
    NewLimitSwitch
}

public class DepthC1_SceneController : Base_SceneController
{
    private readonly int UNWOUND_COUNT_GOAL = 4;
    private int _unwoundCount;
    private C1_LimitSwitchPivotController InitLimitSwitchController;

    public C1_LimitSwitchPivotController limitSwitchPivotController
    {
        get
        {
            return GetObject((int)DepthC1_GameObj.LimitSwitch).GetComponent<C1_LimitSwitchPivotController>();
        }
    }


    public void InitTransform(DepthC1_GameObj obj, bool isAll =false)
    {
        
       
        if (GetObject((int)obj) == null || !defaultPositionMap.ContainsKey((int)obj))
        {
            Logger.LogWarning($"{obj}'s Transform is null.. or key is null Initialize Transform Value Setting Failed");
            return;
        }
        
        GetObject((int)obj).transform.position = defaultPositionMap[(int)obj];
        GetObject((int)obj).transform.rotation = defaultRotationMap[(int)obj];

        if (isAll)
        {
            for (int i = 0; i < Enum.GetValues(typeof(DepthC1_GameObj)).Length; i++)
            {
                defaultPositionMap.TryAdd(i, GetObject(i).transform.position);
                defaultRotationMap.TryAdd(i, GetObject(i).transform.rotation);
            }   
        }
   

    }



    /// <summary>
    /// 1.Depth1,2,3 통틀어서, 초기위치가 불변인 객체만 사용권장
    /// (예를들어, LeverHandle의 초기위치는 depth1,2에서 다르므로 사용권장하지않음)
    /// 2.위치가 새롭게 변경되는 경우 Animation으로 위치,각도를 초기화거나, 별도 로직 추가 사용권장.
    /// </summary>
    public void SetDefaultTransform()
    {
        if (defaultPositionMap == null) defaultPositionMap = new Dictionary<int, Vector3>();
        if (defaultRotationMap == null) defaultRotationMap = new Dictionary<int, Quaternion>();
        
        for (int i = 0; i < Enum.GetValues(typeof(DepthC1_GameObj)).Length; i++)
        {
            if (GetObject(i) == null)
            {
                Logger.LogWarning($"{(DepthC1_GameObj)i}'s Transform is null.. Default Value Setting Failed");
                continue;
            }
            
            defaultPositionMap.TryAdd(i, GetObject(i).transform.position);
            defaultRotationMap.TryAdd(i, GetObject(i).transform.rotation);
        }    
        
    }
    
    
    public void SetAnimator(DepthC1_GameObj obj)
    {
        if (animatorMap == null) animatorMap = new Dictionary<int, Animator>();

        animatorMap.TryAdd((int)obj, GetObject((int)obj).GetComponent<Animator>());
    }

    private IndicatorController _indicator;
    public IndicatorController indicator
    {
        get
        {
            if(_indicator == null)
                _indicator = GetObject((int)DepthC1_GameObj.Indicator).GetComponent<IndicatorController>();
            return _indicator;
        }
        
    }


    public bool isAnodePut; // 음극단자 설정을 위한 bool값입니다.


    private Vector3 _probeDefaultPos;

    public int unwoundCount
    {
        get => _unwoundCount;

        set
        {
            _unwoundCount = value;
            if (_unwoundCount >= UNWOUND_COUNT_GOAL)
            {
                if (Managers.ContentInfo.PlayData.Depth3 == 4 && Managers.ContentInfo.PlayData.Count == 5)
                {
                    Logger.Log($"평가하기: 커버열고 모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    OnStepMissionComplete(animationNumber:6);
                    CurrentActiveTool = -1;
                    isDriverOn = false;
                    _unwoundCount = 0;
                }

                if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 5)
                {
                    Logger.Log($"모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    OnStepMissionComplete(animationNumber:5);
                    _unwoundCount = 0;//초기화 
                }
                
                // if (Managers.ContentInfo.PlayData.Depth3 == 3  && Managers.ContentInfo.PlayData.Count == 6)
                // {
                //     Logger.Log($"모든 나사 풀림 (6) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                //     OnStepMissionComplete(animationNumber:6);
                //     _unwoundCount = 0;//초기화 
                // }
            }
   


        }
    }
    private int _woundCount;

    public int woundCount
    {
        get => _woundCount;

        set
        {
            _woundCount = value;
            Logger.Log($"나사 조임: {_woundCount}개");
            if (_woundCount >= UNWOUND_COUNT_GOAL)
            {
                if (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 10)
                {
                    Logger.Log(
                        $"평가하기: 모든 나사 조임 (10) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _woundCount}");
                    OnStepMissionComplete(animationNumber: 10);
                    _woundCount = 0;
                }

                if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 8)
                {
                    Logger.Log($"훈련모드 나사 조임 (12) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _woundCount}");
                    OnStepMissionComplete(animationNumber: 8);
                    _woundCount = 0; //초기화 
                }
            }
        }
    }


    [Range(-500, 500f)] public float _toolPosXOffset = 0.3f;
    [Range(-500f, 500f)] public float _toolPosYOffset = -0.3f;

    public void ScrewWoundCountInit()
    {
        woundCount = 0;
        unwoundCount = 0;
    }
    public static readonly int UNWIND = Animator.StringToHash("Unwind");

    public static readonly int TO_SCREW_A = Animator.StringToHash("ToScrewA");
    public static readonly int TO_SCREW_B = Animator.StringToHash("ToScrewB");
    public static readonly int TO_SCREW_C = Animator.StringToHash("ToScrewC");
    public static readonly int TO_SCREW_D = Animator.StringToHash("ToScrewD");
    public static readonly int TO_LEVER_SCREW = Animator.StringToHash("ToLeverScrew");
    public static readonly int MULTIMETER_ON = Animator.StringToHash("On");



    public static readonly int TO_GROUNDING_TERMINAL = Animator.StringToHash("GroundingTerminal");

    public ControlPanelController controlPanel;

    private Collider[] _screwColliders;

    
    /// <summary>
    /// 나사의 클릭기능을 원활하게 하기 위한 콜라이더 설정 로직입니다.
    /// </summary>
    public void GetScrewColliders()
    {
        var screwCount = 20;
        _screwColliders = new Collider[screwCount];
        _screwColliders[(int)DepthC1_GameObj.ConnectionScrewA] = GetObject((int)DepthC1_GameObj.ConnectionScrewA).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.ConnectionScrewB] = GetObject((int)DepthC1_GameObj.ConnectionScrewB).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.ConnectionScrewC] = GetObject((int)DepthC1_GameObj.ConnectionScrewC).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.ConnectionScrewD] = GetObject((int)DepthC1_GameObj.ConnectionScrewD).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.LeverScrew] = GetObject((int)DepthC1_GameObj.LeverScrew).GetComponent<Collider>();
    }
    
    
    /// <summary>
    /// 나사의 클릭기능을 원활하게 하기 위한 콜라이더 설정 로직입니다.
    /// </summary>
    public void TurnOffCollider(int objEnumToInt)
    {
        _screwColliders[(int)objEnumToInt].enabled = false;
    }

    /// <summary>
    /// 나사의 클릭기능을 원활하게 하기 위한 콜라이더 설정 로직입니다.
    /// </summary>
    public void TurnOnCollidersAndInit()
    {
        foreach (var collider in _screwColliders)
        {
            if(collider !=null) collider.enabled = true;
        }

        ScrewWoundCountInit();
    }
    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") 
            SetDepthNum(); //개발용

        base.Init();
        BindObject(typeof(DepthC1_GameObj));
        
        
        
        SetAnimator(DepthC1_GameObj.ConnectionScrewA);
        SetAnimator(DepthC1_GameObj.ConnectionScrewB);
        
        
        SetDefaultTransform();
        
        InitializeC1States();
        GetScrewColliders();
        contentController.OnDepth2Init(1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
        
    }
    private void LateCommonInit()
    {
        
        ClearTool();
        isAnodePut = false;
        indicator.ShowNothing();
    }


    
    
    public void DepthC13Init()
    {
        PreCommonInit();
      

        SetDefaultTransform();
        
        BindInteractionEvent();
        InitProbePos();
        InitScrews();
       
        
        SetMultimeterSection();
        SetScrewDriverSection();
        InitScrewForConductiveCheck();
        InitProbePos();
        
        
        
        
        limitSwitchPivotController.InitLimitSwitch();
        
        
        BindHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,"");
        BindHighlight((int)DepthC1_GameObj.ConductiveCheckModeBtn,"통전모드 전환 버튼");
        BindHighlight((int)DepthC1_GameObj.Lever_Handle,"리밋스위치 레버");
        BindHighlight((int)DepthC1_GameObj.MultimeterHandleHighlight,"저항측정 모드로 설정");
        BindHighlight((int)DepthC1_GameObj.LS_Cover,"검출스위치 덮개");
        BindHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,"");
        BindHighlight((int)DepthC1_GameObj.Lever_Handle,"레버 길이 조절");
        BindHighlight((int)DepthC1_GameObj.PowerHandle,"전원");
        BindHighlight((int)DepthC1_GameObj.LimitSwitch,"리밋스위치 교체");
        
        
        BindHighlight((int)DepthC1_GameObj.LeverScrew,"조절 레버");
        
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewA,"나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewB,"접속 단자");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewC,"나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewD,"나사");
        
        GetObject((int)DepthC1_GameObj.PowerHandle).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 3)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 3);
            }
        });
        
        
        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 4)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 4);
            }
        });
        
        GetObject((int)DepthC1_GameObj.LimitSwitch).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 6)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete(animationNumber: 6);
            }
        });
        
        


        
        InitTransform(DepthC1_GameObj.LS_Cover);
        
        LateCommonInit();
    }
    

    public void InitProbePos()
    {
        GetObject((int)DepthC1_GameObj.Probe_Anode).gameObject.SetActive(true);
        GetObject((int)DepthC1_GameObj.Probe_Cathode).gameObject.SetActive(true);
        
        GetObject((int)DepthC1_GameObj.Probe_Anode).transform.position = _probeDefaultPos;
        GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.position = _probeDefaultPos;
        
        GetObject((int)DepthC1_GameObj.Probe_Anode).gameObject.SetActive(false);
        GetObject((int)DepthC1_GameObj.Probe_Cathode).gameObject.SetActive(false);
    }
    protected override void UnBindEventAttatchedObj()
    {
        base.UnBindEventAttatchedObj();
        UnBindInteractionEvent();
    }
    public void DepthC12Init()
    {
       
        PreCommonInit();

        SetDefaultTransform();
        InitScrews();
        BindInteractionEvent();
        InitProbePos();
    
       
        
        SetMultimeterSection();
        SetScrewDriverSection();
        limitSwitchPivotController.InitLimitSwitch();
        
        BindHighlight((int)DepthC1_GameObj.LS_Cover,"검출스위치 덮개");
        BindHighlight((int)DepthC1_GameObj.LeverScrew,"조절 레버");
        BindHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,"");
        BindHighlight((int)DepthC1_GameObj.Lever_Handle,"레버 길이 조절");
        

        
        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 3)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 3);
            }
        });
        
        
        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 11)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 11);
            }
        });

        
        InitTransform(DepthC1_GameObj.LS_Cover);
        
        LateCommonInit();
    }



    protected override void PreCommonInit()
    {
        base.PreCommonInit();
       
        
        defaultRotationMap.TryAdd((int)DepthC1_GameObj.Probe_Cathode,GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC1_GameObj.Probe_Anode,GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation);
        C1_PreCommonObjInit();
        UnBindEventAttatchedObj();
    }
    private void C1_PreCommonObjInit()
    {
        GetObject((int)DepthC1_GameObj.ConveyorCube).SetActive(false);
        GetObject((int)DepthC1_GameObj.NewLimitSwitch).SetActive(false);
        GetObject((int)DepthC1_GameObj.LS_Cover).SetActive(true);
       
    }

    public void DepthC11Init()
    {
      
        PreCommonInit();
        
       
       
        SetMultimeterSection();
        SetScrewDriverSection();
        InitScrewForConductiveCheck();
        InitProbePos();
        
        SetDefaultTransform();
        BindInteractionEvent();
        
        
        limitSwitchPivotController.InitLimitSwitch();
        
        
        BindHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,"");
        BindHighlight((int)DepthC1_GameObj.ConductiveCheckModeBtn,"통전모드 전환 버튼");
        BindHighlight((int)DepthC1_GameObj.MultimeterHandleHighlight,"저항측정 모드로 설정");
            
        BindHighlight((int)DepthC1_GameObj.Lever_Handle,"리밋스위치 레버");
        GetObject((int)DepthC1_GameObj.Lever_Handle).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 5)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.Lever_Handle, 5);
            }
        });
        
        
        BindHighlight((int)DepthC1_GameObj.LS_Cover,"검출스위치 덮개");
                
        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 8)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 8);
            }
        });


        BindHighlight((int)DepthC1_GameObj.ConnectionScrewA,"접속나사 확인");
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
                Logger.Log("MissionComplete limitswitch");
      
                OnStepMissionComplete((int)DepthC1_GameObj.ConnectionScrewA, 9);
            }
        });
        
        GetObject((int)DepthC1_GameObj.ConductiveCheckModeBtn).BindEvent(() =>
        {
            Managers.Sound.Play(SoundManager.Sound.Effect,"Audio/Object/MultermeterConductiveModeClick");
            Managers.Sound.Play(SoundManager.Sound.Effect,"Audio/Object/beep_01");
           
            if (Managers.ContentInfo.PlayData.Count == 14 &&  !multimeterController.isConductive)
            {
                Logger.Log("통전버튼 전환미션 완료");
                multimeterController.isConductive = true;
                OnStepMissionComplete( animationNumber:14);
            }
        });
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewB,"접속나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewC,"접속나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewD,"접속나사");
        
        
        
        LateCommonInit();
        
    }
    

    
    protected virtual void OnUIBtnToolBoxTemperatureSensorClicked()
    {
        
    }

    protected virtual void PowerOnOff(bool isOn)
    {
    
    }

    private void BindInteractionEvent()
    {
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        ControlPanelController.PowerOnOffActionWithBool += PowerOnOff;
                
        
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent -= OnUIBtnToolBoxTemperatureSensorClicked;
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent += OnUIBtnToolBoxTemperatureSensorClicked;

        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
        
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;
        
        
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceModeSet;
        MultimeterController.OnResistanceMeasureReadyAction += OnResistanceModeSet;

        MultimeterController.OnConductiveModeReady -= OnConductiveModeSet;
        MultimeterController.OnConductiveModeReady += OnConductiveModeSet;
        
       UI_ToolBox.ToolBox_LimitSwitchSensorClickedEvent -= OnToolBoxLimitSwitchBtnOnUIClicked;
       UI_ToolBox.ToolBox_LimitSwitchSensorClickedEvent += OnToolBoxLimitSwitchBtnOnUIClicked;

        C1_LimitSwitchPivotController.OnTargetPosArrive -= OnTargetPosArrive;
        C1_LimitSwitchPivotController.OnTargetPosArrive += OnTargetPosArrive;
    }

    private void OnTargetPosArrive()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 2 &&Managers.ContentInfo.PlayData.Count == 6)
        {
            Logger.Log("OnTargetPos Arrive -------------------6 -----------------미션수행 --------------");
            OnStepMissionComplete(animationNumber:6,delayTimeAmount:new WaitForSeconds(3f));
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 7)
        {
            OnStepMissionComplete(animationNumber:7);
        }
    }
    protected  virtual void UnBindInteractionEvent()
    {
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
     //   UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
       
        UI_ToolBox.ToolBox_LimitSwitchSensorClickedEvent -= OnToolBoxLimitSwitchBtnOnUIClicked;
       
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        C1_LimitSwitchPivotController.OnTargetPosArrive -= OnTargetPosArrive;
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceModeSet;
        MultimeterController.OnConductiveModeReady -= OnConductiveModeSet;
        
    }

    private void OnToolBoxLimitSwitchBtnOnUIClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 2)
        {
            OnStepMissionComplete(animationNumber: 2);
        }

        
            if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 9)
            {
                OnStepMissionComplete(animationNumber: 9);
            }

            if (Managers.ContentInfo.PlayData.Count == 12)
            {
                OnStepMissionComplete(animationNumber: 12);
            }
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnBindInteractionEvent();
    }

    /// <summary>
    /// 드라이버로 나사를 푸는 경우의 State에 사용됩니다.
    /// 자원 할당 이외 직접적인 사용은 최대한 Depth별 State에서 사용하도록 합니다. 
    /// 적절한 초기화 작업이 필요합니다(10/14/24)
    /// </summary>
    public void SetScrewDriverSection(bool isWind = false)
    {
        
        currentScrewGaugeStatus = new Dictionary<int, float>();
        
        
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.ConnectionScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.ConnectionScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.ConnectionScrewC, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.ConnectionScrewD, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.LeverScrew, 0);

        animatorMap.TryAdd((int)DepthC1_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).GetComponent<Animator>());

        animatorMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewA,
            GetObject((int)DepthC1_GameObj.ConnectionScrewA).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewB,
            GetObject((int)DepthC1_GameObj.ConnectionScrewB).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewC,
            GetObject((int)DepthC1_GameObj.ConnectionScrewC).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewD,
            GetObject((int)DepthC1_GameObj.ConnectionScrewD).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.LeverScrew,
            GetObject((int)DepthC1_GameObj.LeverScrew).GetComponent<Animator>());
        

        animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련

        GetObject((int)DepthC1_GameObj.ConnectionScrewA)
            .BindEvent(() =>
            {
               animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
               animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.ConnectionScrewA);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.ConnectionScrewA);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC1_GameObj.ConnectionScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.ConnectionScrewB);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.ConnectionScrewB);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC1_GameObj.ConnectionScrewC)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].enabled = false;
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.ConnectionScrewC);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.ConnectionScrewC);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC1_GameObj.ConnectionScrewD)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].enabled = false;
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.ConnectionScrewD);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.ConnectionScrewD);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_D, true);
            }, Define.UIEvent.Pressed);

        
        GetObject((int)DepthC1_GameObj.LeverScrew)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = false;
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.LeverScrew);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.LeverScrew);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_LEVER_SCREW, true);
            }, Define.UIEvent.Pressed);


        
        
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_A, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

           // animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC1_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_B, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

            //animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC1_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_C, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

         
         //   animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
        }, Define.UIEvent.PointerDown);

        
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_D, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

         
            //   animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_D, true);
        }, Define.UIEvent.PointerDown);
        
                
        GetObject((int)DepthC1_GameObj.LeverScrew).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_LEVER_SCREW, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

         
            //   animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_LEVER_SCREW, true);
        }, Define.UIEvent.PointerDown);

        
        #endregion


        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC1_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC1_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_D, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });

                
        GetObject((int)DepthC1_GameObj.LeverScrew).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_LEVER_SCREW, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });



        InitializeTool();



    }

    private void SetMultimeterSection()
    {
        multimeterController = GetObject((int)DepthC1_GameObj.Multimeter).GetComponent<MultimeterController>();
 

        animatorMap.TryAdd((int)DepthC1_GameObj.Multimeter,
            GetObject((int)DepthC1_GameObj.Multimeter).GetComponent<Animator>());

        animatorMap.TryAdd((int)DepthC1_GameObj.Probe_Anode,
            GetObject((int)DepthC1_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.Probe_Cathode,
            GetObject((int)DepthC1_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC1_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;
    }
    protected virtual void OnToolBoxClicked()
    {
      
    }
    
    private void InitScrewForConductiveCheck()
    {
        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
          
            
            if((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 15)||
               (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 12))
            {
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_A, true);
                
                ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewB, "측정단자 B");
                
                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA);
                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB, false);
                
                BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewB);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
           

        });
     
        GetObject((int)DepthC1_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            
                    
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 15)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 12))
            {
                
                if (!isAnodePut) return;

                Logger.Log("Probe Set == 16");
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_B, true);

                Action action = multimeterController.OnAllProbeSetOnConductiveCheckMode;
                         if (Managers.ContentInfo.PlayData.Count == 15)
                {
                    OnStepMissionComplete(animationNumber: 15, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                else if (Managers.ContentInfo.PlayData.Count == 11)
                {
                    OnStepMissionComplete(animationNumber: 12, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                
            }
            
        }, Define.UIEvent.PointerDown);


        
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 16)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_C, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() =>
                {
                                    
                    ChangeTooltipText((int)DepthC1_GameObj.ConnectionScrewD, "측정단자 D");
                    SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD, false);
                    BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewD);

                    isAnodePut = true;
                });
            }

        });
        
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 16)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, true);
                Action action = multimeterController.OnAllProbeSetOnConductiveCheckMode;


                if (Managers.ContentInfo.PlayData.Count == 12)
                {
                    OnStepMissionComplete(animationNumber: 12, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                else if (Managers.ContentInfo.PlayData.Count == 16)
                {
                    OnStepMissionComplete(animationNumber: 16, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                
               

             
                
                SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }

        });
    }

    private float _gaugeDelay = 1.05f;
    private float _pressedTime;
    private Sequence _driveroundSequence;// 사운드 중복실행을 방지하기위한 선언 

    private void OnScrewClickDown()
    {
        _driveroundSequence?.Kill();
        _driveroundSequence = DOTween.Sequence();
        
        
        _driveroundSequence.AppendCallback(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (CurrentActiveTool == (int)DepthC1_GameObj.ElectricScrewdriver) Managers.Sound.Play(SoundManager.Sound.Effect, "Object/ElectronicDriver", 0.4f);
            });
        });

        _driveroundSequence.OnKill(() => { Managers.Sound.Stop(SoundManager.Sound.Effect); });

        if (!CheckDriverUsability())
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        }
    }

    private void OnScrewClickUp()
    {
     
        
        _driveroundSequence?.Kill();
        Managers.Sound.Stop(SoundManager.Sound.Effect);
        
        contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        _pressedTime = 0;
    }

    public bool isWindSession = false;
    public bool isLeverScrewUnwound { get;  set; }
    protected void UpdateDriverSliderUnwind(int screwID)
    {

        
        if (!CheckDriverUsability())
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
            return;
        }
        if (isWindSession) return;

        

        _pressedTime += Time.fixedDeltaTime;

        //잔량 선제 표시
        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        if (_gaugeDelay > _pressedTime) return;


        if (objectHighlightMap[(int)DepthC1_GameObj.ConnectionScrewC].ignore) return;
        if (CurrentActiveTool != (int)DepthC1_GameObj.ElectricScrewdriver)
        {
            Logger.Log("inadequate tool selected. XXXXXX");
            return;
        }


        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        
        contentController.UI_DrverOnly_GaugeSlider.value += 0.007f;


        //Logger.Log($"Wind logic : 현재 풀기 Value -->{ contentController.UI_DrverOnly_GaugeSlider.value}");
        
        currentScrewGaugeStatus[screwID] = contentController.UI_DrverOnly_GaugeSlider.value;

        if (!isScrewUnwindMap[screwID])
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(true);
            var playbackAmount = contentController.UI_DrverOnly_GaugeSlider.value;
            animatorMap[screwID].SetBool(UNWIND,true);
            animatorMap[screwID].Play("UnScrew", 0, playbackAmount);
            animatorMap[screwID].enabled = true;
            
            //animatorMap[screwID].Update(playbackAmount);

            if (contentController.UI_DrverOnly_GaugeSlider.value > 0.8f)
            {
           
                
                animatorMap[screwID].Play("UnScrew", 0, 1);
                isScrewUnwindMap[screwID] = true;
                
                if (screwID == (int)DepthC1_GameObj.LeverScrew)
                {
                    OnLeverScrewUnWound();
                }
                animatorMap[screwID].enabled = false;
                unwoundCount++;
                TurnOffCollider(screwID);
            }
        }
        else
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        }
    }

    
    /// <summary>
    /// 1. 레버Screw를 풀어야만 동작이 가능합니다. 
    /// </summary>
    private void OnLeverScrewUnWound()
    {
        isLeverScrewUnwound = true;
        limitSwitchPivotController.SetLimitSwitchControllableOrClickable(true);
        
      
        Logger.Log("레버나사 풀림 ->컨트롤가능");
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5  )
        {
  
            OnStepMissionComplete(animationNumber:5);
        }
        
        
        SetHighlightIgnore((int)DepthC1_GameObj.LeverHandleReadjustTargetPos,false);
        BlinkHighlight((int)DepthC1_GameObj.LeverHandleReadjustTargetPos);
    }
    private bool  CheckDriverUsability()
    {
        if (((Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5) ||
              (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 10)||
              
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 5) ||
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 7) ||
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 8) ||
              
              (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 6)||
              (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 10)))
        {
            return true;
        }
        else
        {
            Logger.Log("Driver is not usable in this step ------------------");
            return false;
        }
    }
    
    protected void UpdateDriverSliderWind(int screwID)
    {
        if (!isWindSession) return;
        // Depth3가 1이고 Count가 11인 경우, 또는 Depth3가 3이고 Count가 6 또는 12인 경우에는 계속 진행
        if (!CheckDriverUsability())
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
            return;
        }

  
        
        Logger.Log($"Wind logic : 현재 조이기 이벤트 Value -->{ contentController.UI_DrverOnly_GaugeSlider.value}");
        
       

        _pressedTime += Time.fixedDeltaTime;

        //잔량 선제 표시
        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        if (_gaugeDelay > _pressedTime) return;


        if (objectHighlightMap[(int)DepthC1_GameObj.ConnectionScrewC].ignore) return;
        if (CurrentActiveTool != (int)DepthC1_GameObj.ElectricScrewdriver)
        {
            Logger.Log("inadequate tool selected. XXXXXX");
            return;
        }


        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        contentController.UI_DrverOnly_GaugeSlider.value += 0.007f;
            
     
        
        
        currentScrewGaugeStatus[screwID] = contentController.UI_DrverOnly_GaugeSlider.value;

        if (!isScrewWindMap[screwID])
        {

            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(true);
            var playbackAmount = contentController.UI_DrverOnly_GaugeSlider.value;
        
            animatorMap[screwID].SetBool(UNWIND,false);
            //animatorMap[screwID].Update(playbackAmount);
            
            animatorMap[screwID].enabled = true;
            animatorMap[screwID].Play("Screw", 0, playbackAmount);

            if (contentController.UI_DrverOnly_GaugeSlider.value > 0.95f)
            {
                animatorMap[screwID].Play("Screw", 0, 1);
                isScrewWindMap[screwID] = true;
                animatorMap[screwID].enabled = false;
                
                woundCount++;
                TurnOffCollider(screwID);
                
                _driveroundSequence?.Kill();
                Managers.Sound.Stop(SoundManager.Sound.Effect);
            }
        }
        else
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        }
    }

    
  



    public bool _isDriverOn;

    
    private bool _isMultimeterOn;
    public bool isMultimeterOn
    {
        get
        {
            return _isMultimeterOn;
        }
        set
        {
            
            _isMultimeterOn = value;
            animatorMap[(int)DepthC1_GameObj.Multimeter].SetBool(MULTIMETER_ON, isMultimeterOn);
            if (!_isMultimeterOn)
            {
                GetObject((int)DepthC1_GameObj.Probe_Cathode)?.SetActive(false);
                GetObject((int)DepthC1_GameObj.Probe_Anode)?.SetActive(false);
                multimeterController.SetMeasureGuideStatus(false);
            }

        
        }
    }


    public bool isDriverOn
    {
        get => _isDriverOn;

        set
        {
            _isDriverOn = value;

            if (_isDriverOn)
                CurrentActiveTool = (int)DepthC1_GameObj.ElectricScrewdriver;
            else
            {
                CurrentActiveTool = NO_TOOL_SELECTED;
                GetObject((int)DepthC1_GameObj.ElectricScrewdriver).SetActive(false);
            }
        }
    }


    protected virtual void Update()
    {
        SetToolPos();
    }


    [FormerlySerializedAs("_multimeterController")]
    public MultimeterController multimeterController;


    protected virtual void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
            Input.mousePosition.y + _toolPosYOffset,
            distanceFromCamera));


        if (isDriverOn && CurrentActiveTool == (int)DepthC1_GameObj.ElectricScrewdriver)
        {
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).transform.position = mousePosition;
        }

        else if (isMultimeterOn && CurrentActiveTool == (int)DepthC1_GameObj.Multimeter &&
                 multimeterController.isResistanceMode && multimeterController.isConductive)
        {
            GetObject((int)DepthC1_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC1_GameObj.Probe_Anode).SetActive(isMultimeterOn);

            if ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 15 &&
                 !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 16 &&
                    !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 12 &&
                    !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 13 &&
                    !isAnodePut)
                )
            {
                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC1_GameObj.Probe_Anode];

                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if
                ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 15 &&
                  isAnodePut)
                 || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 16 &&
                     isAnodePut)
                 || (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 12 &&
                     isAnodePut)
                 || (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 13 &&
                     isAnodePut)
                 )
            {
                GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.rotation =
                    defaultRotationMap[(int)DepthC1_GameObj.Probe_Cathode];

                GetObject((int)DepthC1_GameObj.Probe_Cathode).transform.position = mousePosition;
                
           
            }
        }
      





    }

    public void ToggleActiveState(GameObject obj, bool isOn)
    {
        obj.SetActive(isOn);
    }

    public virtual void OnElectricScrewdriverBtnClicked()
    {
        
        InitializeTool();
        
        
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 4)
        {
            OnStepMissionComplete(animationNumber:4);
        }

        
        CurrentActiveTool = (int)DepthC1_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) CurrentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC1_GameObj.ElectricScrewdriver), isDriverOn);
        Logger.Log($"Electronic Screw Driver btn Clicked -------is driver on? : {isDriverOn}");
        
    }
    
    

    protected virtual void OnUIToolBoxMultimeterBtnClicked()
    {
      
        InitializeTool();
        CurrentActiveTool = (int)DepthC1_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false) CurrentActiveTool = -1;
        
        Logger.Log($"is Multimeter on? : {isMultimeterOn}");

        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 12)
        {
            OnStepMissionComplete(animationNumber:12);
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 9)
        {
            OnStepMissionComplete(animationNumber:9);
        }
        

    }
    
    protected virtual void OnResistanceModeSet()
    {
      
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 13)
        {
            OnStepMissionComplete(animationNumber:13);
        }
        
            
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 10)
        {
            OnStepMissionComplete(animationNumber:10);
        }
    }
    
    protected virtual void OnConductiveModeSet()
    {
      
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 15)
        {
            OnStepMissionComplete(animationNumber:15);
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11)
        {
            OnStepMissionComplete(animationNumber:11);
        }
    }

    public void TakeDefaultMultimeter()
    {
        isMultimeterOn = true;
        multimeterController.SetMeasureGuideStatus();
        CurrentActiveTool = (int)DepthC1_GameObj.Multimeter;
        contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        multimeterController.SetToDefaultMode();
        
        BlinkHighlight((int)DepthC1_GameObj.MultimeterHandleHighlight);
    }

    public void InitProbe()
    {
        Logger.Log("Init Probe for state 17");
      //  InitProbePos();
        SetProbeToDefaultAnimStatus();
        isAnodePut = false;
        //multimeterController.OnGroundNothing();
    }
    


    public void SetProbeToDefaultAnimStatus()
    {
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
        
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_A, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(TO_SCREW_C, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_B, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, false);

        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;
        
        isAnodePut = false;
        
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].Play("A_ON", 0, 0);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].Play("B_ON", 0, 0);
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].Play("C_ON", 0, 0);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].Play("D_ON", 0, 0);
    }

    public void SetToResistantMode()
    {
        InitProbePos();
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = true;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = true;
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].Play("ON", 0, 0);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].Play("ON", 0, 0);
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].Update(0);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].Update(0);
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;
     
        isMultimeterOn = true;
        multimeterController.SetMeasureGuideStatus();
        CurrentActiveTool = (int)DepthC1_GameObj.Multimeter;
        
        contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        multimeterController.SetToResistanceModeAndRotation();
    
    }


    /// <summary>
    /// Screw관련 스텝이아닌경우 실행
    /// </summary>
    public void BlockScrewInteraction()
    {
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA, true);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB, true);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewC, true);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD, true);
        SetHighlightIgnore((int)DepthC1_GameObj.LeverScrew, true);
    }
    /// <summary>
    /// screw 초기화 로직입니다. forWindAnim =True일경우 조이는 애니메이션을 위해 나사가 풀려있는상태로 초기화됩니다. .
    /// 반대로 forWindAnim =false인경우 나사를 푸는 애니메이션을 위해 나사가 조여진상태로 초기화됩니다. 
    /// </summary>
    /// <param name="toWind"></param>
    public void InitScrews()
    {

        isLeverScrewUnwound = false;
        
        isScrewWindMap = new Dictionary<int, bool>();

        isScrewWindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewA, false);
        isScrewWindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewB, false);
        isScrewWindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewC, false);
        isScrewWindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewD, false);
        isScrewWindMap.TryAdd((int)DepthC1_GameObj.LeverScrew, false);


        isScrewUnwindMap = new Dictionary<int, bool>();

        isScrewUnwindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewA, false);
        isScrewUnwindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewB, false);
        isScrewUnwindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewC, false);
        isScrewUnwindMap.TryAdd((int)DepthC1_GameObj.ConnectionScrewD, false);
        isScrewUnwindMap.TryAdd((int)DepthC1_GameObj.LeverScrew, false);
        
        


                
    }
    protected void InitializeTool()
    {
        CurrentActiveTool = -1;
        ToggleActiveState(GetObject((int)DepthC1_GameObj.ElectricScrewdriver), false);
        animatorMap[(int)DepthC1_GameObj.Multimeter].SetBool(MULTIMETER_ON, false); // 멀티미터는 active상태로 유지합니다.
    }

    public void ClearTool()
    {
        CurrentActiveTool =  -1;
        isDriverOn= false;
        isMultimeterOn = false;
        multimeterController.isConductive = false;
    }

    public void SetScrewStatus(bool forWindAnim)
    {

        TurnOnCollidersAndInit();
        
        foreach (var key in  currentScrewGaugeStatus.Keys.ToList())
        {
            currentScrewGaugeStatus[key] = 0f;
        }
        
             
        foreach (var key in  isScrewUnwindMap.Keys.ToList())
        {
            isScrewUnwindMap[key] = false;
        }

        string StateName = forWindAnim ? "Screw" : "UnScrew";
        
        
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewA, false);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewB, false);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewC, false);
        SetHighlightIgnore((int)DepthC1_GameObj.ConnectionScrewD, false);
        SetHighlightIgnore((int)DepthC1_GameObj.LeverScrew, false);

         BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewA);
         BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewB);
         BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewC);
         BlinkHighlight((int)DepthC1_GameObj.ConnectionScrewD);
         
        
        //나사 위치 초기화
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].enabled = true;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].enabled = true;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].enabled = true;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].enabled = true;
        animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = true;
        
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].Play(StateName, 0, 0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].Play(StateName, 0, 0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].Play(StateName, 0, 0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].Play(StateName, 0, 0);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].Play(StateName, 0, 0);
        
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].Update(0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].Update(0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].Update(0);
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].Update(0);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].Update(0);
        
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].StopPlayback();
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].StopPlayback();
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].StopPlayback();
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].StopPlayback();
        animatorMap[(int)DepthC1_GameObj.LeverScrew].StopPlayback();
        
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewA].enabled = false;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewB].enabled = false;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewC].enabled = false;
        animatorMap[(int)DepthC1_GameObj.ConnectionScrewD].enabled = false;
        animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = false;
     
        contentController.isStepMissionPerformable = true;
        
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
    }

    private void SetLeverScrewStatus(bool forWindAnim)
    {
        
        TurnOnCollidersAndInit();
        
        foreach (var key in  currentScrewGaugeStatus.Keys.ToList())
        {
            currentScrewGaugeStatus[key] = 0f;
        }
        
             
        foreach (var key in  isScrewUnwindMap.Keys.ToList())
        {
            isScrewUnwindMap[key] = false;
        }

        string StateName = forWindAnim ? "Screw" : "UnScrew";
        

        SetHighlightIgnore((int)DepthC1_GameObj.LeverScrew, false);
         BlinkHighlight((int)DepthC1_GameObj.LeverScrew);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = true;
        animatorMap[(int)DepthC1_GameObj.LeverScrew].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].Play(StateName, 0, 0);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].Update(0);
        animatorMap[(int)DepthC1_GameObj.LeverScrew].StopPlayback();
        animatorMap[(int)DepthC1_GameObj.LeverScrew].enabled = false;
     
        contentController.isStepMissionPerformable = true;
        
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
    }
    /// <summary>
    ///     1. 개발전용입니다.
    ///     2. 씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     3. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;
    }

    
    /// <summary>
    /// 1. 공통적인 오브젝트 초기화로직 등 
    /// </summary>

    private void InitializeC1States()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 3110, new DepthC11_State_0(this) },
            { 3111, new DepthC11_State_1(this) },
            { 3112, new DepthC11_State_2(this) },
            { 3113, new DepthC11_State_3(this) },
            { 3114, new DepthC11_State_4(this) },
            { 3115, new DepthC11_State_5(this) },
            { 3116, new DepthC11_State_6(this) },
            { 3117, new DepthC11_State_7(this) },
            { 3118, new DepthC11_State_8(this) },
            { 3119, new DepthC11_State_9(this) },
            { 31110, new DepthC11_State_10(this) },
            { 31111, new DepthC11_State_11(this) },
            { 31112, new DepthC11_State_12(this) },
            { 31113, new DepthC11_State_13(this) },
            { 31114, new DepthC11_State_14(this) },
            { 31115, new DepthC11_State_15(this) },
            { 31116, new DepthC11_State_16(this) },
            { 31117, new DepthC11_State_17(this) },
            { 31118, new DepthC11_State_18(this) },
       
            { 3120, new DepthC12_State_1(this) },
            { 3121, new DepthC12_State_1(this) },
            { 3122, new DepthC12_State_2(this) },
            { 3123, new DepthC12_State_3(this) },
            { 3124, new DepthC12_State_4(this) },
            { 3125, new DepthC12_State_5(this) },
            { 3126, new DepthC12_State_6(this) },
            { 3127, new DepthC12_State_7(this) },
            { 3128, new DepthC12_State_8(this) },
            { 3129, new DepthC12_State_9(this) },
            { 31210, new DepthC12_State_10(this) },
            { 31211, new DepthC12_State_11(this) },
            { 31212, new DepthC12_State_12(this) },
            { 31213, new DepthC12_State_13(this) },
            { 31214, new DepthC12_State_14(this) },
    
            { 3130, new DepthC13_State_1(this) },
            { 3131, new DepthC13_State_1(this) },
            { 3132, new DepthC13_State_2(this) },
            { 3133, new DepthC13_State_3(this) },
            { 3134, new DepthC13_State_4(this) },
            { 3135, new DepthC13_State_5(this) },
            { 3136, new DepthC13_State_6(this) },
            { 3137, new DepthC13_State_7(this) },
            { 3138, new DepthC13_State_8(this) },
            { 3139, new DepthC13_State_9(this) },
            { 31310, new DepthC13_State_10(this) },
            { 31311, new DepthC13_State_11(this) },
            { 31312, new DepthC13_State_12(this) },
            { 31313, new DepthC13_State_13(this) },

        };
    }
}
    
