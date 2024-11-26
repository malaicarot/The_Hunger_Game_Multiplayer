using System.Collections;
using Photon.Pun;
using UnityEngine;

public class GameRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }
        GetPlayerPrefabs();
    }
    public void GetPlayerPrefabs()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 spawnpoint = new Vector3(Random.Range(-18f, 18f), 0, 0);
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefabs[playerIndex].name, spawnpoint, transform.rotation, 0);
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            string isMasterClient = PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "";
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined the room");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room");
    }
}
