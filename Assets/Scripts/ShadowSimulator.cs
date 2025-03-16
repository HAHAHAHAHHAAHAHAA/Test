using UnityEngine;

public class ShadowSimulator : MonoBehaviour
{
    public Transform lightSource; // Объект А (источник "света")
    public Transform shadowSprite; // Спрайт тени
    public float maxShadowLength = 2f; // Максимальная длина тени
    public float shadowOffset = 0.1f; // Смещение тени от объекта
    public float heightDifferenceFactor = 0.5f; // Влияние разницы высот

    private Vector3 initialScale;
    private float initialY;

    void Start()
    {
        // Сохраняем начальные параметры
        initialScale = shadowSprite.localScale;
        initialY = shadowSprite.position.y;
    }

    void Update()
    {
        if (lightSource == null || shadowSprite == null) return;

        // Рассчитываем направление к источнику света
        Vector3 directionToLight = lightSource.position - transform.position;
        directionToLight.y = 0; // Игнорируем разницу высот по Y

        // Поворачиваем тень в противоположную сторону
        float angle = Mathf.Atan2(directionToLight.z, directionToLight.x) * Mathf.Rad2Deg;
        shadowSprite.rotation = Quaternion.Euler(90, -angle + 90, 0);

        // Рассчитываем расстояние до источника света
        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(lightSource.position.x, 0, lightSource.position.z)
        );

        // Учитываем разницу высот
        float heightDifference = Mathf.Abs(lightSource.position.y - transform.position.y);

        // Масштабируем тень
        float scaleFactor = Mathf.Clamp((distance + heightDifference * heightDifferenceFactor) * 0.5f, 0.1f, maxShadowLength);
        shadowSprite.localScale = new Vector3(initialScale.x, initialScale.y * scaleFactor, initialScale.z);

        // Позиционируем тень
        Vector3 newPos = transform.position;
        newPos.y = initialY; // Сохраняем исходную Y-позицию
        shadowSprite.position = newPos + (-new Vector3(directionToLight.x, 0, directionToLight.z)).normalized * shadowOffset;
    }
}