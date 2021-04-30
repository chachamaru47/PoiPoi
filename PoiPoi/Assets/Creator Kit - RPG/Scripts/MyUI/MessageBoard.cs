using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// メッセージ表示UI
    /// とりあえずシングルトン
    /// </summary>
    public class MessageBoard : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI textMeshProUGUI_Message;
        public TMPro.TextMeshProUGUI textMeshProUGUI_Next;

        static MessageBoard instance;

        void Awake()
        {
            instance = this;
            Hide();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="text"></param>
        public static void Show(string text, bool next = false)
        {
            instance.textMeshProUGUI_Message.text = text;
            instance.textMeshProUGUI_Next.gameObject.SetActive(next);
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
