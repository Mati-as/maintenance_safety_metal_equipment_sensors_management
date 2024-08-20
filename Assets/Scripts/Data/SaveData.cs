using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class SaveData
{
    
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string StrVal;
	[XmlAttribute]
	public float MainVolume;
	[XmlAttribute]
	public float Narration;
	[XmlAttribute]
	public float Effect; // 업그레이드 비용
	[XmlAttribute]
	public float Bgm; // 업그레이드 변화값
	[XmlAttribute]
	public float Resolution;
	[XmlAttribute]
	public float GraphicQuality;
	[XmlAttribute]
	public float Kor;
	[XmlAttribute]
	public float Eng;

    
}

[Serializable, XmlRoot("ArrayOfSettingData")]
public class SaveDataLoader : ILoader<int, SaveData>
{
	[XmlElement("TextData")]
	public List<SaveData> SavData = new List<SaveData>();

	public Dictionary<int, SaveData> MakeDict()
	{
		Dictionary<int, SaveData> dic = new Dictionary<int, SaveData>();

		foreach (SaveData data in SavData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}