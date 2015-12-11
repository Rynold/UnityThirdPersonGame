using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    GameObject playerCharacter;

	// Use this for initialization
	void Start () 
    {
        //playerCharacter = GameObject.Find("1st Player");	
	}

    public void init(GameObject player)
    {
        playerCharacter = player;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        transform.position = playerCharacter.transform.position;
    }
}
