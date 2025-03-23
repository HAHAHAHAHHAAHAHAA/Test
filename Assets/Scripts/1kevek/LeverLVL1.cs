using System.Collections;
using UnityEngine;

public class LeverLVL1 : Interactor, IInteractable
{
    [SerializeField] GameObject lever1;
    [SerializeField] Player player;
    [SerializeField] CameraFollow cameraf;
    [SerializeField] GameObject gameobj;
    [SerializeField] Vector3 startPosition; // Начальная позиция
    [SerializeField] Vector3 endPosition;   // Конечная позиция
    [SerializeField] Quaternion startRotation; // Начальный поворот
    [SerializeField] Quaternion endRotation;   // Конечный поворот
    [SerializeField] float duration = 5f;      // Продолжительность перемещения

    [SerializeField] GameObject gameobjC;
    [SerializeField] Vector3 startPositionC; // Начальная позиция
    [SerializeField] Vector3 endPositionC;   // Конечная позиция
    [SerializeField] Quaternion startRotationC; // Начальный поворот
    [SerializeField] Quaternion endRotationC;   // Конечный поворот
    [SerializeField] float durationC = 5f;      // Продолжительность перемещения
    private float elapsedTime = 0f;  // Прошедшее время
    private float elapsedTimeC = 0f;
    private int davaidelai = 0;
    private int davaidelaiC = 0;
    void Start()
    {
        // Устанавливаем начальные позицию и поворот
       gameobj.transform.position = startPosition;
        gameobj.transform.rotation = startRotation;
    }

    void Update()
    {
        if (elapsedTime < duration && davaidelai==1)
        {
            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;

            // Вычисляем интерполяционный коэффициент
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Плавно перемещаем объект
            gameobj.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Плавно поворачиваем объект
            gameobj.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
        }
       
        if (elapsedTimeC < durationC && davaidelaiC == 1)
        {
            // Увеличиваем прошедшее время
            elapsedTimeC += Time.deltaTime;

            // Вычисляем интерполяционный коэффициент
            float t = Mathf.Clamp01(elapsedTimeC / durationC);

            // Плавно перемещаем объект
            gameobjC.transform.position = Vector3.Lerp(startPositionC, endPositionC, t);

            // Плавно поворачиваем объект
            gameobjC.transform.rotation = Quaternion.Slerp(startRotationC, endRotationC, t);
        }
        if (elapsedTimeC < durationC && davaidelaiC == 2)
        {
            // Увеличиваем прошедшее время
            elapsedTimeC += Time.deltaTime;

            // Вычисляем интерполяционный коэффициент
            float t = Mathf.Clamp01(elapsedTimeC / durationC);

            // Плавно перемещаем объект
            gameobjC.transform.position = Vector3.Lerp(endPositionC, startPositionC, t);

            // Плавно поворачиваем объект
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
        davaidelaiC = 1;
        yield return new WaitForSeconds(2f);
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