using UnityEngine;

public class UI_Main : UI_Popup
{
    private enum Btns
    {
        Btn_Start
    }


    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


     
        BindButton(typeof(Btns));

        GetButton((int)Btns.Btn_Start).gameObject.BindEvent(OnMainStartBtnClicked, Define.UIEvent.PointerUp);
        return true;
    }

    private void OnMainStartBtnClicked()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_DepthSelection>();
#if UNITY_EDITOR
        Debug.Log("Main Content Start");
#endif
    }
}