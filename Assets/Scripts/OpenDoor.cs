using UnityEngine;

public class OpenDoor : MonoBehaviour, IInteractable
{
    private bool isDoorOpen = false; // ��������� �����

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
        // ����� ����� �������� ��� ��� �������� �������� �����, ���� ��� ����������
    }

    private void Close()
    {
        isDoorOpen = false;
        print("DoorClosed");
        // ����� ����� �������� ��� ��� �������� �������� �����, ���� ��� ����������
    }
}