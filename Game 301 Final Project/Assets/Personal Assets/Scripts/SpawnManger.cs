using UnityEngine;
using System.Collections;

public class SpawnManger : MonoBehaviour {

    public Transform enemiePrefab;
    GameObject[] spawnLocations;
    bool isSpawning = false;
    float timeBetweenSpawns;
    float lastSpawnTime;

	// Use this for initialization
	void Start () {
        timeBetweenSpawns = 5.0f;
        lastSpawnTime = 0.0f;

        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Location");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isSpawning)
        {
            if (lastSpawnTime + timeBetweenSpawns <= Time.timeSinceLevelLoad)
            {
                Debug.Log("Spawning");

                SpawnEnemie();
                lastSpawnTime = Time.timeSinceLevelLoad;
            }
        }
	}

    void SpawnEnemie()
    {
        Instantiate(enemiePrefab,spawnLocations[Random.Range(0, spawnLocations.GetLength(0))].transform.position,Quaternion.identity);
    }

    public void StartSpawner()
    {
        isSpawning = true;
        Debug.Log("Spawner Started");
    }

    public void StopSpawner()
    {
        isSpawning = false;
        Debug.Log("Stop Spawning Called");
    }
}
