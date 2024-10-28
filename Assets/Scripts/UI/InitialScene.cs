using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialScene : BaseScene
{
    
    
#if UNITY_EDITOR
    public static float _logoShowTime = 2f;
#else
    public static float _logoShowTime = 2;
#endif
    
    
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Dev;
        
        
        Debug.Log($"ui stack count: {Managers.UI.PopupStack.Count}");
        Debug.Log("Scene Init------------------------------------------");
     
        
        StartCoroutine(InitialUIAnimationCo());
        return true;
    }
    
    private IEnumerator InitialUIAnimationCo()
    {
       
        Managers.UI.ShowPopupUI<UI_Loading>();
        Managers.UI.ShowPopupUI<UI_Logo>();
        

        yield return new WaitForSeconds(_logoShowTime);
        
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Loading>().GetComponent<UI_Loading>().PlayLoadingAnimAndLoadMain();
    
        
    }
}
