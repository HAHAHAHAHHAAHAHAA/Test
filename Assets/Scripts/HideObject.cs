using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    [SerializeField] GameObject gmobj;
    [SerializeField] GameObject gmjbj2;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gmobj.SetActive(false);
            gmjbj2.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gmobj.SetActive(true);
            gmjbj2.SetActive(true);
        }
    }
}
