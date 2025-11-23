using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWorld : MonoBehaviour, IInteractable
{
    [SerializeField] SceneDataBridgeSO sceneDataBridge;
    [SerializeField] string scene;
    [SerializeField] PreparedEnemiesSO[] preparedEnemiesSO;

    private Scene _currentScene;
    private GameObject[] _sceneRootObjects;
    public async void LoadBattle()
    {
        Cursor.lockState = CursorLockMode.None;

        _currentScene = SceneManager.GetActiveScene();
        _sceneRootObjects = _currentScene.GetRootGameObjects();
        foreach (GameObject obj in _sceneRootObjects)
                obj.SetActive(false);

        sceneDataBridge.PreparedEnemiesSO = preparedEnemiesSO;

        //await AwaitLoad();
        await LoadingManager.Instance.LoadWithScreen(scene, LoadSceneMode.Additive);

        sceneDataBridge.OnReturnToWorld += ReturnToWorld;

        Scene battleSceneInstance = SceneManager.GetSceneByName(scene);
        SceneManager.SetActiveScene(battleSceneInstance);

        //SceneManager.LoadScene(scene.name);
    }
    public async void ReturnToWorld()
    {
        sceneDataBridge.OnReturnToWorld -= ReturnToWorld;
        Scene battleScene = SceneManager.GetSceneByName(scene);
        //SceneManager.UnloadSceneAsync(battleScene);
        await AwaitUnload();

        foreach (GameObject obj in _sceneRootObjects)
                obj.SetActive(true);

        SceneManager.SetActiveScene(_currentScene);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnDestroy() => sceneDataBridge.OnReturnToWorld = null;
    //private async Awaitable AwaitLoad() => await SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
    private async Awaitable AwaitUnload() => await SceneManager.UnloadSceneAsync(scene);
    public void OnInteract(FirstPersonController player) => LoadBattle();


}