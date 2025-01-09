
using IMR;
using Newtonsoft.Json.Linq;

public class LoginStatement : IMRStatement
{
    public LoginStatement() => Init();
    
    public sealed override void Init()
    {
        UnityEngine.Debug.Log("LoginStatement INIT");
        base.Init();
        SetActor();
        SetVerb("logged-in");
        SetActivity(Lesson.COURSE_NAME + "/login");

        var extensions = new JObject();
        var content = new JObject();
        content["content"] = Lesson.COURSE_NAME;
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, content);
        SetContextExtensions(extensions);
    }

    public void _SetContextExtensionUserID(string userID)
    {
        var extensions = new JObject();
        var ID = new JObject();
        ID["ID"] = userID;
        extensions.Add(Lesson.EXTENSION_CONTEXT_URL, ID);
        SetContextExtensions(extensions);
    }

}
