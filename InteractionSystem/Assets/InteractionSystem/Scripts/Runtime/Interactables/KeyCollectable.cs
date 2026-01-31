using UnityEngine;
using InteractionSystem.Runtime.Core;   // Key ve IInteractable için
using InteractionSystem.Runtime.Player; // KeyInventory için

namespace InteractionSystem.Runtime.Interactables
{
    public class KeyCollectable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Key m_Key;
        [SerializeField] private AudioClip m_PickupSound;
        [SerializeField] private AudioSource m_AudioSource;

        public string InteractionPrompt => "Press E to pick up";
        public float HoldDuration => 0f;

        public void Interact()
        {
            KeyPickup();
        }

        public void KeyPickup()
        {
            if (m_Key != null)
            {
                // KeyInventory'ye eriþim artýk sorunsuz
                if (KeyInventory.Instance != null)
                {
                    KeyInventory.Instance.AddKey(m_Key);
                }
                else
                {
                    Debug.LogWarning("KeyInventory Instance bulunamadý!");
                }

                if (m_PickupSound != null && m_AudioSource != null)
                {
                    m_AudioSource.PlayOneShot(m_PickupSound);
                }

                gameObject.SetActive(false);
            }
        }
    }
}