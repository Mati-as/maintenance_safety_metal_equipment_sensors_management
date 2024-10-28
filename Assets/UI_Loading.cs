using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using static DG.Tweening.DOVirtual;

public class UI_Loading : UI_Popup
{
    private enum TMP
    {
        TMP_LoadingGauge
    }

    public TextMeshProUGUI tmp_loading;

    public override bool Init()
    {
        base.Init();
        BindTMP(typeof(TMP));
        tmp_loading = GetTMP((int)TMP.TMP_LoadingGauge);
        
        tmp_loading.text = "0%";
        gameObject.SetActive(true);
        return true;
    }

    public void PlayLoadingAnimAndLoadMain()
    {
        Float(0, 100, 1.5f, val => { tmp_loading.text = $"{(int)val}%"; })
            .OnComplete(() =>
            {
                Logger.Log("Main화면 표출");
                Managers.UI.ClosePopupUI();
                Managers.UI.ShowSceneUI<UI_Persistent>();
                Managers.UI.ShowPopupUI<UI_Main>();
                
                Managers.Sound.Play(SoundManager.Sound.Bgm, "Bgm");
            });
    }
    
    public void PlayLoadingAnimOnSceneLoad()
    {
        tmp_loading.text = "0%";
        Float(0, 100, 1.0f, val => { tmp_loading.text = $"{(int)val}%"; })
            .OnComplete(() => { Managers.UI.ClosePopupUI(this); });
    }
}
