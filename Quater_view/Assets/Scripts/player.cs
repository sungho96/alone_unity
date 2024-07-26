using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isDodge;

    bool isJump;


    Vector3 moveVec;

    Animator anim;

    Rigidbody rigid;

    //초기화 함수
    void Awake()
    {
        rigid= GetComponent<Rigidbody>();
        anim= GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();

        
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButton("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime;

        anim.SetBool("isRun",moveVec != Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJump) {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            //즉발적으로 힘을줄수 있는 모드 Force Moded.Impulse
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump =true;

            
        }
    }

    void Dodge()
    {
        if (jDown && !isJump) {
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge =true;

            Invoke("DodgeOut",0.4f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
             anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}


