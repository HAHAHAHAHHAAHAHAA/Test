using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] GameObject gameobj;
    [SerializeField] Vector3 startPosition; // Начальная позиция
    [SerializeField] Vector3 endPosition;   // Конечная позиция
    [SerializeField] Quaternion startRotation; // Начальный поворот
    [SerializeField] Quaternion endRotation;   // Конечный поворот
    [SerializeField] float duration = 2f;      // Продолжительность перемещения
    private float elapsedTime = 0f;  // Прошедшее время
    private int davaidelai = 0;

    [SerializeField] GameObject leverb;
    [SerializeField] GameObject leverpre;
    [SerializeField] GameObject leverpost;
    void Update()
    {
        if (elapsedTime < duration && davaidelai == 1)
        {
            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;

            // Вычисляем интерполяционный коэффициент
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Плавно перемещаем объект
            gameobj.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Плавно поворачиваем объект
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
