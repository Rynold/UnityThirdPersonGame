using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Enter Overlap");
        //if (c = playerCharacter.GetComponent<Collider>())
        //{
        //    canDamage = true;

        //}
    }

    void OnTriggerExit(Collider c)
    {
        Debug.Log("End Overlap");
        //if (c = playerCharacter.GetComponent<Collider>())
        //{
        //    canDamage = false;

        //}
    }
}
