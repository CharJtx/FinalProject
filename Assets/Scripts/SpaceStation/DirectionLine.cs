using UnityEngine;

public class DirectionLine : MonoBehaviour
{
    public Transform player; // 玩家的位置
    public Transform target; // 目标点的位置
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

        // 设置线段宽度
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // 设置线段材质和颜色（可选）
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (player != null && target != null)
        {
            // 设置线段的起点和终点
            lineRenderer.SetPosition(0, player.position); // 起点设置为玩家位置
            lineRenderer.SetPosition(1, target.position); // 终点设置为目标位置
        }
    }
}
