using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class StartController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string lobbyScene = "GameLobby";
    [SerializeField] GameObject instructionPanel;



    void Start(){
        instructionPanel.SetActive(false);
    }
    public void PlayGame()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(lobbyScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }

    public void Instruction()
    {
        instructionPanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        instructionPanel.SetActive(false);
    }

}
