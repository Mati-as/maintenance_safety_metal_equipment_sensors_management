using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;


/// <summary>
///     주의사항
///     1.초기화 및 이벤트성 함수만 담도록 구성합니다.
///     2.상태에 따른 애니메이션 수행은 최대한 DepthCState에 구성합니다.
/// </summary>
public enum DepthC2_GameObj
{
    // Common------------------
    ElectricScrewdriver,
    Multimeter,
    MultimeterHandleHighlight,
    Indicator,
    Probe_Anode, // negative
    Probe_Cathode, // positive
    Wrench,
        
    //Temperature sesnor ----------
    TS_CompensatingWire,
    TS_Stabilizer,
    TS_SensingElement,
    TS_Cover,
    TS_LockingScrew,
    TS_ConnectionPiping,
    TS_InnerScrewA,
    TS_InnerScrewB,
    TS_InnerScrewC,
    TS_GroundingTerminalA,
    TS_GroundingTerminalB,
    TemperatureSensor,
    OnTempSensor_Pipe,
    NewTemperatureSensor,
    Pipe_WaterEffect,
    WaterLeakEffect,
    WaterDecal,
    PowerHandle,
    TankValve,
    
    //주의: 튜토리얼전용
    TS_CoverScrew,
    LookAtPoint3
    

}


public class DepthC2_SceneController : Base_SceneController
{

    public Dictionary<int, float> currentScrewGaugeStatus; // 나사 게이지 캐싱
    public Dictionary<int, Animator> animatorMap;
    public Dictionary<int, Sequence> _seqMap;
    public Dictionary<int, bool> isScrewUnwindMap; //3.2.1 , 3,2,3
    public Dictionary<int, bool> isScrewWindMap; // 3.2.2
    public Dictionary<int, Quaternion> defaultRotationMap;


    
    
    
    private readonly int UNWOUND_COUNT_GOAL = 3;
    private int _unwoundCount;
    

    private ParticleSystem _waterLeakPs;

    
    
    public ParticleSystem waterLeakPs
    {
        get
        {
            if (_waterLeakPs == null)
                _waterLeakPs = GetObject((int)DepthC2_GameObj.WaterLeakEffect).GetComponent<ParticleSystem>();
            return _waterLeakPs;
        }
    }

    public void SetParticleStatus(bool isOn =true)
    {
        if (isOn)
            waterLeakPs.Play();
        else
            waterLeakPs.Stop();
    }
    

    private IndicatorController _indicator;
    public IndicatorController indicator
    {
        get
        {
            if(_indicator == null)
                _indicator = GetObject((int)DepthC2_GameObj.Indicator).GetComponent<IndicatorController>();
            return _indicator;
        }
        
    }

    private static readonly int Alpha = Shader.PropertyToID("_alpha");
    private DecalProjector _waterDecal;
    public DecalProjector waterDecal
    {
        get
        {
            if (_waterDecal == null)
                _waterDecal = GetObject((int)DepthC2_GameObj.WaterDecal).GetComponent<DecalProjector>();
            return _waterDecal;
        }
       
    }

    public void SetWaterDecalStatus(bool isOn =true)
    {
        GetObject((int)DepthC2_GameObj.WaterDecal).SetActive(isOn);
    }

    public void FadeInDecal()
    {
        
        // set initial alpha to 0
        waterDecal.material.SetFloat(Alpha, 0);

        // attempt to add a new sequence if one doesn’t exist
        if (!_seqMap.TryAdd((int)DepthC2_GameObj.WaterDecal, DOTween.Sequence()))
        {
            // if already exists, kill and clear it
            _seqMap[(int)DepthC2_GameObj.WaterDecal].Kill();
            _seqMap[(int)DepthC2_GameObj.WaterDecal] = DOTween.Sequence();
        }
        
        

        var newSeq = DOTween.Sequence();
        _seqMap[(int)DepthC2_GameObj.WaterDecal] = newSeq;

        // add fade-in effect to the sequence
        newSeq.AppendCallback(() =>
            {
                GetObject((int)DepthC2_GameObj.WaterDecal).SetActive(true);
            })
            .Append(DOVirtual.Float(0, 0.3f, 1.8f, val =>
            {
                waterDecal.material.SetFloat(Alpha, val);
            })).SetUpdate(true); // optionally set to update even if game is paused;


    }

    public void SetWaterMatAlpha(int alpha=0)
    {
        if (!_seqMap.TryAdd((int)DepthC2_GameObj.WaterDecal, DOTween.Sequence()))
        {
            // if already exists, kill and clear it
            _seqMap[(int)DepthC2_GameObj.WaterDecal].Kill();
            _seqMap[(int)DepthC2_GameObj.WaterDecal] = DOTween.Sequence();
        }
        
        waterDecal.material.SetFloat(Alpha, alpha);
    }


    private Dictionary<int, MeshRenderer> _wireMatMap;
    private void FadeInCompensatingWire()
    {
        // _wireMatMap.TryAdd(GetObject((int)DepthC_GameObj.TS_CompensatingWireA),
        //     GetObject((int)DepthC_GameObj.TS_CompensatingWireA)).GetComponent<MeshRenderer>());
    }

    private void FadeOutCompensatingWire()
    {
        
    }

    public void FadeOutDecal()
    {
        // set initial alpha to 0
        waterDecal.material.SetFloat(Alpha, 1);

        // attempt to add a new sequence if one doesn’t exist
        if (!_seqMap.TryAdd((int)DepthC2_GameObj.WaterDecal, DOTween.Sequence()))
        {
            // if already exists, kill and clear it
            _seqMap[(int)DepthC2_GameObj.WaterDecal].Kill();
            _seqMap[(int)DepthC2_GameObj.WaterDecal] = DOTween.Sequence();
        }

        var newSeq = DOTween.Sequence();
        _seqMap[(int)DepthC2_GameObj.WaterDecal] = newSeq;

        // add fade-in effect to the sequence
        newSeq.AppendCallback(() =>
            {
                GetObject((int)DepthC2_GameObj.WaterDecal).SetActive(true);
            })
            .Append(DOVirtual.Float(0.3f, 0f, 1.8f, val =>
            {
                waterDecal.material.SetFloat(Alpha, val);
            })).SetUpdate(true); // optionally set to update even if game is paused;

        

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
                    currentActiveTool = -1;
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

    public static readonly int SCREW_A = Animator.StringToHash("ScrewA");
    public static readonly int SCREW_B = Animator.StringToHash("ScrewB");
    public static readonly int SCREW_C = Animator.StringToHash("ScrewC");
    public static readonly int UNSCREW_A = Animator.StringToHash("UnscrewA");
    public static readonly int UNSCREW_B = Animator.StringToHash("UnscrewB");
    public static readonly int UNSCREW_C = Animator.StringToHash("UnscrewC");

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
        _screwColliders[(int)DepthC2_GameObj.TS_InnerScrewA] = GetObject((int)DepthC2_GameObj.TS_InnerScrewA).GetComponent<Collider>();
        _screwColliders[(int)DepthC2_GameObj.TS_InnerScrewB] = GetObject((int)DepthC2_GameObj.TS_InnerScrewB).GetComponent<Collider>();
        _screwColliders[(int)DepthC2_GameObj.TS_InnerScrewC] = GetObject((int)DepthC2_GameObj.TS_InnerScrewC).GetComponent<Collider>();
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
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Init()
    {
        SetDepthNum(); //개발용

        base.Init();
        BindObject(typeof(DepthC2_GameObj));
        InitializeC2States();
        GetScrewColliders();
        contentController.OnDepth2Init(2); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
        PreCommonInit();
    }

    private void LateCommonInit()
    {
        
        ClearTool();
        isAnodePut = false;
        GetObject((int)DepthC2_GameObj.NewTemperatureSensor).SetActive(false);
        GetObject((int)DepthC2_GameObj.Pipe_WaterEffect).SetActive(false);
        GetObject((int)DepthC2_GameObj.WaterDecal).SetActive(false);
        GetObject((int)DepthC2_GameObj.WaterLeakEffect).SetActive(false);
        SetWaterDecalStatus(false);
        SetParticleStatus(false);
        indicator.ShowNothing();
    }



    public void DepthC21Init()
    {
        
       // Debug.Assert(Managers.ContentInfo.PlayData.Depth1 == 3 && Managers.ContentInfo.PlayData.Depth1 == 2);
        UnBindEventAttatchedObj();
        PreCommonInit();
        
      
        
        #region 3.2.1 온도센서 점검
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;

        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
     
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
        MultimeterController.OnResistanceMeasureReadyAction += OnResistanceReady;

   

        #region 초기화 및 하이라이트 및 텍스트 바인딩 부분


        InitProbePos();
      
        GetObject((int)DepthC2_GameObj.TemperatureSensor).SetActive(true);
       // StartCoroutine(OnSceneStartCo());

        BindHighlight((int)DepthC2_GameObj.TS_CompensatingWire, "보상전선");
        BindHighlight((int)DepthC2_GameObj.TS_Stabilizer, "고정자");
        BindHighlight((int)DepthC2_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindHighlight((int)DepthC2_GameObj.TS_Cover, "덮개");
        BindHighlight((int)DepthC2_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindHighlight((int)DepthC2_GameObj.TS_LockingScrew, "고정나사 체결확인");
        BindHighlight((int)DepthC2_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "보상도선 확인");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewC, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalA, "A 접지");
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "B 접지");
        BindHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight, "저항측정 모드로 설정");

        GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(false);
        GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(false);
        GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(false);
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewD, "나사");
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewE, "나사");

        #endregion


        GetObject((int)DepthC2_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 5)
            {
              OnStepMissionComplete((int)DepthC2_GameObj.TS_LockingScrew, 5);
                
            }
            
            Logger.Log($"잠금 장치 풀림 현재 카운트 {Managers.ContentInfo.PlayData.Depth3}, count: {Managers.ContentInfo.PlayData.Count} --------------------------------------------------------");
        });

        GetObject((int)DepthC2_GameObj.TS_ConnectionPiping ).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 6)
            {
              
                DOVirtual.DelayedCall(0.8f, () =>
                {
                    FadeOutDecal();
                    SetParticleStatus(false);
                });
                
                
                OnStepMissionComplete((int)DepthC2_GameObj.TS_ConnectionPiping, 6);
            }
            
        });

        GetObject((int)DepthC2_GameObj.TS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            if (Managers.ContentInfo.PlayData.Count != 8) return;
            
            OnStepMissionComplete((int)DepthC2_GameObj.TS_Cover, 8);
        });


        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            if (Managers.ContentInfo.PlayData.Count != 9) return;
            OnStepMissionComplete((int)DepthC2_GameObj.TS_InnerScrewA, 9);
        });


        //드라이버 -------------------------------------------------------------------------  
        SetScrewDriverSection();


        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            if (Managers.ContentInfo.PlayData.Count == 15)
            {
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);
                
                BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "측정단자 B");
                
                SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
                SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
                
                BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            
            if (Managers.ContentInfo.PlayData.Count == 16)
            {
                
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);
                
                
                BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
                SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
                BlinkHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
          

        });

        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            
                    
            if (Managers.ContentInfo.PlayData.Count == 15)
            {
                if (!isAnodePut) return;

                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(PROBE_TO_SCREWB, true);

                Action action = multimeterController.OnAllProbeSetOnResistanceMode;
                OnStepMissionComplete(animationNumber: 15, delayTimeAmount: new WaitForSeconds(4f),
                    ActionBeforeDelay: action);
            }

            
            if (Managers.ContentInfo.PlayData.Count != 16) return;

            SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB);
            //CurrentScene.SetHighlightIgnore((int)DepthC_GameObj.TS_GroundingTerminalA,false);
            BlinkHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB);
            SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
            DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(TO_GROUNDING_TERMINAL, true);
            
        }, Define.UIEvent.PointerDown);

       
        
        // GetObject((int)DepthC2_GameObj.TS_GroundingTerminalA).BindEvent(() =>
        // {
        //     
        // });


        GetObject((int)DepthC2_GameObj.TS_GroundingTerminalB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            if (!isAnodePut) return;
            if (!contentController.isStepMissionPerformable) return;
            if (Managers.ContentInfo.PlayData.Count != 16) return;
            contentController.isStepMissionPerformable = false;

            Logger.Log("접지미션 수행 완료");


            animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
            animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(TO_GROUNDING_TERMINAL, true);
            multimeterController.OnAllProbeSetToGroundingTerminal();
         
            OnStepMissionComplete(animationNumber:16, delayTimeAmount: new WaitForSeconds(6f));
        });


      
           

       #endregion

       LateCommonInit();
    }

    public void InitProbePos()
    {
        GetObject((int)DepthC2_GameObj.Probe_Anode).gameObject.SetActive(true);
        GetObject((int)DepthC2_GameObj.Probe_Cathode).gameObject.SetActive(true);
        GetObject((int)DepthC2_GameObj.Probe_Anode).transform.position = _probeDefaultPos;
        GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.position = _probeDefaultPos;
        GetObject((int)DepthC2_GameObj.Probe_Anode).gameObject.SetActive(false);
        GetObject((int)DepthC2_GameObj.Probe_Cathode).gameObject.SetActive(false);
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
          
    
        
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent -= OnUIBtnToolBoxTemperatureSensorClicked;
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent += OnUIBtnToolBoxTemperatureSensorClicked;

        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;
        
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
       
        
        #region 3.2.2 온도센서 고장 유형
        BindHighlight((int)DepthC2_GameObj.TS_LockingScrew, "고정나사 탈거");
       // BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TemperatureSensor, "온도센서 수거");
        BindHighlight((int)DepthC2_GameObj.TS_Cover, "덮개 열기");
        BindHighlight((int)DepthC2_GameObj.TS_SensingElement, "변형된 감온부");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "나사 체결");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "나사 체결");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewC, "나사 체결");
        BindHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight, "저항측정 모드로 설정하기");
        SetScrewDriverSection();

        GetObject((int)DepthC2_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            Debug.Assert(Managers.ContentInfo.PlayData.Depth3 == 2,$"Depth3 is {Managers.ContentInfo.PlayData.Depth3} but must be 2)");
            if (Managers.ContentInfo.PlayData.Count != 4) return;

            OnStepMissionComplete( animationNumber:4,delayTimeAmount:new WaitForSeconds(8f));
            SetHighlightIgnore((int)DepthC2_GameObj.TS_SensingElement, false);
            BlinkHighlight((int)DepthC2_GameObj.TS_SensingElement);
        });
        
        GetObject((int)DepthC2_GameObj.TS_Cover).BindEvent(() =>
        {
            Debug.Assert(Managers.ContentInfo.PlayData.Depth3 == 2,
                $"Depth3 is {Managers.ContentInfo.PlayData.Depth3} but must be 2)");
            
            if (Managers.ContentInfo.PlayData.Count != 8) return;

            OnStepMissionComplete( animationNumber:8,delayTimeAmount:new WaitForSeconds(4f));
        });

       
        
        
        

        #endregion
        LateCommonInit();
    }

    
    
    public void DepthC23Init()
    {
      
        PreCommonInit();
        UnBindEventAttatchedObj();
        InitProbePos();
        
        BindHighlight((int)DepthC2_GameObj.TS_CompensatingWire, "보상전선");
        BindHighlight((int)DepthC2_GameObj.TS_Stabilizer, "고정자");
        BindHighlight((int)DepthC2_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindHighlight((int)DepthC2_GameObj.TS_Cover, "덮개");
        BindHighlight((int)DepthC2_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindHighlight((int)DepthC2_GameObj.TS_LockingScrew, "고정나사 체결확인");
        BindHighlight((int)DepthC2_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewA, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "보상도선 확인");
        BindHighlight((int)DepthC2_GameObj.TS_InnerScrewC, "나사");
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalA, "A 접지");
        BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "B 접지");
        BindHighlight((int)DepthC2_GameObj.PowerHandle, "전원 끄기");
        BindHighlight((int)DepthC2_GameObj.NewTemperatureSensor, "새 온도센서");
        BindHighlight((int)DepthC2_GameObj.TankValve, "밸브");
        BindHighlight((int)DepthC2_GameObj.TemperatureSensor, "교체 할 센서");
        BindHighlight((int)DepthC2_GameObj.MultimeterHandleHighlight, "저항측정 모드로 설정하기");
        
#region 3.2.3 온도 센서 정비 (추가부분)
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;

        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent += OnElectricScrewdriverBtnClicked;

        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent -= OnUIBtnToolBoxTemperatureSensorClicked;
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent += OnUIBtnToolBoxTemperatureSensorClicked;
        
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent += OnUIToolBoxMultimeterBtnClicked;
        
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
        MultimeterController.OnResistanceMeasureReadyAction += OnResistanceReady;
        
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        ControlPanelController.PowerOnOffActionWithBool += PowerOnOff;
        
     

       
        //드라이버 -------------------------------------------------------------------------  
        SetScrewDriverSection(false);
     
        GetObject((int)DepthC2_GameObj.TankValve).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count != 4) return;

            Logger.Log("벨브 잠금 및 유체 차단 애니메이션 재생 -----------------");
            OnStepMissionComplete(animationNumber: 4,delayTimeAmount: new WaitForSeconds(4.5f));
        });

        GetObject((int)DepthC2_GameObj.TS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count != 5)
            {
                Logger.Log("현재 커버관련 재생불가");
                return;
            }
            OnStepMissionComplete(animationNumber: 5,delayTimeAmount: new WaitForSeconds(2.5f));
        });
        
        GetObject((int)DepthC2_GameObj.TemperatureSensor).BindEvent(() =>
        {
            Logger.Log("온도센서 교체 관련 클릭-----------------------");

            if (Managers.ContentInfo.PlayData.Count != 7) return;
            
            OnStepMissionComplete(animationNumber:7,delayTimeAmount:new WaitForSeconds(6f));
        });
        
        // GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        // {
        //     
        // });
#endregion

 #region 321과 중복부분 (다른이벤트 설정필요)

       
        GetObject((int)DepthC2_GameObj.TemperatureSensor).SetActive(true);
        StartCoroutine(OnSceneStartCo());

        

        GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(false);
        GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(false);
        GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(false);

        //
        //
        // GetObject((int)DepthC2_GameObj.TS_ConnectionPiping).BindEvent(() =>
        // {
        //    
        // });

      
        
       
     
    
        //C23-----------------------------------
        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 3) return;
            
            if (Managers.ContentInfo.PlayData.Count == 10)
            {
                
                BindHighlight((int)DepthC2_GameObj.TS_InnerScrewB, "측정단자 B");
                SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, false);
                BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);
                
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            

            if (Managers.ContentInfo.PlayData.Depth3 != 3) return;
            if (Managers.ContentInfo.PlayData.Count != 11) return;
            
            BindHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB, "접지");
            
            SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA);
            SetHighlightIgnore((int)DepthC2_GameObj.TS_GroundingTerminalB, false);
            
            BlinkHighlight((int)DepthC2_GameObj.TS_GroundingTerminalB);
            
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = true;
            animatorMap[(int)DepthC2_GameObj.Probe_Anode].SetBool(TO_GROUNDING_TERMINAL, true);

            DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            
            

        }, Define.UIEvent.PointerDown);
       
        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count == 10)
            {
                if (!isAnodePut) return;
               
         
                
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(PROBE_TO_SCREWB, true);

                Action action = multimeterController.OnAllProbeSetOnResistanceMode;
                OnStepMissionComplete(animationNumber:10, delayTimeAmount: new WaitForSeconds(6f),
                    ActionBeforeDelay: action);
            }
        });

        GetObject((int)DepthC2_GameObj.TS_GroundingTerminalA).BindEvent(() =>
        {
          
        });


        GetObject((int)DepthC2_GameObj.TS_GroundingTerminalB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 3) return;
            if (Managers.ContentInfo.PlayData.Count != 11) return;
            if (!isAnodePut) return;
            if (!contentController.isStepMissionPerformable) return;
          
            Logger.Log("접지미션 수행 완료");
            contentController.isStepMissionPerformable = false;

            animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = true;
            animatorMap[(int)DepthC2_GameObj.Probe_Cathode].SetBool(TO_GROUNDING_TERMINAL, true);
            multimeterController.OnAllProbeSetToGroundingTerminal();
         
            OnStepMissionComplete(animationNumber:11, delayTimeAmount: new WaitForSeconds(6f));
        });
        
#endregion
 

       
        LateCommonInit();
    


    }
    
    protected virtual void OnResistanceReady()
    {
      
        
        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 9)
            OnStepMissionComplete(animationNumber:9, delayTimeAmount: new WaitForSeconds(1f));
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 14)
            OnStepMissionComplete(animationNumber:14, delayTimeAmount: new WaitForSeconds(1f));
    }
    
    protected virtual void OnUIBtnToolBoxTemperatureSensorClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 2 && Managers.ContentInfo.PlayData.Count == 5 )
        {
            OnStepMissionComplete( animationNumber:5,delayTimeAmount:new WaitForSeconds(12.9f));
            indicator.ShowTemperature(7.5f);
            return;
        }


        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 2)
        {
            OnStepMissionComplete( animationNumber:2,delayTimeAmount:new WaitForSeconds(3.8f));
        }
        
    }

    protected virtual void PowerOnOff(bool isOn)
    {
        if (Managers.ContentInfo.PlayData.Count == 3)
        {
            if(!isOn) OnStepMissionComplete(animationNumber:3);
        }
       
      
    
        if (Managers.ContentInfo.PlayData.Count == 13)
        {
            if(isOn) OnStepMissionComplete(animationNumber:13);
        }
    }



    protected  virtual void UnbindStaticEvents()
    {
        ControlPanelController.PowerOnOffActionWithBool -= PowerOnOff;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBox_TemperatureSensorClickedEvent -= OnUIBtnToolBoxTemperatureSensorClicked;
        UI_ToolBox.ToolBox_MultimeterClickedEvent -= OnUIToolBoxMultimeterBtnClicked;
        UI_ToolBox.ToolBox_ElectronicScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
        MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
        
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
        multimeterController = GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<MultimeterController>();
        
        currentScrewGaugeStatus = new Dictionary<int, float>();
        
        
        currentScrewGaugeStatus.TryAdd((int)DepthC2_GameObj.TS_InnerScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC2_GameObj.TS_InnerScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC2_GameObj.TS_InnerScrewC, 0);


        // State 중간에 초기화 하는경우를 위해 아래와같이 초기화 로직을 추가해줍니다. 
        isScrewWindMap = new Dictionary<int, bool>();

        isScrewWindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewA, false);
        isScrewWindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewB, false);
        isScrewWindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewC, false);


        isScrewUnwindMap = new Dictionary<int, bool>();

        isScrewUnwindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewA, false);
        isScrewUnwindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewB, false);
        isScrewUnwindMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewC, false);


        animatorMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewA,
            GetObject((int)DepthC2_GameObj.TS_InnerScrewA).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewB,
            GetObject((int)DepthC2_GameObj.TS_InnerScrewB).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC2_GameObj.TS_InnerScrewC,
            GetObject((int)DepthC2_GameObj.TS_InnerScrewC).GetComponent<Animator>());


        animatorMap.TryAdd((int)DepthC2_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC2_GameObj.ElectricScrewdriver).GetComponent<Animator>());

        animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = false;


        animatorMap.TryAdd((int)DepthC2_GameObj.Multimeter,
            GetObject((int)DepthC2_GameObj.Multimeter).GetComponent<Animator>());

     


        animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,
            GetObject((int)DepthC2_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,
            GetObject((int)DepthC2_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC2_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC2_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC2_GameObj.Probe_Cathode].enabled = false;

        animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련

        GetObject((int)DepthC2_GameObj.TS_InnerScrewA)
            .BindEvent(() =>
            {
               animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = true;
               animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC2_GameObj.TS_InnerScrewA);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC2_GameObj.TS_InnerScrewA);
                }
                
                animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_A : UNSCREW_A, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC2_GameObj.TS_InnerScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = true;
                animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC2_GameObj.TS_InnerScrewB);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC2_GameObj.TS_InnerScrewB);
                }
                
                animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_B : UNSCREW_B, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC2_GameObj.TS_InnerScrewC)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
                animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = true;
                
                if (isWindSession)
                {
                    UpdateDriverSliderWind((int)DepthC2_GameObj.TS_InnerScrewC);
                }
                else
                {
                    UpdateDriverSliderUnwind((int)DepthC2_GameObj.TS_InnerScrewC);
                }
                
                animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_C : UNSCREW_C, true);
            }, Define.UIEvent.Pressed);


        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Play(isWindSession? SCREW_A : UNSCREW_A, 0, 0);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Update(0);

           // animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_A : UNSCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Play(isWindSession? SCREW_B : UNSCREW_B, 0, 0);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Update(0);

            //animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_B : UNSCREW_B, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC2_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Play(isWindSession? SCREW_C : UNSCREW_C, 0, 0);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].Update(0);

         
         //   animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(isWindSession? SCREW_C : UNSCREW_C, true);
        }, Define.UIEvent.PointerDown);

        #endregion


        GetObject((int)DepthC2_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(UNSCREW_A, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(SCREW_A, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(UNSCREW_B, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(SCREW_B, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC2_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(UNSCREW_C, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].SetBool(SCREW_C, false);
            animatorMap[(int)DepthC2_GameObj.ElectricScrewdriver].enabled = false;
        });


        GetObject((int)DepthC2_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            // OnScrewClickUp();
            // _animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(ON, true);
            // _animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(ON, true);
        });

        InitializeTool();



    }

    protected virtual void OnToolBoxClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth3 == 1 &&Managers.ContentInfo.PlayData.Count == 11 )
        {
        Logger.Log("Toolbox Click event : 툴박스 클릭 이벤트 10 ------------------");
            OnStepMissionComplete(animationNumber: 11);
        }
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
                if (currentActiveTool == (int)DepthC2_GameObj.ElectricScrewdriver) Managers.Sound.Play(SoundManager.Sound.Effect, "Object/ElectronicDriver", 0.4f);
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


        if (objectHighlightMap[(int)DepthC2_GameObj.TS_InnerScrewC].ignore) return;
        if (currentActiveTool != (int)DepthC2_GameObj.ElectricScrewdriver)
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


        if (objectHighlightMap[(int)DepthC2_GameObj.TS_InnerScrewC].ignore) return;
        if (currentActiveTool != (int)DepthC2_GameObj.ElectricScrewdriver)
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

    
  

    private void OnMissionFinish()
    {
        SetHighlightIgnore((int)DepthC2_GameObj.TS_LockingScrew);
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
            animatorMap[(int)DepthC2_GameObj.Multimeter].SetBool(MULTIMITER_ON, isMultimeterOn);
            if (!_isMultimeterOn)
            {
                GetObject((int)DepthC2_GameObj.Probe_Cathode)?.SetActive(false);
                GetObject((int)DepthC2_GameObj.Probe_Anode)?.SetActive(false);
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
                currentActiveTool = (int)DepthC2_GameObj.ElectricScrewdriver;
            else
            {
                currentActiveTool = NO_TOOL_SELECTED;
                GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(false);
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


        if (isDriverOn && currentActiveTool == (int)DepthC2_GameObj.ElectricScrewdriver)
        {

            GetObject((int)DepthC2_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC2_GameObj.ElectricScrewdriver).transform.position = mousePosition;
        }
        else if (isMultimeterOn && currentActiveTool == (int)DepthC2_GameObj.Multimeter && multimeterController.isResistanceMode)
        {
            GetObject((int)DepthC2_GameObj.Probe_Cathode).SetActive(isMultimeterOn);
            GetObject((int)DepthC2_GameObj.Probe_Anode).SetActive(isMultimeterOn);
        
            if ((Managers.ContentInfo.PlayData.Count >= 13 && !isAnodePut) 
                ||(Managers.ContentInfo.PlayData.Depth3 ==3 && Managers.ContentInfo.PlayData.Count>=9 &&!isAnodePut))
            {
                GetObject((int)DepthC2_GameObj.Probe_Anode).transform.rotation =
                    defaultRotationMap[(int)DepthC2_GameObj.Probe_Anode];
                
                GetObject((int)DepthC2_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if ((Managers.ContentInfo.PlayData.Count >= 13 && isAnodePut)
                ||(Managers.ContentInfo.PlayData.Depth3 ==3 && Managers.ContentInfo.PlayData.Count>=9 &&isAnodePut))
            {
                GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation =
                    defaultRotationMap[(int)DepthC2_GameObj.Probe_Cathode];
                
                GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.position = mousePosition;
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
        
        
        
        
        currentActiveTool = (int)DepthC2_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) currentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC2_GameObj.ElectricScrewdriver), isDriverOn);

       
        
        
        
        Logger.Log($"is driver on? : {isDriverOn}");

        if (Managers.ContentInfo.PlayData.Count == 9 && Managers.ContentInfo.PlayData.Depth3 == 2)
        {
            OnStepMissionComplete(animationNumber:9);
        }
    }
    
    

    protected virtual void OnUIToolBoxMultimeterBtnClicked()
    {
        if (Managers.ContentInfo.PlayData.Depth2 != 2) return;
        
        
        InitializeTool();
     
        currentActiveTool = (int)DepthC2_GameObj.Multimeter;
        isMultimeterOn = !isMultimeterOn;

        if (isMultimeterOn == false) currentActiveTool = -1;


        
        // ToggleActiveState(GetObject((int)DepthC_GameObj.Multimeter), _isMultimeterOn);

        Logger.Log($"is Multimeter on? : {isMultimeterOn}");

        if (Managers.ContentInfo.PlayData.Depth3 == 3 && Managers.ContentInfo.PlayData.Count == 8)
        {
            OnStepMissionComplete(animationNumber:8);
        }
        
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 13)
        {
            OnStepMissionComplete(animationNumber:13);
        }
    }

    protected void InitializeTool()
    {

        currentActiveTool = -1;
        ToggleActiveState(GetObject((int)DepthC2_GameObj.ElectricScrewdriver), false);
        animatorMap[(int)DepthC2_GameObj.Multimeter].SetBool(PROBE_TO_SCREWB, false); // 멀티미터는 active상태로 유지합니다.
    }

    public void ClearTool()
    {
        currentActiveTool =  -1;
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
            BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewA);
            BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewB);
            BlinkHighlight((int)DepthC2_GameObj.TS_InnerScrewC);
        }
        
        SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewA, isUnscrewed);
        SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewB, isUnscrewed);
        SetHighlightIgnore((int)DepthC2_GameObj.TS_InnerScrewC, isUnscrewed);
        
        
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = true;
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = true;
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = true;
       
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].SetBool(DepthC2_SceneController.UNWIND,true);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].SetBool(DepthC2_SceneController.UNWIND,true);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].SetBool(DepthC2_SceneController.UNWIND,true);
       
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Play($"UnScrew", 0, isUnscrewed ? screwd : unscrewd);
       
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].Update(0);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].Update(0);
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].Update(0);
       
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].StopPlayback();
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].StopPlayback();
       animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].StopPlayback();
       
      animatorMap[(int)DepthC2_GameObj.TS_InnerScrewA].enabled = false;
      animatorMap[(int)DepthC2_GameObj.TS_InnerScrewB].enabled = false;
      animatorMap[(int)DepthC2_GameObj.TS_InnerScrewC].enabled = false;
    }


    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    
    /// <summary>
    /// 1. 공통적인 오브젝트 초기화로직 등 
    /// </summary>
    protected void PreCommonInit()
    {
         
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        _seqMap = new Dictionary<int, Sequence>();
        
        defaultRotationMap = new Dictionary<int, Quaternion>();
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Cathode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC2_GameObj.Probe_Anode,GetObject((int)DepthC2_GameObj.Probe_Cathode).transform.rotation);
        controlPanel = GetObject((int)DepthC2_GameObj.PowerHandle).GetComponent<ControlPanelController>();


        GetObject((int)DepthC2_GameObj.Wrench).SetActive(false);
    }

    private void InitializeC2States()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 3211, new DepthC21_State_1(this) },
            { 3212, new DepthC21_State_2(this) },
            { 3213, new DepthC21_State_3(this) },
            { 3214, new DepthC21_State_4(this) },
            { 3215, new DepthC21_State_5(this) },
            { 3216, new DepthC21_State_6(this) },
            { 3217, new DepthC21_State_7(this) },
            { 3218, new DepthC21_State_8(this) },
            { 3219, new DepthC21_State_9(this) },
            { 32110, new DepthC21_State_10(this) },
            { 32111, new DepthC21_State_11(this) },
            { 32112, new DepthC21_State_12(this) },
            
            
            { 32113, new DepthC21_State_13(this) },
            { 32114, new DepthC21_State_14(this) },
            { 32115, new DepthC21_State_15(this) },
            { 32116, new DepthC21_State_16(this) },
            { 32117, new DepthC21_State_17(this) },
            // { 32118, new DepthC21_State_18(this) },
            // { 32119, new DepthC21_State_19(this) },


            { 3221, new DepthC22_State_1(this) },
            { 3222, new DepthC22_State_2(this) },
            { 3223, new DepthC22_State_3(this) },
            { 3224, new DepthC22_State_4(this) },
            { 3225, new DepthC22_State_5(this) },
            { 3226, new DepthC22_State_6(this) },
            { 3227, new DepthC22_State_7(this) },
            { 3228, new DepthC22_State_8(this) },
            { 3229, new DepthC22_State_9(this) },
            { 32210, new DepthC22_State_10(this) },
            { 32211, new DepthC22_State_11(this) },
            { 32212, new DepthC22_State_12(this) },
            { 32213, new DepthC22_State_13(this) },
            // { 32214, new DepthC22_State_14(this) },
            // { 32215, new DepthC22_State_15(this) },
            // { 32216, new DepthC22_State_16(this) },
            // { 32217, new DepthC22_State_17(this) },
            // { 32218, new DepthC22_State_18(this) },
            // { 32219, new DepthC22_State_19(this) },
            
            
            { 3231, new DepthC23_State_1(this) },
            { 3232, new DepthC23_State_2(this) },
            { 3233, new DepthC23_State_3(this) },
            { 3234, new DepthC23_State_4(this) },
            { 3235, new DepthC23_State_5(this) },
            { 3236, new DepthC23_State_6(this) },
            { 3237, new DepthC23_State_7(this) },
            { 3238, new DepthC23_State_8(this) },
            { 3239, new DepthC23_State_9(this) },
            { 32310, new DepthC23_State_10(this) },
            { 32311, new DepthC23_State_11(this) },
            { 32312, new DepthC23_State_12(this) },
            { 32313, new DepthC23_State_13(this) },
            { 32314, new DepthC23_State_14(this) },
            { 32315, new DepthC23_State_15(this) },
            { 32316, new DepthC23_State_16(this) },
       
        };
    }
}
    
