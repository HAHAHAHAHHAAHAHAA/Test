using UnityEngine;

public class NeedKillEnemies : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private GameObject Door;
    [SerializeField] private float moveSpeed = 1.0f;
    private bool shouldMove = false;
    [SerializeField] GameObject leverb;
    [SerializeField] GameObject leverpre;
    [SerializeField] GameObject leverpost;
    void Start()
    {
        Door.transform.position = startPosition;
    }

    void Update()
    {
        if (!shouldMove && CheckEnemiesDead())
        {
            shouldMove = true;
        }

        if (shouldMove)
        {
            float step = moveSpeed * Time.deltaTime;
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, endPosition, step);
            leverb.SetActive(true);
            leverpre.SetActive(false);
            leverpost.SetActive(true);
        }
    }

    private bool CheckEnemiesDead()
    {
        if (enemies == null) return false;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy.dead) return false;
        }

        return true;
    }
}