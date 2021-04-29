using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.Gameplay
{
    [CreateAssetMenu(menuName = "MyScriptable/Create TrashData")]
    public class TrashData : ScriptableObject
    {
        public Sprite sprite;
        public RuntimeAnimatorController animatorController;
        public int score;
    }
}
