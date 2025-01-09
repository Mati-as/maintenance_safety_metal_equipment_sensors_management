using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;



public class TextData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string kor;
	[XmlAttribute]
	public string eng;
	// ...
}

[Serializable, XmlRoot("ArrayOfTextData")]
public class TextDataLoader : ILoader<int, TextData>
{
	public TextDataLoader(){}
	[XmlElement("TextData")]
	public List<TextData> _textData = new List<TextData>();

	public Dictionary<int, TextData> MakeDict()
	{
		Dictionary<int, TextData> dic = new Dictionary<int, TextData>();

		foreach (TextData data in _textData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}

public class ObjectTextData
{
	public int EnumId;
	public string Kor;
	public string Eng;
	
	public ObjectTextData(int enumId, string kor, string eng)
	{
		EnumId = enumId;
		Kor = kor;
		Eng = eng;
	}
}
