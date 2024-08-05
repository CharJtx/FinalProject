
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : BulletBase
{
    //public float speed = 10f;
    public float turnSpeed = 5f;
    public float detectionRadius = 50f;
    public LayerMask enemyLayer;
    //public int damage = 50;
    //public int shieldDamange = 50;
    //public float lifeTime = 10f;
    public GameObject explosionEffectPrefab;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        damage = 50;
        shieldDamange = 50;
        lifeTime = 10f;
        Destroy(gameObject, lifeTime);
        FindClosestEnemy();
    }

    void FindClosestEnemy()
    {
        //if (transform.gameObject.layer == 6)
        //{
        //    enemyLayer = 8;
        //} else if (transform.gameObject.layer == 8) {
        //    enemyLayer = 6; 
        //}
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Bullet"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = hit.transform;
                }
            }
            
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (! LayerMaskExtensions.IsLayerInLayerMask(collision.gameObject.layer,enemyLayer)) return;
        IHealth hp = collision.gameObject.GetComponent<IHealth>();
        ShieldCollision shieldCollision = collision.gameObject.GetComponent<ShieldCollision>();

        if (hp == null) 
        {
            hp = collision.gameObject.GetComponentInChildren<IHealth>();
        }
        if (hp != null)
        {
            hp.TakeDamage(damage);
            Destroy(gameObject);

        }

        if (shieldCollision != null) 
        {
            shieldCollision.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindClosestEnemy();
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 rotationAmount = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime,0);
            transform.rotation = Quaternion.LookRotation(rotationAmount);
            transform.position += transform.forward * speed * Time.deltaTime;

        }
    }

    public override string BulletType()
    {
        return "1";
    }
   
}
