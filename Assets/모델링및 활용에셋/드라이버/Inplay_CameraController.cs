using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Inplay_CameraController : MonoBehaviour
{
   
    private Transform _target; // 
    private readonly float ZOOM_SPEED = 10f; // 줌 속도 조정
    private readonly float ROTATION_SPEED = 5.0f; // 회전 속도 조정
    private readonly float _minZoom = 50f; // 최소 줌 거리
    private readonly float _maxZoom = 100f; // 최대 줌 거리
    private readonly float _dragSpeed = 1f; // 드래그 속도
    
    private readonly float _minVerticalAngle = 20f; // 상하 회전 최소 각도
    private readonly float _maxVerticalAngle = 95f; // 상하 회전 최대 각도
    
    private readonly float _minHorizontalAngle = -18f; // 좌우 회전 최소 각도
    private readonly float _maxHorizontalAngle = 18f; // 좌우 회전 최대 각도

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
    public bool isControllable
    {
        get
        {
            return _isControllable;
        }
        set
        {
            _isControllable = value;
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
        _currentDefaultRotation =cam.rotation.eulerAngles;
        
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
        _isControllable = false;
        isDragging = false;
        _updateSeq?.Kill();
        _cameraInitSeq?.Kill();
        Logger.Log("Cam Init for Step Change");
    }
    public void RefreshRotationAndZoom()
    {
        _updateSeq?.Kill();
        _cameraInitSeq?.Kill();
        
        _currentHorizontalAngle = _horizontalPivotCenter;
        _currentVerticalAngle = _verticalPivotCenter + Yoffset;
        _initialMousePosition = Input.mousePosition; // 클릭 시작 위치 저장
        _lastMousePosition = _initialMousePosition; // 초기 위치로 설정
        _distanceToTarget = _defaultDistanceInState; // 거리또한 초기화
        
        _cameraInitSeq = DOTween.Sequence();
        
        _cameraInitSeq.Append(transform.DOMove(_currentDefaultPosition, 0.5f))
            .Join(transform.DORotate(_currentDefaultRotation, 0.5f))
            .OnUpdate(() =>
            {
                isDragging = false;
                isControllable = false;
            })
            .OnComplete(() =>
            {
                // 애니메이션 완료 후 현재 각도 초기화
                _currentHorizontalAngle = _horizontalPivotCenter;
                _currentVerticalAngle = _verticalPivotCenter + Yoffset;
                _initialMousePosition = Input.mousePosition; // 클릭 시작 위치 저장
                _lastMousePosition = _initialMousePosition; // 초기 위치로 설정
                Logger.Log("Camera reset to default position and rotation.");
                isControllable = true;

                
                
                Logger.Log($"Camera Freelook is {isControllable}");
            }).OnKill(() => { isControllable = true; });
        
        _cameraInitSeq.Play();
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
            //isControllable = true;
            _initialMousePosition = Input.mousePosition; // 클릭 시작 위치 저장
            _lastMousePosition = _initialMousePosition; // 초기 위치로 설정
            Logger.Log($"button Down ----- Drag: {isDragging} Controllable: {isControllable}");
        }
        
        
        
        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제 시
        {
            isDragging = false;
            //  isControllable = false;
            _updateSeq?.Kill();
            Logger.Log("Button Up - Dragging Stopped");
            return;
        }

        if ((isControllable && isDragging && _target != null))
        {
            HandleRotation();
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
    public void SetCurrentMainAngleAndPosOnStateEnter(Transform target)
    {
        
        _target = target;
    
        var thisObjectPos = transform.position; // 카메라의 현재 위치
        var targetObjPos = _target.position; // 대상 물체의 위치
        
        transform.DOLookAt(_target.position, 1f).OnComplete(() =>
        {
            _currentHorizontalAngle = _horizontalPivotCenter;
            _currentVerticalAngle = _verticalPivotCenter + Yoffset;
            
            _defaultDistanceInState = Vector3.Distance(thisObjectPos, targetObjPos); // 대상과의 거리 계산
            _distanceToTarget = _defaultDistanceInState;

            // 방향 벡터를 계산 (카메라에서 대상까지의 방향)
            Vector3 direction = (targetObjPos - thisObjectPos).normalized;
        
            // 수평 각도 (y축 회전)
            _horizontalPivotCenter = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
            // 수직 각도 (x축 회전)
            _verticalPivotCenter = Mathf.Asin(direction.y) * Mathf.Rad2Deg;

            // 현재 각도를 설정
       
       
            Logger.Log($"camera Lookat and distance set: obj: {target.gameObject.name}, distance  = {_distanceToTarget}");
            Logger.Log($"Vertical Angle: {_verticalPivotCenter}, Horizontal Angle: {_horizontalPivotCenter}");
            isControllable = true;
        
            currentMaxDistanceToTarget = _distanceToTarget * 1.2f;
            currentMinDistanceToTaget = _distanceToTarget * 0.4f;

        });

        //UpdateRotation(1.5f);
    }

    [Range(-10,50f)]
    public float Yoffset;

    private Vector3 _initialMousePosition; // 클릭 시작 위치를 저장할 변수
    private Vector3 _lastMousePosition;    // 드래그 중 이전 마우스 위치 저장

    private void HandleRotation()
    {

     
            Logger.Log("handle Rotation");
            UpdateRotation();
        
    }



    private Sequence _updateSeq;
    private void UpdateRotation(float updateSpeed = 0.15f)
    {
        _updateSeq = DOTween.Sequence();
        // 클릭 시작 위치와 현재 위치 간의 차이 계산
        Vector3 mouseDelta = Input.mousePosition - _initialMousePosition;

        // 차이만큼 이동한 값을 적용
        float mouseX = mouseDelta.x * _dragSpeed * 0.0012f;
        float mouseY = mouseDelta.y * _dragSpeed * 0.0012f;

        // 로그로 출력해 확인
//        Logger.Log($"Mouse Delta X (Horizontal Move): {mouseX} \n Mouse Delta Y (Vertical Move): {mouseY}");

        // 좌우 회전 각도 (y축)
        _currentHorizontalAngle += mouseX;
        _currentHorizontalAngle = Mathf.Clamp(_currentHorizontalAngle, _horizontalPivotCenter + _minHorizontalAngle,
            _horizontalPivotCenter + _maxHorizontalAngle);

        // 상하 회전 각도 (x축)
        _currentVerticalAngle -= mouseY;
        _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, _verticalPivotCenter + _minVerticalAngle,
            _verticalPivotCenter + _maxVerticalAngle);

        // 새로운 위치 계산 (타겟 주위를 원형으로 회전)
        Quaternion rotation = Quaternion.Euler(_currentVerticalAngle, _currentHorizontalAngle, 0);
        Vector3 newPosition = rotation * new Vector3(0, _distanceToTarget / 3, -_distanceToTarget) + _target.position;

        // DOTween Sequence를 사용해 DOMove와 DOLookAt을 동시에 실행
        
        Quaternion originalRotation = transform.rotation; // 이동 전 회전을 저장
        
        _updateSeq
            .Join(transform.DOMove(newPosition, updateSpeed) .OnUpdate(() => transform.rotation = originalRotation))
            .Join(transform.DOLookAt(_target.position, updateSpeed)); // 동일한 시간으로 설정해 부드럽게 맞춤
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
}