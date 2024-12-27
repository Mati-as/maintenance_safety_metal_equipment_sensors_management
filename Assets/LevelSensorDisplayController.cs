using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelSensorDisplayController : UI_Base
{

    public enum Mode // for referring array or making it string
    {
        Default_ValueCheck,
        SP1,
        rP1,
        SP2,
        rP2,
        MEdI,
        EF,
        rES,
        OutputSelection,
        ValueSetting,
        CircuitStatusSetting
    }

    enum UI
    {
        //depthc5Controller에서 클릭관리
        ModeOrEnterBtn,
        SetBtn,
    }
    public enum TMPs 
    {
        //LevelSensor의 경우 TMP_UGUI가 아니므로 주의합니다.
        LevelSensor_DisplayMain,
        LevelSensor_DisplayLightOut1,
        LevelSensor_DisplayLightOut2,
        LevelSensor_DisplayLightCm,
    }

    public const float SP1_DEFAULT_VALUE = 10.0f;// only compile-time
    public const float RP1_VALUE = 9.5f;
    public const string NULL_VALUE = "NULL_VALUE";// only compile-time
    public const string OUTPUT_ONE= "ou 1";// only compile-time
    public const string NORMAL_CLOSE = "Hnc"; //Hnc only compile-time
    public const string NORMAL_OPEN = "Hno";  //Hno only compile-time
    private string[] valuePerMode = new string[]
    {
        SP1_DEFAULT_VALUE.ToString("F1"),
        SP1_DEFAULT_VALUE.ToString("F1"),
        NULL_VALUE,
        NULL_VALUE,
        NULL_VALUE,
        NORMAL_CLOSE,
        NORMAL_CLOSE,
    };
    private readonly string[] DEFAULT_VALUES = new string[]
    {
        SP1_DEFAULT_VALUE.ToString("F1"),
        SP1_DEFAULT_VALUE.ToString("F1"),
        NULL_VALUE,
        NULL_VALUE,
        NULL_VALUE,
        NORMAL_CLOSE,
        NORMAL_CLOSE,
    };

    private readonly float MAX_VALUE = 12.0f;// only compile-time
    private string _currentDisplayValue = SP1_DEFAULT_VALUE.ToString("F1");
    public string currentDisplayValue
    {
        get
        {
            return _currentDisplayValue;
        }
        set
        {
            _currentDisplayValue = value;
            GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).text = _currentDisplayValue;
        }
    }

    public static event Action<Mode,string> OnModeChange;

    public bool isValueChangeModeUsable;
    
    private bool _isValueSettingMode;

    public bool isValueSettingMode
    {
        get
        {
            return _isValueSettingMode;
        }
        set 
        {
            if (value != _isValueSettingMode && value)
            {
                SetMode(Mode.ValueSetting);
                BlinkForNotifyingValueSettingMode();
            }
            
            _isValueSettingMode = value;
        }
    }

    
    private bool _isClickable;

    public bool isClickable
    {
        get
        {
            return _isClickable;
        }
        set
        {
            _isClickable = value;
        }
    }

    public bool isOn
    {
        get; private set;
        
    }

    private Mode _currentMode = Mode.Default_ValueCheck;

    public Mode currentMode
    {
        get
        {
            return _currentMode;
        }
        set
        {
            value = (Mode)((int)value % Enum.GetValues(typeof(Mode)).Length);

            _currentMode = value;
            if (_currentMode == Mode.Default_ValueCheck)
                GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).text = _currentDisplayValue;
            else GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).text = value.ToString();
            
            Logger.Log($"LevelSensor currentMode: ------------> {_currentMode}");
        }
        
    }

    public override bool Init()
    {

        BindNonUITMP(typeof(TMPs));
        BindObject(typeof(UI));
      
        PowerOffSensor();
        
        // 여기서 바인딩하면 안됩니다 (Scenecntroller가 Unbind함)------------------------
        // GetObject((int)UI.SetBtn).BindEvent(() =>
        // {
        //     isSetBtnClickingForValueSettingMode = true;
        // },Define.UIEvent.Pressed);
        // GetObject((int)UI.SetBtn).BindEvent(() =>
        // {
        //     isSetBtnClickingForValueSettingMode = true;
        // },Define.UIEvent.Click);
        // GetObject((int)UI.SetBtn).BindEvent(() =>
        // {
        //     isSetBtnClickingForValueSettingMode = false;
        // },Define.UIEvent.PointerUp);

        return true;
        
    }

    public void BootLevelSensorDisplay()
    {
        PowerOffSensor();
        
        valuePerMode = DEFAULT_VALUES;

        currentDisplayValue = DEFAULT_VALUES[(int)Mode.Default_ValueCheck].ToString();
        currentMode = Mode.Default_ValueCheck;

        TurnOn(TMPs.LevelSensor_DisplayMain);
        TurnOn(TMPs.LevelSensor_DisplayLightCm);
        TurnOn(TMPs.LevelSensor_DisplayLightOut2);

        isValueChangeModeUsable = false;
        isClickable = false;
        isOn = true;
    }

    public void PowerOffSensor()
    {
        GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).enabled =false;
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightOut1).enabled =false;
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightOut2).enabled =false;
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightCm).enabled =false;

        isOn = false;
    }


    private bool _isSetBtnClickable;
    private bool _isModeEnterBtnClickable;
    
    public void SetClickable(bool clickable =true, bool setBtn=false, bool modeBtn=false)
    {
        isClickable = clickable;
        _isSetBtnClickable = setBtn;
        _isModeEnterBtnClickable = modeBtn;
        
    }
    public void TurnOnSingleDisplayLight(TMPs light = (TMPs)(-1))
    {

        TurnOffAllTopGreenLight();
        
        if ((int)light == -1) return;
        GetNonUITMP((int)light).enabled =true;
    }
    
    public void TurnOffSingleDisplayLight(TMPs light)
    {
        GetNonUITMP((int)light).enabled =false;
    }


    public void TurnOffAllTopGreenLight()
    {
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightOut1).enabled =false;
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightOut2).enabled =false;
        GetNonUITMP((int)TMPs.LevelSensor_DisplayLightCm).enabled =false;
    }
   
    public int clickCountForChangingValue =-1;
    public bool isValueProperlySetToTwelve;
    

    public void SetSp1ValueByClick()
    {
        if (!isValueSettingMode)
        {
            Logger.Log($"isClickable :{isClickable} \n isValueSettingMode:{isValueSettingMode} ");
            return;
        }

        clickCountForChangingValue++;
        clickCountForChangingValue = Mathf.Clamp(clickCountForChangingValue, 0, 4);
        
        var value = ((clickCountForChangingValue) * 0.5f + 10.0f);
        value = Mathf.Clamp(value, 10.0f, 12.0f);
        currentDisplayValue = value.ToString("F1");

        isValueProperlySetToTwelve = value >= 12.0f ? true : false;

    }

    /// <summary>
    /// Exectute when the ValueSetting mode is available.
    /// **note** that it's not "When this is Value setting mode"
    /// </summary>
    public void OnValueSettingModeAccessableInit()
    {
        clickCountForChangingValue = -1; //중복클릭 처리 이슈로 인한 -1 설정
        isValueChangeModeUsable = true;
        isValueSettingMode = false;
        isValueProperlySetToTwelve = false;
    }


    public void OnModeEnterBtnClicked()
    {

        if (!isClickable || !_isModeEnterBtnClickable)
        {
            Logger.Log($"isClickable :{isClickable} \n _isModeEnterBtnClickable:{_isModeEnterBtnClickable} ");
            return;
        }
        isClickable = false;
        DOVirtual.DelayedCall(0.35f, () =>
        {
            isClickable = true;
        });
        
        
        switch (currentMode)
        {
            case Mode.Default_ValueCheck:
                currentMode = Mode.SP1;
                CurrentModeInit(Mode.SP1);
                break;
            
            
            
            case Mode.SP1:
                currentMode++;
                CurrentModeInit(currentMode);
                break;
            case Mode.rP1:
                currentMode++;
                CurrentModeInit(currentMode);
                break;
            case Mode.SP2:
                currentMode++;
                CurrentModeInit(currentMode);
                break;
            case Mode.rP2:
                currentMode++;
                CurrentModeInit(currentMode);
                break;
            case Mode.MEdI:
                currentMode++;
                CurrentModeInit(currentMode);
                break;
            case Mode.EF:
                currentMode = Mode.SP1;
                CurrentModeInit(currentMode);
                break;


            case Mode.rES:
                currentMode = Mode.CircuitStatusSetting;
                CurrentModeInit(Mode.CircuitStatusSetting);
                break;
            case Mode.OutputSelection:
                if (currentDisplayValue == NORMAL_OPEN)
                {
                    SetMode(Mode.Default_ValueCheck);
                }
                else if (currentDisplayValue == OUTPUT_ONE)
                {
                    currentDisplayValue = NORMAL_CLOSE;
                }
                else
                {
                    Logger.LogError("Invalid value for outputselection");
                }
                break;
            case Mode.ValueSetting:
                currentMode = Mode.SP1;
                CurrentModeInit(Mode.SP1);
                break;
            case Mode.CircuitStatusSetting:
                break;
        }
    }

    public void CurrentModeInit(Mode currentMode)
    {
        TurnOnSingleDisplayLight(TMPs.LevelSensor_DisplayLightOut2);
  
        switch (currentMode)
        {
            case Mode.Default_ValueCheck:
                
                GetNonUITMP((int)TMPs.LevelSensor_DisplayLightCm).enabled = true;
                break;
            
            
            case Mode.SP1:
                currentDisplayValue = Mode.SP1.ToString();
                break;
            case Mode.rP1:
                currentDisplayValue = Mode.rP1.ToString();
                break;
            case Mode.SP2:
                currentDisplayValue = Mode.SP2.ToString();
                break;
            case Mode.rP2:
                currentDisplayValue = Mode.rP2.ToString();
                break;
            case Mode.MEdI:
                currentDisplayValue = Mode.MEdI.ToString();
                break;
            case Mode.EF:
                currentDisplayValue = Mode.EF.ToString();
                break;
            
            case Mode.ValueSetting:
                BlinkForNotifyingValueSettingMode();
                currentDisplayValue = SP1_DEFAULT_VALUE.ToString("F1");
                break;
            
            
            case Mode.rES:
                currentDisplayValue = "rES";
                break;
            case Mode.OutputSelection:
                break;
            case Mode.CircuitStatusSetting:
                currentDisplayValue = "ou1";
                break;
        } 
        
        OnModeChange?.Invoke(this.currentMode,currentDisplayValue);
        Logger.Log($"OnModeChangeInvoke : CurrentMode : {this.currentMode},Current Display Value {currentDisplayValue}");
    }
    

    public void OnSetBtnClicked()
    {
        if (!isClickable || !_isSetBtnClickable)
        {
            Logger.Log($"isClickable :{isClickable} \n _isSetBtnClickable:{_isSetBtnClickable} ");
            return;
        }
        isClickable = false;
        DOVirtual.DelayedCall(0.35f, () =>
        {
            isClickable = true;
        });

        
        switch (currentMode)
        {
            case Mode.Default_ValueCheck:
                break;
            
            case Mode.SP1:
                _currentDisplayValue = SP1_DEFAULT_VALUE.ToString("F1");
                currentMode = Mode.Default_ValueCheck;
                break;
            case Mode.rP1:
                _currentDisplayValue = 9.5f.ToString("F1");
                currentMode = Mode.Default_ValueCheck;
                break;
            case Mode.SP2:
                break;
            case Mode.rP2:
                break;
            case Mode.MEdI:
                break;
            case Mode.EF:
                currentMode = Mode.rES;
                CurrentModeInit(Mode.rES);
                break;
            
            case Mode.ValueSetting:
                break;
             
            case Mode.rES:
                break;
            case Mode.OutputSelection:
                if (currentDisplayValue == NORMAL_CLOSE) currentDisplayValue = NORMAL_OPEN;
                break;
            case Mode.CircuitStatusSetting:
                break;
        }
    }
    
    public void SetMode(Mode mode)
    {
      
        isClickable = true;
     
        switch (mode)
        {
            case Mode.Default_ValueCheck:
                currentMode = mode;
                CurrentModeInit(mode);
                break;
            
            case Mode.SP1:
                currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.rP1:
                currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.SP2:
                 currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.rP2:
                 currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.MEdI:
                 currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.EF:
                 currentMode = mode;
                CurrentModeInit(mode);
                break;
            
            case Mode.ValueSetting:
                currentMode = mode;
                CurrentModeInit(mode);
                break;
             
            case Mode.rES:
                CurrentModeInit(mode);
                break;
            case Mode.OutputSelection:
                currentMode = mode;
                CurrentModeInit(mode);
                break;
            case Mode.CircuitStatusSetting:
                break;
        }
        
        
        if(mode != Mode.ValueSetting){  StopBlink();}
    }
    
    
    

    
    private void ChangeToValueSettingMode()
    {
       
      
        isValueSettingMode = true;
    }
    
    

    public bool isSetBtnClickingForValueSettingMode;
    private float _elapsed;
    private readonly float CLICK_DURATION_TO_CHANGE_TO_VALUE_SETTING_MODE = 2.5f;

    private float _elapsedForClickable;

    private void Update()
    {
        if (isValueChangeModeUsable &&
            !_isValueSettingMode 
            &&isSetBtnClickingForValueSettingMode )
        {
            Logger.Log($"isClicking :----------> {_elapsed}");
            _elapsed += Time.deltaTime;
            if (_elapsed > CLICK_DURATION_TO_CHANGE_TO_VALUE_SETTING_MODE)
            {
                _elapsed = 0;
                ChangeToValueSettingMode();
            }
            
        }
      
        if(!isSetBtnClickingForValueSettingMode)  _elapsed = 0;
        
    }

    private Sequence _blinkSeq;
    private void BlinkForNotifyingValueSettingMode()
    {
        Logger.Log($"Blink Invoke---------------------------------------");
        _blinkSeq?.Kill();
        
        _blinkSeq = DOTween.Sequence();
        
        _blinkSeq.AppendCallback(() =>
        {
            GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).enabled = false;
        });
        _blinkSeq.AppendInterval(0.35f);
        _blinkSeq.AppendCallback(() =>
        {
            GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).enabled = true;
        });
        _blinkSeq.AppendInterval(0.35f);
        _blinkSeq.SetLoops(15);
        _blinkSeq.Play();
    }
    private void StopBlink()
    {
        Logger.Log($"Stop Invoke---------------------------------------");
        GetNonUITMP((int)TMPs.LevelSensor_DisplayMain).enabled = true;
        _blinkSeq?.Kill();
    }

    public void TurnOn(TMPs display)
    {
        GetNonUITMP((int)display).enabled =true;
    }

}
