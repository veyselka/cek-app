# **YAZILIM GEREKSİNİM SPESİFİKASYONU (SRS)**

**Proje Adı:** Desktop Check Print & Calibration Master

**Versiyon:** 1.0.0

**Tür:** Masaüstü Uygulaması (Offline / Standalone)

## **1\. PROJE ÖZETİ VE KAPSAMI**

### **1.1. Amaç**

Bu proje, işletmelerin banka çeklerini standart yazıcıları kullanarak hatasız, hızlı ve profesyonel bir şekilde doldurmasını sağlayan internetten bağımsız bir masaüstü yazılımıdır.

### **1.2. Kapsam**

Yazılım; kullanıcıdan çek üzerindeki tarih, alıcı, tutar ve keşide yeri bilgilerini alacak, tutarı otomatik olarak yazıya çevirecek ve yazıcıdan kaynaklanan kağıt kaymalarını önlemek için X ve Y eksenlerinde milimetrik kalibrasyon imkanı sunacaktır.

### **1.3. Hedef Kitle**

Finans departmanları, muhasebeciler ve çek kullanan KOBİ'ler.

## **2\. İŞLEVSEL GEREKSİNİMLER (FUNCTIONAL REQUIREMENTS)**

### **2.1. Veri Giriş ve İşleme Modülü**

* **Keşide Yeri (Otomatik/Manuel):**  
  * Önizlemede "ELAZIĞ" olarak görünen alan, varsayılan olarak Ayarlar dosyasından okunmalı, ancak işlem anında değiştirilebilir olmalıdır.  
* **Tarih Yönetimi:**  
  * Kullanıcı tarih seçici (DatePicker) kullanabilmeli veya manuel giriş yapabilmelidir.  
  * Çıktı formatı, çek standartlarına uygun olarak aralarında boşluklu (Örn: 19 / 02 / 2026\) basılmalıdır.  
* **Alıcı (Kime/Emrine):**  
  * Serbest metin alanı. Büyük harf zorunluluğu opsiyonel olarak sunulmalı (Girilen "ahmet" verisi "AHMET" olarak basılabilmeli).  
* **Tutar (Sayısal) Girişi:**  
  * Sadece sayısal değer ve ondalık ayracı (virgül/nokta) kabul etmelidir.  
  * Görseldeki gibi \# karakterleri (Örn: \#45.234,00\#) güvenlik amacıyla otomatik olarak başa ve sona eklenmelidir.  
* **Tutarın Yazıya Çevrilmesi (Algoritma):**  
  * Sayısal alan değiştiği anda (OnTextChanged event), "Yazı ile Tutar" alanı senkronize güncellenmelidir.  
  * **Kural Seti:**  
    * Tam kısım ve ondalık kısım ayrılmalıdır.  
    * Para birimi (Türk Lirası) ve Kuruş ifadeleri sona eklenmelidir.  
    * Güvenlik için kelime başına "Yalnız" ibaresi konulmalıdır.  
    * Büyük harf standardı uygulanmalıdır.  
    * *Örnek:* 1250,50 \-\> "Yalnız BİN İKİ YÜZ ELLİ Türk Lirası ELLİ Kuruş"

### **2.2. Kalibrasyon ve Yazdırma Ayarları (Kritik Modül)**

Bu modül projenin "kalbi" niteliğindedir. Tarayıcı tabanlı sistemlerdeki kayma sorununu çözecek kısımdır.

* **X Ekseni (Sol-Sağ) Slider:**  
  * **Aralık:** \-50mm ile \+50mm arası.  
  * **Mantık:** Pozitif değer içeriği sağa, negatif değer sola kaydırır.  
  * **Teknik Karşılık:** Graphics.DrawString metodundaki X koordinatına \+ (SliderValue \* DPI\_Conversion\_Factor) eklenir.  
* **Y Ekseni (Yukarı-Aşağı) Slider:**  
  * **Aralık:** \-50mm ile \+50mm arası.  
  * **Mantık:** Pozitif değer içeriği aşağı, negatif değer yukarı taşır.  
* **Ayarları Kaydetme:**  
  * Kullanıcı her yazdırma işleminden sonra kalibrasyon ayarlarını tekrar girmemelidir. Son kullanılan X/Y değerleri yerel bir konfigürasyon dosyasında (JSON/XML) saklanmalıdır.  
* **Test Baskısı (Kılavuz):**  
  * Bu seçenek işaretlendiğinde, çekin üzerine veri yerine, verinin basılacağı alanları gösteren dikdörtgen çerçeveler (bounding boxes) basılmalıdır. Bu, boş kağıt üzerinde hizalama testi yapmak içindir.

### **2.3. Canlı Önizleme (Live Preview)**

* WYSIWYG (What You See Is What You Get) prensibi uygulanacaktır.  
* Sol panelde yapılan her değişiklik (yazı veya kaydırma), sağdaki panelde sanal bir çek görseli üzerinde anlık olarak render edilmelidir.

## **3\. İŞLEVSEL OLMAYAN GEREKSİNİMLER (NON-FUNCTIONAL)**

* **Performans:** Tutar-Yazı dönüşümü 100ms'nin altında gerçekleşmelidir.  
* **Güvenilirlik:** Yazılım internet bağlantısı olmadan %100 fonksiyonel çalışmalıdır.  
* **Hassasiyet:** Yazıcı çıktısı ile ekrandaki koordinatlar arasındaki sapma payı en fazla 1mm olmalıdır.  
* **Kurulum:** "Portable" (taşınabilir) veya tek tıkla kurulan (.msi) hafif bir yapıda olmalıdır. Veritabanı kurulumu gerektirmemelidir.

## **4\. TEKNİK MİMARİ VE TASARIM ÖNERİLERİ**

Görseldeki arayüzü ve teknik ihtiyaçları karşılamak için en uygun teknoloji yığını aşağıdadır:

### **4.1. Teknoloji Yığını (Tech Stack)**

* **Programlama Dili:** C\# (.NET 6 veya .NET 8\)  
* **UI Framework:** **WPF (Windows Presentation Foundation)**  
  * *Neden WPF?* WinForms piksel tabanlıdır ve yüksek DPI'lı ekranlarda veya hassas yazıcı çıktılarında ölçekleme sorunu yaşatabilir. WPF vektörel tabanlıdır; ekrandaki görüntüyü yazıcıya (FixedDocument) aktarırken milimetrik kontrol sağlar. XAML ile görseldeki modern arayüzü tasarlamak çok daha kolaydır.  
* **Veri Saklama:** settings.json (Basit dosya tabanlı saklama).

### **4.2. Önerilen Sınıf (Class) Yapısı (OOP)**

Projenin spagetti koda dönüşmemesi için şu yapıları kullanmanızı öneririm:

#### **A. Model Katmanı**

Veriyi taşıyan yapıdır.

public class CheckModel  
{  
    public DateTime Date { get; set; }  
    public string PayeeName { get; set; } // Kime  
    public decimal Amount { get; set; }   // Tutar  
    public string Location { get; set; }  // Keşide Yeri (Elazığ vb.)  
      
    // Tutarın yazıya çevrilmiş hali (Read-only property)  
    public string AmountInWords \=\> NumberToTextConverter.Convert(Amount);  
}

#### **B. Servis Katmanı (Business Logic)**

Mantıksal işlemlerin yapıldığı yer.

**1\. NumberToTextConverter (Helper Class):**

Girilen sayıyı Türkçe metne çeviren statik sınıf.

public static class NumberToTextConverter  
{  
    public static string Convert(decimal amount)  
    {  
        // 1\. Tam ve ondalık kısımları ayır.  
        // 2\. Basamak analizi yap (Birler, Onlar, Yüzler...).  
        // 3\. "Yalnız" ve "Türk Lirası" ekle.  
        // 4\. Return string.  
    }  
}

**2\. CalibrationSettings (Config Class):**

Ayarları yöneten sınıf.

public class CalibrationConfig  
{  
    public double OffsetX { get; set; } // Sol-Sağ mm  
    public double OffsetY { get; set; } // Yukarı-Aşağı mm  
    public string DefaultLocation { get; set; } // Varsayılan Şehir

    public void Save() { /\* JSON'a yaz \*/ }  
    public static CalibrationConfig Load() { /\* JSON'dan oku \*/ }  
}

**3\. PrinterService (Yazdırma Motoru):**

WPF'in PrintDialog ve DrawingVisual sınıflarını kullanan servis.

public class PrinterService  
{  
    public void PrintCheck(CheckModel data, CalibrationConfig config, bool isTestPrint)  
    {  
        // 1\. Yazıcıyı hazırla.  
        // 2\. Kağıt boyutunu (Çek boyutu) ayarla.  
        // 3\. Koordinatları hesapla:  
        //    DateX \= BaseDateX \+ config.OffsetX;  
        // 4\. Graphics nesnesine çizim yap (DrawString).  
        // 5\. Yazdır.  
    }  
}

### **4.3. Kritik Geliştirme İpuçları**

1. **Ölçü Birimi Dönüşümü:** WPF 96 DPI (Device Independent Pixels) kullanır. Yazıcılar genellikle 300 veya 600 DPI çalışır. Kodlama yaparken milimetreyi piksele çeviren bir fonksiyon yazmalısınız:  
   double MmToPixel(double mm) { return (mm \* 96\) / 25.4; }  
2. **Font Seçimi:** Çek üzerindeki yazıların banka standartlarına (genellikle Monospace veya OCR-B benzeri fontlar veya okunaklı Sans-Serif) uygun olması için sistemde yüklü fontları kontrol ettirin.  
3. **Input Masking:** Tutar girişine sadece rakam girilmesini sağlamak için "Regex" veya WPF kütüphaneleri (örn: MaskedTextBox) kullanın.