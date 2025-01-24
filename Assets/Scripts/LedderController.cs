using UnityEngine;
using System.Collections;

public class LedderController : Interactor, IInteractable
{
    public float moveSpeed = 5f;
    public Transform downPoint;     // Точка начала лестницы
    public Transform upPoint;       // Точка конца лестницы
    public float stepDistance = 1f; // Расстояние шага вперед после подъема

    private bool _isClimbing = false;
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody _playerRb;
    private bool _isClimbingUp;          // Перемещается ли персонаж наверх
    public void Start()
    {
        _playerRb=_player.GetComponent<Rigidbody>();
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

            while (Vector3.Distance(_player.position, targetPosition) > 0.1f)
            {
                _playerRb.useGravity = false;
                _player.position = Vector3.MoveTowards(_player.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // Добавляем шаг вперед после подъема
            Debug.Log($"Before Translate Up: _player.position: {_player.position}, transform.forward: {transform.forward}");
            _player.Translate(transform.right * -stepDistance);
            Debug.Log($"After Translate Up: _player.position: {_player.position}, transform.forward: {transform.forward}");


        }
        else
        {
            targetPosition = downPoint.position;
            while (Vector3.Distance(_player.position, targetPosition) > 0.1f)
            {
                _player.position = Vector3.MoveTowards(_player.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        _isClimbing = false;
        _isClimbingUp = false;
        _playerRb.useGravity = true;
    }
    public void Interact()
    {
        Debug.Log("Interact called");

        // Поворот персонажа к лестнице только по оси Y
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // Игнорируем Y-компоненту
        _player.rotation = Quaternion.LookRotation(forwardDirection);
        StartCoroutine(ClimbCoroutine());
    }
}