using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{

    [SerializeField] string[] collisionTags;
    float hitTime;
    Material material;
    public int shieldValue = 100;
    public int currentValue = 0;
    private Collider shieldCollider;
    public int recoverTime = 5;

    void Start()
    {
        if (GetComponent<Renderer>())
        {
            material = GetComponent<Renderer>().sharedMaterial;
        }
        currentValue = shieldValue;

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
        currentValue -= damage;
        if (currentValue <= 0)
        {
            currentValue = 0;
            DeactivateShield();
        }
    }

    void DeactivateShield()
    {
        GameManager.Instance.StartShieldReactivationCoroutine(ReactiveShieldAfterDelay());
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false; // 禁用护盾的Collider
        }
        gameObject.SetActive(false);

        
    }

    public void IncreaseMaxShieldbyPercent(int percent)
    {
        float percentage = percent / 100;
        shieldValue = (int) Math.Floor(percentage*shieldValue);
    }

    public void IncreaseMaxShieldbyValue(int value)
    {
        shieldValue += value;
    }

    private IEnumerator ReactiveShieldAfterDelay()
    {
        yield return new WaitForSeconds(recoverTime);

        if (gameObject != null)
        {
            gameObject.SetActive(true);
            if (shieldCollider != null)
            {
                shieldCollider.enabled = true;
            }

            currentValue = shieldValue;
        }
        
    }

    public int GetMaxCurrentShieldValue(int value)
    {
        return shieldValue;
    }

    public int GetCurrentShieldValue()
    {
        return currentValue;
    }
}

