using UnityEngine;

public class Chest : Interactor, IInteractable
{
    [SerializeField] private GameObject miniGamePrefab;
    public int money, metal, cloth, pistolAmmo, shotgunAmmo, submachinegunAmmo, rifleAmmo;
    Player player;
    void Start()
    {
        Player player = FindObjectOfType<Player>();
    }
    public void Interact()
    {
        GameObject miniGameInstance = Instantiate(miniGamePrefab);
        ChestMiniGame miniGame = miniGameInstance.GetComponent<ChestMiniGame>();
        player.enabled = false;
        if (miniGame != null)
        {
            miniGame.OnWin += HandleWin;
            miniGame.OnLose += HandleLose;
        }
    }
    private void HandleWin()
    {
        Debug.Log("Победа");
        player.enabled = true;
    }
    private void HandleLose()
    {
        Debug.Log("поражение");
        player.enabled = true;
    }
}
