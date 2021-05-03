using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RPGM.Gameplay
{
    // MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        }

        // マスターサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnConnectedToMaster()
        {
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        }

        // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
            //var position = new Vector3(3.0f, 10.0f);
            Random.InitState((int)(Time.time * 100));
            var position = new Vector3(Random.Range(2f, 4f), Random.Range(9f, 11f));
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
    }
}
