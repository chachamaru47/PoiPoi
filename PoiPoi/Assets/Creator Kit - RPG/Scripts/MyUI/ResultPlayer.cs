using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// リザルト表示のプレイヤー情報UI
    /// </summary>
    public class ResultPlayer : MonoBehaviour
    {
        public LobbyPlayer player;
        public TMPro.TextMeshProUGUI textMeshProUGUIWinner;
        public TMPro.TextMeshProUGUI textMeshProUGUIScore;
        public TMPro.TextMeshProUGUI textMeshProUGUIRecord;

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        /// <param name="you">自分である表示をするか</param>
        /// <param name="winner">勝者表示をするか</param>
        /// <param name="score">スコア</param>
        /// <param name="record">レコード</param>
        public void Show(int playerGameNo, bool you, bool winner, int score, float record)
        {
            // 記録なしは0表示
            if (record < 0.0f) { record = 0.0f; }

            player.SetPlayerGameNo(playerGameNo, Gameplay.NetworkManager.IsOnlineMode);
            if(!Gameplay.NetworkManager.IsOnlineMode)
            {
                // オフラインモードだと表示が寂しいのでキャラを大きくしてみる
                player.transform.localScale *= 3.0f;
            }
            player.Show(you);
            textMeshProUGUIWinner.enabled = winner;
            textMeshProUGUIScore.text = score.ToString();
            textMeshProUGUIRecord.text = $"{record.ToString("0.00")}m";

            gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
