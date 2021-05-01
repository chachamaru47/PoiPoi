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
        public UnityEngine.UI.Slider slider;

        void Awake()
        {
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
        /// <param name="power_ratio">ゲージの割合</param>
        public void Show(float power_ratio)
        {
            slider.value = power_ratio;
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
