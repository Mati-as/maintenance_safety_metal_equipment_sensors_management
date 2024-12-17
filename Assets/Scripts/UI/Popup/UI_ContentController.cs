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
    Btn_Depth1_Title,
    Btn_TopMenu_Hide,
    Btn_Script_Hide,
    Btn_ThirdDepthList_Hide,
    Btn_Help,
    Btn_CameraInit,
    //Btn_Evaluation,
    Btn_ToolBox,
    Btn_Guidebook,

    //  UI_Depth3_List, // Active Area
    Depth3_A,
    Depth3_B,
    Depth3_C,
    Depth3_D,
    Depth3_E,
}
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
        UI_DrverOnly_GaugeSlider,
        UI_CurrentDepth_Mid,
        ToggleGroup_TopMenuBar
       
        // ActiveArea,
        // InactiveAreaA,
       // InactiveAreaB,
        //InactiveAreaC,
        //InactiveAreaD
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
        Text_Depth3_E
    }

    private readonly TextMeshProUGUI[] texts = new TextMeshProUGUI[Enum.GetValues(typeof(TMPs)).Length];

    private Animator _depth3HideBtnAnimator;
    private Animator _depthOneTextMoveAnimator;
    private Animator _instructionAnimator;
    private Animator _topMenuAnimator;
    private Animator _instructionFlipAnimator;
    private readonly int UI_ON = Animator.StringToHash("On");
    private readonly int UI_Flip = Animator.StringToHash("Flip");


    private readonly Toggle[] _depth2Toggles = new Toggle[Enum.GetValues(typeof(Toggles)).Length];
    private readonly Button[] _depth3Btns = new Button[Enum.GetValues(typeof(Btns)).Length]; // 방어적으로 사이즈 크게 할당


    private readonly float _btnClickableDelay = 1.05f;
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

    public Text Text_tooltip { get; set; }
    public Image Text_image { get; set; }

    public Slider UI_DrverOnly_GaugeSlider { get; set; }
    public RectTransform gaugeRectPos { get; set; }
    
    public RectTransform toolTipRectPos { get; set; }
    protected readonly Vector3 _toolTipPosOffset = new(55, 55, 0);
    protected readonly Vector3 _sliderPosOffset = new(-20, 30, 0);
    public bool isGuageUsable;

    private Dictionary<int, Image> _highlightImageMap;
    private Sequence _blinkBtnSeq;
    
    private bool _isStepChangeByMouseClick;

    public bool isStepChangeByMouseClick
    {
        get
        {
            return _isStepChangeByMouseClick;
        }
        set
        {
            _isStepChangeByMouseClick = value;
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


    
    
    /// <summary>
    /// 1. in tutorial, some Ui objs must be shut.they'll be executed in this logic
    /// </summary>
    public void CheckIfTutorialMode()
    { 
        bool isTutorial = Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Tutorial;
        GetObject((int)UI.ToggleGroup_TopMenuBar).gameObject.SetActive(!isTutorial);
        GetButton((int)Btns.Btn_TopMenu_Hide).gameObject.SetActive(!isTutorial);
        GetButton((int)Btns.Btn_Guidebook).gameObject.SetActive(!isTutorial);
    }


    
    
    public override bool Init()
    {
        if (!base.Init())
            return false;

       
        
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
        CheckIfTutorialMode();
        
        return true;
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


        _depth3HideBtnAnimator = GetButton((int)Btns.Btn_ThirdDepthList_Hide).gameObject.GetComponent<Animator>();
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



    private void RefreshTrainingGoalText()
    {
        
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
             i < (int)TMPs.Text_Depth2_A + ContentPlayData.DEPTH_TWO_COUNT_DATA[depth1Int];
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

        Logger.Log(
            $"{texts[(int)TMPs.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");

        
        
        texts[(int)TMPs.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + Managers.ContentInfo.PlayData.CurrentDepthStatus[2])].kor;
       
           
        string text = texts[(int)TMPs.Text_Current3Depth].text;
        int charCountWithoutSpaces = text.Replace(" ", "").Length;
        
        
        textRectcurrentDepth3.sizeDelta = new Vector2(10f + (charCountWithoutSpaces * 32f), // new width
            textRectcurrentDepth3.sizeDelta.y); 
        
     
       

        // set sizeDelta based on non-space characters
        currentDepth3UIRect.sizeDelta = new Vector2(
            65f + (charCountWithoutSpaces * 35f), // width based on non-space characters
            currentDepth3UIRect.sizeDelta.y);
        
        
      
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
    }


    public void SetNextPrevBtnsActiveStatus(bool isOn = true)
    {
        // depth4의 평가하기에서, 인트로 이외에는 버튼 표시가 진행되지 않음


        if (Managers.ContentInfo.PlayData.Depth1 == 4)
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

    private bool Depth4PrevNextBtnShowConditionCheck()
    {
        // Depth1이 4이고, Count가 0 또는 1일 때만 켜짐
      
        
        return (Managers.ContentInfo.PlayData.Count == 0 );
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

        isStepChangeByMouseClick = true;
        if(hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        if(!_currentMainCam.isControllable) HideCamInitBtn();
        
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,true);

     
       
    }


    public void InvokeNextStep()
    {
    
        Logger.Log("스텝별 수행 미션 완료, 다음 스크립트 및 애니메이션 재생");
            OnNextBtnClicked();
        

    }
    /// <summary>
    /// 버튼 클릭외에 스크립트를 넘어가는 경우 (예. 미션수행완료 등) 사용
    /// </summary>

    private void OnNextBtnClicked()
    {
        Precheck();//
        
        SetScriptUI();
        
        if (Managers.ContentInfo.PlayData.Count >= ContentPlayData.CurrentCountMax)
        {
            Logger.Log("current count is Max ---------");
            return;
        }

        if (Managers.ContentInfo.PlayData.Count >= ContentPlayData.CurrentCountMax)
        {
            //depth2 invoke로직 or 버튼에 직접할당하기
        }

        Managers.ContentInfo.PlayData.Count++;
        isStepChangeByMouseClick = true;
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,false);
        if(!_currentMainCam.isControllable) HideCamInitBtn();
        
    }




    public void ChangeInstructionText()
    {
        StartCoroutine(ChangeTextWithAnimCo());
    }

    private WaitForSeconds animDelay;

    private IEnumerator ChangeTextWithAnimCo()
    {
       // _instructionFlipAnimator.SetTrigger(UI_Flip);
        if (animDelay == null) animDelay = new WaitForSeconds(0.15f);
        yield return animDelay;

        if (Managers.ContentInfo.PlayData.Count ==0)
        {
//            Logger.Log("Count is zero.. intro animation playing........");
            yield break;
        }
        
        if(!Managers.Data.Texts.ContainsKey(int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)))
        {
            Logger.LogWarning("해당 스크립트 없음. 스크립트 누락가능성 있음.");
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
        

        //Depth1이 실습단계가 아닌 경우-----------------------------------------
        
        if (Managers.ContentInfo.PlayData.Depth3 == 1) PlayTrainingGoalAnim(); // Depth3가 첫번쨰인경우만 훈련목표 재생(Depth1예외)
        
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
        for (var i = (int)Toggles.Toggle_Depth2_A; i < (int)Toggles.Toggle_Depth2_A + ContentPlayData.DEPTH_TWO_COUNT_DATA[
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
        
        _depth3HideBtnAnimator.SetBool(UI_ON,_isdepth3ListOn);
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(_isdepth3ListOn);
    }

    private bool isOnActiveArea; // 뎁스3내용 Hover시 표출. 표출이후에도 내용범위에 머물러있으면 Hover상태 유지
    private bool isOn3depthArea;


    
    private void OnDepth3BtnClicked(int depth3Num)
    {
        Precheck();
        
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
        
        Logger.Log(
            $"depth 3 current: {texts[(int)TMPs.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");


        Managers.ContentInfo.PlayData.Depth3 = depth3Num;
        texts[(int)TMPs.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + depth3Num)].kor;

        Managers.ContentInfo.PlayData.Count = 1;
        
 

        if (Managers.ContentInfo.PlayData.Depth3 != 1)
        {
            ShutTrainingIntroAnim();
            if(_hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        }
        
        Refresh();
        ChangeInstructionText();
        
        SetInstructionShowOrHideStatus(false);
        OnDepth3ClickedAction?.Invoke();
        
        
    }

    private void OnDepthClickedWhenDepthC()
    {
        
    }
    private void OnDepth3BtnEnter()
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
            UI_AnimSeq.Kill();
            UI_AnimSeq = DOTween.Sequence();
        }

        
        if(Managers.ContentInfo.PlayData.Depth3==1) SetInstructionShowOrHideStatus(false);
        else
        {
            SetInstructionShowOrHideStatus();
        }
        
      

        if(!_uiTrainingInfo.isInit) _uiTrainingInfo.Init();
        _uiTrainingInfo.RefreshUI();
        
        
        
        GetObject((int)UI.UI_TrainingInfo).SetActive(true);

        Logger.Log("Object Info Play -------------------------------------------");
     

        UI_AnimSeq = DOTween.Sequence();
        UI_AnimSeq.AppendCallback(() =>
        {
           
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
            //SetNextPrevBtnsActiveStatus();
            return;
        }
        
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
    
    
}