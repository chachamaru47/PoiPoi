using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// リザルト表示UI
    /// とりあえずシングルトン
    /// </summary>
    public class Result : MonoBehaviour
    {
        public ResultPlayer resultPlayerBase;
        public RectTransform players;

        static Result instance;

        private List<ResultPlayer> resultPlayers;

        void Awake()
        {
            instance = this;

            resultPlayers = new List<ResultPlayer>();
            for (int i = 0; i < Gameplay.NetworkManager.RoomMaxPlayers; i++)
            {
                var player = Instantiate(resultPlayerBase);
                player.transform.SetParent(players);
                player.Hide();
                resultPlayers.Add(player);
            }
            Hide();
        }

        /// <summary>
        /// プレイヤー情報を表示する
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        /// <param name="you">自分である表示をするか</param>
        /// <param name="winner">勝者表示をするか</param>
        /// <param name="score">スコア</param>
        /// <param name="record">レコード</param>
        public static void ShowPlayer(int playerGameNo, bool you, bool winner, int score, float record)
        {
            instance.resultPlayers[playerGameNo].Show(playerGameNo, you, winner, score, record);
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
    }
}
