using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // �ӵ�Ԥ�Ƽ�
    public Transform bulletSpawnPoint; // �ӵ�����λ��
    public float shootRate = 0.5f; // �������
    public int firePowerLevel = 1;
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
        float angleStep = 90f / (firePowerLevel + 1);
        float startAngle = -45f;

        for (int i = 0; i < firePowerLevel; i++)
        {
            float currentAngle = startAngle + ((i+1) * angleStep);
            Quaternion bulletRotation = Quaternion.Euler(0, currentAngle, 0) * bulletSpawnPoint.rotation;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
            BulletBase bulletBase = bullet.GetComponent<BulletBase>();
            bulletBase.IncreaseBulletDamagebyPercentage(bulletDamagePercent);
            bulletBase.IncreaseBulletDamagebyValue(bulletDamageValue);

        }

        

        shootCooldown = shootRate;
    }

    public void IncreaseBulletDamagebyPercentage(int percentage)
    {
        bulletDamagePercent += percentage;
    }

    public void IecreaseBulletDamagebyPercentage(int value) 
    {
        bulletDamageValue += value;
    }

    public void IncreaseFirePowerLever(int value)
    {
        firePowerLevel += value;
    }
}
