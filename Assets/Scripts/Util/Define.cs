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
		LimitSwitch=1,
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


	
	public const int Safety_Helmet =2991;
	public const int Safety_InsulatedGloves= 2992;
	public const int Safety_EarPlugs= 2993;
	public const int Safety_Mask= 2994;
	public const int Safety_ProtectiveGoggles= 2995;
	public const int Safety_Shoes= 2996;
	public const int Safety_FlameResistantSuit= 2997;

	
	public const int Safety_ElectronicDriver =2981;
	public const int Safety_Multimeter= 2982;
	public const int Safety_PressureCalibrator= 2983;
	public const int Safety_Stripper= 2984;
	public const int Safety_Wrench= 2985;




}
