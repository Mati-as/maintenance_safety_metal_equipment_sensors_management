using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Base_SceneController : MonoBehaviour, ISceneController
{
    protected int CurrentActiveTool;
    protected readonly int NO_TOOL_SELECTED = -1;
    protected readonly int ANIMATION_MAX_COUNT;
    public Inplay_CameraController cameraController { get; protected set; }

    public UI_ContentController contentController;
    private readonly float _startDelay = 2f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;

    //Animation Part

    protected Animation _animation; // GameOjbect의 최상 부모위치에 있어야함.
    protected Animation _cameraAnimation; // GameOjbect의 최상 부모위치에 있어야함.

    // SceneState Control
    protected Dictionary<int, ISceneState> _sceneStates;
    protected ISceneState _currentState;
    public int currentCount { get; private set; }


    //Animation
    public Dictionary<int, HighlightEffect> objectHighlightMap;
    private Dictionary<int, Sequence> _seqMap;

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
        
        if(Managers.UI.SceneUI ==null) Managers.UI.ShowSceneUI<UI_Persistent>();
        
        objectHighlightMap = new Dictionary<int, HighlightEffect>();
        _seqMap = new Dictionary<int, Sequence>();

        Debug.Assert(Camera.main != null);
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();

        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
    }

    private void BindEvent()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChange;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepChange;
    }

    private void OnDestroy()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChange;
    }


    protected IEnumerator OnSceneStartCo()
    {
        Logger.Log("Initial UI Intro");
        
        
        PlayInitialIntro();
        _animation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();

        Debug.Assert(_animation != null);

        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;


        contentController.SetActiveInstruction();
        var introAnimation = 1;
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
        contentController.ShowInitialIntro();
        contentController.SetActiveInstruction(false);
    }

    public void ShutUIAndSetDefault()
    {
        contentController.ShutTrainingInfroAnim();
        Managers.Sound.Stop(SoundManager.Sound.Narration);
    }

    public bool isReverseAnim { get; private set; }

    public virtual void OnStepChange(int count, bool isReverse = false)
    {
        //재생관련 파라미터를 미리 설정 한 후 재생합니다. (재생순서, 방식 등)
        isReverseAnim = isReverse;
        currentCount = count;


        ChangeState(currentCount);
    }

    private void ChangeState(int stateNum)
    {
        var processedState = int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                             $"{Managers.ContentInfo.PlayData.Depth2}"+
                             $"{Managers.ContentInfo.PlayData.Depth3}"+
                             $"{stateNum.ToString()}");
        

        Logger.Log($"Current StateNum : {processedState}");
        
        if (_sceneStates.TryGetValue(processedState, out var newState))
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
        else
        {
            Logger.LogWarning($"No state found for depth {stateNum}");
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
    public void PlayAnimationAndNarration(int count, float delay = 0f, bool isReverse = false,bool isServeAnim =false)
    {
      
        Debug.Assert(_animation != null, "Animation component can't be null");

        var path = $"Animation/{Managers.ContentInfo.PlayData.Depth1}" +
                   $"{Managers.ContentInfo.PlayData.Depth2}" +
                   $"{Managers.ContentInfo.PlayData.Depth3}" + $"/{count}";

        if (isServeAnim)
        {
            path += 'A';
        }
        
        Logger.Log($"Animation Path {path}");
        var clip = Resources.Load<AnimationClip>(path);

        if (clip == null)
        {
            Logger.LogWarning($"Animation clip at path {path} not found.");
            OnAnimationComplete();
            return;
        }


        if (!_animation.GetClip(clip.name))
        {
            _animation.AddClip(clip, clip.name);
            Logger.Log($"Added animation clip {clip.name} to _animation.");
        }

        if (isReverse)
        {
            path = $"Animation/{Managers.ContentInfo.PlayData.Depth1}" +
                   $"{Managers.ContentInfo.PlayData.Depth2}" +
                   $"{Managers.ContentInfo.PlayData.Depth3}" + $"/{count + 1}";
            Logger.Log($"Reverse Animation Path {path}");
            var reverseClip = Resources.Load<AnimationClip>(path);

            if (reverseClip != null && reverseClip.length >= 0.3f)
            {
                clip = reverseClip;
                _animation[clip.name].time = _animation[clip.name].length;
            }
            else
            {
                Logger.Log($"Reverse clip too short, using original clip.");
            }
        }

        _animation[clip.name].speed = isReverse ? -1 : 1;
        if(!isReverse) _animation[clip.name].time = 0;

        if (delay > 0.5f)
        {
            StartCoroutine(PlayAnimationWithDelay(clip.name, delay));
        }
        else
        {
            _animation.Play(clip.name);
            StartCoroutine(CheckAnimationEnd(clip, OnAnimationComplete));
        }

        Logger.Log($"Animation clip with index {count} is playing.");

    }


    private IEnumerator PlayAnimationWithDelay(string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);
        _animation.Play(clipName);
    }

    private IEnumerator CheckAnimationEnd(AnimationClip clip, Action onAnimationComplete)
    {
        yield return new WaitForSeconds(clip.length);
        OnAnimationComplete();
    }


    public static event Action<int> OnAnimationCompelete;

    private void OnAnimationComplete()
    {
        Managers.Sound.Play(SoundManager.Sound.Narration, Managers.ContentInfo.PlayData.CurrentDepthStatus);
        OnAnimationCompelete?.Invoke(currentCount);
    }

    public void ChangeInstructionTextWithAnim(int delay = 0)
    {
        contentController.ChangeInstructionTextWithAnim();
    }

    #endregion


    #region Animation

    protected void SetHighlight(int gameObjName, bool isOn = true)
    {
       // Logger.Log($"[{(DepthC_GameObj)gameObjName}]Highight is ON? : {isOn}");
        objectHighlightMap[gameObjName].highlighted = isOn;
    }

    public void SetHighlightIgnore(int gameObjName, bool isOn = true)
    {
       
        objectHighlightMap[gameObjName].ignore = isOn;
    }


    public void HighlightBlink(int gameObj, float startDelay = 1f)
    {
        
        
        var seq = DOTween.Sequence();
        objectHighlightMap[(int)gameObj].enabled = true;
        _seqMap.TryAdd((int)gameObj, seq);

        _seqMap[gameObj] = seq;

        var maxInnerGlow = 0.8f;
        var maxOuterGlow = 1f;
        var duration = 0.6f;

        
//        if (_seqMap[(int)gameObj].IsActive()) _seqMap[(int)gameObj].Kill();

        seq.AppendInterval(startDelay);
        seq.AppendCallback(() => { objectHighlightMap[(int)gameObj].highlighted = true; });

        var loopCount = 4;
        for (var i = 0; i < loopCount; i++)
        {
            seq.Append(DOVirtual.Float(0, maxInnerGlow, duration,
                val =>
                {
                    objectHighlightMap[(int)gameObj].innerGlow = val;
                    objectHighlightMap[(int)gameObj].outlineWidth = val;
                }));

            seq.Append(DOVirtual.Float(maxInnerGlow, 0, duration,
                val =>
                {
                    objectHighlightMap[(int)gameObj].innerGlow = val; 
                    objectHighlightMap[(int)gameObj].outlineWidth = val;
                }));

        }

        seq.AppendCallback(() => { objectHighlightMap[(int)gameObj].highlighted = false; });

        seq.OnKill(() =>
        {
            objectHighlightMap[(int)gameObj].highlighted = false;
            objectHighlightMap[(int)gameObj].innerGlow = 0;
        });
        _seqMap[(int)gameObj] = seq;

        _seqMap[gameObj].Play();
    }

    public void AddToHighlightDictionary(int gameObj)
    {
        var objName = GetObject((int)gameObj).name;
        var highlightEffect = GetObject((int)gameObj).GetComponent<HighlightEffect>();

        //초기하이라이트 설정
        SetDefaultHighlight(ref highlightEffect);
        if (!objectHighlightMap.ContainsKey((int)gameObj)) objectHighlightMap.Add((int)gameObj, highlightEffect);
    }

    private void SetDefaultHighlight(ref HighlightEffect effect)
    {
        effect.highlighted = false;
        effect.ignore = true;
        effect.outlineColor =Color.cyan;
        
    }
    public void BindHighlightAndTooltip(int gameObj, string tooltipText)
    {
        // PointerEnter 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            if (objectHighlightMap[(int)gameObj].ignore) return; 
            
            Logger.Log("sensor hover highlight and tooltip appear ----------------------");
            SetHighlight(gameObj);
            contentController.SetToolTipStatus();
            contentController.SetToolTipText(tooltipText);
        }, Define.UIEvent.PointerEnter);

        // PointerExit 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(gameObj,false);
            contentController.SetToolTipStatus(false);
        }, Define.UIEvent.PointerExit);
        
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(gameObj,false);
            contentController.SetToolTipStatus(false);
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


    #region 바인딩 로직

    protected Dictionary<Type, Object[]> _objects = new();

    protected bool _init = false;

    protected void Bind<T>(Type type) where T : Object
    {
        var names = Enum.GetNames(type);
        var objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

#if UNITY_EDITOR
//s		Debug.Log($"object counts to bind {names.Length}");
#endif
        for (var i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
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