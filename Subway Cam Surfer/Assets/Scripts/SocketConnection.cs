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
    // Start is called before the first frame update

    //void Start()
    //{
    //    Application.runInBackground = true;
    //    startServer();

    //}

    //void startServer()
    //{
    //    //SocketThread = new System.Threading.Thread(networkCode);
    //    SocketThread = new System.Threading.Thread(Begin);
    //    SocketThread.IsBackground = true;
    //    SocketThread.Start();
    //}

    //private string getIPAddress()
    //{
    //    IPHostEntry host;
    //    /*string localIP = "";
    //    host = Dns.GetHostEntry(Dns.GetHostName());
    //    foreach (IPAddress ip in host.AddressList)
    //    {
    //        if (ip.AddressFamily == AddressFamily.InterNetwork)
    //        {
    //            localIP = ip.ToString();
    //        }

    //    }*/
    //    string localIP = "127.0.0.1";
    //    return localIP;
    //}





    //void networkCode()
    //{


    //    // Data buffer for incoming data.
    //    byte[] bytes = new byte[1024];
    //    byte[] bytes2 = new byte[1024];

    //    // host running the application.
    //    Debug.Log("Ip " + getIPAddress().ToString());
    //    IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
    //    IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 6969);
    //    IPEndPoint localEndPoint2 = new IPEndPoint(ipArray[0], 1111);


    //    // Create a TCP/IP socket.
    //    Socket sender = new Socket(ipArray[0].AddressFamily,
    //    SocketType.Stream, ProtocolType.Tcp);
    //    Socket sender2 = new Socket(ipArray[0].AddressFamily,
    // SocketType.Stream, ProtocolType.Tcp);

    //    // Connect the socket to the remote endpoint. Catch any errors.
    //    try
    //    {
    //        // Connect to Remote EndPoint
    //        sender.Connect(localEndPoint);
    //        sender2.Connect(localEndPoint2);

    //        Console.WriteLine("Socket connected to {0}",
    //            sender.RemoteEndPoint.ToString());

    //        // Encode the data string into a byte array.
    //        byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

    //        // Send the data through the socket.
    //        // int bytesSent = sender.Send(msg);

    //        // Receive the response from the remote device.
    //        /*byte[] bytesRec = sender.Receive(bytes);
    //        Console.WriteLine("Echoed test = {0}",
    //            Encoding.ASCII.GetString(bytes, 0, bytesRec));*/

    //        data = new byte[sender.Available];
    //        data2 = new byte[sender2.Available];
    //        // data2 = new float[sender.Available];
    //        //uint converted_data = Convert.ToUInt32(data);
    //        //Debug.Log(data);
    //        //string str = BitConverter.ToString(data);

    //        //Debug.Log(str);
    //        //int test = (int)data[1];
    //        //Debug.Log(test);
    //        int length;
    //        int length2;
    //        length2 = sender2.Receive(data2);
    //        temp = BitConverter.ToUInt16(data2, 0);
    //        while (true)
    //        {
    //            length = sender.Receive(data);
    //            length2 = sender2.Receive(data2);
    //            //Debug.Log(length);
    //            //pointX = BitConverter.ToUInt16(data, 0);
    //            pointX = data[0];
    //            pointY = BitConverter.ToUInt16(data2, 0);
    //            //pointY = data2[0];
    //            //pointP= BitConverter.ToUInt32(data, 0);
    //            //pointY = BitConverter.ToUInt16(data, 2);
    //            // pointZ = BitConverter.ToUInt16(data, 4);
    //            Debug.Log(pointX);
    //            //Debug.Log(pointY);
    //            // Debug.Log("--------");

    //            // int test = (int)data[0];
    //            //Debug.Log(test);
    //            // Debug.Log(data[1]);


    //        }
    //        //Debug.Log(length);
    //        //uint converted_data = BitConverter.ToUInt8(data,1);
    //        //byte converted_data = data[1];
    //        //string str = Encoding.Unicode.GetBytes(data);
    //        //}
    //        //Debug.Log(str);

    //        //Debug.Log(pointX);
    //        // Debug.Log(pointY);
    //        //Debug.Log(data[0]);
    //        //string stringByte = BitConverter.ToString(data);
    //        //Debug.Log(getAngle(pointX));
    //        Debug.Log("after receiving data");
    //        //Debug.Log(stringByte);
    //        /* 
    //      Debug.Log(converted_data);

    //       Debug.Log(data[0]);
    //      Debug.Log(data[1]);*/
    //        // Debug.Log(length);

    //        // Release the socket.
    //        //sender.Shutdown(SocketShutdown.Both);
    //        //sender.Close();

    //    }
    //    catch (ArgumentNullException ane)
    //    {
    //        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
    //    }
    //    catch (SocketException se)
    //    {
    //        Console.WriteLine("SocketException : {0}", se.ToString());
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine("Unexpected exception : {0}", e.ToString());
    //    }



    //}

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
                
                    message = System.Text.Encoding.UTF8.GetString(bytes);
                    data = JsonConvert.DeserializeObject<Data>(message);

                    //if(data.camValues == null)
                    //{
                    //    Globals.Variables.matX = data.x;
                    //    Globals.Variables.matY = data.y;
                    //}

                    if (data.x == null)
                    {
                        Globals.Variables.camValues = data.camValues;
                    }

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


