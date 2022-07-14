using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{

    GroundSpawner groundSpawner;
    PlayerMovement playerMovement;
    [SerializeField] GameObject coinPrefab;
    public GameObject spawnObstacle;
    public bool flag = false;


    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoins();
       
        
            //groundSpawner.SpawnPlot();

      


        }





        public void OnTriggerExit(Collider other) {

        groundSpawner.SpawnTile();
        groundSpawner.SpawnPlot();
        groundSpawner.plotLeft.SetActive(false);
        groundSpawner.plotRight.SetActive(false);
        Destroy(gameObject, 2);
        Destroy(spawnObstacle, 2);
       
        // Destroy(groundSpawner.plots, 2);
        //if (other.gameObject.tag == "Player") {

        // 
        //Destroy(groundSpawner.plotL, 2);
        // for (int i = 0; i < 4; i++)
        // {

        //    Destroy(groundSpawner.plots[i], 2);
        //  }

        // }






    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject obstaclePrefab;
    public List<GameObject> obstacles;
    

    void SpawnObstacle() {
        //for (int i = 0; i < 3; i++)
       // {
            int obstacleSpawnIndex = Random.Range(2, 5);
            Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
            GameObject spawnObstacles = obstacles[Random.Range(0, obstacles.Count)];
        if (spawnObstacles == obstacles[3])
        {

            spawnObstacle = Instantiate(spawnObstacles, new Vector3(5, 0, spawnPoint.position.z * 1), spawnObstacles.transform.rotation);

        }
        else
        {
            spawnObstacle = Instantiate(spawnObstacles, new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z), spawnObstacles.transform.rotation);
        }
        // Instantiate(spawnObstacles, spawnPoint.position, spawnObstacles.transform.rotation);
        // Destroy(spawnObstacle,2);
        // }

    }

      public void SpawnCoins()
    {
        int coinsToSpawn = 5;
        for (int i = 0; i < coinsToSpawn; i++) {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
            //temp.transform.position = new Vector3(-5, 1, 1);
        }
    }

      Vector3 GetRandomPointInCollider(Collider collider)
      {
          Vector3 point = new Vector3(
              Random.Range(collider.bounds.min.x, collider.bounds.max.x),
              Random.Range(collider.bounds.min.y, collider.bounds.max.y),
              Random.Range(collider.bounds.min.z, collider.bounds.max.z)
              );
         // Vector3 point = new Vector3( -5, 0, 5);
          if (point != collider.ClosestPoint(point))
          {
              point = GetRandomPointInCollider(collider);
          }

          point.y = 1;
          return point;
      }


}
