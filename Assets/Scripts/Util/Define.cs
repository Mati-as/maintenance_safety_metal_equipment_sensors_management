using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{
	public static int NO = 0;
	public static int YES = 1;
	public static int OFF = 0;
	public static int ON = 1;
	public static bool YES_BOOL = true;
	public static bool No_BOOL = false;
	public enum LanguageMode
	{
		Kor,
		Eng
	}

	public enum QaulityLevel
	{
		Low =  1,
		Mid =  2,
		Auto = 3,
		High = 4,
	}

	public enum Preferences
	{
		MainVol,
		NarrationVol,
		EffectVol,
		BgmVol,
		Fullscreen,
		Resolution,
		GraphicQuality,
		IsEng,
		ControlGuide, 
		//---------
		Mute_Main,
		Mute_Narration,
		Mute_Effect,
		Mute_Bgm
	}
	public enum Scene
	{
		Unknown,
		Dev,
		Release,
	}

	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
		PointerEnter,
		PointerExit
	}

	public enum SaveData
	{
		MainVolume,
		Narration,
		Effect,
		Bgm,
		Resolution,
		GraphicQuality,
		Language,
		ControlGuideOn
	}
	
	public enum Depth
	{
		SensorOverview=1,
		Safety=2,
		MaintenancePractice=3,
		Evaluation=4,
		Tutorial=5
	}
	
	
	//씬초기화시에만 사용중, 추후 필요없을 가능성 높음(12/12/24)
	public enum DepthC_Sensor
	{
		ProximitySensor=1,
		TemperatureSensor=2,
		PressureSensor=3,
		FlowSensor=4,
		LevelSensor=5
	}
	
	// Menu prefab, 등 순서 고려
	public const int TrainingObjectInfo = 3300;
	public const int OverallTraningGoal= 3301;
	public static int UI_ON = Animator.StringToHash("On");
	
	//public const int PleaseTouchScreen = 6501;

	
	
	
}
