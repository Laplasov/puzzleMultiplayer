using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField] Sprite[] _image;

    public void SetImage(IdentityType identity)
    {
        var displayImage = GetComponent<Image>();
        displayImage.sprite = _image[(int)identity];
    }

}
