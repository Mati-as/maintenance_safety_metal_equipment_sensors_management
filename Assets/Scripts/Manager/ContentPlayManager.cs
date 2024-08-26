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
    public string CurrentDepthInfo = "000"; //<ID>1(Depth1) 1(Depth2)

}
public class ContentPlayManager
{
    
    private ContentPlayData _playPlayData = new ContentPlayData();
    public ContentPlayData PlayData { get { return _playPlayData; } set { _playPlayData = value; } }
    
    public string Name
    {
        get { return _playPlayData.Name; }
        set { _playPlayData.Name = value; }
    }

    public void Init()
    {
 
    }

}
