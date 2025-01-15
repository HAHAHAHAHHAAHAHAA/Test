using UnityEngine;
using static ActionScript;

public class Bridge : BaseAction
{ // ����, � �������� ����� �����������
    public float rotationSpeed = 2f; // �������� ��������
    private int count = 0;
    public override void ExecuteAction()
    {
        count++;
    }
    private void Update()
    {
        if (count == 1)
        {
            float currentAngle = transform.eulerAngles.x;

            // ������ ������������� ����
            float newAngle = Mathf.LerpAngle(currentAngle, -45, rotationSpeed * Time.deltaTime);

            // ��������� ����� ���� � �������
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if(count == 2)
        {
            float currentAngle = transform.eulerAngles.x;

            // ������ ������������� ����
            float newAngle = Mathf.LerpAngle(currentAngle, -91f, rotationSpeed * Time.deltaTime);

            // ��������� ����� ���� � �������
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
