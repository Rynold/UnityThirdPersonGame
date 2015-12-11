using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {

    GameObject owner;
    Vector3 vel;
    public float speed;
    float lifeTime;
    float startTime;
    float energy;
    float damage;

	// Use this for initialization
	void Start () {
        lifeTime = 3.0f;
        startTime = Time.realtimeSinceStartup;
	}

    public void init(Vector3 velDir, GameObject caster, float _energy)
    {
        
        energy = 0;
        damage = 10;

        vel = velDir * speed;
        owner = caster;
        this.energy = _energy;

        damage += _energy;
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

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Hit");
        if (c.gameObject != owner)
        {
            c.GetComponent<GhoulController>().TakeDamage(damage, owner);
            //c.GetComponent<PlayerController>().TakeDamage(damage, owner);
            Destroy(this.gameObject);
        }
    }
}
