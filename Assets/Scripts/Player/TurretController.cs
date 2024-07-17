using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform turret;  // 需要跟随鼠标调整方向的炮台
    public float groundHeight = 0f;  // 地面在世界坐标中的高度

    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 将屏幕坐标转换为世界坐标，使用地面的固定高度
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.y - groundHeight));
        //mouseWorldPosition.y = turret.position.y;  // 保持炮台的Y轴不变

        // 计算炮台指向目标位置的方向
        Vector3 direction = mouseWorldPosition - turret.position;

        // 计算目标旋转角度
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 获取当前的欧拉角旋转值
        Vector3 currentEulerAngles = turret.rotation.eulerAngles;

        // 只改变Y轴的旋转，保持X轴和Z轴不变
        Vector3 newEulerAngles = new Vector3(currentEulerAngles.x, targetRotation.eulerAngles.x, targetRotation.eulerAngles.y +90);

        // 应用新的旋转
        turret.rotation = Quaternion.Euler(newEulerAngles);
    }
}
