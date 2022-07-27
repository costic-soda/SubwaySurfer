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
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;


[System.Serializable]

public enum SIDE {Left, Mid, Right }
public enum HitX { Left, Mid, Right, None }
public enum HitY { UP, Mid, Down , None}
public enum HitZ { Forward, Mid, Backward , None}
public class PlayerMovement : MonoBehaviour
{
    // Socket Code 
    bool alive = true;
    public bool increase = false;
    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;
    byte[] data;
    byte[] data2;
    //ushort pointX, pointY;
    uint pointP;
    Socket listener;
    Socket handler;
    //Game code
    public SIDE m_Side = SIDE.Mid;
    float NewXPos = 0f;
    public bool SwipeLeft = false;
    public bool SwipeRight = false;
    public bool SwipeDown = false;
    public bool SwipeUp = false;
    public float ZValue;
    public Animator anim;

    Vector3 velocity;
    public float gravity = -20f;
    public float JumpHeight = 2f;

    private CharacterController m_char;
    private CapsuleCollider capsuleCollider;
    private float x, y, temp;
    public float fwdSpeed = 7f;
    public float JumpPower = 7f;
    public float speedIncreasePerPoint = 1f;

    public HitX hitX = HitX.None;
    public HitY hitY = HitY.None;
    public HitZ hitZ = HitZ.None;
    public bool flag = false;
    public byte[] results;

    public string camValues;
    public TextMeshProUGUI matData;

    GroundSpawner groundSpawner;
    GroundTile groundTile;

    public float pollingTime = 0.5f;
    private float time;
    private float frameCount;
    public TextMeshProUGUI FPSText;

    public GameObject gameOverPanel;

    private float crouchThreshold;
    public bool thresholdSet = false;
    public float crouchThresholdOffset = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

        m_char = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
        capsuleCollider = GetComponent<CapsuleCollider>();
        transform.position = Vector3.zero;
        float camX;
        //bool isCamNull = float.TryParse(Globals.Variables.camX, out camX);
        //temp = camX;

        //placeObstacles();
        //Application.runInBackground = true;
        // startServer();

        //StartCoroutine(GetText());

    }


    //IEnumerator GetText()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:8696/json-stream");
    //    Debug.Log("1");
    //    yield return www.SendWebRequest();
    //    Debug.Log("2");

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log("IF");
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        Debug.Log("Before");
    //        Debug.Log(www.downloadHandler.text);
    //        Debug.Log("After");
    //        // Or retrieve results as binary data
    //        results = www.downloadHandler.data;
    //        Debug.Log(results);
    //    }
    //}


    public void Speed() {

        if (transform.position.z > 10) {

            fwdSpeed += 0.2f;
        }
    
    }
   /* void placeObstacles() {
        GameObject[] test;
        test = GameObject.FindGameObjectsWithTag("Obstacle");
        //GameObject.Find("Cube_1").transform.localPosition = new Vector3(UnityEngine.Random.Range(-5, 5), 1, UnityEngine.Random.Range(10, 120));
        // test = new Vector3(UnityEngine.Random.Range(-5, 5), 1, UnityEngine.Random.Range(10, 120));
        var random = new System.Random();
        var list = new List<int> { -5, 0, 5 };
        foreach (GameObject go in test)
        {
          

            int index = random.Next(list.Count);
            Debug.Log(list[index]);
            go.transform.localPosition = new Vector3(list[index], 1, UnityEngine.Random.Range(20, 60));
           

            if(go.transform.localPosition.x == 5){

                go.transform.localPosition = new Vector3(-5, 1, UnityEngine.Random.Range(20, 60));

            Debug.Log("inside IF");
            
            }

            else if(go.transform.localPosition.x == -5)
            {
                go.transform.localPosition = new Vector3(0, 1, UnityEngine.Random.Range(20, 60));

                Debug.Log("inside else IF");
            }

            else if(go.transform.localPosition.x == 0 ){

                go.transform.localPosition = new Vector3(5, 1, UnityEngine.Random.Range(20, 60));
            Debug.Log("inside ELSE");
            }
            // transform.localPosition = new Vector3(5, 1, 30);

         

        }

        Debug.Log("inside place");
    
    }*/


    /*void obstacles() {
        GameObject[] obj, player;
        obj = GameObject.FindGameObjectsWithTag("Obstacle");
        player = GameObject.FindGameObjectsWithTag("Player");
        var random = new System.Random();
        var list = new List<int> { -5, 0, 5 };
            foreach (GameObject go in obj)
            {
                int index = random.Next(list.Count);
                Debug.Log(list[index]);
                go.transform.localPosition = new Vector3(list[index], 1, UnityEngine.Random.Range(20, 60));
                if (player.transform.localPosition.x == 5) {
                    go.transform.localPosition = new Vector3(-5, 1, UnityEngine.Random.Range(20, 60));
                
                }

            }

    
    
    }*/

    //void startServer()
    //{
    //    SocketThread = new System.Threading.Thread(networkCode);
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

    void playerMov()
    {
       

        //if (!alive) return;
        //if (sc.pointX <= 21)
        //{
        //    SwipeLeft = true;
        //    if (SwipeLeft)
        //    {
        //        if (m_Side == SIDE.Mid)
        //        {
        //            NewXPos = -ZValue;
        //            m_Side = SIDE.Left;
        //        }
        //        else if (m_Side == SIDE.Right)
        //        {
        //            NewXPos = 0;
        //            m_Side = SIDE.Mid;
        //        }


        //    }
        //    SwipeLeft = false;
        //}


        //else if (sc.pointX >= 30)
        //{
        //    SwipeRight = true;
        //    if (SwipeRight)
        //    {

        //        if (m_Side == SIDE.Mid)
        //        {
        //            NewXPos = ZValue;
        //            m_Side = SIDE.Right;
        //        }
        //        else if (m_Side == SIDE.Left)
        //        {
        //            NewXPos = 0;
        //            m_Side = SIDE.Mid;
        //        }
        //    }




        //}
        //else
        //{
        //    if (m_Side == SIDE.Left)
        //    {
        //        NewXPos = 0;
        //        m_Side = SIDE.Mid;
        //    }
        //    else if (m_Side == SIDE.Right)
        //    {
        //        NewXPos = 0;
        //        m_Side = SIDE.Mid;
        //    }
        //}

        //if (SwipeDown)
        //{

        //    capsuleCollider.height = 3.5f;
        //    anim.Play("crouchRun2");



        //}
        //else
        //{

        //    capsuleCollider.height = 4.944406f;

        //}




        //Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, fwdSpeed * Time.deltaTime);
        //x = Mathf.Lerp(x, NewXPos, Time.deltaTime * 10);
        //m_char.Move(moveVector);

        //Jump();






    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FPSText.text = frameRate.ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }

        //SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        // SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);



        //else if((pointX>20) && (pointX < 30))
        //{


        //    m_Side = SIDE.Mid;

        //}

        ////  SocketConnection.pointY
        //Debug.Log("matX: " + Globals.Variables.matX + "matY: " + Globals.Variables.matY);
        //Debug.Log("camX: " + Globals.Variables.camX + "camY: " + Globals.Variables.camY);
        //int a;
        //bool success = int.TryParse(Globals.Variables.matX, out a);
        //if (success)
        //{
        //    Debug.Log("A = " + a);
        //int matX;
        //try
        //{
        //    matX = Convert.ToInt32(Globals.Variables.matX);
        //    Debug.Log(matX);

        //string matX = Globals.Variables.matX;
        //Debug.Log(matX);

        float matX;
        float matY;
        float camX;
        //string camValues;
      
        Debug.Log("before");
        bool isMatXNull = float.TryParse(Globals.Variables.matX, out matX);
        bool isMatYNull = float.TryParse(Globals.Variables.matY, out matY);
        //bool isCamNull = float.TryParse(Globals.Variables.camX, out camX);
        //camValues = Globals.Variables.camValues; 
        //bool isCamNullTest = float.TryParse(Globals.Variables.camValues, out camValues);
        Debug.Log("Right shoulder");

        //if (isNull)
        //{
        //    Debug.Log("in IF");
        //    Debug.Log(matX);
        //}
        //else
        //{
        //    Debug.Log("in else");
        //}
        Debug.Log("matX - " + matX + " matY - " + matY);
        //Debug.Log("camX-" +camX);

        matData.text = "MatX: " + matX + " MatY: " + matY + " CamValues: " + float.Parse(Globals.Variables.camValues[1]);

        //matData.text = "MatX: " + matX + " MatY: " + matY + " CamValues: " + string.Join(System.Environment.NewLine, Globals.Variables.camValues); 


        if (!alive) return;

        if (matX <= 21)
        {
            SwipeLeft = true;
            if (SwipeLeft)
            {
                if (m_Side == SIDE.Mid)
                {
                    NewXPos = -ZValue;
                    m_Side = SIDE.Left;
                }
                else if (m_Side == SIDE.Right)
                {
                    NewXPos = 0;
                    m_Side = SIDE.Mid;
                }


            }
            SwipeLeft = false;
        }


        else if (matX >= 30)
        {
            SwipeRight = true;
            if (SwipeRight)
            {

                if (m_Side == SIDE.Mid)
                {
                    NewXPos = ZValue;
                    m_Side = SIDE.Right;
                }
                else if (m_Side == SIDE.Left)
                {
                    NewXPos = 0;
                    m_Side = SIDE.Mid;
                }
            }
        }
        else
        {
            if (m_Side == SIDE.Left)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
            else if (m_Side == SIDE.Right)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
        }

        if (Globals.Variables.LEFT_SHOULDER[1] != "" && !thresholdSet)
        {
            crouchThreshold = (float.Parse(Globals.Variables.LEFT_HIP[1]) + float.Parse(Globals.Variables.LEFT_SHOULDER[1])) / 2f;
            crouchThreshold += crouchThresholdOffset;
            thresholdSet = true;
        }
        if (float.Parse(Globals.Variables.LEFT_SHOULDER[1]) < crouchThreshold)
        {
            //if (camX > 50)
            //{
            //SwipeDown = true;
            //if (SwipeDown)
            //{

            matData.text = "MatX: " + Globals.Variables.matX + " MatY: " + Globals.Variables.matY + "\nleft shoouldet " + float.Parse(Globals.Variables.LEFT_SHOULDER[1]) + "\nleft hip: " + float.Parse(Globals.Variables.LEFT_HIP[1]) + "\ncrouvchg thresghold:" + crouchThreshold;


            capsuleCollider.height = 3.5f;
            anim.Play("crouchRun2");

        }
        else if (matY < 26)
        {
            SwipeUp = true;
            Jump();
        }

        else
        {
            anim.Play("Running");
            capsuleCollider.height = 4.944406f;

        }

        //if (SocketConnection.temp - SocketConnection.pointY > 15)
        //{
        //    SwipeUp = true;
        //    Jump();
        //}
        //}



        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, fwdSpeed * Time.deltaTime);
        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * 10);
        m_char.Move(moveVector);


        // playerMov();





        //Debug.Log(SocketConnection.test);
        //Debug.Log("before");
        //Debug.Log(SocketConnection.message);

        //Debug.Log(Globals.Variables.dump);

        //obstacles();
        if (!alive) return;

        //SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        //SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        //SwipeDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        //SwipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        ////Debug.Log("hello");

        //if (SwipeDown)
        //{
        //    Debug.Log("sick");
        //    Slide();
            


        //}
        ////else
        ////{

        ////    capsuleCollider.height = 4.944406f;

        ////}

        //// capsuleCollider.height = 4.944406f;

        //if (SwipeLeft)
        //{
        //    if (m_Side == SIDE.Mid)
        //    {
        //        NewXPos = -ZValue;
        //        m_Side = SIDE.Left;
        //    }
        //    else if (m_Side == SIDE.Right)
        //    {
        //        NewXPos = 0;
        //        m_Side = SIDE.Mid;
        //    }


        //}

        //else if (SwipeRight)
        //{

        //    if (m_Side == SIDE.Mid)
        //    {
        //        NewXPos = ZValue;
        //        m_Side = SIDE.Right;
        //    }
        //    else if (m_Side == SIDE.Left)
        //    {
        //        NewXPos = 0;
        //        m_Side = SIDE.Mid;
        //    }



        //}
        //m_char.Move((NewXPos - transform.position.x) * Vector3.right);

        //Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, fwdSpeed * Time.deltaTime);

        //x = Mathf.Lerp(x, NewXPos, Time.deltaTime * 10);
        ////capsuleCollider.center = capsuleCollider.center;
        //m_char.Move(moveVector);

        //Jump();
        //   Speed();
    //}
        // catch (FormatException)
        //{

        //    Debug.Log("exception raised");
        //}
    }

    IEnumerator myTime()
    {
       
        yield return new WaitForSeconds(2f);

    }
    public void Slide()
    {
        capsuleCollider.height = 2f;
        //anim.Play("crouchRun2");
        anim.Play("newCrouch");
        //Debug.Log("slide");
        //StartCoroutine("SlideTime");
    }
    public void setColliderAfterSlide()
    {
        capsuleCollider.height = 4.944406f;
    }
    IEnumerator SlideTime()
    {
        Debug.Log("bruh");
        yield return new WaitForSeconds(3f);
        //yield WaitForSeconds(anim["clip"].length* anim["clip"].speed);
        capsuleCollider.height = 4.944406f;
    }
    public void Jump()
    {

        if (m_char.isGrounded)
        {
            if (SwipeUp)
            {
                y = JumpPower;
                capsuleCollider.center = new Vector3(0, 30f, 0);
                //  m_char.center = new Vector3(0, 4f, 0);
                anim.Play("newJump");


            }
            //else
            //{
            //    capsuleCollider.center = new Vector3(-0.08411723f, 2.310141f, 4.6511e-11f);
            //    // m_char.center = new Vector3(0, 4f, 0);
            //    //   m_char.center = m_char.center;
            //    //   transform.position = Vector3.zero;

            //}
            // StartCoroutine(myTime());


        }


        else
        {


            y -= JumpPower * 2.25f * Time.deltaTime;
            // 



        }
        //SwipeUp = false;
        //capsuleCollider.center = new Vector3(-0.08411723f, 2.310141f, 4.6511e-11f);
    }
    public void SetColiiderAfterJump()
    {
        capsuleCollider.center = new Vector3(-0.08411723f, 2.310141f, 4.6511e-11f);
    }

    public void OnTriggerExit(Collider other)
    {
        //groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        /*  groundTile = GameObject.FindObjectOfType<GroundTile>();
          Destroy(groundSpawner.plotL,2);
          Destroy(groundTile.spawnObstacle, 2);*/
       // flag = true;
    }

 

    public void Die() {

        alive = false;
        anim.Play("FallOver");
        //  Invoke("Restart", 2);
        //gameOverPanel.SetActive(true);
    
       
    }
    public void SetGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        
    }

    public void GameQuit()
    {

        Application.Quit();
    }
    public void Restart() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    /* public void OnCharacterColliderHit(Collider col) {
         hitX = GetHitX(col);
     }
     public HitX GetHitX(Collider col) {
         Bounds char_bounds = m_char.bounds;
         Bounds col_bounds = col.bounds;
         float min_x = Mathf.Max(col_bounds.min.x, char_bounds.min.x);
         float max_x = Mathf.Min(col_bounds.max.x, char_bounds.max.x);
         float average = (min_x + max_x) / 2f - col_bounds.min.x;
         HitX hit;
         if (average > col_bounds.size.x - 0.30f)
             hit = HitX.Right;
         else if (average < 0.30f)
             hit = HitX.Left;
         else
             hit = HitX.Mid;
         return hit;

     }*/
    
}








