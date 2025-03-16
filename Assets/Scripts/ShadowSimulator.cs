using UnityEngine;

public class ShadowSimulator : MonoBehaviour
{
    public Transform lightSource; // ������ � (�������� "�����")
    public Transform shadowSprite; // ������ ����
    public float maxShadowLength = 2f; // ������������ ����� ����
    public float shadowOffset = 0.1f; // �������� ���� �� �������
    public float heightDifferenceFactor = 0.5f; // ������� ������� �����

    private Vector3 initialScale;
    private float initialY;

    void Start()
    {
        // ��������� ��������� ���������
        initialScale = shadowSprite.localScale;
        initialY = shadowSprite.position.y;
    }

    void Update()
    {
        if (lightSource == null || shadowSprite == null) return;

        // ������������ ����������� � ��������� �����
        Vector3 directionToLight = lightSource.position - transform.position;
        directionToLight.y = 0; // ���������� ������� ����� �� Y

        // ������������ ���� � ��������������� �������
        float angle = Mathf.Atan2(directionToLight.z, directionToLight.x) * Mathf.Rad2Deg;
        shadowSprite.rotation = Quaternion.Euler(90, -angle + 90, 0);

        // ������������ ���������� �� ��������� �����
        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(lightSource.position.x, 0, lightSource.position.z)
        );

        // ��������� ������� �����
        float heightDifference = Mathf.Abs(lightSource.position.y - transform.position.y);

        // ������������ ����
        float scaleFactor = Mathf.Clamp((distance + heightDifference * heightDifferenceFactor) * 0.5f, 0.1f, maxShadowLength);
        shadowSprite.localScale = new Vector3(initialScale.x, initialScale.y * scaleFactor, initialScale.z);

        // ������������� ����
        Vector3 newPos = transform.position;
        newPos.y = initialY; // ��������� �������� Y-�������
        shadowSprite.position = newPos + (-new Vector3(directionToLight.x, 0, directionToLight.z)).normalized * shadowOffset;
    }
}