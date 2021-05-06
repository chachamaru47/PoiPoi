using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// ごみ箱
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrashCan : MonoBehaviour
    {
        public PointEffect pointEffect;

        GameModel model = Schedule.GetModel<GameModel>();

        public void OnTriggerEnter2D(Collider2D collider)
        {
            // アイテムと当たった時
            var item = collider.GetComponent<DropItem>();
            if (item != null)
            {
                // アイテム生成時に当たったは加点処理しないように
                if (model.gameManager.GamePhase!= GameManager.GamePhaseType.Opening)
                {
                    // 加点処理

                    // 基礎点*飛距離が点数
                    float distance = item.GetFlyingDistance();
                    int score = (int)(item.data.score * distance);
                    // 自分の投げたアイテムのみ加点処理
                    if (item.ownerId == model.gameManager.PlayerId)
                    {
                        // 加点
                        model.gameManager.AddScore(score);
                        // 記録更新判定
                        if (model.gameManager.UpdateRecord(distance))
                        {
                            // 記録更新があった
                            MessageBar.Show($"New Record !!!!  Distance:{distance.ToString("0.00")}m");
                        }
                    }
                    // 効果音再生
                    UserInterfaceAudio.OnCollect();
                    // 点数エフェクトを生成
                    var eff = Instantiate(pointEffect, collider.transform.position, Quaternion.identity);
                    eff.gameObject.transform.SetParent(transform);
                    eff.textMeshPro.text = score.ToString();
                    eff.textMeshPro.color = CharacterController2D.GetPlayerTextColor(item.ownerId);
                }

                // アイテムは削除
                model.trashGenerator.DestroyItem(item.id);
            }
        }
    }
}