using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// ごみ生成器
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
        /// ごみアイテムの生成
        /// </summary>
        /// <param name="num">生成数</param>
        /// <param name="rect">生成範囲</param>
        /// <param name="seed">乱数シード</param>
        private void Generation(int num, Rect rect, int seed)
        {
            // 抽選の重み合計を算出
            int total_weight = trashDatas.Sum(v => v.lotWeight);
            if (total_weight <= 0) { return; }

            // 乱数初期化
            Random.InitState(seed);
            // 非アクティブで生成してパラメータを設定してからアクティブにする
            itemBase.gameObject.SetActive(false);

            for (int i = 0; i < num; i++)
            {
                // 生成座標を決める
                var pos = new Vector3(Random.Range(rect.x, rect.xMax), Random.Range(rect.y, rect.yMax), 0.0f);
                // 生成
                var item = Instantiate(itemBase, pos, Quaternion.identity);
                item.gameObject.transform.SetParent(trashCollection.transform);

                // 設定するアイテムデータを抽選
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

                // 最後にアクティブ化
                item.gameObject.SetActive(true);
            }
        }
    }
}
