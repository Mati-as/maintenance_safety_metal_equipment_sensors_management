using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
///     주의사항
///     1.초기화 및 이벤트성 함수만 담도록 구성합니다.
///     2.상태에 따른 애니메이션 수행은 최대한 DepthCState에 구성합니다.
/// </summary>
public enum DepthC_GameObj
{
    // Common------------------
    ElectricScrewdriver,
    Multimeter,
    Probe_Anode, // negative
    Probe_Cathode, // positive
    
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

    
        
    
    
    LimitSwitch,
    TemperatureSensor_Whole, //분해 로직이랑 구분
    LevelSensor,
    FlowMeter,

    PressureSensor
    //Parts
}


public class Depth1C_SceneController : Base_SceneController
{
    [FormerlySerializedAs("_currentScrewGaugeStatus")]
    public Dictionary<int, float> currentScrewGaugeStatus; // 나사 게이지 캐싱

    public Dictionary<int, Animator> animatorMap;
    public Dictionary<int, bool> _isScrewUnwindMap;


    private readonly int UNWOUND_COUNT_GOAL = 3;
    private int _unwoundCount;


    public bool isAnodePut; // 음극단자 설정을 위한 bool값입니다.
    private WaitForSeconds _waitBeforeNextStep;
    private readonly float _waitBeforeNextStepSeconds = 2;

    private int unwoundCount
    {
        get => _unwoundCount;

        set
        {
            _unwoundCount = value;
            if (_unwoundCount >= UNWOUND_COUNT_GOAL)
            {
                Logger.Log($"모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL - _unwoundCount}");
                OnStepMissionComplete(11);
            }


        }
    }



    [Range(-150f, 150f)] public float _toolPosXOffset = 0.3f;
    [Range(-150f, 150f)] public float _toolPosYOffset = -0.3f;

    public static readonly int UNWIND = Animator.StringToHash("Unwind");

    public static readonly int TO_SCREW_A = Animator.StringToHash("ScrewA");
    public static readonly int TO_SCREW_B = Animator.StringToHash("ScrewB");
    public static readonly int TO_SCREW_C = Animator.StringToHash("ScrewC");

    public static readonly int PROBE_TO_SCREWB = Animator.StringToHash("On");

    public static readonly int TO_GROUNDING_TERMINAL = Animator.StringToHash("GroundingTerminal");

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum(); //개발용
        base.Init();
        
        InitializeC2States();
       
    }

    public void DepthC21Init()
    {
        #region 3.2.1 온도센서 점검

        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;


     

        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        _isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();

        #region 초기화 및 하이라이트 및 텍스트 바인딩 부분




        BindObject(typeof(DepthC_GameObj));

        GetObject((int)DepthC_GameObj.TemperatureSensor).SetActive(true);
        StartCoroutine(OnSceneStartCo());

        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Cover, "커버");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_LockingScrew, "고정나사 체결확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "나사");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "보상도선 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewC, "나사");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalA, "A 접지");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "B 접지");

        GetObject((int)DepthC_GameObj.Probe_Cathode).SetActive(false);
        GetObject((int)DepthC_GameObj.Probe_Anode).SetActive(false);
        GetObject((int)DepthC_GameObj.ElectricScrewdriver).SetActive(false);
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewD, "나사");
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewE, "나사");

        #endregion


        GetObject((int)DepthC_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            OnStepMissionComplete((int)DepthC_GameObj.TS_LockingScrew, 5);
        });

        GetObject((int)DepthC_GameObj.TS_ConnectionPiping).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            OnStepMissionComplete((int)DepthC_GameObj.TS_ConnectionPiping, 6);
        });

        GetObject((int)DepthC_GameObj.TS_Cover).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            OnStepMissionComplete((int)DepthC_GameObj.TS_Cover, 8);
        });


        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            if (Managers.ContentInfo.PlayData.Count > 10) return; // ScrewA의 경우 중복애니메이션이 있음에 주의
            OnStepMissionComplete((int)DepthC_GameObj.TS_InnerScrewA, 9);
        });


        //드라이버 -------------------------------------------------------------------------  
        SetScrewDriveSection();





        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            
            if (Managers.ContentInfo.PlayData.Count == 13)
            {
                animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);

                DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }

            if (Managers.ContentInfo.PlayData.Count == 13)
            {
                if (!isAnodePut) return;

                animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
                animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(PROBE_TO_SCREWB, true);

                Action action = multimeterController.OnAllProbeSet;
                OnStepMissionComplete(13, delayAmount: new WaitForSeconds(6f),
                    delayedAction: action);
            }


        }, Define.UIEvent.PointerDown);


        GetObject((int)DepthC_GameObj.TS_GroundingTerminalA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
            animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(TO_GROUNDING_TERMINAL, true);

            DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
        });


        GetObject((int)DepthC_GameObj.TS_GroundingTerminalB).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 != 1) return;
            
            if (!isAnodePut) return;
            if (contentController.isStepMissionPerformable) return;
            if (Managers.ContentInfo.PlayData.Count != 14) return;

            Logger.Log("접지미션 수행 완료");

            contentController.isStepMissionPerformable = true;

            animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = true;
            animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(TO_GROUNDING_TERMINAL, true);
            multimeterController.OnAllProbeSetToGroundingTerminal();
         
            OnStepMissionComplete(14, delayAmount: new WaitForSeconds(6f));
        });

        #endregion

    }
    public void DepthC22Init()
    {
        #region 3.2.2 온도센서 고장 유형
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_LockingScrew, "온도센서 탈거");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TemperatureSensor, "온도센서 수거");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Cover, "덮개 열기");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_SensingElement, "변형된 감온부");

        GetObject((int)DepthC_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            Debug.Assert(Managers.ContentInfo.PlayData.Depth3 == 2,
                $"Depth3 is {Managers.ContentInfo.PlayData.Depth3} but must be 2)");
            if (Managers.ContentInfo.PlayData.Count != 4) return;

            OnStepMissionComplete( animationNumber:4,delayAmount:new WaitForSeconds(5f));
            SetHighlightIgnore((int)DepthC_GameObj.TS_SensingElement, false);
            
        });
        
        GetObject((int)DepthC_GameObj.TemperatureSensor).BindEvent(() =>
        {
            Debug.Assert(Managers.ContentInfo.PlayData.Depth3 == 2);
            if (Managers.ContentInfo.PlayData.Count != 5) return;
            
            OnStepMissionComplete( animationNumber:5,delayAmount:new WaitForSeconds(5f));
            SetHighlightIgnore((int)DepthC_GameObj.TS_SensingElement);
            
        });
        
        #endregion
    }
    
    public void DepthC23Init()
    {
        #region 3.2.3 온도 센서 정비
    
        #endregion
    }
    
    protected void OnDestroy()
    {

        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
    }

    /// <summary>
    /// 드라이버로 나사를 푸는 경우의 State에 사용됩니다.
    /// 자원 할당 이외 직접적인 사용은 최대한 Depth별 State에서 사용하도록 합니다. 
    /// 적절한 초기화 작업이 필요합니다(10/14/24)
    /// </summary>
    private void SetScrewDriveSection()
    {
        currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewA, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewB, 0);
        currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewC, 0);


        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewA, false);
        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewB, false);
        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewC, false);


        animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewA,
            GetObject((int)DepthC_GameObj.TS_InnerScrewA).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewB,
            GetObject((int)DepthC_GameObj.TS_InnerScrewB).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewC,
            GetObject((int)DepthC_GameObj.TS_InnerScrewC).GetComponent<Animator>());


        animatorMap.TryAdd((int)DepthC_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC_GameObj.ElectricScrewdriver).GetComponent<Animator>());

        animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;


        animatorMap.TryAdd((int)DepthC_GameObj.Multimeter,
            GetObject((int)DepthC_GameObj.Multimeter).GetComponent<Animator>());

        multimeterController = GetObject((int)DepthC_GameObj.Multimeter).GetComponent<MultimeterController>();


        animatorMap.TryAdd((int)DepthC_GameObj.Probe_Anode,
            GetObject((int)DepthC_GameObj.Probe_Anode).GetComponent<Animator>());
        animatorMap.TryAdd((int)DepthC_GameObj.Probe_Cathode,
            GetObject((int)DepthC_GameObj.Probe_Cathode).GetComponent<Animator>());

        animatorMap[(int)DepthC_GameObj.Multimeter].enabled = true;
        animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = false;
        animatorMap[(int)DepthC_GameObj.Probe_Cathode].enabled = false;

        animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;

        #region 나사 풀기 애니메이션관련

        GetObject((int)DepthC_GameObj.TS_InnerScrewA)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;

                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewA);
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC_GameObj.TS_InnerScrewB)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewB);
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC_GameObj.TS_InnerScrewC)
            .BindEvent(() =>
            {
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewC);
                animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
            }, Define.UIEvent.Pressed);


        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Play(TO_SCREW_A, 0, 0);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Update(0);

            animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].SetBool(UNWIND, true);

            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Play(TO_SCREW_B, 0, 0);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Update(0);

            animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].SetBool(UNWIND, true);

            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickDown();
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Play(TO_SCREW_C, 0, 0);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].Update(0);

            animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].SetBool(UNWIND, true);

            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
        }, Define.UIEvent.PointerDown);

        #endregion


        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC_GameObj.TS_InnerScrewA].enabled = false;
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, false);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC_GameObj.TS_InnerScrewB].enabled = false;
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, false);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickUp();
            animatorMap[(int)DepthC_GameObj.TS_InnerScrewC].enabled = false;
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, false);
            animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        });



        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            // OnScrewClickUp();
            // _animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(ON, true);
            // _animatorMap[(int)DepthC_GameObj.Probe_Cathode].SetBool(ON, true);
        });

        InitializeTool();



    }

    private void OnToolBoxClicked()
    {
        Logger.Log("Toolbox Click event : 툴박스 클릭 이벤트 10 ------------------");
        OnStepMissionComplete(animationNumber: 10);
    }

    private void OnStepMissionComplete(int objectEnumToInt = -1, int animationNumber = -123456789,
        WaitForSeconds delayAmount = null, Action delayedAction = null)
    {
        if (objectEnumToInt != -1 && objectHighlightMap.ContainsKey(objectEnumToInt) &&
            objectHighlightMap[objectEnumToInt].ignore)
        {
            Logger.Log("클릭불가 상태 ,오브젝트가 없거나 하이라이트 ignore 상태입니다.");

        }

        StartCoroutine(OnStepMissionCompleteCo(animationNumber, delayAmount, delayedAction));
    }


    private float _gaugeDelay = 0.8f;
    private float _pressedTime;

    private void OnScrewClickDown()
    {
        contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(true);
    }

    private void OnScrewClickUp()
    {
        contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
        _pressedTime = 0;
    }

    private void UpdateDriverSlider(int screwID)
    {
        if (Managers.ContentInfo.PlayData.Count != 11)
        {
            contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);
            return;
        }


        contentController.UI_DrverOnly_GaugeSlider.gameObject.SetActive(true);

        _pressedTime += Time.fixedDeltaTime;

        //잔량 선제 표시
        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];


        if (_gaugeDelay > _pressedTime) return;


        if (objectHighlightMap[(int)DepthC_GameObj.TS_InnerScrewC].ignore) return;
        if (CurrentActiveTool != (int)DepthC_GameObj.ElectricScrewdriver)
        {
            Logger.Log("inadequate tool selected. XXXXXX");
            return;
        }


        contentController.UI_DrverOnly_GaugeSlider.value = currentScrewGaugeStatus[screwID];
        contentController.UI_DrverOnly_GaugeSlider.value += 0.02f;
        currentScrewGaugeStatus[screwID] = contentController.UI_DrverOnly_GaugeSlider.value;

        if (!_isScrewUnwindMap[screwID])
        {

            var playbackAmount = contentController.UI_DrverOnly_GaugeSlider.value;
            animatorMap[screwID].Play("UnScrew", 0, playbackAmount);
            animatorMap[screwID].Update(playbackAmount);

            if (contentController.UI_DrverOnly_GaugeSlider.value > 0.95f)
            {
                animatorMap[screwID].Play("UnScrew", 0, 1);
                _isScrewUnwindMap[screwID] = true;
                unwoundCount++;
            }
        }
    }



    private IEnumerator OnStepMissionCompleteCo(int currentStepNum, WaitForSeconds waitForSeconds = null,
        Action ActionBeforeNextStep = null)
    {
        
        PlayAnimationAndNarration(currentStepNum, isServeAnim: true);
        ActionBeforeNextStep?.Invoke();
        
        
        if (Managers.ContentInfo.PlayData.Count != currentStepNum)
            Debug.LogWarning("현재 애니메이션 재생과 카운트 불일치.. 다른 애니메이션이거나 여러 곳 사용되는 애니메이션일 수 있습니다.");
       
        if (contentController.isStepMissionComplete)
        {
            Logger.Log("이미 수행함. 중복실행 X XXXXXXX");
            yield break;
        }


        contentController.isStepMissionComplete = true;
        if (waitForSeconds != null)
        {
            _waitBeforeNextStep = waitForSeconds;
        }

        if (_waitBeforeNextStep == null)
        {
            _waitBeforeNextStep = new WaitForSeconds(_waitBeforeNextStepSeconds);
        }


     

        

        OnMissionFinish(); //사운드 재생 등 성공처리

        yield return _waitBeforeNextStep;



        Logger.Log($"작업 수행을 통한 다음 이벤트 재생 :--------------- {Managers.ContentInfo.PlayData.Count}-");
        contentController.InvokeNextStep(); // 다음 스텝으로 넘어가기
        yield return _waitBeforeNextStep;
        contentController.isStepMissionComplete = false;
    }

    private void OnMissionFinish()
    {
        SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew);
    }

    private bool _isDriverOn;
    private bool _isMultimeterOn;

    public bool isDriverOn
    {
        get => _isDriverOn;

        set
        {
            _isDriverOn = value;

            if (_isDriverOn)
                CurrentActiveTool = (int)DepthC_GameObj.ElectricScrewdriver;
            else
                CurrentActiveTool = NO_TOOL_SELECTED;
        }
    }


    private void Update()
    {
        SetToolPos();
    }


    [FormerlySerializedAs("_multimeterController")]
    public MultimeterController multimeterController;

    private void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
            Input.mousePosition.y + _toolPosYOffset,
            distanceFromCamera));


        if (isDriverOn && CurrentActiveTool == (int)DepthC_GameObj.ElectricScrewdriver)
        {

            GetObject((int)DepthC_GameObj.ElectricScrewdriver).SetActive(isDriverOn);
            GetObject((int)DepthC_GameObj.ElectricScrewdriver).transform.position = mousePosition;
            //Logger.Log($"On_CurrentPos: {mousePosition}");
        }
        else if (_isMultimeterOn && CurrentActiveTool == (int)DepthC_GameObj.Multimeter)
        {
            GetObject((int)DepthC_GameObj.Probe_Cathode).SetActive(_isMultimeterOn);
            GetObject((int)DepthC_GameObj.Probe_Anode).SetActive(_isMultimeterOn);

            if (!GetObject((int)DepthC_GameObj.Multimeter).GetComponent<MultimeterController>().isResistanceMode)
            {

                //초기화작업 진행필요
                return;
            }

            if (Managers.ContentInfo.PlayData.Count >= 13 && !isAnodePut)
            {
                GetObject((int)DepthC_GameObj.Probe_Anode).transform.position = mousePosition;
            }

            if (Managers.ContentInfo.PlayData.Count >= 13 && isAnodePut)
            {
                GetObject((int)DepthC_GameObj.Probe_Cathode).transform.position = mousePosition;
            }

        }





    }

    private void ToggleActiveState(GameObject obj, bool isOn)
    {
        obj.SetActive(isOn);
    }

    public void OnScrewDriverBtnClicked()
    {
        InitializeTool();

        CurrentActiveTool = (int)DepthC_GameObj.ElectricScrewdriver;

        isDriverOn = !isDriverOn;
        if (isDriverOn == false) CurrentActiveTool = -1;

        ToggleActiveState(GetObject((int)DepthC_GameObj.ElectricScrewdriver), isDriverOn);

        Logger.Log($"is driver on? : {isDriverOn}");
    }

    public void OnMultimeterBtnClicked()
    {
        InitializeTool();


        CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
        _isMultimeterOn = !_isMultimeterOn;

        if (_isMultimeterOn == false) CurrentActiveTool = -1;


        animatorMap[(int)DepthC_GameObj.Multimeter].SetBool(PROBE_TO_SCREWB, _isMultimeterOn);
        // ToggleActiveState(GetObject((int)DepthC_GameObj.Multimeter), _isMultimeterOn);

        Logger.Log($"is Multimeter on? : {_isMultimeterOn}");
    }

    private void InitializeTool()
    {

        ToggleActiveState(GetObject((int)DepthC_GameObj.ElectricScrewdriver), false);

        animatorMap[(int)DepthC_GameObj.Multimeter].SetBool(PROBE_TO_SCREWB, false); // 멀티미터는 active상태로 유지합니다.
    }


    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
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
            { 32118, new DepthC21_State_18(this) },
            { 32119, new DepthC21_State_19(this) },


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
            { 32214, new DepthC22_State_14(this) },
            { 32215, new DepthC22_State_15(this) },
            { 32216, new DepthC22_State_16(this) },
            { 32217, new DepthC22_State_17(this) },
            { 32218, new DepthC22_State_18(this) },
            { 32219, new DepthC22_State_19(this) },
            
            
            { 3321, new DepthC23_State_1(this) },
            { 3322, new DepthC23_State_2(this) },
            { 3323, new DepthC23_State_3(this) },
            { 3324, new DepthC23_State_4(this) },
            { 3325, new DepthC23_State_5(this) },
            { 3326, new DepthC23_State_6(this) },
            { 3327, new DepthC23_State_7(this) },
            { 3328, new DepthC23_State_8(this) },
            { 3329, new DepthC23_State_9(this) },
            { 33210, new DepthC23_State_10(this) },
            { 33211, new DepthC23_State_11(this) },
            { 33212, new DepthC23_State_12(this) },
            { 33213, new DepthC23_State_13(this) },
            { 33214, new DepthC23_State_14(this) },
            { 33215, new DepthC23_State_15(this) },
            { 33216, new DepthC23_State_16(this) },
            { 33217, new DepthC23_State_17(this) },
            { 33218, new DepthC23_State_18(this) },
            { 33219, new DepthC23_State_19(this) },
        };
    }
}
    
