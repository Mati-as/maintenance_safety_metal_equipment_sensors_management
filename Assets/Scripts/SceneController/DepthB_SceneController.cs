using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthB_SceneController : Base_SceneController
{

    public bool isEvalStart; // 버튼UI 표출, 점수평가 시작등에 사용됩니다. 
    
    
 
    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum(); //개발용

        InitializeBStates();
        BindObject(typeof(DepthC_GameObj));
     
        contentController.OnDepth2Clicked(1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
        
    }
    
    
    
    private void InitializeBStates()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 2111, new StateB11_1(this) },
                
            { 2121, new StateB12_1(this) },
            { 2122, new StateB12_2(this) },
            { 2123, new StateB12_3(this) },
            { 2124, new StateB12_4(this) },
            { 2125, new StateB12_5(this) },
            { 2126, new StateB12_6(this) },
            { 2127, new StateB12_7(this) },
            { 2128, new StateB12_8(this) },
            { 2129, new StateB12_9(this) },
            { 21210, new StateB12_10(this) },
                
            { 2211, new StateB21_1(this) },
            { 2212, new StateB21_2(this) },
            { 2213, new StateB21_3(this) },
            { 2214, new StateB21_4(this) },
            { 2215, new StateB21_5(this) },
            { 2216, new StateB21_6(this) },
            { 2217, new StateB21_7(this) },
            { 2218, new StateB21_8(this) },
            { 2219, new StateB21_9(this) },
            { 22110, new StateB21_10(this) },
            { 22111, new StateB21_11(this) },
            { 22112, new StateB21_12(this) },

            { 2221, new StateB22_1(this) },
            { 2222, new StateB22_2(this) },
            { 2223, new StateB22_3(this) },
            { 2224, new StateB22_4(this) },
            { 2225, new StateB22_5(this) },
            { 2226, new StateB22_6(this) },
            { 2227, new StateB22_7(this) },
            { 2228, new StateB22_8(this) },
            { 2229, new StateB22_9(this) },
            { 22210, new StateB22_10(this) },
    
         
        };
    }
    
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 2;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }
}
