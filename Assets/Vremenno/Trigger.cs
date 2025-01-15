using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(prefab, new Vector3(gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + 10), Quaternion.identity);
    }
}
