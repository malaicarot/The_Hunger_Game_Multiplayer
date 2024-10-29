using Photon.Pun;
using UnityEngine;

public class GameRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefabs;
    public Transform[] spawnPosition;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }

            // int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            Vector3 spawnPpoint = new Vector3(Random.Range(-18f, 18f), 0, 0);
            // PhotonNetwork.Instantiate(PlayerPrefabs[playerIndex].name, spawnPosition[Random.Range(-18, spawnPosition.Length - 1)].position, spawnPosition[Random.Range(0, spawnPosition.Length - 1)].rotation, 0);
            PhotonNetwork.Instantiate (PlayerPrefabs.name, spawnPpoint, spawnPosition[Random.Range(0, spawnPosition.Length - 1)].rotation, 0);
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        //Leave this Room
        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        //Show the Room name
        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);

        //Show the list of the players connected to this Room
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
            string isMasterClient = PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "";
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        //We have left the Room, return back to the GameLobby
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined the room");
    }

    // Phương thức callback được gọi khi có người chơi rời khỏi phòng
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room");
    }
}
