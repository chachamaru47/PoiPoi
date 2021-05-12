using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// スコア表示UI
    /// </summary>
    public class Score : MonoBehaviour
    {
        public LobbyPlayer player;
        public TMPro.TextMeshProUGUI textMeshProUGUI;

        void Awake()
        {
            SetScore(0);
            Hide();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        /// <param name="showNo">プレイヤーゲーム番号を表示するか</param>
        public void Show(int playerGameNo, bool showNo)
        {
            player.SetPlayerGameNo(playerGameNo, showNo);
            player.Show(false);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 得点を設定
        /// </summary>
        /// <param name="score">得点</param>
        public void SetScore(int score)
        {
            textMeshProUGUI.text = score.ToString();
        }
    }
}
