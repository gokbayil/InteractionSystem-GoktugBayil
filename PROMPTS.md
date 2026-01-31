# LLM Kullanım Dokümantasyonu

> Bu dosya, Ludu Arts Unity Intern Case sürecinde Google Gemini ile gerçekleştirilen kod refactoring ve hata ayıklama süreçlerini belgelemektedir.

## Özet

| Bilgi | Değer |
| --- | --- |
| Toplam prompt sayısı | 3 |
| Kullanılan araçlar | Google Gemini |
| En çok yardım alınan konular | C# Namespace Mimarisi, Debugging, Clean Code |
| Tahmini LLM ile kazanılan süre | 1.5 - 2 Saat |

---

## Prompt 1: Namespace Hatası ve Derleme Sorunu

**Araç:** Google Gemini
**Tarih/Saat:** 31.01.2026 05:42

**Prompt:**

```
namespace'ini düzeltmen için sana gönderdiğim kodu bana hatalı iletmişsin, attığın kod 119. satırda hata veriyor

```

*(Not: Öncesinde kodlar paylaşılmıştı ve DoorInteractable script'i KeyInventory'ye erişemiyordu)*

**Alınan Cevap (Özet):**

```
Hatanın sebebi 'DoorInteractable' scriptinin bir Namespace içinde olması, ancak 'KeyInventory'
scriptinin Global Namespace'de kalmasıdır. Kodun derlenebilmesi için KeyInventory'nin
'InteractionSystem.Runtime.Player' namespace'ine taşınması ve gerekli 'using'
referanslarının eklenmesi gerekmektedir. 4 dosya için düzeltme kodu iletildi.

```

**Nasıl Kullandım:**

* [ ] Direkt kullandım
* [x] Adapte ettim (Mantığı anladım ancak tüm proje geneline yaymak istedim)
* [ ] Reddettim

**Açıklama:**

> `DoorInteractable.cs` içindeki 119. satırda `KeyInventory` sınıfına ulaşılamıyordu. LLM, sorunun bir yazım hatası değil, C# Namespace kapsamı (scope) sorunu olduğunu tespit etti. Parça parça düzeltmek yerine tüm sistemi bu yapıya geçirmeye karar verdim.

---

## Prompt 2: Kapsamlı Kod Düzenlemesi (Refactoring)

**Araç:** Google Gemini
**Tarih/Saat:** 31.01.2026 05:49

**Prompt:**

```
scriptlerimin editördeki hali görseldeki gibi, kodları da iletiyorum teker teker
hepsinin namespace'lerini düzeltip tekrar ilet. gerektiği kadar düşün, acele etme

```

**Alınan Cevap (Özet):**

```
Projedeki 9 adet C# scripti incelendi. Kodlar 'InteractionSystem.Runtime' çatısı altında;
Core (Temel yapılar), Player (Oyuncu ve Envanter), UI (Arayüz) ve Interactables (Etkileşimli Objeler)
olmak üzere 4 farklı Namespace'e ayrıldı. Eksik 'using' direktifleri tamamlandı ve scriptler arası
bağımlılıklar (Dependency) giderilerek modüler hale getirildi.

```

**Nasıl Kullandım:**

* [x] Direkt kullandım
* [ ] Adapte ettim
* [ ] Reddettim

**Açıklama:**

> Projedeki scriptlerin birbiriyle iletişiminde sürekli "Missing Reference" veya "Type not found" hataları almamak için LLM'den tüm mimariyi profesyonel bir yapıya (Namespace Architecture) oturtmasını istedim. Verdiği kodları birebir projeme uygulayarak temiz ve hatasız bir yapı elde ettim.

---

## Genel Değerlendirme

### LLM'in En Çok Yardımcı Olduğu Alanlar

1. **Kod Mimarisi (Namespace Yönetimi):** Scriptlerin birbirine karışmasını önlemek için klasör yapısına uygun Namespace kurgusunu hızlıca oluşturdu.
2. **Hata Ayıklama (Debugging):** Derleyici hatalarının (Compiler Errors) kaynağını (Scope hatası) saniyeler içinde tespit etti.

### LLM'in Yetersiz Kaldığı Alanlar

1. **Unity Editör Referansları:** Kod yapısı değiştiğinde (Namespace eklendiğinde) Unity Inspector üzerindeki script referanslarının koptuğunu (Missing Script) ve tekrar atanması gerektiğini LLM kodu yazarken fark edemez, bunu manuel kontrol etmem gerekti (ancak uyarı olarak belirtti).

### LLM Kullanımı Hakkında Düşüncelerim

> Bu case boyunca LLM'i sadece "kod yazdırıp hazıra konayım" gibi değil de koddaki ufak hataları gidermesi için kullandım. Özellikle Namespace yapısını kurgularken, manuel olarak 9 dosyayı tek tek düzenleyip birbirine bağlamak ciddi zaman alacaktı. LLM sayesinde bu süreci hızlandırıp, asıl odaklanmam gereken oyun mekaniklerine ve mantığına zaman ayırabildim. Öğrendiğim en önemli şey, sorunu parça parça sormak yerine (Prompt 2'deki gibi) tüm bağlamı (Context) vererek genel bir çözüm istemenin daha tutarlı sonuçlar ürettiği oldu.

---

## Notlar

* Scriptlerin Namespace içine alınması, Unity'de `AddComponent` menüsünde daha düzenli durmalarını sağladı.
* `DoorInteractable` scriptindeki hata, aslında OOP prensiplerine daha sıkı bağlı kalmam gerektiğini hatırlattı. En baştan daha planlı başlasaydım böyle zaman kaybetmeyecektim.