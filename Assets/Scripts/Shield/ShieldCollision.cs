using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{

    [SerializeField] string[] collisionTags;
    float hitTime;
    Material material;
    public int shieldValue = 100;
    private Collider shieldCollider;

    void Start()
    {
        if (GetComponent<Renderer>())
        {
            material = GetComponent<Renderer>().sharedMaterial;
        }

    }

    void Update()
    {

        if (hitTime > 0)
        {
            float myTime = Time.fixedDeltaTime * 1000;
            hitTime -= myTime;
            if (hitTime < 0)
            {
                hitTime = 0;
            }
            material.SetFloat("_HitTime", hitTime);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // this is the original code from package
        //for (int i = 0; i < collisionTags.Length; i++)
        //{

        //    if (collisionTags.Length > 0 || collision.transform.CompareTag(collisionTags[i]))
        //    {
        //        //Debug.Log("hit");
        //        ContactPoint[] _contacts = collision.contacts;
        //        for (int i2 = 0; i2 < _contacts.Length; i2++)
        //        {
        //            mat.SetVector("_HitPosition", transform.InverseTransformPoint(_contacts[i2].point));
        //            hitTime = 500;
        //            mat.SetFloat("_HitTime", hitTime);
        //        }
        //    }
        //}
        if (gameObject.layer == collision.gameObject.layer) return;

        foreach (string tag in collisionTags) 
        {
            if (collision.transform.CompareTag(tag))
            {
                ContactPoint[] contacts = collision.contacts;
                foreach (ContactPoint contact in contacts)
                {
                    material.SetVector("_HitPosition", transform.InverseTransformPoint(contact.point));
                    hitTime = 500;
                    material.SetFloat("_HitTime", hitTime);
                }
                break;
            }
        }
    }

    public void TakeDamage (int damage)
    {
        shieldValue -= damage;
        if (shieldValue <= 0)
        {
            shieldValue = 0;
            DeactivateShield();
        }
    }

    void DeactivateShield()
    {
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false; // 禁用护盾的Collider
        }
        gameObject.SetActive(false);
    }
}

