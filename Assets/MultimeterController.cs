using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = System.Random;

public class MultimeterController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
     public TextMeshPro TMPDisplay; 
       private enum Multimeter
    {
        MultimeterHandle,
        Display
    }

    private bool isDragging = false;
    private Vector3 initialMousePos;
    private float currentAngle = 0f;
    private const float minAngle = 0f;   // 최소 각도
    private const float maxAngle = 150f; // 최대 각도

    public static event Action OnResistanceMeasureReadyAction;
    
    
    private void Awake()
    {
        BindObject(typeof(Multimeter));

        TMPDisplay = GetObject((int)Multimeter.Display).GetComponent<TextMeshPro>();
        TMPDisplay.text = "O.L";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Logger.Log("Multimeter Clicked");
        if(eventData.pointerCurrentRaycast.gameObject.name =="MultimeterHandle") isDragging = true;

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
            Vector3 handlePos = GetObject((int)Multimeter.MultimeterHandle).transform.position;

            // 초기 마우스 위치와 현재 마우스 위치에서 핸들 중심으로의 벡터 계산
            Vector3 initialDirection = initialMousePos - handlePos;
            Vector3 currentDirection = currentMousePos - handlePos;

            // 벡터들 사이의 각도 계산 (Z축 기준)
            float angle = Vector3.SignedAngle(initialDirection, currentDirection, Vector3.forward);

            // 현재 회전 각도에 새로운 각도 추가
            float newAngle = currentAngle + angle;

            // 각도를 0도에서 150도 사이로 클램프 (제한)
            newAngle = Mathf.Clamp(newAngle, minAngle, maxAngle);

            // 핸들에 회전 적용 (Z축 기준)
            GetObject((int)Multimeter.MultimeterHandle).transform.localRotation = Quaternion.Euler(270f, newAngle,0f );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // 현재 각도 저장 (Z축 기준)
        currentAngle = GetObject((int)Multimeter.MultimeterHandle).transform.localEulerAngles.y;

        if (currentAngle > 60 && currentAngle < 130)
        {
            Logger.Log("Resistance Sensor Mode On ------------");
            isResistanceMode = true;
            SetHandleToResistanceMode();
            
            OnResistanceMeasureReadyAction?.Invoke();
        }
        else
        {
            isResistanceMode = false;
        }
    }


    public bool isResistanceMode;
    public void SetHandleToResistanceMode()
    {
        var cacheCurrentAngle = currentAngle;
        DOVirtual.Float(cacheCurrentAngle, 100, 0.25f, val =>
        {
            GetObject((int)Multimeter.MultimeterHandle).transform.localRotation = Quaternion.Euler(270f, val,0f );
        });
    }

    private float resistantTarget = 108;
    private Sequence _resistanceCheckSeq;
    public void OnAllProbeSet()
    {
      
        
       
            _resistanceCheckSeq?.Kill();
            _resistanceCheckSeq = DOTween.Sequence();
        
        
        Logger.Log("프로브 접촉 완료, 저항값 변경중 -----------------------------------------------------");
        float lastUpdateTime = 0f;

        _resistanceCheckSeq.AppendInterval(1.5f);
        _resistanceCheckSeq.AppendCallback(()=>DOVirtual.Float(0, resistantTarget, 0.9f, val =>
        {
            float currentTime = Time.time;

            if (currentTime - lastUpdateTime >= 0.5f)
            {
               
                TMPDisplay.text = (val + UnityEngine.Random.Range(0, 2.5f)).ToString("F1");
                lastUpdateTime = currentTime;
            }
        }).SetEase(Ease.InOutBounce));

        _resistanceCheckSeq.AppendCallback(() =>
        {
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ =>
            {
                float currentTime = Time.time;

                if (currentTime - lastUpdateTime >= 0.78f)
                {
                    TMPDisplay.text = (resistantTarget + UnityEngine.Random.Range(0, 2.5f)).ToString("F1");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.Play();
    }
    
    public void OnGroundNothing()
    {
        
        _resistanceCheckSeq?.Kill();
        _resistanceCheckSeq = DOTween.Sequence();
    

        
        float lastUpdateTime = 0f;
        
        _resistanceCheckSeq.AppendInterval(0.5f);
        _resistanceCheckSeq.AppendCallback(() =>
        {
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ =>
            {
                float currentTime = Time.time;
                
                if (currentTime - lastUpdateTime >= 0.78f)
                {
                    TMPDisplay.text =(0 + UnityEngine.Random.Range(0, 0.005f)).ToString("F3");
                    lastUpdateTime = currentTime;
                }
               
            }).SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.SetLoops(-1);
        _resistanceCheckSeq.Play();
       
    }

    public void OnAllProbeSetToGroundingTerminal()
    {
       
        _resistanceCheckSeq?.Kill();
        _resistanceCheckSeq = DOTween.Sequence();

        Logger.Log("OL 표시 완료 -------------------------grounding mission");

        _resistanceCheckSeq.AppendInterval(1.5f);
        _resistanceCheckSeq.AppendCallback(() =>
        {
         
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ =>
            {
                TMPDisplay.text = "O.L";
            }).SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.SetLoops(3);
        _resistanceCheckSeq.Play();
       
    }
}