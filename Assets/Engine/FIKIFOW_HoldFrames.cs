using UnityEngine;

public static class FIKIFOW_HoldFrames
{
    // Di RPG Maker MZ, standar kecepatan adalah 60 frame per detik
    private const float TARGET_FPS = 60.0f;

    /// <summary>
    /// Alat pengganti "Wait..." di RPG Maker.
    /// Mengubah hitungan frame menjadi detik nyata agar aman di PC/HP lemot.
    /// </summary>
    public static WaitForSeconds Wait(int frames)
    {
        // Konversi: 60 frame / 60 = 1 detik. 30 frame / 60 = 0.5 detik.
        float secondsToWait = frames / TARGET_FPS;
        return new WaitForSeconds(secondsToWait);
    }
}