using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoRoom : MonoBehaviour
{
    [SerializeField]
    string scene;
    public void LoadDemoRoom() => SceneManager.LoadScene(scene);
}