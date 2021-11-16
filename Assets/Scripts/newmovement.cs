using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newmovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 22f;

    float gravity = 50.0f;

    private float jumpForce = 42.0f; //how much force you want when jumping

    private float vertvelocity;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 0);
    }


    void Update()
    {
        //*************************************************movement*********************************************
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

       

        //**************************************************gravity*********************************************

        if (controller.isGrounded)
        {
            vertvelocity = -gravity * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                vertvelocity = jumpForce;
            }
        }
        else
        {
            vertvelocity -= gravity * Time.deltaTime;
        }

        direction.y = vertvelocity; //applies gravity
        controller.Move(direction * Time.deltaTime);


        //*************************************************animation*******************************************
        if (vertical == 1 || vertical == -1)
        {
            anim.SetInteger("Condition", 1);

            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetInteger("Condition", 2);
                speed = 100;
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                speed = 22;
            }
        }

        if (vertical == 0 && horizontal == 0)
        {
            anim.SetInteger("Condition", 0);
        }

        if (horizontal >= 0.1f || horizontal <= -0.1f)
        {
            anim.SetInteger("Condition", 1);
        }
    }
}