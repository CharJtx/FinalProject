using System;
using System.Collections;
using UnityEngine;


public class BulletBase : MonoBehaviour
{
    public float speed = 100f;
    public float lifeTime = 5f;
    public int damage = 10;
    public int shieldDamange = 10;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual string BulletType()
    {
        return "0";
    }

    public virtual void IncreaseBulletDamagebyPercentage(int percentage)
    {
        float increasePercentage = percentage / 100;
        damage = (int)Math.Ceiling(increasePercentage * damage);
    }

    public virtual void IncreaseBulletDamagebyValue(int value)
    {
        damage += value;
    }
}
