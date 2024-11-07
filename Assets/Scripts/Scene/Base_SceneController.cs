using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HighlightPlus;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Base_SceneController : MonoBehaviour, ISceneController
{
   
    private WaitForSeconds _waitBeforeNextStep;
    private readonly float _waitBeforeNextStepSeconds = 2;
    public int CurrentActiveTool;
    protected readonly int NO_TOOL_SELECTED = -1;
 
    [Tooltip("Camera and Effect Setting-----------------------")]
    public Inplay_CameraController cameraController { get; protected set; }

    public UI_ContentController contentController;
    private readonly float _startDelay = 1f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;
    
    private int _currentStateNum;

    //Animation Part

    private Animation _mainAnimation; // GameOjbect의 최상 부모위치에 있어야함.
  
    [Tooltip("StateMachine  Setting ----------------------")]
    // SceneState Control
    protected Dictionary<int, ISceneState> _sceneStates;
    protected ISceneState _currentState;
    public int currentCount { get; private set; }


    //Animation
    public Dictionary<int, HighlightEffect> objectHighlightMap;
    private Dictionary<int, Sequence> _seqMap;

    [Tooltip("Highlight Effect Setting ----------------------")]

    private readonly float OUTLINE_WIDTH_ON_HOVER =1;
    public virtual void Start()
    {
        Init();
    }

    /// <summary>
    ///     기본적인 자원로드 위주로 구성하며, override 한 객체가 UI,플레이등을 실행하도록 합니다.
    /// </summary>
    public virtual void Init()
    {
        BindEvent();


        SetMainProperties();

    }
    
    

    public void SetMainProperties()
    {
        _mainAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();
        
        if(Managers.UI.SceneUI ==null) Managers.UI.ShowSceneUI<UI_Persistent>();
        
        objectHighlightMap = new Dictionary<int, HighlightEffect>();
        _seqMap = new Dictionary<int, Sequence>();

        Debug.Assert(Camera.main != null);
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();

        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
       
        Managers.UI.CloseAllPopupUI();
        
        contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        contentController.Init(); // sceneController에서 제어하는 부분이 있으므로 먼저 초기화 수행 
    }

    private void BindEvent()
    {
        UI_ContentController.OnDepth3ClickedAction -= OnDepth3IntroOrClickedAction;
        UI_ContentController.OnDepth3ClickedAction += OnDepth3IntroOrClickedAction;
        
        UI_ContentController.OnDepth2ClickedAction -= OnDepth2IntroOrClickedAction;
        UI_ContentController.OnDepth2ClickedAction += OnDepth2IntroOrClickedAction;
        
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChange;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepChange;
    }

    protected virtual void OnDestroy()
    {
        UI_ContentController.OnDepth2ClickedAction -= OnDepth2IntroOrClickedAction;
        UI_ContentController.OnDepth3ClickedAction -= OnDepth3IntroOrClickedAction;
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChange;
    }
    private void OnDepth3IntroOrClickedAction()
    {
        PreInitBefreDepthChange();
        
        PlayAnimationAndNarration(1);
        ChangeState(1);
    }

    private void PreInitBefreDepthChange()
    {
        isReverseAnim = false;
        ExitCurrentState();

    }

    private void OnDepth2IntroOrClickedAction()
    {
        PreInitBefreDepthChange();
    }
    protected IEnumerator OnSceneStartCo()
    {
        Logger.Log("Initial UI Intro");
        
        //PlayInitialIntro();
        
      //  contentController.SetInstructionShowOrHideStatus(false);
        
        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;

        //contentController.SetShowOrHideInstruction();

        
        var introAnimation = 0;
        PlayAnimationAndNarration(introAnimation);
        Managers.Sound.Play(SoundManager.Sound.Effect, "Depth1Start");
    }


    /// <summary>
    ///     씬로드시 처음만 재생됩니다.
    /// </summary>
    public virtual void PlayInitialIntro()
    {
        Logger.Log("Initial UI Intro");
        contentController.Init();
       // contentController.ShowInitialIntro();
        contentController.SetInstructionShowOrHideStatus(false);
    }

    public bool isReverseAnim { get; private set; }

    public virtual void OnStepChange(int count, bool isReverse = false)
    {
        //재생관련 파라미터를 미리 설정 한 후 재생합니다. (재생순서, 방식 등)
        isReverseAnim = isReverse;
        currentCount = count;


        
        Logger.Log($"State 변경. 현재State----------> {count}");
        ChangeState(currentCount);
    }

    /// <summary>
    /// 각 State마지막에서의 초기화 및 버튼에서 사용됩니다.
    /// 결합도 이슈로 최대한 다른 참조로 사용하지 않는 것을 권장합니다. 
    /// </summary>
    /// <param name="stateNum"></param>
    protected void ChangeState(int stateNum)
    {
        var processedState = int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                             $"{Managers.ContentInfo.PlayData.Depth2}"+
                             $"{Managers.ContentInfo.PlayData.Depth3}"+
                             $"{stateNum.ToString()}");


        _currentStateNum = processedState;
        Logger.Log($"Current StateNum : {processedState}");
        
        if (_sceneStates.TryGetValue(processedState, out var newState))
        {
            if(_currentState==null) Logger.Log($"현재 State 없음, 에러가능성 있습니다 --------------- : {processedState}");
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter();
        }
        else
        {
            Logger.LogWarning($"No state found for depth {stateNum}----------------------------------");
        }
    }

    private void ExitCurrentState()
    {

        if (_sceneStates.TryGetValue(_currentStateNum, out var newState))
        {
            if(_currentState==null) Logger.Log($"현재 State 없음, 에러가능성 있습니다 --------------- : {_currentStateNum}");
            _currentState?.OnExit();
        }
        else
        {
            Logger.LogWarning($"No state found for depth {_currentStateNum}----------------------------------");
        }
    }



    #region Call By States

    
    /// <summary>
    /// 애니메이션을 재생합니다.
    /// isServeAnimation인 경우 서브애니메이션을 재생합니다. 파일형식은 예를들어 5A,5B 가됩니다.
    /// 해당스크립트의 메인 에니메이션은 알파벳없이 숫자로만 구성됩니다.
    /// </summary>
    /// <param name="count"></param>
    /// <param name="delay"></param>
    /// <param name="isReverse"></param>
    /// <param name="isServeAnim"></param>
    ///
    
    public float currentCilpLength { get; private set; }
    public void PlayAnimationAndNarration(int count, float delay = 0f, bool isReverse = false,bool isServeAnim =false)
    {
      
       


      // 중복애니메이션 클립할당을 위해 추가합니다. 추후 성능이슈가 발생하는경우 로직 수정 필요합니다. 10/17/24
         Debug.Assert(_mainAnimation != null, "Animation component can't be null");

       
        
        var path = $"Animation/{Managers.ContentInfo.PlayData.Depth1}" +
                   $"{Managers.ContentInfo.PlayData.Depth2}" +
                   $"{Managers.ContentInfo.PlayData.Depth3}" + $"/{count}";

        if (isServeAnim)
        {
            path += 'A';
        }
       
        Logger.Log($"Animation Path {path}");
        var clip = Resources.Load<AnimationClip>(path);
        
        
        Debug.Assert(_mainAnimation != null, "Animation component can't be null");
        
        
        
        if (clip == null)
        {
            Logger.LogWarning($"Animation clip at path {path} not found.");
            OnAnimationComplete();
            return;
        }
      
        var existedClip = _mainAnimation.GetClip(clip.name);
        if (existedClip == null)
        {
            _mainAnimation.AddClip(clip, clip.name);
            Logger.Log($"Added animation clip {clip.name} to _mainAnimation.");
        }
        else 
        {
            _mainAnimation.RemoveClip(clip.name);
            _mainAnimation.AddClip(clip, clip.name);
            Logger.Log($"Replaced existing animation clip {clip.name}.");
        }
        
        
        
        

        if (isReverse)
        {
            path = $"Animation/{Managers.ContentInfo.PlayData.Depth1}" +
                   $"{Managers.ContentInfo.PlayData.Depth2}" +
                   $"{Managers.ContentInfo.PlayData.Depth3}" + $"/{count + 1}";
            
            Logger.Log($"Reverse Animation Path {path}");
           
            var reverseClip = Resources.Load<AnimationClip>(path);

            if (reverseClip != null && reverseClip.length >= 1.5f) // 1frame 이상인 경우
            {
                clip = reverseClip;
                _mainAnimation[clip.name].time = _mainAnimation[clip.name].length;
                Logger.Log($"Current Anim was long enough: {reverseClip.length} ->  Rewinding Animation.");
                if(_mainAnimation != null)_mainAnimation[clip.name].speed = isReverse ? -1 : 1;
            }
            else
            {
                path = $"Animation/{Managers.ContentInfo.PlayData.Depth1}" +
                       $"{Managers.ContentInfo.PlayData.Depth2}" +
                       $"{Managers.ContentInfo.PlayData.Depth3}" + $"/{count}";
                
                reverseClip = Resources.Load<AnimationClip>(path);
                
                clip = reverseClip;
                Logger.Log($"Reverse clip too short, using original clip. Path:  {path}");
                if(_mainAnimation != null)_mainAnimation[clip.name].speed = 1;
            }
        }

        if (clip == null)
        {
            Logger.Log("Clip is null.");
        }
     
        //else { Logger.Log("animation is null."); }
        if(!isReverse) _mainAnimation[clip.name].time = 0;

  
    
        _mainAnimation.Play(clip.name);
        StartCoroutine(CheckAnimationEnd(clip, OnAnimationComplete));
        currentCilpLength = clip.length;    

        Logger.Log($"Animation clip with index {count} is playing.");
      
    }


    private IEnumerator PlayAnimationWithDelay(string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);
        _mainAnimation.Play(clipName);
    }

    private IEnumerator CheckAnimationEnd(AnimationClip clip, Action onAnimationComplete)
    {
        yield return new WaitForSeconds(clip.length);
        OnAnimationComplete();
    }
    protected float _narrationStartDelay = 0.2f;

    public static event Action<int> OnAnimationCompelete;

    private void OnAnimationComplete()
    {
      //  Managers.Sound.Play(SoundManager.Sound.Narration, Managers.ContentInfo.PlayData.CurrentDepthStatus);
      
        Managers.Sound.PlayNarration(_narrationStartDelay);
        contentController.isStepMissionComplete = false;
        OnAnimationCompelete?.Invoke(currentCount);
    }

    public void ChangeInstructionTextWithAnim(int delay = 0)
    {
        contentController.ChangeInstructionText();
    }

    #endregion


    #region Animation

    protected void SetHighlight(int gameObjName, bool isOn = true)
    {

        objectHighlightMap[gameObjName].enabled = true;
        objectHighlightMap[gameObjName].highlighted = isOn;
       objectHighlightMap[gameObjName].outlineWidth = OUTLINE_WIDTH_ON_HOVER;
    }

    public void SetHighlightIgnore(int gameObjName, bool isOn = true)
    {

        if (objectHighlightMap.ContainsKey(gameObjName))
        {
            objectHighlightMap[gameObjName].ignore = isOn;
            objectHighlightMap[gameObjName].enabled = isOn;
        }
        else
        {
            Debug.LogWarning($"There's no key in highlightmap to ignore : {gameObjName}");
        }
    }

    public void TurnOffAllRegisteredHighlights()
    {
        if (objectHighlightMap == null)
        {
            Logger.LogWarning("there's no registered highlight.. return");
            return;
        }
        foreach (var key in objectHighlightMap.Keys.ToArray())
        {
            objectHighlightMap[key].enabled = false;
        }
    }
   
    

    public void HighlightBlink(int gameObj, float startDelay = 1f)
    {
        
        _seqMap.TryAdd(gameObj, null);
        _seqMap[gameObj]?.Kill();
        _seqMap[gameObj]= DOTween.Sequence();
        
        objectHighlightMap[(int)gameObj].enabled = true;
        objectHighlightMap[(int)gameObj].highlighted= true;
        
        objectHighlightMap[(int)gameObj].innerGlow = 0;
        objectHighlightMap[(int)gameObj].outlineWidth = 0;

        var maxInnerGlow = 0.8f;
        var maxOuterGlow = 1f;
        var duration = 0.55f;


        
//        if (_seqMap[(int)gameObj].IsActive()) _seqMap[(int)gameObj].Kill();

        _seqMap[gameObj].AppendInterval(startDelay);
        _seqMap[gameObj].AppendCallback(() => { objectHighlightMap[(int)gameObj].highlighted = true; });

        var loopCount = 5;
        for (var i = 0; i < loopCount; i++)
        {
            _seqMap[gameObj].AppendCallback(() => { objectHighlightMap[(int)gameObj].highlighted = true; });

            _seqMap[gameObj].Append(DOVirtual.Float(0, maxInnerGlow, duration,
                val =>
                {
                    objectHighlightMap[(int)gameObj].innerGlow = val;
                    objectHighlightMap[(int)gameObj].outlineWidth = val;
                }));

            _seqMap[gameObj].Append(DOVirtual.Float(maxInnerGlow, 0, duration,
                val =>
                {
                    objectHighlightMap[(int)gameObj].innerGlow = val; 
                    objectHighlightMap[(int)gameObj].outlineWidth = val;
                }));

        }

        _seqMap[gameObj].AppendCallback(() => { objectHighlightMap[(int)gameObj].highlighted = false; });

        _seqMap[gameObj].OnKill(() =>
        {
            objectHighlightMap[(int)gameObj].highlighted = false;
            objectHighlightMap[(int)gameObj].innerGlow = 0;
        });
        

        _seqMap[gameObj].Play();
    }

    public void AddToHighlightDictionary(int gameObj)
    {
        var objName = GetObject((int)gameObj).name;
        var highlightEffect = GetObject((int)gameObj).GetComponent<HighlightEffect>();

        //초기하이라이트 설정
        SetDefaultHighlight(ref highlightEffect);
        if (!objectHighlightMap.ContainsKey((int)gameObj))
        {
//            Logger.Log($"하이라이트 Key 추가 ------- {gameObj} :{(DepthC_GameObj)gameObj}");
            objectHighlightMap.Add((int)gameObj, highlightEffect);
        }
    }

    private void SetDefaultHighlight(ref HighlightEffect effect)
    {
        effect.highlighted = false;
        effect.ignore = true;
        effect.outlineColor =Color.cyan;
        effect.outlineWidth = 4;

    }
    public void BindHighlightAndTooltip(int gameObj, string tooltipText)
    {
        // PointerEnter 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            if (objectHighlightMap[(int)gameObj].ignore) return; 
            
//            Logger.Log("sensor hover highlight and tooltip appear ----------------------");
            SetHighlight(gameObj);
            contentController.SetToolTipActiveStatus();
            contentController.SetToolTipText(tooltipText);
        }, Define.UIEvent.PointerEnter);

        // PointerExit 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(gameObj,false);
            contentController.SetToolTipActiveStatus(false);
        }, Define.UIEvent.PointerExit);
        
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(gameObj,false);
            contentController.SetToolTipActiveStatus(false);
        }, Define.UIEvent.PointerDown);
    }
    
    

    public void SetHighlightStatus(int gameObj, bool isOn)
    {
        objectHighlightMap[(int)gameObj].highlighted = isOn;
    }

    public void BindAndAddToDictionaryAndInit(int gameObj, string tooltipText)
    {
        AddToHighlightDictionary(gameObj);
        BindHighlightAndTooltip(gameObj, tooltipText);
    }

    #endregion

    protected void OnStepMissionComplete(int objectEnumToInt = -1, int animationNumber = -123456789,
        WaitForSeconds delayAmount = null, Action delayedAction = null)
    {
        if (objectEnumToInt != -1 && objectHighlightMap.ContainsKey(objectEnumToInt) &&
            objectHighlightMap[objectEnumToInt].ignore)
        {
            Logger.Log("클릭불가 상태 ,오브젝트가 없거나 하이라이트 ignore 상태입니다.");

        }

        StartCoroutine(OnStepMissionCompleteCo(animationNumber, delayAmount, delayedAction));
    }
    
    protected IEnumerator OnStepMissionCompleteCo(int currentStepNum, WaitForSeconds waitForSeconds = null,
        Action ActionBeforeNextStep = null)
    {

        var currentStepNumCache = currentStepNum; 
        
        if(contentController.isStepMissionComplete)
        {
            Logger.Log("애니메이션 재생중. 중복실행 X XXXXXXX");
            yield break;
        }

        contentController.isStepMissionComplete = true;
       
        if (Managers.ContentInfo.PlayData.Count != currentStepNum)
            Debug.LogWarning("현재 애니메이션 재생과 카운트 불일치.. 다른 애니메이션이거나 여러 곳 사용되는 애니메이션일 수 있습니다.");

        
        
        PlayAnimationAndNarration(currentStepNum, isServeAnim: true);
        Logger.Log($"서브 애니이션 재생: {currentStepNum}");
        
        ActionBeforeNextStep?.Invoke();
        
        if (waitForSeconds == null)
        {
            Logger.Log($"waitforsecond is null, wait for currentClipLength..======>{currentCilpLength + 1f}");
            _waitBeforeNextStep = new WaitForSeconds(currentCilpLength + 1f);
        }
    
        if (waitForSeconds != null)
        {Logger.Log($"waitforsecond is not null");
            _waitBeforeNextStep = waitForSeconds;
        }

  


     
        
    //    OnMissionFinish(); //사운드 재생 등 성공처리

        yield return _waitBeforeNextStep;

        

        Logger.Log($"작업 수행을 통한 다음 이벤트 재생 :--------------- {Managers.ContentInfo.PlayData.Count}-");
       
        
        if(currentStepNumCache == Managers.ContentInfo.PlayData.Count)
        {contentController.InvokeNextStep(); // 다음 스텝으로 넘어가기
        }
        else
        {
            Debug.Log("실행하고자하는 다음스텝과 현재 스텝이 달라 미션수행을 통한 다음 스텝 재생을 실행하지 않습니다-----" +
                      $"\n현재스텝: {Managers.ContentInfo.PlayData.Count}, 실행하고자 하는 스텝 {currentStepNumCache}");
        }

      
        yield return _waitBeforeNextStep;
      
        //  isSceneAnimationPlayingToProtectDoublePlaying = false;
    }
    protected virtual void UnBindEventAttatchedObj()
    {
        
        if (_objects != null)

            foreach (var obj in _objects[typeof(GameObject)])
            {
                var gameObj = obj as GameObject;
                if (gameObj != null)
                {
                    gameObj.UnBindEvent();
                    Logger.Log($"Event Unbind ------------------name {gameObj.name}");
                }
            }
    }
    
    
    

    #region 바인딩 로직

    protected Dictionary<Type, Object[]> _objects = new();

    protected bool _init = false;

    protected void Bind<T>(Type type) where T : Object
    {
        var names = Enum.GetNames(type);
        var objects = new Object[names.Length];
        _objects.TryAdd(typeof(T), objects);

#if UNITY_EDITOR
//Debug.Log($"object counts to bind {names.Length}");
#endif
        for (var i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true);
                GameObject go = (GameObject)objects[i];
                go?.GetOrAddComponent<CursorImageController>();
            }
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }


    protected void BindObject(Type type)
    {
        Bind<GameObject>(type);
    }

    protected void BindImage(Type type)
    {
        Bind<Image>(type);
    }

    protected void BindTMP(Type type)
    {
        Bind<TextMeshProUGUI>(type);
    }

    protected void BindText(Type type)
    {
        Bind<Text>(type);
    }

    protected void BindButton(Type type)
    {
        Bind<Button>(type);
    }


    protected void BindToggle(Type type)
    {
        Bind<Toggle>(type);
    }

    protected void BindSlider(Type type)
    {
        Bind<Slider>(type);
    }


    protected T Get<T>(int idx) where T : Object
    {
        Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    public GameObject GetObject(int idx)
    {
        return Get<GameObject>(idx);
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }

    protected TextMeshProUGUI GetTMP(int idx)
    {
        return Get<TextMeshProUGUI>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Slider GetSlider(int idx)
    {
        return Get<Slider>(idx);
    }

    protected Toggle GetToggle(int idx)
    {
        return Get<Toggle>(idx);
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }

    public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        var evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.UIEvent.PointerEnter:
                evt.OnPointerEnterHander -= action;
                evt.OnPointerEnterHander += action;
                break;
            case Define.UIEvent.PointerExit:
                evt.OnPointerExitHandler -= action;
                evt.OnPointerExitHandler += action;
                break;
        }
    }

    #endregion------------------------------------------------------
}