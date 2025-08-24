using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public InputAction m_cameraControl;
    public InputAction m_cameraScroll;
    public InputAction m_selectControl;
    float m_directionY = 0;
    float m_directionX = 0;
    float m_scroll = 0;
    Transform m_Camera;
    public LayerMask interactableLayer;
    public event Action<SpaceMark, bool> OnTarget;

    [SerializeField]
    const float m_speed = 5;

    private void OnEnable()
    {
        m_cameraControl.Enable();
        m_cameraScroll.Enable();
        m_selectControl.Enable();
    }

    private void OnDisable()
    {
        m_cameraControl.Disable();
        m_cameraScroll.Disable();
        m_selectControl.Disable();
    }

    private void Start()
    {
        m_Camera = GetComponent<Transform>();
    }

    void Update()
    {
        MoveCamera();
        RayCastCursor();
    }
    void MoveCamera()
    {
        m_directionY = m_cameraControl.ReadValue<Vector2>().y;
        m_directionX = m_cameraControl.ReadValue<Vector2>().x;
        m_scroll = m_cameraScroll.ReadValue<float>();

        Vector3 movement = new Vector3(m_directionX, m_scroll, m_directionY) * m_speed * Time.deltaTime;

        m_Camera.Translate(movement, Space.World);
    }

    void RayCastCursor()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            Vector3 mouseWorldPos = hit.point;
            hit.collider.GetComponent<IMouseHover>().OnMouseHover(mouseWorldPos);

            if (m_selectControl.triggered)
            {
                bool canInstantiate;
                SpaceMark mark = hit.collider.GetComponent<IMouseSelect>().OnMouseSelect(mouseWorldPos, out canInstantiate);
                OnTarget?.Invoke(mark, canInstantiate);
            }
        }
    }
}
