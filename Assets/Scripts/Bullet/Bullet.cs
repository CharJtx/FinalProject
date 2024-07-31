using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 10;
    public int shieldDamange = 5;
    public GameObject explosionEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.layer == collision.gameObject.layer) return;

        Health health = collision.gameObject.GetComponent<Health>();
        ShieldCollision shieldCollision = collision.gameObject.GetComponent<ShieldCollision>();

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
}
