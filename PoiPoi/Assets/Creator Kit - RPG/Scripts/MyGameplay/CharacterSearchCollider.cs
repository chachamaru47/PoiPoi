using System.Collections;
using System.Collections.Generic;
using RPGM.Core;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// キャラクターのアイテムサーチ用コライダー
    /// </summary>
    public class CharacterSearchCollider : MonoBehaviour
    {
        GameModel model = Schedule.GetModel<GameModel>();

        public void OnTriggerEnter2D(Collider2D collider)
        {
            // アイテムとヒットしたら拾う
            var item = collider.GetComponent<DropItem>();
            if (item != null)
            {
                model.player.PicItem(item);
            }
        }
    }
}
