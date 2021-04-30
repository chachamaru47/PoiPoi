using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGM.Gameplay
{
    public class PointEffect : MonoBehaviour
    {
        public TMPro.TextMeshPro textMeshPro;

        private float time = 0.0f;

        private void Awake()
        {
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = 10;
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (time < 1.0f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f * Time.deltaTime, transform.position.z);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
