using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {

    GameObject owner;
    Vector3 vel;
    public float speed;
    float lifeTime;
    float startTime;

	// Use this for initialization
	void Start () {
        lifeTime = 3.0f;
        startTime = Time.realtimeSinceStartup;
	}

    public void init(Vector3 velDir, GameObject caster)
    {
        vel = velDir * speed;
        owner = caster;
    }
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position += vel;
	}

    void FixedUpdate()
    {
        if (startTime + lifeTime <= Time.realtimeSinceStartup)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision c)
    {


    }
}
