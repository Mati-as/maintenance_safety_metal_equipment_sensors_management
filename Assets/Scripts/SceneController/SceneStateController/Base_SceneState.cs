
using System;
using DG.Tweening;

public class Base_SceneState : ISceneState
{
    protected Base_SceneController CurrentScene;

    private bool _isCurrentStateCameraControllable;
    private Sequence _lookAtSeq;

    private readonly bool _defaultStatus;
    
    //OnEnter에서 카메라 사용여부 설정 - OnStep에서 캐싱 - SetLookAt에서 캐싱값사용.
    public bool isCurrentStateCameraControllable
    {
        get { return _isCurrentStateCameraControllable;}
        set
        {
            _isCurrentStateCameraControllable = value;
            
            if (_isCurrentStateCameraControllable)
            {
                Logger.Log($" [ Current Count: {Managers.ContentInfo.PlayData.Count} ] 카메라 컨트롤 가능");
                CurrentScene.cameraController.isControllable = true;
            }
            else
            {
                Logger.Log($"[ Current Count: {Managers.ContentInfo.PlayData.Count} ] :카메라 컨트롤 불가로 설정");
                CurrentScene.cameraController.isControllable = false;
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
        
     
      
        //평가하기가 아닌경우
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation)
        {
            Managers.evaluationManager.InitPerState();
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
      
        Logger.Log($"[Current Count: {Managers.ContentInfo.PlayData.Count}] 현재 애니메이션 순서 : 애니메이션 재생{CurrentScene.currentCount}");
       
                   
        _lookAtSeq?.Kill();
        
        CurrentScene.cameraController.isControllable = false;
        CurrentScene.contentController.SetCamInitBtnStatus(false);
        CurrentScene.contentController.HideCamInitBtn();



    }


    /// <summary>
    /// 1.카메라 동작여부
    ///     - 기본적으로 컨트롤 불가로 설정해 놓았습니다. 사용 시에만 true로 설정하면 됩니다. 
    /// 2.기타 OnEnter 직후에 실행될 기능을 넣어줍니다.  
    /// </summary>
    public virtual void OnStep()
    {
    //    Logger.Log($"OnStep [ Current Count: {Managers.ContentInfo.PlayData.Count} ] 현재 카메라 움직임 가능 여부 ------{isCurrentStateCameraControllable}");
        
    }

    public virtual void OnExit()
    {
        
        Base_SceneController.OnAnimationCompelete -= OnAnimationCompleteHandler;
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

        
       
       CurrentScene.contentController.ShutTrainingIntroAnim();
        
       

    }
    
    
    public void SetLookAt(int objToActivate)
    {
        CheckValidity();
        if (!isCurrentStateCameraControllable)
        {
            Logger.Log($"[ Current Count: {Managers.ContentInfo.PlayData.Count} ] 현재 cameracontrollable {isCurrentStateCameraControllable}.... LookAt 호출 취소");
            return;
        }
        
       CurrentScene.cameraController.SetRotationDefault(CurrentScene.GetObject(objToActivate).transform);
     
     
        if (_lookAtSeq == null || !_lookAtSeq.IsActive() || !_lookAtSeq.IsPlaying())
        {
            _lookAtSeq?.Kill();
            _lookAtSeq = DOTween.Sequence();
        }

        var interval = 0.6f;
        if (CurrentScene.currentCilpLength > 0.6f)
        {
            interval = CurrentScene.currentCilpLength;
        }

        
        Action actionA = () => CurrentScene.cameraController.UpdateRotation(0.40f);
        _lookAtSeq.AppendCallback(actionA.Invoke);
        _lookAtSeq.SetAutoKill(false);
        _lookAtSeq.AppendInterval(interval);
        _lookAtSeq.AppendCallback(() =>
        {
            CheckValidity();
            if (isCurrentStateCameraControllable && !CurrentScene.isCurrentServeAnimPlaying)
            {
                Logger.Log($"[ Current Count: {Managers.ContentInfo.PlayData.Count} ]카메라 초기화완료, 및 카메라 컨트롤 현재부터 가능");
                CurrentScene.cameraController.isControllable = true;
                CurrentScene.contentController.SetCamInitBtnStatus(true);
            }
            else
            {
                Logger.Log($"[ Current Count: {Managers.ContentInfo.PlayData.Count} ]카메라 컨트롤 불가");
                CurrentScene.cameraController.isControllable = false;
                CurrentScene.contentController.SetCamInitBtnStatus(false);
            }

        });
        _lookAtSeq.OnKill(() =>
        {
            Logger.Log($"[ Current Count: {Managers.ContentInfo.PlayData.Count} ] 카메라 애니메이션 Killed! ------ 카메라 컨트롤 불가");   
            CurrentScene.cameraController.isControllable = false;
     
        });
       


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