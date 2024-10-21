using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;

public enum GameObj
{
    LimitSwitch,
    ProximitySwitch,
    TemperatureSensor,
    TemperatureSensor_Whole,//분해 로직이랑 구분
    LevelSensor,
    FlowMeter,
    PressureSensor,
    //Parts
    TS_CompensatingWire,
    TS_Stabilizer,
    TS_SensingElement,
    TS_Cover,
}




public class Depth1A_SceneController : Base_SceneController
{
    
    
    public override void Init()
    {   
        base.Init();
        
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        InitializeStates();
        SetDepthNum();
        
        BindObject(typeof(GameObj));
       
        
        // 딕셔너리에 추가 및 이벤트 바인딩
        BindAndAddToDictionaryAndInit((int)GameObj.LimitSwitch, "리밋 스위치");
        BindAndAddToDictionaryAndInit((int)GameObj.ProximitySwitch, "근접 스위치");
        BindAndAddToDictionaryAndInit((int)GameObj.TemperatureSensor, "온도 센서");
        BindAndAddToDictionaryAndInit((int)GameObj.TemperatureSensor_Whole, "온도 센서");
        BindAndAddToDictionaryAndInit((int)GameObj.LevelSensor, "레벨 센서");
        BindAndAddToDictionaryAndInit((int)GameObj.FlowMeter, "유량 센서");
        BindAndAddToDictionaryAndInit((int)GameObj.PressureSensor, "압력 센서");
        BindAndAddToDictionaryAndInit((int)GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionaryAndInit((int)GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionaryAndInit((int)GameObj.TS_SensingElement, "감온부");
        BindAndAddToDictionaryAndInit((int)GameObj.TS_Cover, "커버");
        
        GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(true);
        GetObject((int)GameObj.TemperatureSensor).SetActive(false);
        
    //    StartCoroutine(OnSceneStartCo());
        
        contentController.OnDepth2Clicked(1); 
    }


   
    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }
    
    private void OnDepth2Finished()
    {
        
    }
  

    private void InitializeStates()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 1111, new Depth1A_State_1(this) },
            { 1112, new Depth1A_State_2(this) },
            { 1113, new Depth1A_State_3(this) },
            { 1114, new Depth1A_State_4(this) },
            { 1115, new Depth1A_State_5(this) },
            { 1116, new Depth1A_State_6(this) },
            { 1117, new Depth1A_State_7(this) },
            { 1118, new Depth1A_State_8(this) },
            { 1119, new Depth1A_State_9(this) },
            { 11110, new Depth1A_State_10(this) },
            { 11111, new Depth1A_State_11(this) },
            { 11112, new Depth1A_State_12(this) },
            { 11113, new Depth1A_State_13(this) },
            { 11114, new Depth1A_State_14(this) },
            { 11115, new Depth1A_State_15(this) },
            { 11116, new Depth1A_State_16(this) },
            { 11117, new Depth1A_State_17(this) },
            { 11118, new Depth1A_State_18(this) },
            { 11119, new Depth1A_State_19(this) },
            { 11120, new Depth1A_State_20(this) },
            { 11121, new Depth1A_State_16(this) },
            { 11122, new Depth1A_State_17(this) },
            { 11123, new Depth1A_State_18(this) },
            { 11124, new Depth1A_State_19(this) },
            { 11125, new Depth1A_State_20(this) },
        };
    }
}