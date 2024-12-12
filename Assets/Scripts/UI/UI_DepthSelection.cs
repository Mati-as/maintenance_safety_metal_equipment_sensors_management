using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_DepthSelection : UI_Popup
{
    private enum DepthBtns
    {
        Btn_DepthA,
        Btn_DepthB,
        Btn_DepthC,
        Btn_DepthD
        //    Btn_DepthC2,
        //    Btn_DepthD2
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


        BindButton(typeof(DepthBtns));

        GetButton((int)DepthBtns.Btn_DepthA).gameObject.BindEvent(OnDepthBtnAClicked);
        GetButton((int)DepthBtns.Btn_DepthB).gameObject.BindEvent(() =>
        {
            if (GetButton((int)DepthBtns.Btn_DepthB).IsInteractable()) OnDepthBtnBClicked();
        });
        GetButton((int)DepthBtns.Btn_DepthC).gameObject.BindEvent(() =>
        {
            if (GetButton((int)DepthBtns.Btn_DepthC).IsInteractable()) OnDepthCBtnClicked();
        });
        
        GetButton((int)DepthBtns.Btn_DepthD).gameObject.BindEvent(() =>
        {
            if (GetButton((int)DepthBtns.Btn_DepthD).IsInteractable()) OnDepthDBtnCClicked();
        });
//        GetButton((int)DepthBtns.Btn_DepthC2).gameObject.BindEvent(OnDepthCBtnClicked);
        //  GetButton((int)DepthBtns.Btn_DepthD2).gameObject.BindEvent(OnDepthDBtnCClicked);


        GetButton((int)DepthBtns.Btn_DepthA).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHA_OPEN); },Define.UIEvent.PointerEnter);
        GetButton((int)DepthBtns.Btn_DepthB).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHB_OPEN); },Define.UIEvent.PointerEnter);
        GetButton((int)DepthBtns.Btn_DepthC).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHC_OPEN); },Define.UIEvent.PointerEnter);
        GetButton((int)DepthBtns.Btn_DepthD).gameObject.BindEvent(() => { ChangeDepth1Animation(DEPTHD_OPEN); },Define.UIEvent.PointerEnter);
        
        GetButton((int)DepthBtns.Btn_DepthA).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        GetButton((int)DepthBtns.Btn_DepthB).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        GetButton((int)DepthBtns.Btn_DepthC).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);
        GetButton((int)DepthBtns.Btn_DepthD).gameObject.BindEvent(ShutDepth1Animation ,Define.UIEvent.PointerExit);

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

        Managers.Scene.LoadScene(SceneType.DepthC2);
        Managers.ContentInfo.PlayData.Depth1 = 3;
        Managers.ContentInfo.PlayData.Depth2 = 2; //구현 부분뎁스만큼 수정해야합니다.
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
        if (idx <= (int)DepthBtns.Btn_DepthD) return Get<Button>(idx);
        
        
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
}