using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// �t�F�[�hUI
    /// �Ƃ肠�����V���O���g��
    /// </summary>
    public class FadeScreen : MonoBehaviour
    {
        public UnityEngine.UI.Image fade;

        static FadeScreen instance;

        float startAlpha;
        float endAlpha;
        float time;
        Color color;

        void Awake()
        {
            instance = this;
            time = -1.0f;
        }

        private void Update()
        {
            if (time > 0.0f)
            {
                // �t�F�[�h�J�n
                StartCoroutine(instance.FadeCoroutine(startAlpha, endAlpha, time, color));
                time = -1.0f;
            }
        }

        /// <summary>
        /// �t�F�[�h����R���[�`��
        /// </summary>
        /// <param name="startAlpha">�J�n�A���t�@�l</param>
        /// <param name="endAlpha">�I���A���t�@�l</param>
        /// <param name="time">�t�F�[�h����</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float time, Color color)
        {
            float elapsed_time = 0.0f;
            fade.enabled = true;

            // �t�F�[�h���Ԃ������Đ��`���
            while(elapsed_time < time)
            {
                color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed_time / time);
                fade.color = color;
                yield return null;
                elapsed_time += Time.deltaTime;
            }

            // �t�F�[�h�I��
            if(endAlpha > 0.0f)
            {
                color.a = endAlpha;
                fade.color = color;
            }
            else
            {
                fade.enabled = false;
            }
            yield break;
        }

        /// <summary>
        /// �t�F�[�h�C���J�n
        /// </summary>
        /// <param name="time">�t�F�[�h����</param>
        /// <param name="color">�t�F�[�h�F</param>
        public static void FadeIn(float time, Color color)
        {
            instance.startAlpha = 1.0f;
            instance.endAlpha = 0.0f;
            instance.time = time;
            instance.color = color;
        }

        /// <summary>
        /// �t�F�[�h�A�E�g�J�n
        /// </summary>
        /// <param name="time">�t�F�[�h����</param>
        /// <param name="color">�t�F�[�h�F</param>
        public static void FadeOut(float time, Color color)
        {
            instance.startAlpha = 0.0f;
            instance.endAlpha = 1.0f;
            instance.time = time;
            instance.color = color;
        }
    }
}
