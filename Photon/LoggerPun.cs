using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class LoggerPun : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_InputField m_input_Create;
    [SerializeField]
    TMP_InputField m_input_Join;
    [SerializeField]
    GameObject m_loader;
    [SerializeField]
    GameObject m_chat;
    [SerializeField]
    GameObject m_logger;

    private void Start() => m_chat.SetActive(false);
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(m_input_Create.text))
        {
            Debug.LogWarning("Room name cannot be empty!");
            return;
        }

        Debug.Log("Creating room: " + m_input_Create.text);

        PhotonNetwork.CreateRoom(
            m_input_Create.text,
            new RoomOptions() { MaxPlayers = 2, IsVisible = true, IsOpen = true },
            TypedLobby.Default, null);
        m_logger.SetActive(false);
        m_loader.SetActive(true);
        m_chat.SetActive(true);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(m_input_Join.text))
        {
            Debug.LogWarning("Room name cannot be empty!");
            return;
        }

        Debug.Log("Joining room: " + m_input_Join.text);

        PhotonNetwork.JoinRoom(m_input_Join.text);
        m_logger.SetActive(false);
        m_chat.SetActive(true);
        m_chat.GetComponent<Chat>().enabled = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room. Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
        if (!PhotonNetwork.IsMasterClient)
            m_loader.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => m_loader.SetActive(false);

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
        m_logger.SetActive(true);
        m_loader.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        m_logger.SetActive(true);
        m_loader.SetActive(false);
    }
}
