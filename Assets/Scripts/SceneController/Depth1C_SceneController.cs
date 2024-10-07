using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth1C_SceneController : Base_SceneController
{
    
    public enum DepthC_GameObj
    {
        ElectricScrewdriver,
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

   public override void Init()
    {   
        base.Init();
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        InitializeStates();
        SetDepthNum();
        
        BindObject(typeof(DepthC_GameObj));
        
        GetObject((int)DepthC_GameObj.TemperatureSensor).SetActive(true);
        
        StartCoroutine(OnSceneStartCo());
        
        _screwDriver = GameObject.Find("ElectricScrewdriver");
    }
   private GameObject _screwDriver;

   public bool isDriverOn { get; set; }


   private void Update()
   {
       SetDriverPos();
   }

   private void SetDriverPos()
   {
       if (isDriverOn)
       {
           float distanceFromCamera = 0.23f; 
           Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
               distanceFromCamera));
           _screwDriver.transform.position = mousePosition;
           Logger.Log($"On_CurrentPos: {_screwDriver.transform.position}");
            
       }
   }
   private void ToggleActiveState(GameObject obj,  bool isOn)
   {
       isOn = !isOn;
       obj.SetActive(isOn);
   }

   public void OnScrewDriverBtnClicked()
   {
       bool isOn = isDriverOn;
       ToggleActiveState(_screwDriver, isOn);
       isDriverOn = !isOn;

       
       Logger.Log($"is driver on? : {isDriverOn}");
   }
   
   
   
    /// <summary>
    ///     1.씬로드 전,후 두번  파라미터를 로드해줍니다.
    ///     2. 각 씬별로도 테스트를 할 수 있도록 하기 위함입니다.
    /// </summary>
    private void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }
    


    private void InitializeStates()
    {
        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 1, new Depth1C_State_1(this) },
            { 2, new Depth1C_State_2(this) },
            { 3, new Depth1C_State_3(this) },
            { 4, new Depth1C_State_4(this) },
            { 5, new Depth1C_State_5(this) },
            { 6, new Depth1C_State_6(this) },
            { 7, new Depth1C_State_7(this) },
            { 8, new Depth1C_State_8(this) },
            { 9, new Depth1C_State_9(this) },
            { 10, new Depth1C_State_10(this) },
            { 11, new Depth1C_State_11(this) },
            { 12, new Depth1C_State_12(this) },
            { 13, new Depth1C_State_13(this) },
            { 14, new Depth1C_State_14(this) },
            { 15, new Depth1C_State_15(this) },
            { 16, new Depth1C_State_16(this) },
            { 17, new Depth1C_State_17(this) },
            { 18, new Depth1C_State_18(this) },
            { 19, new Depth1C_State_19(this) },
            { 20, new Depth1C_State_20(this) },
            { 21, new Depth1C_State_16(this) },
            { 22, new Depth1C_State_17(this) },
            { 23, new Depth1C_State_18(this) },
            { 24, new Depth1C_State_19(this) },
            { 25, new Depth1C_State_20(this) },
        };
    }
}
