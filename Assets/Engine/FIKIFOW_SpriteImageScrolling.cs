using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class FIKIFOW_SpriteImageScrolling : MonoBehaviour
{
    [Header("FikiFow Scrolling (x_speed/y_speed)")]
    public float speedX = 0.1f; // Muncul sebagai kolom input Speed X
    public float speedY = 0.0f; // Muncul sebagai kolom input Speed Y

    [Header("Opacity")]
    [Range(0f, 1f)]
    public float opacity = 1.0f;

    private Image _spriteImage;
    private FIKIFOW_BlendController _blendController;
    private Material _materialInstance;
    private Vector2 _textureOffset;

    void Start()
    {
        _spriteImage = GetComponent<Image>();
        _blendController = GetComponent<FIKIFOW_BlendController>();
        InitializeMaterial();
    }

    void OnEnable()
    {
        // Ensure initialization on enable
        if (_spriteImage == null) _spriteImage = GetComponent<Image>();
        if (_blendController == null) _blendController = GetComponent<FIKIFOW_BlendController>();
        InitializeMaterial();
    }

    void OnDisable()
    {
        // Cleanup material instance
        if (_materialInstance != null)
        {
            DestroyImmediate(_materialInstance);
            _materialInstance = null;
        }
    }

    private void InitializeMaterial()
    {
        if (_spriteImage != null && _spriteImage.material != null)
        {
            // Create a material instance if it doesn't exist
            if (_materialInstance == null)
            {
                _materialInstance = new Material(_spriteImage.material);
                _spriteImage.material = _materialInstance;
            }
        }
    }

    void Update()
    {
        if (_spriteImage != null && _spriteImage.sprite != null)
        {
            // 1. Logika Scrolling (Seamless) - hanya di Play Mode
            if (Application.isPlaying)
            {
                _textureOffset.x += speedX * Time.deltaTime;
                _textureOffset.y += speedY * Time.deltaTime;

                // Apply texture offset to material
                if (_materialInstance != null)
                {
                    _materialInstance.SetTextureOffset("_MainTex", _textureOffset);
                }
            }

            // 2. Logika Opacity - realtime di Edit Mode & Play Mode
            Color c = _spriteImage.color;
            c.a = opacity;
            _spriteImage.color = c;
            
            // 3. Komunikasi dengan BlendController jika ada
            if (_blendController != null && _blendController.enabled)
            {
                _blendController.UpdateOpacityFromScrolling(opacity);
            }
        }
    }
}
