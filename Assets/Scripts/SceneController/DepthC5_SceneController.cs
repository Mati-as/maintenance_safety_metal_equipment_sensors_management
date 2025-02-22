using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
public enum DepthC5_GameObj
{
    LevelSensor,
    NewLevelSensor,
    LookAtPoint_LevelSensor,
    ElectricScrewdriver,
    Multimeter,
    MultimeterHandleHighlight,
    Probe_Anode, // negative
    Probe_Cathode, // positive,
    CathodeSensorInput,
    Wrench,
    LevelSensor_PipeValve,
    ConnectionScrewA,
    ConnectionScrewB,
    ConnectionScrewC,
    ConnectionScrewD,
    PowerHandle,
        
    AnodeSensorOutput,
  
    LevelSensorConnectingPipe, //연결 배관
    LevelSensorConnectingScrew, // 연결 나사 (어댑터)
    ContaminatedRod,
    PanelDoorHandle,
    BlockingPipePart,
    LevelSensor_TankWaterFluidEffect,
    LevelSensor_ResidueTankWaterFluidEffect,
    
    
    //하이라이트 및 툴팁 적용을 위한 enum (객체컨트롤은 PressureCalibrator에서합니다.)
    ModeOrEnterBtn,
    SetBtn,
}


public class DepthC5_SceneController : Base_SceneController
{

   
    #region 레벨센서 디스플레이

   
    public LevelSensorDisplayController levelSensorDisplayController;


    #endregion


    
    private readonly int UNWOUND_COUNT_GOAL = 1;
    private int _unwoundCount;
    
    public void InitTransform(DepthC5_GameObj obj, bool isAll =false)
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
            for (int i = 0; i < Enum.GetValues(typeof(DepthC5_GameObj)).Length; i++)
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
        
        for (int i = 0; i < Enum.GetValues(typeof(DepthC5_GameObj)).Length; i++)
        {
            if (GetObject(i) == null)
            {
                Logger.LogWarning($"{(DepthC5_GameObj)i}'s Transform is null.. Default Value Setting Failed");
                continue;
            }
            
            defaultPositionMap.TryAdd(i, GetObject(i).transform.position);
            defaultRotationMap.TryAdd(i, GetObject(i).transform.rotation);
        }    
        
    }
    
    
    public void SetAnimator(DepthC5_GameObj obj)
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
               
                    CurrentActiveTool = -1;
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

                if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 9)
                {
                    Logger.Log($"훈련모드 나사 조임 (12) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _woundCount}");
                    OnStepMissionComplete(animationNumber: 9);
                    _woundCount = 0; //초기화 
                }
                
                if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 25)
                {
                    Logger.Log($"훈련모드 나사 조임 (12) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _woundCount}");
                    OnStepMissionComplete(animationNumber: 25);
                    _woundCount = 0; //초기화 
                }
            }
        }
    }
    private void BindEventForLevelSensorDisplay()
    {
   
        BindHighlight((int)DepthC5_GameObj.ModeOrEnterBtn,
            Managers.Data.Preference[(int)Define.Preferences.EngMode]== 0 ?"모드/엔터 버튼" : "Mode/Enter Button");
       
        
        
#region  Mode/Enter 버튼을 통한 컨트롤----------------------------

        GetObject((int)DepthC5_GameObj.ModeOrEnterBtn).BindEvent(() =>
        {
            if (!levelSensorDisplayController.isClickable)
            {
                Logger.Log("levelSensor is not clickable...return ");
                return;
            }
        
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 9)
            {
                levelSensorDisplayController.OnModeEnterBtnClicked();
                levelSensorDisplayController.isClickable = false;
                OnStepMissionComplete(animationNumber:9);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 12)
            {
                levelSensorDisplayController.OnModeEnterBtnClicked();
                
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.rP1)
                {
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:12);
                }
            }
            
                    
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 15)
            {
                levelSensorDisplayController.OnModeEnterBtnClicked();
                
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.SP1)
                {
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:15);
                }
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 18)
            {
                levelSensorDisplayController.OnModeEnterBtnClicked();
                
             
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:18);
                
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 19)
            {
                levelSensorDisplayController.OnModeEnterBtnClicked();
                
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.EF)
                {
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:19);
                }
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 21)
            {
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.rES)
                {
                    levelSensorDisplayController.OnModeEnterBtnClicked();
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:21);
                }
            }

            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 21)
            {
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.rES)
                {
                    levelSensorDisplayController.OnModeEnterBtnClicked();
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:21);
                }
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 22)
            {
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.OutputSelection)
                {
                    levelSensorDisplayController.OnModeEnterBtnClicked();
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:22);
                }
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 24)
            {
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.OutputSelection)
                {
            
                    OnStepMissionComplete(animationNumber:24);
                }
            }
            
        });
#endregion END----Mode Enter Btn------------------------


        #region  setting 버튼을 통한 컨트롤----------------------------

        BindHighlight((int)DepthC5_GameObj.SetBtn,
            Managers.Data.Preference[(int)Define.Preferences.EngMode]== 0 ?"셋 버튼" : "Set Button");
        
        GetObject((int)DepthC5_GameObj.SetBtn).BindEvent(() =>
        {
            if (!levelSensorDisplayController.isClickable)
            {
                Logger.Log("levelSensor is not clickable...return ");
                return;
            }

            
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 10)
            {
                levelSensorDisplayController.OnSetBtnClicked();
                levelSensorDisplayController.isClickable = false;
                OnStepMissionComplete(animationNumber:10);
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 13)
            {
                levelSensorDisplayController.OnSetBtnClicked();
                levelSensorDisplayController.isClickable = false;
               
                if (levelSensorDisplayController.currentMode == LevelSensorDisplayController.Mode.Default_ValueCheck)
                {
                    OnStepMissionComplete(animationNumber:13);
                }
              
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 16)
            {
                levelSensorDisplayController.OnSetBtnClicked();
                levelSensorDisplayController.isClickable = false;
               
        
                OnStepMissionComplete(animationNumber:16);
                
              
            }

            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 17)
            {
                levelSensorDisplayController.OnSetBtnClicked();

              
                if (levelSensorDisplayController.isValueSettingMode)
                {
                    levelSensorDisplayController.SetSp1ValueByClick();
                    
                    if (levelSensorDisplayController.isValueProperlySetToTwelve)
                    {
                        OnStepMissionComplete(animationNumber:17);
                    }
                }
            }
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 20)
            {
             
                    levelSensorDisplayController.OnSetBtnClicked();
                    levelSensorDisplayController.isClickable = false;
                    OnStepMissionComplete(animationNumber:20);
                
            }

            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 23)
            {
             
                levelSensorDisplayController.OnSetBtnClicked();
                levelSensorDisplayController.isClickable = false;
                OnStepMissionComplete(animationNumber:23);
                
            }
            
        });


        GetObject((int)DepthC5_GameObj.SetBtn).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 17)
                levelSensorDisplayController.isSetBtnClickingForValueSettingMode = true;
        }, Define.UIEvent.Pressed);
        GetObject((int)DepthC5_GameObj.SetBtn).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 17)
                levelSensorDisplayController.isSetBtnClickingForValueSettingMode = true;
        }, Define.UIEvent.Click);
        GetObject((int)DepthC5_GameObj.SetBtn).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 17)
                levelSensorDisplayController.isSetBtnClickingForValueSettingMode = false;
        });
    }
    
    #endregion setting 버튼을 통한 컨트롤----------------------------

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
    public static readonly int TO_SCREW_B_WIND = Animator.StringToHash("ToScrewBWind");
    public static readonly int TO_SCREW_C = Animator.StringToHash("ToScrewC");
    public static readonly int TO_SCREW_D = Animator.StringToHash("ToScrewD");
    public static readonly int TO_LEVER_SCREW = Animator.StringToHash("ToLeverScrew");
    public static readonly int MULTIMETER_ON = Animator.StringToHash("On");



    public static readonly int TO_GROUNDING_TERMINAL = Animator.StringToHash("GroundingTerminal");

    public ControlPanelController controlPanel;

    private Collider[] _screwColliders;


    public void OnSetBtnAvailable()
    {
        SetHighlightIgnore((int)DepthC5_GameObj.SetBtn,false);
        BlinkHighlight((int)DepthC5_GameObj.SetBtn);
        SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        levelSensorDisplayController.SetClickable(setBtn:true);
    }
    
    public void OnModeBtnAvailable()
    {
        SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn,false);
        BlinkHighlight((int)DepthC5_GameObj.ModeOrEnterBtn);
        SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        levelSensorDisplayController.SetClickable(modeBtn:true);
    }
    
    public void OnModeOff()
    {
        SetHighlightIgnore((int)DepthC5_GameObj.ModeOrEnterBtn);
        SetHighlightIgnore((int)DepthC5_GameObj.SetBtn);
        levelSensorDisplayController.SetClickable(false,false);
        levelSensorDisplayController.isValueChangeModeUsable = false;
        levelSensorDisplayController.clickCountForChangingValue = 0;
    }

    /// <summary>
    /// 나사의 클릭기능을 원활하게 하기 위한 콜라이더 설정 로직입니다.
    /// </summary>
    public void GetScrewColliders()
    {
        var screwCount = 123;
        
        _screwColliders = new Collider[screwCount];
        _screwColliders[(int)DepthC5_GameObj.ConnectionScrewA] = GetObject((int)DepthC5_GameObj.ConnectionScrewA).GetComponent<Collider>();
        _screwColliders[(int)DepthC5_GameObj.ConnectionScrewB] = GetObject((int)DepthC5_GameObj.ConnectionScrewB).GetComponent<Collider>();
        _screwColliders[(int)DepthC5_GameObj.ConnectionScrewC] = GetObject((int)DepthC5_GameObj.ConnectionScrewC).GetComponent<Collider>();
        _screwColliders[(int)DepthC5_GameObj.ConnectionScrewD] = GetObject((int)DepthC5_GameObj.ConnectionScrewD).GetComponent<Collider>();
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
       // if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") 
            SetDepthNum(); //개발용

        base.Init();
        BindObject(typeof(DepthC5_GameObj));
        
        SetDefaultTransform();
        InitializeC5States();
        GetScrewColliders();
        contentController.OnDepth2Init((int)Define.DepthC_Sensor.LevelSensor,1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
        controlPanel = GetObject((int)DepthC5_GameObj.PowerHandle).GetComponent<ControlPanelController>();
        levelSensorDisplayController =
            GetObject((int)DepthC5_GameObj.LevelSensor).GetComponent<LevelSensorDisplayController>();
        Assert.IsNotNull(levelSensorDisplayController);
        
        
        
        C5_PreCommonObjInit();
        

    }
    private void LateCommonInit()
    {
        
        ClearTool();
        isAnodePut = false;
    }


    
    
    public void DepthC51Init()
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
        
                
        BindHighlight((int)DepthC5_GameObj.LevelSensorConnectingScrew,"연결부 고정 나사");
        GetObject((int)DepthC5_GameObj.LevelSensorConnectingScrew).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 5) 
            {
                Logger.Log("고정나사 확인---------------------");
                OnStepMissionComplete(animationNumber:5);
            }  
        });
        
                        


        
        BindHighlight((int)DepthC5_GameObj.ContaminatedRod,"측정부 오염 확인 및 청소");
        GetObject((int)DepthC5_GameObj.ContaminatedRod).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 6) 
            {
                Logger.Log("측정부 오염 확인 및 청소---------------------");
                OnStepMissionComplete(animationNumber:6);
            }  
        });

        
        
        
        BindHighlight((int)DepthC5_GameObj.CathodeSensorInput,"시그널 컨디셔너 입력단자");
        BindHighlight((int)DepthC5_GameObj.AnodeSensorOutput,"센서 출력 단자");
        
        BindHighlight((int)DepthC5_GameObj.MultimeterHandleHighlight,"전류모드로 설정");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewA,"나사");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewB,"접속단자");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewC,"나사");
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewD,"나사");
        
                

        LateCommonInit();
    }
    

    public void InitProbePos()
    {
        GetObject((int)DepthC5_GameObj.Probe_Anode).gameObject.SetActive(true);
        GetObject((int)DepthC5_GameObj.Probe_Cathode).gameObject.SetActive(true);
        
        GetObject((int)DepthC5_GameObj.Probe_Anode).transform.position = _probeDefaultPos;
        GetObject((int)DepthC5_GameObj.Probe_Cathode).transform.position = _probeDefaultPos;
        
        GetObject((int)DepthC5_GameObj.Probe_Anode).gameObject.SetActive(false);
        GetObject((int)DepthC5_GameObj.Probe_Cathode).gameObject.SetActive(false);
    }
    protected override void UnBindEventAttatchedObj()
    {
        base.UnBindEventAttatchedObj();
        UnBindInteractionEvent();
    }
    public void DepthC52Init()
    {
       
        PreCommonInit();

        SetDefaultTransform();
        BindInteractionEvent();

      
        
        BindHighlight((int)DepthC5_GameObj.BlockingPipePart,"막힌 배관 청소");
        GetObject((int)DepthC5_GameObj.BlockingPipePart).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 4) 
            {
                Logger.Log("밸브개방---------------------");
                OnStepMissionComplete(animationNumber:4);
            }  
        });
        
                
        BindHighlight((int)DepthC5_GameObj.PanelDoorHandle,"전원 패널 덮개 열기");
        GetObject((int)DepthC5_GameObj.PanelDoorHandle).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 7) 
            {
                Logger.Log("패널 덮개 개방---------------------");
                OnStepMissionComplete(animationNumber:7);
            }  
        });
       
        InitProbePos();
        SetPressureSensorCurrentCheckMultimeterSection();
        
        InitScrews();
        SetScrewDriverSection();
        

        
        LateCommonInit();
    }



    protected override void PreCommonInit()
    {
        base.PreCommonInit();
        
        defaultRotationMap.TryAdd((int)DepthC5_GameObj.Probe_Cathode,GetObject((int)DepthC5_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC5_GameObj.Probe_Anode,GetObject((int)DepthC5_GameObj.Probe_Cathode).transform.rotation);
        C5_PreCommonObjInit();
        UnBindEventAttatchedObj();
    }
    private void C5_PreCommonObjInit()
    {
        
        GetObject((int)DepthC5_GameObj.Wrench).SetActive(false);
        GetObject((int)DepthC5_GameObj.ContaminatedRod).SetActive(false);
        GetObject((int)DepthC5_GameObj.LevelSensor_TankWaterFluidEffect).SetActive(false);
        GetObject((int)DepthC5_GameObj.LevelSensor_ResidueTankWaterFluidEffect).SetActive(false);
        GetObject((int)DepthC5_GameObj.NewLevelSensor).SetActive(false);
    }

    public void DepthC53Init()
    {
      
        PreCommonInit();
        
        SetPressureSensorCurrentCheckMultimeterSection();
     
        
        InitScrewForConductiveCheck();

    
        
        BindHighlight((int)DepthC5_GameObj.PowerHandle,"전원 차단");
        GetObject((int)DepthC5_GameObj.PowerHandle).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 3) 
            {
                Logger.Log("전원차단---------------------");
                OnStepMissionComplete(animationNumber:3);
            }  
        });

        
        BindHighlight((int)DepthC5_GameObj.LevelSensor_PipeValve,"밸브 개방");
        GetObject((int)DepthC5_GameObj.LevelSensor_PipeValve).BindEvent(() =>
        {
            
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 4) 
            {
                Logger.Log("밸브개방---------------------");
                OnStepMissionComplete(animationNumber:4);
            }  
        });

        
        BindHighlight((int)DepthC5_GameObj.LevelSensor,"새로운 유량센서로 교체");
        GetObject((int)DepthC5_GameObj.LevelSensor).BindEvent(() =>
        {
          
            if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 6) 
            {
                Logger.Log("새로운 센서로 교체---------------------");
                OnStepMissionComplete(animationNumber:6);
            }  
        });
        
        InitScrews();
        SetScrewDriverSection();
        
        InitProbePos();
        SetDefaultTransform();
        BindInteractionEvent();
        BindEventForLevelSensorDisplay();

        
        LateCommonInit();
        
    }
    

    

    protected virtual void ToolBoxOnLevelSensorBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 2) 
        {
        
            OnStepMissionComplete(animationNumber:2);
        }
        

    }

    private void ToolBoxNewAdapterClicked()
    {
       
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5) 
        { Logger.Log("갑압부 교체---------------------");
            OnStepMissionComplete(animationNumber:5);
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

    private void BindInteractionEvent()
    {
        
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        ControlPanelController.PowerOnOffActionWithBool += PowerOnOff;
        

        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
        
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
      
        UI_ToolBox.ToolBox_LevelSensorBtnClicked -= ToolBoxOnLevelSensorBtnClicked;
        UI_ToolBox.ToolBox_LevelSensorBtnClicked += ToolBoxOnLevelSensorBtnClicked;

        
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
        UI_ToolBox.ToolBox_LevelSensorBtnClicked -= ToolBoxOnLevelSensorBtnClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        
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
        
        BindHighlight((int)DepthC5_GameObj.ConnectionScrewB,"접속단자");

        
        currentScrewGaugeStatus.TryAdd((int)DepthC5_GameObj.ConnectionScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC5_GameObj.ConnectionScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC5_GameObj.ConnectionScrewC, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC5_GameObj.ConnectionScrewD, 0);
       // currentScrewGaugeStatus.TryAdd((int)DepthC5_GameObj.LeverScrew, 0);

        animatorMap.TryAdd((int)DepthC5_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC5_GameObj.ElectricScrewdriver).GetComponent<Animator>());

    
        animatorMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewB,
            GetObject((int)DepthC5_GameObj.ConnectionScrewB).GetComponent<Animator>());

        // animatorMap.TryAdd((int)DepthC5_GameObj.LeverScrew,
        //     GetObject((int)DepthC5_GameObj.LeverScrew).GetComponent<Animator>());
        

        animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련
        

        GetObject((int)DepthC5_GameObj.ConnectionScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].enabled = true;
                animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
              
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC5_GameObj.ConnectionScrewB);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC5_GameObj.ConnectionScrewB);
                }
                
                animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].SetBool(isWindSession? TO_SCREW_B_WIND:TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);
        
        
        GetObject((int)DepthC5_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].Play(TO_SCREW_A, 0, 0);
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].Update(0);

           // animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC5_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].Play(isWindSession? TO_SCREW_B_WIND:TO_SCREW_B, 0, 0);
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].Update(0);

            //animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].SetBool(isWindSession? TO_SCREW_B_WIND:TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);


        
        #endregion

        
        GetObject((int)DepthC5_GameObj.ConnectionScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].SetBool(isWindSession? TO_SCREW_B_WIND:TO_SCREW_B, false);
            animatorMap[(int)DepthC5_GameObj.ElectricScrewdriver].enabled = false;
        });



        InitializeTool();



    }

    
    
    /// <summary>
    /// 1.전류 측정모드 분리를 위한 압력센서 전용 멀티미터 클래스 초기화 로직입니다
    /// 2. 다른센서 사용 XX
    /// </summary>
    private void SetPressureSensorCurrentCheckMultimeterSection()
    {
        multimeterController = GetObject((int)DepthC5_GameObj.Multimeter).GetComponent<CurrentCheckableMultimeterController>();
 

        animatorMap.TryAdd((int)DepthC5_GameObj.Multimeter,
            GetObject((int)DepthC5_GameObj.Multimeter).GetComponent<Animator>());

        animatorMap.TryAdd((int)DepthC5_GameObj.Probe_Anode,
            GetObject((int)DepthC5_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC5_GameObj.Probe_Cathode,
            GetObject((int)DepthC5_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC5_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = false;
    }
    protected virtual void OnToolBoxClicked()
    {
        if((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 7))
        {
         //   OnStepMissionComplete(animationNumber: 7);
        }
    }
    
    private void InitScrewForConductiveCheck()
    {
        GetObject((int)DepthC5_GameObj.AnodeSensorOutput).BindEvent(() =>
        {
          
            
            if((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 11))
            {
                animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(MULTIMETER_ON, true);
                
                SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewA);
                SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB, false);
                
                SetHighlightIgnore((int)DepthC5_GameObj.AnodeSensorOutput);
                BlinkHighlight((int)DepthC5_GameObj.CathodeSensorInput);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
           

        });
     
        GetObject((int)DepthC5_GameObj.CathodeSensorInput).BindEvent(() =>
        {
            
                    
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 11)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 12))
            {
                
                if (!isAnodePut) return;

                Logger.Log("Probe Set == 11");
                animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(MULTIMETER_ON, true);

                Action action = multimeterController.PS_OnAllProbeSetOnCurrentCheckMode;
                         if (Managers.ContentInfo.PlayData.Count == 11)
                {
                    OnStepMissionComplete(animationNumber: 11, delayTimeAmount: new WaitForSeconds(6f),ActionBeforeDelay:action);
                }
   
                
            }
            
        }, Define.UIEvent.PointerDown);


        
        
        GetObject((int)DepthC5_GameObj.ConnectionScrewC).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 17)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(TO_SCREW_C, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() =>
                {
                                    
                    ChangeTooltipText((int)DepthC5_GameObj.ConnectionScrewD, "측정단자 D");
                    SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewD, false);
                    BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewD);

                    isAnodePut = true;
                });
            }

        });
        
        
        GetObject((int)DepthC5_GameObj.ConnectionScrewD).BindEvent(() =>
        {
            if ((Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 17)||
                (Managers.ContentInfo.PlayData.Depth3 == 3 &&Managers.ContentInfo.PlayData.Count == 13))
            {

                animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, true);
                Action action = multimeterController.OnAllProbeSetOnConductiveCheckMode;


                if (Managers.ContentInfo.PlayData.Count == 12)
                {
                    OnStepMissionComplete(animationNumber: 12, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                else if (Managers.ContentInfo.PlayData.Count == 17)
                {
                    OnStepMissionComplete(animationNumber: 17, delayTimeAmount: new WaitForSeconds(4f),ActionBeforeDelay:action);
                }
                
               

             
                
                SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewD);

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
                if (CurrentActiveTool == (int)DepthC5_GameObj.ElectricScrewdriver) Managers.Sound.Play(SoundManager.Sound.Effect, "Object/ElectronicDriver", 0.4f);
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


        if (objectHighlightMap[(int)DepthC5_GameObj.ConnectionScrewB].ignore)
        {
            Logger.Log($"this screw's highlight is ignored.. driver logic can be executed. return");
            return;
        }
        
        if (CurrentActiveTool != (int)DepthC5_GameObj.ElectricScrewdriver)
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
             (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 9) ||
             (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 25) ||
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


        if (objectHighlightMap[(int)DepthC5_GameObj.ConnectionScrewB].ignore) return;
        if (CurrentActiveTool != (int)DepthC5_GameObj.ElectricScrewdriver)
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
                
            animatorMap[(int)DepthC5_GameObj.Multimeter].SetBool(MULTIMETER_ON, isMultimeterOn);
            if (!_isMultimeterOn)
            {
                GetObject((int)DepthC5_GameObj.Probe_Cathode)?.SetActive(false);
                GetObject((int)DepthC5_GameObj.Probe_Anode)?.SetActive(false);
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
                CurrentActiveTool = (int)DepthC5_GameObj.ElectricScrewdriver;
            else
            {
                CurrentActiveTool = NO_TOOL_SELECTED;
                GetObject((int)DepthC5_GameObj.ElectricScrewdriver).SetActive(false);
            }
        }
    }


    protected virtual void Update()
    {
        SetToolPos();
    }
    
    public CurrentCheckableMultimeterController multimeterController;


    protected virtual void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
            Input.mousePosition.y + _toolPosYOffset,
            distanceFromCamera));


        if (isDriverOn && CurrentActiveTool == (int)DepthC5_GameObj.ElectricScrewdriver)
        {
            GetObject((int)DepthC5_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC5_GameObj.ElectricScrewdriver).transform.position = mousePosition;
        }

        else if (isMultimeterOn && CurrentActiveTool == (int)DepthC5_GameObj.Multimeter &&
                 multimeterController.isCurrentCheckMode )
        {
            GetObject((int)DepthC5_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC5_GameObj.Probe_Anode).SetActive(isMultimeterOn);

            if ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                 !isAnodePut)
                || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                    !isAnodePut)

                )
            {
                GetObject((int)DepthC5_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC5_GameObj.Probe_Anode];

                GetObject((int)DepthC5_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if
                ((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                  isAnodePut)
                 || (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 11 &&
                     isAnodePut)
                 )
            {
                GetObject((int)DepthC5_GameObj.Probe_Cathode).transform.rotation =
                    defaultRotationMap[(int)DepthC5_GameObj.Probe_Cathode];

                GetObject((int)DepthC5_GameObj.Probe_Cathode).transform.position = mousePosition;
                
           
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
        
        
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 8)
        {
            OnStepMissionComplete(animationNumber:8);
        }

        
        CurrentActiveTool = (int)DepthC5_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) CurrentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC5_GameObj.ElectricScrewdriver), isDriverOn);
        Logger.Log($"Electronic Screw Driver btn Clicked -------is driver on? : {isDriverOn}");
        
    }
    
    

    protected virtual void OnUIToolBoxMultimeterBtnClicked()
    {
      
        InitializeTool();
        CurrentActiveTool = (int)DepthC5_GameObj.Multimeter;
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
        
        if (isMultimeterOn == false) CurrentActiveTool = -1;
    }
    
    protected virtual void OnCurrentModeSet()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 10)
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
        CurrentActiveTool = (int)DepthC5_GameObj.Multimeter;
        contentController.uiToolBox.Refresh(UI_ToolBox.Btns.Btn_Multimeter);
        
        multimeterController.PS_SetToDefaultMode();
        
        BlinkHighlight((int)DepthC5_GameObj.MultimeterHandleHighlight);
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
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = true;
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = true;
        
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(TO_SCREW_A, false);
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(TO_SCREW_C, false);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(TO_SCREW_B, false);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(TO_SCREW_D, false);

        animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = false;
        
        isAnodePut = false;
        
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].Play("A_ON", 0, 0);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].Play("B_ON", 0, 0);
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].Play("C_ON", 0, 0);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].Play("D_ON", 0, 0);
    }


    /// <summary>
    /// Screw관련 스텝이아닌경우 실행
    /// </summary>
    public void BlockScrewInteraction()
    {
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewA, true);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB, true);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewC, true);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewD, true);
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

        isScrewWindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewA, false);
        isScrewWindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewB, false);
        isScrewWindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewC, false);
        isScrewWindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewD, false);


        isScrewUnwindMap = new Dictionary<int, bool>();

        isScrewUnwindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewA, false);
        isScrewUnwindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewB, false);
        isScrewUnwindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewC, false);
        isScrewUnwindMap.TryAdd((int)DepthC5_GameObj.ConnectionScrewD, false);
        
        


                
    }
    protected void InitializeTool()
    {
        CurrentActiveTool = -1;
        ToggleActiveState(GetObject((int)DepthC5_GameObj.ElectricScrewdriver), false);
        animatorMap[(int)DepthC5_GameObj.Multimeter].SetBool(MULTIMETER_ON, false); // 멀티미터는 active상태로 유지합니다.
    }

    public override void ClearTool()
    {
        CurrentActiveTool =  -1;
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
        
        
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewA, false);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewB, false);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewC, false);
        SetHighlightIgnore((int)DepthC5_GameObj.ConnectionScrewD, false);
      

         BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewA);
         BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewB);
         BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewC);
         BlinkHighlight((int)DepthC5_GameObj.ConnectionScrewD);
         
        
        //나사 위치 초기화
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].enabled = true;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = true;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].enabled = true;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].enabled = true;
        
        
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].SetBool(DepthC2_SceneController.UNWIND,!forWindAnim);
    
        
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].Play(StateName, 0, 0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Play(StateName, 0, 0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].Play(StateName, 0, 0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].Play(StateName, 0, 0);
  
        
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].Update(0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].Update(0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].Update(0);
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].Update(0);
 
        
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].StopPlayback();
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].StopPlayback();
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].StopPlayback();
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].StopPlayback();
       
        
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewA].enabled = false;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewB].enabled = false;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewC].enabled = false;
        animatorMap[(int)DepthC5_GameObj.ConnectionScrewD].enabled = false;
       
     
        contentController.isStepMissionPerformable = true;
        
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
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
        
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Anode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].enabled = false;
        animatorMap[(int)DepthC5_GameObj.Probe_Cathode].SetBool(DepthC2_SceneController.PROBE_TO_SCREWB, false);
    }
    /// <summary>
    ///     1. 개발전용입니다. 
    ///     2. 씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     3. 메인화면부터 각 씬별로 테스트를 할 수 있도록 하기 위한 메소드입니다.
    /// </summary>
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 5;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;
    }


    private void InitializeC5States()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 3511,  new DepthC51_State_1(this) },
            { 3512,  new DepthC51_State_2(this) },
            { 3513,  new DepthC51_State_3(this) },
            { 3514,  new DepthC51_State_4(this) },
            { 3515,  new DepthC51_State_5(this) },
            { 3516,  new DepthC51_State_6(this) },
            { 3517,  new DepthC51_State_7(this) },
            { 3518,  new DepthC51_State_8(this) },
            { 3519,  new DepthC51_State_9(this) },
            { 35110, new DepthC51_State_10(this) },
            { 35111, new DepthC51_State_11(this) },
            { 35112, new DepthC51_State_12(this) },
            // { 34113, new DepthC51_State_13(this) },
            // { 34114, new DepthC51_State_14(this) },
            // { 34115, new DepthC51_State_15(this) },
            // { 34116, new DepthC51_State_16(this) },
            // { 34117, new DepthC51_State_17(this) },
            
            { 3521,  new DepthC52_State_1(this) },
            { 3522,  new DepthC52_State_2(this) },
            { 3523,  new DepthC52_State_3(this) },
            { 3524,  new DepthC52_State_4(this) },
            { 3525,  new DepthC52_State_5(this) },
            { 3526,  new DepthC52_State_6(this) },
            { 3527,  new DepthC52_State_7(this) },
            { 3528,  new DepthC52_State_8(this) },
            { 3529,  new DepthC52_State_9(this) },
            { 35210, new DepthC52_State_10(this) },
            { 35211, new DepthC52_State_11(this) },
            { 35212, new DepthC52_State_12(this) },
            { 35213, new DepthC52_State_13(this) },

            { 3531,  new DepthC53_State_1(this) },
            { 3532,  new DepthC53_State_2(this) },
            { 3533,  new DepthC53_State_3(this) },
            { 3534,  new DepthC53_State_4(this) },
            { 3535,  new DepthC53_State_5(this) },
            { 3536,  new DepthC53_State_6(this) },
            { 3537,  new DepthC53_State_7(this) },
            { 3538,  new DepthC53_State_8(this) },
            { 3539,  new DepthC53_State_9(this) },
            { 35310, new DepthC53_State_10(this) },
            { 35311, new DepthC53_State_11(this) },
            { 35312, new DepthC53_State_12(this) },
            { 35313, new DepthC53_State_13(this) },
            { 35314, new DepthC53_State_14(this) },
            { 35315, new DepthC53_State_15(this) },
            { 35316, new DepthC53_State_16(this) },
            { 35317, new DepthC53_State_17(this) },
            { 35318, new DepthC53_State_18(this) },
            { 35319, new DepthC53_State_19(this) },
            { 35320, new DepthC53_State_20(this) },
            { 35321, new DepthC53_State_21(this) },
            { 35322, new DepthC53_State_22(this) },
            { 35323, new DepthC53_State_23(this) },
            { 35324, new DepthC53_State_24(this) },
            { 35325, new DepthC53_State_25(this) },
            { 35326, new DepthC53_State_26(this) },
        };
    }
}
    
