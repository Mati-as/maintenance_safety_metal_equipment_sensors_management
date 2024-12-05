
using IMR;
using Newtonsoft.Json.Linq;

public class ChoiceStatement : IMRStatement
{
    public ChoiceStatement() => Init();

    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("verbs/performed");
        SetActivity("selection"); // -> Lesson Manager에서 재설정됨
        var extensions = new JObject();
        var content = new JObject();
        content["content"] = Lesson.COURSE_NAME;
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, content);
        SetContextExtensions(extensions);
    }

    public void SetSuccess(bool b)
    {
        _result.success = b;
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
}