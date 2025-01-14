using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SafetyCheckEquipmentList : UI_Popup
{

    private enum TMPs
    {
        TMP_Head_EquipmentName,
        TMP_Body_EquipmentInfo,
        TMP_Body_EquipmentName_HowToUse,
        TMP_EquipmentName_Details,
        
    }

    private Animator _animator;
    
    private RectTransform toolTipRectPos { get; set; }
    public Vector3 toolTipPosOffset =new Vector3(350,115,0);
    public Vector3 toolTipPosOffsetLeft =new Vector3(350,-115,0);
    public override bool Init()
    {
        _animator = GetComponent<Animator>();
        BindTMP(typeof(TMPs));
        SetStatus(false);
        return true;
    }
    public void RefreshText(int EquipmentID)
    {
        GetTMP((int)TMPs.TMP_Head_EquipmentName).text = Managers.GetText(int.Parse(EquipmentID.ToString()));
        GetTMP((int)TMPs.TMP_Body_EquipmentInfo).text = Managers.GetText(int.Parse(EquipmentID +"0" ));
        GetTMP((int)TMPs.TMP_EquipmentName_Details).text =  Managers.GetText(int.Parse(EquipmentID + "00" ));
        
        //안정장구와 사용장비의 구분
        if (Managers.ContentInfo.PlayData.Depth2 == 1)
        {
            GetTMP((int)TMPs.TMP_Body_EquipmentName_HowToUse).text = Managers.Data.IsEngMode() ? "- How to Wear and Use:" : "- 착용 및 사용 방법:";
        }
        else
        {
            GetTMP((int)TMPs.TMP_Body_EquipmentName_HowToUse).text = Managers.Data.IsEngMode() ? "- How to Use:" : "- 사용 방법:";
        }
      
   
    }

    public void SetStatus(bool isOn)
    {
        ConditionCheck();
       
        //훈련목표 이후에 클릭 될 수 있도록  비활성화
        if (Managers.ContentInfo.PlayData.Count < 2)
        {
            _animator.SetBool(Define.UI_ON,false) ;
            return;
        }   
      
        _animator.SetBool(Define.UI_ON,isOn);   
    }

    private bool ConditionCheck()
    {
        if (Managers.ContentInfo.PlayData.Depth1 != (int)Define.Depth.Safety)
        {
            gameObject.SetActive(false);
            return false;
        }
        else
        {
            gameObject.SetActive(true);
            return true;
        }
    }
    
    protected virtual void Update()
    {
        Update_MousePosition();
    }


    protected virtual void Update_MousePosition()
    {
     
        toolTipPosOffset =new Vector3(350,115,0);
        Vector2 mousePos = Input.mousePosition + toolTipPosOffset;

        
        if (toolTipRectPos == null)
        {
            toolTipRectPos = GetComponent<RectTransform>();
            //Logger.Log("get safety tooltip rectpos");
        }
        
        
        toolTipRectPos.position = mousePos;

    }
}
