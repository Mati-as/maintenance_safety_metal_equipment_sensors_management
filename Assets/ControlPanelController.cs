using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private const float minAngle = -130f;   // 최소 각도
    private const float maxAngle = 30f; // 최대 각도

    public static event Action OnPowerOffAction;
    

    private float resistantTarget = 10;
    public bool isPowerOff;
    
    private void Awake()
    {
        BindObject(typeof(ControlPanel));

     
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.name ==gameObject.name) isDragging = true;

        // 월드 좌표에서 핸들의 위치 가져오기
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
        initialMousePos = worldMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // 마우스 포인터의 현재 월드 좌표 계산
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

            // 핸들의 중심 위치
            Vector3 handlePos = GetObject((int)ControlPanel.PowerHandle).transform.position;

            // 초기 마우스 위치와 현재 마우스 위치에서 핸들 중심으로의 벡터 계산
            Vector3 initialDirection = initialMousePos - handlePos;
            Vector3 currentDirection = currentMousePos - handlePos;

            // 벡터들 사이의 각도 계산 (Z축 기준)
            float angle = -Vector3.SignedAngle(initialDirection, currentDirection, Vector3.forward);

            // 현재 회전 각도에 새로운 각도 추가
            float newAngle = currentAngle + angle * 15;

            // 각도를 0도에서 150도 사이로 클램프 (제한)
            newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

            // 핸들에 회전 적용 (Z축 기준)
            GetObject((int)ControlPanel.PowerHandle).transform.localRotation = Quaternion.Euler(0, newAngle,0f );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
  
        
        // 현재 각도 저장 (Z축 기준)
        currentAngle = GetObject((int)ControlPanel.PowerHandle).transform.localEulerAngles.y;
        if(currentAngle > 180)currentAngle -= 360;

        Logger.Log($"Current Angle (POWER) : {currentAngle}");
        if (currentAngle > -50 && currentAngle < 40)
        {
            Logger.Log("Power OFF-----------------------------");
            isPowerOff = true;
            SetHandleToOff();
            
            OnPowerOffAction?.Invoke();
        }
        else
        {
            isPowerOff = false;
        }
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
