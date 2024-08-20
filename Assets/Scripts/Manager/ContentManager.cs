using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// // 1.current info tracking
///    2.Control ContentController
/// </summary>
public class ContentData
{
    public string Name;

}
public class ContentManager
{
    private ContentData _contentData = new ContentData();
    public ContentData SaveData { get { return _contentData; } set { _contentData = value; } }
    
    public string Name
    {
        get { return _contentData.Name; }
        set { _contentData.Name = value; }
    }

}
