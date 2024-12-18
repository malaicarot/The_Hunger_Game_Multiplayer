using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject[] PlayerPrefabs;
    Dictionary<int, bool> PlayerAliveStatus = new Dictionary<int, bool>();
    Dictionary<int, GameObject> PlayerObject = new Dictionary<int, GameObject>();

    bool isLeaveRoom = false;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }
        GetPlayerPrefabs();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerAliveStatus[player.ActorNumber] = true;
        }
        StartCoroutine(PlayerMusic());
    }
    void GetPlayerPrefabs()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 spawnpoint = new Vector3(Random.Range(-18f, 18f), 0, 0);
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefabs[playerIndex].name, spawnpoint, transform.rotation, 0);
        PlayerObject[playerIndex + 1] = player;
    }

    public void PlayerFallOut(int playerId)
    {
        photonView.RPC("RPC_PlayerFallOut", RpcTarget.All, playerId);
    }

    [PunRPC]
    void RPC_PlayerFallOut(int playerId)
    {
        PlayerAliveStatus[playerId] = false;
        int alives = 0;
        int lastPlayerID = -1;
        foreach (var isAlive in PlayerAliveStatus)
        {
            if (isAlive.Value)
            {
                alives++;
                lastPlayerID = isAlive.Key;
            }
        }
        if (alives == 1)
        {
            GetPlayerGameObject(lastPlayerID, "win");

        }
        else if (alives > 1)
        {
            GetPlayerGameObject(playerId, "lose");
        }
    }
    void GetPlayerGameObject(int id, string result)
    {
        if (PlayerObject.TryGetValue(id, out GameObject lastPlayerObject))
        {
            var playerScript = lastPlayerObject.GetComponent<Controller>();
            playerScript.InitResultPanel(result);
            if (!isLeaveRoom)
            {
                isLeaveRoom = true;
                photonView.RPC("RPC_AllLeaveRoom", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    void RPC_AllLeaveRoom()
    {
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        SoundController._instance.WinnerAudioPlay();
        yield return new WaitForSeconds(4f);
        PhotonNetwork.LeaveRoom();
        isLeaveRoom = false;
    }

    IEnumerator PlayerMusic()
    {
        yield return new WaitForSeconds(2f);
        SoundController._instance.PlayBackgroundMusic();
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
        SoundController._instance.StopBackgroundMusic();
        SoundController._instance.WinnerAudioStop();
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
