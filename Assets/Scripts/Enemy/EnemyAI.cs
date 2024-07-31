using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Attacking, Dead}
    public EnemyState currentState = EnemyState.Patrolling;

    public Transform[] patrolPoints;
    public float chaseDistance = 10f;
    public float attackDistance = 15f;
    public float attackCooldown = 1f;
    public int health = 100;
    public GameObject childObject;
    public float angleThreshold = 3f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private Transform player;
    private float attackTimer = 0f;
    private float initialY; // Initial Y-axis position



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //if (patrolPoints.Length > 0)
        //{
        //    agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        //}

        agent.SetDestination(player.transform.position);
        childObject = transform.GetChild(0).gameObject;

        if (childObject != null)
        {
            initialY = childObject.transform.position.y;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyState.Dead) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Chasing:
                Chase();
                if (distanceToPlayer <= attackDistance)
                {
                    currentState = EnemyState.Attacking;
                }
                else if (distanceToPlayer >= chaseDistance)
                {
                    currentState = EnemyState.Patrolling;
                    agent.SetDestination(player.position);
                }
                break;
            case EnemyState.Attacking:
                Attack();
                if (distanceToPlayer >= attackDistance)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
        }
        attackTimer -= Time.deltaTime;

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        // lock Y-axis position
        Vector3 position = transform.position;
        position.y = player.position.y;
        transform.position = position;

        // lock X-axis and Z-axis rotation
        Quaternion rotation = transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;

        if (childObject != null)
        {
            childObject.transform.rotation = rotation;
        }

        
    }

    void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1)% patrolPoints.Length;
            agent.SetDestination(player.position);
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        if (attackTimer <= 0f & player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            
            Vector3 forward = transform.forward;

            directionToPlayer.y = forward.y;

            float angle = Vector3.Angle(forward, directionToPlayer);
            Debug.DrawLine(transform.position, transform.position + transform.forward * 100, Color.red);
            Debug.DrawLine(transform.position, player.position, Color.green);

            if (angle < angleThreshold)
            {
                Debug.Log("Enemy attack the player!");
                attackTimer = attackCooldown;
            }
            
        }
    }
}
