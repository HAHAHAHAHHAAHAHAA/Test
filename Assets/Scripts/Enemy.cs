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
    public bool dead = false;

    bool triggered = false;
    private bool _agredByDamage;
    public Coroutine _damageAgrCoroutine;

    [SerializeField] Rig rig;

    [SerializeField] AudioClip[] walkClips;
    [SerializeField] AudioClip[] walkClips2;
    [SerializeField] AudioClip[] walkClips3;
    [SerializeField] AudioSource audioSource;
    private string currentGroundType = "GroundType1";

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
        if (dist <= 1.5f&&!TakingTimeToNextBite&&!dead)
        {
            animator.Play("Attack");
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
        player.AggroNearbyEnemies(transform);
    }

    IEnumerator Bite()
    {
        Debug.Log("Kuskus");
        TakingTimeToNextBite = true;
        player.Damage(biteDamage);
        yield return new WaitForSeconds(2f);
        TakingTimeToNextBite=false;
    }

    public IEnumerator DamageAgr(float time)
    {
        _agredByDamage = true;
        triggered = true;
        yield return new WaitForSeconds(2);
        Debug.Log("huynya");
        yield return new WaitForSeconds(time);
        _agredByDamage = false;
    }
    public void PlayRandomFootstepSound()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if (hit.collider.CompareTag("GroundType1"))
            {
                currentGroundType = "GroundType1";
            }
            else if (hit.collider.CompareTag("GroundType2"))
            {
                currentGroundType = "GroundType2";
            }
            else if (hit.collider.CompareTag("GroundType3"))
            {
                currentGroundType = "GroundType3";
            }
            else
            {
                currentGroundType = null;
            }
        }
        else
        {
            currentGroundType = null;
        }

        AudioClip[] clips = null;

        if (currentGroundType == null) return;

        switch (currentGroundType)
        {
            case "GroundType1":
                clips = walkClips;
                break;
            case "GroundType2":
                clips = walkClips2;
                break;
            case "GroundType3":
                clips = walkClips3;
                break;
            default:
                return;
        }

        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];

            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogError("AudioSource is not assigned to the Enemy script!");
            }
        }
    }
}
