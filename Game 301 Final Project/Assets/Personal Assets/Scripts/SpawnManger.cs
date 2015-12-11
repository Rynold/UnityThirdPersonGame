using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManger : NetworkBehaviour {

    public Transform enemiePrefab;
    GameObject[] playerCharacters;
    int playerIndex;
    GameObject[] spawnLocations;
    bool isSpawning = false;
    float timeBetweenSpawns;
    float lastSpawnTime;

	// Use this for initialization
	void Start () {
        timeBetweenSpawns = 5.0f;
        lastSpawnTime = 0.0f;
        playerIndex = 0;
        playerCharacters = new GameObject[2];
        isSpawning = true;
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Location");
	}
	
    public void AddPlayer(GameObject player)
    {
        if (playerCharacters[0] == null)
            playerCharacters[0] = player;
        else
            playerCharacters[1] = player;

        isSpawning = true;
    }
	// Update is called once per frame
	void Update () 
    {
        if (isServer)
        {
            if (isSpawning)
            {
                if (lastSpawnTime + timeBetweenSpawns <= Time.timeSinceLevelLoad)
                {
                    Debug.Log("Spawning");
                    lastSpawnTime = Time.timeSinceLevelLoad;

                    CmdSpawnEnemie();
                }
            }
        }
	}

    [Command]
    void CmdSpawnEnemie()
    {
        GameObject enemy = Instantiate(enemiePrefab,spawnLocations[Random.Range(0, spawnLocations.GetLength(0))].transform.position,Quaternion.identity) as GameObject;

        NetworkServer.Spawn(enemy);
    }

    int GetPlayerIndex()
    {
        playerIndex++;
        if (playerIndex == playerCharacters.Length)
            playerIndex = 0;
            
        return playerIndex;
    }

    public void StartSpawner()
    {
        //isSpawning = true;
        //Debug.Log("Spawner Started");
    }

    public void StopSpawner()
    {
        isSpawning = false;
        Debug.Log("Stop Spawning Called");
    }
}
