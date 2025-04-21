using System.Collections;
using System.Collections.Generic;
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
    private bool dead = false;
    [SerializeField] private int damageDelay;

    bool triggered = false;
    private bool _agredByDamage;
    private Coroutine _damageAgrCoroutine;
    [SerializeField] private float _agroRadius;
    [SerializeField] private float _agroTime;


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
        if (dist > spotRange*2.6 && !dead && !_agredByDamage)
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
        AggroNearbyEnemies();
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
    IEnumerator DamageAgr(float time)
    {
        _agredByDamage = true;
        triggered = true;
        yield return new WaitForSeconds(2);
        Debug.Log("huynya");
        yield return new WaitForSeconds(time);
        _agredByDamage = false;
    }

    private void AggroNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _agroRadius);
        List<Enemy> nearbyEnemies = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && !enemy.dead)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        // Запускаем агр у всех найденных врагов
        foreach (Enemy enemy in nearbyEnemies)
        {
            // Если корутина уже запущена, останавливаем её
            if (enemy._damageAgrCoroutine != null)
            {
                enemy.StopCoroutine(enemy._damageAgrCoroutine);
            }

            // Запускаем корутину и сохраняем ссылку
            enemy._damageAgrCoroutine = enemy.StartCoroutine(enemy.DamageAgr(_agroTime));
        }
    }
}
