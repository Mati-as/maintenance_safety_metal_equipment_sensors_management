using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_DepthSelection : UI_Popup
{
    private enum Btns
    {
        Btn_DepthA,
        Btn_DepthB,
        Btn_DepthC,
        Btn_DepthD,
        
        Btn_Close,
        
        //기본학습
        
        B111,
        B112,
        
        B121,
        B122,
        B123,
        B124,
        B125,
            
        
        //안전관리
        B211,
        B221,
        B231,
        B232,
        
        B311,
        B312,
        B313,
        
        B321,
        B322,
        B323,
        
        B331,
        B332,
        B333,
        
        B341,
        B342,
        B343,
        
        B351,
        B352,
        B353,
        
        //평가하기
        B411,
        B421,
        B431,
        B441,
        B451,
    }
    

    private Animator _animator;
    private readonly int DEPTHA_OPEN = Animator.StringToHash("DepthA_Open");
    private readonly int DEPTHB_OPEN = Animator.StringToHash("DepthB_Open");
    private readonly int DEPTHC_OPEN = Animator.StringToHash("DepthC_Open");
    private readonly int DEPTHD_OPEN = Animator.StringToHash("DepthD_Open");


    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        _animator = GetComponentInChildren<Animator>();


        BindButton(typeof(Btns));
        
        
        GetButton((int)Btns.Btn_DepthA).gameObject.BindEvent(OnDepthBtnAClicked);
        GetButton((int)Btns.Btn_DepthB).gameObject.BindEvent(() =>
        {
            if (GetButton((int)Btns.Btn_DepthB).IsInteractable()) OnDepthBtnBClicked();
        });
        GetButton((int)Btns.Btn_DepthC).gameObject.BindEvent(() =>
        {
            if (GetButton((int)Btns.Btn_DepthC).IsInteractable()) OnDepthCBtnClicked();
        });
        
        GetButton((int)Btns.Btn_DepthD).gameObject.BindEvent(() =>
        {
            if (GetButton((int)Btns.Btn_DepthD).IsInteractable()) OnDepthDBtnCClicked();
        });


        GetButton((int)Btns.B111).gameObject.BindEvent(() => { LoadStep(Btns.B111, 111); });
        GetButton((int)Btns.B111).gameObject.BindEvent(() => { LoadStep(Btns.B112, 112); });
        
        GetButton((int)Btns.B121).gameObject.BindEvent(() => { LoadStep(Btns.B121, 121); });
        GetButton((int)Btns.B121).gameObject.BindEvent(() => { LoadStep(Btns.B122, 122); });
        GetButton((int)Btns.B121).gameObject.BindEvent(() => { LoadStep(Btns.B123, 123); });
        GetButton((int)Btns.B121).gameObject.BindEvent(() => { LoadStep(Btns.B124, 124); });
        GetButton((int)Btns.B121).gameObject.BindEvent(() => { LoadStep(Btns.B125, 125); });
        
        GetButton((int)Btns.B211).gameObject.BindEvent(() => { LoadStep(Btns.B211, 211); });
        GetButton((int)Btns.B221).gameObject.BindEvent(() => { LoadStep(Btns.B221, 221); });
        
        GetButton((int)Btns.B231).gameObject.BindEvent(() => { LoadStep(Btns.B231, 231); });
        GetButton((int)Btns.B232).gameObject.BindEvent(() => { LoadStep(Btns.B232, 232); });
        
        
        
        GetButton((int)Btns.B311).gameObject.BindEvent(() => { LoadStep(Btns.B311, 311); });
        GetButton((int)Btns.B312).gameObject.BindEvent(() => { LoadStep(Btns.B312, 312); });
        GetButton((int)Btns.B313).gameObject.BindEvent(() => { LoadStep(Btns.B313, 313); });
        
        GetButton((int)Btns.B321).gameObject.BindEvent(() => { LoadStep(Btns.B321, 321); });
        GetButton((int)Btns.B322).gameObject.BindEvent(() => { LoadStep(Btns.B322, 322); });
        GetButton((int)Btns.B323).gameObject.BindEvent(() => { LoadStep(Btns.B323, 323); });
        
        GetButton((int)Btns.B331).gameObject.BindEvent(() => { LoadStep(Btns.B331, 331); });
        GetButton((int)Btns.B332).gameObject.BindEvent(() => { LoadStep(Btns.B332, 332); });
        GetButton((int)Btns.B333).gameObject.BindEvent(() => { LoadStep(Btns.B333, 333); });

        GetButton((int)Btns.B341).gameObject.BindEvent(() => { LoadStep(Btns.B341, 341); });
        GetButton((int)Btns.B342).gameObject.BindEvent(() => { LoadStep(Btns.B342, 342); });
        GetButton((int)Btns.B343).gameObject.BindEvent(() => { LoadStep(Btns.B343, 343); });
        
        GetButton((int)Btns.B351).gameObject.BindEvent(() => { LoadStep(Btns.B351, 351); });
        GetButton((int)Btns.B352).gameObject.BindEvent(() => { LoadStep(Btns.B352, 352); });
        GetButton((int)Btns.B353).gameObject.BindEvent(() => { LoadStep(Btns.B353, 353); });
        
        
        
        GetButton((int)Btns.B411).gameObject.BindEvent(() => { LoadStep(Btns.B411, 411); });
        GetButton((int)Btns.B421).gameObject.BindEvent(() => { LoadStep(Btns.B421, 421); });
        GetButton((int)Btns.B431).gameObject.BindEvent(() => { LoadStep(Btns.B431, 431); });
        GetButton((int)Btns.B441).gameObject.BindEvent(() => { LoadStep(Btns.B441, 441); });
        GetButton((int)Btns.B451).gameObject.BindEvent(() => { LoadStep(Btns.B451, 451); });
        
        
        GetButton((int)Btns.Btn_DepthA).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHA_OPEN); },Define.UIEvent.PointerDown);
        GetButton((int)Btns.Btn_DepthB).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHB_OPEN); },Define.UIEvent.PointerDown);
        GetButton((int)Btns.Btn_DepthC).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHC_OPEN); },Define.UIEvent.PointerDown);
        GetButton((int)Btns.Btn_DepthD).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHD_OPEN); },Define.UIEvent.PointerDown);
        
        // GetButton((int)Btns.Btn_DepthA).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        // GetButton((int)Btns.Btn_DepthB).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        // GetButton((int)Btns.Btn_DepthC).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        // GetButton((int)Btns.Btn_DepthD).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);

        return true;
    }

    private void ChangeDepth1Animation(int hash)
    {
        _animator.SetBool(DEPTHA_OPEN, false);
        _animator.SetBool(DEPTHB_OPEN, false);
        _animator.SetBool(DEPTHC_OPEN, false);
        _animator.SetBool(DEPTHD_OPEN, false);

        _animator.SetBool(hash, true);
    }
    
    private void ShutDepth1Animation()
    {
        _animator.SetBool(DEPTHA_OPEN, false);
        _animator.SetBool(DEPTHB_OPEN, false);
        _animator.SetBool(DEPTHC_OPEN, false);
        _animator.SetBool(DEPTHD_OPEN, false);

    }

    private void OnDepthBtnAClicked()
    {
//Managers.UI.ClosePopupUI(this);

        Managers.Scene.LoadScene(SceneType.DepthA);
#if UNITY_EDITOR
        Debug.Log("Depth 1 기본학습 Start");
#endif

        Managers.ContentInfo.PlayData.Depth1 = 1;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }

    private void OnDepthBtnBClicked()
    {
         Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 2 안전 관리 Start");
#endif
        Managers.Scene.LoadScene(SceneType.DepthB);
        Managers.ContentInfo.PlayData.Depth1 = 2;
        Managers.ContentInfo.PlayData.Depth2 = 1;
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        //OnDepth1Enter();
    }

    private void OnDepthCBtnClicked()
    {
        // Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 3 계측 센서 정비 Start");
#endif

        Managers.Scene.LoadScene(SceneType.DepthC1);
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 1; //구현 부분뎁스만큼 수정해야합니다.
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }

    private void OnDepthDBtnCClicked()
    {
        // Managers.UI.ClosePopupUI(this);
#if UNITY_EDITOR
        Debug.Log("Depth 3 계측 센서 정비 Start");
#endif

        //Managers.Scene.LoadScene(SceneType.DepthD2);
        Managers.ContentInfo.PlayData.Depth1 = 4;
        Managers.ContentInfo.PlayData.Depth2 = 2; //구현 부분뎁스만큼 수정해야합니다.
        Managers.ContentInfo.PlayData.Depth3 = 1;
        Managers.ContentInfo.PlayData.Count = 1;
        OnDepth1Enter();
    }


    private void OnDepth1Enter()
    {
        Managers.Sound.Pause(SoundManager.Sound.Bgm);
    }
    
    protected override Button GetButton(int idx)
    {
        if (idx <= (int)Btns.Btn_DepthD) return Get<Button>(idx);
        
        
        _isScaleEventOn.TryAdd(idx,false);
        
            
        if (!_isScaleEventOn[idx])
        {
            var btn = Get<Button>(idx);

            var originalScale = btn.transform.localScale;

            // apply mouse enter scaling
            BindEvent(btn.gameObject, () =>
            {
                //if (btn.interactable) btn.transform.DOScale(originalScale * 1.1f, 0.18f);
                //				Logger.Log($"Button Scale Animation Applied: {btn.gameObject.name}");
            }, Define.UIEvent.PointerEnter);

            // apply mouse exit scaling
            BindEvent(btn.gameObject, () =>
                {
                 //   if (btn.interactable) btn.transform.DOScale(originalScale, 0.15f);
                },
                Define.UIEvent.PointerExit);

            _isScaleEventOn[idx] = true;
        }


        return Get<Button>(idx);
		
		
    }
    
    
    private void LoadStep(Btns btn, int statusToLoad)
    {

        var statusToLoadToString = statusToLoad.ToString();
        
        if (!GetButton((int)btn).interactable)
        {
            Logger.Log($"해당버튼 비활성화 상태입니다. 구현완료 시 Interactable 해제 필요합니다.");
            return;
        }
        Managers.ContentInfo.PlayData.Depth1 = int.Parse(statusToLoadToString[0].ToString());
        Managers.ContentInfo.PlayData.Depth2 =  int.Parse(statusToLoadToString[1].ToString());
        Managers.ContentInfo.PlayData.Depth3 =  int.Parse(statusToLoadToString[2].ToString());
        Managers.ContentInfo.PlayData.Count = 1;
     
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
}