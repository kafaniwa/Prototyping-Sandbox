using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmovement : MonoBehaviour
{
    public ParticleSystem dust;
    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Animator anim;

    Vector3 moveDir = Vector3.zero;

    Rigidbody player;


    public float speed = 6f;


    public float gravity = 14.0f;

    private float jumpForce = 20.0f; //how much force you want when jumping

    private float vertvelocity;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 0);
    }

    void CreateDust()
    {
        dust.Play();
    }


    // Update is called once per frame
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
        moveDir.y = vertvelocity; //applies gravity
        controller.Move(moveDir * Time.deltaTime);

        Debug.Log(controller.isGrounded);

        if (controller.isGrounded)
        {
            anim.SetBool("isGrounded", true);
            vertvelocity = -gravity * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("isGrounded", false);
                vertvelocity = jumpForce;
            }
        }
        
        else
        {
            anim.SetBool("isGrounded", false);
            vertvelocity -= gravity * Time.deltaTime;
        }
        

        


        //animation
        if (!controller.isGrounded && direction.magnitude != 0f)
        {
            anim.SetInteger("Condition", 1);
            CreateDust();
        }

        else if (controller.isGrounded && direction.magnitude == 0f)
        {
            anim.SetInteger("Condition", 0);
        }
        
        else if (!controller.isGrounded && anim.GetBool("isGrounded") == false)
        {
            anim.SetInteger("Condition", 2);
        }
        

    }
}
