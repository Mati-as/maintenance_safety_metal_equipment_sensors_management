using DG.Tweening;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

public class Inplay_CameraController : MonoBehaviour
{
    private Base_SceneController _sceneController;
    
    private Transform _target; // 
    private readonly float ZOOM_SPEED = 10f; // 줌 속도 조정
    private readonly float ROTATION_SPEED = 5.0f; // 회전 속도 조정
    private readonly float _minZoom = 50f; // 최소 줌 거리
    private readonly float _maxZoom = 80f; // 최대 줌 거리
    private readonly float _dragSpeed = 1f; // 드래그 속도
    
    private readonly float _minVerticalAngle = -45f; // 상하 회전 최소 각도
    private readonly float _maxVerticalAngle = 45f; // 상하 회전 최대 각도
    private readonly float _minHorizontalAngle = -15f; // 좌우 회전 최소 각도
    private readonly float _maxHorizontalAngle = 15f; // 좌우 회전 최대 각도

    private bool _isDragging;
    private float _currentVerticalAngle; // 현재 상하 회전 각도
    private float _currentHorizontalAngle; // 현재 좌우 회전 각도

    private float _verticalPivotCenter; // 상하 회전 중심
    private float _horizontalPivotCenter; // 좌우 회전 중심

    private float _distanceToTarget; // 카메라와 물체 사이 거리


    private Camera _camera;
    private bool _isControllable =false;
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


    private void Awake()
    {
        // 현재 메인 카메라 가져오기
        _camera = Camera.main;
        _sceneController = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Base_SceneController>();
    }

    private void Update()
    {
        if (!isControllable || _target == null) return;
        HandleRotation();
        HandleZoom();
    }


    public void SetCurrentMainAngleAndPos(Transform target)
    {
        if (target == null ||!isControllable) return;

        _target = target;
    
        var thisObjectPos = transform.position; // 카메라의 현재 위치
        var targetObjPos = _target.position; // 대상 물체의 위치
    
        _distanceToTarget = Vector3.Distance(thisObjectPos, targetObjPos); // 대상과의 거리 계산

        // 방향 벡터를 계산 (카메라에서 대상까지의 방향)
        Vector3 direction = (targetObjPos - thisObjectPos).normalized;
        
        // 수평 각도 (y축 회전)
        _horizontalPivotCenter = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        // 수직 각도 (x축 회전)
        _verticalPivotCenter = Mathf.Asin(direction.y) * Mathf.Rad2Deg;

        // 현재 각도를 설정
        _currentHorizontalAngle = _horizontalPivotCenter;
        _currentVerticalAngle = _verticalPivotCenter;

        Debug.Log($"Vertical Angle: {_verticalPivotCenter}, Horizontal Angle: {_horizontalPivotCenter}");
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시
            _isDragging = true;

        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제 시
            _isDragging = false;
        
        
        if (_target == null) return;
        

        // 마우스 클릭 상태에서만 카메라 회전 가능
        if (_isDragging)
        {
            var mouseX = Input.GetAxis("Mouse X") * _dragSpeed;
            var mouseY = Input.GetAxis("Mouse Y") * _dragSpeed;

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
            Vector3 newPosition = rotation * new Vector3(0, _distanceToTarget/3, -_distanceToTarget) + _target.position;

            Logger.Log($"distance to Target from Camera{_distanceToTarget}");
            // 카메라 위치와 LookAt 적용
            
            transform.DOMove(newPosition,1f);
            transform.DOLookAt(_target.position,0.7f);
        }
    }

    // 마우스 휠 드래그로 줌 인/아웃 제어
    private void HandleZoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
                // Zoom 속도에 따라 필드 오브 뷰(Field of View) 값 조정
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll * ZOOM_SPEED, _minZoom, _maxZoom);
        }
    }

}