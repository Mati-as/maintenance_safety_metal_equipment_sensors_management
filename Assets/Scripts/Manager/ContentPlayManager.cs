using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;

/// <summary>
///     // 1.current info tracking
///     2.Control ContentController
/// </summary>
public class ContentPlayData : MonoBehaviour
{
    public static Dictionary<int, int> DEPTH_TWO_COUNT_DATA = new Dictionary<int, int>
    {
        { 1, 2 }, //depth1 +depth2 , Depth3갯수
        { 2, 3 },
        { 3, 5 }, 
        { 4, 5 }, 
   
    };
    public static Dictionary<int, int> DEPTH_THREE_COUNT_DATA = new Dictionary<int, int>
    {
        { 11, 1 }, 
        { 12, 5 },
        
        { 21, 1 },
        { 22, 1 },
        { 23, 2 },
        
        { 31, 3 },
        { 32, 3 },
        { 33, 3 },
        { 34, 3 },
        { 35, 3 },
        
        { 41, 1 },
        { 42, 1 },
        { 43, 1 },
        { 44, 1 },
        { 45, 1 }
    };

    public enum CurrentDepthData
    {
        Depth1, //UI- approachable
        Depth2, //UI- approachable
        Depth3, //Non UI- approachable
        Count, //Non UI- approachable
        //Depth1 + 2 + 3 + Count = Narration, Text 순서 및 File Data,
        Max
    }

    

    public string CurrentDepthStatus ="00000";
    private char[] _currentDepthStatusChar = new char[(int)CurrentDepthData.Max];


    private int _depth1 =0;
    private int _depth2 =0;
    private int _depth3 =0;
    private int _count = 0;
    
    public static readonly int DEPTH1_MAX_COUNT = 5; // 개요,안전관리,정비실습,평가하기,튜토리얼 순
    
    public static readonly int COUNT_MAX_DEPTH111 = 25;
    public static readonly int COUNT_MAX_DEPTH112 = 25;
    
    // 최대 State 및 스크립트 갯수 + 1 (마지막 상태에서 새로운 뎁스 로드 및 초기화) 
    
    public static readonly int COUNT_MAX_DEPTH211 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH212 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH213 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH221 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH222 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH223 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH231 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH232 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH233 = 3 + 1;

    public static readonly int COUNT_MAX_DEPTH311 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH312 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH313 = 3 + 1;

    public static readonly int COUNT_MAX_DEPTH321 = 17 + 1;
    public static readonly int COUNT_MAX_DEPTH322 = 11 + 1;
    public static readonly int COUNT_MAX_DEPTH323 = 14 + 1;
       
    public static readonly int COUNT_MAX_DEPTH331 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH332 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH333 = 3 + 1;

    public static readonly int COUNT_MAX_DEPTH341 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH342 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH343 = 3 + 1;

    public static readonly int COUNT_MAX_DEPTH351 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH352 = 3 + 1;
    public static readonly int COUNT_MAX_DEPTH353 = 3 + 1;
   
    public static readonly int COUNT_MAX_DEPTH401 = 12 + 1;
    public static readonly int COUNT_MAX_DEPTH402 = 12 + 1;
    public static readonly int COUNT_MAX_DEPTH403 = 12 + 1;
    public static readonly int COUNT_MAX_DEPTH404 = 12 + 1;
    public static readonly int COUNT_MAX_DEPTH405 = 12 + 1;
    
    public static readonly int COUNT_MAX_DEPTH501 = 8 + 1;
    
    private static readonly Dictionary<string, int> DepthCountMaxDictionary = new Dictionary<string, int>
    {
        { "111", COUNT_MAX_DEPTH111},
        { "121", COUNT_MAX_DEPTH112},
        
        /////////////////////////////////////
        { "211", COUNT_MAX_DEPTH211 },
        { "212", COUNT_MAX_DEPTH212 },
        { "213", COUNT_MAX_DEPTH213 },
        
        { "221", COUNT_MAX_DEPTH221 },
        { "222", COUNT_MAX_DEPTH222 },
        { "223", COUNT_MAX_DEPTH223 },
        
        { "231", COUNT_MAX_DEPTH231 },
        { "232", COUNT_MAX_DEPTH232 },
        { "233", COUNT_MAX_DEPTH233 },
        
        /////////////////////////////////////
        { "311", COUNT_MAX_DEPTH311 },
        { "312", COUNT_MAX_DEPTH312 },
        { "313", COUNT_MAX_DEPTH313 },
        
        { "321", COUNT_MAX_DEPTH321 },
        { "322", COUNT_MAX_DEPTH322 },
        { "323", COUNT_MAX_DEPTH323 },
        
        { "331", COUNT_MAX_DEPTH331 },
        { "332", COUNT_MAX_DEPTH332 },
        { "333", COUNT_MAX_DEPTH333 },
        
        { "341", COUNT_MAX_DEPTH341 },
        { "342", COUNT_MAX_DEPTH342 },
        { "343", COUNT_MAX_DEPTH343 },
        
        { "351", COUNT_MAX_DEPTH351 },
        { "352", COUNT_MAX_DEPTH352 },
        { "353", COUNT_MAX_DEPTH353 },
    
        /////////////////////////////////////
        { "411", COUNT_MAX_DEPTH401 },
        { "421", COUNT_MAX_DEPTH402 },
        { "431", COUNT_MAX_DEPTH403 },
        { "441", COUNT_MAX_DEPTH404 },
        { "451", COUNT_MAX_DEPTH405 },
        
        { "511", COUNT_MAX_DEPTH501 },

        
        // ... 추가 DEPTH 값들
    };


    public static int CurrentCountMax;
    
    public int Depth1
    {
        get { return _depth1; }
        set
        {
            Debug.Assert(value <= DEPTH1_MAX_COUNT);
            
            
            _depth1 = value;
            
            // Update the relevant character in CurrentDepthStatus
            UpdateCurrentDepthStatus((int)CurrentDepthData.Depth1, (char)(value + '0'));
            
            Logger.Log($"Current Count Max:{CurrentCountMax}Current Scene Info : {CurrentDepthStatus[0]}-{CurrentDepthStatus[1]}-" +
                       $"{CurrentDepthStatus[2]} : {CurrentDepthStatus[3]}{CurrentDepthStatus[4]}" +
                       $"\n textNarrNum: {CurrentDepthStatus}" );
        }
    }

//Depth2
    public int Depth2
    {
        get { return _depth2; }
        set
        {
            Debug.Assert(value <= 5);
            
            _depth2 = value;

            // Update the relevant character in CurrentDepthStatus
            UpdateCurrentDepthStatus((int)CurrentDepthData.Depth2, (char)(value + '0'));
            

            Logger.Log($"Current Scene Info : {CurrentDepthStatus[0]}-{CurrentDepthStatus[1]}-" +
                      $"{CurrentDepthStatus[2]} : {CurrentDepthStatus[3]}{CurrentDepthStatus[4]}" +
                      $"\n textNarrNum: {CurrentDepthStatus}");

         
        }
    }

//Depth3
    public int Depth3
    {
        get { return _depth3; }
        set
        {
            Debug.Assert(value <= 5 && value >0,$"currentdepth 3 {value} ");
            _depth3 = value;

            // Update the relevant character in CurrentDepthStatus
            UpdateCurrentDepthStatus((int)CurrentDepthData.Depth3, (char)(value + '0'));


            Logger.Log($"Current Scene Info : {CurrentDepthStatus[0]}-{CurrentDepthStatus[1]}-" +
                       $"{CurrentDepthStatus[2]} : {CurrentDepthStatus[3]}{CurrentDepthStatus[4]}" +
                       $"\n textNarrNum: {CurrentDepthStatus}");

          
      
            
            string depthKey = $"{Depth1}{Depth2}{Depth3}";
            
            if (DepthCountMaxDictionary.TryGetValue(depthKey, out int countMax))
            {
      
                    CurrentCountMax = countMax;
            }
            else
            {
                Logger.LogError($"Invalid depth combination: {depthKey}");
            }
        }
    }

//Count
    public int Count
    {
        get { return _count; }
        set
        {
            _count = math.clamp(value, 0, CurrentCountMax);//max
            
            // Format the count value
            var formattedValue = value < 10 ? $"0{value}" : value.ToString();
            UpdateCurrentDepthStatus((int)CurrentDepthData.Count, formattedValue[0]);
            UpdateCurrentDepthStatus((int)CurrentDepthData.Count + 1, formattedValue[1]);

#if UNITY_EDITOR
            Debug.Log($"Current Scene Info : {CurrentDepthStatus[0]}-{CurrentDepthStatus[1]}-" +
                      $"{CurrentDepthStatus[2]} : {CurrentDepthStatus[3]}{CurrentDepthStatus[4]}" +
                      $" \n textNarrNum: {CurrentDepthStatus}");
#endif
        }
    }

// Helper method to update CurrentDepthStatus string..
    private void UpdateCurrentDepthStatus(int index, char newValue)
    {
        if (index < CurrentDepthStatus.Length)
        {
            var tempArray = CurrentDepthStatus.ToCharArray();
            tempArray[index] = newValue;
            CurrentDepthStatus = new string(tempArray);
        }
    }
    
    public void ResetOrSetDepthCount(int depth2=1, int depth3=1, int count=1)
    {
        Depth2 = depth2;
        Depth3 = depth3;
        Count = count;
    }
}


public class ContentPlayManager : MonoBehaviour
{
    public ContentPlayData PlayData { get; set; } = new();
    
    public void Init()
    {
        PlayData = new ContentPlayData();
        
        GameObject root = GameObject.Find("@ContentData_Root");
        if (root == null)
            root = new GameObject { name = "@ContentData_Root" };
			
        DontDestroyOnLoad(root);
      
    }
}