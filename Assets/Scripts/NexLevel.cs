using UnityEngine;
using UnityEngine.SceneManagement;

public class NexLevel : MonoBehaviour
{
    [SerializeField] private string lvlName;
    private void OnTriggerEnter(Collider other)
    {
        if (SceneLoader.Instance == null)
        {
            new GameObject("SceneLoader").AddComponent<SceneLoader>();
        }
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadScene(lvlName);
        }
    }
}
