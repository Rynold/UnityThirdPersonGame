using UnityEngine;
using System.Collections;

public class WaterStrikeScript : MonoBehaviour {

    GameObject owner;
    Vector3 vel;
    public float speed;
    float lifeTime;
    float startTime;
    float energy;
    float damage;

    // Use this for initialization
    void Start()
    {
        lifeTime = 3.0f;
        startTime = Time.realtimeSinceStartup;
        energy = 0;
        damage = 10;
    }

    public void init(Vector3 velDir, GameObject caster, float _energy)
    {
        vel = velDir * speed;
        owner = caster;
        this.energy = _energy;
    }

    // Update is called once per frame
    void Update()
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
            Destroy(this.gameObject);
        }
    }
}
