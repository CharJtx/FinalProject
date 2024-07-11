using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    void Update()
    {
        Move();
        Turn();
    }

    void Move()
    {
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World); // ÒÆ¶¯·É´¬
    }

    void Turn() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float turn = horizontal * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }
}
