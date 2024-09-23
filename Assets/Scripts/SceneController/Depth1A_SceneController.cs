using System;using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;



public enum GameObj
{
    LimitSwitch
}
public class Depth1A_SceneController : Base_SceneController
{
    private Dictionary<string, HighlightEffect> _highlight;
    public override void Init()
    {
        InitializeStates();
        SetParameters();
        
        BindObject(typeof(GameObj));
        _highlight = new Dictionary<string, HighlightEffect>();
        _highlight.Add(GetObject((int)GameObj.LimitSwitch).name,
            GetObject((int)GameObj.LimitSwitch).GetComponent<HighlightEffect>());
        
        GetObject((int)GameObj.LimitSwitch).BindEvent(() =>
        {
            SetHighlight(GetObject((int)GameObj.LimitSwitch).name);
            contentController.SetToolTipStatus(true);
            contentController.SetText("리밋 스위치");
        },Define.UIEvent.PointerEnter);
        GetObject((int)GameObj.LimitSwitch).BindEvent(() =>
        {
            SetHighlight(GetObject((int)GameObj.LimitSwitch).name,false);
            contentController.SetToolTipStatus(false);
        },Define.UIEvent.PointerExit);
        
        base.Init();
    }

    private void SetHighlight(string gameObjName,bool isOn =true)
    {
        _highlight[gameObjName].highlighted = isOn;
        Logger.Log($"Hightlight is ON? : {isOn}");
    }
    
    /// <summary>
    /// 1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    /// 2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다. 
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
