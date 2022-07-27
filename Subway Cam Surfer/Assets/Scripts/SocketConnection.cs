using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
//using WebSocketSharp;
using System.Text;
using UnityEngine.SceneManagement;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine.Networking;




public class SocketConnection : MonoBehaviour
{

    // Socket Code 
    bool alive = true;
    //public bool increase = false;
    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;
    //byte[] data;
    //byte[] data2;
    public static ushort pointX = 1 , pointY;
    //public static float data;
    public static string message, test;
    public static float temp;
    uint pointP;
    Socket listener;
    Socket handler;
    

    Data data;
    WebSocket websocket;


    public class Data
    {
        public string x { get; set; }
        public string y { get; set; }
        public string camX { get; set; }
        public string camY { get; set; }
        public string camZ { get; set; }
        public List<string> camValues { get; set; }
    }

    public class CamData
    {
        public class camValues
        {
            public string x { get; set; }
            public string y { get; set; }
            public List<string> NOSE { get; set; }
            public List<string> LEFT_SHOULDER { get; set; }
            public List<string> LEFT_ELBOW { get; set; }
            public List<string> LEFT_WRIST { get; set; }
            public List<string> RIGHT_SHOULDER { get; set; }
            public List<string> RIGHT_ELBOW { get; set; }
            public List<string> RIGHT_WRIST { get; set; }
            public List<string> LEFT_HIP { get; set; }
            public List<string> LEFT_KNEE { get; set; }
            public List<string> LEFT_ANKLE { get; set; }
            public List<string> RIGHT_HIP { get; set; }
            public List<string> RIGHT_KNEE { get; set; }
            public List<string> RIGHT_ANKLE { get; set; }
        }

        public class Root
        {
            public camValues camValues { get; set; }
        }
    }


    // Start is called before the first frame update

    void Start()
    {
        Application.runInBackground = true;
        Begin();

        StartCoroutine(getRequest("http://localhost:5000/mGetGameInfo"));
        StartCoroutine(getRequest("http://localhost:5000/godotReady"));
    }

    async void Begin()
    {
      
       
       
            websocket = new WebSocket("ws://127.0.0.1:5678");

            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            websocket.OnMessage += (bytes) =>
            {
              

                // getting the message as a string and deserialize the json string
                // also store it in the global variable
     
                List<string> zero = new List<string>();
                message = System.Text.Encoding.UTF8.GetString(bytes);
                CamData.Root myDeserializedClass = JsonConvert.DeserializeObject<CamData.Root>(message);
                var camValues = myDeserializedClass.camValues;

                Globals.Variables.LEFT_SHOULDER = (camValues.LEFT_SHOULDER != null) ? camValues.LEFT_SHOULDER : zero;
                Globals.Variables.LEFT_HIP = (camValues.LEFT_HIP != null) ? camValues.LEFT_HIP : zero;
                Globals.Variables.matX = camValues.x;
                Globals.Variables.matY = camValues.y;


                //if(data.camValues == null)
                //{
                //    Globals.Variables.matX = data.x;
                //    Globals.Variables.matY = data.y;
                //}

                //Globals.Variables.camX = data.camX;
                //Globals.Variables.camY = data.camY; 
                //Globals.Variables.camY = data.camZ;                   


                //message = System.Text.Encoding.UTF8.GetString(bytes);
                //Debug.Log(message);

            };

            // Keep sending messages at every 0.3s
            //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        
            await websocket.Connect();
    
    }


   

    void Update()
    {

       
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
       



    }

    // For sending messages 
        
            //async void SendWebSocketMessage()
            //{
            //    if (websocket.State == WebSocketState.Open)
            //    {
            //        // Sending bytes
            //        await websocket.Send(new byte[] { 10, 20, 30 });

            //        // Sending plain text
            //        await websocket.SendText("plain text message");
            //    }
            //}

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    IEnumerator getRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

    }




}


