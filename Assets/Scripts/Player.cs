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

    // 물리 연산 프레임 이후 작동
    private void FixedUpdate()
    {
        // deltaTime 말고 fixedDeltaTime을 써줘야하는 것 유의
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    // Update 이후에 작동
    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;
    }

    // Input System 함수
    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    public Vector2 GetPlayerDir()
    {
        return inputVec;
    }
}
