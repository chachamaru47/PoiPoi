using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// ���ݐ�����
    /// </summary>
    public class TrashGenerator : MonoBehaviour
    {
        [System.Serializable]
        public class LotTrashData
        {
            public TrashData trashData;
            public int lotWeight;
        }
        public GameObject trashCollection;
        public DropItem itemBase;
        public LotTrashData[] trashDatas;

        // Start is called before the first frame update
        void Start()
        {
            Generation(1000, new Rect(-20, -15, 40, 30), 123456789);
        }

        /// <summary>
        /// ���݃A�C�e���̐���
        /// </summary>
        /// <param name="num">������</param>
        /// <param name="rect">�����͈�</param>
        /// <param name="seed">�����V�[�h</param>
        private void Generation(int num, Rect rect, int seed)
        {
            // ���I�̏d�ݍ��v���Z�o
            int total_weight = trashDatas.Sum(v => v.lotWeight);
            if (total_weight <= 0) { return; }

            // ����������
            Random.InitState(seed);
            // ��A�N�e�B�u�Ő������ăp�����[�^��ݒ肵�Ă���A�N�e�B�u�ɂ���
            itemBase.gameObject.SetActive(false);

            for (int i = 0; i < num; i++)
            {
                // �������W�����߂�
                var pos = new Vector3(Random.Range(rect.x, rect.xMax), Random.Range(rect.y, rect.yMax), 0.0f);
                // ����
                var item = Instantiate(itemBase, pos, Quaternion.identity);
                item.gameObject.transform.SetParent(trashCollection.transform);

                // �ݒ肷��A�C�e���f�[�^�𒊑I
                int lot = Random.Range(0, total_weight);
                foreach (var data in trashDatas)
                {
                    if (lot < data.lotWeight)
                    {
                        item.data = data.trashData;
                        break;
                    }
                    lot -= data.lotWeight;
                }

                // �Ō�ɃA�N�e�B�u��
                item.gameObject.SetActive(true);
            }
        }
    }
}
