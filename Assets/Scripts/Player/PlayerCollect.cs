using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public LayerMask itemMask;
    public float collectRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CollectNearByItems();

    }

    public void CollectNearByItems()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, collectRadius, itemMask);

        foreach (Collider c in nearby) 
        {
            ItemBase item = c.GetComponent<ItemBase>();
            if (item != null)
            {
                item.StartFlyingToPlayer(transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
