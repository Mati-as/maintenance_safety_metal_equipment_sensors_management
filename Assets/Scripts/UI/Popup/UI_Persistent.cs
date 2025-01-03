using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 훈련 보조도구 및 씬에 관계없이 UI전반을 참조하여 사용하고싶을때 사용
/// 과하게 사용시 의존도가 높아질 수 있으므로 사용주의 필요합니다.
/// </summary>

public class UI_Persistent : UI_Scene
{
    private enum Btns
    {
        Btn_Logo_MenuActivation, // 산기대 대학교 로고
        Btn_Main,
        Btn_Help,
        Btn_FullMenu,
        Btn_Setting,
        Btn_Close,
        Btn_SkipIntro,
      
    }
    


    private enum UI
    {
        Persistent_Menu,  FadeInOutEffect
    }

    private Animator _activationAnimator;
    private readonly int UI_ON = Animator.StringToHash("UI_On");
    private readonly Image[] hoverTexts = new Image[Enum.GetValues(typeof(Btns)).Length];
    private Coroutine _mainIntroCo;
    private Canvas _canvas;
    private Image _fadeEffectImage;
    private bool _introSkipBtnClicked;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        Logger.Log("Main 화면 구성--------------------------");

        gameObject.GetComponent<Canvas>().sortingOrder = 20;
        BindButton(typeof(Btns));
        BindObject(typeof(UI));
        persisten_menu = GetObject((int)UI.Persistent_Menu);
       

        _activationAnimator = GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject.GetComponent<Animator>();
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject
            .BindEvent(() =>
            {
                OnMouseEnterActivationBtn();
            }, Define.UIEvent.PointerEnter);
        
        GetButton((int)Btns.Btn_Logo_MenuActivation).gameObject
            .BindEvent(() =>
            {
                OnMouseExitFromActivationBtn();
            }, Define.UIEvent.PointerExit);

        GetButton((int)Btns.Btn_SkipIntro).gameObject.BindEvent(() =>
        {
            if (_introSkipBtnClicked) return;
            
            StopCoroutine(_mainIntroCo);
            
            _fadeEffectImage.DOFade(1, 1.5f).OnComplete(() =>
            {
                _introSkipBtnClicked = true;
                OnAnimationComplete();
            });
            
        });
        
        SetupButton((int)Btns.Btn_Main, OnMainBtnClicked);
        SetupButton((int)Btns.Btn_Help, OnHelpBtnClicked);
        SetupButton((int)Btns.Btn_FullMenu, OnFullMenuBtnClicked);
        SetupButton((int)Btns.Btn_Setting, OnSettingBtnClicked);
        SetupButton((int)Btns.Btn_Close, OnCloseBtnClicked);

      
        
        
        GetButton((int)Btns.Btn_SkipIntro).gameObject.SetActive(false);

        _fadeEffectImage = GetObject((int)UI.FadeInOutEffect).GetComponent<Image>();
        SetStatus(false);
        return true;
    }

    public static bool persistentMenuStatus
    {
        get;
        private set;
    }
    private static GameObject persisten_menu;

    /// <summary>
    /// 전체부모를 deactivate하면 모든 PopUI가 꺼져버리므로 자식객체 하나만 Off합니다.
    /// </summary>
    public static void SetStatus(bool isOn)
    {
        if (persisten_menu != null)
        {
            persistentMenuStatus = isOn;
            persisten_menu.SetActive(isOn);
        }
    }
    // 버튼 설정을 위한 공통 메서드 입니다.
    private void SetupButton(int btnIndex, Action onClickAction)
    {
        // 클릭 Up 이벤트 설정
        if (!GetButton(btnIndex).IsInteractable())
        {
            Logger.Log($"{(Btns)btnIndex} is not interacable..retrun");
            return;
        }
        
        GetButton(btnIndex).gameObject.BindEvent(onClickAction, Define.UIEvent.PointerUp);


        var buttonTransform = GetButton(btnIndex).transform;
        
        // 텍스트 설정 (자기자신 제외) 
        hoverTexts[btnIndex] = GetButton(btnIndex).GetComponentsInChildren<Image>()
            .FirstOrDefault(image => image.transform != buttonTransform);
        
        
        hoverTexts[btnIndex].gameObject.SetActive(false);

        // 마우스 포인터 이벤트 설정
        GetButton(btnIndex).gameObject.BindEvent(() => { SetHoverText(btnIndex); }, Define.UIEvent.PointerEnter);
        GetButton(btnIndex).gameObject.BindEvent(() => { SetHoverText(btnIndex, false); }, Define.UIEvent.PointerExit);
    }
    #region Mouse Hover Logic

    private void OnMouseEnterActivationBtn()
    {
#if UNITY_EDITOR
        Debug.Log("Hover Animation Activating");
#endif
        _activationAnimator.SetBool(UI_ON, true);
    }

    private void OnMouseExitFromActivationBtn()
    {
        DeactivatePersistentUI();
    }

    #endregion


    #region Btn Methods

    private void OnMainBtnClicked()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(SceneType.Main);
        //if (Managers.UI.SceneUI<UI_Persistent>() == null) Managers.UI.ShowSceneUI<UI_Persistent>();
    }

    private void OnHelpBtnClicked()
    {
        if (!GetButton((int)Btns.Btn_Help).IsInteractable()) return;
        
        if(Managers.UI.FindPopup<UI_Help>() ==null)
            Managers.UI.ShowPopupUI<UI_Help>();
    }

    private void OnFullMenuBtnClicked()
    {
        if(Managers.UI.FindPopup<UI_FullMenu>() ==null)
            Managers.UI.ShowPopupUI<UI_FullMenu>();
     
    }

    private void OnSettingBtnClicked()
    {
        if(Managers.UI.FindPopup<UI_Setting>() ==null) 
            Managers.UI.ShowPopupUI<UI_Setting>();
 
    }


    private void OnCloseBtnClicked()
    {
        Managers.UI.ShowPopupUI<UI_Exit>();
        //  DeactivatePersistentUI();
    }

    #endregion

    public void SetHoverText(int idx, bool isOn = true)
    {
        hoverTexts[idx].gameObject.SetActive(isOn);
    }

    private void DeactivatePersistentUI()
    {
#if UNITY_EDITOR
        Debug.Log("Deactivate PersistentUI");
#endif
        _activationAnimator.SetBool(UI_ON, false);
    }
    
    protected override Button GetButton(int idx)
    {
        if (idx == (int)Btns.Btn_Logo_MenuActivation)  return Get<Button>(idx);
        
        _isScaleEventOn.TryAdd(idx,false);

        if (!_isScaleEventOn[idx])
        {
            var btn = Get<Button>(idx);
            var originalScale = btn.transform.localScale;

            // apply mouse enter scaling
            BindEvent(btn.gameObject, () =>
            {
                if (btn.interactable) btn.transform.DOScale(originalScale * 1.1f, 0.18f);
                //				Logger.Log($"Button Scale Animation Applied: {btn.gameObject.name}");
            }, Define.UIEvent.PointerEnter);

            // apply mouse exit scaling
            BindEvent(btn.gameObject, () =>
                {
                    if (btn.interactable) btn.transform.DOScale(originalScale, 0.15f);
                },
                Define.UIEvent.PointerExit);
        }
	
		
        return Get<Button>(idx);
		
		
    }
    
    

  
    
    protected Animation _mainAnimation;

    public void StopAnim()
    {
        if (_mainAnimation != null)
        {
            _mainAnimation?.Stop();
        }
    }
    public void PlayIntroAndShowAnimOnMainUI(string animPath ="Animation/Intro/Main_Intro" )
    {
        Logger.Log("PlayInto 재생");
        _mainAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();
        
        // 중복애니메이션 클립할당을 위해 추가합니다. 추후 성능이슈가 발생하는경우 로직 수정 필요합니다. 10/17/24
        Debug.Assert(_mainAnimation != null, "Animation component can't be null");

        Logger.Log($"Animation Path {animPath}");
        var clip = Resources.Load<AnimationClip>(animPath);
        
        if (clip == null)
        {
            Logger.LogWarning($"Animation clip at animPath {animPath} not found.");
          
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

        _mainAnimation.Play(clip.name);
        _mainIntroCo = StartCoroutine(CheckAnimationEnd(clip));
        
        
        SetStatus(false);
        GetButton((int)Btns.Btn_SkipIntro).gameObject.SetActive(true);
    }
    
    private IEnumerator CheckAnimationEnd(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        OnAnimationComplete();
    }

    protected virtual void OnAnimationComplete()
    {
        
        if (Managers.initialIntroAnimPlayed)
        {
            GetButton((int)Btns.Btn_SkipIntro).gameObject.SetActive(false);
            return;
        }
        
        Managers.initialIntroAnimPlayed = true;
        
        Logger.Log("Main UI -----------------------------------");
        if (!Managers.UI.FindPopup<UI_Main>()) Managers.UI.ShowPopupUI<UI_Main>();
        GetButton((int)Btns.Btn_SkipIntro).gameObject.SetActive(false);
        
    }

    public void OnMainUIPopUP()
    {
        UI_Persistent.SetStatus(true);
        
        PlayIntroAndShowAnimOnMainUI("Animation/Intro/OnMain");
        GetButton((int)Btns.Btn_SkipIntro).gameObject.SetActive(false);
        if(!Managers.Sound.audioSources[(int)SoundManager.Sound.Bgm].isPlaying) Managers.Sound.Play(SoundManager.Sound.Bgm, "Bgm");
    }
    
    private void Update()
    {
       
        HandleAltEnterPressed();
        
    }
    private void HandleAltEnterPressed()
    {
        // 원하는 로직 수행
        //Debug.Log("Alt+Enter detected via Input System!");
        bool isFullScreenMode = false;
        isFullScreenMode = Screen.fullScreen;
        Managers.Data.Preference[(int)Define.Preferences.Fullscreen] = isFullScreenMode? Define.YES : Define.NO ;
    }

    private Sequence _fadeEffectSeq;
    public void FadeIn(float duration =1.25f)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(1, 0.0001f));
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(0, duration));
    }

    public void FadeOut(float duration =1.5f)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();

        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(0, 0.0001f));
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(1, duration));
    }

    public void FadeOutAndIn()
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(1, 1.35f));
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(0, 1.0f));
        
    }
    
    public void FadeOutAndInForDepth3()
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(1, 0.5f));
        _fadeEffectSeq.Append(_fadeEffectImage.DOFade(0, 0.7f));
        
    }
}