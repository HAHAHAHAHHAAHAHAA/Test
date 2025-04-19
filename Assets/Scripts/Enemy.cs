using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioSource deadsound;
    [SerializeField] ParticleSystem _particleSys;
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
    [SerializeField] int spotRange;
    [SerializeField] bool triggered=false;
    private bool dead = false;
    [SerializeField] private int damageDelay;
    [SerializeField] Rig rig;
    private void Start()
    {
        deathtype = Random.Range(1, 3);
        player = FindObjectOfType<Player>();
        agent.updatePosition = false;
    }
    private void FixedUpdate()
    {
        if (rig.weight > 0)
        {
            rig.weight -= 0.02f;
        }
        if (dead) return;
        float dist = Vector3.Distance(transform.position, player.playerPos);
        if(dist < spotRange && !dead)
        {
            triggered = true;
        }
        if (dist > spotRange*2.6 && !dead)
        {
            triggered = false;
        }
        if (triggered && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            agent.isStopped = false;
            // ������������� ���� ��� NavMeshAgent
            agent.SetDestination(player.playerPos);

            // �������� ��������� ������� ������
            Vector3 targetPosition = agent.nextPosition;

            // ���������� SmoothDamp ��� �������� �����������
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            animator.SetBool(RunType,true);
        }
        else
        {
            animator.SetBool(RunType, false);
            agent.isStopped = true;
        }
        if (dist <= 1.2f&&!TakingTimeToNextBite&&!dead)
        {
            StartCoroutine(Bite());
        }
        if (health < 0)
        {
            if (!dead)
            {
                deadsound.Play();
                _particleSys.Play();
            }
            colider.enabled = false;
            dead = true;
            animator.SetInteger("Death", deathtype);
            Destroy(this.gameObject,4f);
        }
    }
    public void Damage(int dmg)
    {
        health -= dmg;
        rig.weight = 0.5f;
    }

    IEnumerator Bite()
    {
        animator.Play("Attack");
        TakingTimeToNextBite = true;
        yield return new WaitForSeconds(damageDelay);
        player.Damage(biteDamage);
        yield return new WaitForSeconds(2f);
        TakingTimeToNextBite=false;
    }
}
