using Photon.Pun;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [SerializeField]
    TMP_InputField m_inputField;

    [SerializeField]
    GameObject m_messageCell;

    [SerializeField]
    GameObject m_content;
    private void Start()
    {
        enabled = false;
    }
    public void SendNewMessage() => GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, m_inputField.text);

    [PunRPC]
    void GetMessage(string message)
    {
        Debug.Log("Received message: " + message);
        Instantiate(m_messageCell, Vector3.zero, Quaternion.identity, m_content.transform)
        .GetComponent<TMP_Text>().text = message;
        m_inputField.text = "";

    }
}
