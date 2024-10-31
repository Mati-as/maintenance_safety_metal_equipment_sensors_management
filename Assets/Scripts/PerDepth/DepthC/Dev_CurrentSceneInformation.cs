using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dev_CurrentSceneInformation : MonoBehaviour
{
   
    private TextMeshProUGUI _text;
    
    [Conditional("Dev")]
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    [Conditional("Dev")]
    private void Update()
    {
        _text.text = $"(Dev)SceneInfo: : {Managers.ContentInfo.PlayData.CurrentDepthStatus}";
    }
   
}
