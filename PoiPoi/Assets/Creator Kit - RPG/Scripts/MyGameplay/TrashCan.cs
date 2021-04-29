using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;


namespace RPGM.Gameplay
{
    /// <summary>
    /// ���ݔ�
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrashCan : MonoBehaviour
    {
        public PointEffect pointEffect;

        GameModel model = Schedule.GetModel<GameModel>();

        public void OnTriggerEnter2D(Collider2D collider)
        {
            // �A�C�e���Ɠ���������
            var item = collider.GetComponent<DropItem>();
            if (item != null)
            {
                // �A�C�e���������ɓ��������͉��_�������Ȃ��悤��
                if (model.gameManager.GamePhase!= GameManager.GamePhaseType.Opening)
                {
                    // ���_����

                    // ��b�_*�򋗗����_��
                    float distance = item.GetFlyingDistance();
                    int score = (int)(item.data.score * distance);
                    // ���_
                    model.gameManager.AddScore(score);
                    // �L�^�X�V����
                    if (model.gameManager.UpdateRecord(distance))
                    {
                        // �L�^�X�V��������
                        MessageBar.Show($"New Record !!!!  Distance:{distance.ToString("0.00")}m");
                    }
                    // ���ʉ��Đ�
                    UserInterfaceAudio.OnCollect();
                    // �_���G�t�F�N�g�𐶐�
                    var eff = Instantiate(pointEffect, collider.transform.position, Quaternion.identity);
                    eff.gameObject.transform.SetParent(transform);
                    eff.textMeshPro.text = score.ToString();
                }

                // �A�C�e���͍폜
                Destroy(collider.gameObject);
            }
        }
    }
}