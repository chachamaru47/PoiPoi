using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// ロビーウィンドウに表示するプレイヤー
    /// </summary>
    public class LobbyPlayer : MonoBehaviour
    {
        public UnityEngine.UI.Image player;
        public TMPro.TextMeshProUGUI noText;
        public TMPro.TextMeshProUGUI youText;

        /// <summary>
        /// プレイヤーのゲーム番号を設定
        /// </summary>
        /// <param name="playerGameNo">プレイヤーゲーム番号</param>
        public void SetPlayerGameNo(int playerGameNo)
        {
            Color textColor = Gameplay.CharacterController2D.GetPlayerTextColor(playerGameNo);
            noText.text = $"{playerGameNo + 1}P";
            noText.color = textColor;
            youText.color = textColor;
            player.color = Gameplay.CharacterController2D.GetPlayerCharacterColor(playerGameNo);
        }

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="you">自分である表示をするか</param>
        public void Show(bool you)
        {
            youText.enabled = you;
            player.gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            player.gameObject.SetActive(false);
        }
    }
}
