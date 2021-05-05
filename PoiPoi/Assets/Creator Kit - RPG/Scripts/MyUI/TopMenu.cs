using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.UI
{
    public class TopMenu : MonoBehaviour
    {
        enum GameMode
        {
            OfflineMode,
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

            FadeScreen.FadeIn(1.0f, Color.black);
        }

        // Update is called once per frame
        void Update()
        {
            // アプリ終了の入力監視
            InputController.MonitoringQuitInput();

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
            if (Input.GetButtonDown("Submit"))
            {
                StartCoroutine(StartGameCoroutine());
            }
        }

        private void SetCursor(GameMode mode)
        {
            cursorPos = mode;
            for (int i = 0; i < CursorList.Length; i++)
            {
                CursorList[i].SetActive(i == (int)cursorPos);
            }
        }

        private IEnumerator StartGameCoroutine()
        {
            audioSource.PlayOneShot(onDecide);
            FadeScreen.FadeOut(0.5f, Color.white);
            yield return new WaitForSeconds(0.5f);

            Gameplay.NetworkManager.IsOnlineMode = (cursorPos == GameMode.OnlineMode);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            yield break;
        }
    }
}
