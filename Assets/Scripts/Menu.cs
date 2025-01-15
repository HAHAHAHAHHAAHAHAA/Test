using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Start()
    {
        
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Countinue()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }

}
