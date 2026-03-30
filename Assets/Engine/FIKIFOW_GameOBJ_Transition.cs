using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// FIKIFOW Game Object Transition - Fade In/Fade Out Controller
/// Plugin global reusable untuk fade in dan fade out game object dengan durasi frame-based
/// Mendukung: UI (Image, RawImage, Text, TextMeshPro) dan Renderer objects
/// </summary>
public static class FIKIFOW_GameOBJ_Transition
{
    private const float TARGET_FPS = 60.0f;

    /// <summary>
    /// Fade In coroutine - SetActive(true) kemudian opacity 0 → 100
    /// </summary>
    /// <param name="targetObject">GameObject yang akan di-fade in</param>
    /// <param name="durationFrames">Durasi dalam frame (60 frame = 1 detik)</param>
    public static IEnumerator FadeIn(GameObject targetObject, int durationFrames)
    {
        if (targetObject == null)
        {
            Debug.LogError("[FIKIFOW_GameOBJ_Transition] Target object is null!");
            yield break;
        }

        // SetActive true di awal
        targetObject.SetActive(true);

        Graphic uiGraphic = targetObject.GetComponent<Graphic>();
        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        Renderer renderer = targetObject.GetComponent<Renderer>();

        // Tentukan tipe object dan set opacity awal ke 0
        if (uiGraphic != null)
        {
            Color color = uiGraphic.color;
            color.a = 0f;
            uiGraphic.color = color;
        }
        else if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        else if (renderer != null)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0f;
            mat.color = color;
        }
        else
        {
            Debug.LogWarning("[FIKIFOW_GameOBJ_Transition] Object tidak memiliki Graphic, CanvasGroup, atau Renderer component!", targetObject);
            yield break;
        }

        // Fade in: opacity 0 → 1
        float elapsedFrames = 0f;
        while (elapsedFrames < durationFrames)
        {
            elapsedFrames += 1f;
            float progress = elapsedFrames / durationFrames;

            if (uiGraphic != null)
            {
                Color color = uiGraphic.color;
                color.a = progress;
                uiGraphic.color = color;
            }
            else if (canvasGroup != null)
            {
                canvasGroup.alpha = progress;
            }
            else if (renderer != null)
            {
                Material mat = renderer.material;
                Color color = mat.color;
                color.a = progress;
                mat.color = color;
            }

            yield return null;
        }

        // Pastikan opacity akhir = 1
        if (uiGraphic != null)
        {
            Color color = uiGraphic.color;
            color.a = 1f;
            uiGraphic.color = color;
        }
        else if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
        else if (renderer != null)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 1f;
            mat.color = color;
        }
    }

    /// <summary>
    /// Fade Out coroutine - opacity 100 → 0 kemudian SetActive(false)
    /// </summary>
    /// <param name="targetObject">GameObject yang akan di-fade out</param>
    /// <param name="durationFrames">Durasi dalam frame (60 frame = 1 detik)</param>
    public static IEnumerator FadeOut(GameObject targetObject, int durationFrames)
    {
        if (targetObject == null)
        {
            Debug.LogError("[FIKIFOW_GameOBJ_Transition] Target object is null!");
            yield break;
        }

        Graphic uiGraphic = targetObject.GetComponent<Graphic>();
        CanvasGroup canvasGroup = targetObject.GetComponent<CanvasGroup>();
        Renderer renderer = targetObject.GetComponent<Renderer>();

        if (uiGraphic == null && canvasGroup == null && renderer == null)
        {
            Debug.LogWarning("[FIKIFOW_GameOBJ_Transition] Object tidak memiliki Graphic, CanvasGroup, atau Renderer component!", targetObject);
            yield break;
        }

        // Fade out: opacity 1 → 0
        float elapsedFrames = 0f;
        while (elapsedFrames < durationFrames)
        {
            elapsedFrames += 1f;
            float progress = 1f - (elapsedFrames / durationFrames);

            if (uiGraphic != null)
            {
                Color color = uiGraphic.color;
                color.a = progress;
                uiGraphic.color = color;
            }
            else if (canvasGroup != null)
            {
                canvasGroup.alpha = progress;
            }
            else if (renderer != null)
            {
                Material mat = renderer.material;
                Color color = mat.color;
                color.a = progress;
                mat.color = color;
            }

            yield return null;
        }

        // Pastikan opacity akhir = 0
        if (uiGraphic != null)
        {
            Color color = uiGraphic.color;
            color.a = 0f;
            uiGraphic.color = color;
        }
        else if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        else if (renderer != null)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0f;
            mat.color = color;
        }

        // SetActive false setelah fade out selesai
        targetObject.SetActive(false);
    }

    /// <summary>
    /// Fade In dengan durasi dalam detik
    /// </summary>
    public static IEnumerator FadeInSeconds(GameObject targetObject, float durationSeconds)
    {
        int durationFrames = Mathf.RoundToInt(durationSeconds * TARGET_FPS);
        yield return FadeIn(targetObject, durationFrames);
    }

    /// <summary>
    /// Fade Out dengan durasi dalam detik
    /// </summary>
    public static IEnumerator FadeOutSeconds(GameObject targetObject, float durationSeconds)
    {
        int durationFrames = Mathf.RoundToInt(durationSeconds * TARGET_FPS);
        yield return FadeOut(targetObject, durationFrames);
    }
}
