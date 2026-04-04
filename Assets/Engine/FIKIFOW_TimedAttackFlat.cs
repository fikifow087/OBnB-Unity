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

    // Variabel baru untuk menyimpan posisi target yang sudah diacak
    private float targetPosX = 0f;

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

        // --- FITUR RANDOMIZER ZONA ---
        // Hitung batas maksimal agar zona Okelah tidak tembus ke luar bar Hitam
        float okelahHalfWidth = (okelahBar.rect.width * okelahBar.localScale.x) / 2f;
        
        float minX = leftLimit + okelahHalfWidth;
        float maxX = rightLimit - okelahHalfWidth;
        
        // Acak posisi target di antara minX dan maxX
        targetPosX = Random.Range(minX, maxX);

        // Geser visual UI Okelah dan Perfect ke posisi yang baru diacak
        okelahBar.anchoredPosition = new Vector2(targetPosX, okelahBar.anchoredPosition.y);
        perfectBar.anchoredPosition = new Vector2(targetPosX, perfectBar.anchoredPosition.y);
        // -----------------------------

        direction = 1; 
        ResetCursor();
        isActive = true;

        Debug.Log("Timed Attack Dimulai! Target Posisi X: " + targetPosX);
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
        
        // PERUBAHAN PENTING: Hitung jarak dari kursor ke TARGET (bukan ke 0 lagi)
        float distanceFromTarget = Mathf.Abs(currentX - targetPosX);

        float perfectHalfWidth = (perfectBar.rect.width * perfectBar.localScale.x) / 2f;
        float okelahHalfWidth = (okelahBar.rect.width * okelahBar.localScale.x) / 2f;

        if (distanceFromTarget <= perfectHalfWidth)
        {
            Debug.Log("<color=green>Perfect!</color>"); 
            onPerfect.Invoke();
        }
        else if (distanceFromTarget <= okelahHalfWidth)
        {
            Debug.Log("<color=yellow>Okelah!</color>"); 
            onOkelah.Invoke();
        }
        else
        {
            Debug.Log("<color=red>Miss!</color>"); 
            onMiss.Invoke();
        }
    }
}