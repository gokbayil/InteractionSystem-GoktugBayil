# Interaction System - [Göktuğ Bayil]

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Bilgi | Değer |
|-------|-------|
| Unity Versiyonu | 6000.0.23f1 |
| Render Pipeline | Built-in / URP / HDRP |
| Case Süresi | 12 saat |
| Tamamlanma Oranı | %100 |

---

## Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/gokbayil/InteractionSystem-GoktugBayil.git
```

2. Unity Hub'da projeyi açın
3. `Assets/[InteractionSystem]/Scenes/TestScene.unity` sahnesini açın
4. Play tuşuna basın

---

## Nasıl Test Edilir

### Kontroller

| Tuş | Aksiyon |
|-----|---------|
| WASD | Hareket |
| Mouse | Bakış yönü |
| E | Etkileşim |

### Test Senaryoları

1. **Door Test:**
   - Kapıya yaklaşın, "Press E to Open" mesajını görün
   - E'ye basın, kapı açılsın
   - Tekrar basın, kapı kapansın

2. **Key + Locked Door Test:**
   - Kilitli kapıya yaklaşın, "Locked - Key Required" mesajını görün
   - Anahtarı bulun ve toplayın
   - Kilitli kapıya geri dönün ve E'ye basın, şimdi açılabilir olmalı

3. **Switch Test:**
   - Switch'e yaklaşın ve aktive edin
   - Bağlı nesnenin (kapı) tetiklendiğini görün

4. **Chest Test:** (Asset Store'um çöktüğü için chest test kapı üzerinde gerçekleştirilebilir.)
   - Kapıya yaklaşın
   - E'ye basılı tutun, progress bar dolsun
   - Kapı açılsın.

---

# Mimari Kararlar

### Interaction System Yapısı

    Player[Player / InteractionDetector] -- Raycast --> I[<< Interface >>\nIInteractable]
    I <|.. Door[DoorInteractable]
    I <|.. Chest[ChestInteractable]
    I <|.. Button[ButtonInteractable]
    I <|.. KeyItem[KeyCollectable]
    
    Door -.->|Check Key| Inventory[KeyInventory \n(Singleton)]
    Inventory -.->|Update UI| UIManager[UIManager \n(Singleton)]

```

**Neden bu yapıyı seçtim:**

> Projenin genişletilebilir (scalable) ve modüler olması için Interface-based (Arayüz tabanlı) bir yapı tercih ettim. Oyuncu (InteractionDetector), baktığı nesnenin kapı mı, sandık mı yoksa bir buton mu olduğunu bilmek zorunda değildir; sadece o nesnenin IInteractable olup olmadığını bilir. Bu sayede InteractionDetector kodunu hiç değiştirmeden oyuna yeni etkileşimli objeler (örn: Lever, NPC) eklenebilir. Bu yapı, Loose Coupling prensibine uygundur.

**Alternatifler:**

> Inheritance: Tüm objeler için BaseInteractable adında bir ana sınıf (parent class) oluşturulabilirdi. Ancak C# multiple inheritance desteklemediği için, ileride bir obje hem hasar alabilir hem de etkileşime geçilebilir olması gerektiğinde Inheritance yapısı tıkanacaktı. Interface yapısı bu esnekliği sağlar.

**Trade-off'lar:**

> Avantaj: Kod bağımlılığı azaldı, yeni özellik eklemek kolaylaştı.
> Dezavantaj: Her yeni obje için Interact() fonksiyonunu tekrar implemente etmek (Boilerplate code) gerekiyor. Ortak kodlar (örn: mesafe kontrolü) bir Base Class'ta toplanabilirdi ancak esneklik adına Interface tercih edildi.

### Kullanılan Design Patterns

| Pattern | Kullanım Yeri | Neden |
| Singleton | KeyInventory, UIManager | Envanter ve UI gibi yöneticilerin sahnede tek olması ve diğer sınıflardan (DoorInteractable) kolayca erişilebilmesi için. |
| Strategy | IInteractable | Etkileşim mantığını (Kapı açılması, Sandık animasyonu, Eşya toplama) nesnelere göre ayrıştırıp, oyuncunun tek bir arayüz üzerinden farklı davranışları tetiklemesini sağlamak için. |
| Dependency Injection | Inspector References | DoorInteractable scriptinde AudioClip veya Key gibi bağımlılıkların kod içinde new ile yaratılmak yerine Inspector üzerinden enjekte edilmesi. |

---

## Ludu Arts Standartlarına Uyum

### C# Coding Conventions

| Kural | Uygulandı | Notlar |
| m_ prefix (private fields) | [x] | m_KeyIds, m_OpenAngle gibi private değişkenlerde uygulandı. |
| s_ prefix (private static) | [x] | Singleton instance'larında (s_Instance) kullanıldı. |
| k_ prefix (private const) | [x] | Sabit değerlerde (k_RotationTolerance) kullanıldı. |
| Region kullanımı | [x] | Fields, Properties, Unity Methods ve Public Methods ayrıştırıldı. |
| Region sırası doğru | [x] | Standart hiyerarşiye uyuldu. |
| XML documentation | [x] | Karmaşık bulunan yerlere rahat okunabilmesi için comment satırları eklenti |
| Silent bypass yok | [x] | Hata durumlarında Debug.LogWarning ile geri bildirim verildi, boş try-catch kullanılmadı. |
| Explicit interface impl. | [ ] | Unity Event sistemleri ve Inspector erişimi kolaylığı için Implicit (public void Interact) tercih edildi. |

### Naming Convention

| Kural | Uygulandı | Örnekler |
| P_ prefix (Prefab) | [x] | `P_KeyCollectable`, `P_DoorInteractable` |
| M_ prefix (Material) | [x] | `M_Door_Wood`, `M_Chest_Gold` |
| T_ prefix (Texture) | [ ] | |
| SO isimlendirme | [x] | `SO_Key_Red`, `SO_Key_Blue` |

### Prefab Kuralları

| Kural | Uygulandı | Notlar |
| Transform (0,0,0) | [x] | Tüm prefablar origin noktasında oluşturuldu. |
| Pivot bottom-center | [x] | Kapı ve masa gibi objelerin pivotları zemin hizasında ayarlandı. |
| Collider tercihi | [x] | Performans için MeshCollider yerine Primitive (Box/Sphere) Colliderlar tercih edildi. |
| Hierarchy yapısı | [x] | Model ve Logic (Script) objeleri temiz bir hiyerarşide tutuldu. |

### Zorlandığım Noktalar

> **Namespace Yönetimi:** Scriptlerin InteractionSystem.Runtime... gibi uzun namespace'ler altına alınması ve bunların birbirini görmesi sırasında using direktiflerini yönetmek başlangıçta karmaşık geldi ancak IDE ve LLM önerileriyle alıştım.
> **Private Field Prefix (m_):** Alışkanlık gereği değişkenleri direkt private float speed şeklinde tanımlamaya meyilliydim, her seferinde m_Speed formatına refactor etmek disiplin gerektirdi.

## Tamamlanan Özellikler

### Zorunlu (Must Have)

- [x] / [x] Core Interaction System
  - [x] / [x] IInteractable interface
  - [x] / [x] InteractionDetector
  - [x] / [x] Range kontrolü

- [x] / [x] Interaction Types
  - [x] / [x] Instant
  - [x] / [x] Hold
  - [x] / [x] Toggle

- [x] / [x] Interactable Objects
  - [x] / [x] Door (locked/unlocked)
  - [x] / [x] Key Pickup
  - [x] / [x] Switch/Lever
  - [x] / [x] Chest/Container (Asset Store'um çöktüğü için chest interaction'ını kapı üzerinde gerçekleştirdim.)

- [x] / [x] UI Feedback
  - [x] / [x] Interaction prompt
  - [x] / [x] Dynamic text
  - [x] / [x] Hold progress bar
  - [x] / [x] Cannot interact feedback

- [x] / [x] Simple Inventory
  - [x] / [x] Key toplama
  - [x] / [x] UI listesi

### Bonus (Nice to Have)

- [x] Animation entegrasyonu
- [x] Sound effects
- [x] Multiple keys / color-coded
- [ ] Interaction highlight
- [ ] Save/Load states
- [x] Chained interactions

---

## Bilinen Limitasyonlar

### Tamamlanamayan Özellikler
1. [Özellik] - [Neden tamamlanamadı]
2. [Özellik] - [Neden]

### Bilinen Bug'lar

### İyileştirme Önerileri
1. [Öneri] - [Daha kaliteli asset'ler kullanabilirdim, düz bir plane üzerinde 5 kapı 1 masa ve 3 anahtar duruyor.]

---

## Ekstra Özellikler

Zorunlu gereksinimlerin dışında eklediklerim:

1. **[Özellik Adı]**

---

## Dosya Yapısı

```
Assets/
├── [InteractionSystem]/
│   ├── Scripts/
│   │   ├── Runtime/
│   │   │   ├── Core/
│   │   │   │   ├── IInteractable.cs
│   │   │   │   └── ...
│   │   │   ├── Interactables/
│   │   │   │   ├── Door.cs
│   │   │   │   └── ...
│   │   │   ├── Player/
│   │   │   │   └── ...
│   │   │   └── UI/
│   │   │       └── ...
│   │   └── Editor/
│   ├── ScriptableObjects/
│   ├── Prefabs/
│   ├── Materials/
│   └── Scenes/
│       └── TestScene.unity
├── Docs/
│   ├── CSharp_Coding_Conventions.md
│   ├── Naming_Convention_Kilavuzu.md
│   └── Prefab_Asset_Kurallari.md
├── README.md
├── PROMPTS.md
└── .gitignore
```

---

## İletişim

| Bilgi | Değer |
|-------|-------|
| Ad Soyad | [Göktuğ Bayil] |
| E-posta | [bayil.goktug@gmail.com] |
| LinkedIn | [www.linkedin.com/in/göktuğ-bayil-633515208] |
| GitHub | [github.com/gokbayil] |

---

*Bu proje Ludu Arts Unity Developer Intern Case için hazırlanmıştır.*
