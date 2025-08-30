using UnityEngine;
using UnityEngine.UI;

public class SwitcherButton : MonoBehaviour
{
    Button button;
    [SerializeField]
    GameObject _gameObject;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if(_gameObject.activeSelf)
            _gameObject.SetActive(false);
        else
            _gameObject.SetActive(true);
    }
}
