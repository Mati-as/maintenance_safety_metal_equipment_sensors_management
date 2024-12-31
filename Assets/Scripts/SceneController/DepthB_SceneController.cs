using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthB_SceneController : Base_SceneController
{

    public bool isEvalStart; // 버튼UI 표출, 점수평가 시작등에 사용됩니다. 
    
    public enum DepthB_Objects // for referring array or making it string
    {
        
    
         
      //안전장구(캐릭터)
      OnCharacter_Helmet,
      OnCharacter_InsulatedGloves,
      OnCharacter_Mask,
      OnCharacter_ProtectiveGoggles,
      OnCharacter_Earplugs,
      OnCharacter_InsulatedShoes,
      OnCharacter_FlameResistantClothing, //캐릭터위에 실제로 배치되어있지는 않음
      //안전장구(보관함)
      OnStorage_Helmet,
      OnStorage_InsulatedGloves,
      OnStorage_InsulatedShoes,
      OnStorage_Mask,
      OnStorage_ProtectiveGoggles,
      OnStorage_Earplugs,
      OnStorage_FlameResistantClothing,
      
      
      Character_NoFlameSuit,
          
      //집기
      Stripper,
      Wrench,
      Multimeter,
      PressureCalibrator,
      ElectronicDriver,
      
    }
 
    public override void Init()
    {
        if (Managers.ContentInfo.PlayData.CurrentDepthStatus == "00000") SetDepthNum(); //개발용

        
        base.Init();
        
        SetMainProperties();
        InitializeBStates();
        BindObject(typeof(DepthB_Objects));
        contentController.OnDepth2Init(1); // 함수명에 혼동의여지있으나, 로직은 동일하게 동작합니다. 
        

    }


 
    
    private void InitializeBStates()
    {
        if (_sceneStates == null)
        {

        

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 2111, new StateB11_1(this) },
            { 2112, new StateB11_2(this) },
            { 2113, new StateB11_3(this) },
            { 2114, new StateB11_4(this) },
            { 2115, new StateB11_5(this) },
            { 2116, new StateB11_6(this) },
            { 2117, new StateB11_7(this) },
            { 2118, new StateB11_8(this) },
            { 2119, new StateB11_9(this) },
            { 21110, new StateB11_10(this) },
                
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
            
            

            { 2311, new StateB31_1(this) },
            { 2312, new StateB31_2(this) },
            { 2313, new StateB31_3(this) },
            { 2314, new StateB31_4(this) },
            { 2315, new StateB31_5(this) },
            { 2316, new StateB31_6(this) },
            { 2317, new StateB31_7(this) },
            { 2318, new StateB31_8(this) },
            { 2319, new StateB31_9(this) },
            { 23110, new StateB31_10(this) },
    
         
        };
        }
    }

    public void TurnOffObjectOnCharacter(DepthB_Objects objOnCharacter = DepthB_Objects.OnCharacter_Helmet)
    {
        for (int i = (int)objOnCharacter; i <= (int)DepthB_Objects.OnCharacter_FlameResistantClothing; i++)
        {
            GetObject(i).gameObject.SetActive(false);
        }
        
    }

    private void TurnOffAllOnCharacter()
    {
        for (int i = (int)DepthB_Objects.OnCharacter_Helmet; i <= (int)DepthB_Objects.OnCharacter_FlameResistantClothing; i++)
        {
            GetObject(i).gameObject.SetActive(false);
        }
    }
    public void DepthB11Init()
    {
        BindHighlight((int)DepthB_Objects.OnStorage_Helmet,"안전모");
        GetObject((int)DepthB_Objects.OnStorage_Helmet).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 3)
            {
                GetObject((int)DepthB_Objects.OnCharacter_Helmet).SetActive(true);
            }  
        });
        
        BindHighlight((int)DepthB_Objects.OnStorage_InsulatedGloves,"절연 장갑");
        GetObject((int)DepthB_Objects.OnStorage_InsulatedGloves).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 4)
            {
                GetObject((int)DepthB_Objects.OnCharacter_InsulatedGloves).SetActive(true);
            }  
        });
        
        
        BindHighlight((int)DepthB_Objects.OnStorage_Earplugs,"귀마개");
        GetObject((int)DepthB_Objects.OnStorage_Earplugs).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 5)
            {
                GetObject((int)DepthB_Objects.OnCharacter_Earplugs).SetActive(true);
            }  
        });
        
        
        BindHighlight((int)DepthB_Objects.OnStorage_Mask,"방진 마스크");
        GetObject((int)DepthB_Objects.OnStorage_Mask).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 6)
            {
                GetObject((int)DepthB_Objects.OnCharacter_Mask).SetActive(true);
            }  
        });
        
        
        BindHighlight((int)DepthB_Objects.OnStorage_ProtectiveGoggles,"보호안경");
        GetObject((int)DepthB_Objects.OnStorage_ProtectiveGoggles).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 7)
            {
                GetObject((int)DepthB_Objects.OnCharacter_ProtectiveGoggles).SetActive(true);
            }  
        });
        
        
        BindHighlight((int)DepthB_Objects.OnStorage_InsulatedShoes,"안전화");
        GetObject((int)DepthB_Objects.OnStorage_InsulatedShoes).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 8)
            {
                GetObject((int)DepthB_Objects.OnCharacter_InsulatedShoes).SetActive(true);
            }  
        });
        
        BindHighlight((int)DepthB_Objects.OnStorage_FlameResistantClothing,"방염복");
        GetObject((int)DepthB_Objects.OnStorage_FlameResistantClothing).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth3 == 1 && Managers.ContentInfo.PlayData.Count == 9) 
            {
                GetObject((int)DepthB_Objects.Character_NoFlameSuit).SetActive(false);
                GetObject((int)DepthB_Objects.OnCharacter_FlameResistantClothing).SetActive(true);
            }

            GetObject((int)DepthB_Objects.OnStorage_Helmet);
        });

        TurnOffAllOnCharacter();
    }
    protected virtual void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 2;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 0;
    }
}
