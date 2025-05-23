using UnityEngine;

public class PickItem : Interactor, IInteractable
{
    [SerializeField] private int money;
    [SerializeField] private int metal;
    [SerializeField] private int cloth;
    [SerializeField] private int pistolAmmo;
    [SerializeField] private int shotgunAmmo;
    [SerializeField] private int submachinegunAmmo;
    [SerializeField] private int rifleAmmo;
    [SerializeField] private AudioSource Player;
    [SerializeField] AudioClip pickupSound;
    public void Interact()
    {
        print("Item picked");
        RecourceGet(money, metal, cloth, pistolAmmo, shotgunAmmo, submachinegunAmmo, rifleAmmo);
        Player.PlayOneShot(pickupSound);
        Destroy(this.gameObject);
    }
}
