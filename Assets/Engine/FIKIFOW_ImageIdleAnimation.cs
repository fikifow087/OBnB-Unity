using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("FIKIFOW/FIKIFOW-Graphic Idle Animation")]
/// <summary>
/// FIKIFOW Idle Animation Component - Port dari RPG Maker MZ plugin
/// Mendukung: Image, RawImage, Text, TextMeshPro UGUI, dan semua Graphic UI
/// </summary>
public class FIKIFOW_ImageIdleAnimation : MonoBehaviour
{
    public enum IdleAnimationType
    {
        Fade,
        Wiggle_XY,
        Wiggle_X,
        Wiggle_Y,
        Wiggle_XYD,
        Pulse,
        Rotate,
        Bounce,
        Shake,
        Combo
    }

    [System.Serializable]
    public class IdleAnimationData
    {
        public IdleAnimationType type = IdleAnimationType.Wiggle_XY;
        public float speed = 60f; // frames/duration
        public float strength = 10f; // effect strength
        [HideInInspector] public float frame = 0f;
        [HideInInspector] public float wiggleDir = 0f; // untuk Wiggle_XYD
    }

    [Header("Idle Animation Settings")]
    [SerializeField] private bool enableAnimation = false;
    [SerializeField] private IdleAnimationData animationData = new IdleAnimationData();

    private Graphic targetGraphic;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private float originalRotation;
    private Color originalColor;
    private bool isInitialized = false;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized) return;

        targetGraphic = GetComponent<Graphic>();
        rectTransform = GetComponent<RectTransform>();

        if (targetGraphic == null || rectTransform == null)
        {
            Debug.LogError("[FIKIFOW_GraphicIdleAnimation] Memerlukan Graphic component (Image, RawImage, Text, TextMeshPro)!", gameObject);
            enabled = false;
            return;
        }

        originalPosition = rectTransform.localPosition;
        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation.eulerAngles.z;
        originalColor = targetGraphic.color;

        if (animationData == null)
            animationData = new IdleAnimationData();

        isInitialized = true;
    }

    private void Update()
    {
        if (!Application.isPlaying || !enableAnimation || targetGraphic == null || rectTransform == null)
        {
            if (!isInitialized && (targetGraphic != null || GetComponent<Graphic>() != null))
                Initialize();
            return;
        }

        if (!isInitialized)
            Initialize();

        ApplyIdleAnimation();
    }

    private void ApplyIdleAnimation()
    {
        animationData.frame++;
        float normalizedFrame = (animationData.frame % animationData.speed) / animationData.speed;
        
        float offsetX = 0f;
        float offsetY = 0f;
        float alpha = originalColor.a;
        float scaleFactor = 1f;
        float rotationAdd = 0f;

        switch (animationData.type)
        {
            case IdleAnimationType.Fade:
                alpha = originalColor.a * (0.5f + 0.5f * Mathf.Cos(normalizedFrame * Mathf.PI * 2f));
                break;

            case IdleAnimationType.Wiggle_XY:
                offsetX += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * animationData.strength;
                offsetY += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * animationData.strength;
                break;

            case IdleAnimationType.Wiggle_X:
                offsetX += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * animationData.strength;
                break;

            case IdleAnimationType.Wiggle_Y:
                offsetY += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * animationData.strength;
                break;

            case IdleAnimationType.Wiggle_XYD:
                if (animationData.wiggleDir == 0f)
                    animationData.wiggleDir = Random.Range(0f, Mathf.PI * 2f);
                
                float sin_val = Mathf.Sin(normalizedFrame * Mathf.PI * 2f);
                float cos_val = Mathf.Cos(animationData.wiggleDir);
                
                offsetX += sin_val * Mathf.Cos(animationData.wiggleDir) * animationData.strength;
                offsetY += sin_val * Mathf.Sin(animationData.wiggleDir) * animationData.strength;
                break;

            case IdleAnimationType.Pulse:
                scaleFactor = 1f + Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * (animationData.strength / 100f);
                break;

            case IdleAnimationType.Rotate:
                rotationAdd = 0.01f * animationData.strength;
                break;

            case IdleAnimationType.Bounce:
                offsetY += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * animationData.strength;
                break;

            case IdleAnimationType.Shake:
                offsetX += (Random.value - 0.5f) * 2f * animationData.strength;
                offsetY += (Random.value - 0.5f) * 2f * animationData.strength;
                break;

            case IdleAnimationType.Combo:
                // Kombinasi Pulse + Rotate + Subtle Wiggle
                scaleFactor = 1f + Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * (animationData.strength / 200f);
                rotationAdd = 0.01f * (animationData.strength / 2f);
                offsetX += Mathf.Sin(normalizedFrame * Mathf.PI * 2f) * (animationData.strength / 2f);
                break;
        }

        // Apply position
        Vector3 newPosition = originalPosition;
        newPosition.x += offsetX;
        newPosition.y += offsetY;
        rectTransform.localPosition = newPosition;

        // Apply scale
        Vector3 newScale = originalScale * scaleFactor;
        rectTransform.localScale = newScale;

        // Apply rotation
        Vector3 eulerAngles = rectTransform.localRotation.eulerAngles;
        eulerAngles.z = originalRotation + rotationAdd;
        rectTransform.localRotation = Quaternion.Euler(eulerAngles);

        // Apply color/alpha
        Color newColor = originalColor;
        newColor.a = alpha;
        targetGraphic.color = newColor;
    }

    /// <summary>
    /// Mulai animasi idle dengan tipe, kecepatan, dan kekuatan yang ditentukan
    /// </summary>
    public void StartIdleAnimation(IdleAnimationType type, float speed = 60f, float strength = 10f)
    {
        if (!isInitialized)
            Initialize();

        enableAnimation = true;
        animationData.type = type;
        animationData.speed = Mathf.Max(1f, speed);
        animationData.strength = strength;
        animationData.frame = 0f;
        animationData.wiggleDir = 0f;
    }

    /// <summary>
    /// Hentikan animasi idle dan kembalikan ke state asli
    /// </summary>
    public void StopIdleAnimation()
    {
        enableAnimation = false;
        animationData.frame = 0f;
        animationData.wiggleDir = 0f;

        if (rectTransform != null)
        {
            rectTransform.localPosition = originalPosition;
            rectTransform.localScale = originalScale;
            
            Vector3 eulerAngles = rectTransform.localRotation.eulerAngles;
            eulerAngles.z = originalRotation;
            rectTransform.localRotation = Quaternion.Euler(eulerAngles);
        }

        if (targetGraphic != null)
            targetGraphic.color = originalColor;
    }

    /// <summary>
    /// Check apakah animasi sedang berjalan
    /// </summary>
    public bool IsAnimating()
    {
        return enableAnimation;
    }

    /// <summary>
    /// Dapatkan data animasi saat ini
    /// </summary>
    public IdleAnimationData GetAnimationData()
    {
        return animationData;
    }

    /// <summary>
    /// Set data animasi custom
    /// </summary>
    public void SetAnimationData(IdleAnimationData data)
    {
        if (data != null)
        {
            animationData = data;
            animationData.frame = 0f;
        }
    }
}
