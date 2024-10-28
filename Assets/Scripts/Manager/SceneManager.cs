
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneType
{
    Main,
    Depth1A,
    Depth1B,
    Depth1C
}

/// <summary>
/// 1. 씬전환시 초기화 로직과 결합가능
/// 2. 화면 가림처리 (로딩화면 여기에 추가)
/// </summary>
public class SceneLoader :MonoBehaviour
{

    public void Init()
    {
        return;
    }
    public void LoadScene(SceneType sceneType)
    {
        Logger.Log($"{sceneType}scene loading");
        
        Time.timeScale = 1f;
        
        Managers.Sound.StopAllAudio();

        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneType.ToString());
        // asyncLoad.allowSceneActivation = false; // prevent the scene from activating until manually done
        // asyncLoad.completed += (AsyncOperation op) => {
        //     
        //     Debug.Log("Scene load complete!");
        //     Managers.UI.ClosePopupUI();
        // };
        // StartCoroutine(UpdateLoadingProgress(asyncLoad));
        
       // Managers.UI.ShowPopupUI<UI_Loading>().GetComponent<UI_Loading>().PlayLoadingAnim();
        
        SceneManager.LoadSceneAsync(sceneType.ToString());
    }


    IEnumerator UpdateLoadingProgress(AsyncOperation asyncLoad)
    {
        while (!asyncLoad.isDone)
        {
            // progress is from 0 to 0.9, so adjust for percentage
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Managers.UI.ShowPopupUI<UI_Loading>().tmp_loading.text = $"{(int)(progress * 100)}%";

            // if the load is essentially complete (at 90%)
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // activate the scene
            }

            yield return null;
        }
    }


    public void ReloadScene()
    {
        Logger.Log($"{SceneManager.GetActiveScene().name}");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        char[] letters = name.ToLower().ToCharArray();
        letters[0] = char.ToUpper(letters[0]);
        return new string(letters);
    }
}
