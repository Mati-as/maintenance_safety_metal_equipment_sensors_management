using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ServiceType
{
    NONE = 0,
    //로그인버전 = 1, 현재 미사용
    런처연동버전 = 2,
    NoAPI버전 = 3
}
public enum ContentsType
{
    NONE = 0,

    //YEAR2024,
     
 	
    금속제조설비계측센서정비및안전관리 = 0,//과정 ID 추후 안내 예정
}

public struct VRCourse
{
    public string Name { get; set; }
    public string ContentsID { get; set; }
    public string CourseID { get; set; }

    public VRCourse(string courseID, string name, string contentsID)
    {
        Name = name;
        ContentsID = contentsID;
        CourseID = courseID;
    }
}

public static class VRContents
{
    static VRContents()
    {
    }

    public static List<VRCourse> m_VRContents = new List<VRCourse>();
    public static ContentsType m_ContentsType;

}


//KOO

public struct VRCourseKoo
{
    public string ncs_code_name { get; set; }
    public string course_id { get; set; }
    public string service_title { get; set; }
    public string study_days { get; set; }
    public string cancel_days { get; set; }
    public string review_days { get; set; }
    public string course_content_id { get; set; }
    public string course_short_description { get; set; }
    public string properties { get; set; }
    public string course_image_url { get; set; }
    public string course_syllabus_url { get; set; }
    public string vt_package_file_url { get; set; }
    public string mobile_compatibility_code { get; set; }
    public string course_video { get; set; }
    public string reformat_install_file_name { get; set; }
    public string reformat_install_file_url { get; set; }


    public VRCourseKoo(string ncs, string courseID, string serviceTitle, string sdays, string cdays, string rdays, string contentsID, string csd, string pro, string ciu, string csu, string vpfu, string mcc, string cv, string rifn, string rifu)
    {
        course_id = courseID;
        service_title = serviceTitle;
        course_content_id = contentsID;
        ncs_code_name = ncs;
        study_days = sdays;
        cancel_days = cdays;
        review_days = rdays;
        course_short_description = csd;
        properties = pro;
        course_image_url = ciu;
        course_syllabus_url = csu;
        vt_package_file_url = vpfu;
        mobile_compatibility_code = mcc;
        course_video = cv;
        reformat_install_file_name = rifn;
        reformat_install_file_url = rifu;

    }
}


public static class VRContentsKoo
{
    static VRContentsKoo()
    {
        // Add Contents        
    }

    public static List<VRCourseKoo> m_VRContentsKoo = new List<VRCourseKoo>();
    public static ContentsType m_ContentsType;
}



public struct VRCourseKoo2
{

    public string course_id { get; set; }
    public string service_title { get; set; }
    public string course_content_id { get; set; }


    public VRCourseKoo2(string courseID, string serviceTitle, string contentsID)
    {
        course_id = courseID;
        service_title = serviceTitle;
        course_content_id = contentsID;


    }
}


public static class VRContentsKoo2
{
    static VRContentsKoo2()
    {
        // Add Contents        
    }

    public static List<VRCourseKoo2> m_VRContentsKoo2 = new List<VRCourseKoo2>();
    public static ContentsType m_ContentsType;
}
