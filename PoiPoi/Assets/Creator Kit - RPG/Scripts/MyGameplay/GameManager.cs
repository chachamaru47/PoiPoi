using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// �Q�[���}�l�[�W���[
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// �Q�[���t�F�[�Y
        /// </summary>
        public enum GamePhaseType
        {
            /// <summary>
            ///  �I�[�v�j���O
            /// </summary>
            Opening,

            /// <summary>
            /// �Q�[����
            /// </summary>
            InGame,

            /// <summary>
            /// �G���f�B���O
            /// </summary>
            Ending,
        }

        [SerializeField]
        float timer;

        public GamePhaseType GamePhase { get; set; }
        public bool Practice { get; set; }

        int score;
        float record;

        private void Awake()
        {
            score = 0;
            record = -1.0f;
            GamePhase = GamePhaseType.Opening;
        }

        // Start is called before the first frame update
        void Start()
        {
            UI.Score.SetScore(score);
            UI.Timer.SetTime(timer);

            // �I�[�v�j���O�V�[���J�n
            StartCoroutine(OpeningCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
            if (GamePhase == GamePhaseType.InGame)
            {
                if (Practice)
                {
                    // ���K���[�h���͎��Ԃ��~�߂�
                    UI.Timer.SetTime(-1.0f);
                }
                else
                {
                    // ���Ԍo�ߏ���
                    timer -= Time.deltaTime;
                    if (timer <= 0.0f)
                    {
                        // �Q�[���I��
                        timer = 0.0f;
                        // �G���f�B���O�V�[���J�n
                        StartCoroutine(EndingCoroutine());
                    }
                    UI.Timer.SetTime(timer);
                }
            }
        }

        /// <summary>
        /// ���_���Z
        /// </summary>
        /// <param name="point">�_��</param>
        public void AddScore(int point)
        {
            if (Practice) { return; }

            score += point;
            UI.Score.SetScore(score);
        }

        /// <summary>
        /// �L�^�X�V����
        /// </summary>
        /// <param name="distance">�򋗗�</param>
        /// <returns>�L�^�X�V������</returns>
        public bool UpdateRecord(float distance)
        {
            // ���K���[�h���͋L�^�X�V���Ȃ�
            if (Practice) { return false; }

            if (record < distance)
            {
                // �L�^�X�V����
                record = distance;
                return true;
            }
            return false;
        }

        /// <summary>
        /// �I�[�v�j���O�V�[���R���[�`��
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator OpeningCoroutine()
        {
            GamePhase = GamePhaseType.Opening;
            UI.FadeScreen.FadeIn(1.0f, Color.black);
            yield return new WaitForSeconds(1.5f);

            UI.MessageBoard.Show(
                "Move : R Stick\n" +
                "Pic or Charge : A\n" +
                "Dash : R Trigger\n" +
                "Stay : L Trigger",
                true);
            yield return new WaitForSeconds(0.5f);

            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

            UI.MessageBoard.Show("Let's ....");
            yield return new WaitForSeconds(1.5f);

            UI.MessageBoard.Show("Poi Poi !!!!");
            yield return new WaitForSeconds(1.5f);

            UI.MessageBoard.Hide();
            UI.Score.Show();
            UI.Timer.Show();
            GamePhase = GamePhaseType.InGame;
            yield break;
        }

        /// <summary>
        /// �G���f�B���O�V�[���R���[�`��
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndingCoroutine()
        {
            GamePhase = GamePhaseType.Ending;
            UI.Score.Hide();
            UI.Timer.Hide();
            UI.MessageBoard.Show("Finish !!!!");
            yield return new WaitForSeconds(3.0f);

            UI.MessageBoard.Hide();
            UI.Result.Show(score, record);
            yield return new WaitForSeconds(0.5f);

            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

            UI.Result.Hide();
            yield return new WaitForSeconds(1.0f);

            UI.FadeScreen.FadeOut(1.0f, Color.white);
            yield return new WaitForSeconds(1.0f);

            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            yield break;
        }
    }
}
