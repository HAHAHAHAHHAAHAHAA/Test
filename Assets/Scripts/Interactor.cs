using TMPro;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    private GameObject interactableObject;
    [SerializeField] private GameObject UI_interactButton;
    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void InteractionMoment()
    {
        if(interactableObject.TryGetComponent(out IInteractable interactableObjectInst))
        {
            interactableObjectInst.Interact();
            InteractionOut();
        }
    }
    public void RecourceGet(int money, int metal, int cloth, int pistolAmmo, int shotgunAmmo, int submachinegunAmmo, int rifleAmmo)
    {
        player.money += money;
        player.metal += metal;
        player.cloth += cloth;
        player.pistolAmmo += pistolAmmo;
        player.shotgunAmmo += shotgunAmmo;
        player.submachineAmmo += submachinegunAmmo;
        player.rifleAmmo += rifleAmmo;
    }
    public void InteractionOut()
    {
        UI_interactButton.SetActive(false);
        interactableObject = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            UI_interactButton.SetActive(true);
            interactableObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            InteractionOut();
        }
    }
}