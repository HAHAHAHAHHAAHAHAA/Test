using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform[] points; // Точки, к которым будет двигаться платформа
    public float[] waitTimes; // Время ожидания на каждой точке
    public float speed = 2f; // Скорость движения платформы

    private int currentPointIndex = 0;
    private bool isMoving = true;
    private float waitTimeStart;

    void Start()
    {
        // Проверяем, есть ли хотя бы одна точка
        if (points.Length == 0 || points.Length != waitTimes.Length)
        {
            Debug.LogError("Необходимо добавить хотя бы одну точку и время ожидания для каждой точки.");
            return;
        }

        // Начинаем движение к первой точке
        MoveToNextPoint();
    }

    void Update()
    {
        if (isMoving)
        {
            // Перемещаем платформу к текущей точке
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);

            // Если платформа достигла текущей точки
            if (transform.position == points[currentPointIndex].position)
            {
                // Если есть время ожидания, останавливаемся
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

    // Перемещение к следующей точке
    void MoveToNextPoint()
    {
        // Увеличиваем индекс текущей точки
        currentPointIndex = (currentPointIndex + 1) % points.Length;

        // Обновляем время ожидания
        waitTimeStart = Time.time;

        // Если есть еще точки, начинаем движение к следующей
        if (isMoving)
        {
            isMoving = false;
            Invoke("StartMoving", waitTimes[currentPointIndex]);
        }
    }

    // Начало движения к следующей точке
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
