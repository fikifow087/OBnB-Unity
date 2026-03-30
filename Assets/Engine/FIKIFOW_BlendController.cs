using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

[ExecuteAlways]
[AddComponentMenu("FIKIFOW/Blend Controller")]
public class FIKIFOW_BlendController : MonoBehaviour
{
    public enum BlendMode
    {
        Normal = 0,
        Add = 1,
        Multiply = 2,
        Screen = 3
    }

    [Header("Blend Settings")]
    [Tooltip("Pilih blend mode seperti di Photoshop.")]
    [SerializeField] private BlendMode blendMode = BlendMode.Normal;

    private Graphic targetGraphic;
    private Material blendMaterial;
    private float _currentOpacity = 1.0f;

    private void OnEnable()
    {
        Initialize();
        ApplyBlendMode();
    }

    private void OnValidate()
    {
        Initialize();
        ApplyBlendMode();
    }

    private void Initialize()
    {
        if (targetGraphic == null) targetGraphic = GetComponent<Graphic>();
    }

    private bool ValidateCanvasOverlay()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[FIKIFOW_BlendController] Harus berada dalam Canvas!", gameObject);
            return false;
        }

        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogError("[FIKIFOW_BlendController] Hanya mendukung Screen Space - Overlay Canvas!", gameObject);
            return false;
        }

        return true;
    }

    public void SetBlendMode(BlendMode newMode)
    {
        if (blendMode != newMode)
        {
            blendMode = newMode;
            ApplyBlendMode();
        }
    }

    /// <summary>
    /// Called by RawImageScrolling to sync opacity value
    /// Opacity control tetap berfungsi dengan blend mode
    /// </summary>
    public void UpdateOpacityFromScrolling(float opacity)
    {
        _currentOpacity = opacity;
        // Opacity already handled by RawImageScrolling via color.a
        // This method exists for potential future tracking/coordination
    }

    private void ApplyBlendMode()
    {
        if (targetGraphic == null) return;
        
        // Validasi Canvas Overlay
        if (!ValidateCanvasOverlay()) return;

        // Gunakan shader tunggal untuk semua blend mode (kompatibel dengan UI Canvas Overlay)
        string shaderName = "UI/FIKIFOW_BlendModes";

        // Ganti material ke shader yang sesuai
        if (blendMaterial == null || blendMaterial.shader.name != shaderName)
        {
            var shader = Shader.Find(shaderName);
            if (shader == null)
            {
                Debug.LogWarning($"[FIKIFOW_BlendController] Shader '{shaderName}' tidak ditemukan!");
                return;
            }
            blendMaterial = new Material(shader);
            blendMaterial.name = $"FIKIFOW_BlendMaterial ({shaderName})";
            targetGraphic.material = blendMaterial;
        }

        // Pass ID blend mode ke shader
        blendMaterial.SetFloat("_BlendModeID", (float)blendMode);
        
        // Apply hardware blend settings
        // Opacity dari RawImageScrolling akan tetap work karena RawImageScrolling set color.a setiap frame
        ApplyHardwareBlendSettings();
    }

    private void ApplyHardwareBlendSettings()
    {
        UnityEngine.Rendering.BlendMode srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
        UnityEngine.Rendering.BlendMode dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
        UnityEngine.Rendering.BlendOp blendOp = UnityEngine.Rendering.BlendOp.Add;

        switch (blendMode)
        {
            case BlendMode.Normal:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Add:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Multiply:
                srcBlend = UnityEngine.Rendering.BlendMode.DstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Screen:
                srcBlend = UnityEngine.Rendering.BlendMode.OneMinusDstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
        }

        blendMaterial.SetFloat("_SrcBlend", (float)srcBlend);
        blendMaterial.SetFloat("_DstBlend", (float)dstBlend);
        blendMaterial.SetFloat("_BlendOp", (float)blendOp);
    }
}
