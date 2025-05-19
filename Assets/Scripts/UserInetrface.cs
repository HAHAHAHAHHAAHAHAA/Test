using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInetrface : MonoBehaviour
{
    [SerializeField] private GameObject openButton;
    [SerializeField] private TextMeshProUGUI metal;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private TextMeshProUGUI cloth;
    [SerializeField] private GameObject panel;
    private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
        Time.timeScale = 1;
    }
    public void OpenMenu()
    {
        Time.timeScale = 0;
        openButton.SetActive(false);
        panel.SetActive(true);
        metal.text = "metal:" + player.metal.ToString();
        money.text = "money:" + player.money.ToString();
        cloth.text = "cloth:" + player.cloth.ToString();
    }
    public void CloseMenu()
    {
        panel.SetActive(false);
        openButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void ExitToMainMenu()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
}
