# 🏦 Çek Yazdırma Uygulaması

**Desktop Check Print & Calibration Master** - Profesyonel çek yazdırma ve kalibrasyon uygulaması

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## 📋 İçindekiler

- [Özellikler](#-özellikler)
- [Kurulum](#-kurulum)
- [Kullanım](#-kullanım)
- [Teknik Detaylar](#-teknik-detaylar)
- [Geliştirme](#-geliştirme)
- [Katkıda Bulunma](#-katkıda-bulunma)

## ✨ Özellikler

### Temel Özellikler
- ✅ **Çek Yazdırma** - Banka çeklerini standart yazıcılarla yazdırma
- ✅ **Otomatik Yazıya Çevirme** - Tutarı Türkçe yazıya otomatik dönüştürme
- ✅ **Önizleme** - Yazdırma öncesi WYSIWYG önizleme
- ✅ **Kalibrasyon** - X/Y ekseninde milimetrik hassasiyet ayarları
- ✅ **Ayar Yönetimi** - Yazıcı ayarlarını kalıcı olarak saklama
- ✅ **Offline Çalışma** - İnternet bağlantısı gerektirmez

### Teknik Özellikler
- 🚀 **Standalone Uygulama** - .NET Runtime gerektirmez
- 💾 **Küçük Boyut** - ~70MB tek dosya
- 🎯 **Hassasiyet** - ±1mm yazdırma hassasiyeti
- ⚡ **Hızlı** - Tutar dönüşümü <100ms
- 🔒 **Güvenli** - Tüm veriler yerel olarak saklanır

## 🚀 Kurulum

### Gereksinimler
- **İşletim Sistemi:** Windows 10/11 (64-bit)
- **Bellek:** En az 2 GB RAM
- **Disk Alanı:** En az 200 MB
- **Yazıcı:** Windows destekli herhangi bir yazıcı

### Hızlı Kurulum

1. **İndirme:**
   - `publish-standalone` klasöründeki `CheckPrintApp.UI.exe` dosyasını indirin

2. **Çalıştırma:**
   - `.exe` dosyasına çift tıklayın
   - Windows Defender uyarısı verirse "Yine de çalıştır" seçin

3. **İlk Kullanım:**
   - Yazıcınızı seçin
   - Kalibrasyon ayarlarını yapın
   - İlk çeki yazdırın!

## 📖 Kullanım

### Temel Kullanım

1. **Çek Bilgilerini Girin:**
   ```
   Alıcı: Mehmet Yılmaz
   Tutar: 1500.50
   Tarih: 20.02.2026
   ```

2. **Önizleme:**
   - "Önizleme" butonuna tıklayın
   - Çekin düzgün görüntülendiğinden emin olun

3. **Yazdırma:**
   - Çeki yazıcıya yerleştirin
   - "Yazdır" butonuna tıklayın

### Kalibrasyon

İlk kullanımda veya yazıcı değişikliğinde kalibrasyon yapmanız önerilir:

1. Test baskısı alın
2. X ve Y ekseninde kaymayı ölçün
3. Ayarlar menüsünden offset değerlerini girin
4. Tekrar test edin

## 🛠️ Teknik Detaylar

### Mimari

```
CheckPrintApp/
├── CheckPrintApp.Core/       # İş mantığı ve servisler
│   ├── Models/               # Veri modelleri
│   ├── Services/             # İş servisleri
│   └── Helpers/              # Yardımcı sınıflar
├── CheckPrintApp.UI/         # WPF kullanıcı arayüzü
│   ├── ViewModels/           # MVVM ViewModels
│   ├── Views/                # XAML görünümler
│   └── Services/             # UI servisleri
└── CheckPrintApp.Tests/      # Unit testler
```

### Teknoloji Stack'i

- **Framework:** .NET 8.0
- **UI Framework:** WPF (Windows Presentation Foundation)
- **Mimari:** MVVM (Model-View-ViewModel)
- **DI Container:** Microsoft.Extensions.DependencyInjection
- **Serialization:** System.Text.Json
- **Logging:** Serilog
- **Testing:** xUnit

### Önemli Sınıflar

- `CheckModel` - Çek veri modeli
- `NumberToTextConverter` - Sayı → Türkçe yazı dönüştürücü
- `PrinterService` - Yazdırma servisi
- `SettingsService` - Ayar yönetimi
- `CalibrationConfig` - Kalibrasyon ayarları

## 👨‍💻 Geliştirme

### Proje Yapısı

```bash
# Projeyi klonlama
git clone https://github.com/KULLANICI_ADIN/cek-yazdirma-uygulamasi.git
cd cek-yazdirma-uygulamasi

# Restore
dotnet restore

# Build
dotnet build

# Testleri çalıştırma
dotnet test

# Release build
dotnet publish CheckPrintApp.UI/CheckPrintApp.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

### Geliştirme Ortamı

- **IDE:** Visual Studio 2022 veya VS Code
- **.NET SDK:** 8.0 veya üzeri
- **Git:** Versiyon kontrolü için

### Testler

```bash
# Tüm testleri çalıştır
dotnet test

# Belirli bir test sınıfı
dotnet test --filter FullyQualifiedName~NumberToTextConverterTests

# Coverage ile
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Özellik Roadmap

### v1.0 (Mevcut) ✅
- [x] Temel çek yazdırma
- [x] Tutar → yazı dönüşümü
- [x] Önizleme
- [x] Kalibrasyon ayarları
- [x] Standalone exe

### v1.5 (Planlanan)
- [ ] Çoklu çek yazdırma (batch)
- [ ] Farklı banka şablonları
- [ ] Yazdırma geçmişi
- [ ] Gelişmiş kalibrasyon UI
- [ ] Test baskısı modu

### v2.0 (Gelecek)
- [ ] Diğer para birimleri
- [ ] Muhasebe yazılımı entegrasyonu
- [ ] Ağ paylaşımlı ayarlar
- [ ] Raporlama özellikleri

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen:

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Push edin (`git push origin feature/AmazingFeature`)
5. Pull Request açın

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 👤 İletişim

Proje Sahibi - [GitHub](https://github.com/KULLANICI_ADIN)

Proje Linki: [https://github.com/KULLANICI_ADIN/cek-yazdirma-uygulamasi](https://github.com/KULLANICI_ADIN/cek-yazdirma-uygulamasi)

## 🙏 Teşekkürler

- .NET Core ekibine
- WPF topluluğuna
- Tüm katılımcılara

---

⭐ **Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!**
