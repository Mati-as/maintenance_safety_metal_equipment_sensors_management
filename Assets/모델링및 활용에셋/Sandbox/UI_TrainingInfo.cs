
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;


public class UI_TrainingInfo : UI_Popup
{
    enum Texts
    {
        Head_Depth1,
     //   Head_Depth3, // 회색음영글자 // 사용 X 011723
        Head_Depth1_2, // Heading one 
        Text_TrainingGoal,
        Text_TrainingInfo,
        Text_Confirm,
    }

    enum Images
    {
        Image_TrainingIntro
    }
    
    enum TMPs
    {
        TMP_TrainingGoal,
        TMP_TrainingInfo
    }

    public bool isInit;

    private static readonly string DEPTH1 ="0";  
    private static readonly string DEPTH2 ="1";  
    private static readonly string GOAL ="2";  
    private static readonly string INFO ="3";  


    public override bool Init()
    {
        if (!base.Init()) return false;
        
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindTMP(typeof(TMPs));
        
        
        
        RefreshUI();


        return true;
    }

    private void SetIntroImage()
    {
        var path = Managers.ContentInfo.PlayData.Depth1.ToString() + Managers.ContentInfo.PlayData.Depth2.ToString();
        var imageSource = Resources.Load<Sprite>("Image/UI_TrainingInfo/" + path);
        
        if (imageSource != null)
        {
            GetImage((int)Images.Image_TrainingIntro).sprite = imageSource;
        }
        else
        {
            Logger.LogWarning($"There's no image for training Info Image: ->{path} ");
        }
        
    }
    public void RefreshUI()
    {


        SetIntroImage();

        
        Logger.Log($"{Managers.ContentInfo.PlayData.CurrentDepthStatus} : Training Goal Refresh UI  -----------");
        
        GetText((int)Texts.Head_Depth1).text = Managers.GetText(int.Parse(Managers.ContentInfo.PlayData.Depth1 + "0000"));
        
        

        GetText((int)Texts.Head_Depth1_2).text =
            Managers.GetText(int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + DEPTH2));

        GetText((int)Texts.Text_TrainingGoal).text =
            Managers.GetText(int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + GOAL));

        GetText((int)Texts.Text_TrainingInfo).text =
            Managers.GetText(int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + INFO));

        GetTMP((int)TMPs.TMP_TrainingGoal).text = Managers.Data.IsEngMode() ? "Training Objectives" : "훈련 목표";
        GetTMP((int)TMPs.TMP_TrainingInfo).text = Managers.Data.IsEngMode() ? "Training Details" : "훈련 내용";
        GetText((int)Texts.Text_Confirm).text = Managers.Data.IsEngMode() ? "Confirm" : "확인";

    }
}
