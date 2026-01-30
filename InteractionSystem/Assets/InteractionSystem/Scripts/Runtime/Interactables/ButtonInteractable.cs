using System.Collections;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [Header("Target")]
    [SerializeField] private DoorInteractable targetDoor;

    [Header("Button Animation")]
    [SerializeField] private Transform buttonMesh;
    [SerializeField] private float pressDepth = 0.05f;
    [SerializeField] private float pressSpeed = 5f;

    private bool isPressed = false;

    public string InteractionPrompt => "Press button to open the door.";

    public void Interact()
    {
        if (isPressed) return;

        StartCoroutine(PressButtonRoutine());
    }

    private IEnumerator PressButtonRoutine()
    {
        isPressed = true;

        if(targetDoor != null)
        {
            targetDoor.ToggleDoorRemotely();
        }

        Vector3 initialPos = buttonMesh.localPosition;
        Vector3 targetPos = initialPos + (Vector3.down * pressDepth);

        float t = 0;
        while (t<1)
        {
            t += Time.deltaTime * pressSpeed;
            buttonMesh.localPosition = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        t = 0;

        while (t<1)
        {
            t += Time.deltaTime * pressSpeed;
            buttonMesh.localPosition = Vector3.Lerp(targetPos, initialPos, t);
            yield return null;
        }

        isPressed = false;

    }
}
