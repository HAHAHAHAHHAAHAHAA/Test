using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Collider colider;
    [SerializeField] string RunType;
    [SerializeField] Animator animator;
    private Player player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool TakingTimeToNextBite;
    [SerializeField] private int health = 15;
    [SerializeField] private int biteDamage = 5;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    private int deathtype;

    private bool dead = false;
    private void Start()
    {
        deathtype = Random.Range(1, 3);
        player = FindObjectOfType<Player>();
        agent.updatePosition = false;
    }
    private void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, player.playerPos);

        if (dist < 22f&&!dead)
        {
            // Устанавливаем цель для NavMeshAgent
            agent.SetDestination(player.playerPos);

            // Получаем следующую позицию агента
            Vector3 targetPosition = agent.nextPosition;

            // Используем SmoothDamp для плавного перемещения
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            animator.SetBool(RunType,true);
        }
        else
        {
            animator.SetBool(RunType, false);
        }
        if (dist <= 1.2f&&!TakingTimeToNextBite&&!dead)
        {
            StartCoroutine(Bite());
        }
        if (health < 0)
        {
            colider.enabled = false;
            dead = true;
            animator.SetInteger("Death", deathtype);
            Destroy(this.gameObject,4f);
        }
    }
    public void Damage(int dmg)
    {
        health -= dmg;
    }

    IEnumerator Bite()
    {
        TakingTimeToNextBite = true;
        player.Damage(biteDamage);
        yield return new WaitForSeconds(2f);
        TakingTimeToNextBite=false;
    }
}
