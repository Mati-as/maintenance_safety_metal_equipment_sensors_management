using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DEV_DeactivateOnBuild : MonoBehaviour
{
    
#if UNITY_EDITOR
    
#else
    void Awake()
    {
        gameObject.SetActive(false);
    }
    

#endif
    
}
