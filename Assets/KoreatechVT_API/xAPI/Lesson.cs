using System.Collections;
using System.Collections.Generic;
using IMR;


public abstract class Lesson
{
    static public string COURSE_NAME = "2022_과정명"; // "해당 개발년도_해당 콘텐츠명"으로 수정
    static public string COURSE_NAME_ENG = "2022_과정명(영문약식)"; // "해당 개발년도_해당 콘텐츠명의 영문 약식 표현"으로 수정
    static public string DEV_YEAR = "2022"; // "해당 개발년도"로 수정
    static public string BASE_URL = "https://www.koreatech.ac.kr";
    static public string EXTENSION_CONTEXT_URL = BASE_URL + "/extension/context";

    public Dictionary<string, IMRStatement> statement_dictionary;

    protected string lessonName;
    protected string lessonNameEng;
    protected double _score;
    protected bool _completion;

    public abstract void Init();
    public abstract void ChangeNewStatement(string name);
    public virtual void UpdateResultStatement(List<Dictionary<string, string>> results, int score, bool complete) { }
    public virtual void UpdateChoice(string item, string step, bool success) { }

    public virtual IMRStatement GetIMRStatement(string name)
    {
        if (statement_dictionary.TryGetValue(name, out IMRStatement statement))
        {
            return statement;
        }
        else
        {
            return new IMRStatement();
        }
    }

}