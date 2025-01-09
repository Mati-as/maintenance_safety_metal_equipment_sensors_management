using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvaluationManager : MonoBehaviour
{

#region 컴파일타임 점수 반영 로직 관련
    // 뎁스별 평가하기에서 평가할 갯수를 저장합니다
    public readonly Dictionary<int, int> ITEM_COUNTS_TO_EVAL_MAP = new Dictionary<int, int>()
    {
        { 1, 10 },
        { 2, 10 },
        { 3, 10 },
        { 4, 10 },
        { 5, 10 },
    };
    
    //(참조) UI_Evalutation에서 평가항목 점수 표기 시 사용
    public readonly Dictionary<int, int> SCORE_PER_ITEM_MAP = new Dictionary<int, int>()
    {
        // 인덱스 참조를 위한 - 1
        { 421, 10 },
        { 422, 20 },
        { 423, 20 },
        { 424, 10 },
        { 425, 20 },
        { 426, 10 },
        { 427, 10 },
        { 428, 1 },
        { 429, 1 },
        { 4210, 1 },
    };
#endregion

    #region 런타임 점수 반영 로직 관련
    
    //동적으로 저장하며, 서버에 점수를 보낼때 아래 딕셔너리를 활요합니다. 
    [FormerlySerializedAs("_scorePerDepthMap")] public Dictionary<int, int> scorePerDepthMap = new Dictionary<int, int>()
    {
        { 41, 0 },
        { 42, 0 }, //온도센서
        { 43, 0 },
        { 44, 0 },
        { 45, 0 },
    };
    
    
    //(참조) UI_Evalutation에서 평가항목 정,오답여부 로직  시 사용
    public Dictionary<int, bool> isCorrectMap = new Dictionary<int, bool>()
    {
        { 420, false },
        { 421, false },
        { 422, false },
        { 423, false },
        { 424, false },
        { 425, false },
        { 426, false },
        { 427, false },
        { 428, false },
        { 429, false },
        { 4210, false },
        { 4211, false },
        { 4212, false },
    };
    
    public void IsCorrectMapInit() =>  isCorrectMap = new Dictionary<int, bool>()
    {
        { 420, false },
        { 421, false },
        { 422, false },
        { 423, false },
        { 424, false },
        { 425, false },
        { 426, false },
        { 427, false },
        { 428, false },
        { 429, false },
        { 4210, false },
        { 4211, false },
        { 4212, false },
    };
    
    #endregion


    public int currentDepthScore; // 저장하기 단계가아닌, 평가중의 실시간 점수
    public int currentItemsToEvaluate { get; private set; }
    private bool _isScoringState = true;
    public bool isCurrentClickAnswer;

    public bool isScoringState
    {
        get
        {
            return _isScoringState;
        }
        set
        {
            _isScoringState = value;
            if (!_isScoringState)
            {
                Logger.Log("evaluation logic currently can't be executed.... return");
            }
            else
            {
                Logger.Log("evaluation logic is Possable");
            }
        }
    }


    public List<int> objAnswerToClick =  new List<int>();
    public List<int> UIanswerToClick = new List<int>();
    
    
    // 기본적으로 true 상태이고, 올바르지않은 행동을 한 경우에, false로 바꿈
    private bool _isUserCorrectedAnswer = true;
    public bool isUserCorrectedAnswer
    {
        get { return _isUserCorrectedAnswer;}
        set
        {
            if(!value) Logger.Log("user took wrong answer.");
            _isUserCorrectedAnswer = value;

        }
    }
    
    
    
    private bool _isAlreadyWrongAnswerChecked;
    public bool isAlreadyWrongAnswerChecked
    {
        get { return _isAlreadyWrongAnswerChecked;}
        set
        {
            _isAlreadyWrongAnswerChecked = value;
          //  if(!value) Logger.Log("duplicate check logic is false --------");
        }
    }

    
    
    private int _correctAnswersCount;
    public int correctAnswersCount
    {
        get { return _correctAnswersCount; }
        set
        {
            _correctAnswersCount = value;
            
            if (_correctAnswersCount < 0)
            {
                Logger.LogError("Error : correctAnswerCount Can't be minus. ");
            }
            
        }
    }

    private float _evalScore;
    
    

    public float evalScore
    {
        get => _evalScore;

        set { Debug.Assert(currentItemsToEvaluate > 0); }

    }

    public int GetEvalScore(int depthNum, int userID = -123)
    {
        return scorePerDepthMap[depthNum];
    }

    public void AddEvalScore(int depthNum, int score, int userID = -123)
    {
        Logger.Log($"{score} is added in depth : {depthNum}");
         currentDepthScore += score;
        
    }

    public  void SaveResult(int depthNum, int score, int userID = -123)
    {
        Logger.Log($"{score} is saved in depth : {depthNum}");
         scorePerDepthMap[depthNum] = currentDepthScore;
    }


    public void OnEvalStart()
    {
        currentDepthScore = 0;
    }
    public void ResetScore()
    {
        scorePerDepthMap = new Dictionary<int, int>()
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clickedObj"></param>
    public bool CheckIfAnswerIsCorrect(DepthC2_GameObj clickedObj)
    {
        if (!isScoringState) return false;
        if (isAlreadyWrongAnswerChecked)
        {
            Logger.Log("The Answer is Already Checked.. User already got wrong answer. continue without getting score ");
        }

        
        bool isAnswerObject = false;
        foreach(var clikcableObj in objAnswerToClick)
        {
            Logger.Log($"answer : {(DepthC2_GameObj)clikcableObj} : Current Clicked Obj {clickedObj}" );
            if (clikcableObj == (int)clickedObj)
            {
                isAnswerObject = true;
                break; 
            }
        }
        
        if (isAnswerObject)
        {
            Logger.Log("correct object, not scoring now---------------");
            return true;
        }
        else
        {
            Logger.Log("wrong answer -------");
            isAlreadyWrongAnswerChecked = true;
            Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/WrongAnswer");
            return false;
        }
    }

    public bool CheckIfAnswerIsCorrect(UI_ToolBox.Btns clickedUI)
    {
        if (!isScoringState) return false;
        if(isAlreadyWrongAnswerChecked) Logger.Log("The Answer is Already Checked.. User already got wrong answer. continue without getting score ");
     
        
        
        //check if it's clikable answer object -----------------------
        bool isAnswerObject = false;
        
        foreach(var clikcableUI in UIanswerToClick)
        { 
            Logger.Log($"answer : {clikcableUI} : Current Clicked UI {clickedUI}" );
            if (clikcableUI == (int)clickedUI)
            {
                isAnswerObject = true;
                break; 
            }
        }
        
        //react -----------------------
        if (isAnswerObject)
        {
          Logger.Log("correct object, not scoring now---------------");
          return true;
        }
        else
        {
            Logger.Log("wrong answer -------");
            isAlreadyWrongAnswerChecked = true;
            Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/WrongAnswer");
            OnWrongAnswer();
            return false;
        }
        
    }

    private void OnCorrectAnswer()
    {
        correctAnswersCount++;
        
        Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            
        string currentDepth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        string currentDepth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
      
        
        //(int)(((float)correctAnswersCount / (float)itemCountsToEvaluate[currentDepth1]) * 100f
        Logger.Log($"Current Depth: {currentDepth1 + currentDepth2} ,Current Score is : {scorePerDepthMap[int.Parse(currentDepth1+currentDepth2)]}");
     
        
     
    }

    private void OnWrongAnswer()
    {
     //UI_Eval의 WrongUI 표출 여기서 하기 --------------------11/15/24   
    }

    public void SaveIsCorrectStatusPerItems(int currentItemIndex, bool isAlreadyWrongAnser)
    {   
        string currentDepth1 = Managers.ContentInfo.PlayData.Depth1.ToString();
        string currentDepth2 = Managers.ContentInfo.PlayData.Depth2.ToString();
        isCorrectMap[int.Parse(currentDepth1 + currentDepth2 +currentItemIndex)] = !isAlreadyWrongAnser;

        if (!isAlreadyWrongAnser)
        {
            AddEvalScore(int.Parse(currentDepth1+currentDepth2), SCORE_PER_ITEM_MAP[int.Parse(currentDepth1+currentDepth2+currentItemIndex)]);
        }
    }

    public void EvalmodeOnStateExit()
    {
        objAnswerToClick = new List<int>();
        UIanswerToClick = new List<int>();

        if (Managers.ContentInfo.PlayData.Count <= 2)
        {
            Logger.Log("Current Count is less then 1...the evaluation logic can't be executed.");
            return;
        }

        // precheck---------------------------------   
        if (!isScoringState)
        {
            Logger.Log("it isn't scroing state---------------------.... return");

            return;
        }


        // Evaluation---------------------------------   
        if (!isAlreadyWrongAnswerChecked && isUserCorrectedAnswer)
            OnCorrectAnswer();
        else
            Logger.Log($"user didn't get score in this state {Managers.ContentInfo.PlayData.CurrentDepthStatus}");
    }

    public void InitPerState()
    {
        // Depth4평가하기에서만 사용하므로 기본적으로는 점수를 주는 State라고 가정합니다.
        
        //중요 : 평가로직 분할 여부 나뉘는 핵심 부분
        //상황: State를 두개이상 사용하여 한 항목(단위)를 평가하는경우를 가정
        //평가하는 State가 아닌데, 이미 정답이 틀렸다면, 다음항목이 맞더라도 틀린것으로 간주하기위한 로직입니다. 
        if(!isScoringState && isAlreadyWrongAnswerChecked) isAlreadyWrongAnswerChecked = true;
        else
        {
            isAlreadyWrongAnswerChecked = false;
        }
        
        
        isScoringState = true;
        isUserCorrectedAnswer = true;
       
    }


    
}

