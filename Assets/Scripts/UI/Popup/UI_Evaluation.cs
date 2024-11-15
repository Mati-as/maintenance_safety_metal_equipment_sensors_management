using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Evaluation : UI_Popup
{
    /*  뎁스4 평가하기 리스트 및 해당 항목별 점수
        1새 온도 센서 준비 10
        2작업 전 전원 차단 20
        3배관 잔류물 제거 20
        4온도 센서 접속나사 해체 10
        5단자 저항 측정 – 상온 15
        6접지 저항 측정 – OL 15
        7설비 전원 복구 10
     */

    //동적으로 바뀔일 없음


    //private Dictionary<int, int> _scoreMap;
    private enum Btns
    {
        //   Btn_Close,
        IncorrectCount,
        Btn_Main,
        Btn_Restart,
        Btn_Checklist_Hide
    }

    private enum UI
    {
        UI_ScoreBoard,
        UI_CheckList, // UI전체 객체 부모 
        CheckLists, //실제항목의 부모
        Eval_Items,
        Score_Stars,
        UI_OnWrongAnswer,
    }

    private void SetNumber()
    {
    }

    private enum TMPs
    {
        TotalScore,
        Head_CurrentEvalDepth,
        IncorrectCount,
        TMP_OverallEvalText//한글로 미흡 보통 우수 표시하는부분
    }

    private enum Images
    {
        EvalTutorialImageA,
        EvalTutorialImageB,
        EvalTutorialImageC
    }

    private Dictionary<int, TextMeshProUGUI> _textMap;
    private Dictionary<int, Image> _checklistImageMap;
    private int _evalId;
    private int _checklistId;
    private Image[] _starImages = new Image[3];
    

    private Animator _UIchecklistAnimator => GetObject((int)UI.UI_CheckList).GetComponent<Animator>();

    public bool isUiChecklistOn { get; private set; }


    public static event Action OnRestartBtnOnEvalClicked;
    private readonly int TMP_SCORE_CHILD_ORDER = 0;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        Debug.Assert(Managers.ContentInfo.PlayData.Depth1 == 4);

        BindImage(typeof(Images));
        BindTMP(typeof(TMPs));
        BindObject(typeof(UI));
        BindButton(typeof(Btns));

        
        
        for (int i = 0; i < 3; i++)
        {
           _starImages[i]= GetObject((int)UI.Score_Stars).transform.GetChild(i).GetComponent<Image>();
        }


        GetButton((int)Btns.Btn_Checklist_Hide).gameObject.BindEvent(() =>
        {
            isUiChecklistOn = !isUiChecklistOn;
            _UIchecklistAnimator.SetBool(Define.UI_ON, isUiChecklistOn);
        });


        _textMap = new Dictionary<int, TextMeshProUGUI>();
        _checklistImageMap = new Dictionary<int, Image>();


        _evalId = GetObject((int)UI.Eval_Items).GetInstanceID()/ -100; //int 범위로인한 나눗셈
        for (var i = 1; i <= GetObject((int)UI.Eval_Items).transform.childCount; i++)
        {
            _textMap.TryAdd(int.Parse(_evalId + i.ToString()),
                GetObject((int)UI.Eval_Items).transform.GetChild(i - 1).GetComponent<TextMeshProUGUI>());

            _textMap.TryAdd(int.Parse(_evalId.ToString() + i.ToString() + TMP_SCORE_CHILD_ORDER.ToString()),
                GetObject((int)UI.Eval_Items).transform.GetChild(i - 1).GetChild(TMP_SCORE_CHILD_ORDER)
                    .GetComponent<TextMeshProUGUI>());
        }


        _checklistId = GetObject((int)UI.CheckLists).GetInstanceID() / -100; //int 범위로인한 나눗셈
        for (var i = 1; i <= GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap.TryAdd(int.Parse(_checklistId.ToString() + i.ToString()),
                GetObject((int)UI.CheckLists).transform.GetChild(i-1).GetComponent<TextMeshProUGUI>());

            _checklistImageMap.TryAdd(int.Parse(_checklistId + i.ToString()),
                GetObject((int)UI.CheckLists).transform.GetChild(i-1).GetComponentInChildren<Image>());

            Logger.Log($"{i} : id({_checklistId.ToString() + i}) : 등록");
        }


        var currentDepthInfo = $"{Managers.ContentInfo.PlayData.Depth1}" +
                               $"{Managers.ContentInfo.PlayData.Depth2}" +
                               $"{Managers.ContentInfo.PlayData.Depth3}";

        GetButton((int)Btns.Btn_Main).gameObject.BindEvent(() =>
        {
            OnMainBtnClicked();
        });

        GetButton((int)Btns.Btn_Restart).gameObject.BindEvent(() =>
        {
            OnRestartBtnOnEvalClicked?.Invoke();
        });


        OnInit();
        RefreshText();

        return true;
    }

    public void OnInit()
    {
        Managers.evaluationManager.IsCorrectMapInit();
        _currentTotalScore = 0;

        GetObject((int)UI.UI_CheckList).SetActive(true);
        _UIchecklistAnimator.SetBool(Define.UI_ON, false);
        GetObject((int)UI.UI_ScoreBoard).SetActive(false);
        
        
        for (var i = 1; i <= GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap[int.Parse(_checklistId.ToString() + i.ToString())].color = _checklistIdleColor;
            _checklistImageMap[int.Parse(_checklistId + i.ToString())].enabled = false;
        }

        InitCheckListStatus();
    }

    private void SetStarImage(int index)
    {
        for (int i = 0; i < index; i++)
        {
            _starImages[i].enabled = true;
        }
    }

    private void SetStarImageAndText()
    {
        
        for (int i = 0; i < 3; i++)
        {
            _starImages[i].enabled = false;
         
        }
        
        if (_currentTotalScore < 30)
        {
            SetStarImage(1);
            GetTMP((int)TMPs.TMP_OverallEvalText).text = "미흡";
            return;
            return;
        }
        if (_currentTotalScore < 70)
        {
            SetStarImage(2);
            GetTMP((int)TMPs.TMP_OverallEvalText).text = "보통";
            return;

        }
        if (_currentTotalScore > 70)
        {
            GetTMP((int)TMPs.TMP_OverallEvalText).text = "우수";
            SetStarImage(3);

        }
    }

    public void OnEvalStart()
    {
        GetObject((int)UI.UI_CheckList).SetActive(true);
        _UIchecklistAnimator.SetBool(Define.UI_ON, true);
        GetObject((int)UI.UI_ScoreBoard).SetActive(false);
    }

    public void OnEvalFinish()
    {
        var currentDepth1 = Managers.ContentInfo.PlayData.Depth1;
        var currentDepth2 = Managers.ContentInfo.PlayData.Depth2;
        
        _currentTotalScore =
            Managers.evaluationManager.scorePerDepthMap[int.Parse(currentDepth1.ToString() + currentDepth2.ToString())];
        
        
        
        GetObject((int)UI.UI_CheckList).SetActive(true);
        // 평가용으로 비활성화 ----------------------------------------수정필요 
        GetObject((int)UI.UI_ScoreBoard).SetActive(false);
        ShowEvalItemScores();
        _UIchecklistAnimator.SetBool(Define.UI_ON, false);
        ShowTotalScore();
        SetStarImageAndText();
    }


    private void OnMainBtnClicked()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(SceneType.Main);
    }

    private void RefreshText()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();

        for (var i = 1; i <= GetObject((int)UI.Eval_Items).transform.childCount; i++)
            _textMap[int.Parse(_evalId + i.ToString())].text =
                Managers.Data.Texts[int.Parse(depth1 + depth2 + "0" + (i))].kor;

        for (var i = 1; i <= GetObject((int)UI.CheckLists).transform.childCount; i++)
            _textMap[int.Parse(_checklistId + i.ToString())].text =
                Managers.Data.Texts[int.Parse(depth1 + depth2 + "00" + (i))].kor;
    }

    private void ShowEvalItemScores()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        for (var i = 1; i <= GetObject((int)UI.Eval_Items).transform.childCount; i++)
        {
            var key = int.Parse(depth1 + depth2 + i);

            _textMap[int.Parse(_evalId + i.ToString() + TMP_SCORE_CHILD_ORDER)].text
                = Managers.evaluationManager.isCorrectMap[key]
                    ? Managers.evaluationManager.SCORE_PER_ITEM_MAP[key].ToString()
                    : 0.ToString();
            Logger.Log($"{i}번 문항 정답? {Managers.evaluationManager.isCorrectMap[key]} ");
        }
    }

    private int _currentTotalScore;
    private void ShowTotalScore()
    {
        var currentDepth1 = Managers.ContentInfo.PlayData.Depth1;
        var currentDepth2 = Managers.ContentInfo.PlayData.Depth2;
        
        DOVirtual.Float(0,_currentTotalScore, 2f,
            val => { GetTMP((int)TMPs.TotalScore).text = ((int)val).ToString(); });
        
        Logger.Log($"총점: {Managers.evaluationManager.scorePerDepthMap[int.Parse(currentDepth1.ToString() + currentDepth2.ToString())]}");
    }


    private readonly Color _checklistIdleColor = new(1, 1, 1, 1);
    private readonly Color _checklistDeactivateColor = new(0.7f, 0.7f, 0.8f, 1);


    /// <summary>
    ///     ///
    ///     <summary>
    ///         1.체크리스트 이미지 활성화 함수
    ///         2. State별 진행 상황에 따라 사용자가 직접 원하는 index까지 Activate할 수 있도록 구성
    ///     </summary>
    /// </summary>
    /// <param name="currentIndex" index 0 부터 CurrentIndex까지의 체크리스트를 활성화></param>
    public void RefreshCheckListImage(int currentIndex)
    {
        for (var i = 1; i <= currentIndex; i++)
        {
            _textMap[int.Parse(_checklistId + i.ToString())].color = _checklistDeactivateColor;
            _checklistImageMap[int.Parse(_checklistId + i.ToString())].enabled = true;
            Logger.Log($"{_checklistId.ToString() + i} : id({_checklistId}) : 비활성화");
        }
    }

    public void InitCheckListStatus()
    {
        for (var i = 1; i < GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap[int.Parse(_checklistId + i.ToString())].color = _checklistIdleColor;
            _checklistImageMap[int.Parse(_checklistId + i.ToString())].enabled = false;
        }
    }


    private Sequence _wrongUIAnimseq;
    
    private void AnimateUIWrongAnswer()
    {
        if (_wrongUIAnimseq != null && _wrongUIAnimseq.IsPlaying())
        {
            Logger.Log("중복실행방지");
            return;
        }
        
        _wrongUIAnimseq?.Kill();
        _wrongUIAnimseq = DOTween.Sequence();
        
        GetObject((int)UI.UI_OnWrongAnswer).transform.localScale = Vector3.zero;

        _wrongUIAnimseq.Append(GetObject((int)UI.UI_OnWrongAnswer).transform.DOScale(1f, 1f));
        _wrongUIAnimseq.Append(GetObject((int)UI.UI_OnWrongAnswer).transform.DOScale(0f, 1f));
    }

    // private void InitImage(int currentDepthInfo)
    // {
    //     Logger.Log($"current Image Info : {currentDepthInfo}");
    //     GetImage((int)Image.EvalTutorialImageA).sprite = 
    //         Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageA");
    //     GetImage((int)Image.EvalTutorialImageA).sprite = 
    //         Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageB");
    //     GetImage((int)Image.EvalTutorialImageA).sprite = 
    //         Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageC");
    // }
    //
}