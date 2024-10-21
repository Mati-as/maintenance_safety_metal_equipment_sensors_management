using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


public class ControlPanelController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
     private enum ControlPanel
    {
        PowerHandle
    }

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private float currentAngle = 0f;


    private const float ON_ANGLE = -90f;  
    private const float OFF_ANGLE = 0f;   

    public static event Action<bool> PowerOnOffActionWithBool;
    

    private float resistantTarget = 10;
    private bool _isPowerOn;
    private const bool INITIAL_STATUS =  true;

    private bool _isClickable =true;
    private float _clikableDelay=1.5f;

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

        //TurnOnPowerHandle();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        // if (isDragging)
        // {
        //     // 마우스 포인터의 현재 월드 좌표 계산
        //     Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
        //
        //     // 핸들의 중심 위치
        //     Vector3 handlePos = GetObject((int)ControlPanel.PowerHandle).transform.position;
        //
        //     // 초기 마우스 위치와 현재 마우스 위치에서 핸들 중심으로의 벡터 계산
        //     Vector3 initialDirection = initialMousePos - handlePos;
        //     Vector3 currentDirection = currentMousePos - handlePos;
        //
        //     // 벡터들 사이의 각도 계산 (Z축 기준)
        //     float angle = -Vector3.SignedAngle(initialDirection, currentDirection, Vector3.forward);
        //
        //     // 현재 회전 각도에 새로운 각도 추가
        //     float newAngle = currentAngle + angle * 15;
        //
        //     // 각도를 0도에서 150도 사이로 클램프 (제한)
        //     newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);
        //
        //     // 핸들에 회전 적용 (Z축 기준)
        //     GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, newAngle,0f );
        // }
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
        _powerOnOffSeq?.Kill();
        _powerOnOffSeq = DOTween.Sequence();

        //var cacheCurrentAngle = currentAngle;
        _powerOnOffSeq.AppendCallback(() =>
        {
            isPowerOn = true;
            PowerOnOffActionWithBool?.Invoke(isPowerOn);
            Logger.Log("컨트롤 페널 스위치 켜기");
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
        _powerOnOffSeq?.Kill();
        _powerOnOffSeq = DOTween.Sequence();

        // var cacheCurrentAngle = currentAngle;
        _powerOnOffSeq.AppendCallback(() =>
        {
            isPowerOn = false;
            PowerOnOffActionWithBool?.Invoke(isPowerOn);
            
            Logger.Log("컨트롤 페널 스위치 끄기");

            DOVirtual.Float(ON_ANGLE, OFF_ANGLE, 1.35f,
                val =>
                {
                    GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, val, 0f);
                });
        });
        
        _powerOnOffSeq.Play();
    }

    
    public void SetPowerHandleOff()
    {
        isPowerOn = false;
        GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, OFF_ANGLE, 0f);
    }
    public void SetPowerHandleOn()
    {
        isPowerOn = true;
        GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, ON_ANGLE, 0f);
    }

    
    public void SetHandleToOff()
    {
        var cacheCurrentAngle = currentAngle;
        DOVirtual.Float(cacheCurrentAngle, resistantTarget, 0.5f, val =>
        {
            GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, val,0f );
        });
    }


}
