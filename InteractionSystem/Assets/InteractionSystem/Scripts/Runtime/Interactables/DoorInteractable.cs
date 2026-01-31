using UnityEngine;
using InteractionSystem.Runtime.Core;   // Key ve IInteractable için
using InteractionSystem.Runtime.Player; // KeyInventory için (HATA BURADAYDI, DÜZELDÝ)

namespace InteractionSystem.Runtime.Interactables
{
    [RequireComponent(typeof(AudioSource))]
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        [Header("Door Settings")]
        [SerializeField] private Transform m_DoorPivot;
        [SerializeField] private float m_OpenAngle = 90f;
        [SerializeField] private float m_CloseAngle = 0f;
        [SerializeField] private float m_RotationSpeed = 2f;

        [Header("Interaction Type")]
        [SerializeField] private bool m_UseHoldInteraction = false;
        [SerializeField] private float m_HoldTime = 1.5f;

        [Header("Lock Settings")]
        [SerializeField] private bool m_IsLocked = false;
        [SerializeField] private Key m_RequiredKey;

        [Header("Audio Settings")]
        [SerializeField] private AudioClip m_OpenSound;
        [SerializeField] private AudioClip m_CloseSound;
        [SerializeField] private AudioClip m_LockedSound;

        private AudioSource m_AudioSource;
        private bool m_IsDoorOpen = false;
        private bool m_IsAnimating = false;
        private Quaternion m_TargetRotation;

        public string InteractionPrompt
        {
            get
            {
                if (m_IsLocked)
                    return m_RequiredKey != null
                        ? $"Locked. Requires {m_RequiredKey.keyName}"
                        : "Locked.";

                if (m_IsDoorOpen) return "Press E to close";

                return m_UseHoldInteraction ? "Hold E to open" : "Press E to Open";
            }
        }

        public float HoldDuration => m_UseHoldInteraction ? m_HoldTime : 0f;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            // Baþlangýç rotasyonu
            m_TargetRotation = transform.localRotation;
            if (m_DoorPivot != null)
            {
                m_TargetRotation = m_DoorPivot.localRotation;
            }
        }

        private void Update()
        {
            if (m_IsAnimating && m_DoorPivot != null)
            {
                m_DoorPivot.localRotation = Quaternion.Slerp(
                    m_DoorPivot.localRotation,
                    m_TargetRotation,
                    Time.deltaTime * m_RotationSpeed
                );

                if (Quaternion.Angle(m_DoorPivot.localRotation, m_TargetRotation) < 0.1f)
                {
                    m_DoorPivot.localRotation = m_TargetRotation;
                    m_IsAnimating = false;
                }
            }
        }

        public void Interact()
        {
            if (m_IsAnimating) return;

            if (m_IsLocked)
            {
                // KeyInventory referansý artýk hata vermez
                if (KeyInventory.Instance != null && KeyInventory.Instance.HasKey(m_RequiredKey))
                {
                    Debug.Log($"Door unlocked with {m_RequiredKey.keyName}");
                    m_IsLocked = false;
                    PlaySound(m_OpenSound);
                }
                else
                {
                    PlaySound(m_LockedSound);
                    Debug.Log("Door is locked.");
                    return;
                }
            }

            ToggleAction();
        }

        public void ToggleDoorRemotely()
        {
            if (m_IsLocked)
            {
                m_IsLocked = false;
            }
            ToggleAction();
        }

        private void ToggleAction()
        {
            if (!m_IsAnimating)
            {
                m_IsDoorOpen = !m_IsDoorOpen;
                float targetAngle = m_IsDoorOpen ? m_OpenAngle : m_CloseAngle;
                m_TargetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                m_IsAnimating = true;
                PlaySound(m_IsDoorOpen ? m_OpenSound : m_CloseSound);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (m_AudioSource != null && clip != null)
            {
                m_AudioSource.PlayOneShot(clip);
            }
        }
    }
}