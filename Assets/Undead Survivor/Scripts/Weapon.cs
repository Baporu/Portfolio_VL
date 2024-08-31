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
        // 오브젝트 기본 설정
        name = "Weapon " + data.itemId;

        transform.parent        = player.transform;
        transform.localPosition = Vector3.zero;

        // Property 설정
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

        // 손 설정
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

            // 이미 생성된 상태에서 추가로 생성할 경우, 기존의 오브젝트들을 그대로 사용
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            // 남아있는 오브젝트가 없을 경우 새로 생성
            else
            {
                bullet = GameManager.Instance.poolManager.Get(prefabId).transform;
                // 생성한 오브젝트들을 이 스크립트를 가진 무기 담당 오브젝트의 자식으로 할당 (기존 오브젝트들은 이미 되어있으니 생략)
                bullet.parent = transform;
            }

            // 위치와 회전을 현재 플레이어의 위치에 맞게 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 각을 개수에 맞게 등분하고 위치 배정(Local 시점에서 회전하고 위로 올리면 중심을 잡고 배치할 수 있음)
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
        // 가장 가까운 타겟이 없으면 return
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos   = player.scanner.nearestTarget.position;
        Vector3 dir         = targetPos - transform.position;
        dir                 = dir.normalized;

        Transform bullet    = GameManager.Instance.poolManager.Get(prefabId).transform;
        bullet.position     = transform.position;
        // Y축을 기준으로 dir만큼 회전
        bullet.rotation     = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
