using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class FIKIFOW_RawImageScrolling : MonoBehaviour
{
    [Header("FikiFow Scrolling (x_speed/y_speed)")]
    public float speedX = 0.1f; // Muncul sebagai kolom input Speed X
    public float speedY = 0.0f; // Muncul sebagai kolom input Speed Y

    [Header("Opacity")]
    [Range(0f, 1f)]
    public float opacity = 1.0f;

    private RawImage _rawImage;

    void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        if (_rawImage != null && _rawImage.texture != null)
        {
            // 1. Logika Scrolling (Seamless) - hanya di Play Mode
            if (Application.isPlaying)
            {
                Rect rect = _rawImage.uvRect;
                rect.x += speedX * Time.deltaTime;
                rect.y += speedY * Time.deltaTime;
                _rawImage.uvRect = rect;
            }

            // 2. Logika Opacity - realtime di Edit Mode & Play Mode
            Color c = _rawImage.color;
            c.a = opacity;
            _rawImage.color = c;
        }
    }
}