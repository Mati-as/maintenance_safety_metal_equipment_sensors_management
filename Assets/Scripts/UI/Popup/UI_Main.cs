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

        GetSlider((int)Slider.LanguageSetting).onValueChanged.AddListener(_=>
        {
            SetText();
        });
        
        Debug.Log("Main UI Init");
        
        
       
        Managers.UI_Persistent.OnMainUIPopUP();
        UI_Persistent.SetStatus(true);
        return true;
    }

    private void OnMainStartBtnClicked()
    {
        if (Managers.isTutorialPlayed)
        {
            Managers.UI.ClosePopupUI();
            Managers.UI.ShowPopupUI<UI_DepthSelection>();
        }
        else
        {
            Managers.UI.ClosePopupUI();
            Managers.UI.ShowPopupUI<UI_Tutorial>();
            Managers.isTutorialPlayed = true;
        }
      
        // Managers.UI.ShowPopupUI<UI_DepthSelection>();
    }

    private void SetText()
    {
        var currentContent = (int)GetSlider((int)Slider.LanguageSetting).value;

        if (currentContent == (int)Define.LanguageMode.Kor)
        {
            // 한국어는 볼드 + 흰색, 영어는 밝은 회색
            GetTMP((int)TMPs.Korean).text = "<color=#FFFFFF>한글</color>";
            GetTMP((int)TMPs.Eng).text = "<color=#9F9F9F>English</color>";
        }
        else
        {
            // 영어는 볼드 + 흰색, 한국어는 밝은 회색
            GetTMP((int)TMPs.Korean).text = "<color=#9F9F9F>한글</color>";
            GetTMP((int)TMPs.Eng).text = "<color=#FFFFFF>English</color>";
        }
    }

}