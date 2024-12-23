using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

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
    private bool _isVented;
    private bool _isLoopPowerBtnClicked;
    private bool _isContinueClicked;
    
    //Animation
    public Dictionary<int, HighlightEffect> objectHighlightMap;
    protected Dictionary<int, Sequence> _seqMap;
    private Dictionary<int, string> _toolTipTextMap;
    private Dictionary<int, Sequence> _textAnimSeqMap = new Dictionary<int, Sequence>();

    public enum UI
    {
        //UI화면
        Default,// highlight,tooltip 중복방지를 위한 번호설정
        Tasks,
        PressureAndMeasureSetting,
        CalibrationModeSetting,
        TestStrategy,
        Calibrating,
        CalibrationFinish,
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
        Btn_Number_Zero,
        SelectedYellowBgA,SelectedYellowBgB,
        TestStrategySelected,
        ToleranceSelected,
        NotReady_Bg,
        Stable_Bg,
        
    }

    public enum NonUITMPs
    {
        Pressure100Psi,
        TMP_TestStrategy,
        Vented,
        TMP_Psi,
        TMP_Calibrating_PsiNum,
        TMP_Calibrating_Current
    }




    private bool _isInit = false;
    public void Init()
    {
        base.Init();
        BindObject(typeof(UI));
        BindNonUITMP(typeof(NonUITMPs));
        

        TurnOnUI(UI.Default);
        BootPressureCalibrator();
        gameObject.SetActive(false);
        
      //  _isInit = true; 
    }

    public void BootPressureCalibrator()
    {
        gameObject.SetActive(true);
        TurnOnUI(UI.Default);
        _textAnimSeqMap.TryAdd((int)NonUITMPs.TMP_Psi, DOTween.Sequence());
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi]?.Kill();
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi] = DOTween.Sequence();
        
        var lastUpdateTime = 0f;
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendCallback(() =>
        {

            DOVirtual.Float(0, 1, 1, _ =>
            {
                var currentTime = Time.time;
                if (currentTime - lastUpdateTime >= 0.2f)
                {
                    GetNonUITMP((int)NonUITMPs.TMP_Psi).text = (Random.Range(0.050f, 0.052f)).ToString("F3");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);;
        });

        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].SetLoops(-1);
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].Play();
    }


    public void OnVentClicked()
    {
        Assert.IsTrue(_textAnimSeqMap.ContainsKey((int)NonUITMPs.TMP_Psi));
        
        if (_isVented) return;
        _isVented = true;
        //초기화
        _isLoopPowerBtnClicked = false;
        _isContinueClicked = false;

    
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi]?.Kill();
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi] = DOTween.Sequence();
        
        var lastUpdateTime = 0f;

        GetNonUITMP((int)NonUITMPs.Vented).text = string.Empty;
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendCallback(() =>
        {
           

            DOVirtual.Float(0.050f, 0f, 2.5f, val =>
            {
                if(val < 0.02)   GetNonUITMP((int)NonUITMPs.Vented).text = "VENTED";
                var currentTime = Time.time;
                if (currentTime - lastUpdateTime >= 0.3f)
                {
                    GetNonUITMP((int)NonUITMPs.TMP_Psi).text = (val).ToString("F3");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);;
        });

        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendInterval(1.5f);
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendCallback(() =>
        {
            OnVentFinished();
        });
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].Play();
    }

    public void OnZeroPressureClicked()
    {
      
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi]?.Kill();
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi] = DOTween.Sequence();
        
        var lastUpdateTime = 0f;

     
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendCallback(() =>
        {

            GetNonUITMP((int)NonUITMPs.TMP_Psi).text = (0.000f).ToString("F3");
        });
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].SetLoops(-1);
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].Play();
     
   
    }
    public void OnVentFinished()
    {
        Assert.IsTrue(_textAnimSeqMap.ContainsKey((int)NonUITMPs.TMP_Psi));
        
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi]?.Kill();
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi] = DOTween.Sequence();
        
        var lastUpdateTime = 0f;

     
        _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].AppendCallback(() =>
        {

            DOVirtual.Float(0, 0, 1, _ =>
            {
                var currentTime = Time.time;
                if (currentTime - lastUpdateTime >= 0.25f)
                {
                    GetNonUITMP((int)NonUITMPs.TMP_Psi).text = Random.Range(-0.003f,-0.001f).ToString("F3");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);;
        });
        
          _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].SetLoops(-1);
            _textAnimSeqMap[(int)NonUITMPs.TMP_Psi].Play();

    }

    public void OnTasksBtnClicked()
    {
        //tasks관련 초기화
        _isLoopPowerBtnClicked = false;
        _isContinueClicked = false;

        if (_currentUI == UI.Default)
        {
            TurnOnUI(UI.Tasks);
        }
    
    }

    public void OnBtn_F3Clicked()
    {
        //The Case of _isLoopPowerBtn
        if (_currentUI == UI.PressureAndMeasureSetting)
        {
            _isLoopPowerBtnClicked = true;
        }
        
        if (_currentUI == UI.Default)
        {
            OnZeroPressureClicked();
        }

       
    }

    public void OnBtn_F4Clicked() //AUTO TEST OR CONTINUE...
    {
        if (_currentUI == UI.PressureAndMeasureSetting && _isLoopPowerBtnClicked)
        {
            _isContinueClicked = true;
            _hasDownToTestStrategy = false;
            
            TurnOnUI(UI.CalibrationModeSetting);
            GetObject((int)UI.ToleranceSelected).gameObject.SetActive(true);
            GetObject((int)UI.TestStrategySelected).gameObject.SetActive(false);
            return;
        }

        if (!_isLoopPowerBtnClicked)
        {
            Logger.Log("F3:Loop Power Btn must be clicked first. ");
            return;
        }
        
        
        if (_currentUI == UI.CalibrationModeSetting)
        {
            TurnOnUI(UI.Calibrating);
            
        }
        
        if (_currentUI == UI.Calibrating)
        {
           CalibratePressure();
        }
               
 
    }

    private bool _numberOneClickedOnSpanSetSession;
    private bool _numberZeroClickedOneTimeOnSpanSetSession;
    
    public void OnBtnNumberOneClicked()
    {
        if (_currentUI == UI.Tasks)
        {
            _hasDownToPressureSpanPoint = false;
            _numberZeroClickedOneTimeOnSpanSetSession = false;
            
            GetNonUITMP((int)NonUITMPs.Pressure100Psi).text = "300.000psi";
            TurnOnUI(UI.PressureAndMeasureSetting);
            
            GetObject((int)UI.SelectedYellowBgA).gameObject.SetActive(true);
            GetObject((int)UI.SelectedYellowBgB).gameObject.SetActive(false);
            return;
        }
        
        if (_currentUI == UI.PressureAndMeasureSetting)
        {
            GetNonUITMP((int)NonUITMPs.Pressure100Psi).text = "1";
        }
    }

    public void OnBtnNumberZeroClicked()
    {
        if (_currentUI == UI.PressureAndMeasureSetting && !_numberZeroClickedOneTimeOnSpanSetSession)
        {
            DOVirtual.DelayedCall(0.75f,()=>
            {
                _numberZeroClickedOneTimeOnSpanSetSession = true;
            });
            
            GetNonUITMP((int)NonUITMPs.Pressure100Psi).text = "10";
        }
        
        if (_currentUI == UI.PressureAndMeasureSetting && _numberZeroClickedOneTimeOnSpanSetSession)
        {
            GetNonUITMP((int)NonUITMPs.Pressure100Psi).text = "100";
        }
    }

    private bool _hasDownToPressureSpanPoint;
    private bool _hasDownToTestStrategy;
    public bool is100PsiSet;
    public void OnEnterBtnClicked()
    {
        if (_currentUI == UI.Default)
        {
            //tasks관련 초기화
            _isLoopPowerBtnClicked = false;
            _isContinueClicked = false;
                
            TurnOnUI(UI.Tasks);
        }
        
        if (_currentUI == UI.CalibrationModeSetting)
        {
            TurnOnUI(UI.TestStrategy);
        }
        
        if (_currentUI == UI.TestStrategy)
        {
            TurnOnUI(UI.CalibrationModeSetting);
        }
        
                
        if (_currentUI == UI.PressureAndMeasureSetting && _numberZeroClickedOneTimeOnSpanSetSession)
        {
            GetNonUITMP((int)NonUITMPs.Pressure100Psi).text = "100.000psi";
            is100PsiSet = true;
        }
    }
    public void OnDownBtnClicked()
    {
        if (_currentUI == UI.PressureAndMeasureSetting)
        {
            GetObject((int)UI.SelectedYellowBgA).gameObject.SetActive(false);
            GetObject((int)UI.SelectedYellowBgB).gameObject.SetActive(true);
            _hasDownToPressureSpanPoint = true;
        }
        
        
        if (_currentUI == UI.PressureAndMeasureSetting)
        {
            GetObject((int)UI.SelectedYellowBgA).gameObject.SetActive(false);
            GetObject((int)UI.SelectedYellowBgB).gameObject.SetActive(true);
            _hasDownToPressureSpanPoint = true;
        }
        
        if (_currentUI == UI.CalibrationModeSetting)
        {
            GetObject((int)UI.ToleranceSelected).gameObject.SetActive(false);
            GetObject((int)UI.TestStrategySelected).gameObject.SetActive(true);
            _hasDownToTestStrategy = true;
            return;
        }

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
        GetObject((int)UI.PressureAndMeasureSetting).gameObject.SetActive(false);
        GetObject((int)UI.CalibrationModeSetting).gameObject.SetActive(false);
        GetObject((int)UI.TestStrategy).gameObject.SetActive(false);
        GetObject((int)UI.Calibrating).gameObject.SetActive(false);
        GetObject((int)UI.CalibrationFinish).gameObject.SetActive(false);
    }


    public void TurnOnUI(UI ui)
    {
        
        _hasDownToPressureSpanPoint = false;
        _numberZeroClickedOneTimeOnSpanSetSession = false;
        _isLoopPowerBtnClicked = false;
        _isContinueClicked = false;
        
        TurnOffAllUI();
        GetObject((int)ui).gameObject.SetActive(true);
        _currentUI = ui;
        OnThisUIInit(ui);

    }

    public void CursorOnTestStrategySetting()
    {
        GetObject((int)UI.ToleranceSelected).gameObject.SetActive(false);
        GetObject((int)UI.TestStrategySelected).gameObject.SetActive(true);
        _hasDownToTestStrategy = true;
    }
    
    public void CursorOnMaxPressureSetting()
    {
        GetObject((int)UI.SelectedYellowBgA).gameObject.SetActive(false);
        GetObject((int)UI.SelectedYellowBgB).gameObject.SetActive(true);
        _hasDownToTestStrategy = true;
    }

    public void KillCalibratePressureSeq()
    {
        _textAnimSeqMap[(int)UI.Calibrating]?.Kill();
        _textAnimSeqMap[(int)UI.Calibrating] = DOTween.Sequence();
    }


    public void CalibratePressure()
    {
        
        Logger.Log("Pressure Calibration Starts....");
        GetObject((int)UI.NotReady_Bg).SetActive(true);
        GetObject((int)UI.Stable_Bg).SetActive(false);
        
        _textAnimSeqMap.TryAdd((int)UI.Calibrating, DOTween.Sequence());
        
        _textAnimSeqMap[(int)UI.Calibrating]?.Kill();
        _textAnimSeqMap[(int)UI.Calibrating] = DOTween.Sequence();
        
        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text = 0.000f.ToString("F3");
                
        // 두 번째 TMP 값: 비례식 적
        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text = 0.000f.ToString("F3");
        
        
        var lastUpdateTime = 0f;

        _textAnimSeqMap[(int)UI.Calibrating]
            .Append(
                DOVirtual.Float(0f, 50.000f, 8, val =>
                {
                    var currentTime = Time.time;
                    if (currentTime - lastUpdateTime >= 0.2f)
                    {
                        // 첫 번째 TMP 값
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text =
                            (val + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        // 두 번째 TMP 값: 비례식 적용
                        float current = 12f + (val) * (8f / 50f);
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text =
                            (current + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        lastUpdateTime = currentTime;
                    }

                    if (val > 49.985f)
                    {
                        GetObject((int)UI.NotReady_Bg).SetActive(false);
                        GetObject((int)UI.Stable_Bg).SetActive(true);
                    }
                    else
                    {
                        GetObject((int)UI.NotReady_Bg).SetActive(true);
                        GetObject((int)UI.Stable_Bg).SetActive(false);
                    }
                }).SetEase(Ease.OutQuint)
            )
            .Append(
                DOVirtual.Float(50f, 50.000f, 2, val =>
                {
                    var currentTime = Time.time;
                    if (currentTime - lastUpdateTime >= 0.2f)
                    {
                        // 첫 번째 TMP 값
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text =
                            (val + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        // 두 번째 TMP 값: 비례식 적용
                        float current = 12f + (val) * (8f / 50f);
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text =
                            (current + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        lastUpdateTime = currentTime;
                    }

                    GetObject((int)UI.NotReady_Bg).SetActive(false);
                    GetObject((int)UI.Stable_Bg).SetActive(true);
                }).SetEase(Ease.OutQuint)
            )
            .Append(
                DOVirtual.Float(50f, 100.000f, 8, val =>
                {
                    var currentTime = Time.time;
                    if (currentTime - lastUpdateTime >= 0.2f)
                    {
                        // 첫 번째 TMP 값
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text =
                            (val + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        // 두 번째 TMP 값: 비례식 적용
                        float current = 12f + (val - 50f) * (8f / 50f);
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text =
                            (current + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        lastUpdateTime = currentTime;
                    }

                    if (val > 99.985f)
                    {
                        GetObject((int)UI.NotReady_Bg).SetActive(false);
                        GetObject((int)UI.Stable_Bg).SetActive(true);
                    }
                    else
                    {
                        GetObject((int)UI.NotReady_Bg).SetActive(true);
                        GetObject((int)UI.Stable_Bg).SetActive(false);
                    }
                }).SetEase(Ease.OutQuint)
            ).Append(
                DOVirtual.Float(100f, 100.000f, 2, val =>
                {
                    var currentTime = Time.time;
                    if (currentTime - lastUpdateTime >= 0.2f)
                    {
                        // 첫 번째 TMP 값
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text =
                            (val + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        // 두 번째 TMP 값: 비례식 적용
                        float current = 12f + (val) * (8f / 50f);
                        GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text =
                            (current + Random.Range(-0.005f, -0.001f)).ToString("F3");

                        lastUpdateTime = currentTime;
                    }

                    GetObject((int)UI.NotReady_Bg).SetActive(false);
                    GetObject((int)UI.Stable_Bg).SetActive(true);
                }).SetEase(Ease.OutQuint)
            ).
            
            OnKill(() =>
            {
                GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text = 0.000f.ToString("F3");
                
                // 두 번째 TMP 값: 비례식 적
                GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text = 0.000f.ToString("F3");
            });

        _textAnimSeqMap[(int)UI.Calibrating].Play();

        
    }

    public void SetTestStrategyModeString(string message)
    {
        GetNonUITMP((int)NonUITMPs.TMP_TestStrategy).text = message;
    }
    private void OnThisUIInit(UI ui)
    {
        
        
        switch (ui)
        {
           case UI.Default:
               _isVented = false;
               break;// highlight,tooltip 중복방지를 위한 번호설정
           case UI.Tasks:
               
               
               break;
           case UI.PressureAndMeasureSetting:
               is100PsiSet = false;
               GetObject((int)UI.SelectedYellowBgA).gameObject.SetActive(true);
               GetObject((int)UI.SelectedYellowBgB).gameObject.SetActive(false);
               
               
               break;
           case UI.CalibrationModeSetting:
               _hasDownToTestStrategy = false;
               GetObject((int)UI.ToleranceSelected).gameObject.SetActive(true);
               GetObject((int)UI.TestStrategySelected).gameObject.SetActive(false);
               GetNonUITMP((int)NonUITMPs.TMP_TestStrategy).text = "3↑↓";
               
               _hasDownToTestStrategy = false;
               break;
           case UI.TestStrategy:
             
           
               
               break;
           case UI.Calibrating:
               
                  
               GetNonUITMP((int)NonUITMPs.TMP_Calibrating_PsiNum).text = 0.000f.ToString("F3");
               GetNonUITMP((int)NonUITMPs.TMP_Calibrating_Current).text = 0.000f.ToString("F3");
               
                 GetObject((int)UI.NotReady_Bg).SetActive(true);
                GetObject((int)UI.Stable_Bg).SetActive(false);
               break;
           case UI.CalibrationFinish:
               
               
            break;
        }
    }
  



}
