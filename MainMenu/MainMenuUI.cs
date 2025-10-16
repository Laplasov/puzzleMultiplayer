using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_mainMenu;

    [SerializeField]
    GameObject m_levelSelect;

    public void GoMainMenu()
    {
        m_mainMenu.SetActive(true);
        m_levelSelect.SetActive(false);
    }
    public void GoLevelSelect()
    {
        m_levelSelect.SetActive(true);
        m_mainMenu.SetActive(false);
    }

    public void Exit() => Application.Quit();
}
