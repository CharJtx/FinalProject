using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallEnemyHealth : MonoBehaviour,IHealth
{
    public int maxHealth = 100;
    private int currentHealth;
    private GameObject[] explosionPrefabs;
    public AudioClip explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        explosionPrefabs = Resources.LoadAll<GameObject>("Effect/Explosion/Ship");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        DropItemManager dropItemManager = FindObjectOfType<DropItemManager>();
        if (dropItemManager == null)
        {
            Debug.LogError("can not find the DropItemManager");
        }
        else
        {
            dropItemManager.DropItem(transform,0);
        }


        if (EnemySpawner.instance != null)
        {
            EnemySpawner.instance.EnemyDeath(1);
        }

        GameObject explosionEffectPrefab = explosionPrefabs[Random.Range(0, explosionPrefabs.Length)];
        Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        SoundEffectManager.instance.playSoundEffect(explosionClip);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
