using UnityEngine;
using UnityEngine.SceneManagement;

public class Restartpanel : MonoBehaviour
{
    private Player _player;
    [SerializeField] private GameObject _panel;
    void Start()
    {
        _player = FindFirstObjectByType<Player>();
    }

    public void ShowDiePanel()
    {
        _panel.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
