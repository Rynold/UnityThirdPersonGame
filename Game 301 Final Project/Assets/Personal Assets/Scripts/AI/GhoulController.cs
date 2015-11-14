using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class GhoulController : MonoBehaviour {

    Animator animator;
    NavMeshAgent agent;
    Rigidbody body;
    public Transform playerCharacter;
    public bool attacking;
    public bool isInAir;
    float JumpVel;

    TrailRenderer[] handTrails;
    ParticleSystem[] bloodSystems;

	// Use this for initialization
	void Start () 
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        playerCharacter = GameObject.Find("PlayerCharacter").transform;
        handTrails = GetComponentsInChildren<TrailRenderer>();
        bloodSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (TrailRenderer trail in handTrails)
            trail.enabled = false;
        foreach (ParticleSystem system in bloodSystems)
            system.Stop();

        agent.stoppingDistance = 1.0f;
        attacking = false;
        isInAir = false;
        JumpVel = 5.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateAnimator();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            if (!attacking)
            {
                if (agent.destination != playerCharacter.position)
                    agent.SetDestination(playerCharacter.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                    Attack();
            }
        }
	}

    void Jump()
    {
        if (!isInAir)
        {
            isInAir = true;
            animator.SetBool("InAir", true);
            agent.Stop();
            body.useGravity = true;
            body.AddRelativeForce(new Vector3(0, JumpVel, 100), ForceMode.Impulse);
        }
    }

    void UpdateAnimator()
    {
        if (agent.velocity.magnitude > 0)
        {
            Vector3 facing = transform.forward;
            Vector3 velocity = agent.velocity;

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

    void OnCollisionEnter(Collision c)
    {
        if (c.transform.name == "FireBall(Clone)")
            TakeDamage();
    }

    void TakeDamage()
    {
        Destroy(this.gameObject);
    }

    void Attack()
    {
        attacking = true;
        animator.SetBool("Attacking", true);
    }

    void DealDamage()
    {
        Debug.Log("Dealing Damage");

        foreach (ParticleSystem system in bloodSystems)
            system.Play();

        playerCharacter.GetComponent<PlayerController>().TakeDamae(10.0f);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 0.1f));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.1f))
        {
            isInAir = false;
            agent.SetDestination(playerCharacter.position);
            body.useGravity = false;
            animator.SetBool("InAir", false);
        }
    }

    void StartHandTrails()
    {
        foreach (TrailRenderer trail in handTrails)
        {
            trail.enabled = true;
        }
    }

    void StopHandTrails()
    {
        foreach (TrailRenderer trail in handTrails)
        {
            trail.enabled = false;
        }
    }
}
