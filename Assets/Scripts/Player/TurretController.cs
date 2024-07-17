using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform turret;  // ��Ҫ�����������������̨
    public float groundHeight = 0f;  // ���������������еĸ߶�

    void Update()
    {
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mouseScreenPosition = Input.mousePosition;

        // ����Ļ����ת��Ϊ�������꣬ʹ�õ���Ĺ̶��߶�
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.y - groundHeight));
        //mouseWorldPosition.y = turret.position.y;  // ������̨��Y�᲻��

        // ������ָ̨��Ŀ��λ�õķ���
        Vector3 direction = mouseWorldPosition - turret.position;

        // ����Ŀ����ת�Ƕ�
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ��ȡ��ǰ��ŷ������תֵ
        Vector3 currentEulerAngles = turret.rotation.eulerAngles;

        // ֻ�ı�Y�����ת������X���Z�᲻��
        Vector3 newEulerAngles = new Vector3(currentEulerAngles.x, targetRotation.eulerAngles.x, targetRotation.eulerAngles.y +90);

        // Ӧ���µ���ת
        turret.rotation = Quaternion.Euler(newEulerAngles);
    }
}
