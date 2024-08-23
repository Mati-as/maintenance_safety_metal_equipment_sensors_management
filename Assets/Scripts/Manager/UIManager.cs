using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
	

    
	
	
	int _order = -20;

	public static readonly bool KOR = false;
	public static readonly bool ENG = true;
	public static readonly bool FULLSCREEN_MODE = true;
	public static readonly bool WINDOWED_MODE = false;
	public static readonly bool CONTROL_GUIDE_ON = true;
	public static readonly bool CONTROL_GUIDE_OFF = false;
	
	private bool _isFullScreen;
	private bool _languageSetting;
	private bool _isGuideOn;

	public bool isFullScreen { get; set; }
	public bool languageSetting { get; set; }
	public bool GuideOn { get; set; }

	Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

	public UI_Scene SceneUI { get; private set; }

	public GameObject Root
	{
		get
		{
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };

			return root;
		}
	}

	public void SetCanvas(GameObject go, bool sort = true)
	{
		Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.overrideSorting = true;

		if (sort)
		{
			canvas.sortingOrder = _order;
			_order++;
		}
		else
		{
			canvas.sortingOrder = 0;
		}
	}

	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/SubItem/{name}");

		GameObject go = Managers.Resource.Instantiate(prefab);
		if (parent != null)
			go.transform.SetParent(parent);

		go.transform.localScale = Vector3.one;
		go.transform.localPosition = prefab.transform.position;

		return Utils.GetOrAddComponent<T>(go);
	}

	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Utils.GetOrAddComponent<T>(go);
		SceneUI = sceneUI;

		go.transform.SetParent(Root.transform);

		return sceneUI;
	}

	public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/Popup/{name}");

		GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
		T popup = Utils.GetOrAddComponent<T>(go);
		_popupStack.Push(popup);

		if (parent != null)
			go.transform.SetParent(parent);
		else if (SceneUI != null)
			go.transform.SetParent(SceneUI.transform);
		else
			go.transform.SetParent(Root.transform);

		go.transform.localScale = Vector3.one;
		go.transform.localPosition = prefab.transform.position;

		return popup;
	}

	public T FindPopup<T>() where T : UI_Popup
	{
		return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
	}

	public T PeekPopupUI<T>() where T : UI_Popup
	{
		if (_popupStack.Count == 0)
			return null;

		return _popupStack.Peek() as T;
	}

	public void ClosePopupUI(UI_Popup popup)
	{
		if (_popupStack.Count == 0)
			return;

		if (_popupStack.Peek() != popup)
		{
			Debug.Log("Close Popup Failed!");
			return;
		}

		ClosePopupUI();
	}

	public void ClosePopupUI()
	{
		if (_popupStack.Count == 0)
			return;

		UI_Popup popup = _popupStack.Pop();
		Managers.Resource.Destroy(popup.gameObject);
		popup = null;
		_order--;
	}

	public void CloseAllPopupUI()
	{
		while (_popupStack.Count > 0)
			ClosePopupUI();
	}

	public void Clear()
	{
		CloseAllPopupUI();
		SceneUI = null;
	}
	
	public void SetResolution(int width, int height, bool fullscreen)
	{
		Screen.SetResolution(width, height, fullscreen);
		isFullScreen = fullscreen;
		// UI 레이아웃이나 스케일 조정을 위한 추가 로직을 여기에 추가
	}
	
	public void SetGraphicQuality(Define.QaulityLevel qualityLevel)
	{
     	QualitySettings.SetQualityLevel((int)qualityLevel);
		// UI 레이아웃이나 스케일 조정을 위한 추가 로직을 여기에 추가
	}

	public void SetScreenMode(bool isFullScreenMode)
	{
#if UNITY_EDITOR
		Debug.Log($"Full Screen mode is {isFullScreenMode}");
#endif
		Screen.fullScreen = isFullScreenMode;
		isFullScreen = isFullScreenMode;
		Managers.Data.Preference[(int)Define.Preferences.Fullscreen] = isFullScreenMode? 1 : 0;
	}

	public void SetEngMode(bool mode)
	{

#if UNITY_EDITOR
		string modename = mode ? "ENG" : "KOR";
		Debug.Log($"Language mode is  {GuideOn}");
#endif
		this.languageSetting = mode;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void SetControlGuideOnMode(bool isGuideOn)
	{
#if UNITY_EDITOR
		Debug.Log($"Control Guide Mode is {isGuideOn}");
#endif
		this.GuideOn = isGuideOn;
	}
}
