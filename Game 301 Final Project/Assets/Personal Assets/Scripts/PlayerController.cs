using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour 
{
    GameManager gm;
    Animator animator;
    Rigidbody body;
    GameObject mainCamera;
    Transform rightHandLocation;

    private float maxHealth;
    public float currentHealth;
    private float maxMana;
    public float currentMana;
    private float maxEnergy;
    public float currentEnergy;

    public float speed;
    float acceleration;
    Vector3 velocity;
    Vector3 lastVel;

    public GameObject fireBall;
    UnityEngine.UI.Text healthText;
    
	
	void Start () 
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        body = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera");

        maxHealth = 100;
        currentHealth = maxHealth;
        maxMana = 100;
        currentMana = maxMana;
        maxEnergy = 100;
        currentEnergy = 0.0f;
        speed = 5.0f;
        acceleration = 0.2f;
        lastVel = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = new Vector3(0.0f, 0.0f, 0.0f);

        healthText = GameObject.Find("Health Text").GetComponent<UnityEngine.UI.Text>();
        healthText.text = "Health: " + currentHealth;
        rightHandLocation = GameObject.Find("Right Hand Spell").transform;
	}
	
	
	void Update() 
    {
        
	}

    void FixedUpdate()
    {
        HandleInput();
        UpdateAnimator();
    }

    void HandleInput()
    {

        float LH = Input.GetAxis("Horizontal");
        float LV = Input.GetAxis("Vertical");
        float RH = Input.GetAxis("RightH");
        float RV = Input.GetAxis("RightV");

        Vector3 m_CamForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        velocity = LV * m_CamForward + LH * mainCamera.transform.right;
        Vector3 lookDirection = RV * m_CamForward + RH * mainCamera.transform.right;

        //velocity.Normalize();
        velocity *= speed;
        body.velocity = new Vector3(velocity.x, body.velocity.y, velocity.z);
        lastVel = body.velocity;

        if (RH != 0 || RV != 0)
        {
            transform.LookAt(transform.position + lookDirection);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("Attacking", true);
        }
    }

    void FireSpell()
    {
            GameObject spell = Instantiate(fireBall,
                                           rightHandLocation.position,
                                           Quaternion.LookRotation(transform.forward, transform.up)) as GameObject;

            //spell.GetComponent<FireBallScript>().init(transform.forward, this.gameObject);
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

    void UpdateAnimator()
    {
        /* Movement */
        if (velocity.Equals(new Vector3(0.0f,0.0f,0.0f)) == false)
        {
            Vector3 facing = transform.forward;

            /* Gets the vector which describes the characters relative velocity to the direction it is facing
             * If the velocity is moving towards the right, make sure the angle is negative. */
            float angle = Vector3.Angle(facing, velocity);
            if (velocity.x < 0)
                angle = -angle;
            Vector3 animationDirection = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(0, 0, 1);

            animator.SetFloat("VelocityX", animationDirection.x);
            animator.SetFloat("VelocityZ", animationDirection.z);
        }
        else
        {
            animator.SetFloat("VelocityX", 0.0f);
            animator.SetFloat("VelocityZ", 0.0f);
        }


    }
}
