using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    CameraMovement _cameraMovement;

    [SerializeField]
    GameObject m_loader;

    [SerializeField]
    UnitManager m_UnitManager;

    void Start() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        m_loader.SetActive(false);
        m_UnitManager.ClearUnits();
        //m_UnitManager.SwitchToPhoton();
    }

    public override void OnLeftRoom()
    {
        m_UnitManager.ClearUnits();
        m_UnitManager.SwitchToLocal();
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
