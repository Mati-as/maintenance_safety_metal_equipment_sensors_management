
using System;
using DG.Tweening;

public class Base_SceneState : ISceneState
{
    protected Base_SceneController CurrentScene;

    private bool _isCurrentStateCameraControllable;
    private Sequence _lookAtSeq;
    
    //OnEnter에서 카메라 사용여부 설정 - OnStep에서 캐싱 - SetLookAt에서 캐싱값사용.
    public bool isCurrentStateCameraControllable
    {
        get { return _isCurrentStateCameraControllable;}
        set
        {
            _isCurrentStateCameraControllable = value;
            if(_isCurrentStateCameraControllable)Logger.Log("currentSceneControllable");
            else
            {
                Logger.Log("카메라 컨트롤 불가로 설정");
            }
        }
        
    }
    
  
    protected float _animationDelay=0;
 
  
    // BaseScene 참조를 생성자에서 주입합니다. 
    protected Base_SceneState(Base_SceneController currentScene)
    {
        CurrentScene = currentScene;
        //Logger.Log($"state logic Uploaded : current scene controller: {currentScene.name}");
    }


    public virtual void OnEnter()
    {
     
 
        
        //misson수행과 사용자 컨트롤의 구분
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
        Base_SceneController.OnAnimationCompelete += OnAnimationCompleteHandler;
   
        CurrentScene.cameraController.isControllable = false;
        isCurrentStateCameraControllable = false;

        //평가하기가 아닌경우
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation)
        {
            Managers.evaluationManager. InitPerState();
        }

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.SensorOverview)
        {
            CurrentScene.contentController.HideToolBoxBtn();
        }
        else
        {
            CurrentScene.contentController.ShowToolBoxAndGuideBookBtn();
        }
        
        //튜토리얼이 아닌경우
        CurrentScene.SetHighlightIgnore((int)DepthC2_GameObj.MultimeterHandleHighlight,false);
          
        CurrentScene.contentController.SetNextPrevBtnsActiveStatus();
      
        CurrentScene.ChangeInstructionTextWithAnim();

        
        //항상 클릭 가능해야하는 것들 ----

        CurrentScene.RefreshAnimationCoroutines();
        CurrentScene.PlayAnimation(CurrentScene.currentCount,isServeAnim: false);
        
        CurrentScene.contentController.isStepMissionComplete = false;
        Logger.Log($"현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");

    }


    public virtual void OnStep()
    {
        CurrentScene.contentController.isStepMissionComplete = false;
        CurrentScene.cameraController.isControllable = false;//ui를 끄기위해 STEP에서 반드시 실행
        
        // isCurrentStateCameraControllable = CurrentScene.cameraController.isControllable;
        // CurrentScene.cameraController.isControllable = false;
        Logger.Log($"OnStep현재 카메라 움직임 가능 여부 ------{isCurrentStateCameraControllable}");
    }

    public virtual void OnExit()
    {
        
       /* Depth4 훈련하기에서만 진행할 수 있도록 다음과 같이 구성하며, UI클릭 미션인경우 다른 오브젝트도
      오답쳐리가 될 수 있도록 다음과 같이 센티널 값을 넣어 구성합니다.
     */
       if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation)
       {
           Managers.evaluationManager.EvalmodeOnStateExit();
          
       }
       CurrentScene.TurnOffAllRegisteredHighlights();
       CurrentScene.contentController.StopBtnUIBlink();
       CurrentScene.contentController.uiToolBox.Refresh();

       Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
    
       CurrentScene.contentController.ShutTrainingIntroAnim();
        
    }
    
    
    public void SetLookAt(int objToActivate)
    {
        CheckValidity();
        if (!isCurrentStateCameraControllable)
        {
            Logger.Log($"현재 cameracontrollable {isCurrentStateCameraControllable}.... LookAt 호출 취소");
            return;
        }
        
        CurrentScene.cameraController.SetRotationDefault(CurrentScene.GetObject(objToActivate).transform);
        if (isCurrentStateCameraControllable) CurrentScene.cameraController.isControllable = false;
        
        CurrentScene.cameraController.isControllable = false;
        
        _lookAtSeq?.Kill();
        _lookAtSeq = DOTween.Sequence();

        Action actionA = () => CurrentScene.cameraController.UpdateRotation(0.45f);
        _lookAtSeq.AppendCallback(actionA.Invoke);
        _lookAtSeq.AppendInterval(0.76f);
        _lookAtSeq.AppendCallback(() =>
        {
            CheckValidity();
            if (isCurrentStateCameraControllable)
            {
                Logger.Log("카메라초기화완료, 및 카메라 컨트롤 현재부터 가능");
                CurrentScene.cameraController.isControllable = true;
            }
            else
            {
                Logger.Log("카메라 컨트롤 불가");
            }

        });

        _lookAtSeq.Play();

    }

    private void CheckValidity()
    {
        if(CurrentScene == null) 
        {
            Logger.LogWarning("Currentscene is null. It must be timing of loading different scene");
            return;
        }
    }
    
    /// <summary>
    /// OnAnimatrionCompele 사용시
    /// 자동으로 1.카메사용 가능, 2. 초기화 버튼 하이라이트 동작
    /// </summary>
    /// <param name="_"></param>
    protected virtual void OnAnimationCompleteHandler(int _)
    { 
        
        if(CurrentScene!=null) CurrentScene.cameraController.isDragging = false;
        if (CurrentScene.contentController == null) Logger.Log("content Controller is Null.. it must be tutorial state.");
    }
    

}