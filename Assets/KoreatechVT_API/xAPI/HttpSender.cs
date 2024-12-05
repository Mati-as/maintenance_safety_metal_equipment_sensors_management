using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Net;
using UnityEngine.Networking;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
public class HttpSender : MonoBehaviour
{
    void PrintRes(string s1, string s2)
    {
        print(s1);
        print(s2);
    }
    public void Send(string json)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=utf-8");
        byte[] post_data = System.Text.Encoding.UTF8.GetBytes(json);

        WWW www = new WWW("https://vtmanager.step.or.kr/apis/updateContentHistory", post_data, headers);
        StartCoroutine((Request(www, PrintRes)));
    }

    IEnumerator Request(WWW www, UnityAction<string, string> done)
    {
        yield return www;
        if (www.error != null || www.url == null)
        {
            Debug.Log("WWW Call Error." + www.error);
            done(www.responseHeaders["STATUS"], www.text);
            yield break;
        }

        if (www.isDone)
        {
            print(www.text);
            if (done != null)
            {
                done(www.responseHeaders["STATUS"], www.text);
            }
        }
    }

}
