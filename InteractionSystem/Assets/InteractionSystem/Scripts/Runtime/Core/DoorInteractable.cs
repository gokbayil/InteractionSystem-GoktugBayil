using UnityEngine;

public class DoorInteractable : MonoBehaviour
{

    [Header("Door Settings")]
    [SerializeField] private Transform doorPivot; //pivotpoint of door
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float closeAngle = 0f;
    [SerializeField] private float rotationSpeed = 2f;

    //Lock settings

    //Audio

    private bool isDoorOpen = false;
    private bool isAnimating = false;
    private Quaternion targetRotation;

    private void Start()
    {
        targetRotation = Quaternion.Euler(0f, closeAngle, 0f);
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
    public void ToggleDoor()
    {
        if(!isAnimating)
        {
            isDoorOpen = !isDoorOpen;

            targetRotation = Quaternion.Euler(0f, isDoorOpen ? openAngle : closeAngle, 0f);

            isAnimating = true;
        }
    }
}
