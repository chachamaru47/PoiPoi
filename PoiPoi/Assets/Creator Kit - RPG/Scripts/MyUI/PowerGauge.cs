using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// パワーゲージUI
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
        /// 表示する
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// ゲージ量を設定したうえで表示する
        /// </summary>
        /// <param name="power_ratio"></param>
        public void Show(float power_ratio)
        {
            gauge.sizeDelta = new Vector2(maxWidth * power_ratio, gauge.sizeDelta.y);
            Show();
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 時間差で非表示にする
        /// </summary>
        /// <param name="delay">遅らせる時間</param>
        public void Hide(float delay)
        {
            Invoke("Hide", delay);
        }
    }
}
