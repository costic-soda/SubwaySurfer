using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using WebSocketSharp;
using System.Text;
using UnityEngine.SceneManagement;

public class TestSocket : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("hi");
        using (var ws = new WebSocket("ws://127.0.0.1:9999"))
        {

            Debug.Log("hi1111");
            ws.OnOpen += Ws_OnOpen;
            ws.OnMessage += (sender, e) =>
                Debug.Log("Message from server: " + e.Data);


            Debug.Log("hi2222");
        ws.Connect();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void test()
    {
        
    }

    private static void Ws_OnOpen(object sender, EventArgs e)
    {
       Debug.Log("connection opened");
        //throw new NotImplementedException();
    }

}
