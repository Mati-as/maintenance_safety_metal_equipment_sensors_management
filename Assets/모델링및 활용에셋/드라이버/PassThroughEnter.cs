
using UnityEngine;
using UnityEngine.EventSystems;


public class PassThroughEnter : MonoBehaviour,IPointerClickHandler
{
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     // 자신의 PointerEnter 이벤트를 처리하고
    //     Debug.Log("ActiveArea entered");
    //
    //     // PointerEnter 이벤트를 부모 객체로 전달
    //     if (transform.parent != null)
    //     {
    //         foreach (Transform child in transform.parent)
    //         {
    //             if (child != transform)
    //             {
    //                 ExecuteEvents.Execute<IPointerEnterHandler>(child.gameObject, eventData, (handler, data) => handler.OnPointerEnter((PointerEventData)data));
    //             }
    //         }
    //     }
    //
    // }
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     // 자신의 PointerEnter 이벤트를 처리하고
    //     Debug.Log("ActiveArea entered");
    //
    //     // PointerEnter 이벤트를 부모 객체로 전달
    //     if (transform.parent != null)
    //     {
    //         foreach (Transform child in transform.parent)
    //         {
    //             if (child != transform)
    //             {
    //                 ExecuteEvents.Execute<IPointerExitHandler>(child.gameObject, eventData, (handler, data) => handler.OnPointerEnter((PointerEventData)data));
    //             }
    //         }
    //     }
    //
    // }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 자신의 PointerEnter 이벤트를 처리하고
        Debug.Log("ActiveArea entered");

        // PointerEnter 이벤트를 부모 객체로 전달
        if (transform.parent != null)
        {
            foreach (Transform child in transform.parent)
            {
                if (child != transform)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(child.gameObject, eventData, (handler, data) => handler.OnPointerClick((PointerEventData)data));
                }
            }
        }
   
    }
}
