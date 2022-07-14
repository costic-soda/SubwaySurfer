using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class SocketClass 
{
    private WebSocket websocket;
    private string hostAddress, message;
    private bool isConnect;
    Data data;
    public class Data
    {
        public string x { get; set; }
        public string y { get; set; }
        public string camX { get; set; }
        public string camY { get; set; }

        public string camZ { get; set; }
    }

    public SocketClass(string hostAddress, bool isConnect)
    {
        this.hostAddress = hostAddress;
        this.isConnect = isConnect;
    }

       public int isConnected()
    {
        if (this.websocket.State == WebSocketState.Open)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


    public async void onConnect()
    {
        this.websocket = new WebSocket(this.hostAddress);
        this.websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        this.websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        this.websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        this.websocket.OnMessage += (bytes) =>
        {
            // getting the message as a string and deserialize the json string
            // also store it in the global variable
            message = System.Text.Encoding.UTF8.GetString(bytes);
            data = JsonConvert.DeserializeObject<Data>(message);
            Globals.Variables.matX = data.x;
            Globals.Variables.matY = data.y;
            //Globals.Variables.camX = data.camX;
            //Globals.Variables.camY = data.camY;
            Debug.Log("in connect;");
        };

        await this.websocket.Connect();
    }

 

    //public void receiveData()
    //{
    //    this.websocket.OnMessage += (bytes) =>
    //    {
    //        // getting the message as a string and deserialize the json string
    //        // also store it in the global variable
    //        message = System.Text.Encoding.UTF8.GetString(bytes);
    //        data = JsonConvert.DeserializeObject<Data>(message);
    //        Globals.Variables.matX = data.x;
    //        Globals.Variables.matY = data.y;
    //        Globals.Variables.camX = data.camX;
    //        Globals.Variables.camY = data.camY;
    //    };
    //}

    public void updateMethod()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        this.websocket.DispatchMessageQueue();
#endif
    }

    public async void onDisconnect()
    {
        await this.websocket.Close();
    }


}




public class FinalSocketClass : MonoBehaviour
{
    SocketClass socketClass;
    void Start()
    {
        string hostAddress = "ws://127.0.0.1:5678";
        bool connect = true;
        this.socketClass = new SocketClass(hostAddress, connect);
        this.socketClass.isConnected();
        this.socketClass.onConnect();
    
    
    }

    void Update()
    {
        this.socketClass.updateMethod();
    }

}