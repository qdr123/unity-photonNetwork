using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; //포톤네트워크 핵심기능!
using Photon.Realtime; //포톤 서비스 관련(룸옵션, 디스커넥션 등)

//네트워크 매니져  : 룸(게임공간)으로 연결시켜주는 역할 
//포톤네트워크 : 마스터서버 -> 로비(대기실) -> 룸(게임공간) 
//MonoBehaviourPunCallbacks:포톤서버 접속,로비접속 , 룸생성실패 등등 이벤트함수 실행시켜줌
public class Netmanger : MonoBehaviourPunCallbacks 
{
    public Text infoText; // 네트워크 상태를 보여줄 텍스트
    public Button connectButton;// 룸 접속 버튼

    string gameVersion = "1"; //게임버전

    private void Awake()
    {
        //해상도 설정
        Screen.SetResolution(800,600,FullScreenMode.Windowed);
    }

    //네트워크 매니져 실행되면 제일 먼저 할일?!


    // Start is called before the first frame update
    void Start()
    {
        //접속에 필요한 정보(게임버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        //마스터 서버에 접속하는 함수 (제일중요)
        PhotonNetwork.ConnectUsingSettings();
        //접속 시도중임을 텍스트로 표시
        infoText.text = "마스터 서버에 접속중!!!";
        //룸(게임공간) 접속 버튼 비활성화
        connectButton.interactable = false;

    }

    public override void OnConnectedToMaster()
    {
        //접속  정보 표시
        infoText.text = "온라인 :  마스터 서버와 연결됨!!!";
        //룸(게임공간) 접속 버튼 비활성화
        connectButton.interactable = true;

        base.OnConnectedToMaster();
    }
  


    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸(게임공간) 접속 버튼 비활성화
        connectButton.interactable = false;
        //접속  정보 표시
        infoText.text = "온라인 :  마스터 서버와 연결됨실패 \n 접속 재시도중 !!!";
        //마스터 서버에 접속하는 함수 (제일중요)
        PhotonNetwork.ConnectUsingSettings();
    }

    //접속 버튼 클릭시 이함수 발동
    public void OnConnect()
    {
        //중복 접속 차단하기 위해 접속벝는 비활성화
        connectButton.interactable = false;

        //마스터 서버에 접속중이냐?
        if(PhotonNetwork.IsConnected)
        {
            //룸으로 바로 접속 실행
            infoText.text = "랜덤방에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //접속 정보 
            infoText.text = "오프라인 :  마스터 서버와 연결 실패 \n 재접속중";
            //마스터 서버에 접속하는 함수(이놈중요)
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinedRoom()
    {
        //접속 정보 
        infoText.text = "방 참가 성공했음";
        //모든 룸참가자들이 "GameScene"을 로드함
        PhotonNetwork.LoadLevel("gameScene");
    }

    //(빈방이 없어)랜덤 룸 참가에 실패한 경우 자동실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 정보 
        infoText.text = "빈방이 없으니 새로운 방 생성중";
        //빈바을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
