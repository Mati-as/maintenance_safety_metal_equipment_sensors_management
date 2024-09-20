using System.Collections;
using UnityEngine;

public class Base_SceneController : MonoBehaviour, ISceneController
{
    private UI_ContentController _contentController;

    private readonly float _startDelay = 2f; // 맨처음 스크립트 시작 딜레이
    private WaitForSeconds _wait;
    
    //Animation Part
    protected Animation[] _animations;
    protected Animator _animator;// GameOjbect의 최상 부모위치에 있어야함.

    public virtual void Start()
    {
        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
        StartCoroutine(OnSceneStartCo());
    }

    private void BindEvent()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepEnter;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStep;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepExit;
    }

    public virtual void DefaultSet()
    {
        _contentController = Managers.UI.ShowPopupUI<UI_ContentController>();
        _contentController.Init();
        _contentController.SetActiveInstruction(false);

        Managers.Sound.Play(SoundManager.Sound.Effect, "Depth1Start");
    }

    private IEnumerator OnSceneStartCo()
    {
        DefaultSet();

        _wait = new WaitForSeconds(_startDelay);
        yield return _wait;

        _contentController.SetActiveInstruction();

        var narrationPlayDelay = 1f;
        _wait = new WaitForSeconds(narrationPlayDelay);
        yield return _wait;

        Managers.Sound.Play(SoundManager.Sound.Narration, "Test_Narration");
     
    }

    public virtual void OnStepEnter(int currentCount)
    {
    }

    public virtual void OnStep(int currentCount)
    {
    }

    public virtual void OnStepExit(int currentCount)
    {
    }
    
    private void PlayObjAnimation(int number, float delay = 0f, float animSpeed = 1f)
    {
    }

    private void PlayCamAnimation(int number, float delay = 0f, float animSpeed = 1f)
    {
    }
}