
using UnityEngine;
using UnityEngine.UI;

public class UI_Evaluation : UI_Popup
{
    private enum Btns
    {
        Btn_Close
        
    }

    private enum Image
    {
        EvalTutorialImageA,EvalTutorialImageB,EvalTutorialImageC
    }
   
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        
        BindImage(typeof(Image));

        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));

        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });

        var currentDepthInfo = $"{Managers.ContentInfo.PlayData.Depth1}" +
                               $"{Managers.ContentInfo.PlayData.Depth2}" +
                               $"{Managers.ContentInfo.PlayData.Depth3}";

        Logger.Log($"current Image Info : {currentDepthInfo}");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageA");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageB");
        GetImage((int)Image.EvalTutorialImageA).sprite = 
            Resources.Load<Sprite>("Image/"+currentDepthInfo + "EvalTutorialImageC");
        return true;
    }
}
