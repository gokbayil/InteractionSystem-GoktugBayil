using UnityEngine;
using System.Collections.Generic;
using InteractionSystem.Runtime.Core; // Key için
using InteractionSystem.Runtime.UI;   // UIManager için

namespace InteractionSystem.Runtime.Player
{
    public class KeyInventory : MonoBehaviour
    {
        [SerializeField] private List<int> m_KeyIds = new List<int>();

        public static KeyInventory Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddKey(Key key)
        {
            if (key == null) return;

            if (!m_KeyIds.Contains(key.keyId))
            {
                m_KeyIds.Add(key.keyId);
                Debug.Log($"Key added: {key.keyName} (ID: {key.keyId})");

                // UIManager kontrolü
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.AddKeyToUI(key);
                }
            }
        }

        public bool HasKey(Key key)
        {
            if (key == null) return false;
            return m_KeyIds.Contains(key.keyId);
        }
    }
}