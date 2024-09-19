using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneType
{
    Main,
    Depth1,
    Depth2,
    Depth3
}

/// <summary>
/// 1. 씬전환시 초기화 로직과 결합가능
/// 2. 화면 가림처리 (로딩화면 여기에 추가)
/// </summary>
public class SceneLoader
{
    public void LoadScene(SceneType sceneType)
    {
        Logger.Log($"{sceneType}scene loading");

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
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
