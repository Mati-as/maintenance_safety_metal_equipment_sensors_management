using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorImageController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button _button;
    private Toggle _toggle;
    private HighlightEffect _hlEffect;
    

    public void Start()
    {
        TryGetComponent(out _button);
        TryGetComponent(out _toggle);
        TryGetComponent(out _hlEffect);
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

        if (_hlEffect != null)
        {
            if (_hlEffect.ignore)
            {
                Cursor.SetCursor(Managers.cursorImageManager.Get_arrow_image(), Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(Managers.cursorImageManager.Get_hand_image(), Vector2.zero, CursorMode.ForceSoftware);
            }

            return;
            
        }
        
        if (_toggle != null)
        {
            if (_toggle.IsInteractable())
            {
                Cursor.SetCursor(Managers.cursorImageManager.Get_hand_image(), Vector2.zero, CursorMode.ForceSoftware);
            }
            return;
        }
        

        // If neither a Button nor a Toggle is present, set the hand image cursor by default
        Cursor.SetCursor(Managers.cursorImageManager.Get_arrow_image(), Vector2.zero, CursorMode.ForceSoftware);
        
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(Managers.cursorImageManager.Get_arrow_image(), Vector2.zero, CursorMode.ForceSoftware);
    }

}
