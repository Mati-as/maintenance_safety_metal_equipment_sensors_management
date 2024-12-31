
using Unity.VisualScripting;
using UnityEngine;


public class UI_TrainingInfo : UI_Popup
{
    enum Texts
    {
        Head_Depth1,
        Head_Depth3, // 회색음영글자
        Head_Depth1_2, // Heading one 
        Text_TrainingGoal,
        Text_TrainingInfo,
    }

    enum Images
    {
        Image_TrainingIntro
    }

    public bool isInit;

    private static readonly string DEPTH1 ="0";  
    private static readonly string DEPTH2 ="1";  
    private static readonly string GOAL ="2";  
    private static readonly string INFO ="3";  


    public override bool Init()
    {
        if (Managers.ContentInfo.PlayData.Depth1 == 5)
        {
            gameObject.SetActive(false);
            return false;
        }
        if (!base.Init()) return false;
        if(!isInit)BindText(typeof(Texts));
        BindImage(typeof(Images));
        RefreshUI();
        
        
      
        isInit = true;
        return isInit;
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

        
        Logger.Log("RefreshUI Objective -----------");
        
        GetText((int)Texts.Head_Depth1).text = Managers.Data.Texts[int.Parse(Managers.ContentInfo.PlayData.Depth1 + "0000")].kor;
        
        GetText((int)Texts.Head_Depth3).text =
            Managers.Data.Texts[int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + DEPTH1)].kor;

        GetText((int)Texts.Head_Depth1_2).text =
            Managers.Data.Texts[int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + DEPTH2)].kor;

        GetText((int)Texts.Text_TrainingGoal).text =
            Managers.Data.Texts[int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + GOAL)].kor;

        GetText((int)Texts.Text_TrainingInfo).text =
            Managers.Data.Texts[int.Parse($"{Managers.ContentInfo.PlayData.Depth1}" +
                                          $"{Managers.ContentInfo.PlayData.Depth2}" + INFO)].kor;
    }
}
