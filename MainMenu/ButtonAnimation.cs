using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Material _materialInstance;
    [SerializeField] float _speed = 3f;
    private float _targetProgress = 0f;
    private float _currentProgress = 0f;
    private bool _isHovered = false;

    void Start()
    {
        Image image = GetComponent<Image>();
        _materialInstance = new Material(image.material);
        image.material = _materialInstance;
        _materialInstance.SetFloat("_HoverProgress", 0f);
    }

    void Update()
    {
        _currentProgress = Mathf.MoveTowards(_currentProgress, _targetProgress, _speed * Time.deltaTime);
        _materialInstance.SetFloat("_HoverProgress", _currentProgress);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        _targetProgress = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        _targetProgress = 0f;
    }

    public void OnPointerDown(PointerEventData eventData) => _targetProgress = 0.8f;
    public void OnPointerUp(PointerEventData eventData) => _targetProgress = _isHovered ? 1f : 0f;
    public void ResetAnimation()
    {
        _isHovered = false;
        _targetProgress = 0f;
        _currentProgress = 0f;
        if (_materialInstance != null)
        {
            _materialInstance.SetFloat("_HoverProgress", 0f);
        }
    }
    void OnEnable() => ResetAnimation();
    void OnDestroy()
    {
        if (_materialInstance != null)
            DestroyImmediate(_materialInstance);
    }
}