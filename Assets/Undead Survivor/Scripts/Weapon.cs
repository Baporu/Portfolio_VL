using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player  player;

    private float   timer;

    public int      id;
    public int      prefabId;
    public float    damage;
    public int      count;
    public float    speed;


    private void Awake()
    {
        player = GameManager.Instance.player;
    }

    public void Init(ItemData data)
    {
        // ������Ʈ �⺻ ����
        name = "Weapon " + data.itemId;

        transform.parent        = player.transform;
        transform.localPosition = Vector3.zero;

        // Property ����
        id      = data.itemId;
        damage  = data.baseDamage * Character.Damage;
        count   = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.Instance.poolManager.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.Instance.poolManager.prefabs[index])
            {
                prefabId = index;

                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Place();

                break;

            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // �� ����
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;

            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }

                break;
        }
    }

    private void Place()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            // �̹� ������ ���¿��� �߰��� ������ ���, ������ ������Ʈ���� �״�� ���
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            // �����ִ� ������Ʈ�� ���� ��� ���� ����
            else
            {
                bullet = GameManager.Instance.poolManager.Get(prefabId).transform;
                // ������ ������Ʈ���� �� ��ũ��Ʈ�� ���� ���� ��� ������Ʈ�� �ڽ����� �Ҵ� (���� ������Ʈ���� �̹� �Ǿ������� ����)
                bullet.parent = transform;
            }

            // ��ġ�� ȸ���� ���� �÷��̾��� ��ġ�� �°� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ���� ������ �°� ����ϰ� ��ġ ����(Local �������� ȸ���ϰ� ���� �ø��� �߽��� ��� ��ġ�� �� ����)
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.4f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinite penet.

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
        {
            Place();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Fire()
    {
        // ���� ����� Ÿ���� ������ return
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos   = player.scanner.nearestTarget.position;
        Vector3 dir         = targetPos - transform.position;
        dir                 = dir.normalized;

        Transform bullet    = GameManager.Instance.poolManager.Get(prefabId).transform;
        bullet.position     = transform.position;
        // Y���� �������� dir��ŭ ȸ��
        bullet.rotation     = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
