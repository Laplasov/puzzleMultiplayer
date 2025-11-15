using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 30f;
    [SerializeField] private InputAction interactAction;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject imageUI;
    private IInteractable _currentInteractable;

    void OnEnable()
    {
        interactAction?.Enable();
        imageUI.SetActive(false);
    }

    void OnDisable()
    {
        interactAction?.Disable();
    }

    private void Awake()
    {
        interactAction.Enable();
        interactAction.performed += OnInteract;
        imageUI.SetActive(false);
    }

    private void Update() => FindInteractable();

    void FindInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        bool foundInteractable = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (_currentInteractable != interactable)
                {
                    if (_currentInteractable != null)
                        _currentInteractable = null;

                    _currentInteractable = interactable;
                }
                foundInteractable = true;
            }
        }

        if (foundInteractable && !imageUI.activeSelf)
        {
            imageUI.SetActive(true);
        }
        else if (!foundInteractable && imageUI.activeSelf)
        {
            imageUI.SetActive(false);
            _currentInteractable = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.OnInteract(GetComponent<FirstPersonController>());
        }
    }
}