using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///     주의사항
///     1.초기화 및 이벤트성 함수만 담도록 구성합니다.
///     2.상태에 따른 애니메이션 수행은 최대한 DepthCState에 구성합니다.
/// </summary>
public enum DepthC_GameObj
{
    TS_CompensatingWire,
    TS_Stabilizer,
    TS_SensingElement,
    TS_Cover,
    TS_LockingScrew,
    TS_ConnectionPiping,
    TS_InnerScrewA,
    TS_InnerScrewB,
    TS_InnerScrewC,

    //TS_InnerScrewD,
    //TS_InnerScrewE,
    TemperatureSensor,
    OnTempSensor_Pipe,
    ElectricScrewdriver,
    Multimeter,
    LimitSwitch,
    TemperatureSensor_Whole, //분해 로직이랑 구분
    LevelSensor,
    FlowMeter,

    PressureSensor
    //Parts
}


public class Depth1C_SceneController : Base_SceneController
{
    private Dictionary<int, float> _currentScrewGaugeStatus; // 나사 게이지 캐싱
    private Dictionary<int, Animator> _animatorMap;
    private Dictionary<int, bool> _isScrewUnwindMap;


    private readonly int UNWOUND_COUNT_GOAL = 3;
    private int _unwoundCount;

    private int unwoundCount
    {
        get => _unwoundCount;

        set
        {
            _unwoundCount = value;
            if (_unwoundCount >= UNWOUND_COUNT_GOAL)
            {
                Logger.Log($"모든 나사 풀림 (11) XXXXXXXleft screw(s) to unwind {UNWOUND_COUNT_GOAL -_unwoundCount}");
                OnStepMissionComplete(11);
            }
            
       
        }
    }
    
    
    
    [Range(-150f, 150f)] public float _toolPosXOffset = 0.3f;
    [Range(-150f, 150f)] public float _toolPosYOffset = -0.3f;

    private static readonly int UNWIND = Animator.StringToHash("Unwind");
    
    private static readonly int TO_SCREW_A = Animator.StringToHash("ScrewA");
    private static readonly int TO_SCREW_B = Animator.StringToHash("ScrewB");
    private static readonly int TO_SCREW_C = Animator.StringToHash("ScrewC");
    
    private static readonly int ON = Animator.StringToHash("On");
    

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Init()
    {
        base.Init();
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        _currentScrewGaugeStatus = new Dictionary<int, float>();
        _isScrewUnwindMap = new Dictionary<int, bool>();
        _animatorMap = new Dictionary<int, Animator>();

        #region 초기화 및 하이라이트 및 텍스트 바인딩 부분

        InitializeStates();

        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum();

        BindObject(typeof(DepthC_GameObj));

        GetObject((int)DepthC_GameObj.TemperatureSensor).SetActive(true);
        StartCoroutine(OnSceneStartCo());

        BindAndAddToDictionary((int)DepthC_GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_Cover, "커버");
        BindAndAddToDictionary((int)DepthC_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_LockingScrew, "고정나사 체결확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewA, "보상도선 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewB, "나사");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewC, "나사");
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewD, "나사");
        // BindAndAddToDictionary((int)DepthC_GameObj.TS_InnerScrewE, "나사");

        #endregion


        GetObject((int)DepthC_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            OnStepMissionComplete((int)DepthC_GameObj.TS_LockingScrew, 5);
        });

        GetObject((int)DepthC_GameObj.TS_ConnectionPiping).BindEvent(() =>
        {
            OnStepMissionComplete((int)DepthC_GameObj.TS_ConnectionPiping, 6);
        });

        GetObject((int)DepthC_GameObj.TS_Cover).BindEvent(() =>
        {
            OnStepMissionComplete((int)DepthC_GameObj.TS_Cover, 8);
        });

        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Count > 10) return; // ScrewA의 경우 중복애니메이션이 있음에 주의
            OnStepMissionComplete((int)DepthC_GameObj.TS_InnerScrewA, 9);
        });

        UI_ToolBox.ToolBoxOnEvent -= OnAllScrewUnWind;
        UI_ToolBox.ToolBoxOnEvent += OnAllScrewUnWind;

        // UI_ToolBox.ScrewDriverClickedEvent -= () =>
        // {
        //     //OnStepMissionComplete(11);
        // };
        // UI_ToolBox.ScrewDriverClickedEvent += () =>
        // {
        //     //OnStepMissionComplete(11);
        // };


        SetScrewDriveSection();
    }


    protected void OnDestroy()
    {
        UI_ToolBox.ToolBoxOnEvent -= OnAllScrewUnWind;
    }
    /// <summary>
    /// 드라이버로 나사를 푸는 경우의 State에 사용됩니다.
    /// 적절한 초기화 작업이 필요합니다(10/14/24)
    /// </summary>
    private void SetScrewDriveSection()
    {
        _currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewA, 0);
        _currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewB, 0);
        _currentScrewGaugeStatus.TryAdd((int)DepthC_GameObj.TS_InnerScrewC, 0);

        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewA, false);
        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewB, false);
        _isScrewUnwindMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewC, false);

        _animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewA,
            GetObject((int)DepthC_GameObj.TS_InnerScrewA).GetComponent<Animator>());
        _animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewB,
            GetObject((int)DepthC_GameObj.TS_InnerScrewB).GetComponent<Animator>());
        _animatorMap.TryAdd((int)DepthC_GameObj.TS_InnerScrewC,
            GetObject((int)DepthC_GameObj.TS_InnerScrewC).GetComponent<Animator>());
        
        
        _animatorMap.TryAdd((int)DepthC_GameObj.ElectricScrewdriver,
            GetObject((int)DepthC_GameObj.ElectricScrewdriver).GetComponent<Animator>());
        
        _animatorMap.TryAdd((int)DepthC_GameObj.Multimeter,
            GetObject((int)DepthC_GameObj.Multimeter).GetComponent<Animator>());

        _animatorMap[(int)DepthC_GameObj.Multimeter].enabled = true;
        _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        
        
        GetObject((int)DepthC_GameObj.TS_InnerScrewA)
            .BindEvent(() =>
            {
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewA);
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC_GameObj.TS_InnerScrewB)
            .BindEvent(() =>
            {
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewB); 
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
            }, Define.UIEvent.Pressed);

        GetObject((int)DepthC_GameObj.TS_InnerScrewC)
            .BindEvent(() =>
            {
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
                UpdateDriverSlider((int)DepthC_GameObj.TS_InnerScrewC);
                _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
            }, Define.UIEvent.Pressed);


        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickDown();
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickDown(); 
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, true);
        }, Define.UIEvent.PointerDown);

        GetObject((int)DepthC_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickDown();
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, true);
        }, Define.UIEvent.PointerDown);


        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            OnScrewClickUp();
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_A, false);
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        });

        GetObject((int)DepthC_GameObj.TS_InnerScrewB).BindEvent(() =>
        {
            OnScrewClickUp();
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_B, false);
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = true;
        });

        GetObject((int)DepthC_GameObj.TS_InnerScrewC).BindEvent(() =>
        {
            OnScrewClickUp();
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].SetBool(TO_SCREW_C, false);
            _animatorMap[(int)DepthC_GameObj.ElectricScrewdriver].enabled = false;
        });
        
       
        
        InitializeTool();

     

    }

    private void OnAllScrewUnWind()
    {
        OnStepMissionComplete(10);
    }
    private void OnStepMissionComplete(int objectEnumToInt = -1, int animationNumber = -1)
    {
        if (objectEnumToInt != -1 && objectHighlightMap.ContainsKey(objectEnumToInt) && objectHighlightMap[objectEnumToInt].ignore)
        {
            Logger.Log("클릭불가 상태 ,오브젝트가 없거나 하이라이트 ignore 상태입니다.");
         
        }

        StartCoroutine(OnStepMissionCompleteCo(animationNumber));
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
     
        _pressedTime += Time.fixedDeltaTime;
        if (_gaugeDelay > _pressedTime) return;
        
        
        if (objectHighlightMap[(int)DepthC_GameObj.TS_InnerScrewC].ignore) return;
        if (CurrentActiveTool != (int)DepthC_GameObj.ElectricScrewdriver)
        {
            Logger.Log("inadequate tool selected. XXXXXX");
            return;
        }


        contentController.UI_DrverOnly_GaugeSlider.value = _currentScrewGaugeStatus[screwID];
        contentController.UI_DrverOnly_GaugeSlider.value += 0.02f;
        _currentScrewGaugeStatus[screwID] = contentController.UI_DrverOnly_GaugeSlider.value;

        if (contentController.UI_DrverOnly_GaugeSlider.value > 0.92f && !_isScrewUnwindMap[screwID])
        {
            _isScrewUnwindMap[screwID] = true;
            _animatorMap[screwID].SetBool(UNWIND, true);
            unwoundCount++;
        }
    }


    private WaitForSeconds _waitBeforeNextStep;
    private readonly float _waitBeforeNextStepSeconds = 2;

    private IEnumerator OnStepMissionCompleteCo(int currentStepNum)
    {
        if (Managers.ContentInfo.PlayData.Count != currentStepNum)
            Debug.LogWarning("현재 애니메이션 재생과 카운트 불일치.. 다른 애니메이션이거나 여러 곳 사용되는 애니메이션일 수 있습니다.");
        if (contentController.isStepMissionComplete)
        {
            Logger.Log("이미 수행함. 중복실행 X XXXXXXX");
            yield break;
        }

        contentController.isStepMissionComplete = true;
        if (_waitBeforeNextStep == null) _waitBeforeNextStep = new WaitForSeconds(_waitBeforeNextStepSeconds);


        PlayAnimationAndNarration(currentStepNum, isServeAnim: true);

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



    private void SetToolPos()
    {
        var distanceFromCamera = 0.09f;
     

        if (isDriverOn && CurrentActiveTool == (int)DepthC_GameObj.ElectricScrewdriver)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + _toolPosXOffset,
                Input.mousePosition.y + _toolPosYOffset,
                distanceFromCamera));
            GetObject((int)DepthC_GameObj.ElectricScrewdriver).transform.position = mousePosition;
           Logger.Log($"On_CurrentPos: {mousePosition}");
        }
        else if (_isMultimeterOn && CurrentActiveTool == (int)DepthC_GameObj.Multimeter)
        {
            
        
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
        
        if (_isMultimeterOn ==false) CurrentActiveTool = -1;
        
        
        _animatorMap[(int)DepthC_GameObj.Multimeter].SetBool(ON, _isMultimeterOn);
       // ToggleActiveState(GetObject((int)DepthC_GameObj.Multimeter), _isMultimeterOn);
        
        Logger.Log($"is Multimeter on? : {_isMultimeterOn}");
    }

    private void InitializeTool()
    {
         
        ToggleActiveState(GetObject((int)DepthC_GameObj.ElectricScrewdriver), false);
        _animatorMap[(int)DepthC_GameObj.Multimeter].SetBool(ON, false); // 멀티미터는 active상태로 유지합니다.
    }


    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }


    private void InitializeStates()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 3211, new Depth1C21_State_1(this) },
            { 3212, new Depth1C21_State_2(this) },
            { 3213, new Depth1C21_State_3(this) },
            { 3214, new Depth1C21_State_4(this) },
            { 3215, new Depth1C21_State_5(this) },
            { 3216, new Depth1C21_State_6(this) },
            { 3217, new Depth1C21_State_7(this) },
            { 3218, new Depth1C21_State_8(this) },
            { 3219, new Depth1C21_State_9(this) },
            { 32110, new Depth1C21_State_10(this) },
            { 32111, new Depth1C21_State_11(this) },
            { 32112, new Depth1C21_State_12(this) },
            { 32113, new Depth1C21_State_13(this) },
            { 32114, new Depth1C21_State_14(this) },
            { 32115, new Depth1C21_State_15(this) },
            { 32116, new Depth1C21_State_16(this) },
            { 32117, new Depth1C21_State_17(this) },
            { 32118, new Depth1C21_State_18(this) },
            { 32119, new Depth1C21_State_19(this) },
            { 32120, new Depth1C21_State_20(this) },
            { 32121, new Depth1C21_State_16(this) },
            { 32122, new Depth1C21_State_17(this) },
            { 32123, new Depth1C21_State_18(this) },
            { 32124, new Depth1C21_State_19(this) },
            { 32125, new Depth1C21_State_20(this) }
        };
    }
}