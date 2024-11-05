using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TrainingInfo : UI_Popup
{
    enum Texts
    {
        Head_Depth1, // 회색음영글자
        Head_Depth1_2, // Heading one 
        Text_TrainingGoal,
        Text_TrainingInfo,
    }

    public bool isInit;

    private static readonly string DEPTH1 ="0";  
    private static readonly string DEPTH2 ="1";  
    private static readonly string GOAL ="2";  
    private static readonly string INFO ="3";  


    public override bool Init()
    {
        if (!base.Init()) return false;
        if(!isInit)BindText(typeof(Texts));
        RefreshUI();


        isInit = true;
        return isInit;
    }

    public void RefreshUI()
    {
        
        
        Logger.Log("RefreshUI Objective -----------");
        
        GetText((int)Texts.Head_Depth1).text =
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
