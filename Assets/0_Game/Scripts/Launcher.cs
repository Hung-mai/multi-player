using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher ins;

    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public GameObject menuButtons;

    public GameObject createRoomScreen;
    public TMP_InputField roomNameInput;

    public GameObject roomScreen;
    public TMP_Text roomNameTxt;
    public TMP_Text playerNameLabel;
    private List<TMP_Text> allPlayerName = new List<TMP_Text>();

    public GameObject errorScreen;
    public TMP_Text errorTxt;

    public GameObject roomBrowserScreen;
    public RoomButton theRoomButton;
    private List<RoomButton> allRoomButtons = new List<RoomButton>();

    public GameObject nameInputScreen;
    public TMP_InputField nameInput;

    public string levelToPlay;
    public GameObject startButton;

    public GameObject roomTestBtn;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        CloseMenu();

        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to network...";

        PhotonNetwork.ConnectUsingSettings();

#if UNITY_EDITOR
        roomTestBtn.SetActive(true);
#endif
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby();
        loadingText.text = "Joining Lobby...";

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        CloseMenu();
        menuButtons.SetActive(true);

        // PhotonNetwork.NickName = Random.Range(0, 100000).ToString();

        if(PlayerPrefs.HasKey("playerName") == false)
        {
            CloseMenu();
            nameInputScreen.SetActive(true);
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
        }
    }

    private void CloseMenu()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
        nameInputScreen.SetActive(false);
    }

    public void btn_OpenRoomCreate()
    {
        CloseMenu();
        createRoomScreen.SetActive(true);
    }

    public void btn_CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInput.text) == false)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;
            // options.

            PhotonNetwork.CreateRoom(roomNameInput.text, options);

            CloseMenu();
            loadingText.text = "Creating room...";
            loadingScreen.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        CloseMenu();

        roomScreen.SetActive(true);
        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;

        ListAllPlayer();

        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    private void ListAllPlayer()
    {
        foreach (TMP_Text player in allPlayerName)
        {
            Destroy(player.gameObject);
        }
        allPlayerName.Clear();

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLabel.text = players[i].NickName;

            allPlayerName.Add(newPlayerLabel);
        }

        playerNameLabel.gameObject.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text newPlayerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        newPlayerLabel.text = newPlayer.NickName;
        allPlayerName.Add(newPlayerLabel);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListAllPlayer();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorTxt.text = "Fail to Create room: " + message;
        CloseMenu();
        errorScreen.SetActive(true);
    }

    public void btn_leaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        CloseMenu();
        loadingText.text = "Leaving Room";

        loadingScreen.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public void btn_closeErrorPanel()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public void btn_OpenRoomBrowser()
    {
        CloseMenu();
        roomBrowserScreen.SetActive(true);
    }
    public void btn_CloseRoomBrowser()
    {
        CloseMenu();
        menuButtons.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomButton item in allRoomButtons)
        {
            item.gameObject.SetActive(false);
        }

        allRoomButtons.Clear();


        for (int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(theRoomButton,  theRoomButton.transform.parent);
                newButton.SetButtonDetails(roomList[i]);

                allRoomButtons.Add(newButton);
            }
        }
        theRoomButton.gameObject.SetActive(false);
    }

    public void JoinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);

        CloseMenu();
        loadingText.text = "Joining room...";
        loadingScreen.SetActive(true);
    }

    public void btn_QuitGame()
    {
        Application.Quit();
    }

    public void btn_setNickName()
    {
        if(string.IsNullOrEmpty(nameInput.text) == false)
        {
            PhotonNetwork.NickName = nameInput.text;

            CloseMenu();
            menuButtons.SetActive(true);

            PlayerPrefs.SetString("playerName", nameInput.text);
        }
    }

    public void btn_StartGame()
    {
        PhotonNetwork.LoadLevel(levelToPlay);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void btn_QuickJoin()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        PhotonNetwork.CreateRoom("test", options);
        CloseMenu();
        loadingText.text = "Creating room...";
        loadingScreen.SetActive(true);
    }
}
