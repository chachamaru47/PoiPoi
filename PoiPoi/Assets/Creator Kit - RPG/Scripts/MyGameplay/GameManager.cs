using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// ゲームマネージャー
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// ゲームフェーズ
        /// </summary>
        public enum GamePhaseType
        {
            /// <summary>
            ///  オープニング
            /// </summary>
            Opening,

            /// <summary>
            /// ゲーム中
            /// </summary>
            InGame,

            /// <summary>
            /// エンディング
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

            // オープニングシーン開始
            StartCoroutine(OpeningCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
            if (GamePhase == GamePhaseType.InGame)
            {
                if (Practice)
                {
                    // 練習モード中は時間を止める
                    UI.Timer.SetTime(-1.0f);
                }
                else
                {
                    // 時間経過処理
                    timer -= Time.deltaTime;
                    if (timer <= 0.0f)
                    {
                        // ゲーム終了
                        timer = 0.0f;
                        // エンディングシーン開始
                        StartCoroutine(EndingCoroutine());
                    }
                    UI.Timer.SetTime(timer);
                }
            }
        }

        /// <summary>
        /// 得点加算
        /// </summary>
        /// <param name="point">点数</param>
        public void AddScore(int point)
        {
            if (Practice) { return; }

            score += point;
            UI.Score.SetScore(score);
        }

        /// <summary>
        /// 記録更新判定
        /// </summary>
        /// <param name="distance">飛距離</param>
        /// <returns>記録更新したか</returns>
        public bool UpdateRecord(float distance)
        {
            // 練習モード中は記録更新しない
            if (Practice) { return false; }

            if (record < distance)
            {
                // 記録更新した
                record = distance;
                return true;
            }
            return false;
        }

        /// <summary>
        /// オープニングシーンコルーチン
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
        /// エンディングシーンコルーチン
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
