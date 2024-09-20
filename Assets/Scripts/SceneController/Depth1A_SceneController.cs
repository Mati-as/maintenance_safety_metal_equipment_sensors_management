using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamearAnimation
{
    Intro,
}

public enum GameObjectAnimation
{
    Intro,
    IntroToSensorZoomIn,
    
}
public class Depth1A_SceneController : Base_SceneController
{

    public override void OnStepEnter(int currentDepth)
    {
        base.OnStepEnter(currentDepth);
    }

    public override void OnStep(int currentDepth)
    {
        base.OnStep(currentDepth);
    }

    public override void OnStepExit(int currentDepth)
    {
        base.OnStepExit(currentDepth);
    }


}
