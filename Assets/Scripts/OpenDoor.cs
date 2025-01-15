using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteractable
{
    private bool isDoorOpen = false; // Состояние двери

    public void Interact()
    {
        if (isDoorOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        isDoorOpen = true;
        print("DoorOpened");
        // Здесь можно добавить код для анимации открытия двери, если это необходимо
    }

    private void Close()
    {
        isDoorOpen = false;
        print("DoorClosed");
        // Здесь можно добавить код для анимации закрытия двери, если это необходимо
    }
}