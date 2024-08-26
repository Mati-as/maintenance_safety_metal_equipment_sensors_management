using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
        Btn_GuideBook,
        Btn_Evaluation,
     //  UI_Depth3_List, // Active Area 
        //
        Depth3_A,
        Depth3_B,
        Depth3_C,
        Depth3_D,
        Depth3_E,
        ActiveArea
    }

    public enum Toggles
    {
        Toggle_Depth2_A,
        Toggle_Depth2_B,
        Toggle_Depth2_C
    }

    public enum UI
    {
        UI_Top,
        UI_CurrentDepth_Mid,
        UI_TextBox,
        UI_Instruction,
        UI_Depth3_List
    }


    private Text[] texts = new Text[Enum.GetValues(typeof(UI)).Length];

    private Animator _depthOneTextMoveAnimator;
    private Animator _instructionAnimator;
    private Animator _topMenuAnimator;
    private readonly int UI_ON = Animator.StringToHash("On");
    private readonly int UI_Flip = Animator.StringToHash("Flip");

    private Button[] _depth3Btns = new Button[Enum.GetValues(typeof(Btns)).Length]; // 방어적으로 사이즈 크게 할당

    private readonly float _btnClickableDelay = 0.85f;
    private WaitForSeconds _waitForClick;
    private bool _clickable = true;


    // UI 클릭관련 설정
    private InputAction _mouseClickAction;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        BindButton(typeof(Btns));
        BindToggle(typeof(Toggles));
        BindObject(typeof(UI));

        // 1.상단 -----------------------------------------------------------------------------------------------
       
        GetToggle((int)Toggles.Toggle_Depth2_A).gameObject
            .BindEvent(() => { OnToggleClicked(1); });
        GetToggle((int)Toggles.Toggle_Depth2_B).gameObject
            .BindEvent(() => { OnToggleClicked(2); });
        GetToggle((int)Toggles.Toggle_Depth2_C).gameObject
            .BindEvent(() => { OnToggleClicked(3);});
        
        _topMenuAnimator = GetObject((int)UI.UI_Top).gameObject.GetComponent<Animator>();
        GetButton((int)Btns.Btn_TopMenu_Hide).gameObject
            .BindEvent(() => { OnTopMenuAnimBtnClicked(); });
        _topMenuAnimator.SetBool(UI_ON, true);
        
        
        _depthOneTextMoveAnimator = GetButton((int)Btns.Btn_Depth1_Title).gameObject.GetComponent<Animator>();
        _depthOneTextMoveAnimator.enabled = false;
        
        GetButton((int)Btns.Btn_Depth1_Title).gameObject.BindEvent(OnDepthOneTitleHover, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.Btn_Depth1_Title).gameObject
            .BindEvent(OnDepthOneTitleHoverExit, Define.UIEvent.PointerExit);
        
        
        // 2.중단(Depth3) UI --------------------------------------------------------------------------------------
        GetButton((int)Btns.Btn_ThirdDepth_Hide).gameObject
            .BindEvent(() => { OnDepthThirdHideBtnHover(); }, Define.UIEvent.PointerEnter);

        GetButton((int)Btns.Btn_ThirdDepth_Hide).gameObject
            .BindEvent(() => { OnDepthThirdHideBtnExit(); }, Define.UIEvent.PointerExit);
        
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
        // 3. 앞,뒤 버튼  -------------------------------------------------------------------------------------------
        GetButton((int)Btns.Btn_Prev).gameObject
            .BindEvent(() => { OnPrevBtnClicked(); });
        GetButton((int)Btns.Btn_Next).gameObject
            .BindEvent(() => { OnNextBtnClicked(); });


        // 4.하단 및 스크립트  --------------------------------------------------------------------------------------
        GetObject((int)UI.UI_Instruction).GetComponent<Text>();

        GetButton((int)Btns.Btn_Script_Hide).gameObject
            .BindEvent(() => { OnInstructionClicked(); });
        
        _instructionAnimator = GetObject((int)UI.UI_Instruction).GetComponent<Animator>();
        _instructionAnimator.SetBool(UI_ON, true);

        texts[(int)UI.UI_TextBox] = GetObject((int)UI.UI_TextBox).GetComponent<Text>();

        // 5.기타 버튼  ---------------------------------------------------------------------------------------------

        _depth3Btns[(int)Btns.Depth3_A] = GetButton((int)Btns.Depth3_A);
        _depth3Btns[(int)Btns.Depth3_B] = GetButton((int)Btns.Depth3_B);
        _depth3Btns[(int)Btns.Depth3_C] = GetButton((int)Btns.Depth3_C);
        _depth3Btns[(int)Btns.Depth3_D] = GetButton((int)Btns.Depth3_D);
        _depth3Btns[(int)Btns.Depth3_E] = GetButton((int)Btns.Depth3_E);
        
        
        GetButton((int)Btns.Depth3_A).gameObject
            .BindEvent(() => { OnDepth3BtnClicked(1); });
        GetButton((int)Btns.Depth3_B).gameObject
            .BindEvent(() => { OnDepth3BtnClicked(2); });
        GetButton((int)Btns.Depth3_C).gameObject
            .BindEvent(() => { OnDepth3BtnClicked(3); });
        GetButton((int)Btns.Depth3_D).gameObject
            .BindEvent(() => { OnDepth3BtnClicked(4); });
        GetButton((int)Btns.Depth3_E).gameObject
            .BindEvent(() => { OnDepth3BtnClicked(5); });
        
        
        GetButton((int)Btns.Depth3_A).gameObject
            .BindEvent(() => { OnDepth3BtnEnter(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_B).gameObject
            .BindEvent(() => { OnDepth3BtnEnter(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_C).gameObject
            .BindEvent(() => { OnDepth3BtnEnter(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_D).gameObject
            .BindEvent(() => { OnDepth3BtnEnter(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_E).gameObject
            .BindEvent(() => { OnDepth3BtnEnter(); },Define.UIEvent.PointerExit);
        
        
        GetButton((int)Btns.Depth3_A).gameObject
            .BindEvent(() => { OnDepth3ABtnExit(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_B).gameObject
            .BindEvent(() => { OnDepth3ABtnExit(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_C).gameObject
            .BindEvent(() => { OnDepth3ABtnExit(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_D).gameObject
            .BindEvent(() => { OnDepth3ABtnExit(); },Define.UIEvent.PointerExit);
        GetButton((int)Btns.Depth3_E).gameObject
            .BindEvent(() => { OnDepth3ABtnExit(); },Define.UIEvent.PointerExit);
   
        
        
        
        GetButton((int)Btns.ActiveArea).gameObject
            .BindEvent(() => { OnDpeth3ActiveAreaExit(); },Define.UIEvent.PointerExit);
        
        texts[(int)UI.UI_TextBox].text = Managers.Data.Texts[int.Parse(Managers.ContentPlayManager.PlayData.CurrentDepthStatus)].kor;
        
        return true;
    }

    
    private void OnPrevBtnClicked()
    {
        Precheck();

        if (Managers.ContentPlayManager.PlayData.Count < 1)
        {
#if UNITY_EDITOR
            Debug.Log("current count is 0 ---------");
            return;
#endif
        }
        Managers.ContentPlayManager.PlayData.Count--;
        ChangeTextWithAnim();
    }

    private void OnNextBtnClicked()
    {
        Precheck();
        if (Managers.ContentPlayManager.PlayData.Count >= ContentPlayData.COUNT_MAX)
        {
#if UNITY_EDITOR
            Debug.Log("current count is Max ---------");
            return;
#endif
        }
        Managers.ContentPlayManager.PlayData.Count++;
        ChangeTextWithAnim();
    }

    private void ChangeTextWithAnim()
    {
        StartCoroutine(ChangeTextWithAnimCo());
    }

    private WaitForSeconds animDelay;
    IEnumerator ChangeTextWithAnimCo()
    {
      
        _instructionAnimator.SetTrigger(UI_Flip);
        if (animDelay == null)
        {
            animDelay = new WaitForSeconds(0.15f);
        }
        yield return animDelay;
        
        texts[(int)UI.UI_TextBox].text = Managers.Data.Texts[int.Parse(Managers.ContentPlayManager.PlayData.CurrentDepthStatus)].kor;
    }


    private void OnToggleClicked(int depth2)
    {
        Precheck();
        Managers.ContentPlayManager.PlayData.Depth2 = depth2;
        Managers.ContentPlayManager.PlayData.Depth3 = 1;
        ChangeTextWithAnim();

    
        
        for (int i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_E + 1; i++)
        {
            _depth3Btns[i].gameObject.SetActive(false);
        } 
        var currentDepth12 = Managers.ContentPlayManager.PlayData.CurrentDepthStatus[1].ToString() +
                             Managers.ContentPlayManager.PlayData.CurrentDepthStatus[2].ToString();
        Debug.Log($"depthCount {currentDepth12}");
        for (int i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_A+ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)]; i++)
        {
            _depth3Btns[i].gameObject.SetActive(true);
        } 
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
        if (_isTopMenuOn)
        {
            GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(true);
            isOnActiveArea = true;
        }
        else
        {
            GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
        }

    }

    private bool isOnActiveArea; // 뎁스3내용 Hover시 표출. 표출이후에도 내용범위에 머물러있으면 Hover상태 유지
    private void OnDpeth3ActiveAreaExit()
    {
        GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
        isOnActiveArea = false;
    }

    private void OnDpeth3ActiveAreaEnter()
    {
      
    }

    private void OnDepth3ABtnExit()
    {
     //Animatiorn
    }


    private void OnDepth3BtnClicked(int depth3Num)
    {
       
        Precheck();
        Managers.ContentPlayManager.PlayData.Depth3 = depth3Num;
        Managers.ContentPlayManager.PlayData.ResetOrSetDepthCount();
        ChangeTextWithAnim();

    
        
        for (int i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_E + 1; i++)
        {
            _depth3Btns[i].gameObject.SetActive(false);
        } 
        var currentDepth12 = Managers.ContentPlayManager.PlayData.CurrentDepthStatus[0].ToString() +
                             Managers.ContentPlayManager.PlayData.CurrentDepthStatus[1].ToString();
        Debug.Log($"depthCount {currentDepth12}");
        for (int i = (int)Btns.Depth3_A; i < (int)Btns.Depth3_A+ContentPlayData.DEPTH_THREE_COUNT_DATA[int.Parse(currentDepth12)]; i++)
        {
            _depth3Btns[i].gameObject.SetActive(true);
        } 
        
        ChangeTextWithAnim();
        
     
        
    }
    private void OnDepth3BtnEnter()
    {
   // OnAnimation
    }
    private void OnDepthThirdHideBtnExit()
    {
        isOnActiveArea = true;
        //GetObject((int)UI.UI_Depth3_List).gameObject.SetActive(false);
    }

    private void Precheck()
    {
        if (!_clickable)
        {
#if UNITY_EDITOR
            Debug.Log("Clicking Too Fast");
#endif
            return;
        }

        SetClickable();
    }

    private bool _isInstructAnimOn = true;
    private bool _isTopMenuOn = true;

    private void OnInstructionClicked()
    {
        Precheck();
        _isInstructAnimOn = !_isInstructAnimOn;
        _instructionAnimator.SetBool(UI_ON, _isInstructAnimOn);
#if UNITY_EDITOR
        Debug.Log($" Current Script UI Status{_isInstructAnimOn}");
#endif
    }
    
    private void OnTopMenuAnimBtnClicked()
    {
        Precheck();
        _isTopMenuOn = !_isTopMenuOn;
        _topMenuAnimator.SetBool(UI_ON, _isTopMenuOn);
#if UNITY_EDITOR
        Debug.Log($" topMenu Status: {_isTopMenuOn}");
#endif
    }
}