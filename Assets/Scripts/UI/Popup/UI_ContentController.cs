using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    Btn_ThirdDepth_Hide,
    Btn_Help,
    Btn_Evaluation,
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
        Toggle_Depth2_E
    }

    public enum UI
    {
        UI_Top,
        UI_TextBox,
        UI_DepthTitle,
        UI_TrainingInfo,
        UI_Instruction,
        UI_Depth3_List,
        UI_ToolTip,
        UI_ToolBox,
        UI_DrverOnly_GaugeSlider,
        ActiveArea,
        InactiveAreaA,
        InactiveAreaB,
        InactiveAreaC,
        InactiveAreaD
    }


    public enum Content_TMP
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

    private readonly TextMeshProUGUI[] texts = new TextMeshProUGUI[Enum.GetValues(typeof(Content_TMP)).Length];

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


    private InputAction _mouseClickAction;

    public Text Text_tooltip { get; set; }
    public Image Text_image { get; set; }

    public Slider UI_DrverOnly_GaugeSlider { get; set; }
    public RectTransform gaugeRectPos { get; set; }
    
    public RectTransform toolTipRectPos { get; set; }
    private readonly Vector3 _toolTipPosOffset = new(55, 55, 0);
    private readonly Vector3 _sliderPosOffset = new(-20, 30, 0);
    public bool isGuageUsable;

    private Dictionary<int, Image> _highlightImageMap;
    
    
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
    
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindUIElements();
        InitTopMenu();
        InitDepth2Toggles();
        InitDepth3Buttons();
        InitCommonUI();
        SetInitialStates();
        Refresh();
        SetBtns();
        SetHeighlightUIImages();
        
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
        return true;
    }

    
    /// <summary>
    /// 하이라이트 기능이 있는 객체는 highlight 이미지를 할당해놓고, 아래 메소드에서 직접 초기화합니다.
    /// </summary>
    private void SetHeighlightUIImages()
    {
        if (_highlightImageMap == null) _highlightImageMap = new Dictionary<int, Image>();

        Image highlightImage = GetButton((int)(Btns.Btn_Evaluation))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); // Looks for the one with the specific name
        _highlightImageMap.TryAdd((int)Btns.Btn_Evaluation, highlightImage);
        
        
        Image GuidebookHlImage = GetButton((int)(Btns.Btn_Guidebook))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_Guidebook, GuidebookHlImage);
        
        Image ToolboxImage = GetButton((int)(Btns.Btn_ToolBox))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_ToolBox, ToolboxImage);
    }


    public void BlinkBtnUI(int btnEnum)
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < 8; i++)
        {
            seq.Append(_highlightImageMap[btnEnum].DOFade(1, 0.2f));
            seq.AppendInterval(0.32f);
            seq.Append(_highlightImageMap[btnEnum].DOFade(0, 0.2f));
            seq.AppendInterval(0.2f);
        }

        seq.Play();
    }
    private void CloseUIPopup<T>() where T : UI_Popup
    {
        var popup = Managers.UI.FindPopup<T>();
        if (popup != null)
            Managers.UI.ClosePopupUI(popup);
    }

    private void BindUIElements()
    {
        BindButton(typeof(Btns));
        BindToggle(typeof(Toggles));
        BindObject(typeof(UI));
        BindTMP(typeof(Content_TMP));
        
        UI_DrverOnly_GaugeSlider = GetObject((int)UI.UI_DrverOnly_GaugeSlider).GetComponent<Slider>();
        UI_DrverOnly_GaugeSlider.gameObject.SetActive(false);

    }



    private void Update()
    {
        Update_MousePosition();
    }


    private void Update_MousePosition()
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
            Logger.Log("get tooltip rectpos");
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

        BindHoverEventToButton(Btns.Btn_Depth1_Title, OnDepthOneTitleHover, OnDepthOneTitleHoverExit);
        _topMenuAnimator = GetObject((int)UI.UI_Top).gameObject.GetComponent<Animator>();
        _topMenuAnimator.SetBool(UI_ON, true);
        GetButton((int)Btns.Btn_TopMenu_Hide).gameObject.BindEvent(OnTopMenuAnimBtnClicked);
    }

    private void InitDepth2Toggles()
    {
        for (var i = 0; i < 5; i++)
        {
            Logger.Log($"Depth2 Banner Toggled {i}");
            var toggle = GetToggle((int)Toggles.Toggle_Depth2_A + i);

            if (toggle == null)
            {
                Logger.LogWarning($"Depth 2 Btn {i} is null");
                continue;
            }
            int toggleIndex = i;

            if (GetToggle((int)Toggles.Toggle_Depth2_A + i).interactable)
            {
                toggle.gameObject.BindEvent(() =>
                { 
                    OnDepth2Clicked(toggleIndex + 1); // use the local copy
                });
            }
          
            _depth2Toggles[(int)Toggles.Toggle_Depth2_A + i] = toggle;
        }
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

    private void InitCommonUI()
    {
        GetObject((int)UI.UI_Depth3_List).BindEvent(OnDepth3BtnAreaEnter);
        GetButton((int)Btns.Btn_ThirdDepth_Hide).gameObject
            .BindEvent(OnDepthThirdHideBtnHover, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.Btn_ThirdDepth_Hide).gameObject
            .BindEvent(OnDepthThirdHideBtnExit, Define.UIEvent.PointerExit);

        InitInstructionSection();
        InitActiveAndInactiveAreas();
    }

    private void InitInstructionSection()
    {
        _instructionAnimator = GetObject((int)UI.UI_Instruction).GetComponent<Animator>();
        _instructionFlipAnimator = GetObject((int)UI.UI_Instruction).transform.GetChild(0).GetComponent<Animator>();
        // _instructionAnimator.SetBool(UI_ON, true);

        GetButton((int)Btns.Btn_Script_Hide).gameObject.BindEvent(OnInstructionHideClicked);
        GetObject((int)UI.UI_Instruction).GetComponent<Text>();
    }

    public void SetInstructionShowOrHideStatus(bool active = true)
    {
        _instructionAnimator.SetBool(UI_ON, active);
    }

    private void InitActiveAndInactiveAreas()
    {
        BindPointerEventToObject(UI.ActiveArea, OnDpeth3ActiveAreaEnter);
        BindPointerEventToObject(UI.InactiveAreaA, InactiveAreaEnter);
        BindPointerEventToObject(UI.InactiveAreaB, InactiveAreaEnter);
        BindPointerEventToObject(UI.InactiveAreaC, InactiveAreaEnter);
        BindPointerEventToObject(UI.InactiveAreaD, InactiveAreaEnter);
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
        _activeAreaRect = GetObject((int)UI.ActiveArea).GetComponent<RectTransform>();
        for (var i = 0; i < texts.Length; i++) texts[i] = GetTMP(i);

        
        GetToggle((int)Toggles.Toggle_Depth2_B).isOn = true;
        GetObject((int)UI.UI_ToolTip).SetActive(false);
        ShowOrHideNextPrevBtns(false);
    }


    
    private void RefreshText()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        texts[(int)Content_TMP.Text_Depth1_Title].text = Managers.Data.Texts[int.Parse(depth1 + "0000")].kor;
        var depth1Int = Managers.ContentInfo.PlayData.Depth1;

        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        var depth2Int = Managers.ContentInfo.PlayData.Depth2;
        // Debug.Log($"depth 2 count : {ContentPlayData.DEPTH_TWO_COUNT_DATA[depth1Int]}");
        //Debug.Log($"depth 3 count : {ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(depth1+depth2)]}");

        var depth2Count = 1;
        for (var i = (int)Content_TMP.Text_Depth2_A;
             i < (int)Content_TMP.Text_Depth2_A + ContentPlayData.DEPTH_TWO_COUNT_DATA[depth1Int];
             i++)
        {
            texts[i].text = Managers.Data.Texts[int.Parse(depth1 + "00" + depth2Count + "0")].kor;
            depth2Count++;
            //Debug.Log($"Depth2 텍스트 변환 완료 :{ texts[i].gameObject.name} : { texts[i].text}");
        }


        var depth3Count = 1;
        for (var i = (int)Content_TMP.Text_Depth3_A;
             i < (int)Content_TMP.Text_Depth3_A + ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(depth1 + depth2)];
             i++)
        {
            texts[i].text = Managers.Data.Texts[int.Parse(depth1 + "00" + depth2 + depth3Count)].kor;
            depth3Count++;
            // Debug.Log($"Depth3 텍스트 변환 완료 : { texts[i].text}");
        }

        Debug.Log(
            $"{texts[(int)Content_TMP.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");

        texts[(int)Content_TMP.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + Managers.ContentInfo.PlayData.CurrentDepthStatus[2])].kor;
    }


    [FormerlySerializedAs("_uiToolBox")] public UI_ToolBox uiToolBox;
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


    private void ShowOrHideNextPrevBtns(bool isOn = true)
    {
        GetButton((int)Btns.Btn_Prev).enabled = isOn;
        GetButton((int)Btns.Btn_Next).enabled = isOn;
        var fade = isOn ? 1 : 0;
        var speed = isOn ? 1 : 0;
        var scale = isOn ? 1 : 0;
        GetButton((int)Btns.Btn_Prev).gameObject.transform.localScale = Vector3.one * scale;
        GetButton((int)Btns.Btn_Next).gameObject.transform.localScale = Vector3.one * scale;
        GetButton((int)Btns.Btn_Prev).GetComponent<Image>().DOFade(fade, speed);
        GetButton((int)Btns.Btn_Next).GetComponent<Image>().DOFade(fade, speed);
    }

    
    //Action<애니메이션 순서(int), Reverse 여부(bool)>
    public static event Action<int,bool> OnStepBtnClicked_CurrentCount;
    public static event Action OnDepth3ClickedAction;
    public static event Action OnDepth2ClickedAction;
    public static event Action<int> OnNextDepthInvoked; //sceneChange 

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

        
        if(hideBtn_isInstructionViewActive)SetInstructionShowOrHideStatus();
        
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
        Precheck();

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
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,false);
        
       
        
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
            Logger.Log("Count is zero.. intro animation playing........");
            yield break;
        }
        
        if(!Managers.Data.Texts.ContainsKey(int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)))
        {
            Logger.LogWarning("해당 스크립트 없음. 스크립트 누락가능성 있음.");
            yield break;;
        }
        
        texts[(int)Content_TMP.Text_Instruction].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)].kor;
    }


    public void OnDepth2Clicked(int depth2)
    {
        Precheck();


        Managers.ContentInfo.PlayData.Depth2 = depth2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;


        //각 뎁스의 첫번쨰 애니메이션을 재생하도록 하기위한 로직
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count, false);


        Logger.Log($"Depth2 Banner Toggled {depth2}");
        if (Managers.ContentInfo.PlayData.Depth3 == 1) PlayObjectiveIntroAnim(); // 뎁스가 첫번쨰인경우만 훈련목표 재생


        ChangeInstructionText();
        RefreshUI();
        RefreshText();


        OnDepth2ClickedAction?.Invoke();
    }

   
    private RectTransform _activeAreaRect;
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
        var resizedHeight = new Vector2(_activeAreaRect.sizeDelta.x, _heightPerDepth3Btn * 1.45f *
            ContentPlayData.DEPTH_THREE_COUNT_DATA[
                int.Parse(currentDepth12)] + 1);
        _activeAreaRect.sizeDelta = resizedHeight;
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
        texts[(int)Content_TMP.Text_Instruction].text =
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


    private void OnDepthThirdHideBtnHover()
    {
    }

    private bool isOnActiveArea; // 뎁스3내용 Hover시 표출. 표출이후에도 내용범위에 머물러있으면 Hover상태 유지
    private bool isOn3depthArea;

    private void InactiveAreaEnter()
    {
        isOnActiveArea = false;

        if (isOnActiveArea) return;
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
        //Debug.Log("3depth UI off ---------------------");
    }

    private void OnDpeth3ActiveAreaEnter()
    {
        if (_isTopMenuOn)
        {
            // Debug.Log("3depth UI On ---------------------");
            // Debug.Log("3depth UI On ---------------------");
            isOnActiveArea = true;
            GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(true);
        }
    }


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
            $"depth 3 current: {texts[(int)Content_TMP.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");


        Managers.ContentInfo.PlayData.Depth3 = depth3Num;
        texts[(int)Content_TMP.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + depth3Num)].kor;

        Managers.ContentInfo.PlayData.Count = 1;
        
        Refresh();
        ChangeInstructionText();
        OnDepth3ClickedAction?.Invoke();
        
        
    }

    private void OnDepth3BtnEnter()
    {
    }

    private void OnDepth3BtnAreaEnter()
    {
        // OnAnimation
    }


    private void OnDepth3ABtnExit()
    {
        isOn3depthArea = false;
        //Animatiorn
    }


    
    private void OnDepth3BtnExit()
    {
        // OnAnimation
    }

    private void OnDepthThirdHideBtnExit()
    {
    }


    private void Precheck()
    {
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

    private bool CheckInstructionUIMode()
    {
        return true;
    }
    
    

    private void OnTopMenuAnimBtnClicked()
    {
        Precheck();
        _isTopMenuOn = !_isTopMenuOn;
        _topMenuAnimator.SetBool(UI_ON, _isTopMenuOn);

        Logger.Log($" topMenu Status: {_isTopMenuOn}");
    }


    private bool isTrainingInfoOpen;
    private Sequence UI_AnimSeq;


    public void ShowInitialIntro()
    {
        Logger.Log("Training Info Play -------------------------------------------");

        

        var _UIOnSeq = DOTween.Sequence();
        //초기화
        _UIOnSeq.AppendCallback(() =>
        {

            GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 0f;
        });
        
        //애니메이션
        _UIOnSeq.Append(GetObject((int)UI.UI_DepthTitle).transform.GetComponent<Image>().DOFade(1, 1f).SetEase(Ease.InCirc));
        _UIOnSeq.Append(GetObject((int)UI.UI_DepthTitle).transform.DOScale(1, 0.8f).SetEase(Ease.InCirc));
        _UIOnSeq.AppendInterval(1f);
        _UIOnSeq.Append(GetObject((int)UI.UI_DepthTitle).transform.GetComponent<Image>().DOFade(0, 1f).SetEase(Ease.InCirc));
        _UIOnSeq.AppendCallback(() => { GetObject((int)UI.UI_DepthTitle).SetActive(false); });
        _UIOnSeq.AppendInterval(0.5f);
        _UIOnSeq.AppendCallback(PlayObjectiveIntroAnim);
        _UIOnSeq.OnKill(() =>
        {
            GetObject((int)UI.UI_DepthTitle).transform.localScale = Vector3.zero;
            _UIOnSeq.AppendCallback(() =>
            {

                GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 0f;
            });
        });
    }

    private UI_TrainingInfo _uiTrainingInfo;
    public void PlayObjectiveIntroAnim()
    {
        
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
           
            ShowOrHideNextPrevBtns();
        });
        UI_AnimSeq.OnKill(() =>
        {
            //GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 1;
        });
        UI_AnimSeq.Play();
    }



    public void ShutTrainingInfroAnim()
    {
        if(GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha < 0.5f) {
            return; // 이미한번 실행됬다고 판단해서 리턴합니다.
}
       
        if (UI_AnimSeq.IsActive())
        {
            UI_AnimSeq.Kill();
            UI_AnimSeq = DOTween.Sequence();
        }

        UI_AnimSeq = DOTween.Sequence();
        UI_AnimSeq.AppendCallback(() =>
        {
            GetObject((int)UI.UI_TrainingInfo).SetActive(true);
            GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 1;
        });
        UI_AnimSeq.Append(GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().DOFade(0, 0.6f).SetEase(Ease.InCirc));
        UI_AnimSeq.AppendCallback(() => { GetObject((int)UI.UI_TrainingInfo).SetActive(false); });
        UI_AnimSeq.OnKill(() =>
        {
            //GetObject((int)UI.UI_TrainingInfo).transform.GetComponent<CanvasGroup>().alpha = 0;
        });
        UI_AnimSeq.Play();
    }
    
    
}