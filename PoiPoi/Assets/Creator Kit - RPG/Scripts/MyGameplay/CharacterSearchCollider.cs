using System.Collections;
using System.Collections.Generic;
using RPGM.Core;
using UnityEngine;

namespace RPGM.Gameplay
{
    public class CharacterSearchCollider : MonoBehaviour
    {
        GameModel model = Schedule.GetModel<GameModel>();

        public void OnTriggerEnter2D(Collider2D collider)
        {
            var item = collider.GetComponent<DropItem>();
            if (item != null)
            {
                model.player.PicItem(item);
            }
        }
    }
}
