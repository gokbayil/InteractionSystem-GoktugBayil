using UnityEngine;
using UnityEngine.UI;
using InteractionSystem.Runtime.Core; // IInteractable için

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float m_InteractionRange = 5f;
        [SerializeField] private LayerMask m_InteractableLayer;
        [SerializeField] private KeyCode m_InteractionKey = KeyCode.E;

        [Header("UI")]
        [SerializeField] private Image m_Crosshair;
        [SerializeField] private Image m_ProgressBar;
        [SerializeField] private Text m_InteractionText;
        [SerializeField] private Color m_DefaultColor = Color.white;
        [SerializeField] private Color m_InteractColor = Color.red;

        private Camera m_Camera;
        private IInteractable m_CurrentInteractable;
        private float m_CurrentHoldTime = 0f;

        private void Start()
        {
            m_Camera = GetComponent<Camera>();

            if (m_Crosshair != null) m_Crosshair.color = m_DefaultColor;
            if (m_InteractionText != null) m_InteractionText.text = string.Empty;
            if (m_ProgressBar != null) m_ProgressBar.fillAmount = 0f;
        }

        private void Update()
        {
            CheckForInteractable();
            HandleInput();
        }

        private void CheckForInteractable()
        {
            Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, m_InteractionRange, m_InteractableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        m_CurrentInteractable = interactable;
                        HighlightCrosshair(true);
                        UpdateInteractionText();
                    }
                    return;
                }
            }

            ClearInteraction();
        }

        private void HandleInput()
        {
            if (m_CurrentInteractable == null) return;

            // Hold Interaction (Basýlý Tutma)
            if (m_CurrentInteractable.HoldDuration > 0f)
            {
                if (Input.GetKey(m_InteractionKey))
                {
                    m_CurrentHoldTime += Time.deltaTime;
                    float duration = m_CurrentInteractable.HoldDuration;

                    if (m_ProgressBar != null)
                    {
                        m_ProgressBar.fillAmount = m_CurrentHoldTime / duration;
                    }

                    if (m_CurrentHoldTime >= duration)
                    {
                        m_CurrentInteractable.Interact();
                        ResetHold();
                        UpdateInteractionText(); // Durum deðiþirse metni güncelle (örn: kapý açýldý)
                    }
                }
                else
                {
                    ResetHold();
                }
            }
            // Instant Interaction (Anlýk Basma)
            else
            {
                if (Input.GetKeyDown(m_InteractionKey))
                {
                    m_CurrentInteractable.Interact();
                    UpdateInteractionText();
                }
            }
        }

        private void UpdateInteractionText()
        {
            if (m_InteractionText != null && m_CurrentInteractable != null)
            {
                m_InteractionText.text = m_CurrentInteractable.InteractionPrompt;
                m_InteractionText.gameObject.SetActive(true);
            }
        }

        private void ResetHold()
        {
            m_CurrentHoldTime = 0f;
            if (m_ProgressBar != null)
                m_ProgressBar.fillAmount = 0f;
        }

        private void ClearInteraction()
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable = null;
                HighlightCrosshair(false);
                ResetHold();

                if (m_InteractionText != null)
                {
                    m_InteractionText.text = string.Empty;
                    m_InteractionText.gameObject.SetActive(false);
                }
            }
        }

        private void HighlightCrosshair(bool on)
        {
            if (m_Crosshair != null)
            {
                m_Crosshair.color = on ? m_InteractColor : m_DefaultColor;
            }
        }
    }
}