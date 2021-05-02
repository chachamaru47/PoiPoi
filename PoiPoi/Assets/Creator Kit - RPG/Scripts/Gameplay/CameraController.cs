//#define PIXEL_PERFECT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// A simple camera follower class. It saves the offset from the
    ///  focus position when started, and preserves that offset when following the focus.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public Transform defaultFocus;
        public float smoothTime = 2;

        [System.NonSerialized]
        public Vector3 positionOffset;

        Transform focus;
        Vector3 offset;
        UnityEngine.U2D.PixelPerfectCamera pixelPerfectCamera;
#if PIXEL_PERFECT
        float defaultAssetsPPU;
        float assetsPPU;
#else
        /// <summary>
        /// デフォルトの表示範囲
        /// </summary>
        float defaultOrthographicSize;
#endif

        void Awake()
        {
            ResetFocus();
            offset = new Vector3(0.0f, -0.1f, 10.0f);
            transform.position = focus.position - offset + positionOffset;
            pixelPerfectCamera = GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
#if PIXEL_PERFECT
            pixelPerfectCamera.enabled = true;
            defaultAssetsPPU = pixelPerfectCamera.assetsPPU;
            assetsPPU = pixelPerfectCamera.assetsPPU;
#else
            pixelPerfectCamera.enabled = false;
            defaultOrthographicSize = Camera.main.orthographicSize;
#endif
        }

        void Update()
        {
#if PIXEL_PERFECT
            assetsPPU = Mathf.Lerp(assetsPPU, defaultAssetsPPU - GetTargetDistance() * 1.5f, Time.deltaTime * smoothTime);
            pixelPerfectCamera.assetsPPU = (int)assetsPPU;
#else
            // デフォルトのフォーカス対象と現在のフォーカス対象の距離が離れるほど表示範囲を大きくする
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, defaultOrthographicSize + GetTargetDistance() * 0.3f, Time.deltaTime * smoothTime);
#endif

            transform.position = Vector3.Lerp(transform.position, focus.position - offset + positionOffset, Time.deltaTime * smoothTime);
        }

        /// <summary>
        /// デフォルトのフォーカス対象を設定する
        /// </summary>
        /// <param name="target">デフォルトのフォーカス対象</param>
        public void SetDefaultFocus(Transform target)
        {
            defaultFocus = target;
            ResetFocus();
        }

        /// <summary>
        /// 指定のターゲットにフォーカスを移す
        /// </summary>
        /// <param name="target">ターゲット</param>
        public void SetFocus(Transform target)
        {
            focus = target;
        }

        /// <summary>
        /// フォーカス対象をデフォルトに戻す
        /// </summary>
        public void ResetFocus()
        {
            SetFocus(defaultFocus);
        }

        /// <summary>
        /// 現在のフォーカスが指定のターゲットにあっているか
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <returns>指定ターゲットをフォーカスしていれば真</returns>
        public bool IsTargetFocus(Transform target)
        {
            return focus == target;
        }

        /// <summary>
        /// フォーカス対象がデフォルト状態か
        /// </summary>
        /// <returns>フォーカス対象がデフォルト状態なら真</returns>
        public bool IsDefaultFocus()
        {
            return IsTargetFocus(defaultFocus);
        }

        /// <summary>
        /// デフォルトのフォーカス対象から現在のフォーカスまでの距離
        /// </summary>
        /// <returns>距離</returns>
        private float GetTargetDistance()
        {
            return Mathf.Abs(Vector3.Distance(focus.position, defaultFocus.position));
        }

    }
}
