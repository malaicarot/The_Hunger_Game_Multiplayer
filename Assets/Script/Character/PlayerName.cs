using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerName : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI playerName;
    PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (playerName != null && photonView != null)
        {
            playerName.text = photonView.Owner.NickName;
        }
    }
}
