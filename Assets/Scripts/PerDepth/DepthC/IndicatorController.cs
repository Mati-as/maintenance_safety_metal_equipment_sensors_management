using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class IndicatorController : UI_Base
{
    
    public TextMeshPro TMPDisplay;

    private Sequence _indicatorDisplaySequence;
    
    private enum Multimeter
    {
        Display
    }
    
    private void Awake()
    {
        BindObject(typeof(Multimeter));

        TMPDisplay = GetObject((int)Multimeter.Display).GetComponent<TextMeshPro>();
        TMPDisplay.text = "";
    }

    public void ShowErrorMessage()
    {
        _indicatorDisplaySequence.Kill();
        _indicatorDisplaySequence = DOTween.Sequence();
        
        _indicatorDisplaySequence.AppendCallback(()=>
        {
            TMPDisplay.text = "ERROR";
        });
        _indicatorDisplaySequence.AppendInterval(1.2f);
        
        _indicatorDisplaySequence.AppendCallback(()=>
        {
            TMPDisplay.text = "";
        });
        _indicatorDisplaySequence.AppendInterval(1.2f);

        _indicatorDisplaySequence.SetLoops(-1);
    }
    
    
    public void ShowTemperature(float delay)
    {
        _indicatorDisplaySequence.Kill();
        
        _indicatorDisplaySequence = DOTween.Sequence();

        _indicatorDisplaySequence.AppendInterval(delay);
        _indicatorDisplaySequence.AppendCallback(()=>
        {
            TMPDisplay.text = "25.0";
        });
        _indicatorDisplaySequence.SetLoops(5);
    }
    
    public void ShowNothing(float delay =0)
    {
        _indicatorDisplaySequence.Kill();
        
        _indicatorDisplaySequence = DOTween.Sequence();

        _indicatorDisplaySequence.AppendInterval(delay);
        _indicatorDisplaySequence.AppendCallback(()=>
        {
            TMPDisplay.text = string.Empty;
        });
        _indicatorDisplaySequence.SetLoops(5);
    }
    
    
}
