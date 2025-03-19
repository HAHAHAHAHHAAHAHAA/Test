using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class chest : MonoBehaviour
{
    [SerializeField] private RectTransform dot;
    [SerializeField] private RectTransform winZone;
    [SerializeField] private RectTransform semiWinArea;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    [SerializeField] private float speed = 0.1f;
    private bool movingRight = true;
    private float time = 0;
    void Update()
    {
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
}
