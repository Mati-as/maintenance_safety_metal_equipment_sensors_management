using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Base_SceneController : MonoBehaviour, ISceneController
{
    
   
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
    private Dictionary<string, HighlightEffect> _highlight;
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
        _highlight = new Dictionary<string, HighlightEffect>();
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
        if (_sceneStates.TryGetValue(stateNum, out var newState))
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

    public void PlayAnimationAndNarration(int number, float delay = 0f, bool isReverse = false)
    {
        if (isReverse) Logger.Log("Reverse Anim Play -----------------------------------------");
    
        var clip = _animation.GetClip(number.ToString());
      
        if (isReverse)
        {
             clip = _animation.GetClip((number+1).ToString());

             if (clip.length < 0.3f) // 0.3f 미만인 경우 시점만 변환하는 단순애니메이션, 그 이상은 일반애니메이션으로 분류합니다.
             {
                 clip  = _animation.GetClip((number).ToString());
             }
             if(clip!=null) _animation[clip.name].time = _animation[clip.name].length;
        }
        else
        {  
            if(clip!=null)_animation[clip.name].time = 0;
        }
        
        if (clip != null)
        {
            var animSpeed = isReverse ? -1 : 1;
          
            _animation[clip.name].speed = animSpeed;

            if (delay > 0.5f)
            {
                StartCoroutine(PlayAnimationWithDelay(clip.name, delay));
            }
            else
            {
                _animation.Play(clip.name);
                StartCoroutine(CheckAnimationEnd(clip, OnAnimationComplete));
            }

            Logger.Log($"Animation clip with index {number} is playing.====================");
        }
        else
        {
            OnAnimationComplete();
            Logger.LogWarning($"카메라 : {number} 애니메이션 없는상태.스크립트는 재생 카메라 애니메이션 정지 ");
        }
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

    protected void SetHighlight(string gameObjName, bool isOn = true)
    {
        //Logger.Log($"[{gameObjName}]Highight is ON? : {isOn}");
        _highlight[gameObjName].highlighted = isOn;
    }


    public void HighlightBlink(GameObj gameObj, float startDelay = 1f)
    {
        var seq = DOTween.Sequence();
        _seqMap.TryAdd((int)gameObj, seq);

        var maxInnerGlow = 0.15f;
        if (_seqMap[(int)gameObj].IsActive()) _seqMap[(int)gameObj].Kill();

        seq.AppendInterval(startDelay);
        seq.AppendCallback(() => { _highlight[GetObject((int)gameObj).name].highlighted = true; });

        var loopCount = 3;
        for (var i = 0; i < loopCount; i++)
        {
            seq.Append(DOVirtual.Float(0, maxInnerGlow, 1f,
                val => { _highlight[GetObject((int)gameObj).name].innerGlow = val; }));

            seq.Append(DOVirtual.Float(maxInnerGlow, 0, 1f,
                val => { _highlight[GetObject((int)gameObj).name].innerGlow = val; }));
        }

        seq.AppendCallback(() => { _highlight[GetObject((int)gameObj).name].highlighted = false; });

        seq.OnKill(() =>
        {
            _highlight[GetObject((int)gameObj).name].highlighted = false;
            _highlight[GetObject((int)gameObj).name].innerGlow = 0;
        });
        _seqMap[(int)gameObj] = seq;
    }

    public void AddToHighlightDictionary(GameObj gameObj)
    {
        var objName = GetObject((int)gameObj).name;
        var highlightEffect = GetObject((int)gameObj).GetComponent<HighlightEffect>();

        if (!_highlight.ContainsKey(objName)) _highlight.Add(objName, highlightEffect);
    }

    public void BindHighlightAndTooltip(GameObj gameObj, string tooltipText)
    {
        // PointerEnter 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(GetObject((int)gameObj).name);
            contentController.SetToolTipStatus();
            contentController.SetText(tooltipText);
        }, Define.UIEvent.PointerEnter);

        // PointerExit 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(GetObject((int)gameObj).name, false);
            contentController.SetToolTipStatus(false);
        }, Define.UIEvent.PointerExit);
    }

    public void SetHighlightStatus(GameObj gameObj, bool isOn)
    {
        _highlight[GetObject((int)gameObj).name].highlighted = isOn;
    }

    protected void BindAndAddToDictionary(GameObj gameObj, string tooltipText)
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