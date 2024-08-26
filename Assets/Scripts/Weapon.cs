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
        player = GetComponentInParent<Player>();
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Place();

                break;

            default:
                speed = 0.3f;
                break;
        }
    }

    private void Update()
    {
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

        // Test Code
        if (Input.GetButtonDown("Jump"))
            LevelUp(5, 1);
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

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is Infinite penet.
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Place();
        }
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
    }
}
