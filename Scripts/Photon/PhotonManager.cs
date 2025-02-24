using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private static PhotonManager instance;
    public static PhotonManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private const string version = "1.0";
    [SerializeField] private string nickname = " ";

    [Header("Button")]
    [SerializeField] private Button randomRoomBtn;
    [SerializeField] private Button makeRoomBtn;
    [SerializeField] private Button exitBtn;

    [Header("Room List")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform contentTr;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        randomRoomBtn.interactable = false;
        makeRoomBtn.interactable = false;
        exitBtn.interactable = false;


        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = nickname;

        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }//Awake

    private void Start()
    {
        randomRoomBtn.onClick.AddListener(() => PhotonNetwork.JoinRandomRoom());
        makeRoomBtn.onClick.AddListener(() => OnMakeRoomButtonClick());
        exitBtn.onClick.AddListener(() => Application.Quit());
    }

    private void OnMakeRoomButtonClick()
    {
        MakeRoom();
    }

    void MakeRoom()
    {
        var roomOption = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true,
        };
        PhotonNetwork.CreateRoom(nickname + "'s Room", roomOption);
    }

    #region �����ݹ��Լ�
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            if (room.RemovedFromList == true)
            {
                if (roomDict.TryGetValue(room.Name, out GameObject removedRoom))
                {
                    Destroy(removedRoom);

                    roomDict.Remove(room.Name);
                }
                continue;
            }
            if (roomDict.ContainsKey(room.Name) == false)
            {
                var _room = Instantiate(roomPrefab, contentTr);
                _room.GetComponent<RoomData>().RoomInfo = room;
                roomDict.Add(room.Name, _room);
            }
            if (roomDict.ContainsKey(room.Name))
            {
                if (roomDict[room.Name].GetComponent<RoomData>().RoomInfo.PlayerCount == room.PlayerCount)
                    continue;
                roomDict[room.Name].GetComponent<RoomData>().RoomInfo = room;
            }

        }//foreach room

    }//OnRoomListUpdate


    public override void OnConnectedToMaster()
    {
        randomRoomBtn.interactable = true;
        makeRoomBtn.interactable = true;
        exitBtn.interactable = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        nickname = $"User_{UnityEngine.Random.Range(0, 1001):0000}";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
    }

    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("3_RoomScene");


        }

    }
    #endregion
}
