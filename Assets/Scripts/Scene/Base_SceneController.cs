using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        
        contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        StartCoroutine(OnSceneStartCo());
        BindEvent();
     
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
        IntroUIAnim();

        _cameraAnimation = Camera.main.gameObject.GetComponent<Animation>();
        _objAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();

        Debug.Assert(_cameraAnimation != null && _objAnimation != null);

        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;


        contentController.SetActiveInstruction();

        OnStepChange(1);
    }

    public virtual void IntroUIAnim()
    {
        contentController.Init();
        contentController.PlayIntroUIAnimation();
        contentController.SetActiveInstruction(false);
    }
    public void ShutUIAndSetDefault()
    {
        Logger.Log("shut info ui");
        contentController.ShutTrainingInfroAnim();
        Managers.Sound.Stop(SoundManager.Sound.Narration);
    }

    public virtual void OnStepChange(int currentCount)
    {
        Managers.Sound.Play(SoundManager.Sound.Narration, "Test_Narration");

        ChangeState(currentCount);

        PlayObjAnimation(currentCount);
        PlayCamAnimation(currentCount);
    }

    private void ChangeState(int depth)
    {
        if (_sceneStates.TryGetValue(depth, out var newState))
        {
            Logger.Log($"state change...---------current State ={newState} ");
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
        else
        {
            Logger.LogWarning($"No state found for depth {depth}");
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
}