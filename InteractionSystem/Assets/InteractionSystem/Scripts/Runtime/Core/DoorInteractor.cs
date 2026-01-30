using UnityEngine;
using UnityEngine.UI;

public class DoorInteractor : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayDistance = 5;

    [Header("Raycast Features")]
    [SerializeField] private KeyCode interactionKey;
    [SerializeField] private Image crosshair;
    
    private Camera _camera;
    private DoorItem doorItem;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        PerformRaycast();
        InteractionInput();
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayDistance)) //from camera to center of the screen, send the raycast forward, go distance of rayDistance
        {
            var _doorItem = hit.collider.GetComponent<DoorItem>();
            if (_doorItem != null)
            {
                doorItem = _doorItem;
                HighlightCrosshair(true);
            }
            else
            {
                ClearItem();
            }

        }
        else
        {
            ClearItem();
        }
    }

    void InteractionInput()
    {
        if(doorItem != null)
        {
            if(Input.GetKeyDown(interactionKey))
            {
                doorItem.ObjectInteraction();
            }
        }
    }

    void ClearItem()
    {
        if(doorItem != null)
        {
            doorItem = null;
            HighlightCrosshair(false);
        }
    }

    void HighlightCrosshair(bool on)
    {
        crosshair.color = on ? Color.red : Color.white;
    }
}
