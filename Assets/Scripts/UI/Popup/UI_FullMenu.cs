using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_FullMenu : UI_Popup
{
    private enum Btns
    {
        Btn_Close,
        //Status Name
        B11101,
        B12101,
        B21101,
        B22101,
        B23101,
        B31101,
        B32101,
        B33101,
        B34101,
        B35101
    }

    private enum Texts
    {
        text_Overall_Training_Goal,
        text_TrainingObjectInfo
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));
        BindTMP(typeof(Texts));

        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });

       GetText((int)Texts.text_Overall_Training_Goal).text = Managers.Data.Texts[Define.OverallTraningGoal].kor;
       GetText((int)Texts.text_TrainingObjectInfo).text = Managers.Data.Texts[Define.TrainingObjectInfo].kor;
      
        
        GetButton((int)Btns.B11101).gameObject.BindEvent(() => {LoadStep((11101).ToString()); });
        GetButton((int)Btns.B12101).gameObject.BindEvent(() => {LoadStep((12101).ToString()); });
        
        GetButton((int)Btns.B21101).gameObject.BindEvent(() => {LoadStep((21101).ToString()); });
        GetButton((int)Btns.B22101).gameObject.BindEvent(() => {LoadStep((22101).ToString()); });
        GetButton((int)Btns.B23101).gameObject.BindEvent(() => {LoadStep((23101).ToString()); });
        
        GetButton((int)Btns.B31101).gameObject.BindEvent(() => {LoadStep((31101).ToString()); });
        GetButton((int)Btns.B32101).gameObject.BindEvent(() => {LoadStep((32101).ToString()); });
        GetButton((int)Btns.B33101).gameObject.BindEvent(() => {LoadStep((33101).ToString()); });
        GetButton((int)Btns.B34101).gameObject.BindEvent(() => {LoadStep((34101).ToString()); });
        GetButton((int)Btns.B35101).gameObject.BindEvent(() => {LoadStep((35101).ToString()); });
        return true;
    }

    private void LoadStep(string CurrentStatus)
    {
        Managers.ContentInfo.PlayData.Depth1 = int.Parse(CurrentStatus[0].ToString());
        Managers.ContentInfo.PlayData.Depth2 =  int.Parse(CurrentStatus[1].ToString());
        Managers.ContentInfo.PlayData.Depth3 =  int.Parse(CurrentStatus[2].ToString());
        Managers.ContentInfo.PlayData.Count  =  int.Parse(CurrentStatus[4].ToString());
     
        Managers.UI.ClosePopupUI(Managers.UI.FindPopup<UI_FullMenu>());
        if(Managers.UI.FindPopup<UI_ContentController>() == null)Managers.UI.ShowPopupUI<UI_ContentController>();
        else
        {
            Managers.UI.ClosePopupUI(Managers.UI.FindPopup<UI_ContentController>());
            Managers.UI.ShowPopupUI<UI_ContentController>();
        }

        LoadScene(int.Parse(CurrentStatus[0].ToString()));
    }

    private void LoadScene(int scene)
    {
        Managers.UI.CloseAllPopupUI();
        switch (scene)
        {
            case 1 :
                SceneManager.LoadSceneAsync("Depth1A");
                break;
            case 2 :
                SceneManager.LoadSceneAsync("Depth1B");
                break;
            case 3 :
                SceneManager.LoadSceneAsync("Depth1C");
                break; 
            
        }
    }
}
