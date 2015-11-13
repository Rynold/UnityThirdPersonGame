using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject [] spawnLocations;

	// Use this for initialization
	void Start () 
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Location");
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
