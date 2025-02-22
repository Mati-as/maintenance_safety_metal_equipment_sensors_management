﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
	protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

	protected bool _init = false;

	protected Dictionary<int, bool> _isScaleEventOn = new Dictionary<int, bool>();
	public virtual bool Init()
	{
		if (_init)
			return false;

		return _init = true;
	}

	private void Start()
	{
		Init();
	}

	protected void Bind<T>(Type type) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects);

#if UNITY_EDITOR
//Debug.Log($"object counts to bind {names.Length}");
#endif
		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = Utils.FindChild(gameObject, names[i], true);
			else
				objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

			if (objects[i] == null)
				Debug.Log($"Failed to bind({names[i]})");
		}
	}


	protected void BindObject(Type type) { Bind<GameObject>(type);  }
	protected void BindImage(Type type) { Bind<Image>(type);  }
	protected void BindTMP(Type type) { Bind<TextMeshProUGUI>(type);  }
	protected void BindNonUITMP(Type type) { Bind<TextMeshPro>(type);  }
	protected void BindText(Type type) { Bind<Text>(type);  }

	protected void BindButton(Type type)
	{
		// bind button as usual
		Bind<Button>(type);

		// retrieve buttons array and check type explicitly


	
	}

	
	protected void BindToggle(Type type) { Bind<Toggle>(type);  }
	protected void BindSlider(Type type) { Bind<Slider>(type);  }
	
	
	protected T Get<T>(int idx) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_objects.TryGetValue(typeof(T), out objects) == false)
			return null;

		return objects[idx] as T;
	}

	protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
	protected Text GetText(int idx) { return Get<Text>(idx); }
	protected TextMeshProUGUI GetTMP(int idx) { return Get<TextMeshProUGUI>(idx); }
	protected TextMeshPro GetNonUITMP(int idx) { return Get<TextMeshPro>(idx); }
	protected virtual Button GetButton(int idx)
	{
		_isScaleEventOn.TryAdd(idx,false);

		if (!_isScaleEventOn[idx])
		{
			var btn = Get<Button>(idx);

			var originalScale = btn.transform.localScale;

			// apply mouse enter scaling
			BindEvent(btn.gameObject, () =>
			{
				if (btn.interactable) btn.transform.DOScale(originalScale * 1.1f, 0.18f);
				//				Logger.Log($"Button Scale Animation Applied: {btn.gameObject.name}");
			}, Define.UIEvent.PointerEnter);

			// apply mouse exit scaling
			BindEvent(btn.gameObject, () =>
				{
					if (btn.interactable) btn.transform.DOScale(originalScale, 0.15f);
				},
				Define.UIEvent.PointerExit);

			_isScaleEventOn[idx] = true;
		}


		return Get<Button>(idx);
		
		
	}
	protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
	protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
	protected Image GetImage(int idx) { return Get<Image>(idx); }

	public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
	{  
		UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);
        
		
		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action;  // Unbind all existing events
				evt.OnClickHandler += action;  // Bind the new action
				break;
			case Define.UIEvent.Pressed:
				evt.OnPressedHandler -= action;  // Unbind all existing events
				evt.OnPressedHandler += action;  // Bind the new action
				break;
			case Define.UIEvent.PointerDown:
				evt.OnPointerDownHandler -= action;  
				evt.OnPointerDownHandler += action;  
				break;
			case Define.UIEvent.PointerUp:
				evt.OnPointerUpHandler -= action;  
				evt.OnPointerUpHandler += action;  
				break;
			case Define.UIEvent.PointerEnter:
				evt.OnPointerEnterHander -= action;  
				evt.OnPointerEnterHander += action;  
				break;
			case Define.UIEvent.PointerExit:
				evt.OnPointerExitHandler -= action;  
				evt.OnPointerExitHandler += action;  
				break;
		}

		
	}

	public static void UnBindEvent(GameObject go)
	{
		UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

	//	Logger.Log($"{go.name}'s events unbound-------------------------------");
	
		evt.OnClickHandler = null;
		evt.OnPressedHandler = null;
		evt.OnPointerDownHandler = null;
		evt.OnPointerUpHandler = null;
		evt.OnPointerEnterHander = null;
		evt.OnPointerExitHandler = null;


	}


}
