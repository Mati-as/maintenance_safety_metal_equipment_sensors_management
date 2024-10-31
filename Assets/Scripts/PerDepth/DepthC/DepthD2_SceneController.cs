using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 1. 기본적으로 센서 로직 상속
/// 2. 애니메이션 별도 구성
/// 3. BasesceneController 기본적으로 상속
/// </summary>
public class DepthD2_SceneController : DepthC2_SceneController
{

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 2;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum(); //개발용
        base.Init();
        
        
        BindObject(typeof(DepthC_GameObj));
        GetScrewColliders();
        InitializeD2States();
        
        contentController.OnDepth2Clicked(2); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다.
    }
    
     private void InitializeD2States()
    {

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 4211, new DepthC21_State_1(this) },
            { 4212, new DepthC21_State_2(this) },
            { 4213, new DepthC21_State_3(this) },
            { 4214, new DepthC21_State_4(this) },
            { 4215, new DepthC21_State_5(this) },
            { 4216, new DepthC21_State_6(this) },
            { 4217, new DepthC21_State_7(this) },
            { 4218, new DepthC21_State_8(this) },
            { 4219, new DepthC21_State_9(this) },
            { 42110, new DepthC21_State_10(this) },
            { 42111, new DepthC21_State_11(this) },
            { 42112, new DepthC21_State_12(this) },
            { 42113, new DepthC21_State_13(this) },
            { 42114, new DepthC21_State_14(this) },
            { 42115, new DepthC21_State_15(this) },
            { 42116, new DepthC21_State_16(this) },
            { 42117, new DepthC21_State_17(this) },
            // { 32118, new DepthC21_State_18(this) },
            // { 32119, new DepthC21_State_19(this) },
       
        };
    }
}
