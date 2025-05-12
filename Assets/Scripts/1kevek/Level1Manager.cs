using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] GameObject gameobj;
    [SerializeField] Vector3 startPosition; // ��������� �������
    [SerializeField] Vector3 endPosition;   // �������� �������
    [SerializeField] Quaternion startRotation; // ��������� �������
    [SerializeField] Quaternion endRotation;   // �������� �������
    [SerializeField] float duration = 2f;      // ����������������� �����������
    private float elapsedTime = 0f;  // ��������� �����
    private int davaidelai = 0;

    [SerializeField] GameObject leverb;
    [SerializeField] GameObject leverpre;
    [SerializeField] GameObject leverpost;
    void Update()
    {
        if (elapsedTime < duration && davaidelai == 1)
        {
            // ����������� ��������� �����
            elapsedTime += Time.deltaTime;

            // ��������� ���������������� �����������
            float t = Mathf.Clamp01(elapsedTime / duration);

            // ������ ���������� ������
            gameobj.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // ������ ������������ ������
            gameobj.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            leverb.SetActive(true);
            leverpre.SetActive(false);
            leverpost.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            davaidelai = 1;
        }
    }
}
