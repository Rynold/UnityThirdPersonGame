using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour 
{
    GameManager gm;

    private float maxHealth;
    public float currentHealth;

    List<PlayerBuff> buffs;

    public GameObject fireBall;
    UnityEngine.UI.Text healthText;
	
	void Start () 
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        healthText = GameObject.Find("Health Text").GetComponent<UnityEngine.UI.Text>();
        healthText.text = "Health: " + currentHealth;
	}
	
	
	void Update () 
    {
	    
	}


    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireSpell();
        }
    }

    void FireSpell()
    {
        GameObject spell = Instantiate(fireBall,
                                       transform.position + transform.forward * 2,
                                       Quaternion.LookRotation(transform.forward,transform.up)) as GameObject;

        spell.GetComponent<FireBallScript>().init(transform.forward, this.gameObject);
    }

    void OnCollisionEnter(Collision collider)
    {
        switch(collider.transform.tag){
            case "Damages Player" :
                TakeDamae(10.0f);
                break;
        }
    }

    public void TakeDamae(float damage)
    {
        currentHealth -= damage;
        healthText.text = "Health: " + currentHealth;
    }
}
