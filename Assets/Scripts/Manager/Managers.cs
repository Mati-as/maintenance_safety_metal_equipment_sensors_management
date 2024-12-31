﻿using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    public static Managers Instance => s_instance;
    private static UI_Persistent s_ui_Persistent;
    private static SceneLoader s_sceneLoader = new();
    private static SoundManager s_soundManager = new();
    private static DataManager s_DataManager = new();
    private static UIManager s_uiManager = new();
    private static ContentPlayManager s_contentPlayManager = new(); 
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static EvaluationManager s_evaluationManager = new();

    private static bool _initialIntroAnimPlayed = false;
    private static bool _isTutorialAlreadyPlayed = false;

    public static bool isTutorialAlreadyPlayed
    {
        get { return _isTutorialAlreadyPlayed;}
        set { _isTutorialAlreadyPlayed = value;}
    }
    
    public static bool initialIntroAnimPlayed
    {
        get { return _initialIntroAnimPlayed;}
        set { _initialIntroAnimPlayed = value;}
    }

    public static UI_Persistent UI_Persistent
    {
        get { return s_ui_Persistent;}
        set { s_ui_Persistent = value;}
    }
    
    public static EvaluationManager evaluationManager
    {get{Init();
        return s_evaluationManager;}}
    
    
    //controller로  이름 변경우 UIManager로 통합가능 (11/01/24)
    private static CursorImageManager s_cursorImageManager= new CursorImageManager();
    public static CursorImageManager cursorImageManager
    {get{Init();return s_cursorImageManager;}}
    public static DataManager Data
    { get { Init(); return s_DataManager; }}

    public static ContentPlayManager ContentInfo  { get { Init(); return s_contentPlayManager;}}
    public static UIManager UI
    { get { Init(); return s_uiManager;}}
     public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    
    public static SceneLoader Scene
    { get { Init(); return s_sceneLoader;}}

    public static SoundManager Sound
    { get { Init(); return s_soundManager; }}

    public static string GetText(int id)
    {
        if (Managers.Data.Texts.TryGetValue(id, out TextData value) == false)
            return null;

        return Data.Preference[(int)Define.Preferences.EngMode] == 0 ? value.kor:value.eng;
    }


    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);
            
            //DataMAnager는 반드시 제일 먼저 초기화 되어야합니다.
            s_DataManager.Init();
            s_contentPlayManager.Init();
            s_resourceManager.Init();
            s_sceneLoader.Init();
            s_soundManager.Init();
            s_cursorImageManager.Init();
            InitialSet();
            Application.runInBackground = true;

        }
    }

    private static void InitialSet()
    {
        
        UI.SetScreenMode((int)(Managers.Data.Preference[(int)Define.Preferences.Fullscreen]) == 0 ? false : true);

        var resolution = (int)(Managers.Data.Preference[(int)Define.Preferences.Resolution]);
        switch (resolution)
        {
            case 1280:  UI.SetResolution(1280,720,UI.isFullScreen);
                break;
            case 1920: UI.SetResolution(1920,1080,UI.isFullScreen);
                break;
            case 2560: UI.SetResolution(2560,1440,UI.isFullScreen);
                break;
        }
            
        Application.targetFrameRate = 60;
    }

  

    private void OnApplicationQuit()
    {
        Managers.Data.SaveCurrentSetting();
    }
}