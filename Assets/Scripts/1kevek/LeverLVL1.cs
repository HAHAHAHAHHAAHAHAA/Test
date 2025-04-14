using System.Collections;
using UnityEngine;

public class LeverLVL1 : Interactor, IInteractable
{
    [SerializeField] GameObject lever1;
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
    [SerializeField] float durationC = 5f;
    [SerializeField] AudioSource DoorAudio;
    [SerializeField] AudioSource LeverAudio;
    private float elapsedTime = 0f;  // ��������� �����
    private float elapsedTimeC = 0f;
    private int davaidelai = 0;
    private int davaidelaiC = 0;
    void Start()
    {
       gameobj.transform.position = startPosition;
       gameobj.transform.rotation = startRotation;
    }

    void Update()
    {
        if (elapsedTime < duration && davaidelai==1)
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
    public void Interact()
    {
        player.enabled = false;
        cameraf.enabled = false;
        startPositionC=gameobjC.transform.position;
        startRotationC=gameobjC.transform.rotation;
        StartCoroutine(Camera());
        this.gameObject.GetComponent<Collider>().enabled = false;
        
    }
    IEnumerator Camera()
    {
        LeverAudio.Play();
        davaidelaiC = 1;
        yield return new WaitForSeconds(2f);
        DoorAudio.Play();
        davaidelai = 1;
        yield return new WaitForSeconds(2f);
        elapsedTimeC = 0;
        davaidelaiC = 2;
        yield return new WaitForSeconds(2.1f);
        cameraf.enabled = true;
        player.enabled = true;
        lever1.SetActive(true);
        Destroy(this.gameObject);
    }
}