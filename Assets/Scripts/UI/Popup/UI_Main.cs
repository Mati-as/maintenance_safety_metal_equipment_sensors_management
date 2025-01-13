using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Main : UI_Popup
{
    private enum Btns
    {
        Btn_Start
    }

    private enum TMPs
    {
        Title_Heading1,
        Title_Heading2,
        TMP_Start,
        
        TMP_ClickInducing,
        
        Korean,
        Eng
    }


    private enum Slider
    {
        LanguageSetting
    }

    private UI_Popup main;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        main = this;


        BindTMP(typeof(TMPs));
        BindButton(typeof(Btns));
        BindSlider(typeof(Slider));
        GetButton((int)Btns.Btn_Start).gameObject.BindEvent(OnMainStartBtnClicked);

        GetSlider((int)Slider.LanguageSetting).value = (int)Managers.Data.Preference[(int)Define.Preferences.IsEng];
        GetSlider((int)Slider.LanguageSetting).onValueChanged.AddListener(_=>
        {
            SetText();
          
        });
        
        Debug.Log("Main UI Init");
        
        
       
        Managers.UI_Persistent.OnMainUIPopUP();
        UI_Persistent.SetStatus(true);
        Managers.UI_Persistent.FadeIn(1.8f);
        RefreshText();
        SetText();
        return true;
    }

    private void OnMainStartBtnClicked()
    {
        if ((int)Managers.Data.Preference[(int)Define.Preferences.ControlGuide] == Define.YES
            && !Managers.isTutorialAlreadyPlayed)
        {
            Managers.UI.ClosePopupUI();
            Managers.UI.ShowPopupUI<UI_Tutorial>();
           
        }
        else
        {
            Managers.UI.ClosePopupUI();
            Managers.UI.ShowPopupUI<UI_DepthSelection>();

        }
      
        // Managers.UI.ShowPopupUI<UI_DepthSelection>();
    }

    
    private const float ENG_SIZE_HEAD1_ENG = 45;
    private const float KOR_SIZE_HEAD1_KOR = 70;
    private const float ENG_SIZE = 26;
    private const float KOR_SIZE = 35;

    public override void RefreshText()
    {
        if (Managers.Data.IsEngMode())
        {
            GetTMP((int)TMPs.Title_Heading1).fontSize = Managers.Data.IsEngMode() ? ENG_SIZE_HEAD1_ENG : KOR_SIZE_HEAD1_KOR;
            //GetTMP((int)TMPs.TMP_DepthB).fontSize = Managers.Data.CheckIfEngMode() ? ENG_SIZE : KOR_SIZE;
            GetTMP((int)TMPs.Title_Heading1).text =
                "Maintenance and Safety Management of\n Metal Manufacturing Equipment and Measurement Sensors";
            GetTMP((int)TMPs.Title_Heading2).text = "금속제조설비 계측센서 정비 및 안전관리";
        }
        else
        {
            GetTMP((int)TMPs.Title_Heading1).fontSize = Managers.Data.IsEngMode() ? ENG_SIZE_HEAD1_ENG : KOR_SIZE_HEAD1_KOR;
            // GetTMP((int)TMPs.TMP_DepthB).fontSize = Managers.Data.CheckIfEngMode() ? ENG_SIZE : KOR_SIZE;
           
            GetTMP((int)TMPs.Title_Heading1).text = "금속제조설비 계측센서\n정비 및 안전관리";
            GetTMP((int)TMPs.Title_Heading2).text =
                "Maintenance and Safety Management of\nMetal Manufacturing Equipment and Measurement Sensors";
        }

        GetTMP((int)TMPs.TMP_Start).text = Managers.Data.IsEngMode() ? "START" : "시작하기";
       // GetTMP((int)TMPs.TMP_Start).fontSize = Managers.Data.CheckIfEngMode() ? ENG_SIZE : KOR_SIZE;
       
       GetTMP((int)TMPs.TMP_ClickInducing).text = Managers.Data.IsEngMode() ? "   Click the Start button to initiate the training" : "시작하기 버튼을 클릭하여 훈련을 시작하세요";
    
    }



    private void SetText()
    {
        var currentContent = (int)GetSlider((int)Slider.LanguageSetting).value;

        if (currentContent == (int)Define.LanguageMode.Kor)
        {
            // 한국어는 볼드 + 흰색, 영어는 밝은 회색
            GetTMP((int)TMPs.Korean).text = "<color=#FFFFFF>한글</color>";
            GetTMP((int)TMPs.Eng).text = "<color=#9F9F9F>English</color>";
            Managers.Data.Preference[(int)Define.Preferences.IsEng] = (int)Define.LanguageMode.Kor;

        }
        else
        {
            // 영어는 볼드 + 흰색, 한국어는 밝은 회색
            GetTMP((int)TMPs.Korean).text = "<color=#9F9F9F>한글</color>";
            GetTMP((int)TMPs.Eng).text = "<color=#FFFFFF>English</color>";
            Managers.Data.Preference[(int)Define.Preferences.IsEng] = (int)Define.LanguageMode.Eng;
        }

        Managers.UI_Persistent.RefreshText();
        RefreshText();
    }

}