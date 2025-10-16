using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_actionMenuHolder;

    [SerializeField]
    UnitManager m_unitManager;

    private void Awake() => m_actionMenuHolder.SetActive(false);



}
