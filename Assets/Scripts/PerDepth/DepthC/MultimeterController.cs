using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MultimeterController : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TextMeshPro TMPDisplay;

    private enum Multimeter
    {
        MultimeterHandle,
        Display,
        MeasureGuide
    }

    private bool isDragging;
    private Vector3 initialMousePos;
    private float currentAngle;
    private const float minAngle = 0f; // 최소 각도
    private const float maxAngle = 150f; // 최대 각도
    private bool _isClickable = true;
    private readonly float _clikableDelay = 0.75f;


    private readonly float resistantTarget = 108;
    private Sequence _resistanceCheckSeq;


    public bool isResistanceMode;
    private readonly float _targetAngle = 100;
    private readonly int CLICK_COUNT_GOAL = 4; //4번 클릭시 저항측정 모드로 변경
    private int _currentClickCount;
    public static event Action OnResistanceMeasureReadyAction;

    public int currentClickCount
    {
        get => _currentClickCount;
        set
        {

            if (_currentClickCount != value) Managers.Sound.Play(SoundManager.Sound.Effect, "Object/DigitalMeterDial",0.5f);
            
            _currentClickCount = value;
            _currentClickCount = Mathf.Clamp(_currentClickCount, 0, 4);


            if (_currentClickCount == 0)
                TMPDisplay.text = "";
            else if (_currentClickCount > 0 && _currentClickCount < 4) TMPDisplay.text = "000.0";
            if (_currentClickCount >= 4)
            {
                TMPDisplay.text = "O.L";
                Logger.Log("Resistance Sensor Mode On ------------");
                isResistanceMode = true;
                OnResistanceMeasureReadyAction?.Invoke();
            }
            else
            {
                isResistanceMode = false;
            }
        }
    }


    private void Awake()
    {
        BindObject(typeof(Multimeter));

        TMPDisplay = GetObject((int)Multimeter.Display).GetComponent<TextMeshPro>();
        TMPDisplay.text = "";
        SetMeasureGuideStatus(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Logger.Log("Multimeter Clicked");
        if (eventData.pointerCurrentRaycast.gameObject.name == "MultimeterHandle") isDragging = true;

        if (!_isClickable) return;
        _isClickable = false;
        DOVirtual.Float(0, 1, _clikableDelay, _ => { }).OnComplete(() => _isClickable = true);


        currentClickCount++;
        PivotMultimeterHandle();
        // // 월드 좌표에서 핸들의 위치 가져오기
        // Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
        // initialMousePos = worldMousePos;
    }

    public void PivotMultimeterHandle()
    {
        var cacheCurrentAngle = currentAngle;
        var targetAngle = _targetAngle * (currentClickCount / (float)CLICK_COUNT_GOAL);
        Logger.Log($"Current Target Angle of Multimeter This time ---------->{targetAngle}");
        DOVirtual.Float(cacheCurrentAngle, targetAngle,
            0.15f,
            val =>
            {
                GetObject((int)Multimeter.MultimeterHandle).transform.localRotation = Quaternion.Euler(270f, val, 0f);
            }).OnComplete(() =>
        {
            currentAngle = GetObject((int)Multimeter.MultimeterHandle).transform.localEulerAngles.y;
        });
    }

    public void SetToResistanceModeAndRotation()
    {
        _currentClickCount = CLICK_COUNT_GOAL;
        var cacheCurrentAngle = currentAngle;
        DOVirtual.Float(cacheCurrentAngle, 0,
            0.15f,
            val =>
            {
                GetObject((int)Multimeter.MultimeterHandle).transform.localRotation =
                    Quaternion.Euler(270f, _targetAngle, 0f);
            }).OnComplete(() =>
        {
            currentAngle = GetObject((int)Multimeter.MultimeterHandle).transform.localEulerAngles.y;
        });

        isResistanceMode = true;
    }

    public void TurnOffResistantMode()
    {
        _currentClickCount = 0;
        var cacheCurrentAngle = currentAngle;
        GetObject((int)Multimeter.MultimeterHandle).transform.localRotation = Quaternion.Euler(270f, 0, 0f);
        currentAngle = GetObject((int)Multimeter.MultimeterHandle).transform.localRotation.y;

        isResistanceMode = false;
    }
    
    public void OnAllProbeSet()
    {
        _resistanceCheckSeq?.Kill();
        _resistanceCheckSeq = DOTween.Sequence();


        Logger.Log("프로브 접촉 완료, 저항값 변경중 -----------------------------------------------------");
        var lastUpdateTime = 0f;

        _resistanceCheckSeq.AppendInterval(1.5f);
        _resistanceCheckSeq.AppendCallback(() => DOVirtual.Float(0, resistantTarget, 0.9f, val =>
        {
            var currentTime = Time.time;

            if (currentTime - lastUpdateTime >= 0.5f)
            {
                TMPDisplay.text = (val + Random.Range(0, 2.5f)).ToString("F1");
                lastUpdateTime = currentTime;
            }
        }).SetEase(Ease.InOutBounce));

        _resistanceCheckSeq.AppendCallback(() =>
        {
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ =>
            {
                var currentTime = Time.time;

                if (currentTime - lastUpdateTime >= 0.78f)
                {
                    TMPDisplay.text = (resistantTarget + Random.Range(0, 2.5f)).ToString("F1");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.Play();
    }

    public void OnGroundNothing()
    {
        _resistanceCheckSeq?.Kill();
        _resistanceCheckSeq = DOTween.Sequence();


        var lastUpdateTime = 0f;

        _resistanceCheckSeq.AppendInterval(0.5f);
        _resistanceCheckSeq.AppendCallback(() =>
        {
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ =>
            {
                var currentTime = Time.time;

                if (currentTime - lastUpdateTime >= 0.78f)
                {
                    TMPDisplay.text = (0 + Random.Range(0, 0.005f)).ToString("F3");
                    lastUpdateTime = currentTime;
                }
            }).SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.SetLoops(-1);
        _resistanceCheckSeq.Play();
    }

    public void OnAllProbeSetToGroundingTerminal()
    {
        _resistanceCheckSeq?.Kill();
        _resistanceCheckSeq = DOTween.Sequence();

        Logger.Log("OL 표시 완료 -------------------------grounding mission");

        _resistanceCheckSeq.AppendInterval(1.5f);
        _resistanceCheckSeq.AppendCallback(() =>
        {
            DOVirtual.Float(resistantTarget, resistantTarget, 3f, _ => { TMPDisplay.text = "O.L"; })
                .SetEase(Ease.InOutBounce);
        });

        _resistanceCheckSeq.SetLoops(3);
        _resistanceCheckSeq.Play();
    }

    private Image _guideImage;
    private Sequence _guideImageSeq;

    public void SetMeasureGuideStatus(bool isOn = true)
    {
        GetObject((int)Multimeter.MeasureGuide).gameObject.SetActive(isOn);
        if (_guideImage == null) _guideImage = GetObject((int)Multimeter.MeasureGuide).GetComponent<Image>();
        
        _guideImageSeq?.Kill();
        _guideImageSeq = DOTween.Sequence();


        _guideImageSeq.AppendInterval(isOn ? 1.0f : 0);
        _guideImageSeq.AppendCallback(()=>
        {
            _guideImage.DOFade(isOn ? 1 : 0, 0.75f);
        });

    }


    public void OnDrag(PointerEventData eventData)
    {
        return;
        // throw new NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        return;
        //throw new NotImplementedException();
    }
}