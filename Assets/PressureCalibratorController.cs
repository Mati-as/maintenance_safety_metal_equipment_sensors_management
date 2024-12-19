using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Assertions;

public class PressureCalibratorController : UI_Popup
{
    private bool _isOn;

    public bool isOn => _isOn; // 읽기 전용 프로퍼티

    public void SetIsOn(bool value)
    {
        if (_isOn == value) return; // 값이 변하지 않으면 작업하지 않음
        _isOn = value;
        SetAnimatorIsOn(_isOn);
    }
    
    private Animator _animator;
    private static readonly int IsOn = Animator.StringToHash("On");
    private Base_SceneController _currentSceneController;

    private UI _currentUI;
    
    //taskUI에서 loopPower와 continue버튼 필요
    private bool _isLoopPowerBtnClicked;
    private bool _isContinueClicked;
    
    //Animation
    public Dictionary<int, HighlightEffect> objectHighlightMap;
    protected Dictionary<int, Sequence> _seqMap;
    private Dictionary<int, string> _toolTipTextMap;

    public enum UI
    {
        //UI화면
        Default,// highlight,tooltip 중복방지를 위한 번호설정
        Tasks,
        SourcePressureAndMeasureCurrent,
        CalibrationMode,
        TestStrategy,
        Calibrating,
        // 버튼
        Btn_F1 , 
        Btn_F2,
        Btn_F3, //Tasks:LoopPower
        Btn_F4, //Tasks::Continue
        Btn_Tasks,
        Btn_Arrow_Down,
        Btn_Arrow_Up,
        Btn_Enter,
        Btn_Vent,
        Btn_Number_One,
        Btn_Number_Zero
        
    }




    private bool _isInit = false;
    public void Init()
    {
        base.Init();
        BindObject(typeof(UI));

        foreach (var key in _objects.Keys.ToArray())
        {
            Logger.Log($"length: {_objects[key].Length}");
        }

        OnDefaultUI();
        OnTaskUI();
     
        
        
       // BindhighlightAndToolTip();
        TurnOnUI(UI.Default);
        _currentUI = UI.Default;
        _isInit = true; 
    }

    private void OnDefaultUI()
    {
        
        GetObject((int)UI.Btn_Tasks).BindEvent(() =>
        { 
            if (_currentUI == UI.Default)
            {
                //tasks관련 초기화
                _isLoopPowerBtnClicked = false;
                _isContinueClicked = false;
                
                TurnOnUI(UI.Tasks);
            }
        });
    }
    
    private void OnTaskUI()
    {
        
        GetObject((int)UI.Btn_F3).BindEvent(() =>
        {
            if (_currentUI == UI.Tasks)
            {
                _isLoopPowerBtnClicked = true;
            }
        });
        GetObject((int)UI.Btn_F4).BindEvent(() =>
        {
            if (_currentUI == UI.Tasks && _isLoopPowerBtnClicked)
            {
                _isContinueClicked = true;
                TurnOnUI(UI.SourcePressureAndMeasureCurrent);
            }
            if(!_isLoopPowerBtnClicked) Logger.Log("F3:Loop Power Btn must be clicked first. ");
        });
    }
    
    private void OnSourcePressureAndMeasureCurrentUI()
    {
        
        GetObject((int)UI.Btn_Enter).BindEvent(() =>
        { 
            if (_currentUI == UI.Default)
            {
                //tasks관련 초기화
                _isLoopPowerBtnClicked = false;
                _isContinueClicked = false;
                
                TurnOnUI(UI.Tasks);
            }
        });
    }
    
    
    
    private void SetAnimatorIsOn(bool isCurrentlyOn)
    {
        if (_animator == null) transform.GetComponent<Animator>();
        _animator.SetBool(IsOn,isCurrentlyOn);
    }


    private void TurnOffAllUI()
    {
        GetObject((int)UI.Default).gameObject.SetActive(false);
        GetObject((int)UI.Tasks).gameObject.SetActive(false);
        GetObject((int)UI.SourcePressureAndMeasureCurrent).gameObject.SetActive(false);
        GetObject((int)UI.CalibrationMode).gameObject.SetActive(false);
        GetObject((int)UI.TestStrategy).gameObject.SetActive(false);
        GetObject((int)UI.Calibrating).gameObject.SetActive(false);
    }


    private void TurnOnUI(UI ui)
    {
        TurnOffAllUI();
        
        GetObject((int)ui).gameObject.SetActive(true);
        _currentUI = ui;
        
    }
    
  



}
