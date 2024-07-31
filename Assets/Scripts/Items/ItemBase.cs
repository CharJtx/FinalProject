using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public float flySpeed = 5f;
    private Transform playerTransform;
    private bool isFlyingToPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlyingToPlayer && playerTransform != null)
        {
            float step = flySpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < 1f)
            {
                Collect();
            }
        }
    }

    public void StartFlyingToPlayer(Transform player)
    {
        playerTransform = player;
        isFlyingToPlayer = true;
    }

    private void Collect()
    {
        Debug.Log("Collected");
        Destroy(gameObject);

    }
}
