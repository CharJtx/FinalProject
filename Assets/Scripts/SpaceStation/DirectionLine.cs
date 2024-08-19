using UnityEngine;

public class DirectionLine : MonoBehaviour
{
    public Transform player; // ��ҵ�λ��
    public Transform target; // Ŀ����λ��
    public Transform targer2;
    private LineRenderer lineRenderer;

    void Start()
    {
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
    }

    void Update()
    {
        if (player != null && target != null)
        {
            // �����߶ε������յ�
            lineRenderer.SetPosition(0, player.position); // �������Ϊ���λ��
            lineRenderer.SetPosition(1, target.position); // �յ�����ΪĿ��λ��
        }
    }
}
