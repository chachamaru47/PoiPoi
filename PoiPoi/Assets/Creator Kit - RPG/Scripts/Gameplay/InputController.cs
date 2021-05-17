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
            /// チャージエイム(エイムリバース)
            /// </summary>
            ChargeAim_Reverse,

            /// <summary>
            /// チャージエイム
            /// </summary>
            ChargeAim,

            /// <summary>
            /// NEWコントロール(エイムリバース)
            /// </summary>
            NewControl_Reverse,

            /// <summary>
            /// NEWコントロール
            /// </summary>
            NewControl,

            /// <summary>
            /// クラシック操作
            /// </summary>
            Classic,
        }
        public ControlStyle controlStyle { get; set; } = ControlStyle.ChargeAim_Reverse;

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
            switch (controlStyle)
            {
                default:
                case ControlStyle.ChargeAim_Reverse:
                    model.player.isChargeLoop = true;
                    model.player.chargeSpeed = 1.0f;
                    CharacterControlChargeAim(true);
                    break;
                case ControlStyle.ChargeAim:
                    model.player.isChargeLoop = true;
                    model.player.chargeSpeed = 1.0f;
                    CharacterControlChargeAim(false);
                    break;
                case ControlStyle.NewControl_Reverse:
                    model.player.isChargeLoop = false;
                    model.player.chargeSpeed = 1.0f;
                    CharacterControlNewControl(true);
                    break;
                case ControlStyle.NewControl:
                    model.player.isChargeLoop = false;
                    model.player.chargeSpeed = 1.0f;
                    CharacterControlNewControl(false);
                    break;
                case ControlStyle.Classic:
                    model.player.isChargeLoop = false;
                    model.player.chargeSpeed = 1.0f;
                    CharacterControlClassic();
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

            model.player.onPick = false;
            model.player.onCharge = false;
            model.player.onFire = false;
            model.player.offCharge = false;
            if (Input.GetButtonDown("Fire1"))
            {
                model.player.onPick = true;
                model.player.onCharge = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                model.player.onFire = true;
                model.player.offCharge = false;
            }
        }

        /// <summary>
        /// キャラクター操作(NEWコントロール)
        /// </summary>
        /// <param name="reverse">エイム軸反転</param>
        void CharacterControlNewControl(bool reverse)
        {
            model.player.onPick = false;
            model.player.onCharge = false;
            model.player.onFire = false;
            model.player.offCharge = false;

            model.player.nextAimCommand = Vector3.up * Input.GetAxis("AimVertical");
            model.player.nextAimCommand += Vector3.right * Input.GetAxis("AimHorizontal");
            if (reverse)
            {
                model.player.nextAimCommand *= -1;
            }
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
                model.player.onPick = true;
                model.player.onCharge = true;
            }
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Dash") || (newTriggertAxis <= 0.5f && triggertAxis > 0.5f))
            {
                model.player.onFire = true;
                model.player.offCharge = true;
            }
            triggertAxis = newTriggertAxis;
        }

        /// <summary>
        /// キャラクター操作(チャージエイム)
        /// </summary>
        /// <param name="reverse">エイム軸反転</param>
        void CharacterControlChargeAim(bool reverse)
        {
            model.player.onPick = false;
            model.player.onCharge = false;
            model.player.onFire = false;

            Vector3 oldAimCommand = reserveAimCommand;
            Vector3 newAimCommand = Vector3.up * Input.GetAxis("AimVertical");
            newAimCommand += Vector3.right * Input.GetAxis("AimHorizontal");
            if (reverse)
            {
                newAimCommand *= -1;
            }
            model.player.nextAimCommand = newAimCommand;
            reserveAimCommand = newAimCommand;

            model.player.isAiming = newAimCommand.magnitude > deadZone;
            model.player.onCharge = newAimCommand.magnitude > deadZone;
            model.player.offCharge = !model.player.onCharge;
#if false
            if (model.player.isAiming)
            {
                // 投げる瞬間にエイム入力を放してもエイムしていた方向に投げる為に保存
                reserveAimTime = 0.2f;
                reserveAimCommand = newAimCommand;
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
                    reserveAimCommand = Vector3.zero;
                    model.player.nextAimCommand = model.player.nextMoveCommand;
                }
            }
#endif
            if (newAimCommand.sqrMagnitude < oldAimCommand.sqrMagnitude)
            {
                if (Vector3.Distance(newAimCommand, oldAimCommand) > 0.5f)
                {
                    // スティックを一気に放したら投げる
                    model.player.nextAimCommand = oldAimCommand;
                    model.player.onFire = true;
                }
            }

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

            float newTriggertAxis = Input.GetAxis("Trigger");
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Dash") || (newTriggertAxis > 0.5f && triggertAxis <= 0.5f))
            {
                model.player.onPick = true;
                model.player.onFire = true;
            }
            triggertAxis = newTriggertAxis;
            if (Input.GetKeyDown("joystick button 9"))
            {
                model.player.onPick = true;
            }
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
