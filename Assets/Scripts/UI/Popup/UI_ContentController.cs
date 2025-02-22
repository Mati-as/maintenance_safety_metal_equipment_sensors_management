using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public enum Btns
{
    Btn_Prev,
    Btn_Next,
    PrevDepth2,
    NextDepth2,
    Btn_Depth1_Title,
    Btn_TopMenu_Hide,
    Btn_Script_Hide,
    Btn_ThirdDepthList_Hide,
    Btn_Help,
    Btn_CameraInit,
    //Btn_Evaluation,
    Btn_ToolBox,
    Btn_Guidebook,
    Btn_TrainingInfo_Confirm,

    //  UI_Depth3_List, // Active Area
    Depth3_A,
    Depth3_B,
    Depth3_C,
    Depth3_D,
    Depth3_E,
    
}

/// <summary>
/// 1.훈련화면상 UI와 메인화면등 기타 UI를 구분하기 위한 클래스입니다.
/// 2.훈련 진행에 필요한 UI를 총괄합니다.
/// 3 크게 사용자입력처리 및 훈련상태를 담당합니다.
/// </summary>
public class UI_ContentController : UI_Popup
{


    public enum Toggles
    {
        Toggle_Depth2_A,
        Toggle_Depth2_B,
        Toggle_Depth2_C,
        Toggle_Depth2_D,
        Toggle_Depth2_E,
        Toggle_Depth2_Count
    }

    public enum UI
    {
        UI_Top,
        UI_TextBox,
       // UI_DepthTitle,
        UI_TrainingInfo,
        UI_Instruction,
        UI_Depth3_List,
        UI_ToolTip,
        UI_ToolBox,
        UI_Checklist,
        
        UI_DrverOnly_GaugeSlider,
        UI_CurrentDepth_Mid,
        ToggleGroup_TopMenuBar,
        FadeInOutEffect
    }


    public enum TMPs
    {
        Text_Depth1_Title,
        Text_Current3Depth,
        Text_Instruction,
        Text_Depth2_A,
        Text_Depth2_B,
        Text_Depth2_C,
        Text_Depth2_D,
        Text_Depth2_E,
        Text_Depth3_A,
        Text_Depth3_B,
        Text_Depth3_C,
        Text_Depth3_D,
        Text_Depth3_E,
        Text_NextDepth2, Text_PrevDepth2
        
    }


    private readonly TextMeshProUGUI[] texts = new TextMeshProUGUI[Enum.GetValues(typeof(TMPs)).Length];

    private Animator animator_depth3Hide;
    private Animator _depthOneTextMoveAnimator;
    private Animator _instructionAnimator;
    private Animator _topMenuAnimator;
    private Animator _instructionFlipAnimator;
    private readonly int UI_ON = Animator.StringToHash("On");
    private readonly int UI_Flip = Animator.StringToHash("Flip");


    private readonly Toggle[] _depth2Toggles = new Toggle[Enum.GetValues(typeof(Toggles)).Length];
    private readonly Button[] _depth3Btns = new Button[Enum.GetValues(typeof(Btns)).Length]; // 방어적으로 사이즈 크게 할당


    private readonly float _btnClickableDelay = 1.55f;
    private WaitForSeconds _waitForClick;
    public bool clickable = true;

    private Inplay_CameraController _currentMainCam;
    private RectTransform _textRectcurrentDepth3;
    public RectTransform textRectcurrentDepth3
    {
        get
        {
            if (_textRectcurrentDepth3 == null)
            {
                _textRectcurrentDepth3 = GetTMP((int)TMPs.Text_Current3Depth).transform.GetComponent<RectTransform>();
            }
            
            return _textRectcurrentDepth3;
        }
       
    }

    private RectTransform _currentDepth3UIRect;
    public RectTransform currentDepth3UIRect
    {
        get
        {
            if (_currentDepth3UIRect == null)
            {
                _currentDepth3UIRect = GetObject((int)UI.UI_CurrentDepth_Mid).transform.GetComponent<RectTransform>();
            }
            
            return _currentDepth3UIRect;
        }
       
    }

    private InputAction _mouseClickAction;

    private Text Text_tooltip { get; set; }
    private Image Text_image { get; set; }

    public Slider UI_DrverOnly_GaugeSlider { get; set; }
    private RectTransform gaugeRectPos { get; set; }
    
    private RectTransform toolTipRectPos { get; set; }
    private readonly Vector3 _toolTipPosOffset = new(55, 55, 0);
    private readonly Vector3 _sliderPosOffset = new(-20, 30, 0);
    public bool isGuageUsable;

    private Dictionary<int, Image> _highlightImageMap;
    private Sequence _blinkBtnSeq;
    
    private bool _isStepChangeByMouseClickForAnimationControl;

    public bool isStepChangeByMouseClickForAnimationControl
    {
        get
        {
            return _isStepChangeByMouseClickForAnimationControl;
        }
        set
        {
            _isStepChangeByMouseClickForAnimationControl = value;
//            Logger.Log($"step chagned by next or prev btn clicked by mosue : {_isStepChangeByMouseClick}");
        }
    }


    // 미션수행 관련 프로퍼티 ------------------------------------------------------------
    private bool _isStepMissionPerformable;
    public bool isStepMissionPerformable {
        get
        {
            return _isStepMissionPerformable;
        } 
        set
        {
            _isStepMissionPerformable = value;
            if (value)
            {
                
               // Logger.Log("해당스텝 미션 수행가능, 드라이버 돌리기 등 미션수행을 통해 다음 스텝으로 넘어갈 수 있습니다. ");
            }
        }
    } 
    
    private bool _isStepMissionComplete;

    public bool isStepMissionComplete {
        get
        {
            return _isStepMissionComplete;
        } 
        set
        {
          
            _isStepMissionComplete = value;
        }
    } // 각 스텝에서 미션수행(도구함클릭, 나사풀기 등) 완료시 true.
    
    //Action<애니메이션 순서(int), Reverse 여부(bool)>
    public static event Action<int,bool> OnStepBtnClicked_CurrentCount;
    public static event Action OnDepth3ClickedAction;
    public static event Action OnDepth2ClickedAction;
    public static event Action<int> OnNextDepthInvoked; //sceneChange 
    public static event Action OnCameraInitBtnClicked;


    private Image _contentControllerFadeEffectImage;
    
    /// <summary>
    /// 1. in tutorial, some Ui objs must be shut.they'll be executed in this logic
    /// </summary>
    public void CheckIfTutorialMode()
    { 
        bool isTutorial = Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Tutorial;
     //   GetObject((int)UI.ToggleGroup_TopMenuBar).gameObject.SetActive(!isTutorial);
      //  GetButton((int)Btns.Btn_TopMenu_Hide).gameObject.SetActive(!isTutorial);
       // GetButton((int)Btns.Btn_Guidebook).gameObject.SetActive(!isTutorial);
    }
    
    public override bool Init()
    {
        if (!base.Init())
            return false;

        
      
       
        this.gameObject.SetActive(true);
        
        SoundManager.OnNarrationComplete -= BlinkNextBtnUI;
        SoundManager.OnNarrationComplete += BlinkNextBtnUI;
        
#if UNITY_EDITOR
        Dev_CurrentSceneInformation.OnNextClickedOnDev -= OnNextBtnClicked;
        Dev_CurrentSceneInformation.OnPrevClickedOnDev -= OnPrevBtnClicked;
        Dev_CurrentSceneInformation.OnNextClickedOnDev += OnNextBtnClicked;
        Dev_CurrentSceneInformation.OnPrevClickedOnDev += OnPrevBtnClicked;
        
#endif 

        _currentMainCam =Camera.main.GetComponent<Inplay_CameraController>();
        
        
        BindUIElements();
        InitTopMenu();
        InitDepth2Toggles();
        InitDepth3Buttons();
        InitCommonUI();
        SetInitialStates();
        Refresh();
        SetBtns();
        SetHeighlightUIImages();

        HideCamInitBtn();
        
        
        if (_uiTrainingInfo == null)
        {
            var obj = GetObject((int)UI.UI_TrainingInfo);
            if (obj != null)
            {
                obj.TryGetComponent(out _uiTrainingInfo);
            }
            else
            {
                Debug.LogError("UI_TrainingInfo object not found.");
            }
        }

        OnSensorOverall();
        EmptyInstructionTextBox();
        
        
        
        //CheckIfTutorialMode();

        _contentControllerFadeEffectImage = GetObject((int)UI.FadeInOutEffect).GetComponent<Image>();
        FadeOutAndIn(0.25f,0.8f);
        if(Managers.UI_Persistent!=null)Managers.UI_Persistent.FadeIn();
        
        SetInstructionShowOrHideStatus(true);
        return true;
    }

    private void EmptyInstructionTextBox()
    {
        texts[(int)TMPs.Text_Instruction].text = string.Empty;
    }
    private void OnDestroy()
    {
        
#if UNITY_EDITOR
        Dev_CurrentSceneInformation.OnNextClickedOnDev -= OnNextBtnClicked;
        Dev_CurrentSceneInformation.OnPrevClickedOnDev -= OnPrevBtnClicked;
#endif 
         SoundManager.OnNarrationComplete -= BlinkNextBtnUI;
    }

    private void OnSensorOverall()
    {
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.SensorOverview)
        {
            HideToolBoxBtn();
        }
        else
        {
            ShowToolBoxAndGuideBookBtn();
        }

    }

    private void BlinkNextBtnUI()
    {
        Logger.Log("next btn click blink -------------------------------");
        BlinkBtnUI((int)Btns.Btn_Next);
    }
    /// <summary>
    /// 하이라이트 기능이 있는 객체는 highlight 이미지를 할당해놓고, 아래 메소드에서 직접 초기화합니다.
    /// </summary>
    private void SetHeighlightUIImages()
    {
        if (_highlightImageMap == null) _highlightImageMap = new Dictionary<int, Image>();
        
        
        Image GuidebookHlImage = GetButton((int)(Btns.Btn_Guidebook))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_Guidebook, GuidebookHlImage);
        
        Image ToolboxImage = GetButton((int)(Btns.Btn_ToolBox))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_ToolBox, ToolboxImage);
        
        Image NextBtnImage = GetButton((int)(Btns.Btn_Next))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_Next, NextBtnImage);
        
        
        Image PrevBtnImage = GetButton((int)(Btns.Btn_Prev))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_Prev, PrevBtnImage);
        
        Image cameraInitbtn = GetButton((int)(Btns.Btn_CameraInit))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_CameraInit, cameraInitbtn);
    }



    public void BlinkBtnUI(int btnEnum)
    {
        
        if(btnEnum == (int)Btns.Btn_Next)
        {
            Logger.Log("현재 다음 버튼 하이라이트 로직 미구현 상태 11/18/24-------------------------");
            return;
        }
        _blinkBtnSeq?.Kill();
        _blinkBtnSeq = DOTween.Sequence();
        for (int i = 0; i < 5; i++)
        {
            _blinkBtnSeq.Append(_highlightImageMap[btnEnum].DOFade(1, 0.25f));
            _blinkBtnSeq.AppendInterval(0.3f);
            _blinkBtnSeq.Append(_highlightImageMap[btnEnum].DOFade(0, 0.25f));
            _blinkBtnSeq.AppendInterval(0.3f);
        }

        _blinkBtnSeq.OnKill(() =>
        {
            _highlightImageMap[btnEnum].DOFade(0, 0);
        });
        _blinkBtnSeq.Play();
    }

    public void StopBtnUIBlink()
    {
        _blinkBtnSeq?.Kill();
        _blinkBtnSeq = DOTween.Sequence();
    }


    private void BindUIElements()
    {
        BindButton(typeof(Btns));
        BindToggle(typeof(Toggles));
        BindObject(typeof(UI));
        BindTMP(typeof(TMPs));
        
        UI_DrverOnly_GaugeSlider = GetObject((int)UI.UI_DrverOnly_GaugeSlider).GetComponent<Slider>();
        UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);

    }



    protected virtual void Update()
    {
        Update_MousePosition();
    }


    protected virtual void Update_MousePosition()
    {
     
        
        Vector2 mousePos = Input.mousePosition + _toolTipPosOffset;
        Vector2 sliderPos = Input.mousePosition + _sliderPosOffset;
        
        if (toolTipRectPos == null)
        {
            toolTipRectPos = GetObject((int)UI.UI_ToolTip).GetComponent<RectTransform>();
            Logger.Log("get tooltip rectpos");
        }

        if (gaugeRectPos == null)
        {
           
            gaugeRectPos = GetObject((int)UI.UI_DrverOnly_GaugeSlider).GetComponent<RectTransform>();
//            Logger.Log("get tooltip rectpos");
        }


        
        toolTipRectPos.position = mousePos;
        gaugeRectPos.position = sliderPos;
    }

    public void SetToolTipText(string text = null)
    {
        SetToolTipActiveStatus();
        if (Text_tooltip == null)
        {
            Text_tooltip = GetObject((int)UI.UI_ToolTip).GetComponentInChildren<Text>();
            Text_image = GetObject((int)UI.UI_ToolTip).GetComponentInChildren<Image>();
//            Logger.Log("get tooltip text");
        }

        Text_tooltip.text = text;
        var rect_1 = (RectTransform)Text_image.transform;
        rect_1.sizeDelta = new Vector2(text.Length * 30, 50);
        var rect_2 = (RectTransform)Text_tooltip.transform;
        rect_2.sizeDelta = new Vector2(text.Length * 30, 50);
    }

    public void SetToolTipActiveStatus(bool isOn = true)
    {
        GetObject((int)UI.UI_ToolTip).SetActive(isOn);
    }

    private void InitTopMenu()
    {
        _depthOneTextMoveAnimator = GetButton((int)Btns.Btn_Depth1_Title).gameObject.GetComponent<Animator>();
        _depthOneTextMoveAnimator.enabled = false;


        animator_depth3Hide = GetObject((int)UI.UI_CurrentDepth_Mid).gameObject.GetComponent<Animator>();
        BindHoverEventToButton(Btns.Btn_Depth1_Title, OnDepthOneTitleHover, OnDepthOneTitleHoverExit);
        _topMenuAnimator = GetObject((int)UI.UI_Top).gameObject.GetComponent<Animator>();
        _btn_TopMenu_Hide_animator =GetObject((int)UI.UI_Top).GetComponent<Animator>(); 
        _topMenuAnimator.SetBool(UI_ON, true);
        GetButton((int)Btns.Btn_TopMenu_Hide).gameObject.BindEvent(OnTopMenuAnimBtnClicked);
    }


    private void InitDepth2Toggles()
    {
        for (var i = (int)Toggles.Toggle_Depth2_A; i < (int)Toggles.Toggle_Depth2_Count; i++)
        {
            Logger.Log($"Depth2 Banner Toggled {i}");
            var toggle = GetToggle((int)Toggles.Toggle_Depth2_A + i);

            if (toggle == null)
            {
                Logger.LogWarning($"Depth 2 Btn {i} is null");
                continue;
            }

            var toggleIndex = i;

            if (GetToggle((int)Toggles.Toggle_Depth2_A + i).interactable)
            {
                // 구현된 부분만 활성화 및 클릭상태가 될 수 있도록될 수 있도록 구성 ----------------------------------------------
                if ((Managers.ContentInfo.PlayData.Depth1 == 3 && i == (int)Toggles.Toggle_Depth2_A) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 3 && i == (int)Toggles.Toggle_Depth2_B) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 3 && i == (int)Toggles.Toggle_Depth2_C) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 1 && i == (int)Toggles.Toggle_Depth2_A) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 1 && i == (int)Toggles.Toggle_Depth2_B) ||
                    
                    (Managers.ContentInfo.PlayData.Depth1 == 2 && i == (int)Toggles.Toggle_Depth2_A) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 2 && i == (int)Toggles.Toggle_Depth2_B) ||
                    (Managers.ContentInfo.PlayData.Depth1 == 2 && i == (int)Toggles.Toggle_Depth2_C) ||
                    
                    (Managers.ContentInfo.PlayData.Depth1 == 4 && i == (int)Toggles.Toggle_Depth2_B)
                   )
                {
                    SwitchDepthToggle();
              
                    GetToggle((int)Toggles.Toggle_Depth2_A + i).interactable = true;

                    toggle.gameObject.BindEvent(() =>
                    {
                        OnDepth2ClickedIncludingSceneChange(toggleIndex + 1); // use the local copy
                    });
                    Logger.Log($"Depth{Managers.ContentInfo.PlayData.Depth3} 부분 구현 상태..{(Toggles)i} 활성화");
                }
                else
                {
                    GetToggle((int)Toggles.Toggle_Depth2_A + i).interactable = false;
                }
            }

            _depth2Toggles[(int)Toggles.Toggle_Depth2_A + i] = toggle;
        }


    }

    private void SwitchDepthToggle()
    {
        if (Managers.ContentInfo.PlayData.Depth2 == 1) GetToggle((int)Toggles.Toggle_Depth2_A).isOn = true;
        if (Managers.ContentInfo.PlayData.Depth2 == 2) GetToggle((int)Toggles.Toggle_Depth2_B).isOn = true;
        if (Managers.ContentInfo.PlayData.Depth2 == 3) GetToggle((int)Toggles.Toggle_Depth2_C).isOn = true;
        if (Managers.ContentInfo.PlayData.Depth2 == 4) GetToggle((int)Toggles.Toggle_Depth2_D).isOn = true;

    }
    private void InitDepth3Buttons()
    {
        for (var i = 0; i < 5; i++)
        {
            var btn = GetButton((int)Btns.Depth3_A + i);
            var i2 = i;
            btn.gameObject.BindEvent(() =>
            {
                Logger.Log($"{btn.gameObject.name} clicked depth value = {i2+1}");
                OnDepth3BtnClicked(i2 + 1);
            });
            btn.gameObject.BindEvent(OnDepth3BtnEnter, Define.UIEvent.PointerEnter);
            btn.gameObject.BindEvent(OnDepth3ABtnExit, Define.UIEvent.PointerExit);
            _depth3Btns[(int)Btns.Depth3_A + i] = btn;
        }
    }

    public void SetCamInitBtnStatus(bool isOn =true)
    {
        GetButton((int)Btns.Btn_CameraInit).gameObject.SetActive(isOn);
        Logger.Log($"카메라 초기화 버튼 활성화?:----- {isOn}");
    }

    public void HideCamInitBtn()
    {
        GetButton((int)Btns.Btn_CameraInit).gameObject.SetActive(false);
    }
    public void ShowToolBoxAndGuideBookBtn()
    {
        GetButton((int)Btns.Btn_ToolBox).gameObject.SetActive(true);
    }

    
    public void HideToolBoxBtn()
    {
        GetButton((int)Btns.Btn_Guidebook).gameObject.SetActive(false);
        GetButton((int)Btns.Btn_ToolBox).gameObject.SetActive(false);
    }

    private void OnDepth3BtnEnter()
    {
        
    }

    private void InitCommonUI()
    {
        
      GetButton((int)Btns.Btn_CameraInit).gameObject
        .BindEvent(() =>
        {
            _currentMainCam.RefreshRotationAndZoom();
            OnCameraInitBtnClicked?.Invoke();

        }, Define.UIEvent.PointerUp);
    
        GetButton((int)Btns.Btn_ThirdDepthList_Hide).gameObject
            .BindEvent(OnDepthThirdHideBtnClicked, Define.UIEvent.PointerDown);
        
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
        InitInstructionSection();
    
    }

    private void InitInstructionSection()
    {
        _instructionAnimator = GetObject((int)UI.UI_Instruction).GetComponent<Animator>();
        _instructionFlipAnimator = GetObject((int)UI.UI_Instruction).transform.GetChild(0).GetComponent<Animator>();

        GetButton((int)Btns.Btn_Script_Hide).gameObject.BindEvent(OnInstructionHideClicked);
        GetObject((int)UI.UI_Instruction).GetComponent<Text>();
    }

    public void SetInstructionShowOrHideStatus(bool active = true)
    {
        _instructionAnimator.SetBool(UI_ON, active);
    }


    private void BindPointerEventToObject(UI uiElement, Action eventAction)
    {
        GetObject((int)uiElement).BindEvent(eventAction, Define.UIEvent.PointerEnter);
    }

    private void BindHoverEventToButton(Btns btnType, Action onEnter, Action onExit)
    {
        GetButton((int)btnType).gameObject.BindEvent(onEnter, Define.UIEvent.PointerEnter);
        GetButton((int)btnType).gameObject.BindEvent(onExit, Define.UIEvent.PointerExit);
    }

    private void SetInitialStates()
    {
        _heightPerDepth3Btn = GetButton((int)Btns.Depth3_A).GetComponent<RectTransform>().sizeDelta.y;
     //   _activeAreaRect = GetObject((int)UI.ActiveArea).GetComponent<RectTransform>();
        for (var i = 0; i < texts.Length; i++) texts[i] = GetTMP(i);

        
        //GetToggle((int)Toggles.Toggle_Depth2_B).isOn = true;
        GetObject((int)UI.UI_ToolTip).SetActive(false);
        SetNextPrevBtnsActiveStatus(false);
    }




    private void RefreshText()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        texts[(int)TMPs.Text_Depth1_Title].text = Managers.Data.Texts[int.Parse(depth1 + "0000")].kor;
        var depth1Int = Managers.ContentInfo.PlayData.Depth1;

        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        var depth2Int = Managers.ContentInfo.PlayData.Depth2;
        // Debug.Log($"depth 2 count : {ContentPlayData.DEPTH_TWO_COUNT_DATA[depth1Int]}");
        //Debug.Log($"depth 3 count : {ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(depth1+depth2)]}");

        var depth2Count = 1;
        for (var i = (int)TMPs.Text_Depth2_A;
             i < (int)TMPs.Text_Depth2_A + ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[depth1Int];
             i++)
        {
            texts[i].text = Managers.Data.Texts[int.Parse(depth1 + "00" + depth2Count + "0")].kor;
            depth2Count++;
            //Debug.Log($"Depth2 텍스트 변환 완료 :{ texts[i].gameObject.name} : { texts[i].text}");
        }


        var depth3Count = 1;
        for (var i = (int)TMPs.Text_Depth3_A;
             i < (int)TMPs.Text_Depth3_A + ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(depth1 + depth2)];
             i++)
        {
            texts[i].text = Managers.Data.Texts[int.Parse(depth1 + "00" + depth2 + depth3Count)].kor;
            depth3Count++;
            // Debug.Log($"Depth3 텍스트 변환 완료 : { texts[i].text}");
        }


        //prev next depth3 refresh----------------------------------------------
        var currentDepth2 = Managers.ContentInfo.PlayData.Depth2;

        var prevDepth2 = currentDepth2 - 1;
        var processedPrevDepth2 = int.Parse(depth1 + "00" + prevDepth2 + "0");

        var nextDepth2 = currentDepth2 + 1;
        var processedNextDepth2 = int.Parse(depth1 + "00" + nextDepth2 + "0");
        
     
            GetTMP((int)TMPs.Text_PrevDepth2).text = String.Empty;
            GetTMP((int)TMPs.Text_NextDepth2).text = String.Empty;

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.SensorOverview)
        {
            if (Managers.ContentInfo.PlayData.Depth2 == 1)
            {
                    GetTMP((int)TMPs.Text_PrevDepth2).text = string.Empty;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[processedNextDepth2].kor;
            }
            else if (Managers.ContentInfo.PlayData.Depth2 ==
                    ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[(int)Define.Depth.SensorOverview])
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[processedPrevDepth2].kor;
                if (Managers.Data.Texts.ContainsKey(20010))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[20010].kor;
            }
            else
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[processedPrevDepth2].kor;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[processedNextDepth2].kor;
            }
        }

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Safety)
        {
            if (Managers.ContentInfo.PlayData.Depth2 == 1)
            {
                if (Managers.Data.Texts.ContainsKey(10020))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[10020].kor;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[processedNextDepth2].kor;
            }
            else if (Managers.ContentInfo.PlayData.Depth2 ==
                     ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[(int)Define.Depth.Safety])
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[processedPrevDepth2].kor;
                if (Managers.Data.Texts.ContainsKey(30010))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[30010].kor;
            }
            else
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text =
                        Managers.Data.Texts[processedPrevDepth2].kor;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text =
                        Managers.Data.Texts[processedNextDepth2].kor;
            }
        }

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.MaintenancePractice)
        {
            if (Managers.ContentInfo.PlayData.Depth2 == 1)
            {
                if (Managers.Data.Texts.ContainsKey(20030))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[20030].kor;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[processedNextDepth2].kor;
            }
            else if (Managers.ContentInfo.PlayData.Depth2 ==
                     ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[(int)Define.Depth.MaintenancePractice])
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[processedPrevDepth2].kor;
                GetTMP((int)TMPs.Text_NextDepth2).text = string.Empty; //평가하기?
            }
            else
            {
                if (Managers.Data.Texts.ContainsKey(processedPrevDepth2))
                    GetTMP((int)TMPs.Text_PrevDepth2).text = Managers.Data.Texts[processedPrevDepth2].kor;
                if (Managers.Data.Texts.ContainsKey(processedNextDepth2))
                    GetTMP((int)TMPs.Text_NextDepth2).text = Managers.Data.Texts[processedNextDepth2].kor;
            }
        }
          

     

        

        
         
        //current  depth3 refresh----------------------------------------------
        texts[(int)TMPs.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + Managers.ContentInfo.PlayData.CurrentDepthStatus[2])].kor;

        Logger.Log($"depth3 문구 변경 현재 뎁스인포 :{int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] +"00"+ Managers.ContentInfo.PlayData.Depth2 + Managers.ContentInfo.PlayData.Depth3)}");


        string text = texts[(int)TMPs.Text_Current3Depth].text;
        int charCountWithoutSpaces = text.Replace(" ", "").Length;
        
        
        textRectcurrentDepth3.sizeDelta = new Vector2(10f + (charCountWithoutSpaces * 32f), // new width
            textRectcurrentDepth3.sizeDelta.y); 
        
     
       

        // set sizeDelta based on non-space characters
        currentDepth3UIRect.sizeDelta = new Vector2(
            65f + (charCountWithoutSpaces * 35f), // width based on non-space characters
            currentDepth3UIRect.sizeDelta.y);
        
      Logger.Log($"{texts[(int)TMPs.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.Depth2 + Managers.ContentInfo.PlayData.Depth3)].kor}");
      
    //    Logger.Log($"Current Depth Size delta = {currentDepth3UIRect.sizeDelta.x}");
    }

 
    public UI_ToolBox uiToolBox
    {
        get
        {
            return GetObject((int)UI.UI_ToolBox).GetComponent<UI_ToolBox>();
        }
        private set
        {
            uiToolBox = GetObject((int)UI.UI_ToolBox).GetComponent<UI_ToolBox>();
        }
    }
    
    public UI_Checklist uiChecklist
    {
        get
        {
            return GetObject((int)UI.UI_Checklist).GetComponent<UI_Checklist>();
        }
    }
    
    private void SetBtns()
    {
        GetButton((int)Btns.Btn_Prev).gameObject.BindEvent(OnPrevBtnClicked);
        GetButton((int)Btns.Btn_Next).gameObject.BindEvent(OnNextBtnClicked);
        GetButton((int)Btns.Btn_ToolBox).gameObject.BindEvent(() =>
        { 
            if (uiToolBox == null) uiToolBox = GetObject((int)UI.UI_ToolBox).GetComponent<UI_ToolBox>();
            GetObject((int)UI.UI_ToolBox).SetActive(true);
           
            uiToolBox.SetToolBox();
            //Managers.UI.ShowPopupUI<UI_ToolBox>();
        });

        GetButton((int)Btns.PrevDepth2).gameObject.BindEvent(() =>
        {
            LoadStep(Managers.ContentInfo.PlayData.Depth2 - 1);
        });
        GetButton((int)Btns.NextDepth2).gameObject.BindEvent(() =>
        {
            LoadStep(Managers.ContentInfo.PlayData.Depth2 + 1);
        });
        
        GetButton((int)Btns. Btn_TrainingInfo_Confirm).gameObject.BindEvent(() =>
        {
            ShutTrainingIntroAnim();
            SetInstructionShowOrHideStatus();

            
            Managers.ContentInfo.PlayData.Count = 1;
            OnStepBtnClicked_CurrentCount?.Invoke(1,false);
 
        });
       
    }


    public void SetNextPrevBtnsActiveStatus(bool isOn = true)
    {
        // depth4의 평가하기에서, 인트로 이외에는 버튼 표시가 진행되지 않음


        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation)
        {
            // 아래 부분은 씬로드 및 초기화 부분에만 사용됨에 주의
            if (Managers.ContentInfo.PlayData.Count <= 1 )
            {
  
                SetButtonState((int)Btns.Btn_Next, true);
                SetButtonState((int)Btns.Btn_Prev, false);
            }
            else
            {
                SetButtonState((int)Btns.Btn_Next, false);
                SetButtonState((int)Btns.Btn_Prev, false);
            }
        }
        else
        {
            if (Managers.ContentInfo.PlayData.Count <= 1)
            {
                Logger.Log("count is less than 1. show only next btn");
                SetButtonState((int)Btns.Btn_Prev, false);
                SetButtonState((int)Btns.Btn_Next, true); 
            }
            else
            {
             //   Logger.Log("show all navigate pre,next btn");
                SetButtonState((int)Btns.Btn_Prev, true);
                SetButtonState((int)Btns.Btn_Next, true); 
            }
      
        }
   

    }


    private Dictionary <int,Sequence> _btnSeq = new Dictionary<int, Sequence>();
    private Dictionary <int,bool> _btnStatusMap = new Dictionary<int, bool>();
    private void SetButtonState(int buttonIndex, bool isOn)
    {
        _btnStatusMap.TryAdd(buttonIndex, true);
        if (_btnStatusMap[buttonIndex] == isOn)
        {
//            Logger.Log("Button is already " + (isOn ? "On" : "Off"));
            return;
        }
        
        _btnStatusMap[buttonIndex] = isOn;
     
        
        var button = GetButton(buttonIndex);
        button.enabled = isOn;
        button.interactable= isOn;
        
        _btnSeq.TryAdd(buttonIndex, DOTween.Sequence());

        _btnSeq[buttonIndex]?.Kill();
        _btnSeq[buttonIndex] = DOTween.Sequence();
        
        button.enabled = isOn;

        float fade = isOn ? 1 : 0;
        float speed = isOn ? 0.55f : 0.55f;
        float scale = isOn ? 1 : 0;

        //초기값구성
        float initialFade = isOn ? 0 : 1;
        _btnSeq[buttonIndex].Append(button.GetComponent<Image>().DOFade(initialFade, 0.001f));
        
        _btnSeq[buttonIndex].AppendCallback(() => { button.gameObject.SetActive(true); });
        _btnSeq[buttonIndex].AppendCallback(() => { button.gameObject.transform.localScale = Vector3.one * scale; });
        _btnSeq[buttonIndex].Append(button.GetComponent<Image>().DOFade(fade, speed));
        _btnSeq[buttonIndex].AppendCallback(() => { button.gameObject.SetActive(isOn); });
        _btnSeq[buttonIndex].Play();
    }

    


    
    private void OnPrevBtnClicked()
    {
        Precheck();

        Debug.Assert(Managers.ContentInfo.PlayData.Count > 0);

        if (Managers.ContentInfo.PlayData.Count <= 1)
        {

            Logger.Log("currentCount is 1, Start Point");

            return;
        }

        Managers.ContentInfo.PlayData.Count--;

        isStepChangeByMouseClickForAnimationControl = true;
        if(hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        if(!_currentMainCam.isControllable) HideCamInitBtn();

        SetDepthThirdHideBtnStatus(false);
        
        
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,true);
        
     
       
    }


    public void InvokeNextStepByMissionComplete()
    {
    
        
        isStepChangeByMouseClickForAnimationControl = false;
        
        Precheck();//
        SetScriptUI();
        CheckAndGoToNextStep();
        
        Logger.Log($"스텝별 수행 미션 완료, 다음 스크립트 및 애니메이션 재생----> {Managers.ContentInfo.PlayData.Count}");

    }

    private void CheckAndGoToNextStep()
    {
        if (Managers.ContentInfo.PlayData.Count >= ContentPlayData.CurrentCountMax)
        {
            Logger.Log("current count is Max ---------");
            return;
        }

        if (Managers.ContentInfo.PlayData.Count >= ContentPlayData.CurrentCountMax)
        {
            //depth2 invoke로직 or 버튼에 직접할당하기
        }

        SetDepthThirdHideBtnStatus(false);
        
        Managers.ContentInfo.PlayData.Count++;
      
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,false);
        if(!_currentMainCam.isControllable) HideCamInitBtn();
    }
    /// <summary>
    /// 버튼 클릭외에 스크립트를 넘어가는 경우 (예. 미션수행완료 등) 사용
    /// </summary>

    private void OnNextBtnClicked()
    {
        
        isStepChangeByMouseClickForAnimationControl = true;
        isStepMissionComplete = false;
        Precheck();
        SetScriptUI();
        CheckAndGoToNextStep();
       // Logger.Log($"Nxt Btn Clicked By Mouse ---> {Managers.ContentInfo.PlayData.Count}");
    }




    public void ChangeInstructionText()
    {
        this.gameObject.SetActive(true); 
        StartCoroutine(ChangeTextWithAnimCo());
    }

    private WaitForSeconds animDelay;

    private IEnumerator ChangeTextWithAnimCo()
    {
        _instructionFlipAnimator.SetTrigger(UI_Flip);
        if (animDelay == null) animDelay = new WaitForSeconds(0.2f);
        yield return animDelay;

        if (Managers.ContentInfo.PlayData.Count ==0)
        {
//            Logger.Log("Count is zero.. intro animation playing........");
            yield break;
        }
        
        if(!Managers.Data.Texts.ContainsKey(int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)))
        {
            Logger.LogWarning("해당 스크립트 없음. 공백으로 초기화. 스크립트 누락가능성 있음.");
            EmptyInstructionTextBox();
            yield break;;
        }
        
        texts[(int)TMPs.Text_Instruction].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)].kor;
    }


    /// <summary>
    /// 사용자가 UI_ContentController 와상호작용할때만 동작합니다. 
    /// </summary>
    /// <param name="depth2"></param>
    private void OnDepth2ClickedIncludingSceneChange(int depth2)
    {
        Precheck();
        
        //Depth1이 실습단계인 경우 (중복방지 로직포함)-----------------------------------------
        if (Managers.ContentInfo.PlayData.Depth2 != depth2 &&Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.MaintenancePractice)
        {
          
            Managers.ContentInfo.PlayData.Depth2 = depth2;
            Managers.ContentInfo.PlayData.Depth3 = 1;
            Managers.ContentInfo.PlayData.Count = 0;
            
            
            Logger.Log($"Scene Changed : {"DepthC" + Managers.ContentInfo.PlayData.Depth2.ToString()}");
            
            SceneManager.LoadScene("DepthC" + Managers.ContentInfo.PlayData.Depth2.ToString() );
            return;
        }

        
        //Depth1이 실습단계가 아닌 경우-----------------------------------------
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1) PlayTrainingGoalAnim(); // Depth3가 첫번쨰인경우만 훈련목표 재생(Depth1예외)
        
        Managers.ContentInfo.PlayData.Depth2 = depth2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;
        SwitchDepthToggle();


        //각 뎁스의 첫번쨰 애니메이션을 재생하도록 하기위한 로직
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count, false);
        if(!_currentMainCam.isControllable) HideCamInitBtn();
        
        Logger.Log($"Depth2 Banner Toggled {depth2}");
        
        
        ChangeInstructionText();
       
        RefreshUI();
        RefreshText();
        
     


        OnDepth2ClickedAction?.Invoke();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="depth2"></param>
    /// <param name="depth3"> depth3는 기본 1이나 개발용인 경우에 parameter를 다르게 사용가능 (ex. 3.3.2 개발중인경우, depth3를 2로 설정</param>
    public void OnDepth2Init(int depth2,int depth3 =1)
    {
        Precheck();
        
        HideCamInitBtn(); 
        

        //Depth1이 실습단계가 아닌 경우-----------------------------------------
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1) PlayTrainingGoalAnim(); 
        
        Managers.ContentInfo.PlayData.Depth2 = depth2;
        Managers.ContentInfo.PlayData.Depth3 = depth3;
        Managers.ContentInfo.PlayData.Count = 0;
        SwitchDepthToggle();


        //각 뎁스의 첫번쨰 애니메이션을 재생하도록 하기위한 로직
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count, false);
        if(!_currentMainCam.isControllable) HideCamInitBtn();
        
        Logger.Log($"Depth2 Banner Toggled {depth2}");
        
        
        ChangeInstructionText();
        RefreshUI();
        RefreshText();


        OnDepth2ClickedAction?.Invoke();
    }


   // private RectTransform _activeAreaRect;
    private float _heightPerDepth3Btn;

    private void RefreshUI()
    {
        Debug.Log($"Refreshing UI : CurrentStatus: {Managers.ContentInfo.PlayData.CurrentDepthStatus}");


        //1.모든 UI 먼저 비활성화  합니다. 
        for (var i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_E + 1; i++) _depth3Btns[i].gameObject.SetActive(false);
        for (var i = (int)Toggles.Toggle_Depth2_A; i < (int)Toggles.Toggle_Depth2_E + 1; i++)
            _depth2Toggles[i].gameObject.SetActive(false);

        //2. 활성화될 UI만 다시 활성화 합니다.
        for (var i = (int)Toggles.Toggle_Depth2_A; i < (int)Toggles.Toggle_Depth2_A + ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[
                 int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0].ToString())];
             i++)
            _depth2Toggles[i].gameObject.SetActive(true);

        var currentDepth12 = Managers.ContentInfo.PlayData.CurrentDepthStatus[0] +
                             Managers.ContentInfo.PlayData.CurrentDepthStatus[1].ToString();

        for (var i = (int)Btns.Depth3_A;
             i < (int)Btns.Depth3_A + ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)];
             i++) _depth3Btns[i].gameObject.SetActive(true);


        // ActiveArea의 사이즈를 재조정합니다. 
      //  var resizedHeight = new Vector2(_activeAreaRect.sizeDelta.x, _heightPerDepth3Btn * 1.45f *ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)] + 1);
      //  _activeAreaRect.sizeDelta = resizedHeight;
//        Debug.Log($"_activeAreaRect Height: {_activeAreaRect.sizeDelta.y}");
    }


    /// <summary>
    ///     Text,UI  total Refresh
    /// </summary>
    public void Refresh()
    {
        RefreshUI();
        RefreshText();
      
        if(!Managers.Data.Texts.ContainsKey(int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)))
        {
            Logger.LogWarning("해당 스크립트 없음. 스크립트 누락가능성 있음.");
            return;
        }
        texts[(int)TMPs.Text_Instruction].text =
           Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)].kor;
    }

    private void OnDepthOneTitleHover()
    {
        Precheck();
        _depthOneTextMoveAnimator.enabled = true;
    }

    private void OnDepthOneTitleHoverExit()
    {
        Precheck();
        _depthOneTextMoveAnimator.Rebind();
        _depthOneTextMoveAnimator.enabled = false;
    }


    private void SetClickable()
    {
        StartCoroutine(SetClickableCo());
    }

    private IEnumerator SetClickableCo()
    {
        clickable = false;
        if (_waitForClick == null) _waitForClick = new WaitForSeconds(_btnClickableDelay);
        yield return _waitForClick;
        clickable = true;
    }


    private bool _isdepth3ListOn = false;

    private void OnDepthThirdHideBtnClicked()
    {
        _isdepth3ListOn = !_isdepth3ListOn;
        
        animator_depth3Hide.SetBool(UI_ON,_isdepth3ListOn);
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(true);
    }
    
    private void SetDepthThirdHideBtnStatus(bool isOn)
    {
        _isdepth3ListOn = isOn;
        
        animator_depth3Hide.SetBool(UI_ON,isOn);
       // GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(isOn);
    }

    private bool isOnActiveArea; // 뎁스3내용 Hover시 표출. 표출이후에도 내용범위에 머물러있으면 Hover상태 유지
    private bool isOn3depthArea;


    
    public void OnDepth3BtnClicked(int depth3Num)
    {
        Precheck();
        HideCamInitBtn();      
        Managers.ContentInfo.PlayData.Depth3 = depth3Num;
        Logger.Log($"current Status{Managers.ContentInfo.PlayData.CurrentDepthStatus}");


 

        for (var i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_E + 1; i++)
        {
            _depth3Btns[i].gameObject.SetActive(false);
        }


        var currentDepth12 = Managers.ContentInfo.PlayData.CurrentDepthStatus[0] +
                             Managers.ContentInfo.PlayData.CurrentDepthStatus[1].ToString();
                             
        Logger.Log($"depthCount {currentDepth12}");

        for (var i = (int)Btns.Depth3_A;
             i < (int)Btns.Depth3_A + ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)];
             i++)
        {
            _depth3Btns[i].gameObject.SetActive(true);
        }
        
        // Logger.Log($"depth 3 current: {texts[(int)TMPs.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");
        //
        //
        // Managers.ContentInfo.PlayData.Depth3 = depth3Num;
        // texts[(int)TMPs.Text_Current3Depth].text =
        //     Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
        //         + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + depth3Num)].kor;

        Logger.Log($"depth3 문구 변경 현재 뎁스인포 :{int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"+ Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + depth3Num)} ");
                   
        
        
        Managers.ContentInfo.PlayData.Count = 1;


        if (Managers.ContentInfo.PlayData.Depth3 == 1)
        {
            animator_depth3Hide.SetBool(UI_ON,false);
            PlayTrainingGoalAnim();
        }
        
        if (Managers.ContentInfo.PlayData.Depth3 != 1)
        {
   
            if(_hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        }
        
        Refresh();
        ChangeInstructionText();
        
        SetInstructionShowOrHideStatus(true);
        FadeOutAndIn(0.3f,0.8f);
        OnDepth3ClickedAction?.Invoke();
        
        
    }


    private void Depth3TextChange()
    {
        
    }

    private void OnDepth3ABtnExit()
    {
        isOn3depthArea = false;
        //Animatiorn
    }

    private void Precheck()
    {
        
        //SetNextPrevBtnsActiveStatus();
        
        if (!clickable)
        {
            Logger.Log("Clicking Too Fast");
            return;
        }
        
      

        SetClickable();
    }

    private bool _hideBtn_isInstructionViewActive=true;

    public bool hideBtn_isInstructionViewActive
    {
        get
        {
            return _hideBtn_isInstructionViewActive;
        }
        private set
        {
            _hideBtn_isInstructionViewActive = value;
        }
    }

    private bool _isTopMenuOn = true;

    private void OnInstructionHideClicked()
    {
        Precheck();
        hideBtn_isInstructionViewActive = !hideBtn_isInstructionViewActive;
        _instructionAnimator.SetBool(UI_ON, hideBtn_isInstructionViewActive);

        Logger.Log($" Current Script UI Status{hideBtn_isInstructionViewActive}");
    }

    public void SetScriptUI()
    {
        if(hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        
    }

    
    private Animator _btn_TopMenu_Hide_animator;
    private void OnTopMenuAnimBtnClicked()
    {
        Precheck();
        _isTopMenuOn = !_isTopMenuOn;
        _topMenuAnimator.SetBool(UI_ON, _isTopMenuOn);
        _btn_TopMenu_Hide_animator.SetBool(UI_ON, _isTopMenuOn);
        
        if(!_isTopMenuOn)animator_depth3Hide.SetBool(UI_ON,false);

        Logger.Log($" topMenu Status: {_isTopMenuOn}");
    }


    private bool isTrainingInfoOpen;
    private Sequence UI_AnimSeq;




    private UI_TrainingInfo _uiTrainingInfo;
    public void PlayTrainingGoalAnim()
    {
        isTrainingInfoOn = true;
       
        if (UI_AnimSeq.IsActive())
        {
            UI_AnimSeq?.Kill();
            UI_AnimSeq = DOTween.Sequence();
        }



        if(!_uiTrainingInfo.isInit) _uiTrainingInfo.Init();
        _uiTrainingInfo.RefreshUI();
        
        
        
        GetObject((int)UI.UI_TrainingInfo).SetActive(true);

        Logger.Log("Object Info Play -------------------------------------------");
     

        UI_AnimSeq = DOTween.Sequence();
        UI_AnimSeq.AppendCallback(() =>
        {
            EmptyInstructionTextBox();//더 결합도 낮은곳으로 옮길여지 있음 010825
           
            
            GetObject((int)UI.UI_TrainingInfo).GetComponent<CanvasGroup>().alpha = 0;
        });
        UI_AnimSeq.Append(GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().DOFade(1, 0.6f).SetEase(Ease.InCirc));
        UI_AnimSeq.AppendCallback(() =>
        {
            SetNextPrevBtnsActiveStatus();
        });
        UI_AnimSeq.OnKill(() =>
        {
            //GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 1;
        });
        UI_AnimSeq.Play();
    }


    private bool isTrainingInfoOn;


    public void ShutTrainingIntroAnim()
    {
        // 이미 Taining UI가 꺼져있다면 중복으로 실행하지 않도록 합니다. 
        if (!isTrainingInfoOn)
        {
            Logger.Log("이미 훈련목표 UI가 꺼져있습니다. 실행취소.");
            return;
        }
        Logger.Log("이미 훈련목표 끄기 (확인버튼 누른경우 아닌경우)");
        isTrainingInfoOn = false;
        
        UI_AnimSeq.Kill();
        UI_AnimSeq = DOTween.Sequence();
        

        UI_AnimSeq = DOTween.Sequence();
        UI_AnimSeq.AppendCallback(() =>
        {
            GetObject((int)UI.UI_TrainingInfo).SetActive(true);
            GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 1;
        });
        UI_AnimSeq.Append(GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().DOFade(0, 0.6f).SetEase(Ease.InCirc));
        UI_AnimSeq.AppendCallback(() =>
        {
            GetObject((int)UI.UI_TrainingInfo).SetActive(false);
        });
        UI_AnimSeq.Play();
    }
    
    
    private void LoadStep( int depth2ToLoad)
    {
        
        
        Logger.Log("LoadStep By UI Top Controller Next/Prev Arrow");
      

        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Tutorial)
        {
            Logger.Log("튜토리얼에서는 아무동작 하지않습니다.");
            return;
        }
        
        
        //뎁스3(실습) 부분은 센서별로 별도로 씬구성하기에 조건문으로 구분.
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.MaintenancePractice)
        {
            
            if (depth2ToLoad <= 0)
            {
                FadeOutAndLoadScene(SceneType.DepthB);
                return;
            }
            
            
            if (depth2ToLoad<= 5 && depth2ToLoad >= 1)
            {
                
                FadeOutAndLoadScene("DepthC" + depth2ToLoad.ToString());
                return;
            }
            else
            {
                Logger.Log("학습 제일 끝부분입니다.");
                return;
            }
        }
        else  //뎁스3 이외 나머지 부분의 로직.
        {

            if (Managers.ContentInfo.PlayData.Depth1 ==  (int)Define.Depth.SensorOverview)
            {
                if (depth2ToLoad > ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[(int)Define.Depth.SensorOverview])
                {
                    FadeOutAndLoadScene(SceneType.DepthB);
                    return;
                }
                
                if (depth2ToLoad < 1)
                {
                    Logger.Log("학습 최초화면 입니다. Do nothing..");
                    return;
                }
            }
            
            
            if (Managers.ContentInfo.PlayData.Depth1 ==  (int)Define.Depth.Safety)
            {
                if (depth2ToLoad > ContentPlayData.DEPTH_TWO_MAX_COUNT_DATA[(int)Define.Depth.Safety])
                {
                 
                    FadeOutAndLoadScene(SceneType.DepthC1);
                 
                    return;
                }
                
                if (depth2ToLoad < 1)
                {
                    FadeOutAndLoadScene(SceneType.DepthA);
                    return;
                }
            }
            
            // depth2 이동전 Initialize Part---------------------------------
            animator_depth3Hide.SetBool(UI_ON,false);
            EmptyInstructionTextBox();
            
            Managers.ContentInfo.PlayData.Depth2 = depth2ToLoad;
            Managers.ContentInfo.PlayData.Depth3 = 1;
            Managers.ContentInfo.PlayData.Count = 0;
            
            if (Managers.ContentInfo.PlayData.Depth3 == 1) PlayTrainingGoalAnim();
            
            OnDepth2ClickedAction?.Invoke();
            
            Refresh();
        }
    }


    private void FadeOutAndLoadScene(SceneType sceneType)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
    
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, 0.5f));
        _fadeEffectSeq.AppendCallback(() =>
        {
            Managers.Scene.LoadScene(sceneType);
        });
    }
    
    private void FadeOutAndLoadScene(string sceneName)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
    
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, 0.5f));
        _fadeEffectSeq.AppendCallback(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
    private Sequence _fadeEffectSeq;
    public void FadeIn(float duration =2.5f)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, 0.0001f));
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(0, duration));
    }

    public void FadeOut(float duration =1.5f)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();

        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(0, 0.0001f));
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, duration));
    }

    public void FadeOutAndIn(float outTime= 1.0f, float inTime =1.0f)
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, outTime));
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(0, inTime));
        
    }
    
    public void FadeOutAndInForDepth3()
    {
        _fadeEffectSeq?.Kill();
        _fadeEffectSeq = DOTween.Sequence();
        
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(1, 0.5f));
        _fadeEffectSeq.Append(_contentControllerFadeEffectImage.DOFade(0, 0.7f));
        
    }
   
}