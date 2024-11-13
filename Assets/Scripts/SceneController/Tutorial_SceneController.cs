using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HighlightPlus;
using UnityEngine;

public class Tutorial_SceneController : DepthC2_SceneController
{

    private UI_Tutorial _UITutorial;

    public UI_Tutorial UITutorial
    {
        get { return _UITutorial; }

        set { _UITutorial = value; }
    }
    
    
    public override void Init()
    {
       SetDepthNum(); //튜토리얼에서는 반드시 Depth를 Set해주고 들어가야합니다. 

        
        BindObject(typeof(DepthC_GameObj));
        BindEvent();
        InitTutorialStates();
        SetMainProperties();
    }

    protected override void SetDepthNum()
    {
        Managers.ContentInfo.PlayData.Depth1 = 5;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
    }

    public override void SetMainProperties()
    {
        _mainAnimation = GameObject.FindWithTag("ObjectAnimationController").GetComponent<Animation>();
        
        if(Managers.UI.SceneUI ==null) Managers.UI.ShowSceneUI<UI_Persistent>();
        
        objectHighlightMap = new Dictionary<int, HighlightEffect>();
        _seqMap = new Dictionary<int, Sequence>();

        Debug.Assert(Camera.main != null);
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();

        Logger.Log($"현재 씬 정보(status) : {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
       

        
    }
     public void TutorialInit()
    {
        cameraController = Camera.main.GetComponent<Inplay_CameraController>();
        currentScrewGaugeStatus = new Dictionary<int, float>();
        isScrewUnwindMap = new Dictionary<int, bool>();
        animatorMap = new Dictionary<int, Animator>();
        defaultRotationMap = new Dictionary<int, Quaternion>();
        if (UITutorial == null) UITutorial = Managers.UI.FindPopup<UI_Tutorial>().GetComponent<UI_Tutorial>();
        
        UI_Evaluation.OnRestartBtnOnEvalClicked -= TutorialInit;
        UI_Evaluation.OnRestartBtnOnEvalClicked += TutorialInit;
        UnBindEventAttatchedObj();
    
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TemperatureSensor, "온도센서");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_CompensatingWire, "보상전선");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Stabilizer, "고정자");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_Cover, "덮개");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_ConnectionPiping, "연결부 누수 확인");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewA, "측정 단자 A");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_GroundingTerminalB, "접지");
        BindAndAddToDictionaryAndInit((int)DepthC_GameObj.MultimeterHandleHighlight, "측정모드 설정");
        
        SetScrewDriverSection();
        
        InitProbePos();
        defaultRotationMap.TryAdd((int)DepthC_GameObj.Probe_Cathode,GetObject((int)DepthC_GameObj.Probe_Cathode).transform.rotation);
        defaultRotationMap.TryAdd((int)DepthC_GameObj.Probe_Anode,GetObject((int)DepthC_GameObj.Probe_Cathode).transform.rotation);
       
        
        UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
        UI_ToolBox.ToolBoxOnEvent += OnToolBoxClicked;

        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;

        UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
        UI_ToolBox.MultimeterClickedEvent += OnUI_MultimeterBtnClicked;

        
        

        GetObject((int)DepthC_GameObj.TS_InnerScrewA).BindEvent(() =>
        {
            if (Managers.ContentInfo.PlayData.Depth1 != 5) return;
            
            
            //108옴 저항측정
            if (Managers.ContentInfo.PlayData.Count == 7)
            {
                // isAnodePut = true; 
                // BindAndAddToDictionaryAndInit((int)DepthC_GameObj.TS_InnerScrewB, "측정 단자 B");
                // SetHighlightIgnore((int)DepthC_GameObj.TS_InnerScrewB, false);
                // //HighlightBlink((int)DepthC_GameObj.TS_InnerScrewB);
                //
                // animatorMap[(int)DepthC_GameObj.Probe_Anode].enabled = true;
                // animatorMap[(int)DepthC_GameObj.Probe_Anode].SetBool(PROBE_TO_SCREWB, true);
                //
                // DOVirtual.Float(0, 0, 2f, _ => { }).OnComplete(() => { isAnodePut = true; });
            }
            

        }, Define.UIEvent.PointerDown);


        PlayAnimationAndNarration(1);
    }
     
     protected override void BindEvent()
     {
         UI_Tutorial.OnStepBtnClicked_CurrentCount -= OnStepChange;
         UI_Tutorial.OnStepBtnClicked_CurrentCount += OnStepChange;
   
     }
     
     private new void OnDestroy()
     {
         base.OnDestroy();
         
         UI_Tutorial.OnStepBtnClicked_CurrentCount -= OnStepChange;
         
         UI_Evaluation.OnRestartBtnOnEvalClicked -= TutorialInit;
         UI_ToolBox.ToolBoxOnEvent -= OnToolBoxClicked;
         UI_ToolBox.MultimeterClickedEvent -= OnUI_MultimeterBtnClicked;
         UI_ToolBox.ScrewDriverClickedEvent -= OnElectricScrewdriverBtnClicked;
         MultimeterController.OnResistanceMeasureReadyAction -= OnResistanceReady;
         UI_ToolBox.TemperatureSensorClickedEvent -= OnUI_Btn_TemperatureSensorClicked;
     }

     protected override void OnUI_MultimeterBtnClicked()
     {
                 
         InitializeTool();
     
         CurrentActiveTool = (int)DepthC_GameObj.Multimeter;
         isMultimeterOn = !isMultimeterOn;
         multimeterController.SetToResistanceModeAndRotation();

         if (isMultimeterOn == false) CurrentActiveTool = -1;
     }
     

     
       private void InitTutorialStates()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 5111, new Tutorial_State_1(this) },
            { 5112, new Tutorial_State_2(this) },
            { 5113, new Tutorial_State_3(this) },
            { 5114, new Tutorial_State_4(this) },
            { 5115, new Tutorial_State_5(this) },
            { 5116, new Tutorial_State_6(this) },
            { 5117, new Tutorial_State_7(this) },
            { 5118, new Tutorial_State_8(this) },
            { 5119, new Tutorial_State_9(this) },
            { 51110, new Tutorial_State_10(this) },
            { 51111, new Tutorial_State_11(this) },

            // { 32118, new DepthC21_State_18(this) },
            // { 32119, new DepthC21_State_19(this) },


       
        };
    }
}
