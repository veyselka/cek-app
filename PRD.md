# Product Requirements Document (PRD)
# Desktop Check Print & Calibration Master

**Version:** 1.0.0  
**Last Updated:** 19 Şubat 2026  
**Status:** Ready for Development  
**Owner:** Development Team

---

## 1. Executive Summary

### 1.1 Product Overview
Desktop Check Print & Calibration Master, işletmelerin banka çeklerini standart yazıcıları kullanarak hatasız ve profesyonel şekilde doldurmasını sağlayan, tamamen offline çalışan bir masaüstü uygulamasıdır.

### 1.2 Problem Statement
Mevcut çek yazdırma çözümleri:
- Web tabanlı olduğu için tarayıcı ayarlarına bağımlı ve yazdırma sırasında kayma sorunları yaşıyor
- Milimetrik hassasiyet gerektiren çek yazdırma işlemlerinde yetersiz kalıyor
- Her yazıcı için manuel ayar gerektiriyor ve ayarlar saklanmıyor
- Tutar yazıya çevirme işlemi manuel yapılıyor veya hatalı çevrimler oluyor

### 1.3 Solution
Masaüstü tabanlı, kalibrasyon odaklı bir uygulama ile:
- ✅ X ve Y ekseninde milimetrik kalibrasyon
- ✅ Ayarların kalıcı olarak saklanması
- ✅ Otomatik tutar-yazı dönüşümü
- ✅ Canlı önizleme (WYSIWYG)
- ✅ Test baskısı özelliği
- ✅ İnternet bağımsız çalışma

### 1.4 Success Criteria
- Yazıcı çıktısı ile ekran önizleme arasında maksimum 1mm sapma
- Tutar-yazı dönüşümü 100ms altında
- Kullanıcı tek seferlik kalibrasyon ile tüm çekleri hatasız yazdırabilmeli
- Uygulama 50MB altında ve kurulum gerektirmeden çalışabilmeli

---

## 2. Product Vision & Goals

### 2.1 Vision
Türkiye'deki tüm KOBİ'lerin ve finans departmanlarının tercih ettiği, güvenilir ve profesyonel çek yazdırma aracı olmak.

### 2.2 Goals
**Short-term (v1.0):**
- Temel çek yazdırma ve kalibrasyon özellikleri
- Türk Lirası desteği
- Tek sayfa (tek çek) yazdırma

**Mid-term (v1.5):**
- Çoklu çek yazdırma (batch)
- Çek şablonları (farklı bankalar için)
- Yazdırma geçmişi

**Long-term (v2.0):**
- Diğer para birimleri desteği
- Ağ üzerinden paylaşımlı ayarlar
- Muhasebe yazılımları ile entegrasyon

---

## 3. Target Users & Use Cases

### 3.1 Primary Users
1. **Muhasebeciler**
   - Günlük 10-50 çek işlemi
   - Hassasiyet ve hız odaklı
   - Teknik bilgi seviyesi: Orta

2. **Finans Departmanları**
   - Haftalık 20-100 çek işlemi
   - Profesyonel görünüm beklentisi
   - Çoklu kullanıcı ortamı

3. **KOBİ Sahipleri**
   - Haftalık 1-10 çek işlemi
   - Kullanım kolaylığı beklentisi
   - Teknik bilgi seviyesi: Düşük-Orta

### 3.2 Use Cases

#### UC-01: Yeni Çek Yazdırma
**Actor:** Muhasebeci  
**Precondition:** Uygulama açık, yazıcı bağlı  
**Flow:**
1. Kullanıcı tarih, alıcı ve tutar bilgilerini girer
2. Sistem otomatik olarak tutarı yazıya çevirir
3. Kullanıcı önizlemede kontrolü yapar
4. Çeki yazıcıya yerleştirir
5. "Yazdır" butonuna tıklar
6. Çek yazdırılır

**Postcondition:** Çek doğru formatta yazdırılmış olmalı

#### UC-02: İlk Kalibrasyon
**Actor:** Sistem Yöneticisi  
**Precondition:** Uygulama ilk kez kullanılıyor  
**Flow:**
1. Kullanıcı "Test Baskısı" seçeneğini işaretler
2. Boş bir kağıda test baskısı yapar
3. Çerçevelerin çek ile hizasını kontrol eder
4. X ve Y slider'larını ayarlar
5. Tekrar test baskısı yapar
6. Hizalama doğru olduğunda ayarları kaydeder

**Postcondition:** Kalibrasyon ayarları kaydedilmiş ve gelecek yazdırmalarda kullanılacak

#### UC-03: Keşide Yeri Güncelleme
**Actor:** Kullanıcı  
**Precondition:** Farklı şehirden çek keşide edilecek  
**Flow:**
1. Kullanıcı "Keşide Yeri" alanını manuel değiştirir
2. Önizleme güncellensin
3. Çek yazdırılır

**Postcondition:** O çek için keşide yeri özel olarak güncellenmiş olmalı

---

## 4. Functional Requirements

### 4.1 FR-01: Veri Girişi

#### FR-01.1 Tarih Girişi
- **Priority:** P0 (Critical)
- **Description:** Kullanıcı çek tarihi girebilmeli
- **Acceptance Criteria:**
  - DatePicker komponenti ile tarih seçilebilmeli
  - Manuel tarih girişi (DD/MM/YYYY formatında) yapılabilmeli
  - Geçersiz tarih girişlerinde hata mesajı gösterilmeli
  - Çıktı formatı: "DD / MM / YYYY" (aralarında boşluklu)
  - Varsayılan tarih: Bugünün tarihi olmalı

#### FR-01.2 Alıcı (Kime) Girişi
- **Priority:** P0 (Critical)
- **Description:** Çek alıcısının adı girilmeli
- **Acceptance Criteria:**
  - Serbest metin alanı (max 100 karakter)
  - Otomatik büyük harf dönüşümü seçeneği (varsayılan: açık)
  - Özel karakterler kabul edilmeli (ğ, ü, ş, ı, ö, ç)
  - Boş bırakılamaz validasyonu

#### FR-01.3 Tutar Girişi
- **Priority:** P0 (Critical)
- **Description:** Çek tutarı sayısal olarak girilmeli
- **Acceptance Criteria:**
  - Sadece sayısal değer ve ondalık ayracı (virgül) kabul edilmeli
  - Format: 0,00 (iki ondalık basamak)
  - Minimum değer: 0,01 TL
  - Maksimum değer: 999.999.999,99 TL
  - Binlik ayraç otomatik eklenebilmeli (1000 -> 1.000,00)
  - Güvenlik karakterleri (#) başa ve sona otomatik eklenecek: "#1.000,00#"

#### FR-01.4 Keşide Yeri
- **Priority:** P1 (High)
- **Description:** Çekin keşide edildiği yer girilmeli
- **Acceptance Criteria:**
  - Varsayılan değer: Ayarlar dosyasından okunmalı
  - Her çek için özelleştirilebilir olmalı
  - Serbest metin alanı (max 50 karakter)
  - Otomatik büyük harf dönüşümü

### 4.2 FR-02: Tutar-Yazı Dönüşümü

#### FR-02.1 Otomatik Dönüşüm
- **Priority:** P0 (Critical)
- **Description:** Girilen tutar otomatik olarak yazıya çevrilmeli
- **Acceptance Criteria:**
  - Tutar alanı değiştiğinde anlık güncellenmeli (max 100ms)
  - Format: "Yalnız [TAM_KISIM] Türk Lirası [KURUŞ_KISIM] Kuruş"
  - Tüm harfler büyük olmalı
  - Türkçe sayı okuma kurallarına uygun olmalı

#### FR-02.2 Dönüşüm Kuralları
- **Priority:** P0 (Critical)
- **Acceptance Criteria:**
  - 0,50 TL -> "Yalnız ELLİ Kuruş"
  - 1,00 TL -> "Yalnız BİR Türk Lirası SIFIR Kuruş"
  - 1.250,50 TL -> "Yalnız BİN İKİ YÜZ ELLİ Türk Lirası ELLİ Kuruş"
  - 1.000.000,00 TL -> "Yalnız BİR MİLYON Türk Lirası SIFIR Kuruş"
  - Özel durumlar: Bir, On, Yüz, Bin, Milyon, Milyar

### 4.3 FR-03: Kalibrasyon Sistemi

#### FR-03.1 X Ekseni Kalibrasyonu
- **Priority:** P0 (Critical)
- **Description:** Sol-sağ yönde milimetrik ayar
- **Acceptance Criteria:**
  - Slider kontrolü: -50mm ile +50mm arası
  - Adım değeri: 0.5mm
  - Pozitif değer: sağa kaydırma
  - Negatif değer: sola kaydırma
  - Gerçek zamanlı önizleme güncellemesi
  - Değer label'da gösterilmeli (örn: "+5.5 mm")

#### FR-03.2 Y Ekseni Kalibrasyonu
- **Priority:** P0 (Critical)
- **Description:** Yukarı-aşağı yönde milimetrik ayar
- **Acceptance Criteria:**
  - Slider kontrolü: -50mm ile +50mm arası
  - Adım değeri: 0.5mm
  - Pozitif değer: aşağı kaydırma
  - Negatif değer: yukarı kaydırma
  - Gerçek zamanlı önizleme güncellemesi
  - Değer label'da gösterilmeli

#### FR-03.3 Kalibrasyon Kaydetme
- **Priority:** P0 (Critical)
- **Description:** Ayarlar kalıcı olarak saklanmalı
- **Acceptance Criteria:**
  - JSON formatında yerel dosyaya kayıt (settings.json)
  - Uygulama kapanıp açıldığında ayarlar korunmalı
  - "Varsayılana Dön" butonu (X:0, Y:0)
  - "Kaydet" butonu ile manuel kayıt

### 4.4 FR-04: Test Baskısı

#### FR-04.1 Kılavuz Baskısı
- **Priority:** P1 (High)
- **Description:** Boş kağıt üzerine hizalama çerçeveleri basılmalı
- **Acceptance Criteria:**
  - Checkbox: "Test Baskısı (Kılavuz)"
  - İşaretli iken: Veri yerine dikdörtgen çerçeveler basılır
  - Çerçeveler: Tarih, Alıcı, Tutar alanlarını gösterir
  - Kalibrasyon ayarlarını etkiler (offset uygulanır)

### 4.5 FR-05: Canlı Önizleme

#### FR-05.1 WYSIWYG Önizleme
- **Priority:** P0 (Critical)
- **Description:** Ekranda görünen çıktıyla aynı olmalı
- **Acceptance Criteria:**
  - Sol panel: Veri girişi ve ayarlar
  - Sağ panel: Çek önizlemesi
  - Her değişiklik anında önizlemede görünmeli
  - Önizleme gerçek çek boyutlarında (proporsiyon korunmalı)
  - Font, boyut, pozisyon gerçek çıktı ile eşleşmeli

### 4.6 FR-06: Yazdırma

#### FR-06.1 Çek Yazdırma
- **Priority:** P0 (Critical)
- **Description:** Çek fiziksel yazıcıdan yazdırılmalı
- **Acceptance Criteria:**
  - PrintDialog kullanılmalı (kullanıcı yazıcı seçebilmeli)
  - Sayfa boyutu: Standart çek boyutu
  - Kalibrasyon offset'leri uygulanmalı
  - Önizleme ile çıktı arasında max 1mm tolerans
  - "Yazdır" butonuna tıklama ile başlamalı

#### FR-06.2 Yazdırma Validasyonu
- **Priority:** P1 (High)
- **Acceptance Criteria:**
  - Tüm zorunlu alanlar doldurulmalı
  - Yazıcı hazır olmalı
  - Hata durumunda açıklayıcı mesaj gösterilmeli

### 4.7 FR-07: Ayarlar Yönetimi

#### FR-07.1 Genel Ayarlar
- **Priority:** P1 (High)
- **Acceptance Criteria:**
  - Varsayılan keşide yeri ayarı
  - Otomatik büyük harf açma/kapama
  - Font seçimi (önerileri listelenecek)
  - Font boyutu ayarı
  - Ayarlar JSON dosyasına kaydedilecek

---

## 5. Non-Functional Requirements

### 5.1 NFR-01: Performance
- **Tutar-Yazı Dönüşümü:** < 100ms
- **Önizleme Güncellemesi:** < 50ms
- **Uygulama Başlatma:** < 3 saniye
- **Yazdırma İşlemi:** < 10 saniye

### 5.2 NFR-02: Reliability
- **Uptime:** %100 (offline uygulama)
- **Yazıcı Hatası Yönetimi:** Kullanıcı dostu hata mesajları
- **Veri Kaybı:** Ayarlar her değişiklikte otomatik kaydedilmeli
- **Crash Recovery:** Uygulama son durumdan devam etmeli

### 5.3 NFR-03: Usability
- **Öğrenme Süresi:** < 5 dakika (deneyimli kullanıcılar için)
- **İlk Kalibrasyon:** < 10 dakika
- **Dil:** Türkçe
- **Klavye Kısayolları:** Tab, Enter ile navigation
- **Erişilebilirlik:** Yüksek kontrastlı tema desteği

### 5.4 NFR-04: Compatibility
- **İşletim Sistemi:** Windows 10/11 (64-bit)
- **Yazıcılar:** Windows destekli tüm yazıcılar
- **Ekran Çözünürlüğü:** Min 1280x720
- **.NET Version:** .NET 6 veya 8

### 5.5 NFR-05: Security
- **Veri Şifreleme:** Ayarlar plain-text (hassas veri yok)
- **Offline Çalışma:** İnternet bağlantısı gerektirmez
- **Gizlilik:** Hiçbir veri dışarı gönderilmez

### 5.6 NFR-06: Portability
- **Kurulum Boyutu:** < 50MB
- **Bağımlılıklar:** Minimum (sadece .NET Runtime)
- **Portable Mod:** Kurulum gerektirmeden çalışabilmeli
- **Veritabanı:** Dosya tabanlı (JSON), dış DB gerektirmez

### 5.7 NFR-07: Maintainability
- **Kod Standardı:** C# coding conventions
- **Dokümantasyon:** Inline comments + developer guide
- **Test Coverage:** Min %70
- **Versiyon Kontrolü:** Git

---

## 6. Technical Architecture

### 6.1 Technology Stack

#### 6.1.1 Core Stack
- **Framework:** .NET 6 / .NET 8
- **Language:** C# 10+
- **UI Framework:** WPF (Windows Presentation Foundation)
- **UI Pattern:** MVVM (Model-View-ViewModel)
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection

#### 6.1.2 Libraries & Packages
- **JSON Handling:** System.Text.Json
- **Logging:** Serilog
- **Testing:** xUnit + Moq
- **UI Components:** Material Design In XAML Toolkit (opsiyonel)

### 6.2 Application Architecture

```
CheckPrintApp/
├── CheckPrintApp.Core/
│   ├── Models/
│   │   ├── CheckModel.cs
│   │   ├── CalibrationConfig.cs
│   │   └── AppSettings.cs
│   ├── Services/
│   │   ├── INumberToTextConverter.cs
│   │   ├── NumberToTextConverter.cs
│   │   ├── IPrinterService.cs
│   │   ├── PrinterService.cs
│   │   ├── ISettingsService.cs
│   │   └── SettingsService.cs
│   └── Helpers/
│       ├── UnitConverter.cs (mm to pixel)
│       └── ValidationHelper.cs
├── CheckPrintApp.UI/
│   ├── ViewModels/
│   │   ├── MainViewModel.cs
│   │   ├── SettingsViewModel.cs
│   │   └── ViewModelBase.cs
│   ├── Views/
│   │   ├── MainWindow.xaml
│   │   ├── SettingsWindow.xaml
│   │   └── PreviewControl.xaml
│   ├── Converters/
│   │   └── CurrencyConverter.cs
│   └── App.xaml
└── CheckPrintApp.Tests/
    ├── Services/
    │   ├── NumberToTextConverterTests.cs
    │   └── PrinterServiceTests.cs
    └── Helpers/
        └── UnitConverterTests.cs
```

### 6.3 Class Diagram (Core Components)

#### CheckModel
```csharp
public class CheckModel
{
    public DateTime Date { get; set; }
    public string PayeeName { get; set; }
    public decimal Amount { get; set; }
    public string Location { get; set; }
    public string AmountInWords => /* Calculated */
}
```

#### CalibrationConfig
```csharp
public class CalibrationConfig
{
    public double OffsetX { get; set; } // mm
    public double OffsetY { get; set; } // mm
    public string DefaultLocation { get; set; }
    public string FontFamily { get; set; }
    public double FontSize { get; set; }
    public bool AutoUpperCase { get; set; }
}
```

#### INumberToTextConverter
```csharp
public interface INumberToTextConverter
{
    string Convert(decimal amount);
}
```

#### IPrinterService
```csharp
public interface IPrinterService
{
    void PrintCheck(CheckModel check, CalibrationConfig config, bool isTestPrint);
    bool ValidatePrinterStatus();
}
```

### 6.4 Data Flow

```
User Input (View)
    ↓
ViewModel (Data Binding)
    ↓
Model (CheckModel)
    ↓
Service (NumberToTextConverter)
    ↓
ViewModel (Update UI)
    ↓
Preview (Live Update)
    ↓
Print Button
    ↓
PrinterService
    ↓
Physical Printer
```

### 6.5 Coordinate System & Calibration Logic

```csharp
// Base coordinates for check fields (without calibration)
const double BaseDateX = 120; // mm from left
const double BaseDateY = 30;  // mm from top

// Apply calibration
double ActualX = BaseDateX + CalibrationConfig.OffsetX;
double ActualY = BaseDateY + CalibrationConfig.OffsetY;

// Convert to pixels for printing
double PixelX = MmToPixel(ActualX);
double PixelY = MmToPixel(ActualY);
```

### 6.6 Unit Conversion

```csharp
public static class UnitConverter
{
    private const double DPI = 96.0; // WPF default
    private const double MM_PER_INCH = 25.4;
    
    public static double MmToPixel(double mm)
    {
        return (mm * DPI) / MM_PER_INCH;
    }
    
    public static double PixelToMm(double pixel)
    {
        return (pixel * MM_PER_INCH) / DPI;
    }
}
```

---

## 7. User Interface Design

### 7.1 Main Window Layout

```
┌─────────────────────────────────────────────────────────────┐
│  Desktop Check Print Master                          [_][□][X]│
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌─────────────────────┐  ┌─────────────────────────────┐  │
│  │   Çek Bilgileri     │  │      Önizleme               │  │
│  │                     │  │                             │  │
│  │  Tarih:             │  │   ┌───────────────────┐    │  │
│  │  [19/02/2026]  📅   │  │   │                   │    │  │
│  │                     │  │   │   ÇEK ÖNİZLEME    │    │  │
│  │  Kime:              │  │   │                   │    │  │
│  │  [AHMET YILMAZ  ]   │  │   │  Tarih: ___/___/_ │    │  │
│  │                     │  │   │  Alıcı: _________ │    │  │
│  │  Tutar:             │  │   │  Tutar: ####,##   │    │  │
│  │  [1.250,50 TL]      │  │   │  Yazıyla: ______  │    │  │
│  │                     │  │   │                   │    │  │
│  │  Yazıyla:           │  │   └───────────────────┘    │  │
│  │  Yalnız BİN İKİ...  │  │                             │  │
│  │                     │  │                             │  │
│  │  Keşide Yeri:       │  │                             │  │
│  │  [ELAZIĞ]           │  │                             │  │
│  │                     │  │                             │  │
│  ├─────────────────────┤  └─────────────────────────────┘  │
│  │ Kalibrasyon Ayarı   │                                    │
│  │                     │                                    │
│  │  X (Sol/Sağ):       │  [  -50mm ═══●═══ +50mm  ]        │
│  │  [-5.5 mm]          │                                    │
│  │                     │                                    │
│  │  Y (Yukarı/Aşağı):  │  [  -50mm ═══●═══ +50mm  ]        │
│  │  [+2.0 mm]          │                                    │
│  │                     │                                    │
│  │  ☐ Test Baskısı     │                                    │
│  │                     │                                    │
│  │  [Varsayılana Dön]  │  [ Ayarları Kaydet ]              │
│  │                     │                                    │
│  └─────────────────────┘                                    │
│                                                               │
│  [⚙ Ayarlar]                              [🖨 YAZDIR]       │
└─────────────────────────────────────────────────────────────┘
```

### 7.2 Settings Window

```
┌───────────────────────────────────────┐
│  Ayarlar                      [_][X]  │
├───────────────────────────────────────┤
│                                       │
│  Genel Ayarlar                        │
│  ─────────────                        │
│                                       │
│  Varsayılan Keşide Yeri:              │
│  [ELAZIĞ                          ]   │
│                                       │
│  ☑ Otomatik Büyük Harf                │
│                                       │
│                                       │
│  Görünüm Ayarları                     │
│  ─────────────────                    │
│                                       │
│  Font:                                │
│  [Arial                    ▼]         │
│                                       │
│  Font Boyutu:                         │
│  [12                       ▼]         │
│                                       │
│                                       │
│         [İptal]        [Kaydet]       │
│                                       │
└───────────────────────────────────────┘
```

### 7.3 UI/UX Principles

1. **Simplicity:** Tek ekranda tüm işlemler yapılabilmeli
2. **Immediate Feedback:** Her değişiklik anında önizlemede görünmeli
3. **Error Prevention:** Invalid girişler engellenmeli
4. **Clear Visual Hierarchy:** Birincil aksiyon belirgin olmalı (YAZDIR butonu)
5. **Accessibility:** Klavye ile full navigation
6. **Responsive:** Farklı ekran boyutlarında düzgün görünmeli

---

## 8. Data Management

### 8.1 Settings File Structure (settings.json)

```json
{
  "version": "1.0.0",
  "calibration": {
    "offsetX": -5.5,
    "offsetY": 2.0
  },
  "general": {
    "defaultLocation": "ELAZIĞ",
    "autoUpperCase": true,
    "fontFamily": "Arial",
    "fontSize": 12
  },
  "lastCheck": {
    "date": "2026-02-19",
    "payeeName": "AHMET YILMAZ",
    "amount": 1250.50,
    "location": "ELAZIĞ"
  }
}
```

### 8.2 File Location
- **Windows:** `%APPDATA%\CheckPrintMaster\settings.json`
- **Portable Mode:** `.\config\settings.json` (exe ile aynı klasör)

---

## 9. Testing Strategy

### 9.1 Unit Tests
- NumberToTextConverter: Tüm edge case'ler (0, 0.01, 999999999.99)
- UnitConverter: mm-pixel dönüşümleri
- Validation: Tutar formatı, tarih validasyonu

### 9.2 Integration Tests
- Settings kaydetme/yükleme
- PrinterService mock testleri

### 9.3 Manual Testing
- Gerçek yazıcı testleri (farklı markalar)
- Kalibrasyon hassasiyet testleri
- Farklı çek formatları ile test

### 9.4 Test Cases (Critical)

| Test ID | Scenario | Expected Result |
|---------|----------|-----------------|
| TC-01 | 1250,50 TL yazıya çevir | "Yalnız BİN İKİ YÜZ ELLİ Türk Lirası ELLİ Kuruş" |
| TC-02 | X offset +5mm uygula | İçerik 5mm sağa kaymalı |
| TC-03 | Y offset -3mm uygula | İçerik 3mm yukarı kaymalı |
| TC-04 | Test baskısı yap | Sadece çerçeveler basılmalı |
| TC-05 | Ayarları kaydet/yükle | Uygulama açılınca ayarlar korunmalı |

---

## 10. Success Metrics & KPIs

### 10.1 Technical Metrics
- **Yazdırma Hassasiyeti:** Max 1mm sapma (Target: %95 başarı)
- **Performance:** Tutar-Yazı < 100ms (Target: %100 başarı)
- **Crash Rate:** < 1% (Target: 0 crash)
- **Hata Oranı:** Yanlış çek basımı < 0.5%

### 10.2 User Metrics
- **İlk Kullanım Başarısı:** %90 kullanıcı ilk denemede başarılı çek basmalı
- **Kalibrasyon Süresi:** Ortalama < 5 dakika
- **Kullanıcı Memnuniyeti:** NPS > 50
- **Ortalama Kullanım Süresi:** < 2 dakika/çek

---

## 11. Release Plan

### 11.1 Version 1.0.0 (MVP) - Target: 4 Hafta

**Sprint 1 (Week 1):**
- Project setup
- Core models & services
- NumberToTextConverter implementation

**Sprint 2 (Week 2):**
- WPF UI implementation
- Data binding & MVVM setup
- Live preview

**Sprint 3 (Week 3):**
- Calibration system
- Settings management
- PrinterService implementation

**Sprint 4 (Week 4):**
- Testing & bug fixes
- Deployment setup
- Documentation

### 11.2 Version 1.1.0 (Enhancements) - Target: +2 Hafta
- Çoklu çek yazdırma
- Yazdırma geçmişi
- Şablon desteği

### 11.3 Version 2.0.0 (Advanced) - Target: +4 Hafta
- Diğer para birimleri
- Cloud sync (opsiyonel)
- Advanced reporting

---

## 12. Risks & Mitigation

| Risk | Probability | Impact | Mitigation |
|------|------------|--------|------------|
| Yazıcı DPI farklılıkları | High | High | Dinamik DPI algılama, test suite |
| Font rendering farklılıkları | Medium | Medium | Standart font kullanımı, fallback mekanizması |
| Windows versiyon uyumsuzluğu | Low | Medium | .NET 6/8 kullanımı, geniş test |
| Performans sorunları | Low | Low | Profiling, optimizasyon |

---

## 13. Dependencies & Assumptions

### 13.1 Dependencies
- ✅ Windows 10/11 OS
- ✅ .NET 6 veya 8 Runtime
- ✅ Yazıcı driver'ları yüklü
- ✅ Standart çek boyutları (değişmez)

### 13.2 Assumptions
- ✅ Kullanıcılar temel bilgisayar kullanımı biliyor
- ✅ Yazıcılar Windows Print API'sini destekliyor
- ✅ Çek formatları banka standardına uygun
- ✅ Kullanıcılar kalibrasyon kavramını anlayabiliyor

---

## 14. Glossary

- **Kalibrasyon:** Yazıcı çıktısını çek formuna hizalama işlemi
- **Offset:** Temel koordinattan sapma değeri (mm cinsinden)
- **Test Baskısı:** Veri yerine hizalama çerçevelerinin basılması
- **WYSIWYG:** What You See Is What You Get - Ekrandaki ile çıktının aynı olması
- **DPI:** Dots Per Inch - İnç başına nokta sayısı
- **Keşide Yeri:** Çekin düzenlendiği şehir
- **Alacaklı/Emrine:** Çek alıcısının adı

---

## 15. Approval

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Product Owner | - | - | - |
| Lead Developer | - | - | - |
| QA Lead | - | - | - |

---

**Document End**

*Bu PRD, Desktop Check Print & Calibration Master v1.0.0 için tam gereksinim setini içermektedir. Değişiklikler versiyon kontrolü ile takip edilecektir.*
