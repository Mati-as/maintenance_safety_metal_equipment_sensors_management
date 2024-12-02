using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;

public enum DepthAGameObj
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




public class DepthA_SceneController : Base_SceneController
{
    
    
    // public override void Start()
    // {
    //     base.Start();
    //     Init();
    // }
    /// <summary>
    /// 1.Base Setup구성
    /// 2.virtual로 센서실습 구현
    /// 3.override로 센서 평가하기 구현
    /// </summary>
    public override void Init()
    {   
        Dev_SetDepthNum();
        
        base.Init();
        InitializeStates();
        BindObject(typeof(DepthAGameObj));

        
       
        
        // 딕셔너리에 추가 및 이벤트 바인딩
        BindHighlight((int)DepthAGameObj.LimitSwitch, "리밋 스위치");
        BindHighlight((int)DepthAGameObj.ProximitySwitch, "근접 스위치");
        BindHighlight((int)DepthAGameObj.TemperatureSensor, "온도 센서");
        BindHighlight((int)DepthAGameObj.TemperatureSensor_Whole, "온도 센서");
        BindHighlight((int)DepthAGameObj.LevelSensor, "레벨 센서");
        BindHighlight((int)DepthAGameObj.FlowMeter, "유량 센서");
        BindHighlight((int)DepthAGameObj.PressureSensor, "압력 센서");
        BindHighlight((int)DepthAGameObj.TS_CompensatingWire, "보상전선");
        BindHighlight((int)DepthAGameObj.TS_Stabilizer, "고정자");
        BindHighlight((int)DepthAGameObj.TS_SensingElement, "감온부");
        BindHighlight((int)DepthAGameObj.TS_Cover, "덮개");
        
        GetObject((int)DepthAGameObj.TemperatureSensor_Whole).SetActive(true);
        GetObject((int)DepthAGameObj.TemperatureSensor).SetActive(false);
        contentController.OnDepth2Clicked( Managers.ContentInfo.PlayData.Depth2); 
        
    }


   
    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void Dev_SetDepthNum()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000")
        {
            Managers.ContentInfo.PlayData.Depth1 = 1;
            Managers.ContentInfo.PlayData.Depth2 = 1;
            Managers.ContentInfo.PlayData.Depth3 = 1;
            Managers.ContentInfo.PlayData.Count = 1;
        }
 
    }
    
    private void OnDepth2Finished()
    {
        
    }
  

    private void InitializeStates()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 1111, new DepthA1_State_1(this) },
            { 1112, new DepthA1_State_2(this) },
            { 1113, new DepthA1_State_3(this) },
            { 1114, new DepthA1_State_4(this) },
            { 1115, new DepthA1_State_5(this) },
            { 1116, new DepthA1_State_6(this) },
            { 1117, new DepthA1_State_7(this) },
            { 1118, new DepthA1_State_8(this) },
            { 1119, new DepthA1_State_9(this) },
            { 11110, new DepthA1_State_10(this) },
            { 11111, new DepthA1_State_11(this) },

            
            { 1211, new DepthA2_State_1(this) },
            { 1212, new DepthA2_State_2(this) },
            { 1213, new DepthA2_State_3(this) },
            { 1214, new DepthA2_State_4(this) },
            { 1215, new DepthA2_State_5(this) },
            { 1216, new DepthA2_State_6(this) },
            { 1217, new DepthA2_State_7(this) },
            { 1218, new DepthA2_State_8(this) },
            { 1219, new DepthA2_State_9(this) },
            { 12110, new DepthA2_State_10(this) },
            { 12111, new DepthA2_State_11(this) },
            { 12112, new DepthA2_State_12(this) },
            { 12113, new DepthA2_State_13(this) },
            { 12114, new DepthA2_State_14(this) },
            { 12115, new DepthA2_State_15(this) },
            { 12116, new DepthA2_State_16(this) }
        };
    }
}