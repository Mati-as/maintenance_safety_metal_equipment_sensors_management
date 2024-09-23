// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UI_ToolTip : MonoBehaviour
// {
//
//     public Text Text_tooltip;   
//     public Image Text_image;
//     
//     void Update()
//     {
//         Update_MousePosition();
//     }
//     private void Update_MousePosition()
//     {
//         Vector2 mousePos = Input.mousePosition + (new Vector3(0,20)*2f);
//         this.gameObject.GetComponent<RectTransform>().position = mousePos;       
//     }
//
//  
//     public void Change_text(string text)
//     {
//         Text_tooltip.GetComponent<Text>().text= text;
//         RectTransform rect_1 = (RectTransform)Text_image.transform;
//         rect_1.sizeDelta = new Vector2(text.Length * 30, 50);
//
//         RectTransform rect_2 = (RectTransform)Text_tooltip.transform;
//         rect_2.sizeDelta = new Vector2(text.Length * 30, 50);
//     }
//
// }