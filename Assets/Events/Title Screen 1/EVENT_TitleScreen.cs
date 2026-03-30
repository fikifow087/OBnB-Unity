// Nama File: EVENT_TitleConfirm.cs
using UnityEngine;

[AddComponentMenu("EVENT/Title Confirm")]
public class EVENT_TitleConfirm : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioSource sfxConfirm; // Tarik Audio Source SFX ke sini

    [Header("UI Reference")]
    public GameObject pressSpaceText; // Tarik objek "txt-PRESS TO" ke sini

    private bool _isPressed = false;

    void Update()
    {
        // Mengecek apakah tombol Space ditekan DAN belum pernah ditekan sebelumnya
        if (Input.GetKeyDown(KeyCode.Space) && !_isPressed)
        {
            ConfirmSelection();
        }
    }

    void ConfirmSelection()
    {
        _isPressed = true;

        // 1. Play Suara (Seperti Play SE di RPG Maker)
        if (sfxConfirm != null)
        {
            sfxConfirm.Play();
        }

        // 2. Efek Visual (Menyembunyikan teks kedip-kedip)
        if (pressSpaceText != null)
        {
            pressSpaceText.SetActive(false);
        }

        // 3. Logika Lanjutan (Pindah Scene setelah 2 detik)
        Invoke("GoToNextScene", 2.0f);
    }

    void GoToNextScene()
    {
        // Ganti "GameplayScene" dengan nama scene game kamu sesungguhnya
        // SceneManager.LoadScene("GameplayScene");
        Debug.Log("Pindah ke Map Pertama Ourakh!");
    }
}