using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// ���݃A�C�e��
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class DropItem : MonoBehaviour
    {
        public TrashData data;

        public SpriteRenderer body;
        public SpriteRenderer shadow;

        GameModel model = Schedule.GetModel<GameModel>();

        private GameObject oldHome;
        private float aboveGround = 0.0f;
        private Vector3? throwStartPos;

        void Reset()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        void OnEnable()
        {
            // �A�C�e���f�[�^����摜��ݒ�
            if (body != null)
            {
                body.sprite = data.sprite;
                var animator = body.GetComponent<Animator>();
                if (data.animatorController != null)
                {
                    animator.runtimeAnimatorController = data.animatorController;
                    animator.enabled = true;
                }
                else
                {
                    animator.enabled = false;
                }
            }

            // ���݂̐e�I�u�W�F�N�g���̋��ɐݒ�
            oldHome = transform.parent?.gameObject;
        }

        /// <summary>
        /// �E����
        /// </summary>
        public void PicItem()
        {
            // �E���Ă���ԓ����蔻��͏����Ă���
            GetComponent<CircleCollider2D>().enabled = false;
            // �E���Ă���ԉe�������Ă���
            shadow.gameObject.SetActive(false);
            // �L�����N�^�[����O��
            body.sortingOrder = 1;
            // ���������I�t
            GetComponent<Rigidbody2D>().simulated = false;
            // ���ˊJ�n�ʒu���N���A
            throwStartPos = null;

            UserInterfaceAudio.OnPic();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="force">���������</param>
        /// <param name="above_ground">�J�n���̒n�ʂ���̍���</param>
        public void ThrowItem(Vector2 force, float above_ground)
        {
            // �n�ʂ���̍�����ݒ�
            aboveGround = above_ground;
            // ���[���h���W�͍������l�����Ȃ��ʒu�ɂ������̂Œn�ʂ���̍��������������W�ɐݒ�
            transform.position = new Vector3(transform.position.x, transform.position.y - above_ground, transform.position.z);
            // ���ˊJ�n�ʒu��ݒ�
            throwStartPos = transform.position;
            // ��΂������J�n
            StartCoroutine(FlyingCoroutine(force));
        }

        /// <summary>
        /// �򋗗����擾����
        /// </summary>
        /// <returns>�򋗗�</returns>
        public float GetFlyingDistance()
        {
            if (throwStartPos != null)
            {
                return Mathf.Abs(Vector3.Distance(transform.position, (Vector3)throwStartPos));
            }
            return 0.0f;
        }

        /// <summary>
        /// ��΂������̃R���[�`��
        /// </summary>
        /// <param name="force">��΂���</param>
        /// <returns>IEnumerator</returns>
        private System.Collections.IEnumerator FlyingCoroutine(Vector2 force)
        {
            // �̋��I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɖ߂�
            transform.SetParent(oldHome.transform);
            // �e���I��
            shadow.gameObject.SetActive(true);
            // ���̕`�揇�ɖ߂�
            body.sortingOrder = 0;

            // ���������J�n���ė͂�������
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.simulated = true;
            rigidbody.AddForce(force, ForceMode2D.Impulse);

            // �󒆂ɂ���Ԃ̃��[�v
            float flying_time = 0.0f;
            float start_ag = aboveGround;
            bool camera_focus = false;
            while (aboveGround > 0.0f)
            {
                // �{�̂̃��[�J�����W��n�ʂ���̍������グ��
                body.transform.localPosition = new Vector3(0.0f, aboveGround, 0.0f);
                // �K���ɂ��[�邮��
                body.transform.Rotate(new Vector3(0.0f, 0.0f, 720.0f * Time.deltaTime));

                // �����̍X�V�A�v�Z���͎b��
                flying_time += Time.deltaTime;
                if (flying_time < 1.0f)
                {
                    // �Ƃ肠����1�b�Ԃ�sin�J�[�u��`���ď㉺
                    aboveGround = start_ag + Mathf.Sin(flying_time * Mathf.PI) * 2.0f;
                }
                else
                {
                    // �ȍ~�͈�葬�x�ŗ���
                    aboveGround -= 5.0f * Time.deltaTime;
                }

                // �򋗗����傫���Ȃ����炱�̃A�C�e���Ƀt�H�[�J�X���ڂ��ăJ����������
                if(!camera_focus && GetFlyingDistance() > 5.0f)
                {
                    camera_focus = true;
                    model.cameraController.SetFocus(transform);
                }

                yield return null;
            }

            // �n�ʂɒ�����

            // �������͂����N���A
            aboveGround = 0.0f;
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            rigidbody.velocity = Vector2.zero;
            // �����蔻����I��
            GetComponent<CircleCollider2D>().enabled = true;
            // �J�����̃t�H�[�J�X�����̃A�C�e���Ɉڂ����܂܂�������t�H�[�J�X�����Ԃ�����
            if(model.cameraController.IsTargetFocus(transform))
            {
                model.cameraController.ResetFocus();
            }

            yield break;
        }
    }
}
