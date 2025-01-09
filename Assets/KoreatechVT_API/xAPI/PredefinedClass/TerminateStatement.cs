using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMR;
using Newtonsoft.Json.Linq;
public class TerminateStatement : IMRStatement
{
    public TerminateStatement() => Init();

    public sealed override void Init()
    {
        base.Init();

        SetActor();
        SetVerb("verbs/terminated");
        SetActivity( Lesson.COURSE_NAME + "/");
        _result.score = new TinCan.Score();
        
        var extensions = new JObject();
        var content = new JObject();
        var version = new JObject();
        content["content"] = Lesson.COURSE_NAME;
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, content);
        SetContextExtensions(extensions);
    }

    public void SetResultExtensionFromResultStatements(List<Dictionary<string, string>> resultStatements, string lessonName)
    {
        JObject resultExtension = new JObject();
        List<JObject> results = new List<JObject>();
        JObject tempObject = new JObject();
        JArray jArray = new JArray();
        
        /* Terminate 시 추가 정보 부분(콘텐츠에 맞게 편집)
        for (int i = 0; i < resultStatements.Count; i++)
        {
            foreach (KeyValuePair<string, string> element in resultStatements[i])
            {
                JObject tempProperty = new JObject();
                JProperty item = new JProperty("evaluation-item", element.Key);
                JProperty score = new JProperty("evaluation-score", element.Value);

                tempProperty = new JObject(item, score);
                results.Add(tempProperty);

            }
            tempObject.Add("result" + (i < 9 ? "0" : "") + (i + 1).ToString(), results[i]);
        }
        */

        resultExtension.Add(Lesson.BASE_URL + "/extension/" + lessonName + "/result", tempObject);
        SetResultExtensions(resultExtension);
    }
    
    public void SetContextExtensionLesson(string lessonName)
    {
        var extensions = new JObject();
        JProperty year = new JProperty("year", Lesson.DEV_YEAR);
        JProperty content = new JProperty("content", Lesson.COURSE_NAME);
        JProperty lesson = new JProperty("lesson", lessonName);

        JObject tempProperty = new JObject(year, content, lesson);
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, tempProperty);
        SetContextExtensions(extensions);
    }

    public void SetScore(double i)
    {
        _result.score.raw = i;
    }

    public void SetSuccess(bool b)
    {
        _result.completion = b;
    }

}
