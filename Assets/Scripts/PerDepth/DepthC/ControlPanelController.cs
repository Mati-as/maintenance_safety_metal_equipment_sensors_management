using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Sequence = DG.Tweening.Sequence;


public class ControlPanelController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
     private enum ControlPanel
    {
        PowerHandle
    }

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private float currentAngle = 0f;
    private bool _isAlreadyClicked = false;


    private const float ON_ANGLE = -90f;  
    private const float OFF_ANGLE = 0f;   

    public static event Action<bool> PowerOnOffActionWithBool;
    

    private float resistantTarget = 10;
    private bool _isPowerOn=true;
    private const bool INITIAL_STATUS =  true;

    private bool _isClickable =true;
    private float _clikableDelay=3.5f;

    public bool isPowerOn
    {
        get
        {
            return _isPowerOn;
        }
        
        set
        {
            _isPowerOn = value;
        }
    }


    private void Awake()
    {
        BindObject(typeof(ControlPanel));

        GetObject((int)ControlPanel.PowerHandle).transform
            .localRotation = Quaternion.Euler(0, ON_ANGLE, 0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        if (!_isClickable) return;
        _isClickable = false;
        DOVirtual.DelayedCall(_clikableDelay, () => _isClickable = true);


        if (isPowerOn && (Managers.ContentInfo.PlayData.Count == 13 || Managers.ContentInfo.PlayData.Count == 3))
            TurnOffPowerHandle();
        else
            TurnOnPowerHandle();
        
        

    }


    private Sequence _powerOnOffSeq;

    public void TurnOnPowerHandle()
    {
        if (_isAlreadyClicked) return;
        _isAlreadyClicked = true;
        
        _powerOnOffSeq?.Kill();
        _powerOnOffSeq = DOTween.Sequence();
        
        //var cacheCurrentAngle = currentAngle;
        _powerOnOffSeq.AppendCallback(() =>
        {
          
          
            isPowerOn = true;
            PowerOnOffActionWithBool?.Invoke(isPowerOn);
            Logger.Log("컨트롤 페널 스위치 켜기");
            
            Managers.Sound.Play(SoundManager.Sound.Effect,"Object/PowerOn");
            
            DOVirtual.Float(OFF_ANGLE, ON_ANGLE, 1.35f,
                val =>
                {
                    GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, val, 0f);
                });
        });

        _powerOnOffSeq.Play();
    }

    public void TurnOffPowerHandle()
    {
        if (_isAlreadyClicked) return;
        _isAlreadyClicked = true;
            
        _powerOnOffSeq?.Kill();
        _powerOnOffSeq = DOTween.Sequence();

        // var cacheCurrentAngle = currentAngle;
        _powerOnOffSeq.AppendCallback(() =>
        {
            isPowerOn = false;
            PowerOnOffActionWithBool?.Invoke(isPowerOn);
            
            Logger.Log("컨트롤 페널 스위치 끄기");
            Managers.Sound.Play(SoundManager.Sound.Effect,"Object/PowerOff");
            DOVirtual.Float(ON_ANGLE, OFF_ANGLE, 1.35f,
                val =>
                {
                    GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, val, 0f);
                });
        });
        
        _powerOnOffSeq.Play();
    }

    
    /// <summary>
    /// PowerOn을위한 초기화
    /// </summary>
    public void SetPowerHandleOff()
    {
        _isAlreadyClicked = false;
        isPowerOn = false;
        GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, OFF_ANGLE, 0f);
    }
    
    
    /// <summary>
    /// PowerOff을위한 초기화
    /// </summary>
    public void SetPowerHandleOn()
    {
        _isAlreadyClicked = false;
        isPowerOn = true;
        GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, ON_ANGLE, 0f);
    }

    



}
