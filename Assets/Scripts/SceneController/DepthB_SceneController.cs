using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthB_SceneController : Base_SceneController
{

    private void InitializeC2States()
    {
        if (_sceneStates == null)
        {

        }

        _sceneStates = new Dictionary<int, ISceneState>
        {
            { 2111, new DepthC21_State_1(this) },
            { 2112, new DepthC21_State_2(this) },
            { 2113, new DepthC21_State_3(this) },
            { 2114, new DepthC21_State_4(this) },
            { 2115, new DepthC21_State_5(this) },
            { 2116, new DepthC21_State_6(this) },
            { 2117, new DepthC21_State_7(this) },
            { 2118, new DepthC21_State_8(this) },
            { 2119, new DepthC21_State_9(this) },
            { 21110, new DepthC21_State_10(this) },
            { 21111, new DepthC21_State_11(this) },
            { 21112, new DepthC21_State_12(this) },
            { 21113, new DepthC21_State_13(this) },
            { 21114, new DepthC21_State_14(this) },
            { 21115, new DepthC21_State_15(this) },
            { 21116, new DepthC21_State_16(this) },
            { 21117, new DepthC21_State_17(this) },
            
            { 2121, new DepthC21_State_1(this) },
            { 2122, new DepthC21_State_2(this) },
            { 2123, new DepthC21_State_3(this) },
            { 2124, new DepthC21_State_4(this) },
            { 2125, new DepthC21_State_5(this) },
            { 2126, new DepthC21_State_6(this) },
            { 2127, new DepthC21_State_7(this) },
            { 2128, new DepthC21_State_8(this) },
            { 2129, new DepthC21_State_9(this) },
            { 21210, new DepthC21_State_10(this) },
            { 21211, new DepthC21_State_11(this) },
            { 21212, new DepthC21_State_12(this) },
            { 21213, new DepthC21_State_13(this) },
            { 21214, new DepthC21_State_14(this) },
            { 21215, new DepthC21_State_15(this) },
            { 21216, new DepthC21_State_16(this) },
            { 21217, new DepthC21_State_17(this) },
            
            { 2211, new DepthC21_State_1(this) },
            { 2212, new DepthC21_State_2(this) },
            { 2213, new DepthC21_State_3(this) },
            { 2214, new DepthC21_State_4(this) },
            { 2215, new DepthC21_State_5(this) },
            { 2216, new DepthC21_State_6(this) },
            { 2217, new DepthC21_State_7(this) },
            { 2218, new DepthC21_State_8(this) },
            { 2219, new DepthC21_State_9(this) },
            { 22110, new DepthC21_State_10(this) },
            { 22111, new DepthC21_State_11(this) },
            { 22112, new DepthC21_State_12(this) },
            { 22113, new DepthC21_State_13(this) },
            { 22114, new DepthC21_State_14(this) },
            { 22115, new DepthC21_State_15(this) },
            { 22116, new DepthC21_State_16(this) },
            { 22117, new DepthC21_State_17(this) },
            
            { 2221, new DepthC21_State_1(this) },
            { 2222, new DepthC21_State_2(this) },
            { 2223, new DepthC21_State_3(this) },
            { 2224, new DepthC21_State_4(this) },
            { 2225, new DepthC21_State_5(this) },
            { 2226, new DepthC21_State_6(this) },
            { 2227, new DepthC21_State_7(this) },
            { 2228, new DepthC21_State_8(this) },
            { 2229, new DepthC21_State_9(this) },
            { 22210, new DepthC21_State_10(this) },
            { 22211, new DepthC21_State_11(this) },
            { 22212, new DepthC21_State_12(this) },
            { 22213, new DepthC21_State_13(this) },
            { 22214, new DepthC21_State_14(this) },
            { 22215, new DepthC21_State_15(this) },
            { 22216, new DepthC21_State_16(this) },
            { 22217, new DepthC21_State_17(this) },

        };
    }
}
