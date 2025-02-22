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
        B11101,
        B12101,
        B21101,
        B22101,
        B23101,
        B31101,
        B32101,
        B33101,
        B34101,
        B35101,
        B41101,
        B42101,
        B43101,
        B44101,
        B45101,
        
    }

    private enum Texts
    {
        Text_Overall_Training_Goal,
        Text_TrainingObjectInfo,
        Text_TrainingObjectInfo_Detail
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        // BindObject(typeof(GameObj));
        BindButton(typeof(Btns));
        BindText(typeof(Texts));

        GetButton((int)Btns.Btn_Close).gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(this); });

        GetText((int)Texts.Text_Overall_Training_Goal).text = Managers.Data.Texts[Define.OverallTraningGoal].kor;
        GetText((int)Texts.Text_TrainingObjectInfo).text = Managers.Data.Texts[Define.TrainingObjectInfo].kor;

        GetButton((int)Btns.B11101).gameObject.BindEvent(() => { LoadStep(Btns.B11101, 11101.ToString()); });
        GetButton((int)Btns.B12101).gameObject.BindEvent(() => { LoadStep(Btns.B12101, 12101.ToString()); });
        GetButton((int)Btns.B21101).gameObject.BindEvent(() => { LoadStep(Btns.B21101, 21101.ToString()); });
        GetButton((int)Btns.B22101).gameObject.BindEvent(() => { LoadStep(Btns.B22101, 22101.ToString()); });
        GetButton((int)Btns.B23101).gameObject.BindEvent(() => { LoadStep(Btns.B23101, 23101.ToString()); });
        GetButton((int)Btns.B31101).gameObject.BindEvent(() => { LoadStep(Btns.B31101, 31101.ToString()); });
        GetButton((int)Btns.B32101).gameObject.BindEvent(() => { LoadStep(Btns.B32101, 32101.ToString()); });
        GetButton((int)Btns.B33101).gameObject.BindEvent(() => { LoadStep(Btns.B33101, 33101.ToString()); });
        GetButton((int)Btns.B34101).gameObject.BindEvent(() => { LoadStep(Btns.B34101, 34101.ToString()); });
        GetButton((int)Btns.B35101).gameObject.BindEvent(() => { LoadStep(Btns.B35101, 35101.ToString()); });
        GetButton((int)Btns.B41101).gameObject.BindEvent(() => { LoadStep(Btns.B41101, 41101.ToString()); });
        GetButton((int)Btns.B42101).gameObject.BindEvent(() => { LoadStep(Btns.B42101, 42101.ToString()); });
        GetButton((int)Btns.B43101).gameObject.BindEvent(() => { LoadStep(Btns.B43101, 43101.ToString()); });
        GetButton((int)Btns.B44101).gameObject.BindEvent(() => { LoadStep(Btns.B44101, 44101.ToString()); });
        GetButton((int)Btns.B45101).gameObject.BindEvent(() => { LoadStep(Btns.B45101, 45101.ToString()); });

        GetButton((int)Btns.B11101).gameObject.BindEvent(() => { SetText(1); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B12101).gameObject.BindEvent(() => { SetText(1); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B21101).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B22101).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B23101).gameObject.BindEvent(() => { SetText(2); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B31101).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B32101).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B33101).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B34101).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B35101).gameObject.BindEvent(() => { SetText(3); }, Define.UIEvent.PointerEnter);
        
        GetButton((int)Btns.B41101).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B42101).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B43101).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B44101).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);
        GetButton((int)Btns.B45101).gameObject.BindEvent(() => { SetText(4); }, Define.UIEvent.PointerEnter);

        GetButton((int)Btns.B11101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B12101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B21101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B22101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B23101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B31101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B32101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B33101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B34101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);
        GetButton((int)Btns.B35101).gameObject.BindEvent(ResetText, Define.UIEvent.PointerExit);

        ResetText();
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
                GetText((int)Texts.Text_TrainingObjectInfo_Detail).text =
                    "현장 계측 센서 점검, 정비 실습 전 계측 센서가 활용되는 설비에 대한 전반적인 구성을 확인하고 각 센서의 대략적인 동작원리 및 구조에 대해 학습합니다.";
                break;
            case 2 :
                GetText((int)Texts.Text_TrainingObjectInfo_Detail).text =
                    "현장 계측 센서 정비작업의 안전한 수행을 위해 위험 요소, 안전 관리 작업에 대해 학습합니다.";
                break;
            case 3 :
                GetText((int)Texts.Text_TrainingObjectInfo_Detail).text =
                    "현장 계측 센서(검출 스위치, 온도 센서, 압력 센서, 유량 센서, 레벨 센서)의 점검, 고장 유형, 정비 과정에 대해 실습을 진행합니다.";
                break;
            case 4 :
                GetText((int)Texts.Text_TrainingObjectInfo_Detail).text =
                    "이전 학습과정에서 학습한 계측 센서 정비 내용을 확인합니다";
                break;
        }
    }

    private void ResetText()
    {
     
        GetText((int)Texts.Text_TrainingObjectInfo_Detail).text =
            "(문구필요)마우스 커서를 과정명에 올려 훈련 목표를 확인합니다.";
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
