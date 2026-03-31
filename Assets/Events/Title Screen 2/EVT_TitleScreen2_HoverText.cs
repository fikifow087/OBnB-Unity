using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class EVT_TitleScreen2_HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmpText;
    private Color originalColor;
    [SerializeField] private Color hoverColor = Color.red;
    private bool isInitialized = false;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized) return;

        // Cari TextMeshProUGUI di component ini atau child
        tmpText = GetComponent<TextMeshProUGUI>();
        
        if (tmpText == null)
        {
            tmpText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (tmpText == null)
        {
            Debug.LogError("[EVT_TitleScreen2_HoverText] TextMeshProUGUI tidak ditemukan di " + gameObject.name);
            return;
        }

        // Simpan warna original
        originalColor = tmpText.color;
        isInitialized = true;
        
        Debug.Log("[EVT_TitleScreen2_HoverText] Initialized with text: " + tmpText.text);
    }

    // Method public untuk Event Trigger (Pointer Enter)
    public void OnHoverEnter()
    {
        if (!isInitialized) Initialize();
        
        if (tmpText != null)
        {
            tmpText.color = hoverColor;
            Debug.Log("[EVT_TitleScreen2_HoverText] Hover Enter");
        }
    }

    // Method public untuk Event Trigger (Pointer Exit)
    public void OnHoverExit()
    {
        if (!isInitialized) Initialize();
        
        if (tmpText != null)
        {
            tmpText.color = originalColor;
            Debug.Log("[EVT_TitleScreen2_HoverText] Hover Exit");
        }
    }

    // Interface implementation untuk auto-detect
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}
