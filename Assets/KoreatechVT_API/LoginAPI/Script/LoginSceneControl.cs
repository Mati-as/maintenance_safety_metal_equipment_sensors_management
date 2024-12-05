using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoginSceneControl : MonoBehaviour
{
    public ContentsType m_ContentsType;
    public InputField m_Id;
    public InputField m_Pw;
    public Button m_Check;
    public Button m_Close;

    public ServiceType serviceType;
    public static LoginSceneControl current;

    // Start is called before the first frame update
    void Start()
    {
        LoginNetworkManager networkManager = LoginNetworkManager.S;

        // for Editor
        if (m_ContentsType == ContentsType.NONE)
        {
            LoginNetworkManager.S.OnError("컨텐츠 타입을 설정해 주세요.", null);
            return;
        }

        if (serviceType == ServiceType.NONE)
        {
            LoginNetworkManager.S.OnError("서비스 타입을 설정해 주세요.", null);
            return;
        }
        VRContentsKoo2.m_ContentsType = m_ContentsType;

        switch ((int)serviceType)
        {
            case 1: // 로그인버전
                m_Check.onClick.AddListener(RequestLogin);
                m_Close.onClick.AddListener(OnProgramQuit);
                break;
            case 2: // 런처연동버전
                LauncherSetting();
                break;
            case 3: // noAPI버전
                SkipLogin();
                break;
            default:
                Debug.LogError("서비스 타입 오류");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RequestLogin();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_Id.isFocused)
            {
                m_Pw.Select();
            }

            if (m_Pw.isFocused)
            {
                m_Id.Select();
            }
        }

    }

    void OnProgramQuit()
    {
        Application.Quit();
    }

    void RequestLogin()
    {
        if (XAPIApplication.current)
        {
            Debug.Log("XAPI present.");
        }
        else
        {
            Debug.Log("XAPI not present.");
        }

        if (LoginNetworkManager.S.m_IsHttpWebRequesting) { return; }

        if (string.IsNullOrEmpty(m_Id.text))
        {
            LoginNetworkManager.S.OnError("아이디를 입력하세요.");
            return;
        }

        if (string.IsNullOrEmpty(m_Pw.text))
        {
            LoginNetworkManager.S.OnError("비밀번호를 입력하세요.");
            return;
        }

        if (!LoginNetworkManager.S.IsNetworkAvailable(0))
        {
            LoginNetworkManager.S.OnError("네트워크 연결 상태를 확인해 주세요.");
            return;
        }

        LoginNetworkManager.S.ReqAuthenticate(m_Id.text, m_Pw.text, ResAuthenticate);
    }


    void ResAuthenticate(string data)
    {
        Authenticate authenticate = JsonUtility.FromJson<Authenticate>(data);

        if (authenticate.code != "10000")
        {
            LoginNetworkManager.S.OnError("아이디나 비밀번호를 확인해 주세요.");
            return;
        }

        string token = authenticate.body.access_token;
        LoginNetworkManager.S.ReqMemberInfo(token);
        LoginNetworkManager.S.CheckNetworkState();
    }

    void LauncherSetting()
    {
        GameObject.Find("Close").SetActive(false);
        GameObject.Find("Contents").SetActive(false);

        // 런처연동형일 경우 서버측에 콘텐츠 리스트 요청
        LoginNetworkManager.S.ReqVtCourseList(2);
    }
    void SkipLogin()
    {
        GameObject.Find("Close").SetActive(false);
        GameObject.Find("Contents").SetActive(false);

        // noAPI일 경우 단순 다음 Scene으로 전환
        SceneManager.LoadScene(LoginNetworkManager.S.m_NextSceneLevel);
    }
}
