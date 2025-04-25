using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Start()
    {
        
    }
    public void NewGame()
    {
        SceneLoader.Instance.LoadScene("Sewers");
    }
    public void Countinue()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }

}
