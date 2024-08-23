using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    public static Managers Instance => s_instance;

    private static SceneManagerEx s_sceneManager = new();
    private static SoundManager s_soundManager = new();
    private static DataManager _sDataManager = new();
    private static UIManager s_uiManager = new();
    private static ContentPlayManager _sContentPlayManager = new(); 
    private static ResourceManager s_resourceManager = new ResourceManager();
    
    
    // private static AdsManager s_adsManager = new AdsManager();
    // private static GameManagerEx s_gameManager = new GameManagerEx();
    // private static IAPManager s_iapManager = new IAPManager();

    //  public static GameManagerEx Game { get { Init(); return s_gameManager; } }
    public static DataManager Data
    { get { Init(); return _sDataManager; }}

    public static ContentPlayManager ContentPlay;
    public static UIManager UI
    { get { Init(); return s_uiManager;}}
     public static ResourceManager Resource { get { Init(); return s_resourceManager; } }

    // public static AdsManager Ads { get { Init(); return s_adsManager; } }
    // public static IAPManager IAP { get { Init(); return s_iapManager; } }
    public static SceneManagerEx Scene
    { get { Init(); return s_sceneManager;}}

    public static SoundManager Sound
    { get { Init(); return s_soundManager; }}

    public string GetText(int id)
    {
        if (Managers.Data.Texts.TryGetValue(id, out TextData value) == false)
            return null;

        return value.kor.Replace("{userName}", Managers.ContentPlay.Name);
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
            _sDataManager.Init();
            
            
            s_resourceManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
            _sContentPlayManager.Init();

            InitialSet();

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
}