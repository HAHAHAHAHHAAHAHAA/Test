using UnityEngine;
using System.Collections;

public class LedderController : Interactor, IInteractable
{
    public float moveSpeed = 5f;
    public Transform downPoint;     // Точка начала лестницы
    public Transform upPoint;       // Точка конца лестницы
    public float stepDistance = 1f; // Расстояние шага вперед после подъема
    public Player playerController; //Ссылка на скрипт управления игроком

    private bool _isClimbing = false;
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody _playerRb;
    private bool _isClimbingUp;          // Перемещается ли персонаж наверх


    public void Start()
    {
        _playerRb = _player.GetComponent<Rigidbody>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController не назначен!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isClimbing = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isClimbing = false;
        }
    }

    void Update()
    {
        if (_isClimbing && _player != null)
        {
            // Определяем, нужно ли подниматься или спускаться
            if (Vector3.Distance(_player.position, downPoint.position) < Vector3.Distance(_player.position, upPoint.position))
            {
                _isClimbingUp = true;
            }
            else
            {
                _isClimbingUp = false;
            }
        }
    }

    private IEnumerator ClimbCoroutine()
    {
        Debug.Log("Climb Coroutine started");
        Vector3 targetPosition;
        if (_isClimbingUp)
        {
            targetPosition = upPoint.position;
            playerController.enabled = false; //Отключаем управление игроком

            while (Vector3.Distance(_player.position, targetPosition) > 0.1f)
            {
                _playerRb.useGravity = false;
                _player.position = Vector3.MoveTowards(_player.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // Плавное перемещение вперед
            Vector3 finalPosition = _player.position + transform.right * -stepDistance;
            while (Vector3.Distance(_player.position, finalPosition) > 0.1f)
            {
                _player.position = Vector3.MoveTowards(_player.position, finalPosition, moveSpeed/2 * Time.deltaTime);
                yield return null;
            }

        }
        else
        {
            targetPosition = downPoint.position;
            playerController.enabled = false; //Отключаем управление игроком

            while (Vector3.Distance(_player.position, targetPosition) > 0.1f)
            {
                _player.position = Vector3.MoveTowards(_player.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        _isClimbing = false;
        _isClimbingUp = false;
        _playerRb.useGravity = true;
        playerController.enabled = true; //Включаем управление игроком
    }
    public void Interact()
    {
        Debug.Log("Interact called");

        // Поворот персонажа к лестнице только по оси Y
        Vector3 forwardDirection = -transform.right;
        forwardDirection.y = 0; // Игнорируем Y-компоненту
        _player.rotation = Quaternion.LookRotation(forwardDirection);
        StartCoroutine(ClimbCoroutine());
    }
}