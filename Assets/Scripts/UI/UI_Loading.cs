using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UI_Loading : UI_Popup
{
    public TextMeshProUGUI tmp_loading;
    public Image progressBar;


    private enum UI
    {
        Progress
    }
    private enum TMP
    {
        TMP_LoadingGauge,
        
    }


    public override bool Init()
    {
        base.Init();
        BindTMP(typeof(TMP));
        BindObject(typeof(UI));

        tmp_loading = GetTMP((int)TMP.TMP_LoadingGauge);
        tmp_loading.text = "0%";

        progressBar = GetObject((int)UI.Progress).GetComponent<Image>();
        return true;
    }


    public void PlayLoadingAnimAndLoadMain()
    {
        DOVirtual.Float(0, 100, 2.5f, val =>
            {
                tmp_loading.text = $"{(int)val}%";
                progressBar.fillAmount = val / 100;
            })
            .OnComplete(() =>
            {
                Managers.UI.ClosePopupUI();
                Logger.Log("Main화면 표출");
                if (Managers.UI.SceneUI == null)
                {
                    Managers.UI_Persistent = Managers.UI.ShowSceneUI<UI_Persistent>();
                    Managers.UI_Persistent.Init();
                }

                Managers.UI_Persistent.PlayIntroAndShowAnimOnMainUI();

                Managers.Sound.Play(SoundManager.Sound.Bgm, "Bgm");
            });
    }
}