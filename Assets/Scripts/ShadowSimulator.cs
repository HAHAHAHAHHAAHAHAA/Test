using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public GameObject shadowPrefab;
    public LayerMask lightSourceLayer;
    public LayerMask groundLayer;
    public float maxDistance = 5f;

    public float maxShadowWidth = 0.5f;  // Максимальная ширина тени
    public float maxShadowLength = 1.5f; // Максимальная длина тени

    public float shadowYOffset = 0.01f;

    private GameObject _currentShadow;

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & lightSourceLayer) != 0)
        {
            CreateShadow(other.transform);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & lightSourceLayer) != 0 && _currentShadow != null)
        {
            UpdateShadow(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & lightSourceLayer) != 0 && _currentShadow != null)
        {
            Destroy(_currentShadow);
            _currentShadow = null;
        }
    }

    private void CreateShadow(Transform lightSource)
    {
        _currentShadow = Instantiate(shadowPrefab, Vector3.zero, Quaternion.identity);
        UpdateShadow(lightSource);
    }

    private void UpdateShadow(Transform lightSource)
    {
        float distance = Vector3.Distance(transform.position, lightSource.position);
        distance = Mathf.Clamp(distance, 0f, maxDistance);

        // Рассчитываем масштаб для ширины и длины независимо
        float shadowWidth = Mathf.Lerp(maxShadowWidth, 0.1f, distance / maxDistance);
        float shadowLength = Mathf.Lerp(0.5f, maxShadowLength, distance / maxDistance); // Длина растет больше

        float opacity = Mathf.Lerp(1f, 0f, distance / maxDistance);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            _currentShadow.transform.position = hit.point + Vector3.up * shadowYOffset;

            Vector3 lightDirection = (transform.position - lightSource.position).normalized;
            Vector3 shadowDirection = new Vector3(lightDirection.x, 0, lightDirection.z).normalized;

            //Длина тени до позиции на земле
            //float offsetLength = Mathf.Lerp(0.0f, 0.5f, distance / maxDistance);
            _currentShadow.transform.position += shadowDirection * 0.05f;

            _currentShadow.transform.rotation = Quaternion.LookRotation(-shadowDirection);
        }

        // Применяем масштаб по отдельным осям
        _currentShadow.transform.localScale = new Vector3(shadowWidth, 0.001f, shadowLength);

        Renderer rend = _currentShadow.GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            Color color = rend.material.color;
            color.a = opacity;
            rend.material.color = color;
        }
    }
}