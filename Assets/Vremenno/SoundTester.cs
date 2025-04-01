using UnityEngine;

public class SoundTester : MonoBehaviour
{
    [SerializeField] private AudioSource f1;
    [SerializeField] private AudioSource f2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            f1.Play();
            f2.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            f2.Play();
            f1.Stop();
        }
    }
    public void fi()
    {
        f1.Play();
        f2.Stop();
    }
    public void se()
    {
        f2.Play();
        f1.Stop();
    }
}
