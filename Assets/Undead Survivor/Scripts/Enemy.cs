using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer      spriter;
    private Rigidbody2D         rigid;
    private Collider2D          coll;
    private Animator            anim;
    private WaitForFixedUpdate  wait;

    [SerializeField]
    private Rigidbody2D     target;

    public RuntimeAnimatorController[] animCon;

    public bool     isLive;
    public float    speed;
    public float    health;
    public float    maxHealth;


    private void OnEnable()
    {
        target          = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive          = true;
        coll.enabled    = true;
        rigid.simulated = true;

        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

        health = maxHealth;
    }

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid   = GetComponent<Rigidbody2D>();
        coll    = GetComponent<Collider2D>();
        anim    = GetComponent<Animator>();
        wait    = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec  = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];

        speed       = data.speed;
        maxHealth   = data.health;
        health      = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������� �� �Ѿ˿� ���� ��찡 �ƴϸ� return
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive          = false;
            coll.enabled    = false;
            rigid.simulated = false;

            // ���� ���Ͱ� �ٸ� ���͸� ������ �� �ϰ� Order in Layer�� ������
            spriter.sortingOrder = 1;

            anim.SetBool("Dead", true);

            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        // ���� �ϳ��� ���� �����Ӹ�ŭ ������
        yield return wait;

        Vector3 playerPos   = GameManager.Instance.player.transform.position;
        Vector3 dirVec      = transform.position - playerPos;

        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    // �ִϸ��̼ǿ��� ȣ��
    private void SetDead()
    {
        gameObject.SetActive(false);
    }

}
