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
        float defaultOrthographicSize;
#endif

        void Awake()
        {
            focus = defaultFocus;
            offset = focus.position - transform.position;
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
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, defaultOrthographicSize + GetTargetDistance() * 0.3f, Time.deltaTime * smoothTime);
#endif

            transform.position = Vector3.Lerp(transform.position, focus.position - offset + positionOffset, Time.deltaTime * smoothTime);
        }

        public void SetFocus(Transform target)
        {
            focus = target;
        }

        public void ResetFocus()
        {
            SetFocus(defaultFocus);
        }

        public bool IsTargetFocus(Transform target)
        {
            return focus == target;
        }

        public bool IsDefaultFocus()
        {
            return IsTargetFocus(defaultFocus);
        }

        private float GetTargetDistance()
        {
            return Mathf.Abs(Vector3.Distance(focus.position, defaultFocus.position));
        }

    }
}
