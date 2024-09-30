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
        BindAndAddToDictionary(GameObj.LimitSwitch, "리밋 스위치");
        BindAndAddToDictionary(GameObj.ProximitySwitch, "근접 스위치");
        //BindAndAddToDictionary(GameObj.TemperatureSensor, "온도 센서");
        BindAndAddToDictionary(GameObj.TemperatureSensor_Whole, "온도 센서");
        BindAndAddToDictionary(GameObj.LevelSensor, "레벨 센서");
        BindAndAddToDictionary(GameObj.FlowMeter, "유량 센서");
        BindAndAddToDictionary(GameObj.PressureSensor, "압력 센서");
        BindAndAddToDictionary(GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionary(GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionary(GameObj.TS_SensingElement, "감온부");
        BindAndAddToDictionary(GameObj.TS_Cover, "커버");
        
        GetObject((int)GameObj.TemperatureSensor_Whole).SetActive(true);
        GetObject((int)GameObj.TemperatureSensor).SetActive(false);
        
        StartCoroutine(OnSceneStartCo());
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
            { 20, new Depth1A_State_20(this) },
            { 21, new Depth1A_State_16(this) },
            { 22, new Depth1A_State_17(this) },
            { 23, new Depth1A_State_18(this) },
            { 24, new Depth1A_State_19(this) },
            { 25, new Depth1A_State_20(this) },
        };
    }
}