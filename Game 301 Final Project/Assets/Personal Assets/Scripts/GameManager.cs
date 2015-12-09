using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    SpawnManger spawner;

	// Use this for initialization
	void Start () 
    {
        spawner = GetComponent<SpawnManger>();
        spawner.StartSpawner();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
