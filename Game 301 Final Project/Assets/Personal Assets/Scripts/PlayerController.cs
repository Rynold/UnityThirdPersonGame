using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : NetworkBehaviour 
{
    GameManager gm;
    Animator animator;
    Rigidbody body;
    GameObject mainCamera;
    Transform rightHandLocation;
    Vector3 spellSpawnPosition;

    private float maxHealth;
    public float currentHealth;
    private float maxMana;
    public float currentMana;
    private float maxEnergy;
    public float currentEnergy;
    public float burnedEnergy;

    float manaRechargeTimer;
    float timeLastAbilityUse;
    

    enum SpellType
    {
        Fire,
        Water
    }

    public float speed;
    float acceleration;
    Vector3 velocity;
    Vector3 lastVel;

    public GameObject fireBall;
    public GameObject waterStrike;
    UnityEngine.UI.Text healthText;
    UnityEngine.UI.Text manaText;
    UnityEngine.UI.Text energyText;
    UnityEngine.UI.Text burnedEnergyText;
    SpellType currentSpellType;
    
	
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
        currentEnergy = 100.0f;
        burnedEnergy = 0.0f;

        manaRechargeTimer = 2.0f;
        timeLastAbilityUse = 0.0f;

        speed = 5.0f;
        acceleration = 0.2f;
        lastVel = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = new Vector3(0.0f, 0.0f, 0.0f);

        healthText = GameObject.Find("Health Text").GetComponent<UnityEngine.UI.Text>();
        healthText.text = "Health: " + currentHealth;
        manaText = GameObject.Find("Mana Text").GetComponent<UnityEngine.UI.Text>();
        manaText.text = "Mana: " + currentMana;
        energyText = GameObject.Find("Energy Text").GetComponent<UnityEngine.UI.Text>();
        energyText.text = "Energy: " + currentEnergy;
        burnedEnergyText = GameObject.Find("Burned Energy Text").GetComponent<UnityEngine.UI.Text>();
        burnedEnergyText.text = "Energy: " + currentEnergy;

        //rightHandLocation = GameObject.Find("Right Hand Spell").transform;
        //rightHandLocation = transform.Find("Right Hand Spell");

        rightHandLocation = GameObject.Find("Right Hand Spell").transform;
        currentSpellType = SpellType.Fire;
	}
	
	
	void Update() 
    {
        
	}

    void FixedUpdate()
    {
        HandleInput();
        UpdateAnimator();


        if (CheckManaRecharge())
        {
            currentMana += 0.25f;
            manaText.text = "Mana: " + Mathf.Floor(currentMana);
        }
        if (burnedEnergy > 2)
            rightHandLocation.GetComponent<ParticleSystem>().emissionRate = burnedEnergy;
        else
            rightHandLocation.GetComponent<ParticleSystem>().emissionRate = 2;
    }

    bool CheckManaRecharge()
    {
        if (currentMana >= maxMana)
            return false;
        else if (timeLastAbilityUse + manaRechargeTimer <= Time.timeSinceLevelLoad)
            return true;
        else
            return false;
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
        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            Charge();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("Fire Attack", true);
            currentSpellType = SpellType.Fire;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            animator.SetBool("Ice Attack", true);
            currentSpellType = SpellType.Water;
        }
    }

    void Charge()
    {
        if (currentEnergy > 0.0)
        {
            if(burnedEnergy < currentEnergy)
                burnedEnergy += 0.25f;

            burnedEnergyText.text = "Burned: " + Mathf.Floor(burnedEnergy);
        }
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        energyText.text = "Energy: " + currentEnergy;
    }

    
    void FireSpell()
    {
        Debug.Log("fire spell");
        if (currentMana >= 10)
        {
            Debug.Log("mana fine");
            currentMana -= 10;
            manaText.text = "Mana: " + Mathf.Floor(currentMana);

            GameObject spell;

            spellSpawnPosition = transform.position;
            spellSpawnPosition.y += 1;

            switch (currentSpellType)
            {
                case SpellType.Fire:
                    {
                        Debug.Log("type fire");
                        CmdSpawnFireBall(spellSpawnPosition);
                        //spell = Instantiate(fireBall,
                        //                    spellSpawnPosition,//rightHandLocation.position,
                        //                    Quaternion.LookRotation(transform.forward, transform.up)) as GameObject;
                        ////Debug.Log("should have spawned");

                        //spell.GetComponent<FireBallScript>().init(transform.forward, this.gameObject, burnedEnergy);
                        //NetworkServer.Spawn(spell);
                    }
                    break;
                case SpellType.Water:
                    {
                        Debug.Log("type water");
                        spell = Instantiate(waterStrike,
                                            spellSpawnPosition,//rightHandLocation.position,
                                            Quaternion.LookRotation(transform.forward, transform.up)) as GameObject;

                        spell.GetComponent<FireBallScript>().init(transform.forward, this.gameObject, burnedEnergy);
                        NetworkServer.Spawn(spell);
                    }
                    break;
            }

            Debug.Log(burnedEnergy);
            currentEnergy -= burnedEnergy;
            burnedEnergy = 0.0f;
            energyText.text = "Energy: " + Mathf.Floor(currentEnergy);
            burnedEnergyText.text = "Burned: " + Mathf.Floor(burnedEnergy);
            timeLastAbilityUse = Time.timeSinceLevelLoad;
        }
    }

    [Command]
    void CmdSpawnFireBall(Vector3 pos)
    {
        GameObject spell;
        spell = Instantiate(fireBall,
                                            pos,//rightHandLocation.position,
                                            Quaternion.LookRotation(transform.forward, transform.up)) as GameObject;
        //Debug.Log("should have spawned");

        spell.GetComponent<FireBallScript>().init(transform.forward, this.gameObject, burnedEnergy);
        NetworkServer.Spawn(spell);
    }

    void OnCollisionEnter(Collision collider)
    {
        switch(collider.transform.tag){
            case "Damages Player" :
                TakeDamage(10.0f,collider.gameObject);
                break;
        }
    }

    public void TakeDamage(float damage, GameObject owner)
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

    public void AddHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
    }
}
