using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem; 

public class FIKIFOW_TimedAttackFlat : MonoBehaviour
{
    [Header("UI Objects")]
    public RectTransform missBar;       
    public RectTransform okelahBar;     
    public RectTransform perfectBar;    
    public RectTransform cursor;        

    [Header("Settings")]
    public float speed = 500f;          

    [Header("Events")]
    public UnityEvent onPerfect;
    public UnityEvent onOkelah;
    public UnityEvent onMiss;

    private bool isActive = false;
    private float leftLimit;
    private float rightLimit;
    private float startingY;
    private int direction = 1; 

    void Start()
    {
        startingY = cursor.anchoredPosition.y;
        ResetCursor();
    }

    public void OnClickStart()
    {
        float halfWidth = missBar.rect.width / 2f;
        leftLimit = -halfWidth;
        rightLimit = halfWidth;

        direction = 1; 
        ResetCursor();
        isActive = true;

        // Log saat Timed Attack baru mulai
        Debug.Log("Timed Attack Dimulai!");
    }

    private void ResetCursor()
    {
        cursor.anchoredPosition = new Vector2(leftLimit, startingY);
    }

    void Update()
    {
        if (!isActive) return;

        float moveAmount = speed * Time.deltaTime * direction;
        float nextX = cursor.anchoredPosition.x + moveAmount;

        if (nextX >= rightLimit)
        {
            nextX = rightLimit;
            direction = -1; 
        }
        else if (nextX <= leftLimit)
        {
            nextX = leftLimit;
            direction = 1; 
        }

        cursor.anchoredPosition = new Vector2(nextX, startingY);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckResult(nextX);
        }
    }

    void CheckResult(float currentX)
    {
        isActive = false;
        float distanceFromCenter = Mathf.Abs(currentX);

        float perfectHalfWidth = (perfectBar.rect.width * perfectBar.localScale.x) / 2f;
        float okelahHalfWidth = (okelahBar.rect.width * okelahBar.localScale.x) / 2f;

        if (distanceFromCenter <= perfectHalfWidth)
        {
            Debug.Log("<color=green>Perfect!</color>"); // Log Perfect dengan warna
            onPerfect.Invoke();
        }
        else if (distanceFromCenter <= okelahHalfWidth)
        {
            Debug.Log("<color=yellow>Okelah!</color>"); // Log Okelah dengan warna
            onOkelah.Invoke();
        }
        else
        {
            Debug.Log("<color=red>Miss!</color>"); // Log Miss dengan warna
            onMiss.Invoke();
        }
    }
}