using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
public enum DepthC3_GameObj
{
    ElectricScrewdriver,
    Multimeter,
    MultimeterHandleHighlight,
    Probe_Anode, // negative
    Probe_Cathode, // positive,
    CathodeSensorInput,
        
    ConnectionScrewA,
    ConnectionScrewB,
    ConnectionScrewC,
    ConnectionScrewD,
    PowerHandle,
    PressureSensor,
    PressureSensorScale,
    PressureSensorToConditionerCable,
    PressureSensorHose,
    PressureSensorAdapter,
    PressureSensorAdapter_Sub,
    PressureSensorDamagedPart,
    PressureSensor_Display,
    PressureSensor_Display_Uni,
    NewPressureSensor,
    PressureSensorWaterPipeValve, // FluidInsidePipe
    
    
    ControlPanelFrontDoor,
    AnodeSensorOutput,
    PowerCable,
    PressureCalibrator,
    PressureSensorConnectingPipe, //연결 배관
    PressureSensorConnectingScrew, // 연결 나사 (어댑터)
    
    //하이라이트 및 툴팁 적용을 위한 enum (객체컨트롤은 PressureCalibrator에서합니다.)
    Btn_F1 , 
    Btn_F2,
    Btn_F3, //Tasks:LoopPower
    Btn_F4, //Tasks::Continue
    Btn_Tasks,
    Btn_Arrow_Down,
    Btn_Arrow_Up,
    Btn_Enter,
    Btn_Vent,
    Btn_Number_One,
    Btn_Number_Zero
    
}


public class DepthC3_SceneController : Base_SceneController
{

    #region 압력교정기

    [FormerlySerializedAs("_pressureCalibratorController")] public PressureCalibratorController pressureCalibratorController;

    #endregion
    
    
    
    
private readonly int UNWOUND_COUNT_GOAL = 1;
    private int _unwoundCount;
    
    public void InitTransform(DepthC3_GameObj obj, bool isAll =false)
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
            for (int i = 0; i < Enum.GetValues(typeof(DepthC3_GameObj)).Length; i++)
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
        
        for (int i = 0; i < Enum.GetValues(typeof(DepthC3_GameObj)).Length; i++)
        {
            if (GetObject(i) == null)
            {
                Logger.LogWarning($"{(DepthC3_GameObj)i}'s Transform is null.. Default Value Setting Failed");
                continue;
            }
            
            defaultPositionMap.TryAdd(i, GetObject(i).transform.position);
            defaultRotationMap.TryAdd(i, GetObject(i).transform.rotation);
        }    
        
    }
    
    
    public void SetAnimator(DepthC3_GameObj obj)
    {
        if (animatorMap == null) animatorMap = new Dictionary<int, Animator>();

        animatorMap.TryAdd((int)obj, GetObject((int)obj).GetComponent<Animator>());
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
                if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 8)
                {
                    Logger.Log($"모든나사 풀림 -> 멀티미터모드로");
                    ClearTool();
                    //↑↓ 순서바뀌지않도록주의
                    OnStepMissionComplete(animationNumber:8);
               
                    currentActiveTool = -1;
                    isDriverOn = false;
                    _unwoundCount = 0;
                }

                if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 5)
                {
                    Logger.Log($"모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    ClearTool();
                    //↑↓ 순서바뀌지않도록주의
                    OnStepMissionComplete(animationNumber:5);
            
                    _unwoundCount = 0;//초기화 
                }
                
            
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
    private void BindEventForPsCalibrator()
    {

        BindHighlight((int)DepthC3_GameObj.Btn_F1,"F1");
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_F2,"F2");
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_F3,"F3");
        GetObject((int)DepthC3_GameObj.Btn_F3).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 10)
            {
                pressureCalibratorController.OnBtn_F3Clicked();
                OnStepMissionComplete(animationNumber:10);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 16)
            {
                pressureCalibratorController.OnBtn_F3Clicked();
                ChangeTooltipText((int)DepthC3_GameObj.Btn_F4, "F4 : Continue");
               
            }
        });
        
        BindHighlight((int)DepthC3_GameObj.Btn_F4,"F4");
        GetObject((int)DepthC3_GameObj.Btn_F4).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 16)
            {
                pressureCalibratorController.OnBtn_F4Clicked();
                OnStepMissionComplete(animationNumber:16);
               
            }
            
                     
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 20)
            {
                pressureCalibratorController.OnBtn_F4Clicked();
                OnStepMissionComplete(animationNumber:20);
            }
            
            //AutoTest
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 21)
            {
               
                OnStepMissionComplete(animationNumber:21);
            }
        });
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Vent,"VENT");
        GetObject((int)DepthC3_GameObj.Btn_Vent).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 9)
            {
                pressureCalibratorController.OnVentClicked();
                OnStepMissionComplete(animationNumber:9);
            }
        });
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Tasks,"TASKS");
        GetObject((int)DepthC3_GameObj.Btn_Tasks).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 11)
            {
                pressureCalibratorController.OnTasksBtnClicked();
                OnStepMissionComplete(animationNumber:11);
            }
        });
        
        

        
        BindHighlight((int)DepthC3_GameObj.Btn_Arrow_Down,"DOWN");
        GetObject((int)DepthC3_GameObj.Btn_Arrow_Down).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 14)
            {
                pressureCalibratorController.OnDownBtnClicked();
                OnStepMissionComplete(animationNumber:14);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 18)
            {
                pressureCalibratorController.OnDownBtnClicked();
            }
        });
        
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Arrow_Up,"UP");
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Enter,"ENTER");
        GetObject((int)DepthC3_GameObj.Btn_Enter).BindEvent(() =>
        {
       
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 15)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                if(pressureCalibratorController.is100PsiSet)OnStepMissionComplete(animationNumber:15);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 18)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                OnStepMissionComplete(animationNumber:18);
            }
            
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 19)
            {
                pressureCalibratorController.OnEnterBtnClicked();
                OnStepMissionComplete(animationNumber:19);

            }
        });
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Number_One,"1");
        GetObject((int)DepthC3_GameObj.Btn_Number_One).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 12)
            {
                pressureCalibratorController.OnBtnNumberOneClicked();
                OnStepMissionComplete(animationNumber:12);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 15)
            {
                pressureCalibratorController.OnBtnNumberOneClicked();

            }
        });
        
        
        BindHighlight((int)DepthC3_GameObj.Btn_Number_Zero,"0");
        GetObject((int)DepthC3_GameObj.Btn_Number_Zero).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 15)
            {
                pressureCalibratorController.OnBtnNumberZeroClicked();

            }
        });
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
        var screwCount = 123;
        
        _screwColliders = new Collider[screwCount];
        _screwColliders[(int)DepthC3_GameObj.ConnectionScrewA] = GetObject((int)DepthC3_GameObj.ConnectionScrewA).GetComponent<Collider>();
        _screwColliders[(int)DepthC3_GameObj.ConnectionScrewB] = GetObject((int)DepthC3_GameObj.ConnectionScrewB).GetComponent<Collider>();
        _screwColliders[(int)DepthC3_GameObj.ConnectionScrewC] = GetObject((int)DepthC3_GameObj.ConnectionScrewC).GetComponent<Collider>();
        _screwColliders[(int)DepthC3_GameObj.ConnectionScrewD] = GetObject((int)DepthC3_GameObj.ConnectionScrewD).GetComponent<Collider>();
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
     //   if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") 
            SetDepthNum(); //개발용

        base.Init();
        BindObject(typeof(DepthC3_GameObj));
        
        
        
        
        SetDefaultTransform();

        InitializeC3States();
        GetScrewColliders();
        contentController.OnDepth2Init((int)Define.DepthC_Sensor.PressureSensor,1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
     
        
        pressureCalibratorController = GetObject((int)DepthC3_GameObj.PressureCalibrator).GetComponent<PressureCalibratorController>();
        Assert.IsNotNull(pressureCalibratorController);
        pressureCalibratorController.Init();


        controlPanel = GetObject((int)DepthC3_GameObj.PowerHandle).GetComponent<ControlPanelController>();
        
        C3_PreCommonObjInit();

    }
    private void LateCommonInit()
    {
        
        ClearTool();
        isAnodePut = false;
    }


    
    
    public void DepthC31Init()
    {
        PreCommonInit();

        SetDefaultTransform();
        BindInteractionEvent();
        InitProbePos();
        InitScrews();
       
        
        SetPressureSensorCurrentCheckMultimeterSection();
        SetScrewDriverSection();
        InitScrewForConductiveCheck();
        InitProbePos();
        
        
        
        
        BindHighlight((int)DepthC3_GameObj.CathodeSensorInput,"시그널 컨디셔너 입력단자");
        BindHighlight((int)DepthC3_GameObj.AnodeSensorOutput,"센서 출력 단자");
        BindHighlight((int)DepthC3_GameObj.PressureSensor,"압력 센서");
        BindHighlight((int)DepthC3_GameObj.PressureCalibrator,"압력 교정기");
        BindHighlight((int)DepthC3_GameObj.PressureSensorConnectingPipe,"배관 연결부");
        BindHighlight((int)DepthC3_GameObj.PressureSensorConnectingScrew,"연결부 고정 나사");
        BindHighlight((int)DepthC3_GameObj.MultimeterHandleHighlight,"전류모드로 설정");

        
        
        BindHighlight((int)DepthC3_GameObj.ConnectionScrewA,"나사");
        BindHighlight((int)DepthC3_GameObj.ConnectionScrewB,"나사");
        BindHighlight((int)DepthC3_GameObj.ConnectionScrewC,"나사");
        BindHighlight((int)DepthC3_GameObj.ConnectionScrewD,"나사");
        
                
        GetObject((int)DepthC3_GameObj.PressureSensorConnectingScrew).BindEvent(() =>
        {
            
            Logger.Log("잔유물제거---------------------");
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 5) 
            {
                OnStepMissionComplete(animationNumber:5);
            }  
        });
        
        
        GetObject((int)DepthC3_GameObj.PressureSensorConnectingPipe).BindEvent(() =>
        {
            
                Logger.Log("잔유물제거---------------------");
                if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 6) 
                {
                    OnStepMissionComplete(animationNumber:6);
                }  
        });

        
        LateCommonInit();
    }
    

    public void InitProbePos()
    {
        GetObject((int)DepthC3_GameObj.Probe_Anode).gameObject.SetActive(true);
        GetObject((int)DepthC3_GameObj.Probe_Cathode).gameObject.SetActive(true);
        
        GetObject((int)DepthC3_GameObj.Probe_Anode).transform.position = _probeDefaultPos;
        GetObject((int)DepthC3_GameObj.Probe_Cathode).transform.position = _probeDefaultPos;
        
        GetObject((int)DepthC3_GameObj.Probe_Anode).gameObject.SetActive(false);
        GetObject((int)DepthC3_GameObj.Probe_Cathode).gameObject.SetActive(false);
    }
    protected override void UnBindEventAttatchedObj()
    {
        base.UnBindEventAttatchedObj();
        UnBindInteractionEvent();
    }
    public void DepthC32Init()
    {
       
        PreCommonInit();

        SetDefaultTransform();
        
        BindInteractionEvent();
        
        BindHighlight((int)DepthC3_GameObj.PressureSensorDamagedPart,"변형(손상된) 감압부");
        
          
        BindHighlight((int)DepthC3_GameObj.PressureSensorAdapter,"압력센서 어댑터");
        GetObject((int)DepthC3_GameObj.PressureSensorAdapter).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 3) 
            {
                Logger.Log(" 어댑터교체---------------------");
                OnStepMissionComplete(animationNumber:3);
            }  
        });
        BindHighlight((int)DepthC3_GameObj.PressureSensorAdapter_Sub,"압력센서 어댑터");
        GetObject((int)DepthC3_GameObj.PressureSensorAdapter_Sub).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 4) 
            {
                Logger.Log(" 어댑터교체---------------------");
                OnStepMissionComplete(animationNumber:4);
            }  
        });
 
        InitProbePos();
        SetPressureSensorCurrentCheckMultimeterSection();
        SetScrewDriverSection();
        

        
        LateCommonInit();
    }



    protected override void PreCommonInit()
    {
        base.PreCommonInit();
        
        defaultRotationMap.TryAdd((int)DepthC3_GameObj.Probe_Cathode,GetObject((int)DepthC3_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC3_GameObj.Probe_Anode,GetObject((int)DepthC3_GameObj.Probe_Cathode).transform.rotation);
        C3_PreCommonObjInit();
        UnBindEventAttatchedObj();
    }
    private void C3_PreCommonObjInit()
    {
        GetObject((int)DepthC3_GameObj.PressureCalibrator).SetActive(false);
         GetObject((int)DepthC3_GameObj.PressureSensorHose).SetActive(false);
         GetObject((int)DepthC3_GameObj.PressureSensor_Display).SetActive(false);
         GetObject((int)DepthC3_GameObj.PressureSensor_Display_Uni).SetActive(false);
         GetObject((int)DepthC3_GameObj.PressureSensorAdapter_Sub).SetActive(false);

    }

    public void DepthC33Init()
    {
      
        PreCommonInit();
        
        SetPressureSensorCurrentCheckMultimeterSection();
        SetScrewDriverSection();
        InitScrews();
        
        InitScrewForConductiveCheck();
        InitProbePos();
        SetDefaultTransform();
        BindInteractionEvent();
        BindEventForPsCalibrator();
        
        BindHighlight((int)DepthC3_GameObj.ConnectionScrewB,"나사");
        
        
        BindHighlight((int)DepthC3_GameObj.PressureSensorWaterPipeValve,"밸브 열기");        
        GetObject((int)DepthC3_GameObj.PressureSensorWaterPipeValve).BindEvent(() =>
        {
            Logger.Log("잔유물제거---------------------");
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 4) 
            {
                OnStepMissionComplete(animationNumber:4);
            }  
        });
        
        
        BindHighlight((int)DepthC3_GameObj.PowerHandle,"전원 차단");
        
        GetObject((int)DepthC3_GameObj.PowerHandle).BindEvent(() =>
        {
            Logger.Log("잔유물제거---------------------");
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 3) 
            {
              
                OnStepMissionComplete(animationNumber:3);
            }  
        });
        
               
        BindHighlight((int)DepthC3_GameObj.PressureSensor,"기존 압력센서 회수");
        
        GetObject((int)DepthC3_GameObj.PressureSensor).BindEvent(() =>
        {
            Logger.Log("잔유물제거---------------------");
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 6) 
            {
              
                OnStepMissionComplete(animationNumber:6);
            }  
        });

        
    
        
        LateCommonInit();
        
    }
    

    
    protected virtual void OnPressureCalibratorClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 8) 
        {
            pressureCalibratorController.BootPressureCalibrator();
            OnStepMissionComplete(animationNumber:8);
        }
    }

    protected virtual void ToolBoxOnPressureSensorBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 2) 
        {
        
            OnStepMissionComplete(animationNumber:2);
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 7) 
        {
        
            OnStepMissionComplete(animationNumber:7);
        }
    }

    private void ToolBoxNewAdapterClicked()
    {
       
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 4) 
        { Logger.Log("갑압부 교체---------------------");
            OnStepMissionComplete(animationNumber:4);
        }  
    }

    protected virtual void PowerOnOff(bool isOn)
    {
        var currentMissionStepA = 4;
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == currentMissionStepA)
        {
            OnStepMissionComplete(animationNumber: currentMissionStepA);
        }
        
    
        var currentMissionStepB = 24;
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == currentMissionStepB)
        {
            OnStepMissionComplete(animationNumber: currentMissionStepB);
        }
    }

    protected void BindInteractionEvent()
    {
        
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        ControlPanelController.PowerOnOffActionWithBool += PowerOnOff;
        

        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
        
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
        UI_ToolBox.ToolBox_PressureCalibratorClickedEvent -= OnPressureCalibratorClicked;
        UI_ToolBox.ToolBox_PressureCalibratorClickedEvent += OnPressureCalibratorClicked;
        
        UI_ToolBox.ToolBox_PressureSensorClicked -= ToolBoxOnPressureSensorBtnClicked;
        UI_ToolBox.ToolBox_PressureSensorClicked += ToolBoxOnPressureSensorBtnClicked;

        
        UI_ToolBox.ToolBox_PS_NewAdaptorCliked -= ToolBoxNewAdapterClicked;
        UI_ToolBox.ToolBox_PS_NewAdaptorCliked += ToolBoxNewAdapterClicked;
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;
        
        
        CurrentCheckableMultimeterController.OnCurrentModeReady -= OnCurrentModeSet;
        CurrentCheckableMultimeterController.OnCurrentModeReady += OnCurrentModeSet;
        
        C1_LimitSwitchPivotController.OnTargetPosArrive -= OnTargetPosArrive;
        C1_LimitSwitchPivotController.OnTargetPosArrive += OnTargetPosArrive;
    }

    private void OnTargetPosArrive()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 2 &&Managers.ContentInfo.PlayData.Count == 8)
        {
            OnStepMissionComplete(animationNumber:8);
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 7)
        {
            OnStepMissionComplete(animationNumber:7);
        }
    }
    protected  virtual void UnBindInteractionEvent()
    {
        
        UI_ToolBox.ToolBox_PS_NewAdaptorCliked -= ToolBoxNewAdapterClicked;
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        UI_ToolBox.ToolBox_PressureSensorClicked -= ToolBoxOnPressureSensorBtnClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        
        UI_ToolBox.ToolBox_PressureCalibratorClickedEvent -= OnPressureCalibratorClicked;
        UI_ToolBox.ToolBox_LimitSwitchSensorClickedEvent -= OnToolBoxLimitSwitchBtnClicked;
       
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        C1_LimitSwitchPivotController.OnTargetPosArrive -= OnTargetPosArrive;
       
        CurrentCheckableMultimeterController.OnCurrentModeReady -= OnCurrentModeSet;

        
    }

    private void OnToolBoxLimitSwitchBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Count == 2)
        {
            OnStepMissionComplete(animationNumber:2);
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
        
        
        currentScrewGaugeStatus.TryAdd((int)DepthC3_GameObj.ConnectionScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC3_GameObj.ConnectionScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC3_GameObj.ConnectionScrewC, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC3_GameObj.ConnectionScrewD, 0);
       // currentScrewGaugeStatus.TryAdd((int)DepthC3_GameObj.LeverScrew, 0);

        animatorMap.TryAdd((int)DepthC3_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC3_GameObj.ElectricScrewdriver).GetComponent<Animator>());

    
        animatorMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewB,
            GetObject((int)DepthC3_GameObj.ConnectionScrewB).GetComponent<Animator>());

        // animatorMap.TryAdd((int)DepthC3_GameObj.LeverScrew,
        //     GetObject((int)DepthC3_GameObj.LeverScrew).GetComponent<Animator>());
        

        animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련
        

        GetObject((int)DepthC3_GameObj.ConnectionScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].enabled = true;
                animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = false;
              
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC3_GameObj.ConnectionScrewB);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC3_GameObj.ConnectionScrewB);
                }
                
                animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);
        
        
        GetObject((int)DepthC3_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].Play(TO_SCREW_A, 0, 0);
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].Update(0);

           // animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC3_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].Play(TO_SCREW_B, 0, 0);
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].Update(0);

            //animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);


        
        #endregion

        
        GetObject((int)DepthC3_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = false;
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, false);
            animatorMap[(int)DepthC3_GameObj.ElectricScrewdriver].enabled = false;
        });



        InitializeTool();



    }

    
    
    /// <summary>
    /// 1.전류 측정모드 분리를 위한 압력센서 전용 멀티미터 클래스 초기화 로직입니다
    /// 2. 다른센서 사용 XX
    /// </summary>
    private void SetPressureSensorCurrentCheckMultimeterSection()
    {
        multimeterController = GetObject((int)DepthC3_GameObj.Multimeter).GetComponent<CurrentCheckableMultimeterController>();
 

        animatorMap.TryAdd((int)DepthC3_GameObj.Multimeter,
            GetObject((int)DepthC3_GameObj.Multimeter).GetComponent<Animator>());

        animatorMap.TryAdd((int)DepthC3_GameObj.Probe_Anode,
            GetObject((int)DepthC3_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC3_GameObj.Probe_Cathode,
            GetObject((int)DepthC3_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC3_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = false;
    }
    protected virtual void OnToolBoxClicked()
    {
        if((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 7))
        {
            OnStepMissionComplete(animationNumber: 7);
        }
    }
    
    private void InitScrewForConductiveCheck()
    {
        GetObject((int)DepthC3_GameObj.AnodeSensorOutput).BindEvent(() =>
        {
          
            
            if((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 11))
            {
                animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(MULTIMETER_ON, true);
                
              //  ChangeTooltipText((int)DepthC3_GameObj.CathodeSensorInput, "측정단자 B");
                
                SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewA);
                SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB, false);
                
                SetHighlightIgnore((int)DepthC3_GameObj.AnodeSensorOutput);
                BlinkHighlight((int)DepthC3_GameObj.CathodeSensorInput);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
           

        });
     
        GetObject((int)DepthC3_GameObj.CathodeSensorInput).BindEvent(() =>
        {
            
                    
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 11)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 12))
            {
                
                if (!isAnodePut) return;

                Logger.Log("Probe Set == 12");
                animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(MULTIMETER_ON, true);
                
                Action action11 = multimeterController.PS_OnAllProbeSetOnCurrentCheckMode;
                if (Managers.ContentInfo.PlayData.Count == 11)
                {
                    OnStepMissionComplete(animationNumber: 11, delayTimeAmount: new WaitForSeconds(6f),ActionBeforeDelay:action11);
                }

                Action action = multimeterController.PS_OnAllProbeSetOnCurrentCheckMode;
                         if (Managers.ContentInfo.PlayData.Count == 12)
                {
                    OnStepMissionComplete(animationNumber: 12, delayTimeAmount: new WaitForSeconds(6f),ActionBeforeDelay:action);
                }
   
                
            }
            
        }, Define.UIEvent.PointerDown);


        
        
        GetObject((int)DepthC3_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 17)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(TO_SCREW_C, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() =>
                {
                                    
                    ChangeTooltipText((int)DepthC3_GameObj.ConnectionScrewD, "측정단자 D");
                    SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewD, false);
                    BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewD);

                    isAnodePut = true;
                });
            }

        });
        
        
        GetObject((int)DepthC3_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 17)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, true);
                Action action = multimeterController.OnAllProbeSetOnConductiveCheckMode;


                if (Managers.ContentInfo.PlayData.Count == 12)
                {
                    OnStepMissionComplete(animationNumber: 12, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                else if (Managers.ContentInfo.PlayData.Count == 17)
                {
                    OnStepMissionComplete(animationNumber: 17, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                
               

             
                
                SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewD);

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
                if (currentActiveTool == (int)DepthC3_GameObj.ElectricScrewdriver) Managers.Sound.Play(SoundManager.Sound.Effect, "Object/ElectronicDriver", 0.4f);
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
            Logger.Log($"driver is not usable in this step {currentCount}");
            return;
        }
        if (isWindSession) return;

        

        _pressedTime += Time.fixedDeltaTime;

        //잔량 선제 표시
        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        if (_gaugeDelay > _pressedTime) return;


        if (objectHighlightMap[(int)DepthC3_GameObj.ConnectionScrewB].ignore)
        {
            Logger.Log($"this screw's highlight is ignored.. driver logic can be executed. return");
            return;
        }
        
        if (currentActiveTool != (int)DepthC3_GameObj.ElectricScrewdriver)
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
                
  
                animatorMap[screwID].enabled = false;
                unwoundCount++;
                TurnOffCollider(screwID);
            }
        }
        else
        {
            Logger.Log($"It's already unwound. Slider off.");
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        }
    }


    private bool  CheckDriverUsability()
    {
        if (((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 8) ||
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 5)))
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


        if (objectHighlightMap[(int)DepthC3_GameObj.ConnectionScrewC].ignore) return;
        if (currentActiveTool != (int)DepthC3_GameObj.ElectricScrewdriver)
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
            
            if (animatorMap ==null) return;
                
            animatorMap[(int)DepthC3_GameObj.Multimeter].SetBool(MULTIMETER_ON, isMultimeterOn);
            if (!_isMultimeterOn)
            {
                GetObject((int)DepthC3_GameObj.Probe_Cathode)?.SetActive(false);
                GetObject((int)DepthC3_GameObj.Probe_Anode)?.SetActive(false);
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
                currentActiveTool = (int)DepthC3_GameObj.ElectricScrewdriver;
            else
            {
                currentActiveTool = NO_TOOL_SELECTED;
                GetObject((int)DepthC3_GameObj.ElectricScrewdriver).SetActive(false);
            }
        }
    }


    protected virtual void Update()
    {
        SetToolPos();
    }
    
    [FormerlySerializedAs("_multimeterController")]
    public CurrentCheckableMultimeterController multimeterController;


    protected virtual void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
            Input.mousePosition.y + _toolPosYOffset,
            distanceFromCamera));


        if (isDriverOn && currentActiveTool == (int)DepthC3_GameObj.ElectricScrewdriver)
        {
            GetObject((int)DepthC3_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC3_GameObj.ElectricScrewdriver).transform.position = mousePosition;
        }

        else if (isMultimeterOn && currentActiveTool == (int)DepthC3_GameObj.Multimeter &&
                 multimeterController.isCurrentCheckMode )
        {
            GetObject((int)DepthC3_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC3_GameObj.Probe_Anode).SetActive(isMultimeterOn);

            if ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                 !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                    !isAnodePut)

                )
            {
                GetObject((int)DepthC3_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC3_GameObj.Probe_Anode];

                GetObject((int)DepthC3_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if
                ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                  isAnodePut)
                 || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                     isAnodePut)
                 )
            {
                GetObject((int)DepthC3_GameObj.Probe_Cathode).transform.rotation =
                    defaultRotationMap[(int)DepthC3_GameObj.Probe_Cathode];

                GetObject((int)DepthC3_GameObj.Probe_Cathode).transform.position = mousePosition;
                
           
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
        
        
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5)
        {
            OnStepMissionComplete(animationNumber:5);
        }

        
        currentActiveTool = (int)DepthC3_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) currentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC3_GameObj.ElectricScrewdriver), isDriverOn);
        Logger.Log($"Electronic Screw Driver btn Clicked -------is driver on? : {isDriverOn}");
        
    }
    
    

    protected virtual void OnUIToolBoxMultimeterBtnClicked()
    {
      
        InitializeTool();
        currentActiveTool = (int)DepthC3_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

     
        
        Logger.Log($"is Multimeter on? : {isMultimeterOn}");

        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 9)
        {
            OnStepMissionComplete(animationNumber:9);
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 9)
        {
            OnStepMissionComplete(animationNumber:9);
        }
        
        if (isMultimeterOn == false) currentActiveTool = -1;
    }
    
    protected virtual void OnCurrentModeSet()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 10)
        {
            OnStepMissionComplete(animationNumber:10);
        }
    }
    

    public void SetProbeToDefaultAnimStatus()
    {
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = true;
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = true;
        
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(TO_SCREW_A, false);
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(TO_SCREW_C, false);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(TO_SCREW_B, false);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, false);

        animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = false;
        
        isAnodePut = false;
        
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].Play("A_ON", 0, 0);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].Play("B_ON", 0, 0);
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].Play("C_ON", 0, 0);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].Play("D_ON", 0, 0);
    }


    /// <summary>
    /// Screw관련 스텝이아닌경우 실행
    /// </summary>
    public void BlockScrewInteraction()
    {
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewA, true);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB, true);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewC, true);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewD, true);
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

        isScrewWindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewA, false);
        isScrewWindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewB, false);
        isScrewWindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewC, false);
        isScrewWindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewD, false);


        isScrewUnwindMap = new Dictionary<int, bool>();

        isScrewUnwindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewA, false);
        isScrewUnwindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewB, false);
        isScrewUnwindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewC, false);
        isScrewUnwindMap.TryAdd((int)DepthC3_GameObj.ConnectionScrewD, false);
        
        


                
    }
    protected void InitializeTool()
    {
        currentActiveTool = -1;
        ToggleActiveState(GetObject((int)DepthC3_GameObj.ElectricScrewdriver), false);
        animatorMap[(int)DepthC3_GameObj.Multimeter].SetBool(MULTIMETER_ON, false); // 멀티미터는 active상태로 유지합니다.
    }

    public override void ClearTool()
    {
        currentActiveTool =  -1;
        isDriverOn= false;
        isMultimeterOn = false;
        if(multimeterController!=null)multimeterController.isCurrentCheckMode = false;
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
        
        
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewA, false);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewB, false);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewC, false);
        SetHighlightIgnore((int)DepthC3_GameObj.ConnectionScrewD, false);
      

         BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewA);
         BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewB);
         BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewC);
         BlinkHighlight((int)DepthC3_GameObj.ConnectionScrewD);
         
        
        //나사 위치 초기화
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].enabled = true;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = true;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].enabled = true;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].enabled = true;
        
        
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
    
        
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].Play(StateName, 0, 0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Play(StateName, 0, 0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].Play(StateName, 0, 0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].Play(StateName, 0, 0);
  
        
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].Update(0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].Update(0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].Update(0);
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].Update(0);
 
        
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].StopPlayback();
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].StopPlayback();
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].StopPlayback();
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].StopPlayback();
       
        
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewA].enabled = false;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewB].enabled = false;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewC].enabled = false;
        animatorMap[(int)DepthC3_GameObj.ConnectionScrewD].enabled = false;
       
     
        contentController.isStepMissionPerformable = true;
        
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
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
        
        
     
        contentController.isStepMissionPerformable = true;
        
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC3_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
    }
    /// <summary>
    ///     1. 개발전용입니다. 
    ///     2. 씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     3. 메인화면부터 각 씬별로 테스트를 할 수 있도록 하기 위한 메소드입니다.
    /// </summary>
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 3;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;
    }


    private void InitializeC3States()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 3311, new DepthC31_State_1(this) },
            { 3312, new DepthC31_State_2(this) },
            { 3313, new DepthC31_State_3(this) },
            { 3314, new DepthC31_State_4(this) },
            { 3315, new DepthC31_State_5(this) },
            { 3316, new DepthC31_State_6(this) },
            { 3317, new DepthC31_State_7(this) },
            { 3318, new DepthC31_State_8(this) },
            { 3319, new DepthC31_State_9(this) },
            { 33110, new DepthC31_State_10(this) },
            { 33111, new DepthC31_State_11(this) },
            { 33112, new DepthC31_State_12(this) },
            { 33113, new DepthC31_State_13(this) },
            { 33114, new DepthC31_State_14(this) },
            { 33115, new DepthC31_State_15(this) },
            { 33116, new DepthC31_State_16(this) },
            { 33117, new DepthC31_State_17(this) },
            // { 32118, new DepthC21_State_18(this) },
            // { 32119, new DepthC21_State_19(this) },
            
            
            { 3321, new DepthC32_State_1(this) },
            { 3322, new DepthC32_State_2(this) },
            { 3323, new DepthC32_State_3(this) },
            { 3324, new DepthC32_State_4(this) },
            { 3325, new DepthC32_State_5(this) },
            { 3326, new DepthC32_State_6(this) },
            { 3327, new DepthC32_State_7(this) },
            { 3328, new DepthC32_State_8(this) },
            { 3329, new DepthC32_State_9(this) },
            { 33210, new DepthC32_State_10(this) },
            // { 32214, new DepthC32_State_14(this) },
            // { 32215, new DepthC32_State_15(this) },
            // { 32216, new DepthC32_State_16(this) },
            // { 32217, new DepthC32_State_17(this) },
            // { 32218, new DepthC32_State_18(this) },
            // { 32219, new DepthC32_State_19(this) },
            
            
            { 3331, new DepthC33_State_1(this) },
            { 3332, new DepthC33_State_2(this) },
            { 3333, new DepthC33_State_3(this) },
            { 3334, new DepthC33_State_4(this) },
            { 3335, new DepthC33_State_5(this) },
            { 3336, new DepthC33_State_6(this) },
            { 3337, new DepthC33_State_7(this) },
            { 3338, new DepthC33_State_8(this) },
            { 3339, new DepthC33_State_9(this) },
            { 33310, new DepthC33_State_10(this) },
            { 33311, new DepthC33_State_11(this) },
            { 33312, new DepthC33_State_12(this) },
            { 33313, new DepthC33_State_13(this) },
            { 33314, new DepthC33_State_14(this) },
            { 33315, new DepthC33_State_15(this) },
            { 33316, new DepthC33_State_16(this) },
            { 33317, new DepthC33_State_17(this) },
            { 33318, new DepthC33_State_18(this) },
            { 33319, new DepthC33_State_19(this) },
            { 33320, new DepthC33_State_20(this) },
            { 33321, new DepthC33_State_21(this) },
            { 33322, new DepthC33_State_22(this) },
            { 33323, new DepthC33_State_23(this) },
            { 33324, new DepthC33_State_24(this) },
            { 33325, new DepthC33_State_25(this) },
       
        };
    }
}
    
