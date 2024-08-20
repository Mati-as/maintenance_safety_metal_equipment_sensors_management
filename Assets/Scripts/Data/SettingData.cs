using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class SettingData
{
    
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public float mainVolume;
	[XmlAttribute]
	public float narration;
	[XmlAttribute]
	public float effect; // 업그레이드 비용
	[XmlAttribute]
	public float bgm; // 업그레이드 변화값
	[XmlAttribute]
	public float resolution;
	[XmlAttribute]
	public float graphicQuality;
	[XmlAttribute]
	public float kor;
	[XmlAttribute]
	public float eng;

    
}

[Serializable, XmlRoot("ArrayOfSettingData")]
public class SettingDataLoader : ILoader<int, SettingData>
{
	[XmlElement("TextData")]
	public List<SettingData> settingData = new List<SettingData>();

	public Dictionary<int, SettingData> MakeDict()
	{
		Dictionary<int, SettingData> dic = new Dictionary<int, SettingData>();

		foreach (SettingData data in settingData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}