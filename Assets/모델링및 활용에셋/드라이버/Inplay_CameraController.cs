using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Inplay_CameraController : MonoBehaviour
{
    private Base_SceneController _sceneController;
    
    private Transform _target; // 
    private readonly float ZOOM_SPEED = 5f; // 줌 속도 조정
    private readonly float ROTATION_SPEED = 5.0f; // 회전 속도 조정
    private readonly float _minZoom = 30f; // 최소 줌 거리
    private readonly float _maxZoom = 70f; // 최대 줌 거리
    private readonly float _dragSpeed = 1.25f; // 드래그 속도
    
    private readonly float _minVerticalAngle = 20f; // 상하 회전 최소 각도
    private readonly float _maxVerticalAngle = 120f; // 상하 회전 최대 각도
    
    private readonly float _minHorizontalAngle = -180f; // 좌우 회전 최소 각도
    private readonly float _maxHorizontalAngle = 180f; // 좌우 회전 최대 각도

    public bool isDragging;
    private float _currentVerticalAngle; // 현재 상하 회전 각도
    private float _currentHorizontalAngle; // 현재 좌우 회전 각도

    private float _verticalPivotCenter; // 상하 회전 중심
    private float _horizontalPivotCenter; // 좌우 회전 중심

    private float _distanceToTarget; // 카메라와 물체 사이 거리
    
    

    private Camera _camera;
    private bool _isControllable =false;
    private Vector3 _currentDefaultRotation;
    private Vector3 _currentDefaultPosition;
    
    // 카메라 btnUI는 isControllable에 의존관계입니다. 
    public bool isControllable
    {
        get
        {
            return _isControllable;
        }
        set
        {
            if (_isControllable != value)
            {
                _isControllable = value;
            }
            _sceneController.contentController.SetCamInitBtnStatus(_isControllable);
        }
    }

    public void SaveStateDefaultTransform()
    {
       
        if (this == null)
        {
            Debug.LogWarning("Attempted to access a destroyed object.");
            return;
        }
        
        var cam = transform;
        _currentDefaultPosition =cam.position;
        _currentDefaultRotation =cam.localRotation.eulerAngles;
        isControllable = true;
        Logger.Log($"saved current state default rotation and position :\n rotation : {_currentDefaultPosition}" +
                   $"position {_currentDefaultPosition}");
    }

    private Sequence _cameraInitSeq;
    private void Awake()
    {
        // 현재 메인 카메라 가져오기
        _camera = Camera.main;
        //_sceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Base_SceneController>();
    }

    protected virtual void Start()
    {
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChanged;
        UI_ContentController.OnStepBtnClicked_CurrentCount += OnStepChanged;

        _sceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Base_SceneController>();
        
        // Get the Volume component from the main camera
        volume = Camera.main.GetComponent<Volume>();

        if (volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = true;
        }
        else
        {
            Debug.LogError("ColorAdjustments not found or Volume missing!");
        }
    }

    protected virtual void OnDestroy()
    {
        Destroy(this);
        UI_ContentController.OnStepBtnClicked_CurrentCount -= OnStepChanged;
    }

    /// <summary>
    /// 카메라 자유시점 이동중, 다음 스텝등으로 버튼이 넘어갈때 종료하는 로직
    /// </summary>
    protected void OnStepChanged(int _, bool __)
    {
        
        isDragging = false;
        _updateSeq?.Kill();
        _cameraInitSeq?.Kill();
        Logger.Log("Cam Init for Step Change");
    }
    public void RefreshRotationAndZoom()
    {
        _updateSeq?.Kill();
        _cameraInitSeq?.Kill();

        InitRotationParams();
        
        _cameraInitSeq = DOTween.Sequence();
        
        _cameraInitSeq.Append(transform.DOMove(_currentDefaultPosition, 0.5f))
            .Join(transform.DORotate(_currentDefaultRotation, 0.5f))
            .OnStart(() =>
            {
                isControllable = false;
            })
            .OnUpdate(() =>
            {
                isDragging = false;
                isControllable = false;
            })
            .OnComplete(() =>
            {
                isControllable = true;
                Logger.Log($"Camera Freelook is {isControllable}");
            }).OnKill(() => { isControllable = true; });
        
        _cameraInitSeq.Play();
    }

    private void InitRotationParams()
    {
        _currentHorizontalAngle = _horizontalPivotCenter;
        _currentVerticalAngle = _verticalPivotCenter;
        _initialMousePosition = Input.mousePosition; // 클릭 시작 위치 저장
        _lastMousePosition = _initialMousePosition; // 초기 위치로 설정
        _distanceToTarget = _defaultDistanceInState; // 거리또한 초기화
    }

    private void Update()
    {
        if (!isControllable) return;
        
        if (Input.GetMouseButtonDown(0)) // 마우스를 처음 클릭했을 때
        {
            if (IsPointerOverUI()) 
            {
                Logger.Log("UI element clicked - Ignoring rotation");
                return; // UI 클릭 시 카메라 제어 로직 무시
            }
            
            isDragging = true;
            _initialMousePosition = Input.mousePosition; // 클릭 시작 위치 저장
            
        }
        
        
        
        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제 시
        {
            isDragging = false;
            return;
        }

        if ((isControllable && isDragging && _target != null))
        {
            UpdateRotation();
            return;
        }
        
        HandleZoom();
       
    }
    
    public GraphicRaycaster uiRaycaster =null; // Canvas에 있는 GraphicRaycaster를 연결해야 함
    public EventSystem eventSystem;

    
    protected virtual bool IsPointerOverUI()
    {
        if (uiRaycaster == null)
        {
         
            uiRaycaster =  Managers.UI.FindPopup<UI_ContentController>().gameObject.GetComponent<GraphicRaycaster>();
            
          
            Logger.Log($"get grahicRayCaster : -->{uiRaycaster.gameObject.name}");
        }
        // UI 위에 있는지 확인
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerData, raycastResults);

        return raycastResults.Count > 0; // UI 요소와 충돌한 경우에만 true 반환
    }

    private float _defaultDistanceInState;
    
    public void SetRotationDefault(Transform target)
    {
        
        _target = target;
    
        var thisObjectPos = transform.position; // 카메라의 현재 위치
        var targetObjPos = _target.position; // 대상 물체의 위치
        _defaultDistanceInState = Vector3.Distance(thisObjectPos, targetObjPos); // 대상과의 거리 계산
      
        _distanceToTarget = _defaultDistanceInState;

        Vector3 direction = (targetObjPos - thisObjectPos).normalized;
        
        _horizontalPivotCenter = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        _verticalPivotCenter = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        
        currentMaxDistanceToTarget = _distanceToTarget * 1.2f;
        currentMinDistanceToTaget = _distanceToTarget * 0.4f;
        // transform.DOLookAt(_target.position, 0.7f).OnComplete(() =>
        // {
        //    
        // });

        _currentHorizontalAngle = _horizontalPivotCenter;
        _currentVerticalAngle = _verticalPivotCenter ;

        Logger.Log($"camera Lookat and distance set: obj: {target.gameObject.name}, distance  = {_distanceToTarget}");
        Logger.Log($"Vertical Angle: {_verticalPivotCenter}, Horizontal Angle: {_horizontalPivotCenter}");
            
        SaveStateDefaultTransform();
        isControllable = true;
    }

  

    private Vector3 _initialMousePosition; // 클릭 시작 위치를 저장할 변수
    private Vector3 _lastMousePosition;    // 드래그 중 이전 마우스 위치 저장



    private Sequence _updateSeq;

    public void UpdateRotation(float updateSpeed = 0.0001f)
    {
        // 현재 카메라 회전 상태를 가져와 동기화
        Vector3 currentRotation = transform.rotation.eulerAngles;
        _currentHorizontalAngle = currentRotation.y;
        _currentVerticalAngle = currentRotation.x;

        _updateSeq = DOTween.Sequence();

        // 클릭 시작 위치와 현재 위치 간의 차이 계산
        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition;

        // 차이만큼 이동한 값을 적용
        float mouseX = mouseDelta.x * _dragSpeed * 0.002f;
        float mouseY = mouseDelta.y * _dragSpeed * 0.002f;

        _currentHorizontalAngle += mouseX;
        _currentVerticalAngle -= mouseY;

        // 수직 회전 각도 제한
        _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, _minVerticalAngle, _maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(_currentVerticalAngle, _currentHorizontalAngle, 0);
        Vector3 offset = new Vector3(0, 0, -_distanceToTarget);
        Vector3 newPosition = rotation * offset + _target.position;

        // DOTween을 사용하여 부드럽게 움직임과 회전 적용
        _updateSeq
            .Join(transform.DOMove(newPosition, updateSpeed).SetEase(Ease.Linear))
            .Join(transform.DORotateQuaternion(rotation, updateSpeed).SetEase(Ease.Linear));
    }

    // 마우스 휠 드래그로 줌 인/아웃 제어

    private float currentMaxDistanceToTarget;
    private float currentMinDistanceToTaget;
    private void HandleZoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _distanceToTarget = Mathf.Clamp(_distanceToTarget - scroll * 0.5f, currentMinDistanceToTaget,currentMaxDistanceToTarget);
            
            // Zoom 속도에 따라 필드 오브 뷰(Field of View) 값 조정
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll * ZOOM_SPEED, _minZoom, _maxZoom);
            UpdateRotation();
        }
    }
    
    public float fadeDuration = 1.0f; // Fade duration as a variable
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private Coroutine fadeCoroutine;



    public void FadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeEffect(-100f));
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeEffect(0f));
    }

    private IEnumerator FadeEffect(float targetValue)
    {
        if (colorAdjustments == null) yield break;

        float startValue = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeDuration);
            colorAdjustments.postExposure.value = newValue;
            yield return null;
        }

        colorAdjustments.postExposure.value = targetValue;
    }

}