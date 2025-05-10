using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;


public class Bridge : BaseAction
{
    private bool hasPlayedClip1 = false;
    private bool hasPlayedClip2 = false;
    public AudioSource m_AudioSource;
    public AudioClip m_Clip;
    public AudioClip m_Clip2;
    public GameObject bridgeControl1, bridgeControl2;
    public GameObject idiotControll;
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
        // Сброс флагов, если состояние изменилось
        if (count != 1) hasPlayedClip1 = false;
        if (count != 2) hasPlayedClip2 = false;

        if (count == 1)
        {
            float currentAngle = transform.eulerAngles.x;
            float newAngle = Mathf.LerpAngle(currentAngle, -25, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);

            // Проигрываем звук только один раз при входе в count == 1
            if (!hasPlayedClip1)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();
                hasPlayedClip1 = true;
            }
        }

        if (count == 2)
        {
            if (!isCoroutineStarted)
            {
                StartCoroutine(RebuildNavMeshWithDelay());
                isCoroutineStarted = true;
            }

            float currentAngle = transform.eulerAngles.x;
            float newAngle = Mathf.LerpAngle(currentAngle, -111f, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);

            // Проигрываем звук только один раз при входе в count == 2
            if (!hasPlayedClip2)
            {
                m_AudioSource.clip = m_Clip2;
                m_AudioSource.Play();
                hasPlayedClip2 = true;
            }

            StartCoroutine(Col());
        }
    }
    IEnumerator Col()
    {
        yield return new WaitForSeconds(0.8f);
        idiotControll.SetActive(false);
        bridgeControl1.SetActive(true);
        bridgeControl2.SetActive(true);
        Destroy(this.gameObject.GetComponent<Bridge>());
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
}