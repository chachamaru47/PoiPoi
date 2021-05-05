using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RPGM.Gameplay
{
    // MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static bool IsOnlineMode { get; set; } = false;

        private void Start()
        {
            // プレイヤー自身の名前をPCのユーザー名に設定する
            PhotonNetwork.NickName = System.Environment.UserName;

            if (IsOnlineMode)
            {
                // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                // オフラインモードで接続する
                // falseからtrueに設定されると、PhotonはコールOnConnectedToMaster() とともにコールバックをおこないます。その後ルームを作成可能になり、このルームはオフラインになります。
                PhotonNetwork.OfflineMode = true;
            }
        }

        // マスターサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnConnectedToMaster()
        {
#if false
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
#endif
            // ランダムなルームに参加する
            PhotonNetwork.JoinRandomRoom();
        }

        // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // ルームの参加人数を設定する
            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            ;
        }
    }
}
