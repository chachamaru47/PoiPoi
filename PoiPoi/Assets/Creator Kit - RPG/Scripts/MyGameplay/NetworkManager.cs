using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RPGM.Gameplay
{
    // MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static bool IsOnlineMode { get; set; } = false;
        public static byte RoomMaxPlayers { get; } = 4;

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
            roomOptions.MaxPlayers = RoomMaxPlayers;

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            ;
        }
    }

    /// <summary>
    /// Photonのプレイヤーカスタムプロパティの取得や設定する拡張メソッド
    /// </summary>
    public static class PhotonPlayerPropertiesExtensions
    {
        private const string ScoreKey = "Score";
        private const string RecordKey = "Record";
        private const string GameNoKey = "GameNo";
        private const string ReadyKey = "Ready";

        private static readonly Hashtable propsToSet = new Hashtable();

        // プレイヤーのスコアを取得する
        public static int GetScore(this Player player)
        {
            return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
        }
        // プレイヤーのスコアを設定する
        public static void SetScore(this Player player, int score)
        {
            propsToSet[ScoreKey] = score;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        // プレイヤーのレコードを取得する
        public static float GetRecord(this Player player)
        {
            return (player.CustomProperties[RecordKey] is int record) ? record / 100.0f : -1.0f;
        }
        // プレイヤーのレコードを設定する
        public static void SetRecord(this Player player, float record)
        {
            propsToSet[RecordKey] = (int)(record * 100.0f);
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        // プレイヤーのゲーム番号を取得する
        public static int GetGameNo(this Player player)
        {
            return (player.CustomProperties[GameNoKey] is int gameNo) ? gameNo : -1;
        }
        // プレイヤーのゲーム番号を設定する
        public static void SetGameNo(this Player player, int gameNo)
        {
            propsToSet[GameNoKey] = gameNo;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        // プレイヤーの準備完了フラグを取得する
        public static bool GetReady(this Player player)
        {
            return (player.CustomProperties[ReadyKey] is bool ready) ? ready : false;
        }
        // プレイヤーの準備完了フラグを設定する
        public static void SetReady(this Player player, bool ready)
        {
            propsToSet[ReadyKey] = ready;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }
    }
}
