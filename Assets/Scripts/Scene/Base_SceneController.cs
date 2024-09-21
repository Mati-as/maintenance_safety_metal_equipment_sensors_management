using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class Base_SceneController : MonoBehaviour, ISceneController
{
    protected UI_ContentController _contentController;

    private readonly float _startDelay = 2f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;
    
    //Animation Part
    
    protected Animation _objAnimation;// GameOjbect의 최상 부모위치에 있어야함.
    protected Animation _cameraAnimation;// GameOjbect의 최상 부모위치에 있어야함.

    public virtual void Start()
    {
        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        StartCoroutine(OnSceneStartCo());
        BindEvent();

    }

    private void BindEvent()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepEnter;
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStep;
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepExit;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepEnter;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStep;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepExit;
    }

    private void OnDestroy()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepEnter;
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStep;
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepExit;
    }

    public virtual void DefaultSet()
    {
        _contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        _contentController.Init();
        _contentController.PlayIntroUIAnimation();
        _contentController.SetActiveInstruction(false);

        Managers.Sound.Play(SoundManager.Sound.Effect, "Depth1Start");
    }

    private IEnumerator OnSceneStartCo()
    {
        DefaultSet();

        _cameraAnimation = Camera.main.gameObject.GetComponent<Animation>();
        _objAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();
        
        Debug.Assert(_cameraAnimation != null && _objAnimation !=null);
        
        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;
        
        

        _contentController.SetActiveInstruction();

        OnStepEnter(1);
    }

    private void SetDefault()
    {
        _contentController.ShutTrainingInfroAnim();
        Managers.Sound.Stop(SoundManager.Sound.Narration);
    }
    public virtual void OnStepEnter(int currentCount)
    {
        SetDefault();
        
        Managers.Sound.Play(SoundManager.Sound.Narration, "Test_Narration");
        
        PlayObjAnimation(currentCount);
        PlayCamAnimation(currentCount);
    }

    public virtual void OnStep(int currentCount)
    {
        
    }

    public virtual void OnStepExit(int currentCount)
    {
        
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
                Debug.Log($"Animation clip with index {number} is playing.");
            }
        }
        else
        {
            Debug.LogWarning($"카메라 : {number} 애니메이션 없는상태. 카메라 애니메이션 정지 " );
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