using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// �X�R�A�\��UI
    /// �Ƃ肠�����V���O���g��
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
        /// �\������
        /// </summary>
        public static void Show()
        {
            instance.gameObject.SetActive(true);
        }

        /// <summary>
        /// ��\���ɂ���
        /// </summary>
        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }

        /// <summary>
        /// ���_��ݒ�
        /// </summary>
        /// <param name="score">���_</param>
        public static void SetScore(int score)
        {
            instance.textMeshProUGUI.text = score.ToString();
        }
    }
}
