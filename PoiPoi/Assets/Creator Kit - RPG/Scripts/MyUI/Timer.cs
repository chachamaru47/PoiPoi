using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// タイマー表示UI
    /// とりあえずシングルトン
    /// </summary>
    public class Timer : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI textMeshProUGUI;

        static Timer instance;

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
        /// 表示時間を設定
        /// </summary>
        /// <param name="time">時間、マイナス値で無制限</param>
        public static void SetTime(float time)
        {
            if (time < 0.0f)
            {
                // 無制限
                instance.textMeshProUGUI.text = "INF";
            }
            else
            {
                instance.textMeshProUGUI.text = Mathf.FloorToInt(time).ToString();
            }
        }
    }
}
