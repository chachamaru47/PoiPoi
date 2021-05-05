using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// ごみアイテム
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class DropItem : MonoBehaviour
    {
        public int id;
        public int ownerId = -1;
        public TrashData data;

        public SpriteRenderer body;
        public SpriteRenderer shadow;

        GameModel model = Schedule.GetModel<GameModel>();

        private GameObject oldHome;
        private float aboveGround = 0.0f;
        private Vector3? throwStartPos;
        private CircleCollider2D collider2d;
        private Rigidbody2D rigidbody2d;

        void Reset()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        void OnEnable()
        {
            collider2d = GetComponent<CircleCollider2D>();
            rigidbody2d = GetComponent<Rigidbody2D>();

            // アイテムデータから画像を設定
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

            rigidbody2d.mass = data.mass;

            // 現在の親オブジェクトを故郷に設定
            oldHome = transform.parent?.gameObject;
        }

        /// <summary>
        /// 拾われる
        /// </summary>
        public void PickItem(int playerId)
        {
            ownerId = playerId;

            // 拾われている間当たり判定は消しておく
            collider2d.enabled = false;
            // 拾われている間影も消しておく
            shadow.gameObject.SetActive(false);
            // キャラクターより手前に
            body.sortingOrder = 1;
            // 物理挙動オフ
            rigidbody2d.simulated = false;
            // 投射開始位置をクリア
            throwStartPos = null;

            UserInterfaceAudio.OnPick();
        }

        /// <summary>
        /// 投げられる
        /// </summary>
        /// <param name="force">投げられる力</param>
        /// <param name="above_ground">開始時の地面からの高さ</param>
        public void ThrowItem(Vector2 force, float above_ground)
        {
            // 地面からの高さを設定
            aboveGround = above_ground;
            // ワールド座標は高さを考慮しない位置にしたいので地面からの高さを引いた座標に設定
            transform.position = new Vector3(transform.position.x, transform.position.y - above_ground, transform.position.z);
            // 投射開始位置を設定
            throwStartPos = transform.position;
            // 飛ばす処理開始
            StartCoroutine(FlyingCoroutine(force));
        }

        /// <summary>
        /// 飛距離を取得する
        /// </summary>
        /// <returns>飛距離</returns>
        public float GetFlyingDistance()
        {
            if (throwStartPos != null)
            {
                return Mathf.Abs(Vector3.Distance(transform.position, (Vector3)throwStartPos));
            }
            return 0.0f;
        }

        /// <summary>
        /// 拾われている状態か
        /// </summary>
        /// <returns>拾われている状態なら真</returns>
        public bool IsPicked()
        {
            // 当たり判定が無効になっている間は拾われている状態とみなす
            return !collider2d.enabled;
        }

        /// <summary>
        /// 飛ばす処理のコルーチン
        /// </summary>
        /// <param name="force">飛ばす力</param>
        /// <returns>IEnumerator</returns>
        private System.Collections.IEnumerator FlyingCoroutine(Vector2 force)
        {
            // 故郷オブジェクトの子オブジェクトに戻す
            transform.SetParent(oldHome.transform);
            // 影をオン
            shadow.gameObject.SetActive(true);
            // 元の描画順に戻す
            body.sortingOrder = 0;
            // 絵を飛ぶ方向に向ける
            body.flipX = (force.x > 0.0f);

            // 物理挙動開始して力を加える
            rigidbody2d.simulated = true;
            rigidbody2d.AddForce(force, ForceMode2D.Impulse);

            // 空中にいる間のループ
            float flying_time = 0.0f;
            float start_ag = aboveGround;
            bool camera_focus = false;
            bool camera_out_focus = false;
            while (aboveGround > 0.0f)
            {
                // 本体のローカル座標を地面からの高さ分上げる
                body.transform.localPosition = new Vector3(0.0f, aboveGround, 0.0f);
                // 適当にぐーるぐる
                body.transform.Rotate(new Vector3(0.0f, 0.0f, ((force.x > 0.0f) ? -data.rotateSpeed : data.rotateSpeed) * Time.deltaTime));

                // 高さの更新、計算式は暫定
                flying_time += Time.deltaTime;
                if (flying_time < data.flyingTime)
                {
                    // とりあえず1秒間はsinカーブを描いて上下
                    aboveGround = start_ag + Mathf.Sin(flying_time / data.flyingTime * Mathf.PI) * data.flyingHeight;
                }
                else
                {
                    // 以降は一定速度で落下
                    aboveGround -= 5.0f * Time.deltaTime;
                }

                // 飛距離が大きくなったらこのアイテムにフォーカスを移してカメラを引く
                if(!camera_focus && GetFlyingDistance() > 5.0f)
                {
                    camera_focus = true;
                    model.cameraController.SetFocus(transform);
                }
                // 飛距離が大きくなりすぎた時このアイテムをフォーカスしたままだったらフォーカスをお返しする
                if (!camera_out_focus && GetFlyingDistance() > 30.0f)
                {
                    camera_out_focus = true;
                    if (model.cameraController.IsTargetFocus(transform))
                    {
                        model.cameraController.ResetFocus();
                    }
                }

                yield return null;
            }

            // 地面に着いた

            // 高さやら力やらをクリア
            aboveGround = 0.0f;
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            rigidbody2d.velocity = Vector2.zero;
            // 当たり判定をオン
            collider2d.enabled = true;
            // カメラがこのアイテムをフォーカスしたままだったらフォーカスをお返しする
            if(model.cameraController.IsTargetFocus(transform))
            {
                model.cameraController.ResetFocus();
            }

            yield break;
        }
    }
}
