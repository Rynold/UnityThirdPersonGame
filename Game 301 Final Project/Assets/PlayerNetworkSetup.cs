using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField]
    GameObject playersCamera;

    // Use this for initialization
    void Start () {
	    if (isLocalPlayer)
        {
            //playersCamera.SetActive(true);
            GetComponent<PlayerController>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
