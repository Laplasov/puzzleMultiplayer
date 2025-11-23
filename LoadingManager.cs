using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Canvas m_loadingCanvas;
    [SerializeField] Image m_load;
    [SerializeField] Camera m_camera;

    public static LoadingManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            m_loadingCanvas.gameObject.SetActive(false);
            m_camera.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Awaitable LoadWithScreen(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        m_camera.enabled = true;
        m_loadingCanvas.gameObject.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            await Awaitable.NextFrameAsync();

            m_load.fillAmount = asyncLoad.progress / 0.9f;

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
        await Awaitable.NextFrameAsync();

        m_loadingCanvas.gameObject.SetActive(false);
        m_load.fillAmount = 0;
        m_camera.enabled = false;
    }
}