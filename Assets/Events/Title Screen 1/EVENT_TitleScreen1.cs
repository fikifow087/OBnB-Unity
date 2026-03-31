// Nama File: EVENT_TitleConfirm.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; 
using System.Collections;
using UnityEngine.SceneManagement;

[AddComponentMenu("EVENT/Title Screen Confirm 1")]
public class EVENT_TitleScreen1 : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioSource sfxBoom; // Tarik Audio Source SFX ke sini
    public AudioSource sfxButtonOk; // Tarik Audio Source BGM ke sini

    [Header("UI Reference")]
    public GameObject pressSpaceText; // Tarik objek "txt-PRESS TO" ke sini

    public GameObject KirisaSplash;
    public GameObject uiGroup1;
    public GameObject uiGroup2;
    public GameObject uiGroup3;
    public GameObject TransisiPutih;

    [Header("Language Settings")]
    public TMP_Dropdown dropdownLang; // Tarik objek Dropdown_Lang ke sini
    public string selectedLangCode = ""; // Variabel output

    private int TITLE_MAIN_PHASE = 0;
    private bool splashFinished = false;

    void Update()
    {
        // Mengecek apakah tombol Space ditekan DAN belum pernah ditekan sebelumnya
        // HANYA bisa input setelah splash screen selesai
        if (Keyboard.current.spaceKey.wasPressedThisFrame && TITLE_MAIN_PHASE < 1 && splashFinished)
        {
            TITLE_MAIN_PHASE = 1;
            StartCoroutine(ConfirmSelection());
        }
    }

    void Start()
    {
        // Aktifkan splash screen di awal
        KirisaSplash.SetActive(true);
        StartCoroutine(SplashScreenSequence());

        uiGroup1.SetActive(false);
        uiGroup2.SetActive(false);
        uiGroup3.SetActive(false);
        TransisiPutih.SetActive(false);
    }

    IEnumerator SplashScreenSequence()
    {
        // Tunggu 120 lama splash screen muncul
        yield return FIKIFOW_HoldFrames.Wait(120);

        // Fade out splash screen selama 60 frame
        yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeOut(KirisaSplash, 0f, 60));

        // Tunggu 30 frame tambahan
        yield return FIKIFOW_HoldFrames.Wait(30);

        // Sekarang bisa menerima input
        splashFinished = true;
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

    public void OnClickOkWarning()
    {
        // 1. Mainkan SFX Klik
        if (sfxButtonOk != null)
        {
            sfxButtonOk.Play();
        }

        // 2. Pindah ke Fase berikutnya
        TITLE_MAIN_PHASE = 4;
        uiGroup3.SetActive(false); // Sembunyikan menu peringatan
        StartCoroutine(GoToNextScene());
    }

    IEnumerator GoToNextScene()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 4)
        {
            yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeIn(TransisiPutih, 1f, 120));
            yield return FIKIFOW_HoldFrames.Wait(120);
            //Pindah Scene.
            SceneManager.LoadScene("Tittle Screen 2");
        }
    }
}