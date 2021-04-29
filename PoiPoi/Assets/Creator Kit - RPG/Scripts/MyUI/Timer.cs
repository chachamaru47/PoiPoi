using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// �^�C�}�[�\��UI
    /// �Ƃ肠�����V���O���g��
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
        /// �\�����Ԃ�ݒ�
        /// </summary>
        /// <param name="time">���ԁA�}�C�i�X�l�Ŗ�����</param>
        public static void SetTime(float time)
        {
            if (time < 0.0f)
            {
                // ������
                instance.textMeshProUGUI.text = "INF";
            }
            else
            {
                instance.textMeshProUGUI.text = Mathf.FloorToInt(time).ToString();
            }
        }
    }
}
