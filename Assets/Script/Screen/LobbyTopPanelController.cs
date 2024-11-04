
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopPanelController : MonoBehaviour
{
    private readonly string connectionStatusMessage = "   Connecting Status: ";

    [Header("UI References")]
    [SerializeField] Text ConnectionStatusText;

    #region Unity
    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;

    }
    #endregion
}
