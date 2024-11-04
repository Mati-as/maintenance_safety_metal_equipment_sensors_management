using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvaluationManager : MonoBehaviour
{
    
    // 뎁스별 평가하기에서 평가할 갯수를 저장합니다
     public readonly Dictionary<int,int> itemCountsToEvaluate = new Dictionary<int, int>()
    {
        {1,10},
        {2,10},
        {3,10},
        {4,10},
        {5,10},
    }; 

     //동적으로 저장하며, 서버에 점수를 보낼때 아래 딕셔너리를 활요합니다. 
     public Dictionary<int,int> scorePerDepthMap = new Dictionary<int, int>()
     {
         {1,0},
         {2,0},
         {3,0},
         {4,0},
         {5,0},
     }; 

     public int currentItemsToEvaluate
     {
         get;
         private set;
     }

     public int correctCount;
     
     private float _evalScore;

     public float evalScore
     {
         get => _evalScore;

         set => Debug.Assert(currentItemsToEvaluate > 0);
     }

     public int GetEvalScore(int depthNum,int userID = -123)
     {
         return scorePerDepthMap[depthNum];
     }
     
     public int SetEvalScore(int depthNum,int score,int userID = -123)
     {
         return scorePerDepthMap[depthNum] = score;
     }

     public void ResetScore()
     {
         scorePerDepthMap = new Dictionary<int, int>()
         {
             {1,0},
             {2,0},
             {3,0},
             {4,0},
             {5,0},
         }; 
     }
}
