
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Evaluation : UI_Popup
{
    private enum Btns
    {
     //   Btn_Close,
        IncorrectCount,
        Btn_Main,
        Btn_Restart,
        Btn_Checklist_Hide,
        
    }
    private enum UI
    {
      UI_Score,
      UI_CheckList,// UI전체 객체 부모 
      CheckLists,//실제항목의 부모
      Eval_Items
        
    }

    private enum TMPs
    {
        TotalScore,
        Head_CurrentEvalDepth,
        IncorrectCount
    }
    
    private enum Image
    {
        EvalTutorialImageA,EvalTutorialImageB,EvalTutorialImageC
    }

    private Dictionary<int, TextMeshProUGUI> _textMap;
    private int _evalId;
    private int _checklistId;


    private Animator _UIchecklistAnimator
    {
        get { return GetObject((int)UI.UI_CheckList).GetComponent<Animator>(); }
    }

    public bool isUiChecklistOn { get; private set; }
    
    
    
    
    
    public static event Action OnRestartBtnOnEvalClicked; 
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

       
        
        Debug.Assert(Managers.ContentInfo.PlayData.Depth1 == 4 );
        
        BindImage(typeof(Image));
        BindTMP(typeof(TMPs));
        BindObject(typeof(UI));
        BindButton(typeof(Btns));
//        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });
       
        
        
        GetButton((int)Btns.Btn_Checklist_Hide).gameObject.BindEvent(() =>
        {
            isUiChecklistOn = !isUiChecklistOn;
            _UIchecklistAnimator.SetBool(Define.UI_ON,isUiChecklistOn);
        });
        
   
        _textMap = new Dictionary<int, TextMeshProUGUI>();
        
   
        _evalId = GetObject((int)UI.Eval_Items).GetInstanceID();
        for (int i = 0; i < GetObject((int)UI.Eval_Items).transform.childCount; i++)
        {
            _textMap.TryAdd(int.Parse(_evalId.ToString() + i.ToString()),
                GetObject((int)UI.Eval_Items).transform.GetChild(i).GetComponent<TextMeshProUGUI>());

        }


        _checklistId = GetObject((int)UI.CheckLists).GetInstanceID();
        for (int i = 0; i <  GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap.TryAdd(int.Parse(_checklistId.ToString() + i.ToString()),
                GetObject((int)UI.CheckLists).transform.GetChild(i).GetComponent<TextMeshProUGUI>());
              
        }
        
        
        
        
        var currentDepth1 = Managers.ContentInfo.PlayData.Depth2;
        DOVirtual.Float(0, (int)Managers.evaluationManager.scorePerDepthMap[currentDepth1], 2f, val =>
        {
            GetTMP((int)TMPs.TotalScore).text = ((int)val).ToString();
        });
       

        var currentDepthInfo = $"{Managers.ContentInfo.PlayData.Depth1}" +
                               $"{Managers.ContentInfo.PlayData.Depth2}" +
                               $"{Managers.ContentInfo.PlayData.Depth3}";
      
        GetButton((int)Btns.Btn_Main).gameObject.BindEvent((() =>
        {
            OnMainBtnClicked();
        }));
        
        GetButton((int)Btns.Btn_Restart).gameObject.BindEvent((() =>
        {
            Managers.UI.ClosePopupUI(this);
            OnRestartBtnOnEvalClicked?.Invoke();
        }));



        RefreshText();
        return true;
    }

    
    private void InitImage(int currentDepthInfo)
    {
    

        Logger.Log($"current Image Info : {currentDepthInfo}");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageA");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageB");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageC");
    }
    
    private void OnMainBtnClicked()
    {
        Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(SceneType.Main);
    }
    
    private void RefreshText()
    {
        var depth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        var depth1Int = Managers.ContentInfo.PlayData.Depth1;
        var depth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        var depth2Int = Managers.ContentInfo.PlayData.Depth2;
        var depth2Count = 1;
        var depth3Count = 1;
        
        for (int i = 0; i < GetObject((int)UI.Eval_Items).transform.childCount; i++)
        {
            _textMap[int.Parse(_evalId.ToString()+i.ToString())].text = Managers.Data.Texts[int.Parse(depth1 + depth2 + "0" + (i+1))].kor;
        }

        for (int i = 0; i <  GetObject((int)UI.CheckLists).transform.childCount; i++)
        {
            _textMap[int.Parse(_checklistId.ToString()+i.ToString())].text = Managers.Data.Texts[int.Parse(depth1 + depth2 + "00" +  (i+1))].kor;
        }
      
 
    }


}

