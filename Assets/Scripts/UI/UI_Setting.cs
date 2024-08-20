using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_Setting : UI_Popup
{
       private enum Btn_Type
    {
        Btn_Setting,
        Btn_Home,
        Btn_SensorRefresh,
        Btn_Quit,
        SettingCloseButton
     
    }
    
   public enum UI_Type
   {
       MainVolume,
       Narration,
       Effect,
       Bgm,
       Resolution,
       GraphicQuality,
       Language,
       ControlGuideOn
   }

   public enum ResolutionToggles
   {
       Resolution1280x720,
       Resolution1920x1080,
       Resolution2560x1440,
   }
   public enum GraphicQualityToggles
   {
       Low = 1,
       Mid = 3,
       High = 5,
       Auto 
   }


    //sensor-related part.-----------------------------------
    public static event Action OnRefreshEvent;
    public static event Action<string, DateTime> OnSceneQuit;
    public static event Action<string, DateTime> OnAppQuit;


    private Button[] _btns;
    // scene-related part -----------------------------------

    // Start is called before the first frame update
    private void Start()
    {
        BindButton(typeof(Btn_Type));
        BindObject(typeof(UI_Type));
        
        GetButton((int)Btn_Type.Btn_Home).gameObject.BindEvent(OnSceneQuitAndToHomeScreen);
        GetButton((int)Btn_Type.Btn_Home).gameObject.BindEvent(OnHomeBtnClicked);
        
        GetButton((int)Btn_Type.Btn_Quit).gameObject.BindEvent(OnQuit);
        GetButton((int)Btn_Type.Btn_SensorRefresh).gameObject.BindEvent(RefreshSensor);
        
        GetButton((int)Btn_Type.Btn_Setting).gameObject.BindEvent(OnSettingBtnClicked,Define.UIEvent.PointerUp);
        GetButton((int)Btn_Type.SettingCloseButton).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
        
        SetVolumeSlider();
    }

    private void RefreshSensor()
    {
      

       
        OnRefreshEvent?.Invoke();
    }

  

    private void OnSceneQuitAndToHomeScreen()
    {
        
    }

    private void OnQuit()
    {
        StartCoroutine(QuitApplicationCo());
    }

    private IEnumerator QuitApplicationCo()
    {
#if UNITY_EDITOR
        Debug.Log("App Quit ");
# endif

        OnAppQuit?.Invoke(SceneManager.GetActiveScene().name, DateTime.Now);
        yield return new WaitForSeconds(1f);
        Application.Quit();
       
    }
    



    private bool isSettingActive = false;
    public void OnSettingBtnClicked()
    {
     
    }
    
    
    public void OnHomeBtnClicked()
    {

    }

    private WaitForSeconds _waitForSceneChange =new WaitForSeconds(1.0f); 
    // private IEnumerator ChangeScene()
    // {
    // }

    /// <summary>
    /// 씬이동 초기화 수행 전, 다양한 초기화를 진행합니다.
    /// </summary>
    private void TerminateProcess()
    {
    }
    private Slider[] _volumeSliders = new Slider[(int)SoundManager.Sound.Max];

    private void SetVolumeSlider()
    {
        _volumeSliders = new Slider[(int)SoundManager.Sound.Max];

        for (var i = 0; i < (int)SoundManager.Sound.Max; i++)
        {
            _volumeSliders[i] = GetObject((int)(UI_Type)i).GetComponent<Slider>();
            _volumeSliders[i].maxValue = Managers.Sound.VOLUME_MAX[i];
            _volumeSliders[i].value = Managers.Sound.volumes[i];

#if UNITY_EDITOR
            if (i == (int)SoundManager.Sound.Main)
            {
                Debug.Log($" 메인 볼륨 {Managers.Sound.volumes[i]}");
            }
#endif

            int index = i;
            _volumeSliders[i].onValueChanged.AddListener(_ =>
            {
                Managers.Sound.volumes[index] = _volumeSliders[index].value;
                if (index == (int)SoundManager.Sound.Effect)
                {
                    Managers.Sound.Play(SoundManager.Sound.Effect, "Audio/TestSound/Test_Effect");
                }
                if (index == (int)SoundManager.Sound.Narration && !Managers.Sound.audioSources[index].isPlaying)
                {
                    Managers.Sound.Play(SoundManager.Sound.Narration, "Audio/TestSound/Test_Narration");
                }

                Managers.Sound.audioSources[index].volume =
                    Mathf.Lerp(0, Managers.Sound.VOLUME_MAX[index],
                        Managers.Sound.volumes[(int)SoundManager.Sound.Main] *
                        _volumeSliders[index].value);

                if (index == (int)SoundManager.Sound.Main)
                {
                    UpdateLinkedVolumes();
                }
            });
        }
    }

    private void UpdateLinkedVolumes()
    {
        UpdateVolume((int)SoundManager.Sound.Bgm);
        UpdateVolume((int)SoundManager.Sound.Effect);
        UpdateVolume((int)SoundManager.Sound.Narration);
    }

    private void UpdateVolume(int index)
    {
        Managers.Sound.volumes[index] = _volumeSliders[index].value;
        Managers.Sound.audioSources[index].volume =
            Mathf.Lerp(0, Managers.Sound.VOLUME_MAX[index],
                Managers.Sound.volumes[(int)SoundManager.Sound.Main] *
                _volumeSliders[index].value);
    }
}
