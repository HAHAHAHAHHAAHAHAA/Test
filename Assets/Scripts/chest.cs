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
        playerAnim.Play("Closing");
        chestHead.Play("OpenChest");
        player.StartCoroutine(player.StopPlayer(2));
    }
    private void HandleLose()
    {
        Debug.Log("поражение");
        player.StartCoroutine(player.StopPlayer(2));
    }
}
