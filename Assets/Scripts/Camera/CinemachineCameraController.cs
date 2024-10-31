using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    //
    // public CinemachineVirtualCamera virtualCamera;
    // public float zoomSpeed = 2f;
    // public float minZoom = 5f;
    // public float maxZoom = 15f;
    // public float panSpeed = 0.1f;
    //
    // private Vector3 dragOrigin;
    // private float maxPitchAngle = 30f;  // maximum allowed pitch angle (up and down rotation)
    // private float maxYawAngle = 30f;    // maximum allowed yaw angle (left and right rotation)
    // private float currentPitch;
    // private float currentYaw;
    //
    // private void Awake()
    // {
    //     virtualCamera = GetComponent<CinemachineVirtualCamera>();
    //
    //     // Initialize current pitch and yaw from the camera's starting rotation
    //     Vector3 initialRotation = transform.eulerAngles;
    //     currentPitch = initialRotation.x;
    //     currentYaw = initialRotation.y;
    // }
    //
    // void Update()
    // {
    //     // Zoom in and out using the scroll wheel
    //     float scrollValue = Mouse.current.scroll.y.ReadValue();
    //     if (scrollValue != 0)
    //     {
    //         CinemachineComponentBase component = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    //         if (component != null)
    //         {
    //             var framingTransposer = (CinemachineFramingTransposer)component;
    //             framingTransposer.m_CameraDistance -= scrollValue * zoomSpeed * Time.deltaTime;
    //             framingTransposer.m_CameraDistance = Mathf.Clamp(framingTransposer.m_CameraDistance, minZoom, maxZoom);
    //         }
    //         else
    //         {
    //             Debug.LogError("Framing transposer is null");
    //         }
    //     }
    //
    //     // Start panning the camera
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         dragOrigin = Input.mousePosition;
    //         return;
    //     }
    //
    //     if (!Input.GetMouseButton(0)) return;
    //
    //     Vector3 difference = Input.mousePosition - dragOrigin;
    //     dragOrigin = Input.mousePosition;
    //
    //     // Apply movement to pitch and yaw
    //     float moveX = difference.x * panSpeed * Time.deltaTime;
    //     float moveY = difference.y * panSpeed * Time.deltaTime;
    //
    //     // Update current pitch and yaw based on mouse movement
    //     currentPitch -= moveY;
    //     currentYaw += moveX;
    //
    //     // Clamp pitch and yaw to prevent exceeding the limits
    //     currentPitch = Mathf.Clamp(currentPitch, -maxPitchAngle, maxPitchAngle);
    //     currentYaw = Mathf.Clamp(currentYaw, -maxYawAngle, maxYawAngle);
    //
    //     // Apply the clamped rotation back to the camera
    //     transform.eulerAngles = new Vector3(currentPitch, currentYaw, 0);
    // }
}