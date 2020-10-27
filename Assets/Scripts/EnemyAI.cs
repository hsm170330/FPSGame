using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Level01Controller Level01Controller;

    public float damage = 10f;
    public float range = 100f;
    public float health = 60f;
    public float shootForce, upwardForce;

    public Transform attackPoint;

    [SerializeField] AudioClip gun01 = null;
    [SerializeField] AudioClip gun02 = null;
    [SerializeField] AudioClip gun03 = null;

    public ParticleSystem muzzleFlash, blood;
    public Camera fpsCam;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject bullet;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("First Person Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            PlayAudio();
            Rigidbody rb = Instantiate(bullet, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            rb.AddForce(transform.up * upwardForce, ForceMode.Impulse);
            //if (muzzleFlash != null)
            //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            muzzleFlash.Play();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void PlayAudio()
    {
        int s = Random.Range(0, 3);
        if (s == 0)
        {
            AudioManager.PlayClip2D(gun01, 1);
        }
        else if (s == 1)
        {
            AudioManager.PlayClip2D(gun02, 1);
        }
        else if (s == 2)
        {
            AudioManager.PlayClip2D(gun03, 1);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        blood.Play();
        if (health <= 0) Invoke(nameof(DestroyEnemy), .1f);
    }
    private void DestroyEnemy()
    {
        Level01Controller.IncreaseScore(10);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage Enemy"))
        {
            TakeDamage(20);
            Debug.Log("Enemy took damage");
            Level01Controller.IncreaseScore(5);
        }

    }
}
