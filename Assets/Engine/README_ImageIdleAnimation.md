# FIKIFOW Graphic Idle Animation - Unity Port

Ini adalah port dari plugin RPG Maker MZ `FIKIFOW-Idle_Animation.js` ke Unity untuk **semua UI Graphic components** termasuk Image, RawImage, Text, dan **TextMeshPro**.

## 📋 Daftar Animasi Tersedia

| Tipe Animasi | Deskripsi |
|---|---|
| **Fade** | Opacity berubah-ubah (alpha blinking) |
| **Wiggle XY** | Object bergerak kiri-kanan dan atas-bawah |
| **Wiggle X** | Object bergerak kiri-kanan saja |
| **Wiggle Y** | Object bergerak atas-bawah saja |
| **Wiggle XYD** | Wiggle dengan arah random (directional) |
| **Pulse** | Object membesar dan mengecil (scale) |
| **Rotate** | Object berputar terus |
| **Bounce** | Object melompat naik-turun |
| **Shake** | Object gemetar random (vibrate) |
| **Combo** | Kombinasi Pulse + Rotate + Wiggle subtle |

## 🚀 Cara Menggunakan

### Setup Basic

1. **Buat UI Element** di Canvas (Image, RawImage, Text, atau **TextMeshPro UGUI**)
2. **Add Component** → Cari "FIKIFOW Graphic Idle Animation"
3. Atau gunakan **FIKIFOW_IdleAnimationHelper** untuk control programmatic

### Method 1: Inspector Setup (Static)

1. Attach script `FIKIFOW_ImageIdleAnimation` ke GameObject Image/RawImage
2. Di Inspector:
   - Centang **Enable Animation** untuk memulai langsung
   - Pilih **Type** (Fade, Wiggle XY, dll)
   - Atur **Speed** (durasi cycle dalam frame) - default 60
   - Atur **Strength** (kekuatan efek) - default 10

### Method 2: Script Control (Dynamic)

```csharp
// Dapatkan reference ke component
FIKIFOW_ImageIdleAnimation idleAnim = GetComponent<FIKIFOW_ImageIdleAnimation>();

// Mulai animasi
idleAnim.StartIdleAnimation(
    FIKIFOW_ImageIdleAnimation.IdleAnimationType.Wiggle_XY,
    speed: 60f,
    strength: 10f
);

// Cek apakah animasi sedang berjalan
if (idleAnim.IsAnimating())
{
    Debug.Log("Animasi sedang berjalan");
}

// Hentikan animasi
idleAnim.StopIdleAnimation();
```

### Method 3: Menggunakan Helper Script

```csharp
FIKIFOW_IdleAnimationHelper helper = GetComponent<FIKIFOW_IdleAnimationHelper>();

// Gunakan method bawaan
helper.PlayWiggleAnimation();   // Wiggle XY
helper.PlayPulseAnimation();    // Pulse
helper.PlayFadeAnimation();     // Fade
helper.PlayBounceAnimation();   // Bounce
helper.PlayShakeAnimation();    // Shake
helper.PlayRotateAnimation();   // Rotate
helper.PlayComboAnimation();    // Combo

// Atau custom
helper.PlayCustomAnimation(
    FIKIFOW_ImageIdleAnimation.IdleAnimationType.Pulse,
    speed: 80f,
    strength: 20f
);

helper.StopAnimation();
```

## ⚙️ Parameter Pengaturan

| Parameter | Tipe | Default | Keterangan |
|---|---|---|---|
| **type** | Enum | Wiggle_XY | Jenis animasi yang digunakan |
| **speed** | float | 60 | Durasi cycle animasi (frame). Lebih kecil = lebih cepat |
| **strength** | float | 10 | Kekuatan efek animasi. Berpengaruh berbeda per tipe |

### Strength untuk Masing-masing Tipe:

- **Fade**: 1-10 (1 = subtle, 10 = strong opacity change)
- **Wiggle XY/X/Y**: 5-30 pixels (offset maksimal)
- **Wiggle XYD**: 5-30 pixels
- **Pulse**: 5-50 (% scale change)
- **Rotate**: 1-20 (rotation speed multiplier)
- **Bounce**: 10-50 pixels
- **Shake**: 10-30 pixels (random vibration amount)
- **Combo**: 10-50 (combined effect strength)

## 🔧 Contoh Penggunaan Umum

### Idle Character Portrait
```csharp
public class CharacterUI : MonoBehaviour
{
    public Image portraitImage;
    private FIKIFOW_ImageIdleAnimation idleAnim;

    void Start()
    {
        idleAnim = portraitImage.GetComponent<FIKIFOW_ImageIdleAnimation>();
        idleAnim.StartIdleAnimation(
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Wiggle_XY,
            speed: 60f,
            strength: 5f
        );
    }

    void OnDestroy()
    {
        idleAnim.StopIdleAnimation();
    }
}
```

### Button Hover Effect
```csharp
public class ButtonIdleEffect : MonoBehaviour
{
    private FIKIFOW_ImageIdleAnimation idleAnim;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        idleAnim = GetComponent<FIKIFOW_ImageIdleAnimation>();

        button.onSelect.AddListener(() => 
        {
            idleAnim.StartIdleAnimation(
                FIKIFOW_ImageIdleAnimation.IdleAnimationType.Pulse,
                speed: 60f,
                strength: 20f
            );
        });

        button.onDeselect.AddListener(() => 
        {
            idleAnim.StopIdleAnimation();
        });
    }
}
```

### Multiple Animations Rotation
```csharp
public class RotatingIdleEffect : MonoBehaviour
{
    private FIKIFOW_ImageIdleAnimation idleAnim;
    private float rotationTimer = 0f;
    private float rotationDuration = 5f;

    void Start()
    {
        idleAnim = GetComponent<FIKIFOW_ImageIdleAnimation>();
        StartNextAnimation();
    }

    void Update()
    {
        rotationTimer += Time.deltaTime;
        if (rotationTimer >= rotationDuration)
        {
            StartNextAnimation();
            rotationTimer = 0f;
        }
    }

    void StartNextAnimation()
    {
        var animTypes = new[]
        {
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Wiggle_XY,
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Pulse,
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Bounce,
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Rotate
        };

        var randomType = animTypes[Random.Range(0, animTypes.Length)];
        idleAnim.StartIdleAnimation(randomType, 60f, 15f);
    }
}
```

## 🎨 TextMeshPro Support

Component ini **100% kompatibel dengan TextMeshPro UGUI**! Gunakan seperti normal:

```csharp
// Attach ke TextMeshPro UGUI GameObject
TextMeshProUGUI tmpText = GetComponent<TextMeshProUGUI>();
FIKIFOW_ImageIdleAnimation idleAnim = GetComponent<FIKIFOW_ImageIdleAnimation>();

// Buat text berdenyut (Pulse effect)
idleAnim.StartIdleAnimation(
    FIKIFOW_ImageIdleAnimation.IdleAnimationType.Pulse,
    speed: 60f,
    strength: 20f
);
```

### Contoh: Floating Text Label
```csharp
public class FloatingTextLabel : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    private FIKIFOW_ImageIdleAnimation idleAnim;

    void Start()
    {
        idleAnim = damageText.GetComponent<FIKIFOW_ImageIdleAnimation>();
        if (idleAnim == null)
            idleAnim = damageText.gameObject.AddComponent<FIKIFOW_ImageIdleAnimation>();
        
        // Damage text bergerak naik dengan bounce effect
        idleAnim.StartIdleAnimation(
            FIKIFOW_ImageIdleAnimation.IdleAnimationType.Bounce,
            speed: 80f,
            strength: 30f
        );

        Destroy(gameObject, 2f); // Destroy setelah 2 detik
    }
}
```

## 📝 Catatan Penting

- ✅ **Kompatibel dengan**: Image, RawImage, Text, **TextMeshPro UGUI**, dan semua GUI component yang inherit dari `Graphic`
- ✅ **Berfungsi di**: Play Mode (dan Edit Mode untuk preview)
- ✅ **Performance**: Lightweight - bisa ditambahkan ke puluhan UI element
- ⚠️ **Perhatian**: Jangan kombinasikan dengan animator/tween lain pada Object yang sama untuk menghindari conflict
- ⚠️ **RectTransform**: Component akan mengubah position, scale, dan rotation secara realtime

## 🐛 Troubleshooting

| Masalah | Solusi |
|---|---|
| Animasi tidak muncul | Pastikan "Enable Animation" di-toggle atau `StartIdleAnimation()` dipanggil |
| Animasi conflict dengan animator | Gunakan Object terpisah atau hentikan animator lain |
| Performance drop | Kurangi jumlah UI dengan idle animation atau tingkatkan speed untuk cycle lebih cepat |
| Posisi kembali ke 0,0 | Pastikan original position di-save dengan benar (auto-save saat Initialize) |

## 📂 File Structure

```
Assets/Engine/
├── FIKIFOW_ImageIdleAnimation.cs      (Main component)
├── FIKIFOW_IdleAnimationHelper.cs     (Helper untuk kemudahan)
├── FIKIFOW_ImageIdleAnimation.cs.meta
├── FIKIFOW_IdleAnimationHelper.cs.meta
└── README_ImageIdleAnimation.md       (Dokumentasi ini)
```

---

**Dibuat dari port plugin FIKIFOW-Idle_Animation.js untuk Unity**
