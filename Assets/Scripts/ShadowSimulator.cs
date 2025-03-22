using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    public GameObject shadowPrefab;
    public float shadowOffset = 0.1f;
    public LayerMask lightSourceLayer;
    public float rotationSpeedMultiplier = 1.0f;
    public float maxShadowWidth = 2.0f;
    public float minShadowWidth = 0.5f;
    public float maxShadowLength = 0.5f;
    public float minShadowLength = 2.0f;
    public float maxShadowOpacity = 1.0f;
    public float minShadowOpacity = 0.2f;
    public float distanceForMinOpacity = 10f;

    private Dictionary<Transform, GameObject> shadows = new Dictionary<Transform, GameObject>(); // Словарь для хранения теней.

    void OnTriggerEnter(Collider other)
    {
        if ((lightSourceLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Transform lightSource = other.transform;

            // Если для этого источника света еще нет тени, создаем её.
            if (!shadows.ContainsKey(lightSource))
            {
                SpawnShadow(lightSource);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((lightSourceLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Transform lightSource = other.transform;

            // Если для этого источника света есть тень, уничтожаем её.
            if (shadows.ContainsKey(lightSource))
            {
                Destroy(shadows[lightSource]);
                shadows.Remove(lightSource);
            }
        }
    }

    void Update()
    {
        // Перебираем все активные тени и обновляем их.
        foreach (var shadowEntry in shadows)
        {
            Transform lightSource = shadowEntry.Key;
            GameObject shadow = shadowEntry.Value;

            // Убедимся, что источник света все еще существует.
            if (lightSource == null || shadow == null)
            {
                // Источник света был уничтожен, удаляем тень из словаря и уничтожаем её.
                Destroy(shadow);
                shadows.Remove(lightSource);
                continue; // Переходим к следующей тени.
            }

            // Располагаем тень под персонажем.
            shadow.transform.position = transform.position + Vector3.down * shadowOffset;

            // Вычисляем вектор от источника света к персонажу.
            Vector3 lightToPlayer = (transform.position - lightSource.position).normalized;

            // Вычисляем угол поворота, чтобы тень смотрела в противоположном направлении.
            float angle = Mathf.Atan2(lightToPlayer.x, lightToPlayer.z) * Mathf.Rad2Deg;

            // Поворачиваем тень.
            shadow.transform.rotation = Quaternion.Euler(90, angle - 90, 0);

            // Вращаем тень
            shadow.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeedMultiplier * angle);

            // Вычисляем расстояние до источника света.
            float distanceToLight = Vector3.Distance(transform.position, lightSource.position);

            // Вычисляем ширину и длину тени в зависимости от расстояния.
            float shadowWidth = Mathf.Lerp(maxShadowWidth, minShadowWidth, distanceToLight / distanceForMinOpacity);
            float shadowLength = Mathf.Lerp(maxShadowLength, minShadowLength, distanceToLight / distanceForMinOpacity);

            // Применяем масштаб к тени.
            shadow.transform.localScale = new Vector3(shadowLength, shadowWidth, 1); // Y должен быть shadowWidth

            // Вычисляем прозрачность тени в зависимости от расстояния.
            float opacity = Mathf.Lerp(maxShadowOpacity, minShadowOpacity, distanceToLight / distanceForMinOpacity);

            // Получаем Renderer и Material тени.
            Renderer shadowRenderer = shadow.GetComponent<Renderer>();
            Material shadowMaterial = shadowRenderer.material; // Важно: убедитесь, что материал поддерживает прозрачность!

            // Устанавливаем прозрачность материала тени.
            Color color = shadowMaterial.color;
            color.a = opacity;
            shadowMaterial.color = color;
        }
    }

    void SpawnShadow(Transform lightSource)
    {
        // Создаем экземпляр префаба тени.
        GameObject shadow = Instantiate(shadowPrefab, transform.position + Vector3.down * shadowOffset, Quaternion.identity);

        // Задаем имя тени
        shadow.name = "Shadow for " + gameObject.name + " from " + lightSource.name;

        // Добавляем тень в словарь.
        shadows.Add(lightSource, shadow);
    }
}