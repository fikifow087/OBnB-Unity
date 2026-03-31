using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    void Awake()
    {
        // Set target frame rate ke 60
        Application.targetFrameRate = 60;
        
        // Opsional: Matikan VSync agar targetFrameRate bekerja maksimal
        // 0 = Don't Sync, 1 = Every VBlank
        QualitySettings.vSyncCount = 0; 
    }
}