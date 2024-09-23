using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_ContentController : UI_Popup
{
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

        //  UI_Depth3_List, // Active Area
        //
        Depth3_A,
        Depth3_B,
        Depth3_C,
        Depth3_D,
        Depth3_E
    }

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


    private readonly float _btnClickableDelay = 0.85f;
    private WaitForSeconds _waitForClick;
    private bool _clickable = true;


    private InputAction _mouseClickAction;

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
        return true;
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
    }
    
    public Text Text_tooltip { get; set; }
    public Image Text_image { get; set; }
    public RectTransform toolTipRectPos { get; set; }
    private Vector3 _toolTipPosOffset = new Vector3(20, 20, 0);
    void Update()
    {
        Update_MousePosition();
    }
    private void Update_MousePosition()
    {
        Vector2 mousePos = Input.mousePosition + _toolTipPosOffset;
        if (toolTipRectPos == null)
        {
            toolTipRectPos =GetObject((int)UI.UI_ToolTip).GetComponent<RectTransform>();
            Logger.Log("get tooltip rectpos");
        }
        toolTipRectPos.position = mousePos;
    }
    public void SetText(string text=null)
    {
        SetToolTipStatus(true);
        if (Text_tooltip == null)
        {
            Text_tooltip = GetObject((int)UI.UI_ToolTip).GetComponentInChildren<Text>();
            Text_image =GetObject((int)UI.UI_ToolTip).GetComponentInChildren<Image>();
            Logger.Log("get tooltip text");
        }

        Text_tooltip.text = text;
        RectTransform rect_1 = (RectTransform)Text_image.transform;
        rect_1.sizeDelta = new Vector2(text.Length * 30, 50);
        RectTransform rect_2 = (RectTransform)Text_tooltip.transform;
        rect_2.sizeDelta = new Vector2(text.Length * 30, 50);
    }

    public void SetToolTipStatus(bool isOn=true)
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
            toggle.gameObject.BindEvent(() => OnDepth2Clicked(i + 1));
            _depth2Toggles[(int)Toggles.Toggle_Depth2_A + i] = toggle;
        }
    }

    private void InitDepth3Buttons()
    {
        for (var i = 0; i < 5; i++)
        {
            var btn = GetButton((int)Btns.Depth3_A + i);
            btn.gameObject.BindEvent(() => OnDepth3BtnClicked(i + 1));
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

    public void SetActiveInstruction(bool active = true)
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

        GetToggle((int)Toggles.Toggle_Depth2_A).isOn = true;
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
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor;
    }


    private void SetBtns()
    {
        GetButton((int)Btns.Btn_Prev).gameObject.BindEvent(OnPrevBtnClicked);
        GetButton((int)Btns.Btn_Next).gameObject.BindEvent(OnNextBtnClicked);
    }


    public static event Action<int> OnStepBtnClicked_CurrentCount;

    private void OnPrevBtnClicked()
    {
        Precheck();

        Debug.Assert(Managers.ContentInfo.PlayData.Count > 0);

        if (Managers.ContentInfo.PlayData.Count <= 1)
        {
#if UNITY_EDITOR
            Logger.Log("currentCount is 1, Start Point");
#endif
            return;
        }

        Managers.ContentInfo.PlayData.Count--;


        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count);
        ChangeInstructionTextWithAnim();
    }

    private void OnNextBtnClicked()
    {
        Precheck();
        if (Managers.ContentInfo.PlayData.Count >= ContentPlayData.COUNT_MAX)
        {
#if UNITY_EDITOR
            Debug.Log("current count is Max ---------");
            return;
#endif
        }

        Managers.ContentInfo.PlayData.Count++;
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count);
        ChangeInstructionTextWithAnim();
    }

    private void ChangeInstructionTextWithAnim()
    {
        StartCoroutine(ChangeTextWithAnimCo());
    }

    private WaitForSeconds animDelay;

    private IEnumerator ChangeTextWithAnimCo()
    {
        _instructionFlipAnimator.SetTrigger(UI_Flip);
        if (animDelay == null) animDelay = new WaitForSeconds(0.15f);
        yield return animDelay;

        texts[(int)Content_TMP.Text_Instruction].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus)].kor;
    }


    private void OnDepth2Clicked(int depth2)
    {
        Precheck();
        Logger.Log($"Depth2 Banner Toggled {depth2}");
        Managers.ContentInfo.PlayData.Depth2 = depth2;
        Managers.ContentInfo.PlayData.Depth3 = 1;

        ChangeInstructionTextWithAnim();


        RefreshUI();
        RefreshText();
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
        for (var i = (int)Toggles.Toggle_Depth2_A;
             i < (int)Toggles.Toggle_Depth2_A
             + ContentPlayData.DEPTH_TWO_COUNT_DATA[
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
        Debug.Log($"_activeAreaRect Height: {_activeAreaRect.sizeDelta.y}");
    }

    private void ResizeActiveArea()
    {
    }

    /// <summary>
    ///     Text,UI  total Refresh
    /// </summary>
    public void Refresh()
    {
        RefreshUI();
        RefreshText();
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
        _clickable = false;
        if (_waitForClick == null) _waitForClick = new WaitForSeconds(_btnClickableDelay);

        yield return _waitForClick;
        _clickable = true;
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
        //  Managers.ContentInfo.PlayData.ResetOrSetDepthCount(depth3Num);

#if UNITY_EDITOR
        Debug.Log($"current Status{Managers.ContentInfo.PlayData.CurrentDepthStatus}");
#endif

        for (var i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_E + 1; i++) _depth3Btns[i].gameObject.SetActive(false);
        var currentDepth12 = Managers.ContentInfo.PlayData.CurrentDepthStatus[0] +
                             Managers.ContentInfo.PlayData.CurrentDepthStatus[1].ToString();
        Debug.Log($"depthCount {currentDepth12}");

        for (var i = (int)Btns.Depth3_A;
             i < (int)Btns.Depth3_A + ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)];
             i++) _depth3Btns[i].gameObject.SetActive(true);
        Debug.Log(
            $"depth 3 current: {texts[(int)Content_TMP.Text_Current3Depth].text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00" + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + 1)].kor}");


        Managers.ContentInfo.PlayData.Depth3 = depth3Num;
        texts[(int)Content_TMP.Text_Current3Depth].text =
            Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.CurrentDepthStatus[0] + "00"
                + Managers.ContentInfo.PlayData.CurrentDepthStatus[1] + depth3Num)].kor;
        Refresh();
        ChangeInstructionTextWithAnim();
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
        if (!_clickable)
        {
            Logger.Log("Clicking Too Fast");
            return;
        }

        SetClickable();
    }

    public bool isInstructAnimOn { get; private set; }
    private bool _isTopMenuOn = true;

    private void OnInstructionHideClicked()
    {
        Precheck();
        isInstructAnimOn = !isInstructAnimOn;
        _instructionAnimator.SetBool(UI_ON, isInstructAnimOn);

        Logger.Log($" Current Script UI Status{isInstructAnimOn}");
    }

    public void ShowScriptUI()
    {
        isInstructAnimOn = true;
        _instructionAnimator.SetBool(UI_ON, isInstructAnimOn);
    }

    private void OnTopMenuAnimBtnClicked()
    {
        Precheck();
        _isTopMenuOn = !_isTopMenuOn;
        _topMenuAnimator.SetBool(UI_ON, _isTopMenuOn);

        Logger.Log($" topMenu Status: {_isTopMenuOn}");
    }


    private bool isTrainingInfoOpen;
    private Sequence _introUIOnSeq;

    public void ShowMainIntro()
    {
        GetObject((int)UI.UI_DepthTitle).transform.localScale = Vector3.zero;
        
        var seq = DOTween.Sequence();
        seq.Append(GetObject((int)UI.UI_DepthTitle).transform.DOScale(1, 0.8f).SetEase(Ease.InCirc));
        _introUIOnSeq.AppendInterval(1f);
        _introUIOnSeq.Append(GetObject((int)UI.UI_DepthTitle).transform.DOScale(0, 1f).SetEase(Ease.InCirc));
        _introUIOnSeq.AppendCallback(() =>
        {
            GetObject((int)UI.UI_DepthTitle).SetActive(false);
            PlayObjectiveIntroAnim();
        });

    }

    public void PlayObjectiveIntroAnim()
    {
        GetObject((int)UI.UI_TrainingInfo).transform.localScale = Vector3.zero;
        GetObject((int)UI.UI_TrainingInfo).SetActive(true);
        GetObject((int)UI.UI_DepthTitle).SetActive(true);
        
        if(_introUIOnSeq.IsActive()) _introUIOnSeq.Kill();
        if(_introUICloseSeq.IsActive()) _introUICloseSeq.Kill();

        
        _introUIOnSeq = DOTween.Sequence();
        _introUIOnSeq.AppendCallback(() =>
        {
            GetObject((int)UI.UI_DepthTitle).SetActive(false);
        });
        _introUIOnSeq.Append(GetObject((int)UI.UI_TrainingInfo).transform.DOScale(1, 0.8f).SetEase(Ease.InCirc));
        _introUIOnSeq.OnKill(() =>
        {
            GetObject((int)UI.UI_DepthTitle).transform.localScale = Vector3.zero;
            GetObject((int)UI.UI_TrainingInfo).transform.localScale = Vector3.zero;
            GetObject((int)UI.UI_TrainingInfo).SetActive(true);
            GetObject((int)UI.UI_DepthTitle).SetActive(true);
        });
    }
    private Sequence _introUICloseSeq;
    public void ShutTrainingInfroAnim()
    {
    
        if(_introUIOnSeq.IsActive()) _introUIOnSeq.Kill();
        if(_introUICloseSeq.IsActive()) _introUICloseSeq.Kill();

        _introUICloseSeq = DOTween.Sequence();

        _introUICloseSeq.Append(GetObject((int)UI.UI_TrainingInfo).transform.DOScale(0, 0.8f).SetEase(Ease.InCirc));
        _introUICloseSeq.AppendCallback(() =>
        {    
            GetObject((int)UI.UI_TrainingInfo).SetActive(false);
        });
        _introUICloseSeq.OnKill(() =>
        {
        
        });
    
    }
}