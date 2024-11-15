using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




/// <summary>
/// <개발용> : 테스트시 버튼이동을 자유롭게하거나, 각종 부가정보를 표시하기 위한 객체
/// 빌드파일 포함안되도록 조건부 구성
/// </summary>

public class Dev_CurrentSceneInformation : UI_Popup
{
#if UNITY_EDITOR
   private TextMeshProUGUI _text;
   
   
   public static event Action OnNextClickedOnDev;
   public static event Action OnPrevClickedOnDev;



   private enum DevBtns
   {
      Prev,
      Next,
   }

   public override bool Init()
   {
      if (base.Init() == false)
         return false;
      
      BindButton(typeof(DevBtns));
      
      
      GetButton((int)DevBtns.Prev).gameObject.BindEvent(() =>
      {
         OnPrevClickedOnDev?.Invoke();
      });
      GetButton((int)DevBtns.Next).gameObject.BindEvent(() =>
      {
         OnNextClickedOnDev?.Invoke();
      });
      
      
      _text = GetComponentInChildren<TextMeshProUGUI>();
      return true;
   }


   private void Update()
   {
      _text.text = $"(Dev)SceneInfo: : {Managers.ContentInfo.PlayData.CurrentDepthStatus}";
   }
#else

   private void Start()
   {
      gameObject.SetActive(false);
   }

#endif
 
}

