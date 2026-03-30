# FIKIFOW Blend Controller - Dokumentasi Perbaikan

## Ringkasan Masalah
Blend mode di UI layer menyebabkan layer lain (di atas atau di belakang) hilang atau tidak terrender dengan benar.

### Penyebab
- **Shader Advanced lama** menggunakan `SampleSceneColor()` dari `OpaqueTexture` yang membaca seluruh scene, menyebabkan "ambil alih" render framebuffer
- Hasil: Layer di belakang tidak terrender, atau layer di depan tertutup
- **Blend State menjadi masalah**: `Blend One Zero` tanpa proper GrabPass

### Solusi yang Diterapkan

#### 1. **Validasi Canvas Overlay (C# Script)**
```csharp
private bool ValidateCanvasOverlay()
{
    Canvas canvas = GetComponentInParent<Canvas>();
    if (canvas == null) { return false; }
    if (canvas.renderMode != RenderMode.ScreenSpaceOverlay) { return false; }
    return true;
}
```
- Memastikan blend mode **HANYA** berfungsi di Screen Space - Overlay Canvas
- Mencegah penyalahgunaan pada Canvas type lain

#### 2. **Ganti GrabPass di Advanced Shader**
- **LAMA**: `SampleSceneColor()` - membaca seluruh scene
- **BARU**: `GrabPass { "_UIGrabScreenTex" }` - membaca hanya framebuffer sebelum element ini

**Keuntungan GrabPass**:
- Membaca satu frame sebelumnya, bukan seluruh scene
- Menghormati rendering order/ordering layer
- Layer di depan tetap bisa di-render di atas
- Layer di belakang tetap terlihat

#### 3. **Penambahan Mode Support**
Advanced shader sekarang mendukung:
- **Darken** & **Lighten** (baru)
- Semua mode existing tetap berfungsi dengan benar

#### 4. **Fragment Shader Improvement**
```glsl
// Discard jika alpha 0
if (sColor.a <= 0.0) discard;

// Proper lerp berdasarkan alpha
half3 finalRGB = lerp(bColor, blendedRGB, sColor.a);

// Return dengan alpha 1.0 untuk framebuffer
return half4(finalRGB, 1.0);
```

## Cara Penggunaan

### Requirements
- **Canvas Type**: Screen Space - Overlay WAJIB
- **Graphic Component**: Image, RawImage, atau UI Graphic lainnya

### Setup
1. Tambahkan component `FIKIFOW_BlendController` ke UI element
2. Pilih Blend Mode dari dropdown
3. Opacity tetap bisa diatur dari alpha channel Color

### Contoh Kasus: BG > VFX Fog 1 > VFX Fog 2
```
- BG (paling belakang, ordering layer paling tinggi)
- VFX Fog 1 (dengan BlendController + Multiply) ✓ HANYA fog 1 yang terpengaruh
- VFX Fog 2 (paling depan, ordering layer paling rendah)
```

**Hasil**:
- ✅ BG tetap terlihat normal di belakang
- ✅ Fog 1 menerapkan multiply hanya pada dirinya
- ✅ Fog 2 tetap bisa di-render di depan

## Mode Blend Support

### Hardware Blending (Sederhana, Cepat)
- Normal
- Multiply
- Screen
- LinearDodge (Add)
- Subtract
- Exclusion
- Dissolve (Normal + Opacity)

### Advanced Blending (Kompleks, Akurat)
- Darken
- Lighten
- ColorBurn
- LinearBurn
- DarkerColor
- ColorDodge
- LighterColor
- Overlay
- SoftLight
- HardLight
- VividLight
- LinearLight
- PinLight
- HardMix
- Difference
- Divide

## Performa
- **Hardware Mode**: Sangat cepat (1 pass)
- **Advanced Mode**: Medium (1 pass + GrabPass)
- Minimal overhead, cocok untuk mobile

## Troubleshooting

### Issue: Blend tidak terjadi / Layer tetap normal
**Solusi**: Pastikan Canvas type adalah "Screen Space - Overlay"

### Issue: Layer di belakang hilang
**Solusi**: Sudah diperbaiki dengan GrabPass approach. Update shader Anda.

### Issue: Alpha/Opacity tidak bekerja dengan benar
**Solusi**: Pastikan UI element punya alpha channel proper (di color tint atau image alpha)

## Catatan Teknis
- GrabPass membaca frame sebelumnya, tidak immediate per-frame
- Untuk dynamic/real-time blending yang sangat akurat, pertimbangkan approach lain
- Stencil masking Canvas tetap berfungsi normal

---
**Version**: 1.1 (Fixed)
**Last Update**: 2026-03-30
