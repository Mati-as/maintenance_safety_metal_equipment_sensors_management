using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial_CameriaController : Inplay_CameraController
{
    protected override void Start()
    {
        UI_Tutorial.OnStepBtnClicked_CurrentCount -= OnStepChanged;
        UI_Tutorial.OnStepBtnClicked_CurrentCount += OnStepChanged;
    }

    protected override void OnDestroy()
    {
        UI_Tutorial.OnStepBtnClicked_CurrentCount -= OnStepChanged;
    }

    protected override bool IsPointerOverUI()
    {
        if (uiRaycaster == null)
        {
            if (Managers.ContentInfo.PlayData.Depth1 == 5)
            {
                uiRaycaster =  Managers.UI.FindPopup<UI_Tutorial>().gameObject.GetComponent<GraphicRaycaster>();
                Logger.Log($"get grahicRayCaster : -->{uiRaycaster.gameObject.name}");
            }
        
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
}
