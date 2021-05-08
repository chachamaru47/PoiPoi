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
        public int playerGameNo;
        public TMPro.TextMeshProUGUI textMeshProUGUI;

        void Awake()
        {
            player.SetPlayerGameNo(playerGameNo, Gameplay.NetworkManager.IsOnlineMode);
            player.Show(false);
            SetScore(0);
            Hide();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        public void Show()
        {
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
