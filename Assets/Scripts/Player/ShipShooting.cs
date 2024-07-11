using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // �ӵ�Ԥ�Ƽ�
    public Transform bulletSpawnPoint; // �ӵ�����λ��
    public float shootRate = 0.5f; // �������
    private float shootCooldown;

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
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        shootCooldown = shootRate;
    }
}
