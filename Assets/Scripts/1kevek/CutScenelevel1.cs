using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutScenelevel1 : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] Player player;
    private bool started=false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&started==false)
        {
            StartCoroutine(Cutscene());
            started = false;
            this.GetComponent<Collider>().enabled = false;
        }
    }
    IEnumerator Cutscene()
    {
        player.enabled = false;
        director.Play();
        yield return new WaitForSeconds(4.85f);
        director.Stop();
        player.enabled = true;
        Destroy(this.gameObject,1f);

    }
}
