
using IMR;
using Newtonsoft.Json.Linq;

public class InitalizeStatement : IMRStatement
{
    public InitalizeStatement() => Init();
    
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("verbs/initialized");
        SetActivity(Lesson.COURSE_NAME + "/");

        var extensions = new JObject();
        var content = new JObject();
        var version = new JObject();
        content["content"] = Lesson.COURSE_NAME;
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, content);
        SetContextExtensions(extensions);
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
