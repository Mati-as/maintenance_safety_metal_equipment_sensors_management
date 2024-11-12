
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Evaluation : UI_Popup
{
    private enum Btns
    {
        Btn_Close,
        IncorrectCount,
        Btn_Main,
        Btn_Restart
        
    }

    private enum TMPs
    {
        EvalItem1,
        EvalItem2,
        EvalItem3,
        EvalItem4,
        EvalItem5,
        EvalItem6,
        EvalItem7,
        EvalItem8,
        EvalItem9,
        EvalItem10,
        TotalScore,
        Head_CurrentEvalDepth
    }
    
    private enum Image
    {
        EvalTutorialImageA,EvalTutorialImageB,EvalTutorialImageC
    }

    public static event Action OnRestartBtnOnEvalClicked; 
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        
        BindImage(typeof(Image));
        // BindObject(typeof(GameObj));
        
        BindButton(typeof(Btns));
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });
      
        BindTMP(typeof(TMPs));
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

}

