// Nama File: EVENT_TitleConfirm.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; 
using System.Collections;

[AddComponentMenu("EVENT/Title Screen Confirm 1")]
public class EVENT_TitleScreen1 : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioSource sfxBoom; // Tarik Audio Source SFX ke sini
    public AudioSource sfxButtonOk; // Tarik Audio Source BGM ke sini

    [Header("UI Reference")]
    public GameObject pressSpaceText; // Tarik objek "txt-PRESS TO" ke sini

    public GameObject uiGroup1;
    public GameObject uiGroup2;
    public GameObject uiGroup3;

    [Header("Language Settings")]
    public TMP_Dropdown dropdownLang; // Tarik objek Dropdown_Lang ke sini
    public string selectedLangCode = ""; // Variabel output

    private int TITLE_MAIN_PHASE = 0;

    void Update()
    {
        // Mengecek apakah tombol Space ditekan DAN belum pernah ditekan sebelumnya
        if (Keyboard.current.spaceKey.wasPressedThisFrame && TITLE_MAIN_PHASE < 1)
        {
            TITLE_MAIN_PHASE = 1;
            StartCoroutine(ConfirmSelection());
        }
    }

    void Start()
    {
        uiGroup1.SetActive(false);
        uiGroup2.SetActive(false);
        uiGroup3.SetActive(false);
    }
    IEnumerator ConfirmSelection()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 1)
        {
            if (sfxBoom != null)
            {
                sfxBoom.Play();
            }

            // Efek Visual (Menyembunyikan teks kedip-kedip dengan fade out)
            if (pressSpaceText != null)
            {
                // Stop idle animation
                FIKIFOW_ImageIdleAnimation idleAnim = pressSpaceText.GetComponent<FIKIFOW_ImageIdleAnimation>();

                if (idleAnim != null)
                {
                    idleAnim.StopIdleAnimation();
                    // Fade out text dengan durasi 60 frame (1 detik)
                    yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeOut(pressSpaceText, 0f, 60));
                }
                else
                {
                    pressSpaceText.SetActive(false);
                }
            }

            yield return FIKIFOW_HoldFrames.Wait(60);
            yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeIn(uiGroup1, 0.7f, 60));
            TITLE_MAIN_PHASE = 2;
            Invoke("LanguagePhase", 1.0f);  
        }
    }

    void LanguagePhase()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 2)
        {
            uiGroup2.SetActive(true);  // Menu pemilihan bahasa, Btn OK, Dropdown.
        }
    }

    // --- FUNGSI BARU UNTUK TOMBOL OK ---
    public void OnClickOkLanguage()
    {
        // 1. Mainkan SFX Klik
        if (sfxButtonOk != null)
        {
            sfxButtonOk.Play();
        }

        // 2. Tentukan variabel berdasarkan pilihan Dropdown
        // Index 0 = English (gb), Index 1 = Indonesian (id) berdasarkan urutan asset kamu
        if (dropdownLang.value == 0)
        {
            selectedLangCode = "en";
        }
        else if (dropdownLang.value == 1)
        {
            selectedLangCode = "id";
        }

        Debug.Log("Bahasa yang dipilih: " + selectedLangCode);

        // 3. Pindah ke Fase berikutnya
        TITLE_MAIN_PHASE = 3;
        uiGroup2.SetActive(false); // Sembunyikan pilihan bahasa
        WarningPhase();
    }
    
    void WarningPhase()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 3)
        {

            uiGroup3.SetActive(true);  // Menu peringatan sebelum memulai game.

        }
    }

    void GoToNextScene()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 4)
        {

        }
        // Ganti "GameplayScene" dengan nama scene game kamu sesungguhnya
        // SceneManager.LoadScene("GameplayScene");
    }
}