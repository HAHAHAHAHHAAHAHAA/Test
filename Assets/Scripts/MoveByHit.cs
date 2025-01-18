using UnityEngine;
using System.Collections;
public class MoveByHit : MonoBehaviour
    {
    public float pushForce = 100f; // Сила толчка от выстрела
    [SerializeField] private Rigidbody rb;
    public void PushObject(Vector3 forceDirection)
    {
        if (rb != null)
        {
            rb.AddForce(forceDirection * pushForce, ForceMode.Impulse);
        }
    }
}
