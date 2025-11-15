using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoRoom : MonoBehaviour
{
    [SerializeField]
    SceneAsset scene;
    public void LoadDemoRoom() => SceneManager.LoadScene(scene.name);
}