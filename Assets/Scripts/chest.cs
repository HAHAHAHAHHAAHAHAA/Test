using UnityEngine;

public class Chest : Interactor, IInteractable
{
    [SerializeField] private GameObject miniGamePrefab;
    public int money, metal, cloth, pistolAmmo, shotgunAmmo, submachinegunAmmo, rifleAmmo;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator chestHead;
    private Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
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
        playerAnim.Play("Closing");
        chestHead.Play("OpenChest");
    }
    private void HandleLose()
    {
        Debug.Log("поражение");
        player.enabled = true;
    }
}
