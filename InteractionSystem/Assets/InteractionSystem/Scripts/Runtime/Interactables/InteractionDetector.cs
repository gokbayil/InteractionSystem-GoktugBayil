using UnityEngine;
using UnityEngine.UI;

public class InteractionDetector : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private Image crosshair;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color interactColor = Color.red;

    private Camera _camera;
    private IInteractable _currentInteractable;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        if(crosshair != null)
        {
            crosshair.color = defaultColor;
        }
    }

    private void Update()
    {
        CheckForInteractable();
        HandleInput();
    }

    private void CheckForInteractable()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null)
            {
                if(_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    HighlightCrosshair(true);
                }
                return;
            }
        }

        ClearInteraction();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(interactionKey) && _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    private void ClearInteraction()
    {
        if(_currentInteractable != null)
        {
            _currentInteractable = null;
            HighlightCrosshair(false);
        }
    }

    private void HighlightCrosshair(bool on)
    {
        if (crosshair != null)
        {
            crosshair.color = on ? interactColor : defaultColor;
        }
    }
}
