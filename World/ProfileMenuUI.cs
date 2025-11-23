using UnityEngine;
using UnityEngine.InputSystem;

public class ProfileMenuUI : MonoBehaviour
{
    [SerializeField] private InputAction tabAction;
    [SerializeField] private GameObject holderUI;
    [SerializeField] private FirstPersonController controller;

    void OnDisable() => tabAction?.Disable();

    private void Awake()
    {
        tabAction.Enable();
        holderUI?.SetActive(false);
        tabAction.performed += ShowUI;
    }
    void ShowUI(InputAction.CallbackContext context)
    {
        if(holderUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            holderUI?.SetActive(false);
            controller.Toggle();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            holderUI?.SetActive(true);
            controller.Toggle();
        }
    }
}
