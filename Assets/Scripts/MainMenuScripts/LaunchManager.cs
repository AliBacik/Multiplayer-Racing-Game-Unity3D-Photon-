using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using JetBrains.Annotations;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    byte maxPlayersPerRoom = 4;
    bool isConnecting;

    public TMP_InputField playerName;
    public TextMeshProUGUI feedBackText;
    string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void ConnectNetwork()
    {
        feedBackText.text = "";
        isConnecting = true;

        PhotonNetwork.NickName = playerName.text;
        if (PhotonNetwork.IsConnected)
        {
            feedBackText.text += "\nJoining Room..";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            feedBackText.text += "\nConnecting..";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName",name);
        PlayerPrefs.Save();
    }
    
    public void ConnectSinglePlayerMode()
    {
        SceneManager.LoadScene("GameScene");
    }

    //NETWORK
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            feedBackText.text += "\nOnConnectedToMaster..";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedBackText.text += "\nFailed to join..";
        PhotonNetwork.CreateRoom(null,new RoomOptions { MaxPlayers=maxPlayersPerRoom});
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedBackText.text += "\nDisconnected because " + cause;
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        feedBackText.text += "\nJoined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players.";
        PhotonNetwork.LoadLevel("GameScene");
    }
}
