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

        private Dictionary<int, DropItem> itemList;

        // Start is called before the first frame update
        void Start()
        {
            itemList = new Dictionary<int, DropItem>();
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
                // 設定するアイテムデータを抽選
                var data = LotItemData(trashDatas, total_weight);
                if (data == null) { break; }

                // 生成座標を決める
                var pos = new Vector3(Random.Range(rect.x, rect.xMax), Random.Range(rect.y, rect.yMax), 0.0f);
                // 生成
                var item = Instantiate(itemBase, pos, Quaternion.identity);
                item.gameObject.transform.SetParent(trashCollection.transform);
                item.id = i + 1;
                item.data = data;

                // 最後にアクティブ化
                item.gameObject.SetActive(true);

                itemList.Add(item.id, item);
            }
        }

        /// <summary>
        /// ごみデータ抽選
        /// </summary>
        /// <param name="datas">ごみデータリスト</param>
        /// <param name="totalWeight">重み合計</param>
        /// <returns></returns>
        private TrashData LotItemData(LotTrashData[] datas, int totalWeight)
        {
            if (totalWeight <= 0) { return null; }

            int lot = Random.Range(0, totalWeight);
            foreach (var data in datas)
            {
                if (lot < data.lotWeight)
                {
                    return data.trashData;
                }
                lot -= data.lotWeight;
            }
            return null;
        }

        /// <summary>
        /// 指定IDのアイテムを取得する
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DropItem GetItem(int itemId)
        {
            return itemList.ContainsKey(itemId) ? itemList[itemId] : null;
        }

        /// <summary>
        /// 指定IDのアイテムを破棄する
        /// </summary>
        /// <param name="itemId">アイテムID</param>
        public void DestroyItem(int itemId)
        {
            if (itemList.ContainsKey(itemId))
            {
                Destroy(itemList[itemId].gameObject);
                itemList.Remove(itemId);
            }
        }
    }
}
