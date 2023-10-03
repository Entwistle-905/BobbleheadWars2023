using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;

    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;

    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // current spawn Time -> time passed since last update call 
        currentSpawnTime += Time.deltaTime;

        // condition to generate a new wave of Aliens
        if(currentSpawnTime > generatedSpawnTime)
        {
            // resets the timer after a spawn occurs
            currentSpawnTime = 0;

            // spawn-time randomizer
            // creates a time between minSpawnTime and maxSpawnTime
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            // ensures number of aliens within limits
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                // this List keeps track of where you spawn aliens each wave.
                List<int> previousSpawnLocations = new List<int>();

                // limit number of aliens you can spawn by the number of spawn points
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    // check a alienOnScreen is less than the maximum
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                // compare it and set it. if aliensPerSpawn is bigger than totalAliens, put 'alienPerSpawn - totalAliens' to alienPerSpawn, 
                // if it is the opposite, put aliensPerSpawn to alienPerSpawn.
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;
                
                // this code loops once for each spawned data
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        // keep track of number of alien spawned
                        aliensOnScreen += 1;

                        // value of -1 means no index has been assigned or found for the spawnpoint
                        int spawnPoint = -1;
                        // while loop keeps looking for a spawning point (index) that that has not been used yet
                        while (spawnPoint == -1)
                        {
                            // create random index of List between 0 and number of spawnpoint
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            //check the previous spawn location array to see if the random number is an active spawn point.
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }

                        // grabs the spawn point based on the index
                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        // Instantiate() create an instance of any prefab 
                        GameObject newAlien = Instantiate(alien) as GameObject;

                        // position the alien at the spawn point
                        newAlien.transform.position = spawnLocation.transform.position;

                        // get a reference to the Alien Script
                        Alien alienScript = newAlien.GetComponent<Alien>();                             alienScript.target = player.transform;

                        // set the target to the space marine's current position.
                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                            newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                    }
                }
            }
        }
    }
}
