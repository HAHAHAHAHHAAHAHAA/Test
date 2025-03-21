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
    private bool gamePlayed = true;
    Vector3[] corners;
    Vector3 mincorner;
    Vector3 maxcorner;
    void Update()
    {
        if (gamePlayed == false) {return;}
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
        Vector3 dotScreenPosition = dot.transform.localPosition;
        //Vector3 dotScreenPosition = Camera.main.WorldToScreenPoint(dotWorldPosition);
        if (dotScreenPosition.x > mincorner.x && dotScreenPosition.x < maxcorner.x && dotScreenPosition.y > mincorner.y && dotScreenPosition.y < maxcorner.y)
        {
            Debug.Log("workwork");
        }
        Debug.Log(dotScreenPosition);
        gamePlayed = false;
    }
    void Start()
    {
        corners = new Vector3[4];
        mincorner = new Vector2(100000f, 100000f);
        maxcorner = new Vector2(-100000f, -100000f);
        winZone.GetLocalCorners(corners);
        Debug.Log("Screen Corners:");
        for (int i = 0; i < 4; i++)
        {
            if (corners[i].x < mincorner.x)
            {
                mincorner.x = corners[i].x;
            }
            if (corners[i].y < mincorner.y)
            {
                mincorner.y = corners[i].y;
            }
            if (corners[i].x > maxcorner.x)
            {
                maxcorner.x = corners[i].x;
            }
            if (corners[i].y > maxcorner.y)
            {
                maxcorner.y = corners[i].y;
            }
            //Debug.Log("Corner " + i + ": " + Camera.main.WorldToScreenPoint(corners[i]));
            Debug.Log("_____________________________________________");
            Debug.Log(maxcorner.x);
            Debug.Log(mincorner.x);
        }
    }
}
