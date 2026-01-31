using UnityEngine;
using System.Collections;
using InteractionSystem.Runtime.Core; // IInteractable için

namespace InteractionSystem.Runtime.Interactables
{
    public class ButtonInteractable : MonoBehaviour, IInteractable
    {
        [Header("Target")]
        [SerializeField] private DoorInteractable m_TargetDoor;

        [Header("Button Animation")]
        [SerializeField] private Transform m_ButtonMesh;
        [SerializeField] private float m_PressDepth = 0.05f;
        [SerializeField] private float m_PressSpeed = 5f;

        private bool m_IsPressed = false;

        public string InteractionPrompt => "Press button to open the door.";
        public float HoldDuration => 0f;

        public void Interact()
        {
            if (m_IsPressed) return;
            StartCoroutine(PressButtonRoutine());
        }

        private IEnumerator PressButtonRoutine()
        {
            m_IsPressed = true;

            if (m_TargetDoor != null)
            {
                m_TargetDoor.ToggleDoorRemotely();
            }

            if (m_ButtonMesh != null)
            {
                Vector3 initialPos = m_ButtonMesh.localPosition;
                Vector3 targetPos = initialPos + (Vector3.down * m_PressDepth);

                // Aþaðý in
                float t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime * m_PressSpeed;
                    m_ButtonMesh.localPosition = Vector3.Lerp(initialPos, targetPos, t);
                    yield return null;
                }

                yield return new WaitForSeconds(0.2f);

                // Yukarý çýk
                t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime * m_PressSpeed;
                    m_ButtonMesh.localPosition = Vector3.Lerp(targetPos, initialPos, t);
                    yield return null;
                }
            }

            m_IsPressed = false;
        }
    }
}