using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dev_CurrentSceneInformation : MonoBehaviour
{
    #if UNITY_EDITOR
    private TextMeshProUGUI _text;
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _text.text = $"(Dev)SceneInfo: : {Managers.ContentInfo.PlayData.CurrentDepthStatus}";
    }
    #endif
}
