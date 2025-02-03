using UnityEngine;
using static ActionScript;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;


public class Bridge : BaseAction
{
    public float rotationSpeed = 2f;
    private int count = 0;
    public NavMeshSurface navMeshSurface; // Добавили переменную для NavMeshSurface
    private bool isCoroutineStarted = false; // Флаг для предотвращения запуска корутины несколько раз.
    public override void ExecuteAction()
    {
        count++;
    }

    private void FixedUpdate()
    {
        if (count == 1)
        {
            float currentAngle = transform.eulerAngles.x;
            float newAngle = Mathf.LerpAngle(currentAngle, -45, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if (count == 2)
        {
            if (!isCoroutineStarted)
            {
                StartCoroutine(RebuildNavMeshWithDelay()); // Запускаем корутину
                isCoroutineStarted = true;
            }
            float currentAngle = transform.eulerAngles.x;
            float newAngle = Mathf.LerpAngle(currentAngle, -91f, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);

        }
    }

    IEnumerator RebuildNavMeshWithDelay()
    {
        yield return new WaitForSeconds(3f); // ждем 3 секунды
        RebuildNavMesh(); // перестраиваем навмеш
        enabled = false; // отключаем скрипт
    }


    private void RebuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh перестроен!");
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
