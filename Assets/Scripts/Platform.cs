using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] points; // �����, � ������� ����� ��������� ���������
    public float[] waitTimes; // ����� �������� �� ������ �����
    public float speed = 2f; // �������� �������� ���������

    private int currentPointIndex = 0;
    private bool isMoving = true;
    private float waitTimeStart;

    void Start()
    {
        // ���������, ���� �� ���� �� ���� �����
        if (points.Length == 0 || points.Length != waitTimes.Length)
        {
            Debug.LogError("���������� �������� ���� �� ���� ����� � ����� �������� ��� ������ �����.");
            return;
        }

        // �������� �������� � ������ �����
        MoveToNextPoint();
    }

    void Update()
    {
        if (isMoving)
        {
            // ���������� ��������� � ������� �����
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);

            // ���� ��������� �������� ������� �����
            if (transform.position == points[currentPointIndex].position)
            {
                // ���� ���� ����� ��������, ���������������
                if (waitTimes[currentPointIndex] > 0)
                {
                    if (Time.time - waitTimeStart >= waitTimes[currentPointIndex])
                    {
                        MoveToNextPoint();
                    }
                }
                else
                {
                    MoveToNextPoint();
                }
            }
        }
    }

    // ����������� � ��������� �����
    void MoveToNextPoint()
    {
        // ����������� ������ ������� �����
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        // ��������� ����� ��������
        waitTimeStart = Time.time;

        // ���� ���� ��� �����, �������� �������� � ���������
        if (isMoving)
        {
            isMoving = false;
            Invoke("StartMoving", waitTimes[currentPointIndex]);
        }
    }

    // ������ �������� � ��������� �����
    void StartMoving()
    {
        isMoving = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
