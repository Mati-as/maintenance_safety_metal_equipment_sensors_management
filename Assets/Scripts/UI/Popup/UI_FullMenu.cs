using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_FullMenu : UI_Popup
{
    private enum Btns
    {
        Btn_Close,
        //Status Name
        B110,
        B120,
        B210,
        B220,
        B230,
        B310,
        B320,
        B330,
        B340,
        B350,
        B410,
        B420,
        B430,
        B440,
        B450,
        
    }
    




    
    private enum TMPs
    {
        Text_FullMenuHeading,
        
        TMP_Overall_Training_Goal_Title,
        TMP_Overall_Traning_Goal_Info,
        TMP_TrainingObjectInfo_Info,
        TMP_TrainingObjectInfo_Title,
        
        TMP_B110,
        TMP_B120,
        TMP_B210,
        TMP_B220,
        TMP_B230,
        TMP_B310,
        TMP_B320,
        TMP_B330,
        TMP_B340,
        TMP_B350,
        TMP_B410,
        TMP_B420,
        TMP_B430,
        TMP_B440,
        TMP_B450,

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));
        BindTMP(typeof(TMPs));

        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });

       GetTMP((int)TMPs.TMP_Overall_Training_Goal_Title).text = Managers.GetText(Define.OverallTraningGoal);
       GetTMP((int)TMPs.TMP_Overall_Traning_Goal_Info).text =  Managers.GetText(Define.TrainingObjectInfo);

        GetButton((int)Btns.B110).gameObject.BindEvent(() => { LoadStep(Btns.B110, 11101.ToString()); });
        GetButton((int)Btns.B120).gameObject.BindEvent(() => { LoadStep(Btns.B120, 12101.ToString()); });
        GetButton((int)Btns.B210).gameObject.BindEvent(() => { LoadStep(Btns.B210, 21101.ToString()); });
        GetButton((int)Btns.B220).gameObject.BindEvent(() => { LoadStep(Btns.B220, 22101.ToString()); });
        GetButton((int)Btns.B230).gameObject.BindEvent(() => { LoadStep(Btns.B230, 23101.ToString()); });
        GetButton((int)Btns.B310).gameObject.BindEvent(() => { LoadStep(Btns.B310, 31101.ToString()); });
        GetButton((int)Btns.B320).gameObject.BindEvent(() => { LoadStep(Btns.B320, 32101.ToString()); });
        GetButton((int)Btns.B330).gameObject.BindEvent(() => { LoadStep(Btns.B330, 33101.ToString()); });
        GetButton((int)Btns.B340).gameObject.BindEvent(() => { LoadStep(Btns.B340, 34101.ToString()); });
        GetButton((int)Btns.B350).gameObject.BindEvent(() => { LoadStep(Btns.B350, 35101.ToString()); });
        GetButton((int)Btns.B410).gameObject.BindEvent(() => { LoadStep(Btns.B410, 41101.ToString()); });
        GetButton((int)Btns.B420).gameObject.BindEvent(() => { LoadStep(Btns.B420, 42101.ToString()); });
        GetButton((int)Btns.B430).gameObject.BindEvent(() => { LoadStep(Btns.B430, 43101.ToString()); });
        GetButton((int)Btns.B440).gameObject.BindEvent(() => { LoadStep(Btns.B440, 44101.ToString()); });
        GetButton((int)Btns.B450).gameObject.BindEvent(() => { LoadStep(Btns.B450, 45101.ToString()); });

        GetButton((int)Btns.B110).gameObject.BindEvent(() => { SetText(1); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B120).gameObject.BindEvent(() => { SetText(1); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B210).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B220).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B230).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B310).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B320).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B330).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B340).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B350).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B410).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B420).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B430).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B440).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B450).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);

        GetButton((int)Btns.B110).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B110).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B210).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B210).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B210).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B310).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B310).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B310).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B310).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B310).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B410).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B420).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B430).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B440).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B450).gameObject.BindEvent(SetEmptyText, Define.UIEvent.PointerExit);

        RefreshText();
        SetEmptyText();
        return true;
    }

    private void LoadStep(Btns btn, string statusToLoad)
    {

        if (!GetButton((int)btn).interactable)
        {
            Logger.Log($"해당버튼 비활성화 상태입니다. 구현완료 시 Interactable 해제 필요합니다.");
            return;
        }
        Managers.ContentInfo.PlayData.Depth1 = int.Parse(statusToLoad[0].ToString());
        Managers.ContentInfo.PlayData.Depth2 =  int.Parse(statusToLoad[1].ToString());
        Managers.ContentInfo.PlayData.Depth3 =  int.Parse(statusToLoad[2].ToString());
        Managers.ContentInfo.PlayData.Count  =  10*int.Parse(statusToLoad[3].ToString()) + int.Parse(statusToLoad[4].ToString());
     
        Managers.UI.ClosePopupUI(Managers.UI.FindPopup<UI_FullMenu>());
        if(Managers.UI.FindPopup<UI_ContentController>() == null)Managers.UI.ShowPopupUI<UI_ContentController>();
        else
        {
            Managers.UI.ClosePopupUI(Managers.UI.FindPopup<UI_ContentController>());
            Managers.UI.ShowPopupUI<UI_ContentController>();
        }

        
        //뎁스3(실습) 부분은 센서별로 별도로 씬구성하기에 조건문으로 구분.
        if (Managers.ContentInfo.PlayData.Depth1 == (int)Define.Depth.MaintenancePractice)
        {
            SceneManager.LoadScene("DepthC" + Managers.ContentInfo.PlayData.Depth2.ToString());
        }
        else
        {
            LoadScene(Managers.ContentInfo.PlayData.Depth1);
        }
    }
    
    private void LoadScene(int scene)
    {
        Managers.UI.CloseAllPopupUI();
        switch (scene)
        {
            case 1:

                Managers.Scene.LoadScene(SceneType.DepthA);
                break;
            case 2:

                Managers.Scene.LoadScene(SceneType.DepthB);
                break;
            case 3:

             
                break;
            case 4:

                Managers.Scene.LoadScene(SceneType.DepthD2);
                break;
        }
    }

    private void SetText(int depth)
    {
        switch (depth)
        {
            case 1:
                GetTMP((int)TMPs.TMP_TrainingObjectInfo_Info).text = Managers.Data.IsEngMode() ?
                    "Prior to practicing the inspection and maintenance of field measurement sensors, trainees will review the overall configuration of the equipment utilizing the sensors and study the basic principles of operation and structure of each sensor."
                    : 
                    "현장 계측 센서 점검, 정비 실습 전 계측 센서가 활용되는 설비에 대한 전반적인 구성을 확인하고 각 센서의 대략적인 동작원리 및 구조에 대해 학습합니다.";
                break;
            case 2:
                GetTMP((int)TMPs.TMP_TrainingObjectInfo_Info).text = Managers.Data.IsEngMode() ?
                    "To ensure the safe execution of field measurement sensor maintenance tasks, trainees will study potential hazards and safety management procedures."
                    : 
                    "현장 계측 센서 정비작업의 안전한 수행을 위해 위험 요소, 안전 관리 작업에 대해 학습합니다.";
                break;
            case 3:
                GetTMP((int)TMPs.TMP_TrainingObjectInfo_Info).text = Managers.Data.IsEngMode() ?
                    "Trainees will conduct practical exercises on the inspection, failure types, and maintenance procedures for field measurement sensors, including detection switches, temperature sensors, pressure sensors, flow sensors, and level sensors."
                    : 
                    "현장 계측 센서(검출 스위치, 온도 센서, 압력 센서, 유량 센서, 레벨 센서)의 점검, 고장 유형, 정비 과정에 대해 실습을 진행합니다.";
                break;
            case 4:
                GetTMP((int)TMPs.TMP_TrainingObjectInfo_Info).text = Managers.Data.IsEngMode() ?
                    "Trainees will review the content learned in previous training sessions regarding the maintenance of measurement sensors."
                    : 
                    "이전 학습과정에서 학습한 계측 센서 정비 내용을 확인합니다";
                break;
        }
    }

    private void SetEmptyText()
    {
     
        GetTMP((int)TMPs.TMP_TrainingObjectInfo_Info).text =
            "";
    }

    public override void RefreshText()
    {
        GetTMP((int)TMPs.Text_FullMenuHeading).text = Managers.Data.IsEngMode()?  "Full Menu" :"전체 메뉴"  ;
        
        GetTMP((int)TMPs.TMP_TrainingObjectInfo_Title).text = Managers.Data.IsEngMode()?  "Training\nObjective" : "전체 훈련 목표"  ;
        
       GetTMP((int)TMPs.TMP_B110).text = Managers.GetText(110);
       GetTMP((int)TMPs.TMP_B120).text = Managers.GetText(120);
       GetTMP((int)TMPs.TMP_B210).text = Managers.GetText(210);
       GetTMP((int)TMPs.TMP_B220).text = Managers.GetText(220);
       GetTMP((int)TMPs.TMP_B230).text = Managers.GetText(230);
       GetTMP((int)TMPs.TMP_B310).text = Managers.GetText(310);
       GetTMP((int)TMPs.TMP_B320).text = Managers.GetText(320);
       GetTMP((int)TMPs.TMP_B330).text = Managers.GetText(330);
       GetTMP((int)TMPs.TMP_B340).text = Managers.GetText(340);
       GetTMP((int)TMPs.TMP_B350).text = Managers.GetText(350);
       GetTMP((int)TMPs.TMP_B410).text = Managers.GetText(410);
       GetTMP((int)TMPs.TMP_B420).text = Managers.GetText(420);
       GetTMP((int)TMPs.TMP_B430).text = Managers.GetText(430);
       GetTMP((int)TMPs.TMP_B440).text = Managers.GetText(440);
       GetTMP((int)TMPs.TMP_B450).text = Managers.GetText(450);
       

    }
    
    protected override Button GetButton(int idx)
    {
        if (idx != (int)Btns.Btn_Close)  return Get<Button>(idx);
        
        _isScaleEventOn.TryAdd(idx,false);

        if (!_isScaleEventOn[idx])
        {
            var btn = Get<Button>(idx);
            var originalScale = btn.transform.localScale;

            // apply mouse enter scaling
            BindEvent(btn.gameObject, () =>
            {
                btn.transform.DOScale(originalScale * 1.1f, 0.18f);
//				Logger.Log($"Button Scale Animation Applied: {btn.gameObject.name}");
            }, Define.UIEvent.PointerEnter);

            // apply mouse exit scaling
            BindEvent(btn.gameObject, () => { btn.transform.DOScale(originalScale, 0.15f); },
                Define.UIEvent.PointerExit);

            _isScaleEventOn[idx] = true;
        }
	
		
        return Get<Button>(idx);
		
		
    }
}
