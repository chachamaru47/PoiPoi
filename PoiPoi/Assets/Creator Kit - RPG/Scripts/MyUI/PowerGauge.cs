using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// �p���[�Q�[�WUI
    /// </summary>
    public class PowerGauge : MonoBehaviour
    {
        public RectTransform gauge;

        private float maxWidth;

        void Awake()
        {
            maxWidth = gauge.rect.width;
            Hide();
        }

        /// <summary>
        /// �\������
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �Q�[�W�ʂ�ݒ肵�������ŕ\������
        /// </summary>
        /// <param name="power_ratio"></param>
        public void Show(float power_ratio)
        {
            gauge.sizeDelta = new Vector2(maxWidth * power_ratio, gauge.sizeDelta.y);
            Show();
        }

        /// <summary>
        /// ��\���ɂ���
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ���ԍ��Ŕ�\���ɂ���
        /// </summary>
        /// <param name="delay">�x�点�鎞��</param>
        public void Hide(float delay)
        {
            Invoke("Hide", delay);
        }
    }
}
