using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_Popup
{
    public enum Btns
    {   
        
        Btn_Confirmation_Yes,
        Btn_Confirmation_No,
        Btn_Prev,
        Btn_Next,
        Btn_ToolBox,
        Btn_CameraInit,
       // Btn_Close
    }

    public enum TMPs
    {
        Text_Instruction
    }
    
    public enum UI
    {
        UI_Bottom,
        UI_ToolBox,
        UI_Confirmation
    }

    private bool _isTutorialEndedOrSkipped;
    private Tutorial_CameriaController _currentMainCam;
    public bool isTutorialEndedOrSkipped
    {
        get { return _isTutorialEndedOrSkipped; }
        set { _isTutorialEndedOrSkipped = value; }
    }
    
    public override bool Init()
    {
        
        BindTMP(typeof(TMPs));
        BindButton(typeof(Btns));
        BindObject(typeof(UI));
        _currentMainCam = Camera.main.GetComponent<Tutorial_CameriaController>();
        
             
        GetButton((int)Btns.Btn_CameraInit).gameObject.BindEvent(() =>
        {
            _currentMainCam.SetDefaultRotationThisState();
        });

        GetButton((int)Btns.Btn_Prev).gameObject.BindEvent(OnPrevBtnClicked);
        GetButton((int)Btns.Btn_Next).gameObject.BindEvent((OnNextBtnClicked));
        
            
        GetButton((int)Btns.Btn_ToolBox).gameObject.BindEvent(() =>
        {
            if (uiToolBox == null) uiToolBox = GetObject((int)UI.UI_ToolBox).GetComponent<UI_ToolBox>();
            GetObject((int)UI.UI_ToolBox).SetActive(true);

            uiToolBox.SetToolBox();
        });

        GetButton((int)Btns.Btn_Confirmation_Yes).gameObject.BindEvent(() =>
        {
#if UNITY_EDITOR
            Debug.Log("Tutorial Start");
#endif

            Managers.UI.SceneUI.GetComponent<UI_Persistent>().StopAnim();
            GetObject((int)UI.UI_Confirmation).gameObject.SetActive(false);
            GetObject((int)UI.UI_ToolBox).gameObject.SetActive(true);
            GetObject((int)UI.UI_Bottom).gameObject.SetActive(true);
        });


        GetButton((int)Btns.Btn_Confirmation_No).gameObject.BindEvent(() =>
        {
            Managers.UI.ShowPopupUI<UI_DepthSelection>();
            GetObject((int)UI.UI_Confirmation).gameObject.SetActive(false);
            isTutorialEndedOrSkipped = true;
        });
        
        
        // GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        // {
        //     GetObject((int)UI.UI_Confirmation).gameObject.SetActive(false);
        //     isTutorialEndedOrSkipped = true;
        // });
        
        GetObject((int)UI.UI_Confirmation).gameObject.SetActive(true);
        GetObject((int)UI.UI_ToolBox).gameObject.SetActive(false);
        GetObject((int)UI.UI_Bottom).gameObject.SetActive(false);

        isTutorialEndedOrSkipped = false;
        SetBtns();
        
        
        for (var i = 0; i < texts.Length; i++) texts[i] = GetTMP(i);
        SetHeighlightUIImages();
        ChangeInstructionText();
        return true;

    }
    
    private readonly TextMeshProUGUI[] texts = new TextMeshProUGUI[Enum.GetValues(typeof(TMPs)).Length];
    public static event Action<int,bool> OnStepBtnClicked_CurrentCount;
    private void OnPrevBtnClicked()
    {
  

        Debug.Assert(Managers.ContentInfo.PlayData.Count > 0);

        if (Managers.ContentInfo.PlayData.Count <= 1)
        {

            Logger.Log("currentCount is 1, Start Point");

            return;
        }

        Managers.ContentInfo.PlayData.Count--;


        ChangeInstructionText();
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,true);


       
    }
    private void OnNextBtnClicked()
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

        Managers.ContentInfo.PlayData.Count++;
        ChangeInstructionText();
        Logger.Log($"currentCount is {Managers.ContentInfo.PlayData.Count}");
        OnStepBtnClicked_CurrentCount?.Invoke(Managers.ContentInfo.PlayData.Count,false);
        
       
        
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

     private Dictionary<int, Image> _highlightImageMap;
     private Sequence _blinkBtnSeq;
     /// <summary>
    /// 하이라이트 기능이 있는 객체는 highlight 이미지를 할당해놓고, 아래 메소드에서 직접 초기화합니다.
    /// </summary>
    private void SetHeighlightUIImages()
    {
        if (_highlightImageMap == null) _highlightImageMap = new Dictionary<int, Image>();
        
        Image ToolboxImage = GetButton((int)(Btns.Btn_ToolBox))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_ToolBox, ToolboxImage);
        
        Image NextBtnImage = GetButton((int)(Btns.Btn_Next))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
        _highlightImageMap.TryAdd((int)Btns.Btn_Next, NextBtnImage);
        
        Image camInit = GetButton((int)(Btns.Btn_CameraInit))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name 
                                   && img.gameObject.name == "Highlight_Image");
        _highlightImageMap.TryAdd((int)Btns.Btn_Prev, camInit);
        
        Image PrevBtnImage = GetButton((int)(Btns.Btn_Prev))
            .GetComponentsInChildren<Image>(true) // Gets all Image components
            .FirstOrDefault(img => img.gameObject.name != gameObject.name && img.gameObject.name == "Highlight_Image"); 
      
        _highlightImageMap.TryAdd((int)Btns.Btn_Prev, PrevBtnImage);
    }

    public void BlinkBtnUI(int btnEnum)
    {
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


}
