using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// トップメニュー
    /// </summary>
    public class TopMenu : MonoBehaviour
    {
        /// <summary>
        /// ゲームモード
        /// </summary>
        enum GameMode
        {
            /// <summary>
            /// オフラインゲーム
            /// </summary>
            OfflineMode,

            /// <summary>
            /// オンラインゲーム
            /// </summary>
            OnlineMode,
        }

        public float deadZone = 0.2f;
        public GameObject[] CursorList;
        public AudioClip onSelect;
        public AudioClip onDecide;

        private AudioSource audioSource;
        private GameMode cursorPos;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            SetCursor(Gameplay.NetworkManager.IsOnlineMode ? GameMode.OnlineMode : GameMode.OfflineMode);

            StartCoroutine(ModeSelectCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
            // アプリ終了の入力監視
            InputController.MonitoringQuitInput();
        }

        /// <summary>
        /// カーソル位置の設定
        /// </summary>
        /// <param name="mode">カーソルが合っているゲームモード</param>
        private void SetCursor(GameMode mode)
        {
            cursorPos = mode;
            for (int i = 0; i < CursorList.Length; i++)
            {
                CursorList[i].SetActive(i == (int)cursorPos);
            }
        }

        /// <summary>
        /// モード選択コルーチン
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator ModeSelectCoroutine()
        {
            FadeScreen.FadeIn(1.0f, Color.black);

            while(!Input.GetButtonDown("Submit"))
            {
                float vertical = Input.GetAxis("Vertical");
                if (vertical > deadZone)
                {
                    if (cursorPos != GameMode.OfflineMode)
                    {
                        audioSource.PlayOneShot(onSelect);
                        SetCursor(GameMode.OfflineMode);
                    }
                }
                if (vertical < -deadZone)
                {
                    if (cursorPos != GameMode.OnlineMode)
                    {
                        audioSource.PlayOneShot(onSelect);
                        SetCursor(GameMode.OnlineMode);
                    }
                }
                yield return null;
            }

            StartCoroutine(StartGameCoroutine());
            yield break;
        }

        /// <summary>
        /// ゲームスタートコルーチン
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator StartGameCoroutine()
        {
            audioSource.PlayOneShot(onDecide);
            FadeScreen.FadeOut(0.5f, Color.white);
            yield return new WaitForSeconds(0.5f);

            Gameplay.NetworkManager.IsOnlineMode = (cursorPos == GameMode.OnlineMode);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            yield break;
        }
    }
}
