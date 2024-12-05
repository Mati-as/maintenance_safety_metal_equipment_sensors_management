using System;
using System.Collections;
using System.Collections.Generic;
using IMR;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json.Linq;

public class XAPIApplication : MonoBehaviour
{

    public static XAPIApplication current;
    public bool terminatied = false;

    private string _endpoint = "http://www.vt-lrs.com/data/xAPI";
    private string _key = "812073176a6f25e4ddd1cb6c80fadac19e05221e";
    private string _secret_key = "2307f2b781f07c4aaaaede4ef5ee6cfce306f2f8";
    public string debug_msg = "";
    
    private RemoteLRS lrs;
    public StatementLRSResponse lrs_res = null;
    
    public string actor_name = "korea_tech";
    public string actor_mbox = "mailto:korea_tech@google.com";
    public string tempUserName = "null";

    public LoginLessonManager loginLesson;

    /* 평가 수만큼 LessonManager 선언
    public CourseLessonManager firstLesson;
    public CourseLessonManager secondLesson;
    public CourseLessonManager thirdLesson;
    */

    // 평가명 입력
    public string[] lessonName = new string[]
    {
        "평가명1",
        "평가명2",
        "평가명3"
    };
    // 평가코드(평가명 영문 약식 표현) 입력
    public string[] lessonNameEng = new string[]
    {
        "평가명(영문약식)1",
        "평가명(영문약식)2",
        "평가명(영문약식)3"
    };

    public Lesson activeLesson; // 현재 평가명
    public int actvieLessonIndex; // 현재 평가의 인덱스

    public HttpSender httpSender;
    public DateTime startTime;
    public DateTime lessonStartTime;

    public string ActorName
    {
        get => actor_name;
        set => actor_name = value;
    }
    public string ActorMboxName
    {
        get => actor_mbox;
        set => actor_mbox = value;
    }

    public string EndPoint
    {
        get => _endpoint;
        set => _endpoint = value;
    }
    public string UserName
    {
        get => _key;
        set => _key = value;
    }
    public string Password
    {
        get => _secret_key;
        set => _secret_key = value;
    }

    public void SetRemoteLRS()
    {
        lrs = new RemoteLRS(_endpoint, _key, _secret_key);
    }
    public void SetRemoteLRS(string s)
    {
        lrs = new RemoteLRS(s, _key, _secret_key);
    }

    private void Awake()
    {
    }

    public void Init()
    {
        SetRemoteLRS();
        
        loginLesson = new LoginLessonManager();
    }
    
    public void LessonManagerInit(string chapterName)
    {
        /* // 평가 수에 맞게 편집
        firstLesson = new CourseLessonManager(lessonName[0], lessonNameEng[0]);
        secondLesson = new CourseLessonManager(lessonName[1], lessonNameEng[1]);
        thirdLesson = new CourseLessonManager(lessonName[2], lessonNameEng[2]);
        */

        SelectActiveLesson(chapterName);
    }

    public void SendInitStatement(string chapterName)
    {
        if (!SelectActiveLesson(chapterName))
        {
            return;
        }
        terminatied = false;
        SendIMRStatement(chapterName, "Init");
    }

    public void SendTerminateStatement(string chapterName, List<Dictionary<string, string>> results, int score, bool complete)
    {
        if (!SelectActiveLesson(chapterName))
        {
            return;
        }
        activeLesson.UpdateResultStatement(results, score, complete);
        activeLesson.ChangeNewStatement("Terminate");
        SendIMRStatement(chapterName, "Terminate");
        terminatied = true;
    }

    public void SendChoiceStatement(string chapterName, string item, string step, bool success)
    {
        if (!SelectActiveLesson(chapterName))
        {
            return;
        }

        activeLesson.UpdateChoice(item, step, success);
        SendIMRStatement(chapterName, "Choice");
    }
    
    public bool SelectActiveLesson(string chapterName)
    {
        switch (chapterName)
        {
            /* // 평가 수에 맞게 편집
            case "0": activeLesson = firstLesson; actvieLessonIndex = 0; break;
            case "1": activeLesson = secondLesson; actvieLessonIndex = 1; break;
            case "2": activeLesson = thirdLesson; actvieLessonIndex = 2; break;
            */
            default:
                Debug.Log("XAPIApplication unknown chapter - " + chapterName);
                return false;
        }
        return true;
    }
    
    public bool SendIMRStatement(string chapterName, string name)
    {
        if (!SelectActiveLesson(chapterName))
        {
            Debug.Log("Save statement failed: invalid chapter name. " + chapterName);
            return false;
        }

        var imr_statement = activeLesson.GetIMRStatement(name);
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
        if (lrs_res.success) //Success
        {
            activeLesson.ChangeNewStatement(name);
            Debug.Log("Save statement2: " + lrs_res.content.id);
            return true;
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
            return false;
        }
    }

    public bool SendLoginStatement(string userID)
    {
        Init();

        string[] splitText = userID.Split('@');
        ActorName = splitText[0];
        ActorMboxName = userID;

        loginLesson.SetContextExtensionUserID(ActorName);

        LessonManagerInit("Unknown");

        startTime = DateTime.Now;
        return true;
    }

    private JObject JSONBody()
    {
        // 데이터 전송을 위한 일부 정보를 런처에서 받은 파라미터에서 확인
        string[] arguments = LoginNetworkManager.S.args;

        JObject fullBody = new JObject();
        JObject contentsBody = new JObject();
        JObject contentsPart = new JObject();
        JObject reqHeaderBody = new JObject();
        JObject reqHeaderPart = new JObject();

        JProperty id = new JProperty("student_id", XAPIApplication.current.ActorMboxName);
        JProperty st_name = new JProperty("student_name", XAPIApplication.current.ActorName);
        JProperty course_id = new JProperty("course_id", arguments[arguments.Length - 2]);
        JProperty enrollment_count = new JProperty("enrollment_count", arguments[arguments.Length - 1]);
  
        JProperty contents_code = new JProperty("contents_code", Lesson.COURSE_NAME_ENG);
        JProperty cont_name = new JProperty("contents_name", Lesson.COURSE_NAME);
        JProperty cont_devyear = new JProperty("contents_dev_year", Lesson.DEV_YEAR);
        JProperty lesson_code;
        JProperty lesson_name;

        //평가코드 및 평가명 설정 부분
        lesson_code = new JProperty("lession_code", lessonNameEng[actvieLessonIndex]);
        lesson_name = new JProperty("lession_name", lessonName[actvieLessonIndex]);

        contentsPart = new JObject
            (
                id,
                st_name,
                course_id,
                enrollment_count,
                contents_code,
                cont_name,
                cont_devyear,
                lesson_code,
                lesson_name
            );

        reqHeaderPart = GetRequestHeader(arguments[arguments.Length - 3]);

        fullBody.Add("reqHeader", reqHeaderPart);
        fullBody.Add("contents", contentsPart);

        return fullBody;
    }

    public void SendChoiceVT(string item, bool success, int step)
    {
        JObject resultObject = JSONBody();

        JProperty lesson_item_name;
        JProperty lesson_item_code;
        JProperty time_start = new JProperty("time_start", XAPIApplication.current.startTime.ToString("s"));
        JProperty time_end = new JProperty("time_end", DateTime.Now.ToString("s"));
        JProperty lesson_completion = new JProperty("lession_item_completion", success ? "success" : "fail");
        JProperty lesson_score = new JProperty("lession_item_score", 0); // 개별문항 배점은 항상 0으로 통일
        JProperty learning_time = new JProperty("learning_time", (int)Math.Round((DateTime.Now - XAPIApplication.current.startTime).TotalSeconds));

        lesson_item_code = new JProperty("lession_item_code", item + "-step" + step.ToString());
        lesson_item_name = new JProperty("lession_item_name", "진행 > " + item);

        var node = resultObject.SelectToken("contents") as JObject;
        node.Add(lesson_item_code);
        node.Add(lesson_item_name);
        node.Add(time_start);
        node.Add(time_end);
        node.Add(lesson_completion);
        node.Add(lesson_score);
        node.Add(learning_time);

        Debug.Log(resultObject);

        XAPIApplication.current.httpSender.Send(resultObject.ToString());
        XAPIApplication.current.startTime = DateTime.Now;
    }

    public void SendTerminateVT(bool success, int score)
    {
        JObject resultObject = JSONBody();

        JProperty lesson_item_name;
        JProperty lesson_item_code;
        JProperty time_start = new JProperty("time_start", XAPIApplication.current.startTime.ToString("s"));
        JProperty time_end = new JProperty("time_end", DateTime.Now.ToString("s"));
        JProperty lesson_completion = new JProperty("lession_item_completion", success ? "success" : "fail");
        JProperty lesson_score = new JProperty("lession_item_score", score);
        JProperty learning_time = new JProperty("learning_time", (int)Math.Round((DateTime.Now - XAPIApplication.current.startTime).TotalSeconds));

        lesson_item_code = new JProperty("lession_item_code", "EvaluationResult");
        lesson_item_name = new JProperty("lession_item_name", "평가결과");

        var node = resultObject.SelectToken("contents") as JObject;
        node.Add(lesson_item_code);
        node.Add(lesson_item_name);
        node.Add(time_start);
        node.Add(time_end);
        node.Add(lesson_completion);
        node.Add(lesson_score);
        node.Add(learning_time);

        Debug.Log(resultObject);

        XAPIApplication.current.httpSender.Send(resultObject.ToString());
        XAPIApplication.current.startTime = DateTime.Now;
    }

    static string AuthKey = "5CEQh6NR6qXg2d7wzR6qXg5CEQh6Narp";

    #region [ GetRequestHeader - 서버 전문 해더 생성 ]
    private static JObject GetRequestHeader(string urid)
    {
        JObject jsonHeader = new JObject();
        string userId = string.IsNullOrEmpty(AuthKey) ? urid : AESEncrypt256(urid, AuthKey);
        string siteName = "stepvt";
        string requestTime = String.Format("{0:yyyyMMdd HHmmss}", DateTime.Now);
        jsonHeader.Add("userId", userId);
        jsonHeader.Add("siteName", siteName);
        jsonHeader.Add("requestTime", requestTime);
        jsonHeader.Add("userAuth", SHAEncrypt256(userId + siteName + requestTime));
        return jsonHeader;
    }
    #endregion

    static string AES256_iv = "00000000000000000000000000000000 ";

    #region [ AES_256 ]
    public static string AESEncrypt256(string Input, string key)
    {
        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = ConvertHexStringToByte(AES256_iv);
        aes.Key = System.Text.Encoding.UTF8.GetBytes(key);

        var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] xBuff = null;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Encoding.UTF8.GetBytes(Input);
                cs.Write(xXml, 0, xXml.Length);
            }

            xBuff = ms.ToArray();
        }

        string Output = Convert.ToBase64String(xBuff);
        return Output;
    }
    public static String AESDecrypt256(string Input, string key)
    {
        if (key.Length > 32) key = key.Substring(0, 32);

        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = ConvertHexStringToByte(AES256_iv);
        aes.Key = Encoding.UTF8.GetBytes(key);

        var decrypt = aes.CreateDecryptor();
        byte[] xBuff = null;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Convert.FromBase64String(Input);
                cs.Write(xXml, 0, xXml.Length);
            }

            xBuff = ms.ToArray();
        }

        String Output = Encoding.UTF8.GetString(xBuff);
        return Output;
    }
    #endregion

    #region [ SHA_256 ]
    public static string SHAEncrypt256(string rawPass)
    {
        UTF8Encoding encoder = new UTF8Encoding();
        SHA256Managed sha256hasher = new SHA256Managed();

        byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(rawPass));
        string hashedPwd = byteArrayToString(hashedDataBytes);

        return hashedPwd;
    }
    private static string byteArrayToString(byte[] inputArray)
    {
        StringBuilder output = new StringBuilder("");
        for (int i = 0; i < inputArray.Length; i++)
        {
            output.Append(inputArray[i].ToString("X2").ToLower());
        }
        return output.ToString();
    }
    #endregion

    #region [ ConvertHexStringToByte ]
    public static byte[] ConvertHexStringToByte(string convertString)
    {
        byte[] convertArr = new byte[convertString.Length / 2];

        for (int i = 0; i < convertArr.Length; i++)
        {
            convertArr[i] = Convert.ToByte(convertString.Substring(i * 2, 2), 16);
        }
        return convertArr;
    }
    #endregion
}
