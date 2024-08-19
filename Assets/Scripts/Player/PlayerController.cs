using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public Vector2 minBounds = new Vector2(-50, -50); // ���Χ����С�߽�
    public Vector2 maxBounds = new Vector2(50, 50); // ���Χ�����߽�

    private static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameManager.Instance.RegisterPersistentObject(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    public float getSpeed()
    {
        return moveSpeed;
    }

    void Update()
    {
        Move();
        Turn();
        LimitPosition();
    }

    void Move()
    {
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World); // �ƶ��ɴ�
    }

    void Turn() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float turn = horizontal * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }

    void LimitPosition()
    {
        // ��ȡ��ǰ�ɴ�λ��
        Vector3 position = transform.position;

        // ���Ʒɴ�λ����ָ����Χ��
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.z = Mathf.Clamp(position.z, minBounds.y, maxBounds.y);

        // Ӧ���µ�λ��
        transform.position = position;
    }
}
