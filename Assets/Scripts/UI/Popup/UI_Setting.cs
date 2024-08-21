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
       // -----------------
       Slider_FullScreen,
       Slider_Language,
       Slider_ControlGuideOn
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

    public readonly int OFF = 0;
    public readonly int ON = 1;

    private Button[] _btns;
    private Slider[] _volumeSliders;
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
        
        GetToggle((int)Toggles.Toggle_Main_Mute).onValueChanged.AddListener(isOn =>
        {
            if (GetToggle((int)Toggles.Toggle_Main_Mute).isOn)
            {
                Managers.Sound.SetMute(SoundManager.Sound.Main);
                Managers.Sound.SetMute(SoundManager.Sound.Narration);
                Managers.Sound.SetMute(SoundManager.Sound.Effect);
                Managers.Sound.SetMute(SoundManager.Sound.Bgm);
            }
            else
            {
                Managers.Sound.SetMute(SoundManager.Sound.Main,false);
                Managers.Sound.SetMute(SoundManager.Sound.Narration,false);
                Managers.Sound.SetMute(SoundManager.Sound.Effect,false);
                Managers.Sound.SetMute(SoundManager.Sound.Bgm,false);
            }
     
        });

        GetToggle((int)Toggles.Toggle_Narration_Mute).onValueChanged.AddListener(isOn => {
            if (GetToggle((int)Toggles.Toggle_Narration_Mute).isOn)
                Managers.Sound.SetMute(SoundManager.Sound.Narration);
            else
                Managers.Sound.SetMute(SoundManager.Sound.Narration,false);
            
        });

        GetToggle((int)Toggles.Toggle_Effect_Mute).onValueChanged.AddListener(isOn => {
            if (GetToggle((int)Toggles.Toggle_Effect_Mute).isOn)
                Managers.Sound.SetMute(SoundManager.Sound.Effect);
            else
              Managers.Sound.SetMute(SoundManager.Sound.Effect,false);

               
        });

        GetToggle((int)Toggles.Toggle_Bgm_Mute).onValueChanged.AddListener(isOn => {
            if (GetToggle((int)Toggles.Toggle_Bgm_Mute).isOn)
                Managers.Sound.SetMute(SoundManager.Sound.Bgm);
            else
                Managers.Sound.SetMute(SoundManager.Sound.Bgm,false);
               
        });

        GetSlider((int)Sliders.Slider_FullScreen).onValueChanged.AddListener(_ =>
        {
            if (GetSlider((int)Sliders.Slider_FullScreen).value >= ON)
                Managers.UI.SetFullScreenMode(UIManager.FULLSCREEN_ON);
            else
                Managers.UI.SetFullScreenMode(UIManager.FULLSCREEN_OFF);
        });
        
        
        GetSlider((int)Sliders.Slider_Language).onValueChanged.AddListener(_ =>
        {
            if (GetSlider((int)Sliders.Slider_FullScreen).value >= ON)
                Managers.UI.SetLanguageMode(UIManager.ENG);
            else
                Managers.UI.SetLanguageMode(UIManager.KOR);
        });
        
        
        GetSlider((int)Sliders.Slider_ControlGuideOn).onValueChanged.AddListener(_ =>
        {
            if (GetSlider((int)Sliders.Slider_ControlGuideOn).value >= ON)
                Managers.UI.SetControlGuideOnMode(UIManager.CONTROL_GUIDE_ON);
            else
                Managers.UI.SetControlGuideOnMode(UIManager.CONTROL_GUIDE_OFF);
        });
        
        
        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() =>
        {
            Managers.UI.ClosePopupUI(this);
        });
        
        SetVolumeSlider();

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



    private void SetVolumeSlider()
    {
        
        _volumeSliders = new Slider[(int)SoundManager.Sound.Max];
        for (int sliderIndex = 0; sliderIndex < (int)SoundManager.Sound.Max; sliderIndex++)
        {
            _volumeSliders[sliderIndex] = GetSlider(sliderIndex);
        }
        

        for (var i = 0; i < Managers.Sound.volumes.Length; i++)
        {
            _volumeSliders[i].maxValue = Managers.Sound.VOLUME_MAX[i];
            _volumeSliders[i].value = Managers.Sound.volumes[i];

#if UNITY_EDITOR
            if (i == (int)Sliders.Slider_Main)
            {
                Debug.Log($" 메인 볼륨 {Managers.Sound.volumes[i]}");
            }
#endif

            int audioIndex = i;
            _volumeSliders[i].onValueChanged.AddListener(_ =>
            {
                Managers.Sound.volumes[audioIndex] = _volumeSliders[audioIndex].value;
                
                if (audioIndex == (int)SoundManager.Sound.Effect)
                {
                    if (Managers.Sound.audioSources[(int)SoundManager.Sound.Effect] &&!Managers.Sound.audioSources[audioIndex].isPlaying)
                        Managers.Sound.Play(SoundManager.Sound.Effect, "Audio/Test_Effect");
                }
                
                
                if (audioIndex == (int)Sliders.Slider_Narration && !Managers.Sound.audioSources[audioIndex].isPlaying)
                {
                    Managers.Sound.Play(SoundManager.Sound.Narration, "Audio/Test_Narration");
                }

                Managers.Sound.audioSources[audioIndex].volume =
                    Mathf.Lerp(0, Managers.Sound.VOLUME_MAX[audioIndex],
                        Managers.Sound.volumes[(int)SoundManager.Sound.Main] *
                        _volumeSliders[audioIndex].value);

                if (audioIndex == (int)Sliders.Slider_Main)
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
