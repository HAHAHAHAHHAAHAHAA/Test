using System.Collections;
using UnityEngine;

public class TriggerLVL1 : MonoBehaviour
{
    [SerializeField] Animator mut1,mut2;
    [SerializeField] Animator plyr;
    [SerializeField] Player player;
    [SerializeField] CameraFollow cameraf;
    [SerializeField] GameObject gameobj;
    [SerializeField] Vector3 startPosition; // ��������� �������
    [SerializeField] Vector3 endPosition;   // �������� �������
    [SerializeField] Quaternion startRotation; // ��������� �������
    [SerializeField] Quaternion endRotation;   // �������� �������
    [SerializeField] float duration = 5f;      // ����������������� �����������

    [SerializeField] GameObject gameobjC;
    [SerializeField] Vector3 startPositionC; // ��������� �������
    [SerializeField] Vector3 endPositionC;   // �������� �������
    [SerializeField] Quaternion startRotationC; // ��������� �������
    [SerializeField] Quaternion endRotationC;   // �������� �������
    [SerializeField] float durationC = 5f;      // ����������������� �����������
    private float elapsedTime = 0f;  // ��������� �����
    private float elapsedTimeC = 0f;
    private int davaidelai = 0;
    private int davaidelaiC = 0;
    void Start()
    {
        // ������������� ��������� ������� � �������
        gameobj.transform.position = startPosition;
        gameobj.transform.rotation = startRotation;
    }

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
        }

        if (elapsedTimeC < durationC && davaidelaiC == 1)
        {
            // ����������� ��������� �����
            elapsedTimeC += Time.deltaTime;

            // ��������� ���������������� �����������
            float t = Mathf.Clamp01(elapsedTimeC / durationC);

            // ������ ���������� ������
            gameobjC.transform.position = Vector3.Lerp(startPositionC, endPositionC, t);

            // ������ ������������ ������
            gameobjC.transform.rotation = Quaternion.Slerp(startRotationC, endRotationC, t);
        }
        if (elapsedTimeC < durationC && davaidelaiC == 2)
        {
            // ����������� ��������� �����
            elapsedTimeC += Time.deltaTime;

            // ��������� ���������������� �����������
            float t = Mathf.Clamp01(elapsedTimeC / durationC);

            // ������ ���������� ������
            gameobjC.transform.position = Vector3.Lerp(endPositionC, startPositionC, t);

            // ������ ������������ ������
            gameobjC.transform.rotation = Quaternion.Slerp(endRotationC, startRotationC, t);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.enabled = false;
            cameraf.enabled = false;
            startPositionC = gameobjC.transform.position;
            startRotationC = gameobjC.transform.rotation;
            StartCoroutine(Camera());
            this.gameObject.GetComponent<Collider>().enabled = false;
        } 
    }
    IEnumerator Camera()
    {
        plyr.SetBool("Run",false);
        davaidelai = 1;
        yield return new WaitForSeconds(1f);
        davaidelaiC = 1;
        yield return new WaitForSeconds(.1f);
        mut1.Play("Scream");
        yield return new WaitForSeconds(.3f);
        mut2.Play("Scream");
        yield return new WaitForSeconds(1.6f);
        elapsedTimeC = 0;
        davaidelaiC = 2;
        yield return new WaitForSeconds(1.1f);
        mut1.SetBool("Scream", false);
        mut2.SetBool("Scream", false);
        cameraf.enabled = true;
        player.enabled = true;
        Destroy(this.gameObject);
    }
}
