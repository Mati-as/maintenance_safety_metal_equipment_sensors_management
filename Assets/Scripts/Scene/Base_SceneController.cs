using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Base_SceneController : MonoBehaviour, ISceneController
{
	protected readonly int ANIMATION_MAX_COUNT;
  
	public UI_ContentController contentController;
    private readonly float _startDelay = 2f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;
    private Inplay_CameraController _cameraController;
  
    //Animation Part

    protected Animation _animation; // GameOjbect의 최상 부모위치에 있어야함.
    protected Animation _cameraAnimation; // GameOjbect의 최상 부모위치에 있어야함.

    // SceneState Control
    protected Dictionary<int, ISceneState> _sceneStates;
    protected ISceneState _currentState;
    public int currentCount { get; private set; }

    public virtual void Start()
    {
        Init();
    }
    
    public virtual void Init()
    {
	    
	    BindEvent();

	    Debug.Assert(Camera.main != null);
		    _cameraController = Camera.main.GetComponent<Inplay_CameraController>();
	    

	    Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        
        contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        StartCoroutine(OnSceneStartCo());
   
        Managers.Sound.Play(SoundManager.Sound.Effect, "Depth1Start");


        int introAnimation = 1;
        PlayAnimationAndNarration(introAnimation);
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



    private IEnumerator OnSceneStartCo()
    {
        PlayInitialIntro();

        
        _animation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();

        Debug.Assert(_animation != null);

        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;


        contentController.SetActiveInstruction();

        
    }

    
    /// <summary>
    /// 씬로드시 처음만 재생됩니다. 
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

    
    public virtual void OnStepChange(int count)
    {
        currentCount = count;
        ChangeState(currentCount);
        
    }

    private void ChangeState(int stateNum)
    {
        if (_sceneStates.TryGetValue(stateNum, out var newState))
        {
            
            Logger.Log($"state change...---------current State ={newState} ");
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
        else
        {
            Logger.LogWarning($"No state found for depth {stateNum}");
        }
    }

    #region State 호출함수
    
    public void PlayAnimationAndNarration( int number, float delay = 0f, float animSpeed = 1f)
    {
        var clip = _animation.GetClip(number.ToString());

        if (clip != null)
        {
	        _animation[clip.name].speed = animSpeed;

            if (delay > 0.5f)
            {
                StartCoroutine(PlayAnimationWithDelay( clip.name, delay));
            }
            else
            {
	            _animation.Play(clip.name);
	            StartCoroutine(CheckAnimationEnd(clip, OnAnimationComplete));
                Logger.Log($"Animation clip with index {number} is playing.");
            }
        }
        else
        {
	        OnAnimationComplete();
            Logger.LogWarning($"카메라 : {number} 애니메이션 없는상태.스크립트는 재생 카메라 애니메이션 정지 ");
        }
    }

    private IEnumerator PlayAnimationWithDelay( string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);
        _animation.Play(clipName);
    }
    private IEnumerator CheckAnimationEnd(AnimationClip clip, Action onAnimationComplete)
    {
	    yield return new WaitForSeconds(clip.length);
	    OnAnimationComplete();
    }

    private void  OnAnimationComplete()
    {
	    
	    Managers.Sound.Play(SoundManager.Sound.Narration, Managers.ContentInfo.PlayData.CurrentDepthStatus);
	    _cameraController.SetCurrentMainAngle();
	 
    }

    public void ChangeInstructionTextWithAnim(int delay =0)
    {
	    contentController.ChangeInstructionTextWithAnim();
    }
    
    #endregion



    


    #region 바인딩 로직


    
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

	protected bool _init = false;
    
	protected void Bind<T>(Type type) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);

#if UNITY_EDITOR
//s		Debug.Log($"object counts to bind {names.Length}");
#endif
		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = Utils.FindChild(gameObject, names[i], true);
			else
				objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

			if (objects[i] == null)
				Debug.Log($"Failed to bind({names[i]})");
		}
	}


	protected void BindObject(Type type) { Bind<GameObject>(type);  }
	protected void BindImage(Type type) { Bind<Image>(type);  }
	protected void BindTMP(Type type) { Bind<TextMeshProUGUI>(type);  }
	protected void BindText(Type type) { Bind<Text>(type);  }
	protected void BindButton(Type type) { Bind<Button>(type);  }

	
	protected void BindToggle(Type type) { Bind<Toggle>(type);  }
	protected void BindSlider(Type type) { Bind<Slider>(type);  }
	
	
	protected T Get<T>(int idx) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_objects.TryGetValue(typeof(T), out objects) == false)
			return null;

		return objects[idx] as T;
	}

	public GameObject GetObject(int idx) { return Get<GameObject>(idx); }
	protected Text GetText(int idx) { return Get<Text>(idx); }
	protected TextMeshProUGUI GetTMP(int idx) { return Get<TextMeshProUGUI>(idx); }
	protected Button GetButton(int idx) { return Get<Button>(idx); }
	protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
	protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
	protected Image GetImage(int idx) { return Get<Image>(idx); }

	public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
	{  
		UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);
        
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