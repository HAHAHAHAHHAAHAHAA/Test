using System.Collections;
using UnityEngine;

public class Obuchenie : MonoBehaviour
{
    public GameObject panel;
    public GameObject button;
    void Start()
    {
        StartCoroutine(TutorialC());
    }

    IEnumerator TutorialC()
    {
        yield return new WaitForSeconds(2f);
        panel.SetActive(true);
        yield return new WaitForSeconds(5f);
        button.SetActive(true);
    }
    public void Delete()
    {
        panel.SetActive(false);
        Destroy(this.gameObject, 1f);
    }
}
