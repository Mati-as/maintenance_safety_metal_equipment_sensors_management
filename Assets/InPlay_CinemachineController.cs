using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class InPlay_CinemachineController : MonoBehaviour
{
    private CinemachineFreeLook _freeLookCamera;

    public bool isFreeLookAvailable { get; set; }
    
    public float zoomSpeed = 0.1f; // 줌 속도 조정
    private float minRadius = 2f; // 최소 줌
    private float maxRadius = 3f; // 최대 줌

    private bool isDragging = false;

    private void Awake()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    public void SetLookAtAndFollow(Transform target)
    {
        _freeLookCamera.LookAt = target;
        _freeLookCamera.Follow = target;

    }
    
    // 클릭 시 좌우보기를 위한 회전 제어
    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제 시
        {
            isDragging = false;
        }

        // 마우스 클릭 상태에서만 카메라 회전 가능
        if (isDragging)
        {
            _freeLookCamera.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
        }
        else
        {
            _freeLookCamera.m_XAxis.m_InputAxisValue = 0;
        }
    }

    // 마우스 휠 드래그로 줌 인/아웃 제어
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // Zoom 속도에 따라 Radius 값 조정
            for (int i = 0; i < 3; i++)
            {
                float newRadius = _freeLookCamera.m_Orbits[i].m_Radius - scroll * zoomSpeed;
                _freeLookCamera.m_Orbits[i].m_Radius = Mathf.Clamp(newRadius, minRadius, maxRadius);

                // 디버그 로그 출력
                Logger.Log($"Orbit {i} : Radius Val = {_freeLookCamera.m_Orbits[i].m_Radius}");
            }
        }
    }
}