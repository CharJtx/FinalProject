using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public Vector2 minBounds = new Vector2(-50, -50); // 活动范围的最小边界
    public Vector2 maxBounds = new Vector2(50, 50); // 活动范围的最大边界

    public CameraFollow cameraFollow;

    public static PlayerController instance;

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

    private void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.target = transform;
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
        transform.Translate(moveDirection, Space.World); // 移动飞船
    }

    void Turn() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float turn = horizontal * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }

    void LimitPosition()
    {
        // 获取当前飞船位置
        Vector3 position = transform.position;

        // 限制飞船位置在指定范围内
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.z = Mathf.Clamp(position.z, minBounds.y, maxBounds.y);

        // 应用新的位置
        transform.position = position;
    }

    public void IncreaseSpeedByValue(float speed)
    {
        moveSpeed += speed;
    }

    public void IncreaseTurnSpeedByValue(float speed)
    {
        turnSpeed += speed;
    }
}
