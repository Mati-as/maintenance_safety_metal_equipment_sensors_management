using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine;

public class C1_LimitSwitchPivotController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private enum LimitSwitch
    {
        Limitswitch_ArmPivot
    }

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private float currentAngle = 0f;
    private float minAngle = -70f;
    private float maxAngle = 70f;
    private GameObject _onLight;
    private GameObject _offLight;

    private Collider _collider;
    private void Awake()  
    {
        BindObject(typeof(LimitSwitch));
       // InitLimitSwitch();
       _onLight = GameObject.Find("OnLight");
       _offLight = GameObject.Find("OffLight");
       _collider = GetComponent<Collider>();

    }


    public void SetLimitSwitchControllableOrClickable(bool isActive)
    {
        DOVirtual.DelayedCall(1f, () =>
        {
          _collider.enabled = isActive;
          isTargetPosEventInvoked = false;
        });
    }

    public void InitLimitSwitch()
    {
        
        InitLamp();

        minZ = transform.position.z-0.0275f;
        maxZ= transform.position.z + 0.02f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.MaintenancePractice && Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 6)
        {
            RotateHandle(eventData);
        }
        else if((Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.Evaluation && Managers.ContentInfo.PlayData.Count == 6 )||
                (Managers.ContentInfo.PlayData.Depth3 == 2  && Managers.ContentInfo.PlayData.Count == 6)||
                (Managers.ContentInfo.PlayData.Depth3 == 3  && Managers.ContentInfo.PlayData.Count == 7 && 
                 GameObject.FindWithTag("ObjectAnimationController").GetComponent<DepthC1_SceneController>().isLeverScrewUnwound))
        {
            MoveHandleZ(eventData);
        }

      
    }

    public void RotateHandle(PointerEventData eventData)
    {
        Logger.Log("리밋스위치 핸들 각도 조절중 --------------------");

        // 마우스 포인터의 현재 월드 좌표 계산
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 핸들의 중심 위치
        Vector3 handlePos = transform.position;

        // 초기 마우스 위치와 현재 마우스 위치에서 핸들 중심으로의 벡터 계산
        Vector3 initialDirection = initialMousePos - handlePos;
        Vector3 currentDirection = currentMousePos - handlePos;

        // 벡터들 사이의 각도 계산 (Z축 기준)
        float changedAngle = -Vector3.SignedAngle(initialDirection, currentDirection, Vector3.forward);

        // 현재 회전 각도에 새로운 각도 추가
        float newAngle = currentAngle - changedAngle * 300;

        // 각도를 제한
        newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

        // 핸들에 회전 적용 (Y축 기준)
        GetObject((int)LimitSwitch.Limitswitch_ArmPivot).transform.localRotation = Quaternion.Euler(0, newAngle, 0f);

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

    private float minZ;
    private float maxZ;

    public static event Action OnTargetPosArrive; 
    [Range(0, 100000)] public float sensitivity;

    public bool isTargetPosEventInvoked;

    private float _newZPos;
    public void MoveHandleZ(PointerEventData eventData)
    {
        if (isTargetPosEventInvoked)
        {
            Logger.Log("이미 Z축 움직이기 수행과제 완료..  return");
            return;
        }
        Logger.Log("리밋스위치 핸들 Z축 이동 조절중 --------------------");
        
        // 마우스 포인터의 현재 월드 좌표 계산
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 기존 핸들의 위치
        Vector3 handlePos = GetObject((int)LimitSwitch.Limitswitch_ArmPivot).transform.position;

        // 초기 마우스 위치와 현재 마우스 위치의 차이를 계산
        float deltaZ = currentMousePos.y - initialMousePos.y; // y축 차이를 Z축 이동에 매핑

        // 새로운 Z축 위치 계산
        _newZPos = handlePos.z + deltaZ/sensitivity ;

        // Z축 위치를 제한 (필요하면 범위 설정)
        _newZPos = Mathf.Clamp(_newZPos, minZ, maxZ); // minZ와 maxZ를 원하는 값으로 설정

        // 핸들의 위치 업데이트
        GetObject((int)LimitSwitch.Limitswitch_ArmPivot).transform.position = new Vector3(handlePos.x, handlePos.y, _newZPos);

        // 초기 마우스 위치 갱신
        initialMousePos = currentMousePos;

     
    }


    public void CheckAndInvokeTargetPosArrivalEvent()
    {
         if (!isTargetPosEventInvoked && _newZPos - minZ < 0.008f )
        {
            OnTargetPosArrive?.Invoke();
            isTargetPosEventInvoked = true;
            Logger.Log("target arrvie action invoke");
        }
         else
         {
             Logger.Log($"target arrvive invoke condition is not met. value: ({_newZPos - minZ})has to be lower than 0.02");
         }
    }
    public void InitLamp()
    {
        _offLight.gameObject.SetActive(true);
        _onLight.gameObject.SetActive(false);
    }

    public void InitLeverRotation()
    {
        GetObject((int)LimitSwitch.Limitswitch_ArmPivot).transform.localRotation = Quaternion.Euler(0, 0, 0f);   
    }
    
    


   
    private bool _isLimitSwitchOn;
    
    private bool isSwitchOn(float currentAngle)
    {
        if (currentAngle > maxAngle - 30)
        {
            return true;
        }

        if (currentAngle < minAngle + 30)
        {
            return true;
        }
        else
            return false;
            
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CheckAndInvokeTargetPosArrivalEvent();
        isDragging = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // 드래그 종료 처리
        Vector3 mousePos = Input.mousePosition;
        initialMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        isDragging = true;
    }
}
