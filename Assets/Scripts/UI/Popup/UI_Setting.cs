using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_Setting : UI_Popup
{
       private enum Btns
    {
   
        Btn_Close
     
    }
    
   public enum GameObj
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

   public enum Sliders
   {
       Slider_Main,
       Slider_Narration,
       Slider_Effect,
       Slider_Bgm,
   }

   public enum Toggles
   {
       Toggle_Main_Mute, 
       Toggle_Narration_Mute, 
       Toggle_Effect_Mute,
       Toggle_Bgm_Mute, 
       Toggle_GraphicQuality_Low, 
       Toggle_GraphicQuality_Mid,
       Toggle_GraphicQuality_High, 
       Toggle_GraphicQuality_Auto,
       Toggle_Resolution_1280x720,
       Toggle_Resolution_1920x1080,
       Toggle_Resolution_2560x1440,
   }

    //sensor-related part.-----------------------------------
    public static event Action OnRefreshEvent;
    public static event Action<string, DateTime> OnSceneQuit;
    public static event Action<string, DateTime> OnAppQuit;


    private Button[] _btns;
    // scene-related part -----------------------------------

    // Start is called before the first frame update
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        
        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));
        BindToggle(typeof(Toggles));
        BindSlider(typeof(Sliders));
        
        GetToggle((int)Toggles.Toggle_Resolution_1280x720).onValueChanged.AddListener(_=>OnResolutionChanged(1280,720));
        GetToggle((int)Toggles.Toggle_Resolution_1920x1080).onValueChanged.AddListener(_=>OnResolutionChanged(1920,1080));
        GetToggle((int)Toggles.Toggle_Resolution_2560x1440).onValueChanged.AddListener(_=>OnResolutionChanged(2560,1440));
        
        GetToggle((int)Toggles.Toggle_GraphicQuality_Low).onValueChanged.AddListener(_=>OnGraphicQualityChanged(Define.QaulityLevel.Low));
        GetToggle((int)Toggles.Toggle_GraphicQuality_Mid).onValueChanged.AddListener(_=>OnGraphicQualityChanged(Define.QaulityLevel.Mid));
        GetToggle((int)Toggles.Toggle_GraphicQuality_High).onValueChanged.AddListener(_=>OnGraphicQualityChanged(Define.QaulityLevel.High));
        GetToggle((int)Toggles.Toggle_GraphicQuality_Auto).onValueChanged.AddListener(_=>OnGraphicQualityChanged(Define.QaulityLevel.Auto));
        
        
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
        
        //SetVolumeSlider();

        return true;
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
    private UnityEngine.UI.Slider[] _volumeSliders = new UnityEngine.UI.Slider[(int)SoundManager.Sound.Max];

    private void SetVolumeSlider()
    {
        _volumeSliders = new UnityEngine.UI.Slider[(int)SoundManager.Sound.Max];

        for (var i = 0; i < (int)SoundManager.Sound.Max; i++)
        {
            _volumeSliders[i] = GetObject((int)(GameObj)i).GetComponent<UnityEngine.UI.Slider>();
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

    private void OnResolutionChanged(int width, int height)
    {
#if UNITY_EDITOR
        Debug.Log($"Resolution Change => {width} x {height} ");
# endif
        Managers.UI.SetResolution(width,height,Managers.UI.isFullScreen);
    }
    
    private void OnGraphicQualityChanged(Define.QaulityLevel qaulityLevel)
    {
#if UNITY_EDITOR
        Debug.Log($"Graphic Quality {qaulityLevel}");
# endif
        Managers.UI.SetGraphicQuality(qaulityLevel);
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
