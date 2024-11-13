using System.Collections;
using UnityEngine;

public class Main_Initializer : BaseScene
{
#if UNITY_EDITOR
    public static float _logoShowTimeDuration = 1.5f;
#else
    public static float _logoShowTimeDuration = 2;
#endif


    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Dev;
        Logger.Log("Scene Init------------------------------------------");


        if (!Managers.initialIntroAnimPlayed)
        {
            
            StartCoroutine(InitialUIAnimationCo());
        }
        else
        {
            if(!Managers.UI.FindPopup<UI_Main>()) Managers.UI.ShowPopupUI<UI_Main>();
        }
        return true;
    }


    private IEnumerator InitialUIAnimationCo()
    {
        Managers.UI.ShowPopupUI<UI_Logo>();
        yield return new WaitForSeconds(_logoShowTimeDuration);

        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Loading>().GetComponent<UI_Loading>().PlayLoadingAnimAndLoadMain();
    }

    

    


}