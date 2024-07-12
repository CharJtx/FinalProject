using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        
    }

    void Idle()
    {
        //agent.ResetPath();
    }
}
