
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BulletBase
{

    //public float speed = 20f;
    //public float lifeTime = 5f;
    //public int damage = 10;
    //public int shieldDamange = 5;
    public GameObject explosionEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.layer == collision.gameObject.layer) return;

        IHealth health = collision.gameObject.GetComponent<IHealth>();
        ShieldCollision shieldCollision = collision.gameObject.GetComponent<ShieldCollision>();

        if(health == null)
        {
            health = collision.gameObject.GetComponentInChildren<IHealth>();
        }
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        if (shieldCollision != null)
        {
            shieldCollision.TakeDamage(shieldDamange);
            Destroy(gameObject);
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        }
        
    }

    public override void  IncreaseBulletDamagebyPercentage(int percentage)
    {
        float increasePercentage = percentage / 100;
        damage = (int) Math.Ceiling(increasePercentage * damage);
    }

    public override string BulletType()
    {
        return "0";
    }
}
