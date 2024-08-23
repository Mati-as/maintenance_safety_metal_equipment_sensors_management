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

        GetButton((int)Btns.Btn_Start).gameObject.BindEvent(OnMainStartBtnClicked, Define.UIEvent.PointerExit);
        return true;
    }

    private void OnMainStartBtnClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Main Content Start");
#endif
    }
}