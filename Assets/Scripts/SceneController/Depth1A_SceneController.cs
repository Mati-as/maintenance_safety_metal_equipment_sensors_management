using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;

public enum GameObj
{
    LimitSwitch,
    ProximitySwitch,
    LooAt_plumbingSystemOrPipework,
    TemperatureSensor,
    LevelSensor,
    FlowMeter,
    PressureSensor
}




public class Depth1A_SceneController : Base_SceneController
{
    private Dictionary<string, HighlightEffect> _highlight;
    private Dictionary<int, Sequence> _seqMap;
    public InPlay_CinemachineController cameraController;
    
    
    public override void Init()
    {
        InitializeStates();
        SetParameters();
        BindObject(typeof(GameObj));
        cameraController = Camera.main.GetComponent<InPlay_CinemachineController>();
        
        _highlight = new Dictionary<string, HighlightEffect>();
        _seqMap = new Dictionary<int, Sequence>();

        // 딕셔너리에 추가 및 이벤트 바인딩
        BindAndAddToDictionary(GameObj.LimitSwitch, "리밋 스위치");
        BindAndAddToDictionary(GameObj.ProximitySwitch, "근접 스위치");
        BindAndAddToDictionary(GameObj.TemperatureSensor, "온도 센서");
        BindAndAddToDictionary(GameObj.LevelSensor, "레벨 센서");
        BindAndAddToDictionary(GameObj.FlowMeter, "유량 센서");
        BindAndAddToDictionary(GameObj.PressureSensor, "압력 센서");
        base.Init();
    }

    private void BindAndAddToDictionary(GameObj gameObj, string tooltipText)
    {
        AddToHighlightDictionary(gameObj);
        BindHighlightAndTooltip(gameObj, tooltipText);
    }

    private void AddToHighlightDictionary(GameObj gameObj)
    {
        var objName = GetObject((int)gameObj).name;
        var highlightEffect = GetObject((int)gameObj).GetComponent<HighlightEffect>();

        if (!_highlight.ContainsKey(objName)) _highlight.Add(objName, highlightEffect);
    }

    private void BindHighlightAndTooltip(GameObj gameObj, string tooltipText)
    {
        // PointerEnter 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(GetObject((int)gameObj).name);
            contentController.SetToolTipStatus();
            contentController.SetText(tooltipText);
        }, Define.UIEvent.PointerEnter);

        // PointerExit 이벤트 바인딩
        GetObject((int)gameObj).BindEvent(() =>
        {
            SetHighlight(GetObject((int)gameObj).name, false);
            contentController.SetToolTipStatus(false);
        }, Define.UIEvent.PointerExit);
    }


    private void SetHighlight(string gameObjName, bool isOn = true)
    {
        _highlight[gameObjName].highlighted = isOn;
        Logger.Log($"Hightlight is ON? : {isOn}");
    }


    public void HighlightBlink(GameObj gameObj)
    {
        var seq = DOTween.Sequence();
        _seqMap.TryAdd((int)gameObj, seq);

        var maxInnerGlow = 0.15f;
        if (_seqMap[(int)gameObj].IsActive()) _seqMap[(int)gameObj].Kill();
        seq.AppendCallback(() => { _highlight[GetObject((int)gameObj).name].highlighted = true; });

        var loopCount = 3;
        for (var i = 0; i < loopCount; i++)
        {
            seq.Append(DOVirtual.Float(0, maxInnerGlow, 1f,
                val => { _highlight[GetObject((int)gameObj).name].innerGlow = val; }));

            seq.Append(DOVirtual.Float(maxInnerGlow, 0, 1f,
                val => { _highlight[GetObject((int)gameObj).name].innerGlow = val; }));
        }

        seq.AppendCallback(() => { _highlight[GetObject((int)gameObj).name].highlighted = false; });

        seq.OnKill(() =>
        {
            _highlight[GetObject((int)gameObj).name].highlighted = false;
            _highlight[GetObject((int)gameObj).name].innerGlow = 0;
        });
        _seqMap[(int)gameObj] = seq;
    }

    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void SetParameters()
    {
        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    private void InitializeStates()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 1, new Depth1A_State_1(this) },
            { 2, new Depth1A_State_2(this) },
            { 3, new Depth1A_State_3(this) },
            { 4, new Depth1A_State_4(this) },
            { 5, new Depth1A_State_5(this) },
            { 6, new Depth1A_State_6(this) },
            { 7, new Depth1A_State_7(this) },
            { 8, new Depth1A_State_8(this) },
            { 9, new Depth1A_State_9(this) },
            { 10, new Depth1A_State_10(this) },
            { 11, new Depth1A_State_11(this) },
            { 12, new Depth1A_State_12(this) },
            { 13, new Depth1A_State_13(this) },
            { 14, new Depth1A_State_14(this) },
            { 15, new Depth1A_State_15(this) },
            { 16, new Depth1A_State_16(this) },
            { 17, new Depth1A_State_17(this) },
            { 18, new Depth1A_State_18(this) },
            { 19, new Depth1A_State_19(this) },
            { 20, new Depth1A_State_20(this) }
        };
    }
}