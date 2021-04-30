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
        public TMPro.TextMeshProUGUI textMeshProUGUIScore;
        public TMPro.TextMeshProUGUI textMeshProUGUIRecord;

        static Result instance;

        void Awake()
        {
            instance = this;
            Hide();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="score">得点</param>
        /// <param name="record">記録</param>
        public static void Show(int score, float record)
        {
            // 記録なしは0表示
            if (record < 0.0f) { record = 0.0f; }

            instance.textMeshProUGUIScore.text = score.ToString();
            instance.textMeshProUGUIRecord.text = $"{record.ToString("0.00")}m";
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
