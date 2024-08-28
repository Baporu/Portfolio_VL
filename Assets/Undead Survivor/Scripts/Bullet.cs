using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float    damage;
    public int      penet;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int penet, Vector3 dir)
    {
        this.damage = damage;
        this.penet = penet;

        if (penet > -1)
        {
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penet == -1)
            return;

        penet--;

        if (penet == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

}
