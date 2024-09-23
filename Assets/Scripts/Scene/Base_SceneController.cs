using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Base_SceneController : MonoBehaviour, ISceneController
{
    public UI_ContentController contentController;

    private readonly float _startDelay = 2f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;

    //Animation Part

    protected Animation _objAnimation; // GameOjbect의 최상 부모위치에 있어야함.
    protected Animation _cameraAnimation; // GameOjbect의 최상 부모위치에 있어야함.

    // SceneState Control
    protected Dictionary<int, ISceneState> _sceneStates;
    protected ISceneState _currentState;

    public virtual void Start()
    {
        Init();
    }
    
    public virtual void Init()
    {
	    
	    BindEvent();

        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        
        contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        StartCoroutine(OnSceneStartCo());
   
        Managers.Sound.Play(SoundManager.Sound.Effect, "Depth1Start");
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
        PlayInitialUIIntro();

        _cameraAnimation = Camera.main.gameObject.GetComponent<Animation>();
        _objAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();

        Debug.Assert(_cameraAnimation != null && _objAnimation != null);

        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;


        contentController.SetActiveInstruction();

        OnStepChange(1);
    }

    
    /// <summary>
    /// 씬로드시 처음만 재생됩니다. 
    /// </summary>
    public virtual void PlayInitialUIIntro()
    {
	    Logger.Log("Initial UI Intro");
        contentController.Init();
        contentController.ShowMainIntro();
        contentController.SetActiveInstruction(false);
    }
    public void ShutUIAndSetDefault()
    {
       
        contentController.ShutTrainingInfroAnim();
        Managers.Sound.Stop(SoundManager.Sound.Narration);
    }

    public virtual void OnStepChange(int currentCount)
    {
        ChangeState(currentCount);

        PlayObjAnimation(currentCount);
        PlayCamAnimation(currentCount);
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

    private void PlayAnimation(Animation animationComponent, int number, float delay = 0f, float animSpeed = 1f)
    {
        var clip = animationComponent.GetClip(number.ToString());

        if (clip != null)
        {
            animationComponent[clip.name].speed = animSpeed;

            if (delay > 0.5f)
            {
                StartCoroutine(PlayAnimationWithDelay(animationComponent, clip.name, delay));
            }
            else
            {
                animationComponent.Play(clip.name);
                Logger.Log($"Animation clip with index {number} is playing.");
            }
        }
        else
        {
            Logger.LogWarning($"카메라 : {number} 애니메이션 없는상태. 카메라 애니메이션 정지 ");
        }
    }

    private IEnumerator PlayAnimationWithDelay(Animation animationComponent, string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animationComponent.Play(clipName);
    }


    private void PlayObjAnimation(int number, float delay = 0f, float animSpeed = 1f)
    {
        PlayAnimation(_objAnimation, number, delay, animSpeed);
    }

    private void PlayCamAnimation(int number, float delay = 0f, float animSpeed = 1f)
    {
        PlayAnimation(_cameraAnimation, number, delay, animSpeed);
    }
    
    
    
    
    
    
    
    
    
    
    //바인딩 로직------------------------------------------------------
    
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

	protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
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
}