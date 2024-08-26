using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D     rigid;
    private SpriteRenderer  spriter;
    private Animator        anim;

    private Vector2 inputVec;

    public Scanner scanner;
    
    public float speed = 1.0f;

    private void Awake()
    {
        rigid   = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim    = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    // ���� ���� ������ ���� �۵�
    private void FixedUpdate()
    {
        // deltaTime ���� fixedDeltaTime�� ������ϴ� �� ����
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    // Update ���Ŀ� �۵�
    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;
    }

    // Input System �Լ�
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    public Vector2 GetPlayerDir()
    {
        return inputVec;
    }
}
