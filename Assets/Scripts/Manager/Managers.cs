using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    public static Managers Instance => s_instance;

    private static SceneManagerEx s_sceneManager = new();
    private static SoundManager s_soundManager = new();
    private static DataManager s_dataManager = new();
    private static UIManager s_uiManager = new();
    private static ContentPlayManager _sContentPlayManager = new(); 
    private static ResourceManager s_resourceManager = new ResourceManager();
    
    
    // private static AdsManager s_adsManager = new AdsManager();
    // private static GameManagerEx s_gameManager = new GameManagerEx();
    // private static IAPManager s_iapManager = new IAPManager();

    //  public static GameManagerEx Game { get { Init(); return s_gameManager; } }
    public static DataManager Data
    { get { Init(); return s_dataManager; }}

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
    public static string GetData(Define.SaveData idx)
    {
        if (Managers.Data.SaveData.TryGetValue((int)idx, out SaveData value) == false)
            return "null";
        return value.StrVal;

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

            //   s_adsManager.Init();
            //   s_iapManager.Init();
            s_resourceManager.Init();
            s_dataManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
            _sContentPlayManager.Init();

            Sound.Play(SoundManager.Sound.Bgm, "Bgm",0.1f);
            Application.targetFrameRate = 60;
        }
    }
}