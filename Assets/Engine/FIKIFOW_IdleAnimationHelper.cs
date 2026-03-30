using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper script untuk menggunakan FIKIFOW_ImageIdleAnimation dengan mudah
/// Contoh: Attach ke GameObject yang sama atau gunakan script ini untuk control animasi
/// </summary>
public class FIKIFOW_IdleAnimationHelper : MonoBehaviour
{
    [Header("Target Reference")]
    [SerializeField] private FIKIFOW_ImageIdleAnimation idleAnimation;

    private void OnEnable()
    {
        if (idleAnimation == null)
            idleAnimation = GetComponent<FIKIFOW_ImageIdleAnimation>();
    }

    /// <summary>
    /// Contoh: Mulai animasi Wiggle XY dengan kecepatan 60 frame dan strength 10
    /// </summary>
    public void PlayWiggleAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Wiggle_XY, 60f, 10f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Pulse
    /// </summary>
    public void PlayPulseAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Pulse, 60f, 15f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Fade
    /// </summary>
    public void PlayFadeAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Fade, 60f, 1f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Rotate
    /// </summary>
    public void PlayRotateAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Rotate, 60f, 5f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Bounce
    /// </summary>
    public void PlayBounceAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Bounce, 60f, 20f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Shake
    /// </summary>
    public void PlayShakeAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Shake, 60f, 15f);
    }

    /// <summary>
    /// Contoh: Mulai animasi Combo (pulse + rotate + wiggle)
    /// </summary>
    public void PlayComboAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType.Combo, 60f, 20f);
    }

    /// <summary>
    /// Hentikan animasi
    /// </summary>
    public void StopAnimation()
    {
        if (idleAnimation != null)
            idleAnimation.StopIdleAnimation();
    }

    /// <summary>
    /// Custom animation dengan parameter
    /// </summary>
    public void PlayCustomAnimation(FIKIFOW_ImageIdleAnimation.IdleAnimationType type, float speed, float strength)
    {
        if (idleAnimation != null)
            idleAnimation.StartIdleAnimation(type, speed, strength);
    }
}
