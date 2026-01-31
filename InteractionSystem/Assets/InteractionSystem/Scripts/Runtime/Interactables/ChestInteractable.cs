using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class ChestInteractable : MonoBehaviour, IInteractable
    {
        [Header("Chest Settings")]
        [SerializeField] private float m_TimeToOpen = 2.0f;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private string m_OpenAnimParam = "IsOpen";

        [Header("Audio")]
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private AudioClip m_OpenSound;

        private bool m_IsOpen = false;

        public string InteractionPrompt => m_IsOpen ? "Empty" : "Hold to open.";
        public float HoldDuration => m_IsOpen ? 0f : m_TimeToOpen;

        public void Interact()
        {
            if (m_IsOpen) return;
            OpenChest();
        }

        private void OpenChest()
        {
            m_IsOpen = true;
            Debug.Log("Chest opened.");

            if (m_Animator != null)
            {
                m_Animator.SetBool(m_OpenAnimParam, true);
            }

            if (m_AudioSource != null && m_OpenSound != null)
            {
                m_AudioSource.PlayOneShot(m_OpenSound);
            }

            // Tekrar etkileþimi engellemek için collider'ý kapat
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }
}