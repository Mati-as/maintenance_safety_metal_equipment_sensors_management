using JetBrains.Annotations;
using UnityEngine;

public class UI_DepthSelection : UI_Popup
{
    private enum DepthBtns
    {
        Btn_DepthA,
        Btn_DepthB,
        Btn_DepthC2,
        Btn_DepthD2
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindButton(typeof(DepthBtns));

        GetButton((int)DepthBtns.Btn_DepthA).gameObject.BindEvent(OnDepthBtnAClicked);
        GetButton((int)DepthBtns.Btn_DepthB).gameObject.BindEvent(OnDepthBtnBClicked);
        GetButton((int)DepthBtns.Btn_DepthC2).gameObject.BindEvent(OnDepthCBtnClicked);
        GetButton((int)DepthBtns.Btn_DepthD2).gameObject.BindEvent(OnDepthDBtnCClicked);
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

    private void OnDepthCBtnClicked()
    {
       // Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 3 계측 센서 정비 Start");
#endif

        Managers.Scene.LoadScene(SceneType.DepthC2);
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2;  //구현 부분뎁스만큼 수정해야합니다.
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }
    private void OnDepthDBtnCClicked()
    {
        // Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 3 계측 센서 정비 Start");
#endif

        Managers.Scene.LoadScene(SceneType.DepthD2);
        Managers.ContentInfo.PlayData.Depth1 = 4;
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