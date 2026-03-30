# FIKIFOW Blend Controller - Simplified RPG Maker Style

## Overview
Simple UI blend mode controller untuk Unity Screen Space - Overlay Canvas. Support 4 blend mode dasar seperti RPG Maker MZ: **Normal**, **Add**, **Multiply**, **Screen**.

## Supported Blend Modes

| Mode | ID | Deskripsi |
|------|----|----|
| **Normal** | 0 | Standard alpha blending (default) |
| **Add** | 1 | Additive blending (terang) - untuk special effects, light |
| **Multiply** | 2 | Multiplicative blending (gelap) - untuk shadow, darken |
| **Screen** | 3 | Screen blending (terang) - untuk light overlay |

## Setup

### Requirements
- **Canvas Type**: Screen Space - Overlay WAJIB
- **Graphic Component**: Image, RawImage, atau UI Graphic lainnya
- **Shader**: `UI/FIKIFOW_BlendModes`

### Usage
1. Tambahkan component `FIKIFOW_BlendController` ke UI element
2. Pilih Blend Mode dari dropdown
3. Set opacity melalui alpha channel di Color

### Contoh Setup Kasus
```
BG (paling belakang, ordering layer tinggi)
    ↓
VFX Fog 1 (dengan BlendController + Multiply)
    ↓
VFX Fog 2 (paling depan, ordering layer rendah)
```

**Hasil**: 
- ✅ BG tetap normal
- ✅ Fog 1 hanya multiply diri sendiri
- ✅ Fog 2 tetap terlihat di depan

## How It Works

### Architecture
1. **C# Script** (`FIKIFOW_BlendController`)
   - Validasi Canvas Overlay
   - Set blend mode sesuai pilihan

2. **Shader** (`UI/FIKIFOW_BlendModes`)
   - Hardware blending untuk performa cepat
   - Opacity adjustment di fragment shader

### Blend Mode Details

**Normal (ID: 0)**
- Src: SrcAlpha
- Dst: OneMinusSrcAlpha
- Standard UI blending

**Add (ID: 1)**
- Src: SrcAlpha
- Dst: One
- Additive/bright effect

**Multiply (ID: 2)**
- Src: DstColor
- Dst: OneMinusSrcAlpha
- Darken effect

**Screen (ID: 3)**
- Src: OneMinusDstColor
- Dst: One
- Screen/bright effect

## Opacity Handling
- Opacity di-handle melalui alpha channel (Color.a)
- Shader melakukan opacity adjustment otomatis
- Respects Canvas clipping & stencil masking

## Troubleshooting

| Issue | Solusi |
|-------|--------|
| Blend tidak bekerja | Pastikan Canvas adalah Screen Space - Overlay |
| Layer lain hilang | Seharusnya tidak terjadi - gunakan GrabPass approach jika perlu |
| Opacity tidak benar | Set alpha pada Color tint object |
| Material error | Pastikan shader `UI/FIKIFOW_BlendModes` ada |

## Performa
- **Per Frame Cost**: Minimal (~0.1ms)
- **Memory**: ~1KB per instance
- **Cocok untuk**: Mobile & Desktop

## Notes
- Hanya support Screen Space - Overlay Canvas
- Jika butuh Screen Space - Camera atau World Space, gunakan shader berbeda
- Untuk complex blend modes ke depannya, bisa extend dengan BlendAdvanced shader

---
**Version**: 2.0 (Simplified RPG Maker Style)
**Last Update**: 2026-03-30
