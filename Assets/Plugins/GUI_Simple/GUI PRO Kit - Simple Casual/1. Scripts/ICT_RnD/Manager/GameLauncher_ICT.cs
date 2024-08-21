using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameLauncher_ICT : MonoBehaviour
{
    public GameObject ICT_RnD_UI;
    private GameObject Loading;
    private GameObject Home;
    private GameObject Setting;
    private GameObject Login;
    private GameObject Tool;
    private GameObject Result;
    private GameObject Contents;
    private GameObject Mode;
    private GameObject Survey;
    private GameObject Monitoring_Music;
    private GameObject Monitoring_C1;
    private GameObject Monitoring_C2;
    private GameObject Monitoring_C3;
    private GameObject Monitoring_C4;

    public GameObject Message_UI;
    private GameObject Message_Tool;
    private GameObject Message_Content_StudentCheck;
    private GameObject Message_Intro;
    private GameObject Message_L_StudentCheck;
    private GameObject Message_L_FieldEmpty;
    private GameObject Message_L_StudentDataSaved;
    private GameObject Message_L_SelectedStudentCheck;
    private GameObject Message_L_Nonselect;
    private GameObject Message_Survey_StudentCheck;
    private GameObject Message_L_Completed;

    private GameObject Message_EndMusicContent;
    private Message_anim_controller MAC;

    private GameObject Prev_page;
    private GameObject Next_page;
    private bool Is_Toolsaved = false;


    // Start is called before the first frame update
    [Header("[LOADING PAGE COMPONENT]")]
    [SerializeField]
    public Slider progressBar;
    public Text loadingPercent;
    public Image loadingIcon;

    private bool loadingCompleted;
    private int nextScene;

    public int Session;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadScene());
        StartCoroutine(RotateIcon());

        loadingCompleted = false;
        nextScene = 0;
        Init_page();
    }

    IEnumerator LoadScene()
    {
        //yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        //while (!op.isDone)
        while (true)
        {
            //yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);
                loadingPercent.text = "progressBar.value";

                if (progressBar.value == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                if (progressBar.value >= op.progress)
                {
                    timer = 0f;

                    //End of scene index
                    if (nextScene == 2 && loadingCompleted)
                    {
                        StopAllCoroutines();
                    }
                }
            }
        }
    }

    IEnumerator RotateIcon()
    {
        float timer = 0f;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            timer += Time.deltaTime;

            //Debug.Log(progressBar.value);
            //Debug.Log("check");
            if (progressBar.value < 100f)
            {
                progressBar.value = Mathf.RoundToInt(Mathf.Lerp(progressBar.value, 100f, timer / 8));
                loadingIcon.rectTransform.Rotate(new Vector3(0, 0, 100 * Time.deltaTime));
                loadingPercent.text = progressBar.value.ToString();
            }
            else
            {
                StopAllCoroutines();
                //Debug.Log("100%");

                Next_page = Home;
                UI_change();

                //Loading.SetActive(false);
                ////Mode.SetActive(true);
                //Home.SetActive(true);
            }
        }
    }

    public void UI_change()
    {
        GameObject page;
        for (int i = 0; i < ICT_RnD_UI.transform.childCount; i++)
        {
            page = ICT_RnD_UI.transform.GetChild(i).gameObject;
            if (page.gameObject.activeSelf)
            {
                Prev_page = page.gameObject;
                //Debug.Log(Prev_page);
            }
        }
        Prev_page.SetActive(false);
        Next_page.SetActive(true);
    }

    public void Button_Save_Tool()
    {
        //���۵��� ���忩��
        Is_Toolsaved = true;

        Next_page = Home;
        UI_change();
    }
    public void Button_Back_ToHome()
    {
        Next_page = Home;
        UI_change();
    }
    public void Button_Back_ToContent()
    {
        Next_page = Contents;
        UI_change();
    }
    public void Button_Back_ToMode()
    {
        Next_page = Mode;
        UI_change();
    }
    public void Button_Setting()
    {
        Setting.SetActive(true);
    }

    public void Button_Setting_Close()
    {
        Setting.SetActive(false);
    }

    public void Button_Home()
    {
        //������ ���� ���� ��� �ش� ������ ��Ȱ��ȭ ��� ���� �ʿ�
        Next_page = Home;
        UI_change();
    }
    public void Button_Tool()
    {
        Next_page = Tool;
        UI_change();
    }
    public void Button_Result()
    {
        Next_page = Result;
        UI_change();
        Manager_Result.instance.Refresh_data();
    }
    public void Button_Contents()
    {
        bool Is_Logindatasaved = Manager_login.instance.Get_Islogindatasaved();

        if (Is_Logindatasaved)
        {
            Next_page = Contents;
            UI_change();
        }
        else
        {
            Message_Content_StudentCheck.SetActive(true);
        }
    }
    public void Button_Mode(int num_mode)
    {
        //0 : Music, 1 : Contents
        if (num_mode == 0)
        {
            Run_Music_Contents();
        }
        else if (num_mode == 1)
        {
            Run_Contents();
        }
    }

    public void Button_End_Musiccontent()
    {
        //���ǳ��� ������ ����
        Manager_ResultInDetail.instance.Save_RIDdata(Session);
        Message_EndMusicContent.SetActive(true);
    }
    
    public void Run_Mode(int contentname)
    {
        Session = contentname;

        Next_page = Mode;
        UI_change();
    }
    public void Run_Music_Contents()
    {
        Next_page = Monitoring_Music;
        UI_change();
        Manager_ResultInDetail.instance.Clear_RIDdata();

        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }

    public void Button_Music_Play()
    {

        Debug.Log("PLAY " + "Session : " + Session);
        //�ش� ���� ������ ��� ��� ���� �ʿ�
        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }

    public void Button_Music_Replay()
    {

        Debug.Log("REPLAY " + "Session : " + Session);
        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }

    public void Button_Music_Stop()
    {
        Debug.Log("STOP " + "Session : " + Session);
        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }

    public void Button_Music_Analysis()
    {

        Debug.Log("Analysis " + "Session : " + Session);
        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }

    public void Button_Music_Listening()
    {

        Debug.Log("Listening " + "Session : " + Session);
        if (Session == 0)
        {

        }
        else if (Session == 1)
        {

        }
        else if (Session == 2)
        {

        }
        else if (Session == 3)
        {

        }
    }
    public void Run_Contents()
    {
        //���� ��ȯ
        Is_Toolsaved = false;

        //�ش� ������ ���� ���� ��� ����
        Dummy_setting_content();

        //Message_Intro setting
        Message_Intro.SetActive(true);

        if (Session == 0)
        {

            Next_page = Monitoring_C1;
            MAC.Change_text("(�׽�Ʈ)ģ���� �ɺ��̿� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (Session == 1)
        {
            Next_page = Monitoring_C2;
            MAC.Change_text("(�׽�Ʈ)ģ���� ��ٿ� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (Session == 2)
        {
            Next_page = Monitoring_C3;
            MAC.Change_text("(�׽�Ʈ)ģ���� �˷ο��� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (Session == 3)
        {
            Next_page = Monitoring_C4;
            MAC.Change_text("(�׽�Ʈ)ģ���� �������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        UI_change();
        //SceneManager.LoadSceneAsync(1);
    }
    public void Run_Contents_Func(int content_func)
    {
        //�ٸ� ������ ���α�� ���� ���ΰ� ��Ȱ��ȭ
        //�� ������ ���� ��� �����ų��

        Message_Intro.SetActive(true);
        //C1
        if (content_func == 0)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� ������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 1)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� �˰��� ���������?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 2)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� ���̿� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 3)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� Ư¡�� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 4)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� �������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 5)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� ��Ȱ�翡 ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 6)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�ɺ��� ü��Ȱ���� �غ����?");
            MAC.Animation_On_Off();
        }//C2
        else if (content_func == 10)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)��� ������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 11)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)��� �˰��� ���������?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 12)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)��� Ư¡�� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 13)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)��� ü��Ȱ���� �غ����?");
            MAC.Animation_On_Off();
        }//C3
        else if (content_func == 20)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)������ ������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 21)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)������ �˰��� ���������?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 22)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)������ Ư¡�� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 23)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)������ ü��Ȱ���� �غ����?");
            MAC.Animation_On_Off();
        }//C4
        else if (content_func == 30)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�˷ο� ������� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 31)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�˷ο� �˰��� ���������?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 32)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�˷ο� Ư¡�� ���� �˾ƺ����?");
            MAC.Animation_On_Off();
        }
        else if (content_func == 33)
        {
            //�ش� ������ ���� ��� ����
            MAC.Change_text("(�׽�Ʈ)�˷ο� ü��Ȱ���� �غ����?");
            MAC.Animation_On_Off();
        }
        //SceneManager.LoadSceneAsync(1);
    }


    //���� ����, �л� ���� ������ Ȯ��
    public void Button_Message_Contents()
    {
        if (Is_Toolsaved)
        {
            Button_Contents();
        }
        else
        {
            Message_Tool.SetActive(true);
        }
    }
    public void Button_Message_Contents_Select(int Num_content)
    {
        Run_Mode(Num_content);
    }
    public void Button_Login()
    {
        Login.SetActive(true);
    }
    public void Button_Survey()
    {
        bool Is_Logindatasaved = Manager_login.instance.Get_Islogindatasaved();

        if (Is_Logindatasaved)
        {
            Survey.SetActive(true);
            Manager_Survey.instance.Init_Survey();
        }
        else
        {
            Message_Survey_StudentCheck.SetActive(true);
        }
    }

    public void Button_Message_Login_SelectedStudentCheck()
    {
        bool Is_Studentdatasaved = Manager_login.instance.Get_Is_StudentDataSelected();

        if (Is_Studentdatasaved)
        {
            Message_L_SelectedStudentCheck.SetActive(true);
            Message_L_SelectedStudentCheck.GetComponent<Message_SelectedStudentInfo>().Change_Info("�л����� �α����ұ��?");
        }
        else
        {
            Message_L_Nonselect.SetActive(true);
        }
    }
    public void Button_Message_Login_StudentNotSelect()
    {
        Message_L_Nonselect.SetActive(true);
    }
    public void Button_Message_Login_StudentDataSaved()
    {
        Message_L_StudentDataSaved.SetActive(true);
    }
    public void Button_Message_Login_FieldEmpty()
    {
        Message_L_FieldEmpty.SetActive(true);
    }
    public void Button_Message_Login_Completed()
    {
        Message_L_Completed.SetActive(true);
        Login.SetActive(false);
    }

    public void Save_Data()
    {
        //���⼭ ���� result�� ���� �ϴ� �κ�
        DialogueData Saved_data = new DialogueData();

        Saved_data.ID = Manager_login.instance.ID;
        Saved_data.Name = Manager_login.instance.Name;
        Saved_data.Birth_date = Manager_login.instance.Birthdate;
        Saved_data.Date = Manager_login.instance.Date;
        Saved_data.Session = Manager_login.instance.Session.ToString();
        Saved_data.Data_1 = Manager_login.instance.Data_1;
        Saved_data.Data_2 = Manager_login.instance.Data_2;
        Manager_Result.instance.Add_data(Saved_data);
        Manager_Result.instance.Write();

    }
    void Dummy_setting_content()
    {
        //������ ����
    }
    void Dummy_setting_content_Func()
    {
        //������ ���� ��� ����
    }

    void Init_page()
    {
        //Page
        Loading = ICT_RnD_UI.transform.GetChild(0).gameObject;
        Home = ICT_RnD_UI.transform.GetChild(1).gameObject;
        Tool = ICT_RnD_UI.transform.GetChild(2).gameObject;
        Result = ICT_RnD_UI.transform.GetChild(3).gameObject;
        Contents = ICT_RnD_UI.transform.GetChild(4).gameObject;
        Mode = ICT_RnD_UI.transform.GetChild(5).gameObject;
        Monitoring_Music = ICT_RnD_UI.transform.GetChild(6).gameObject;
        Monitoring_C1 = ICT_RnD_UI.transform.GetChild(7).gameObject;
        Monitoring_C2 = ICT_RnD_UI.transform.GetChild(8).gameObject;
        Monitoring_C3 = ICT_RnD_UI.transform.GetChild(9).gameObject;
        Monitoring_C4 = ICT_RnD_UI.transform.GetChild(10).gameObject;

        Setting = ICT_RnD_UI.transform.GetChild(11).gameObject;
        Login = ICT_RnD_UI.transform.GetChild(12).gameObject;
        Survey = ICT_RnD_UI.transform.GetChild(13).gameObject;

        //Message
        Message_Tool = Message_UI.transform.GetChild(0).gameObject;
        Message_Content_StudentCheck = Message_UI.transform.GetChild(1).gameObject;
        Message_Intro = Message_UI.transform.GetChild(2).gameObject;
        Message_L_FieldEmpty = Message_UI.transform.GetChild(3).gameObject;
        Message_L_StudentDataSaved = Message_UI.transform.GetChild(4).gameObject;
        Message_L_SelectedStudentCheck = Message_UI.transform.GetChild(5).gameObject;
        Message_L_Nonselect = Message_UI.transform.GetChild(6).gameObject;
        Message_Survey_StudentCheck = Message_UI.transform.GetChild(7).gameObject;
        Message_EndMusicContent = Message_UI.transform.GetChild(8).gameObject;
        Message_L_Completed = Message_UI.transform.GetChild(9).gameObject;

        //Message_Intro setting, Inspector���� scale 0,0,0���� ����
        Message_Intro.SetActive(true);
        MAC = Message_Intro.GetComponent<Message_anim_controller>();

    }

    public void UI_Back()
    {
        Prev_page.SetActive(true);
        Next_page.SetActive(false);
    }
}
