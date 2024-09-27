using UnityEngine;

public class Inplay_CameraController : MonoBehaviour
{
    private readonly float _zoomSpeed = 0.8f; // 줌 속도 조정
    private readonly float _rotationSpeed = 5.0f; // 회전 속도 조정
    private readonly float _minZoom = 15f; // 최소 줌 거리
    private readonly float _maxZoom = 60f; // 최대 줌 거리
    private readonly float _dragSpeed = 1f; // 드래그 속도

    private readonly float _minVerticalAngle = -30f; // 상하 회전 최소 각도
    private readonly float _maxVerticalAngle = 30f; // 상하 회전 최대 각도
    private readonly float _minHorizontalAngle = -60f; // 좌우 회전 최소 각도
    private readonly float _maxHorizontalAngle = 60f; // 좌우 회전 최대 각도

    private bool _isDragging;
    private Camera _camera;
    private float _currentVerticalAngle; // 현재 상하 회전 각도
    private float _currentHorizontalAngle; // 현재 좌우 회전 각도

    private float _verticalPivotCenter; // 현재 상하 회전 각도
    private float _horizontalPivotCenter; // 현재 좌우 회전 각도

    private void Awake()
    {
        // 현재 메인 카메라 가져오기
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    public void SetCurrentMainAngle()
    {
        // 현재 카메라의 오일러 각도를 가져옴
        var eulerAngles = transform.localEulerAngles;

        _verticalPivotCenter = (eulerAngles.x > 180f ? eulerAngles.x - 360f : eulerAngles.x); // 상하 회전 (반전)
        _horizontalPivotCenter = eulerAngles.y > 180f ? eulerAngles.y - 360f : eulerAngles.y; // 좌우 회전

        _currentVerticalAngle = _verticalPivotCenter;
        _currentHorizontalAngle = _horizontalPivotCenter;
        // 상하 회전 각도를 ±180도 범위로 변환
        if (_horizontalPivotCenter > 180f) _horizontalPivotCenter -= 360f;

        // 디버그 로그로 확인
        Debug.Log($"Vertical Angle: {_verticalPivotCenter}, Horizontal Angle: {_horizontalPivotCenter}");
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시
            _isDragging = true;

        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제 시
            _isDragging = false;

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

            // 제한된 범위 내에서 회전 적용
            transform.localRotation = Quaternion.Euler(_currentVerticalAngle, _currentHorizontalAngle, 0);
        }
    }

    // 마우스 휠 드래그로 줌 인/아웃 제어
    private void HandleZoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
            // Zoom 속도에 따라 필드 오브 뷰(Field of View) 값 조정
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll * _zoomSpeed, _minZoom, _maxZoom);
    }
}