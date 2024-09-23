using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamearAnimation
{
    Intro,
    Zoomin_IntroToLimitSensor,
    
}

public enum GameObjectAnimation
{
    Intro,
    MachineOn_ConveyorBeltRoll
}
public class Depth1A_SceneController : Base_SceneController
{
   
    public override void Init()
    {
        InitializeStates();
        SetParameters();
        base.Init();

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
    
    public override void OnStepChange(int currentDepth)
    {
       
        base.OnStepChange(currentDepth);
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
            { 20, new Depth1A_State_20(this) } // Depth 20은 예시로 Depth2State로 유지했지만, 새로운 상태로 교체할 수 있어
        };
    }

}
