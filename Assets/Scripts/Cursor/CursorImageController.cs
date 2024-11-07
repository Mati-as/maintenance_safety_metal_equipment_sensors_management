using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorImageController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button _button;
    private Toggle _toggle;


    public void Start()
    {
        TryGetComponent(out _button);
        TryGetComponent(out _toggle);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Cursor.SetCursor(Managers.cursorImageManager.Get_arrow_image(), Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If a Button component exists and is not interactable, prevent cursor change
        // if (_button != null)
        // {
        //     if (_button.interactable)
        //     {
        //         Cursor.SetCursor(Managers.cursorImageManager.Get_hand_image(), Vector2.zero, CursorMode.ForceSoftware);
        //     }
        //     return;
        // }

        // If a Toggle component exists and is not interactable, prevent cursor change
        
        if (_toggle != null)
        {
            if (_toggle.interactable)
            {
                Cursor.SetCursor(Managers.cursorImageManager.Get_hand_image(), Vector2.zero, CursorMode.ForceSoftware);
            }
            return;
        }

        // If neither a Button nor a Toggle is present, set the hand image cursor by default
        Cursor.SetCursor(Managers.cursorImageManager.Get_hand_image(), Vector2.zero, CursorMode.ForceSoftware);
        
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(Managers.cursorImageManager.Get_arrow_image(), Vector2.zero, CursorMode.ForceSoftware);
    }

}
