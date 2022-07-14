using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

    public GameObject groundTile;
    GroundTile gt;
    Vector3 nextSpawnPoint;
    PlayerMovement playerMovement;
    private int initamount =  4;
    private float initStartZ=16;
    private float plotSize = 30f;
    private float xPosLeft=-30f;
    private float xPosRight=30f;
    private float lastZPos;
    public List<GameObject> plots;
    public List<int> used_numbers;
    int objNo;
    public GameObject plotLeft;
    public GameObject plotRight;
    public float delayTimer = 1.0f;
    float timer;
   


    public void SpawnTile() {

        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;


    
    }

    public void SpawnPlot()
    {


        //  for (int i = 0; i < initamount; i++)
        //  {
        //timer += Time.deltaTime;
        // if (timer <= 0)
        foreach (GameObject go in plots)
        {
            PoolManager.instance.CreatePool(go, 5);
        }
            plotRight = plots[Random.Range(0, plots.Count)];
           plotLeft = plots[Random.Range(0, plots.Count)];
            // Debug.Log(plotLeft);
            float zPos = lastZPos + plotSize;

            // plotL = Instantiate(plotLeft, new Vector3(xPosLeft,0.2f, zPos), plotLeft.transform.rotation);
            // Debug.Log( plotL);
            //GameObject plotR = Instantiate(plotRight, new Vector3(xPosRight, 0.2f, zPos), new Quaternion(0, 90, 0, 0));
            //if (playerMovement.flag == true)
            // {
            PoolManager.instance.ReuseObject(plotLeft, new Vector3(xPosLeft, 0.2f, zPos), plotLeft.transform.rotation);
            PoolManager.instance.ReuseObject(plotRight, new Vector3(xPosRight, 0.2f, zPos), new Quaternion(0, 90, 0, 0));
            //Debug.Log(plotL);

            // }
            lastZPos += plotSize;
           // timer = delayTimer;
      //  }
        /* for (int i = 0; i < initamount; i++) {

             Destroy(plots[i], 2);
         }*/
        // Destroy(plotL,3);

        // }


    }
 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++) {

            SpawnTile();
          
        }
        SpawnPlot();
        // foreach (GameObject go in plots)
        //{
        //    PoolManager.instance.CreatePool(go, 5);
        //}
        //timer = delayTimer;
    }

   
}
