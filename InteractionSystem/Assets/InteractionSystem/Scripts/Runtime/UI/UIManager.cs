using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using InteractionSystem.Runtime.Core; // Key sýnýfý için

namespace InteractionSystem.Runtime.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Transform m_KeyPanel;
        [SerializeField] private GameObject m_KeyImagePrefab;

        private Dictionary<Key, GameObject> m_KeyImages = new Dictionary<Key, GameObject>();
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void AddKeyToUI(Key key)
        {
            if (!m_KeyImages.ContainsKey(key))
            {
                GameObject keyImage = Instantiate(m_KeyImagePrefab, m_KeyPanel);
                if (keyImage.GetComponent<Image>() != null)
                {
                    keyImage.GetComponent<Image>().sprite = key.keySprite;
                }
                m_KeyImages[key] = keyImage;
            }
        }
    }
}