using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// ロビーウィンドウ
    /// ログインしているプレイヤーの表示UI
    /// </summary>
    public class Lobby : MonoBehaviour
    {
        public LobbyPlayer lobbyPlayerBase;
        public RectTransform players;

        static Lobby instance;

        private List<LobbyPlayer> playerList;

        void Awake()
        {
            instance = this;

            playerList = new List<LobbyPlayer>();

            for (int i = 0; i < Gameplay.NetworkManager.RoomMaxPlayers; i++)
            {
                var player = Instantiate(lobbyPlayerBase);
                player.transform.SetParent(players);
                player.SetPlayerGameNo(i);
                player.Hide();
                playerList.Add(player);
            }

            Hide();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        public static void Show()
        {
            instance.gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }

        /// <summary>
        /// 指定のプレイヤー表示をオン
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        /// <param name="you">自分である表示をするか</param>
        public static void PlayerOn(int playerGameNo, bool you)
        {
            instance.playerList[playerGameNo].Show(you);
        }

        /// <summary>
        /// 指定のプレイヤー表示をオフ
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        public static void PlayerOff(int playerGameNo)
        {
            instance.playerList[playerGameNo].Hide();
        }
    }
}
