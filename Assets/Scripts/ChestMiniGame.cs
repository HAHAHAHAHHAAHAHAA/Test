using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class ChestMiniGame : MonoBehaviour
{
    [SerializeField] private RectTransform dot;
    [SerializeField] private RectTransform winZone;
    [SerializeField] private RectTransform semiWinZone;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject toDestroy;
    private bool movingRight = true;
    private bool gamePlayed = true;
    private int points = 0;
    public int winPoints;

    public delegate void WinEvent();
    public event WinEvent OnWin;
    public event WinEvent OnLose;

    void Update()
    {
        if (!gamePlayed) return;

        float distance = Vector2.Distance(left.anchoredPosition, right.anchoredPosition);
        float currentPosition = Vector2.Distance(dot.anchoredPosition, left.anchoredPosition);
        float normalizedPosition = currentPosition / distance;
        float speedMultiplier = Mathf.Sin(normalizedPosition * Mathf.PI) + 0.1f;

        if (movingRight)
        {
            dot.anchoredPosition = Vector2.MoveTowards(dot.anchoredPosition, right.anchoredPosition, speed * speedMultiplier * Time.deltaTime);
            if (Vector2.Distance(dot.anchoredPosition, right.anchoredPosition) < 0.1f)
            {
                movingRight = false;
            }
        }
        else
        {
            dot.anchoredPosition = Vector2.MoveTowards(dot.anchoredPosition, left.anchoredPosition, speed * speedMultiplier * Time.deltaTime);
            if (Vector2.Distance(dot.anchoredPosition, left.anchoredPosition) < 0.1f)
            {
                movingRight = true;
            }
        }
    }

    public void StopGame()
    {
        Vector3 dotWorldPosition = dot.position;

        if (IsPointInsideRect(dotWorldPosition, semiWinZone))
        {
            Debug.LogWarning("Добавил балл");
            points += 1;
        }

        else if (IsPointInsideRect(dotWorldPosition, winZone))
        {
            WinMiniGame();
        }
        else
        {
            LoseMiniGame();
        }

        if (points == winPoints)
        {
            WinMiniGame();
        }
    }

    private bool IsPointInsideRect(Vector3 point, RectTransform rectTransform)
    {
        // Получаем мировые координаты углов RectTransform
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Определяем границы RectTransform в мировых координатах
        float minX = Mathf.Min(corners[0].x, corners[2].x);
        float maxX = Mathf.Max(corners[0].x, corners[2].x);
        float minY = Mathf.Min(corners[0].y, corners[2].y);
        float maxY = Mathf.Max(corners[0].y, corners[2].y);

        // Проверяем, находится ли точка внутри этих границ
        return point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;
    }

    public void WinMiniGame()
    {
        Debug.Log("WIN!!!!!!!!");
        OnWin?.Invoke();
        Destroy(toDestroy);
    }
    public void LoseMiniGame()
    {
        Debug.Log("NU TI LOH");
        OnLose?.Invoke();
        Destroy(toDestroy);
    }
}