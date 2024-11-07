using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvaluationManager : MonoBehaviour
{

    // 뎁스별 평가하기에서 평가할 갯수를 저장합니다
    public readonly Dictionary<int, int> itemCountsToEvaluate = new Dictionary<int, int>()
    {
        { 1, 10 },
        { 2, 10 },
        { 3, 10 },
        { 4, 10 },
        { 5, 10 },
    };

    //동적으로 저장하며, 서버에 점수를 보낼때 아래 딕셔너리를 활요합니다. 
    public Dictionary<int, int> scorePerDepthMap = new Dictionary<int, int>()
    {
        { 1, 0 },
        { 2, 0 },
        { 3, 0 },
        { 4, 0 },
        { 5, 0 },
    };

    public int currentItemsToEvaluate { get; private set; }
    private bool _isScoringState = true;

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


    [FormerlySerializedAs("answerObjectToClick")] public List<int> objAnswerToClick =  new List<int>();
    [FormerlySerializedAs("answerUIToClick")] public List<int> UIanswerToClick = new List<int>();
    
    
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

    public int SaveEvalScore(int depthNum, int score, int userID = -123)
    {
        return scorePerDepthMap[depthNum] = score;
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
    public void CheckIfAnswerIsCorrect(DepthC_GameObj clickedObj)
    {
        if (!isScoringState) return;
        if(isAlreadyWrongAnswerChecked) Logger.Log("The Answer is Already Checked.. User already got wrong answer. continue without getting score ");

        
        bool isAnswerObject = false;
        
        foreach(var clikcableObj in objAnswerToClick)
        {
            Logger.Log($"answer : {(DepthC_GameObj)clikcableObj} : Current Clicked Obj {clickedObj}" );
            if (clikcableObj == (int)clickedObj)
            {
                isAnswerObject = true;
                break; 
            }
        }
        
        
        if (isAnswerObject)
        {
            Logger.Log("correct object, not scoring now---------------");
        }
        else
        {
            Logger.Log("wrong answer -------");
            isAlreadyWrongAnswerChecked = true;
            Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/WrongAnswer");
        }
    }

    public void CheckIfAnswerIsCorrect(UI_ToolBox.Btns clickedUI)
    {
        if (!isScoringState) return;
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
        }
        else
        {
            Logger.Log("wrong answer -------");
            isAlreadyWrongAnswerChecked = true;
            Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/WrongAnswer");
        }
        
    }

    private void OnCorrectAnswer()
    {
        correctAnswersCount++;
        
        Managers.Sound.Play(SoundManager.Sound.Effect, "Etc/Correct");
            
        var currentDepth1 = Managers.ContentInfo.PlayData.Depth2;
        SaveEvalScore(currentDepth1,
            (int)(((float)correctAnswersCount / (float)itemCountsToEvaluate[currentDepth1]) * 100f));
            
        Logger.Log($"Current Depth: {currentDepth1} ,Current Score is : {scorePerDepthMap[currentDepth1]}");
    }

    public void OnStateExit()
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
        if (isUserCorrectedAnswer)
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

