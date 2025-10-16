using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField]
    SceneDataBridgeSO sceneDataBridge;
    [SerializeField]
    SceneAsset scene;
    [SerializeField]
    PreparedEnemiesSO[] preparedEnemiesSO;
    public void OnButton()
    {
        sceneDataBridge.PreparedEnemiesSO = preparedEnemiesSO;
        SceneManager.LoadScene(scene.name);
    }
}
