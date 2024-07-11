using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // 子弹预制件
    public Transform bulletSpawnPoint; // 子弹发射位置
    public float shootRate = 0.5f; // 射击速率
    private float shootCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && shootCooldown <= 0f) // 按下射击键（默认是鼠标左键或Ctrl键）
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        shootCooldown = shootRate;
    }
}
