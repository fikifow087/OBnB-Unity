// Nama File: EVENT_TitleConfirm.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[AddComponentMenu("EVENT/Title Screen Confirm 1")]
public class EVENT_TitleScreen1 : MonoBehaviour 
{
    [Header("Audio Settings")]
    public AudioSource sfxConfirm; // Tarik Audio Source SFX ke sini

    [Header("UI Reference")]
    public GameObject pressSpaceText; // Tarik objek "txt-PRESS TO" ke sini

    public GameObject uiGroup1;
    public GameObject uiGroup2;
    public GameObject uiGroup3;

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
            if (sfxConfirm != null)
            {
                sfxConfirm.Play();
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

            yield return FIKIFOW_HoldFrames.Wait(90);
            yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeIn(uiGroup1, 0.6f, 60));
            TITLE_MAIN_PHASE = 2;
            Invoke("LanguagePhase", 2.0f);  
        }
    }

    void LanguagePhase()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 2)
        {
 
        }
    }
    
    void WarningPhase()
    {
        Debug.Log("Fase TITLE SCREEN saat ini adalah: " + TITLE_MAIN_PHASE);
        if (TITLE_MAIN_PHASE == 3)
        {
            Invoke("GoToNextScene", 2.0f);  
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