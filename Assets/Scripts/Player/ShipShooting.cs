using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // �ӵ�Ԥ�Ƽ�
    public Transform bulletSpawnPoint; // �ӵ�����λ��
    public float shootRate = 0.5f; // �������
    private float shootCooldown;
    private int bulletDamagePercent = 0;
    private int bulletDamageValue = 0;
    private int bulletShieldDamagePercent = 0;
    private int bulletShieldDamageValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && shootCooldown <= 0f) // �����������Ĭ������������Ctrl����
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        BulletBase bulletBase = bullet.GetComponent<BulletBase>();
        bulletBase.IncreaseBulletDamagebyPercentage(bulletDamagePercent);
        bulletBase.IncreaseBulletDamagebyValue(bulletDamageValue);

        shootCooldown = shootRate;
    }

    public void IncreaseBulletDamagebyPercentage(int percentage)
    {
        bulletDamagePercent += percentage;
    }

    public void DecreaseBulletDamagebyPercentage(int value) 
    {
        bulletDamageValue += value;
    }
}
