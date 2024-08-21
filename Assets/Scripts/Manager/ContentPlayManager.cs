using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// // 1.current info tracking
///    2.Control ContentController
/// </summary>
public class ContentPlayData
{
    public string Name;

}
public class ContentPlayManager
{
    private ContentPlayData _contentPlayData = new ContentPlayData();
    public ContentPlayData savePlayData { get { return _contentPlayData; } set { _contentPlayData = value; } }
    
    public string Name
    {
        get { return _contentPlayData.Name; }
        set { _contentPlayData.Name = value; }
    }

    public void Init()
    {
 
    }

}
