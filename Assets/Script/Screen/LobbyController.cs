using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class LobbyController : MonoBehaviourPunCallbacks
{
    string playerName = "Player 1";
    string gameVersion = "1.0";
    List<RoomInfo> createdRooms = new List<RoomInfo>();

    string roomName = "Room 1";
    Vector2 roomListScroll = Vector2.zero;

    bool joiningRoom = false;
    void Start()
    {
        playerName = $"Player {Random.Range(1, 5)}";

        //Dong bo canh tu dong
        PhotonNetwork.AutomaticallySyncScene = true;

        //Kiem tra ket noi, neu chua ket noi thi ket noi theo thiet lap mac dinh
        if (!PhotonNetwork.IsConnected)
        {
            // Thiet lap phien ban
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            // Thiet lap vung server co dinh
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
            // Ket noi toi Photon
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //OnDisconnetedduoc goi khi Photon mat ket noi toi Photon Server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconected to server. Error code: {cause.ToString()}. Server Address: {PhotonNetwork.ServerAddress}");
    }

    // OnConnectedToMaster duoc goi khi ket noi thanh cong toi master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Mater server successfully!!!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //OnListRoomUpdate duoc goi khi nhan duoc danh sach phong
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Received room list");
        createdRooms = roomList;

    }

    //GUI dung de hien thi giao dien nguoi dung
    void OnGUI()
    {
        GUI.Window(0, new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400), LobbyWindow, "Lobby");
    }
    void LobbyWindow(int index)
    {
        // Hiển thị trạng thái kết nối và nút tạo phòng
        GUILayout.BeginHorizontal();

        GUILayout.Label("Trạng thái: " + PhotonNetwork.NetworkClientState); // Hiển thị trạng thái kết nối hiện tại

        // Kiểm tra nếu đang kết nối, đang gia nhập phòng hoặc chưa kết nối lobby thì nút sẽ bị vô hiệu hóa
        if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }

        GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

        // Cho phép nhập tên phòng và nút tạo phòng
        roomName = GUILayout.TextField(roomName, GUILayout.Width(250)); // Hiển thị ô nhập tên phòng

        if (GUILayout.Button("Tạo phòng", GUILayout.Width(125))) // Nút tạo phòng
        {
            if (roomName != "") // Kiểm tra nếu tên phòng trống thì không thực hiện tạo phòng
            {
                joiningRoom = true;

                RoomOptions roomOptions = new RoomOptions(); // Thiết lập các tùy chọn phòng
                roomOptions.IsOpen = true; // Phòng mở (cho phép người khác tham gia)
                roomOptions.IsVisible = true; // Phòng hiển thị trong danh sách
                roomOptions.MaxPlayers = (byte)10; // Số người chơi tối đa (có thể thay đổi)

                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // Tạo hoặc tham gia phòng với tên đã nhập
            }
        }

        GUILayout.EndHorizontal();

        // Duyệt qua danh sách phòng có thể tham gia
        roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true); // Khởi tạo thanh cuộn danh sách phòng

        if (createdRooms.Count == 0) // Kiểm tra nếu không có phòng nào
        {
            GUILayout.Label("Hiện chưa có phòng nào...");
        }
        else
        {
            for (int i = 0; i < createdRooms.Count; i++) // Lặp qua từng phòng trong danh sách
            {
                GUILayout.BeginHorizontal("box"); // Khung nền cho mỗi phòng

                GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400)); // Hiển thị tên phòng
                GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers); // Hiển thị số người chơi hiện tại/tối đa

                GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

                if (GUILayout.Button("Tham gia phòng")) // Nút tham gia phòng
                {
                    joiningRoom = true;

                    // Thiết lập tên người chơi
                    PhotonNetwork.NickName = playerName;

                    // Tham gia phòng với tên đã chọn
                    PhotonNetwork.JoinRoom(createdRooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView(); // Kết thúc thanh cuộn danh sách phòng

        // Hiển thị ô nhập tên người chơi và nút làm mới danh sách phòng
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tên người chơi: ", GUILayout.Width(85));
        //Player name text field
        playerName = GUILayout.TextField(playerName, GUILayout.Width(250)); // Hiển thị ô nhập tên người chơi

        GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

        // Kiểm tra trạng thái kết nối và không đang gia nhập phòng thì nút làm mới mới hoạt động
        GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby || PhotonNetwork.NetworkClientState == ClientState.Disconnected) && !joiningRoom;
        if (GUILayout.Button("Làm mới", GUILayout.Width(100))) // Nút làm mới danh sách phòng
        {
            if (PhotonNetwork.IsConnected) // Kiểm tra nếu đã kết nối
            {
                // Tham gia lại lobby để lấy danh sách phòng mới nhất
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
            else // Nếu chưa kết nối thì thiết lập kết nối mới
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        GUILayout.EndHorizontal();

        // Hiển thị thông báo "Đang kết nối..." khi đang trong quá trình gia nhập phòng
        if (joiningRoom)
        {
            GUI.enabled = true; // Bật giao diện hiển thị thông báo
            GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Đang kết nối..."); // Hiển thị thông báo
        }
    }

    //OnCreateRoomFailed duoc goi khi tao phong that bai
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Create room failed. Error code: {returnCode}. Notification: {message}. Maybe room is exits even if not displayed ");
        joiningRoom = false;
    }

    //OnJoinRoomFailed duoc goi khi tham gia phong that bai
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join room failed. Error code: {returnCode}. Notification: {message}. Maybe room is full, close or doesn't exits!");
        joiningRoom = false;
    }

    //OnJoinRandomFailed duoc tao khi tham gia phong ngau nhien that bai

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join room failed. Error code: {returnCode}. Notification: {message}. Maybe room is full, close or doesn't exits!");
        joiningRoom = false;
    }

    //OncreatedRoom duoc goi khi tao phong thanh cong
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room!");
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.LoadLevel("GameRoom");
    }

    //OnJoinedRoom duoc goi khi tham gia phong thanh cong
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room!");
    }
}
