using UnityEngine;

public class BAKSO_MenuUI_Switcher : MonoBehaviour
{
    [Header("Canvas That Has UI Packed Objects")]
    public GameObject UI_PackedObjects;

    [Header("Selective UI Objects")]
    public GameObject UI_Order_Menu;
    public GameObject UI_Buku_Resep;

    //
    //[Header("UI Switcher Buttons")]
    //public GameObject BTN_Order_Menu;
    //public GameObject BTN_Buku_Resep;
    //\\

    [Header("UI Switcher Variables")]
    [SerializeField] private bool UI_VAR_Order_Menu;
    [SerializeField] private bool UI_VAR_Buku_Resep;

    [Header("UI Switcher SFX")]
    public AudioSource BTN_Order_Menu_SFX;
    public AudioSource BTN_Buku_Resep_SFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI_Order_Menu.SetActive(false);
        UI_Buku_Resep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (UI_VAR_Order_Menu == true)
        {
            UI_Order_Menu.SetActive(true);
        }
        else
        {
            UI_Order_Menu.SetActive(false);
        }
        if (UI_VAR_Buku_Resep == true)
        {
            UI_Buku_Resep.SetActive(true);
        }
        else
        {
            UI_Buku_Resep.SetActive(false);
        }
    }

    public void OnClick_BTN_Order_Menu()
    {
        BTN_Order_Menu_SFX.Play();
        if (UI_VAR_Order_Menu == false)
        {
            UI_VAR_Order_Menu = true;
        }
        else
        {
            UI_VAR_Order_Menu = false;
        }
        
    }

    public void OnClick_BTN_Buku_Resep()
    {
        BTN_Buku_Resep_SFX.Play();
        if (UI_VAR_Buku_Resep == false)
        {
            UI_VAR_Buku_Resep = true;
        }
        else
        {
            UI_VAR_Buku_Resep = false;
        }
    }

}
