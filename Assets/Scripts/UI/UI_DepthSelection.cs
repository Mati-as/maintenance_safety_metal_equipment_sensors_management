using JetBrains.Annotations;
using UnityEngine;

public class UI_DepthSelection : UI_Popup
{
    private enum DepthBtns
    {
        Btn_Depth1A,
        Btn_Depth1B,
        Btn_Depth1C
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindButton(typeof(DepthBtns));

        GetButton((int)DepthBtns.Btn_Depth1A).gameObject.BindEvent(OnDepthBtnAClicked);
        GetButton((int)DepthBtns.Btn_Depth1B).gameObject.BindEvent(OnDepthBtnBClicked);
        GetButton((int)DepthBtns.Btn_Depth1C).gameObject.BindEvent(OnDepthCtnCClicked);
        return true;
    }

    private void OnDepthBtnAClicked()
    {
//Managers.UI.ClosePopupUI(this);
      
        Managers.Scene.LoadScene(SceneType.Depth1A);
#if UNITY_EDITOR
        Debug.Log("Depth 1 기본학습 Start");
#endif

        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }

    private void OnDepthBtnBClicked()
    {
      //  Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 2 안전 관리 Start");
#endif
        Managers.Scene.LoadScene(SceneType.Depth1B);
        Managers.ContentInfo.PlayData.Depth1 = 2;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        //OnDepth1Enter();
    }

    private void OnDepthCtnCClicked()
    {
       // Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 3 계측 센서 정비 Start");
#endif

        Managers.Scene.LoadScene(SceneType.Depth1C);
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;  //구현 부분뎁스만큼 수정해야합니다.
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }

    private void OnDepth1Enter()
    {
        Managers.Sound.Pause(SoundManager.Sound.Bgm);
    }
}