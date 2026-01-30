using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{

    [Header("Door Settings")]
    [SerializeField] private Transform doorPivot; //pivotpoint of door
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float closeAngle = 0f;
    [SerializeField] private float rotationSpeed = 2f;

    [Header("Lock Settings")]
    [SerializeField] private bool isLocked = false; //is the door locked
    [SerializeField] private Key requiredKey; //which key is required to unlock the door

    [Header("Audio Settings")]
    [SerializeField] private AudioClip openSound; // door opening sound
    [SerializeField] private AudioClip closeSound; // door closing sound
    [SerializeField] private AudioClip lockedSound;

    private AudioSource audioSource;

    private bool isDoorOpen = false;
    private bool isAnimating = false;
    private Quaternion targetRotation;

    public string InteractionPrompt => isLocked ? "Locked." : (isDoorOpen ? "Close the Door" : "Open the Door");

    private void Start()
    {
        targetRotation = Quaternion.Euler(0f, closeAngle, 0f);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAnimating)
        {
            doorPivot.localRotation = Quaternion.Lerp(doorPivot.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

            if(Quaternion.Angle(doorPivot.localRotation, targetRotation) < 0.1f)
            {
                doorPivot.localRotation = targetRotation;
                isAnimating = false;
            }
        }
    }

    public void Interact()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        if(isLocked)
        {
            if(KeyInventory.Instance.HasKey(requiredKey))
            {
                Debug.Log($"Door '{gameObject.name}' unlocked with {requiredKey.keyName}");
                isLocked = false;
            }
            else
            {
                PlaySound(lockedSound);
                Debug.Log($"Door '{gameObject.name}' is locked. You need the {requiredKey.keyName} to open it.");
                return;
            }
        }

        if(!isAnimating)
        {
            isDoorOpen = !isDoorOpen;
            targetRotation = Quaternion.Euler(0f, isDoorOpen ? openAngle : closeAngle, 0f);
            isAnimating = true;
            PlaySound(isDoorOpen ? openSound : closeSound);
        }
    }

    public void ToggleDoorRemotely()
    {
        if (isLocked)
        {
            isLocked = false;
            Debug.Log("Door unlocked remotely via button.");
        }

        if (!isAnimating)
        {
            isDoorOpen = !isDoorOpen;
            targetRotation = Quaternion.Euler(0f, isDoorOpen ? openAngle : closeAngle, 0f);
            isAnimating = true;
            PlaySound(isDoorOpen ? openSound : closeSound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if(audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
