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
     

        GetButton((int)DepthBtns.Btn_Depth1A).gameObject.BindEvent(OnDepthBtnAClicked, Define.UIEvent.PointerUp);
        GetButton((int)DepthBtns.Btn_Depth1B).gameObject.BindEvent(OnDepthBtnBClicked, Define.UIEvent.PointerUp);
        GetButton((int)DepthBtns.Btn_Depth1C).gameObject.BindEvent(OnDepthCtnCClicked, Define.UIEvent.PointerUp);
        return true;
    }

    private void OnDepthBtnAClicked()
    {

        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_ContentController>();
#if UNITY_EDITOR
        Debug.Log("Depth 1 기본학습 Start");
#endif
    }
    
    private void OnDepthBtnBClicked()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_ContentController>();
#if UNITY_EDITOR
        Debug.Log("Depth 2 안전 관리 Start");
#endif
    }
        private void OnDepthCtnCClicked()
        {
            Managers.UI.ClosePopupUI(this);
            Managers.UI.ShowPopupUI<UI_ContentController>();
    #if UNITY_EDITOR
            Debug.Log("Depth 3 계측 센서 정비 Start");
    #endif
            
        }
}
