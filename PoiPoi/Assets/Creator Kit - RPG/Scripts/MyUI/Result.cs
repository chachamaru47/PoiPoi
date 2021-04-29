using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// ���U���g�\��UI
    /// �Ƃ肠�����V���O���g��
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
        /// �\������
        /// </summary>
        /// <param name="score">���_</param>
        /// <param name="record">�L�^</param>
        public static void Show(int score, float record)
        {
            // �L�^�Ȃ���0�\��
            if (record < 0.0f) { record = 0.0f; }

            instance.textMeshProUGUIScore.text = score.ToString();
            instance.textMeshProUGUIRecord.text = $"{record.ToString("0.00")}m";
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
