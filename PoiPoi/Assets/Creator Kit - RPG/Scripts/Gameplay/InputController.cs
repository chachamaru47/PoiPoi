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
        public float stepSize = 0.1f;
        public float accelMax = 2f;
        public float deadZone = 0.2f;
        GameModel model = Schedule.GetModel<GameModel>();

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
            // �A�v���I��
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey("joystick button 4") && Input.GetKey("joystick button 5") && Input.GetKey("joystick button 6") && Input.GetKey("joystick button 7"))
            {
                Quit();
            }

            // ���K���[�h�؂�ւ�
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown("joystick button 3"))
            {
                model.gameManager.Practice = !model.gameManager.Practice;
            }

            // �Q�[�����ȊO�͈ȍ~�̑���͎󂯕t�����ɋA��
            if (model.gameManager.GamePhase != GameManager.GamePhaseType.InGame)
            {
                model.player.nextMoveCommand = Vector3.zero;
                return;
            }

            // ��Ԃ��Ƃ̑���
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
        /// �_�C�A���O����
        /// </summary>
        void DialogControl()
        {
            model.player.nextMoveCommand = Vector3.zero;

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
        /// �L�����N�^�[����
        /// </summary>
        void CharacterControl()
        {
            model.player.nextMoveCommand = Vector3.up * Input.GetAxis("Vertical");
            model.player.nextMoveCommand += Vector3.right * Input.GetAxis("Horizontal");
            if (model.player.nextMoveCommand.magnitude > deadZone)
            {
                float dash = Input.GetAxis("Dash");
                float accel = 1.0f + accelMax * ((dash > 0.01f) ? dash : Input.GetButton("Dash") ? 1.0f : 0.0f);
                model.player.nextMoveCommand *= accel * stepSize;
            }
            else
            {
                model.player.nextMoveCommand = Vector3.zero;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                model.player.onFire = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                model.player.releaseFire = true;
            }
            model.player.moveBrake = (Input.GetAxis("Dash") < -0.01f);
        }

        /// <summary>
        /// �A�v���I��
        /// </summary>
        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }
}
