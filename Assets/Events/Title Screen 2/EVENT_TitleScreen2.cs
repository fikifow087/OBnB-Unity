using UnityEngine;
using System.Collections;

public class EVENT_TitleScreen2 : MonoBehaviour
{

    [Header("Audio Settings")]
    public AudioSource sfxHover;
    public AudioSource sfxButtonOk;

    [Header("UI Reference")]
    public GameObject TransisiPutih;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TransisiPutih.SetActive(true);
        StartCoroutine(SceneStartSequence());
    }

    IEnumerator SceneStartSequence()
    {
        yield return FIKIFOW_HoldFrames.Wait(30);
        yield return StartCoroutine(FIKIFOW_GameOBJ_Transition.FadeOut(TransisiPutih, 0f, 60));

    }

    public void OnClickButtonOK()
    {
        sfxButtonOk.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
