using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraPostEffectController : MonoBehaviour
{
        
    public float fadeDuration = 2.5f; // Fade duration as a variable
    private float _fadeInDefaultStartVal = -20;
    private float _fadeOutDefaultEndtVal = -20;
    private float _originalPostExposureValue;
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private Coroutine fadeCoroutine;



    public void FadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutEffect());
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeInEffect());
    }

    private IEnumerator FadeOutEffect()
    {
        if (colorAdjustments == null) yield break;

        float startValue = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(_originalPostExposureValue, _fadeOutDefaultEndtVal, elapsedTime / fadeDuration);
            colorAdjustments.postExposure.value = newValue;
            yield return null;
        }

        colorAdjustments.postExposure.value = _fadeOutDefaultEndtVal;
    }
    
    private IEnumerator FadeInEffect()
    {
        if (colorAdjustments == null) yield break;

        float startValue = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(_fadeInDefaultStartVal, _originalPostExposureValue, elapsedTime / fadeDuration);
            colorAdjustments.postExposure.value = newValue;
            yield return null;
        }

        colorAdjustments.postExposure.value = _originalPostExposureValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the Volume component from the main camera
        volume = Camera.main.GetComponent<Volume>();

        if (volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = true;
            _originalPostExposureValue =colorAdjustments.postExposure.value;
        }
        else
        {
            Debug.LogError("ColorAdjustments not found or Volume missing!");
        }

        FadeIn();
    }


}
