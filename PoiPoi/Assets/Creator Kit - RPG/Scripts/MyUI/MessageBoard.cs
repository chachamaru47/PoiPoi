using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// ���b�Z�[�W�\��UI
    /// �Ƃ肠�����V���O���g��
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
        /// �\������
        /// </summary>
        /// <param name="text"></param>
        public static void Show(string text, bool next = false)
        {
            instance.textMeshProUGUI_Message.text = text;
            instance.textMeshProUGUI_Next.gameObject.SetActive(next);
            instance.gameObject.SetActive(true);
        }

        /// <summary>
        /// ��\���ɂ���
        /// </summary>
        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }
    }
}
