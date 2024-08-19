using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public float flySpeed = 5f;
    private Transform playerTransform;
    private bool isFlyingToPlayer = false;
    public int itemType = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlyingToPlayer && playerTransform != null)
        {
            //flySpeed = playerTransform.gameObject.GetComponent<ShipController>
            float step = flySpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < 1f)
            {
                Collect(playerTransform);
            }
        }
    }

    public void StartFlyingToPlayer(Transform player)
    {
        playerTransform = player;
        flySpeed = player.GetComponent<PlayerController>().getSpeed() * 2;
        isFlyingToPlayer = true;
    }

    private void Collect(Transform player)
    {
        Debug.Log("Collected");
        switch (itemType)
        {
            case 1001:
                ExpItem exp = gameObject.GetComponent<ExpItem>();
                if (exp != null)
                {
                    exp.ExpGain(player);
                }
                break;
        }
        Destroy(gameObject);

    }
}
