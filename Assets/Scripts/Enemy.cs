using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriter;
    private Rigidbody2D     rigid;
    private Animator        anim;

    [SerializeField]
    private Rigidbody2D     target;

    public RuntimeAnimatorController[] animCon;

    public bool     isLive;
    public float    speed;
    public float    health;
    public float    maxHealth;


    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        health = maxHealth;
        isLive = true;
    }

    private void Awake()
    {
        rigid   = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim    = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isLive)
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

}
