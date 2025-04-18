using UnityEngine;
using UnityEngine.SceneManagement;

public class NexLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
