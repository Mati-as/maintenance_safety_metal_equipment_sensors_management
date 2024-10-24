using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Main : UI_Popup
{
    private enum Btns
    {
        Btn_Start
    }

    private enum UI
    {
        UI_Loading,
        UI_Logo
    }

    private enum TMP
    {
        TMP_LoadingGauge
    }

    private TextMeshProUGUI _tmp_loading;
    private UI_Popup main;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        main = this;


        BindButton(typeof(Btns));
        BindTMP(typeof(TMP));
        BindObject(typeof(UI));

        GetButton((int)Btns.Btn_Start).gameObject.BindEvent(OnMainStartBtnClicked);
        Debug.Log("Main UI Init");


        GetObject((int)UI.UI_Loading).SetActive(false);
        GetObject((int)UI.UI_Logo).SetActive(false);

        _tmp_loading = GetTMP((int)TMP.TMP_LoadingGauge);
        
        PlayMainLogoAndLodaingIntro();
        return true;
    }

    private void OnMainStartBtnClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Main Content Start");
#endif
        Managers.UI.ClosePopupUI(main);
        Managers.UI.ShowPopupUI<UI_DepthSelection>();
    }

    private void PlayMainLogoAndLodaingIntro()
    {
        GetObject((int)UI.UI_Logo).SetActive(true);


        DOVirtual.DelayedCall(2, () =>
        {
            GetObject((int)UI.UI_Logo).SetActive(false);
            PlayLoadingAnim();
        });
    }

    private void PlayLoadingAnim()
    {
        GetObject((int)UI.UI_Logo).SetActive(false);
        GetObject((int)UI.UI_Loading).SetActive(true);

        DOVirtual.Float(0, 100, 1.5f, val =>
        {

            _tmp_loading.text = $"{(int)val}%";
           
        }).OnComplete(() =>
        {
            GetObject((int)UI.UI_Loading).SetActive(false);
        });
    }
}