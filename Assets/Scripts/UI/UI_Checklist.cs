using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Checklist : UI_Popup
{
    private enum UI
    {
        UI_ScoreBoard,
        UI_CheckList, // UI전체 객체 부모 
        CheckLists, //실제항목의 부모
    
    }
    
    private enum Btns
    {
        Btn_Checklist_Hide
    }
    
    private Dictionary<int, TextMeshProUGUI> _textMap;
    private Dictionary<int, Image> _checklistImageMap;

    private int _checklistId;
    private readonly Color _checklistIdleColor = new(1, 1, 1, 1);
    private readonly Color _checklistDeactivateColor = new(0.7f, 0.7f, 0.8f, 1);

    private Animator _UIchecklistAnimator;

    private Animator thisAnimator
    {
        get
        {

            if (_UIchecklistAnimator == null) return this.gameObject.GetComponent<Animator>();
            else
            {
                return _UIchecklistAnimator;
            }
        }

    }
    public void SetChecklistAnimStatus(bool isOn)
    {
        if (thisAnimator != null)
        {
            GetObject((int)UI.UI_CheckList).GetComponent<Animator>();
            thisAnimator.SetBool(Define.UI_ON, isOn);
        }
    }

    public bool isUiChecklistOn { get; private set; }
    
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
   
        BindTMP(typeof(LevelSensorDisplayController.TMPs));
        BindObject(typeof(UI));
        BindButton(typeof(Btns));

        


        GetButton((int)Btns.Btn_Checklist_Hide).gameObject.BindEvent(() =>
        {
            isUiChecklistOn = !isUiChecklistOn;
            thisAnimator.SetBool(Define.UI_ON, isUiChecklistOn);
        });


        _textMap = new Dictionary<int, TextMeshProUGUI>();
        _checklistImageMap = new Dictionary<int, Image>();
        

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

        if(thisAnimator!=null) thisAnimator.SetBool(Define.UI_ON, false);

        OnInitUI();
        RefreshText();
      
        return true;
    }

    private void RefreshText()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        
        for (var i = 1; i <= GetObject((int)UI.CheckLists).transform.childCount; i++)
            _textMap[int.Parse(_checklistId + i.ToString())].text =
                Managers.Data.Texts[int.Parse(depth1 + depth2 + "00" + (i))].kor;
    }

    
    public void OnInitUI()
    {
        Managers.EvaluationManager.IsCorrectMapInit();
        
        if(thisAnimator!=null)thisAnimator.SetBool(Define.UI_ON, false);
        GetObject((int)UI.UI_ScoreBoard).SetActive(false);
        
        
        for (var i = 1; i <= GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap[int.Parse(_checklistId.ToString() + i.ToString())].color = _checklistIdleColor;
            _checklistImageMap[int.Parse(_checklistId + i.ToString())].enabled = false;
        }

        InitCheckListStatus();
    }

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

    public void Show() => thisAnimator.SetBool(Define.UI_ON,true);
    public void Hide() => thisAnimator.SetBool(Define.UI_ON,false);

}
