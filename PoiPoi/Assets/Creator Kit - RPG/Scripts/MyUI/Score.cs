using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// スコア表示UI
    /// とりあえずシングルトン
    /// </summary>
    public class Score : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI textMeshProUGUI;

        static Score instance;

        void Awake()
        {
            instance = this;
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
        /// 得点を設定
        /// </summary>
        /// <param name="score">得点</param>
        public static void SetScore(int score)
        {
            instance.textMeshProUGUI.text = score.ToString();
        }
    }
}
