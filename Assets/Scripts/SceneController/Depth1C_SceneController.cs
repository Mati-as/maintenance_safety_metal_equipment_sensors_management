using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DepthC_GameObj
{
    ElectricScrewdriver,
    LimitSwitch,
    TemperatureSensor,
    OnTempSensor_Pipe,
    TemperatureSensor_Whole,//분해 로직이랑 구분
    LevelSensor,
    FlowMeter,
    PressureSensor,
    //Parts
    TS_CompensatingWire,
    TS_Stabilizer,
    TS_SensingElement,
    TS_Cover,
    TS_LockingScrew
}
public class Depth1C_SceneController : Base_SceneController
{
    


   public override void Init()
    {   
        base.Init();
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        InitializeStates();
        
        if(Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum();
        
        BindObject(typeof(DepthC_GameObj));
        
        GetObject((int)DepthC_GameObj.TemperatureSensor).SetActive(true);
        StartCoroutine(OnSceneStartCo());
        
        BindAndAddToDictionary((int)DepthC_GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_SensingElement, "센서 연결부분 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_Cover, "커버");
        BindAndAddToDictionary((int)DepthC_GameObj.OnTempSensor_Pipe, "배관 연결 확인");
        BindAndAddToDictionary((int)DepthC_GameObj.TS_LockingScrew, "고정나사 체결확인");
        
        GetObject((int)DepthC_GameObj.TS_LockingScrew).BindEvent(() =>
        {
            contentController.isStepMissionComplete = true;
            contentController.InvokeNextStep();
            OnMissionFinish();
            contentController.isStepMissionComplete = false;
        });
        _screwDriver = GameObject.Find("ElectrcScrewdriver");
    }
   private GameObject _screwDriver;

   private void OnMissionFinish()
   {
       SetHighlightIgnore((int)DepthC_GameObj.TS_LockingScrew,true);
   }
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
            { 3211, new Depth1C21_State_1(this) },
            { 3212, new Depth1C21_State_2(this) },
            { 3213, new Depth1C21_State_3(this) },
            { 3214, new Depth1C21_State_4(this) },
            { 3215, new Depth1C21_State_5(this) },
            { 3216, new Depth1C21_State_6(this) },
            { 3217, new Depth1C21_State_7(this) },
            { 3218, new Depth1C21_State_8(this) },
            { 3219, new Depth1C21_State_9(this) },
            { 32110, new Depth1C21_State_10(this) },
            { 32111, new Depth1C21_State_11(this) },
            { 32112, new Depth1C21_State_12(this) },
            { 32113, new Depth1C21_State_13(this) },
            { 32114, new Depth1C21_State_14(this) },
            { 32115, new Depth1C21_State_15(this) },
            { 32116, new Depth1C21_State_16(this) },
            { 32117, new Depth1C21_State_17(this) },
            { 32118, new Depth1C21_State_18(this) },
            { 32119, new Depth1C21_State_19(this) },
            { 32120, new Depth1C21_State_20(this) },
            { 32121, new Depth1C21_State_16(this) },
            { 32122, new Depth1C21_State_17(this) },
            { 32123, new Depth1C21_State_18(this) },
            { 32124, new Depth1C21_State_19(this) },
            { 32125, new Depth1C21_State_20(this) },
        };
    }
}
