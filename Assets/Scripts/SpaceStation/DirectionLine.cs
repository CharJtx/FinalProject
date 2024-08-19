using UnityEngine;

public class DirectionLine : MonoBehaviour
{
    public Transform player; // ��ҵ�λ��
    public Transform target; // Ŀ����λ��
    public Transform target2;
    private LineRenderer lineRenderer;
    private Transform nowTarget;
    public static DirectionLine instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        player = PlayerController.instance.transform;



        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found.");
            return;
        }

        // �����߶ο��
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // �����߶β��ʺ���ɫ����ѡ��
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        nowTarget = target;
    }

    void Update()
    {
        if (player != null && target != null)
        {
            // �����߶ε������յ�
            lineRenderer.SetPosition(0, player.position); // �������Ϊ���λ��
            lineRenderer.SetPosition(1, nowTarget.position); // �յ�����ΪĿ��λ��
        }
    }

    public void ChangeTarget()
    {
        nowTarget = target2;
    }
}
