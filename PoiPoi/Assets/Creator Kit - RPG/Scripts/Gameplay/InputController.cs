using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.UI
{
    /// <summary>
    /// Sends user input to the correct control systems.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        /// <summary>
        /// 操作スタイル
        /// </summary>
        public enum ControlStyle
        {
            /// <summary>
            /// クラシック操作
            /// </summary>
            Classic,

            /// <summary>
            /// NEWコントロール
            /// </summary>
            NewControl,
        }
        public ControlStyle controlStyle { get; set; } = ControlStyle.NewControl;

        public float stepSize = 0.1f;
        public float accelMax = 2f;
        public float deadZone = 0.2f;
        GameModel model = Schedule.GetModel<GameModel>();
        float triggertAxis = 0.0f;
        Vector3 reserveAimCommand = Vector3.zero;
        float reserveAimTime = 0.0f;

        public enum State
        {
            CharacterControl,
            DialogControl,
            Pause
        }

        State state;

        public void ChangeState(State state) => this.state = state;

        void Update()
        {
            // アプリ終了の入力監視
            MonitoringQuitInput();

            // 練習モード切り替え
            if (Input.GetButtonDown("Jump"))
            {
                model.gameManager.Practice = !model.gameManager.Practice;
            }

            // ゲーム中以外は以降の操作は受け付けずに帰る
            if (model.gameManager.GamePhase != GameManager.GamePhaseType.InGame)
            {
                model.player.nextMoveCommand = Vector3.zero;
                model.player.nextAimCommand = Vector3.zero;
                model.player.isAiming = false;
                reserveAimTime = 0.0f;
                reserveAimCommand = Vector3.zero;
                triggertAxis = 0.0f;
                return;
            }

            // 状態ごとの操作
            switch (state)
            {
                case State.CharacterControl:
                    CharacterControl();
                    break;
                case State.DialogControl:
                    DialogControl();
                    break;
            }
        }

        /// <summary>
        /// ダイアログ操作
        /// </summary>
        void DialogControl()
        {
            model.player.nextMoveCommand = Vector3.zero;
            model.player.nextAimCommand = Vector3.zero;
            model.player.isAiming = false;
            reserveAimTime = 0.0f;
            reserveAimCommand = Vector3.zero;
            triggertAxis = 0.0f;

            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal < -deadZone)
            {
                model.dialog.FocusButton(-1);
            }
            if (horizontal > deadZone)
            {
                model.dialog.FocusButton(+1);
            }
            if (Input.GetButtonDown("Submit"))
            {
                model.dialog.SelectActiveButton();
            }
        }

        /// <summary>
        /// キャラクター操作
        /// </summary>
        void CharacterControl()
        {
            switch(controlStyle)
            {
                case ControlStyle.Classic:
                    CharacterControlClassic();
                    break;
                case ControlStyle.NewControl:
                default:
                    CharacterControlNewControl();
                    break;
            }
        }

        /// <summary>
        /// キャラクター操作(クラシック操作)
        /// </summary>
        void CharacterControlClassic()
        {
            model.player.nextAimCommand = Vector3.up * Input.GetAxis("Vertical");
            model.player.nextAimCommand += Vector3.right * Input.GetAxis("Horizontal");
            triggertAxis = Input.GetAxis("Trigger");
            if ((triggertAxis < -0.01f) || Input.GetButton("Stay"))
            {
                model.player.isAiming = model.player.nextAimCommand.magnitude > deadZone;
                model.player.nextMoveCommand = Vector3.zero;
            }
            else
            {
                model.player.isAiming = false;
                model.player.nextMoveCommand = Vector3.up * Input.GetAxis("Vertical");
                model.player.nextMoveCommand += Vector3.right * Input.GetAxis("Horizontal");
                if (model.player.nextMoveCommand.magnitude > deadZone)
                {
                    float accel = 1.0f + accelMax * ((triggertAxis > 0.01f) ? triggertAxis : Input.GetButton("Dash") ? 1.0f : 0.0f);
                    model.player.nextMoveCommand *= accel * stepSize;
                }
                else
                {
                    model.player.nextMoveCommand = Vector3.zero;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                model.player.onFire = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                model.player.releaseFire = true;
            }
        }

        /// <summary>
        /// キャラクター操作(NEWコントロール)
        /// </summary>
        void CharacterControlNewControl()
        {
            model.player.nextAimCommand = Vector3.up * Input.GetAxis("AimVertical");
            model.player.nextAimCommand += Vector3.right * Input.GetAxis("AimHorizontal");
            model.player.nextMoveCommand = Vector3.up * Input.GetAxis("Vertical");
            model.player.nextMoveCommand += Vector3.right * Input.GetAxis("Horizontal");

            if (model.player.nextMoveCommand.magnitude > deadZone)
            {
                float accel = 1.0f + accelMax;
                model.player.nextMoveCommand *= accel * stepSize;
            }
            else
            {
                model.player.nextMoveCommand = Vector3.zero;
            }

            model.player.isAiming = model.player.nextAimCommand.magnitude > deadZone;
            if (model.player.isAiming)
            {
                // 投げる瞬間にエイム入力を放してもエイムしていた方向に投げる為に保存
                reserveAimTime = 0.2f;
                reserveAimCommand = model.player.nextAimCommand;
            }
            else
            {
                if (reserveAimTime > 0.0f)
                {
                    // 投げる瞬間にエイム入力を放してもエイムしていた方向に投げる
                    reserveAimTime -= Time.deltaTime;
                    model.player.nextAimCommand = reserveAimCommand;
                }
                else
                {
                    // エイム入力が無くなったら移動入力方向に投げる
                    model.player.nextAimCommand = model.player.nextMoveCommand;
                }
            }

            // 拾う投げるはRトリガー操作だが、とりあえずAボタンでも反応するようにしておく
            float newTriggertAxis = Input.GetAxis("Trigger");
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Dash") || (newTriggertAxis > 0.5f && triggertAxis <= 0.5f))
            {
                model.player.onFire = true;
            }
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Dash") || (newTriggertAxis <= 0.5f && triggertAxis > 0.5f))
            {
                model.player.releaseFire = true;
            }
            triggertAxis = newTriggertAxis;
        }

        /// <summary>
        /// アプリ終了
        /// </summary>
        static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }

        /// <summary>
        /// アプリ終了の入力監視
        /// </summary>
        public static void MonitoringQuitInput()
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey("joystick button 4") && Input.GetKey("joystick button 5") && Input.GetKey("joystick button 6") && Input.GetKey("joystick button 7"))
            {
                // アプリ終了
                Quit();
            }
        }
    }
}
