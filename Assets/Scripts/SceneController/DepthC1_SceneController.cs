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

/// <summary>
/// 검출스위치
/// </summary>

public enum DepthC1_GameObj
{
    // Common------------------
    ElectricScrewdriver,
    Multimeter,
    MultimeterHandleHighlight,
    Indicator,
    Probe_Anode, // negative
    Probe_Cathode, // positive
    PxS_PowerHandle,
    PxS_InnerScrewA, //Proximity Sensor
    PxS_InnerScrewB,
    PxS_InnerScrewC,
    ControlPanel,
    LimitSwitch,
    Lever_Handle,
    Limitswitch_ArmPivot,
    LS_Cover,
    ConnectionScrewA,
    ConnectionScrewB,
    ConnectionScrewC,
    ConnectionScrewD
}

public class DepthC1_SceneController : Base_SceneController
{

   
   

    private readonly int UNWOUND_COUNT_GOAL = 3;
    private int _unwoundCount;

    public C1_LimitSwitchPivotController limitSwitchPivotController
    {
        get
        {
            return GetObject((int)DepthC1_GameObj.LimitSwitch).GetComponent<C1_LimitSwitchPivotController>();
        }
    }


    public void InitTransform(DepthC1_GameObj obj)
    {
        GetObject((int)obj).transform.position = defaultPositionMap[(int)obj];
        GetObject((int)obj).transform.rotation = defaultRotationMap[(int)obj];
    }

    public void SetAnimator(DepthC1_GameObj obj)
    {
        if (animatorMap == null) animatorMap = new Dictionary<int, Animator>();

        animatorMap.TryAdd((int)obj, GetObject((int)obj).GetComponent<Animator>());
    }

    public void SetDefaultTransform(DepthC1_GameObj obj)
    {
        if (defaultPositionMap == null) defaultPositionMap = new Dictionary<int, Vector3>();
        var defaultPosCache =   GetObject((int)obj).transform;
        defaultPositionMap.TryAdd((int)obj,defaultPosCache.position);
        
        if (defaultRotationMap == null) defaultRotationMap = new Dictionary<int, Quaternion>();
        var defaultRotationCache =   GetObject((int)obj).transform.rotation;
        defaultRotationMap.TryAdd((int)obj,defaultRotationCache);
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
                if (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 6)
                {
                    Logger.Log($"평가하기: 커버열고 모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    OnStepMissionComplete(animationNumber:6);
                    CurrentActiveTool = -1;
                    isDriverOn = false;
                    _unwoundCount = 0;
                }

                if (Managers.ContentInfo.PlayData.Depth1 == 3 && Managers.ContentInfo.PlayData.Depth3 == 1 )
                {
                    Logger.Log($"모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    OnStepMissionComplete(animationNumber:12);
                    _unwoundCount = 0;//초기화 
                }
                
                if (Managers.ContentInfo.PlayData.Depth1 == 3 && Managers.ContentInfo.PlayData.Depth3 == 3  && Managers.ContentInfo.PlayData.Count == 6)
                {
                    Logger.Log($"모든 나사 풀림 (6) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                    OnStepMissionComplete(animationNumber:6);
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

                if (Managers.ContentInfo.PlayData.Depth1 == 3 && Managers.ContentInfo.PlayData.Count == 12)
                {
                    Logger.Log($"훈련모드모든 나사 조임 (12) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _woundCount}");
                    OnStepMissionComplete(animationNumber: 12);
                    _woundCount = 0; //초기화 
                }
            }
        }
    }


    [Range(-200f, 250f)] public float _toolPosXOffset = 0.3f;
    [Range(-200f, 250f)] public float _toolPosYOffset = -0.3f;

    public void ScrewWoundCountInit()
    {
        woundCount = 0;
        unwoundCount = 0;
    }
    public static readonly int UNWIND = Animator.StringToHash("Unwind");

    public static readonly int TO_SCREW_A = Animator.StringToHash("ScrewA");
    public static readonly int TO_SCREW_B = Animator.StringToHash("ScrewB");
    public static readonly int TO_SCREW_C = Animator.StringToHash("ScrewC");

    public static readonly int PROBE_TO_SCREWB = Animator.StringToHash("On");
    public static readonly int MULTIMITER_ON = Animator.StringToHash("On");

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
        _screwColliders[(int)DepthC1_GameObj.PxS_InnerScrewA] = GetObject((int)DepthC1_GameObj.PxS_InnerScrewA).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.PxS_InnerScrewB] = GetObject((int)DepthC1_GameObj.PxS_InnerScrewB).GetComponent<Collider>();
        _screwColliders[(int)DepthC1_GameObj.PxS_InnerScrewC] = GetObject((int)DepthC1_GameObj.PxS_InnerScrewC).GetComponent<Collider>();
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
        
        
        InitializeC1States();
        GetScrewColliders();
        contentController.OnDepth2Clicked(1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
        
    }
    private void LateCommonInit()
    {
        
        ClearTool();
        isAnodePut = false;
        indicator.ShowNothing();
    }



    public void DepthC21Init()
    {

    }
    
    public void DepthC23Init()
    {

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
        UnbindStaticEvents();
    }
    public void DepthC22Init()
    {
        UnBindEventAttatchedObj();
        PreCommonInit();
        InitProbePos();
        BindToolBoxUIEvent();
       
        
        LateCommonInit();
    }

    private void BindToolBoxUIEvent()
    {
        
        UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
        UI_ToolBox.TemperatureSensorClickedEvent += OnUI_Btn_TemperatureSensorClicked;

        UI_ToolBox.ScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
        
        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;
        
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceModeSet;
        MultimeterController.OnResistanceMeasureReadyAction += OnResistanceModeSet;

    }
    
    public void DepthC11Init()
    {
      
        PreCommonInit();
        UnBindEventAttatchedObj();


     
        SetScrewDriverSection();

        InitProbePos();
        BindToolBoxUIEvent();
        
        SetDefaultTransform(DepthC1_GameObj.LS_Cover);
        
        
        
        BindHighlight((int)DepthC1_GameObj.Lever_Handle,"리밋스위치 레버");
        
        
        GetObject((int)DepthC1_GameObj.Lever_Handle).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 6)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.Lever_Handle, 6);
            }
        });
        
        
        BindHighlight((int)DepthC1_GameObj.LS_Cover,"리밋스위치 커버");
                
        GetObject((int)DepthC1_GameObj.LS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 9)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.LS_Cover, 9);
            }
        });


        BindHighlight((int)DepthC1_GameObj.ConnectionScrewA,"접속나사 확인");
        
        GetObject((int)DepthC1_GameObj.ConnectionScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 10)
            {
                Logger.Log("MissionComplete limitswitch");
                OnStepMissionComplete((int)DepthC1_GameObj.ConnectionScrewA, 10);
            }
        });
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewB,"접속나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewC,"접속나사");
        BindHighlight((int)DepthC1_GameObj.ConnectionScrewD,"접속나사");
        
        
        
        
        LateCommonInit();
        
    }
    

    
    protected virtual void OnUI_Btn_TemperatureSensorClicked()
    {
        
    }

    protected virtual void PowerOnOff(bool isOn)
    {
    
    }



    protected  virtual void UnbindStaticEvents()
    {
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.ScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceModeSet;
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnbindStaticEvents();
    }

    /// <summary>
    /// 드라이버로 나사를 푸는 경우의 State에 사용됩니다.
    /// 자원 할당 이외 직접적인 사용은 최대한 Depth별 State에서 사용하도록 합니다. 
    /// 적절한 초기화 작업이 필요합니다(10/14/24)
    /// </summary>
    public void SetScrewDriverSection(bool isWind = false)
    {
        multimeterController = GetObject((int)DepthC1_GameObj.Multimeter).GetComponent<MultimeterController>();
        
        currentScrewGaugeStatus = new Dictionary<int, float>();
        
        
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewC, 0);


        animatorMap.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewA,
            GetObject((int)DepthC1_GameObj.PxS_InnerScrewA).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewB,
            GetObject((int)DepthC1_GameObj.PxS_InnerScrewB).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.PxS_InnerScrewC,
            GetObject((int)DepthC1_GameObj.PxS_InnerScrewC).GetComponent<Animator>());


        animatorMap.TryAdd((int)DepthC1_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC1_GameObj.ElectricScrewdriver).GetComponent<Animator>());

        animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;


        animatorMap.TryAdd((int)DepthC1_GameObj.Multimeter,
            GetObject((int)DepthC1_GameObj.Multimeter).GetComponent<Animator>());

     


        animatorMap.TryAdd((int)DepthC1_GameObj.Probe_Anode,
            GetObject((int)DepthC1_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC1_GameObj.Probe_Cathode,
            GetObject((int)DepthC1_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC1_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC1_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC1_GameObj.Probe_Cathode].enabled = false;

        animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewA)
            .BindEvent(() =>
            {
               animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
               animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.PxS_InnerScrewA);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.PxS_InnerScrewA);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.PxS_InnerScrewB);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.PxS_InnerScrewB);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewC)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].enabled = false;
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = true;
                
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC1_GameObj.PxS_InnerScrewC);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC1_GameObj.PxS_InnerScrewC);
                }
                
                animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
            }, Define.UIEvent.Pressed);


        GetObject((int)DepthC1_GameObj.PxS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_A, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

           // animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_B, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

            //animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Play(TO_SCREW_C, 0, 0);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].Update(0);

         
         //   animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
        }, Define.UIEvent.PointerDown);

        #endregion


        GetObject((int)DepthC1_GameObj.PxS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC1_GameObj.PxS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].enabled = false;
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, false);
            animatorMap[(int)DepthC1_GameObj.ElectricScrewdriver].enabled = false;
        });



        GetObject((int)DepthC1_GameObj.PxS_InnerScrewB).BindEvent(() =>
        {
            // OnScrewClickUp();
            // _animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(ON, true);
            // _animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(ON, true);
        });

        InitializeTool();



    }

    protected virtual void OnToolBoxClicked()
    {
      
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


        if (objectHighlightMap[(int)DepthC1_GameObj.PxS_InnerScrewC].ignore) return;
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

    private bool CheckDriverUsability()
    {
        if (((Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 12) ||
              (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 10)||
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 6) ||
              (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 12) ||
              (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 6)||
              (Managers.ContentInfo.PlayData.Depth1 == 4 && Managers.ContentInfo.PlayData.Count == 10)))
        {
            return true;
        }
        else
        {
            Logger.Log("Driver is not usable fn ------------------");
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


        if (objectHighlightMap[(int)DepthC1_GameObj.PxS_InnerScrewC].ignore) return;
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
            animatorMap[(int)DepthC1_GameObj.Multimeter].SetBool(MULTIMITER_ON, isMultimeterOn);
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
        else if (isMultimeterOn && CurrentActiveTool == (int)DepthC1_GameObj.Multimeter && multimeterController.isResistanceMode)
        {
            GetObject((int)DepthC1_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC1_GameObj.Probe_Anode).SetActive(isMultimeterOn);
        
            if ((Managers.ContentInfo.PlayData.Count >= 13 && !isAnodePut) 
                ||(Managers.ContentInfo.PlayData.Depth3 ==3 && Managers.ContentInfo.PlayData.Count>=9 &&!isAnodePut))
            {
                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC1_GameObj.Probe_Anode];
                
                GetObject((int)DepthC1_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if ((Managers.ContentInfo.PlayData.Count >= 13 && isAnodePut)
                ||(Managers.ContentInfo.PlayData.Depth3 ==3 && Managers.ContentInfo.PlayData.Count>=9 &&isAnodePut))
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
        
        
        
        
        CurrentActiveTool = (int)DepthC1_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) CurrentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC1_GameObj.ElectricScrewdriver), isDriverOn);

       
        
        
        
        Logger.Log($"is driver on? : {isDriverOn}");

        if (Managers.ContentInfo.PlayData.Count == 9 && Managers.ContentInfo.PlayData.Depth3 == 2)
        {
            OnStepMissionComplete(animationNumber:9);
        }
    }
    
    

    protected virtual void OnUI_MultimeterBtnClicked()
    {
      
        InitializeTool();
        CurrentActiveTool = (int)DepthC1_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false) CurrentActiveTool = -1;
        
        Logger.Log($"is Multimeter on? : {isMultimeterOn}");

        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 13)
        {
            OnStepMissionComplete(animationNumber:13);
        }
        

    }
    
    protected virtual void OnResistanceModeSet()
    {
      
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 14)
        {
            OnStepMissionComplete(animationNumber:14);
        }
    }
    
    protected virtual void OnConductiveModeSet()
    {
      
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 15)
        {
            OnStepMissionComplete(animationNumber:15);
        }
    }

    protected void InitializeTool()
    {
        CurrentActiveTool = -1;
        ToggleActiveState(GetObject((int)DepthC1_GameObj.ElectricScrewdriver), false);
        animatorMap[(int)DepthC1_GameObj.Multimeter].SetBool(PROBE_TO_SCREWB, false); // 멀티미터는 active상태로 유지합니다.
    }

    public void ClearTool()
    {
        CurrentActiveTool =  -1;
        isDriverOn= false;
        isMultimeterOn = false;
        
    }

    public void SetUnscrewStatus(bool isUnscrewed)
    {

        int unscrewd = 0;
        int screwd = 1;
        
        
        contentController.isStepMissionPerformable = true;
        foreach (var key in  currentScrewGaugeStatus.Keys.ToList())
        {
            currentScrewGaugeStatus[key] = 0f;
        }
        
             
        foreach (var key in  isScrewUnwindMap.Keys.ToList())
        {
            isScrewUnwindMap[key] = isUnscrewed;
        }

        if (isUnscrewed)
        {
            BlinkHighlight((int)DepthC1_GameObj.PxS_InnerScrewA);
            BlinkHighlight((int)DepthC1_GameObj.PxS_InnerScrewB);
            BlinkHighlight((int)DepthC1_GameObj.PxS_InnerScrewC);
        }
        
        SetHighlightIgnore((int)DepthC1_GameObj.PxS_InnerScrewA, isUnscrewed);
        SetHighlightIgnore((int)DepthC1_GameObj.PxS_InnerScrewB, isUnscrewed);
        SetHighlightIgnore((int)DepthC1_GameObj.PxS_InnerScrewC, isUnscrewed);
        
        
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].enabled = true;
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].enabled = true;
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].enabled = true;
       
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
       
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].Update(0);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].Update(0);
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].Update(0);
       
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].StopPlayback();
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].StopPlayback();
       animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].StopPlayback();
       
      animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewA].enabled = false;
      animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewB].enabled = false;
      animatorMap[(int)DepthC1_GameObj.PxS_InnerScrewC].enabled = false;
    }


    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
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
       

            { 3211, new DepthC12_State_1(this) },
            { 3212, new DepthC12_State_2(this) },
            { 3213, new DepthC12_State_3(this) },
            { 3214, new DepthC12_State_4(this) },
            { 3215, new DepthC12_State_5(this) },
            { 3216, new DepthC12_State_6(this) },
            { 3217, new DepthC12_State_7(this) },
            { 3218, new DepthC12_State_8(this) },
            { 3219, new DepthC12_State_9(this) },
            { 32110, new DepthC12_State_10(this) },
            { 32111, new DepthC12_State_11(this) },
            { 32112, new DepthC12_State_12(this) },
            { 32113, new DepthC12_State_13(this) },
    
            { 3311, new DepthC13_State_1(this) },
            { 3312, new DepthC13_State_2(this) },
            { 3313, new DepthC13_State_3(this) },
            { 3314, new DepthC13_State_4(this) },
            { 3315, new DepthC13_State_5(this) },
            { 3316, new DepthC13_State_6(this) },
            { 3317, new DepthC13_State_7(this) },
            { 3318, new DepthC13_State_8(this) },
            { 3319, new DepthC13_State_9(this) },
            { 33110, new DepthC13_State_10(this) },
            { 33111, new DepthC13_State_11(this) },
            { 33112, new DepthC13_State_12(this) },
            { 33113, new DepthC13_State_12(this) },

        };
    }
}
    
