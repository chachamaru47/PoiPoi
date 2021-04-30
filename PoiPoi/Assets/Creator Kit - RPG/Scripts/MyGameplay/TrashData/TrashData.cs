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
        public float mass = 1.0f;
        public float flyingTime = 1.0f;
        public float flyingHeight = 2.0f;
        public float rotateSpeed = 720.0f;
    }
}
