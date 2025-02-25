
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : BulletBase
{
    public float setSpeed = 10f;
    public float turnSpeed = 5f;
    public float detectionRadius = 50f;
    public LayerMask enemyLayer;
    public int setDamage = 50;
    public int setShieldDamange = 50;
    public float setLifeTime = 10f;
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        speed = setSpeed;
        damage = setDamage;
        shieldDamange = setShieldDamange;
        lifeTime = setLifeTime;
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

        // Find all enemies within range
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;

        // find the closest one
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
        // if the collision is not a enemy,then return
        if (! LayerMaskExtensions.IsLayerInLayerMask(collision.gameObject.layer,enemyLayer)) return;
        // find the health interface of enemy
        IHealth hp = collision.gameObject.GetComponent<IHealth>();
        ShieldCollision shieldCollision = collision.gameObject.GetComponent<ShieldCollision>();

        // if health component is null then find health component in its children
        if (hp == null) 
        {
            hp = collision.gameObject.GetComponentInChildren<IHealth>();
        }
        if (hp != null)
        {
            hp.TakeDamage(damage);
            if (explosionSound != null)
            {
                SoundEffectManager.instance.playSoundEffect(explosionSound);
            }
            Destroy(gameObject);
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            }
        }
        // if the collision is not a enemy or a enemy shield
        if (shieldCollision != null) 
        {
            shieldCollision.TakeDamage(damage);
            if (explosionSound != null)
            {
                SoundEffectManager.instance.playSoundEffect(explosionSound);
            }
            Destroy(gameObject);

            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            }
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
