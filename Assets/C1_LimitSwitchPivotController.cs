using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine;

public class C1_LimitSwitchPivotController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private enum LimitSwitch
    {
        Pivot,
    }

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private float currentAngle = 0f;
    private float minAngle = -80f;
    private float maxAngle = 80f;
    private GameObject _onLight;
    private GameObject _offLight;

    private void Awake()  
    {
        // 초기 회전값 설정
        transform.localRotation = Quaternion.Euler(0, 0, 0f);

        // PointerDown 이벤트 등록
        gameObject.BindEvent(() =>
        {
            isDragging = true;

            Logger.Log("리밋스위치 핸듣 움직이기 클릭!");
            // 초기 마우스 위치 저장
            Vector3 mousePos = Input.mousePosition;
            initialMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

        }, Define.UIEvent.PointerDown);

        // PointerUp 이벤트 등록
        gameObject.BindEvent(() =>
        {
            isDragging = false;
        }, Define.UIEvent.PointerUp);

        _onLight = GameObject.Find("OnLight");
        _offLight = GameObject.Find("OffLight");
        
        InitLamp();
      
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Logger.Log("리밋스위치 핸들 조절중 --------------------");

        // 마우스 포인터의 현재 월드 좌표 계산
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 핸들의 중심 위치
        Vector3 handlePos = transform.position;

        // 초기 마우스 위치와 현재 마우스 위치에서 핸들 중심으로의 벡터 계산
        Vector3 initialDirection = initialMousePos - handlePos;
        Vector3 currentDirection = currentMousePos - handlePos;

        // 벡터들 사이의 각도 계산 (Z축 기준)
        float angle = -Vector3.SignedAngle(initialDirection, currentDirection, Vector3.forward);

        // 현재 회전 각도에 새로운 각도 추가
        float newAngle = currentAngle - angle * 28;

        // 각도를 제한
        newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

        // 핸들에 회전 적용 (Y축 기준)
        transform.localRotation = Quaternion.Euler(0, newAngle, 0f);

        // 현재 각도 업데이트
        currentAngle = newAngle;

        // 초기 마우스 위치 갱신 (누적 회전 방지)
        initialMousePos = currentMousePos;

        if (isSwitchOn(currentAngle))
        {
            _onLight.gameObject.SetActive(true);
            _offLight.gameObject.SetActive(false);
        }
        else
        {
            _offLight.gameObject.SetActive(true);
            _onLight.gameObject.SetActive(false);
        }

  

    }

    public void InitLamp()
    {
        _offLight.gameObject.SetActive(true);
        _onLight.gameObject.SetActive(false);
    }

    public void InitLeverRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0f);   
    }
    
    


   
    private bool _isLimitSwitchOn;
    
    private bool isSwitchOn(float currentAngle)
    {
        if (currentAngle > maxAngle - 25)
        {
            return true;
        }

        if (currentAngle < minAngle + 25)
        {
            return true;
        }
        else
            return false;
            
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 드래그 종료 처리
        isDragging = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // 드래그 종료 처리
        isDragging = false;
    }
}
