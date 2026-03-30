using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

[ExecuteAlways]
[AddComponentMenu("FIKIFOW/Blend Controller")]
public class FIKIFOW_BlendController : MonoBehaviour
{
    public enum BlendMode
    {
        // 1. Normal Group
        Normal = 0,
        Dissolve = 1,
        
        // 2. Darken Group
        Darken = 2,
        Multiply = 3,
        ColorBurn = 4,
        LinearBurn = 5,
        DarkerColor = 6,
        
        // 3. Lighten Group
        Lighten = 7,
        Screen = 8,
        ColorDodge = 9,
        LinearDodge = 10,
        LighterColor = 11,
        
        // 4. Contrast Group
        Overlay = 12,
        SoftLight = 13,
        HardLight = 14,
        VividLight = 15,
        LinearLight = 16,
        PinLight = 17,
        HardMix = 18,
        
        // 5. Inversion/Cancellation Group
        Difference = 19,
        Exclusion = 20,
        Subtract = 21,
        Divide = 22
    }

    [Header("Blend Settings")]
    [Tooltip("Pilih blend mode seperti di Photoshop.")]
    [SerializeField] private BlendMode blendMode = BlendMode.Normal;

    private Graphic targetGraphic;
    private Material blendMaterial;

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
            case BlendMode.Dissolve:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Darken:
                // Darken: Min(src, dst) - approximated with multiply-like behavior
                srcBlend = UnityEngine.Rendering.BlendMode.DstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.Zero;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Multiply:
                srcBlend = UnityEngine.Rendering.BlendMode.DstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.ColorBurn:
                // Similar to multiply
                srcBlend = UnityEngine.Rendering.BlendMode.DstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.LinearBurn:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.ReverseSubtract;
                break;
                
            case BlendMode.DarkerColor:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Lighten:
                // Lighten: Max(src, dst) - approximated
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Screen:
                srcBlend = UnityEngine.Rendering.BlendMode.OneMinusDstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.ColorDodge:
                // Similar to screen
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.LinearDodge:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.LighterColor:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Overlay:
                // Complex blend - using standard alpha blend
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.SoftLight:
            case BlendMode.HardLight:
            case BlendMode.VividLight:
            case BlendMode.LinearLight:
            case BlendMode.PinLight:
            case BlendMode.HardMix:
                // Complex blends - using standard alpha blend
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Difference:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Exclusion:
                srcBlend = UnityEngine.Rendering.BlendMode.OneMinusDstColor;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcColor;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
                
            case BlendMode.Subtract:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.One;
                blendOp = UnityEngine.Rendering.BlendOp.ReverseSubtract;
                break;
                
            case BlendMode.Divide:
                srcBlend = UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                blendOp = UnityEngine.Rendering.BlendOp.Add;
                break;
        }

        blendMaterial.SetFloat("_SrcBlend", (float)srcBlend);
        blendMaterial.SetFloat("_DstBlend", (float)dstBlend);
        blendMaterial.SetFloat("_BlendOp", (float)blendOp);
    }
}
