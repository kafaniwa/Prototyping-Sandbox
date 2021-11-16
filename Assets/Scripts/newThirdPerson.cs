using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newThirdPerson : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Animator anim;

    Vector3 moveDir = Vector3.zero;

    Rigidbody player;


    public float speed = 6f;


    float gravity = 14.0f;

    private float jumpForce = 20.0f; //how much force you want when jumping

    private float vertvelocity;

    State playerState;

    enum State
    {
        IDLE,
        MOVING,
        JUMPING,
        LEDGEGRAB
    };


    void Start()
    {
        anim = GetComponent<Animator>();
        playerState = State.IDLE;
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //*************************************************movement*********************************************
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        stateMachine(horizontal, vertical);

        Debug.Log(controller.isGrounded);
        //**************************************************gravity*********************************************

        if (controller.isGrounded)
        {
            vertvelocity = -gravity * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                playerState = State.JUMPING;
            }
        }
        else
        {
            vertvelocity -= gravity * Time.deltaTime;
            anim.SetInteger("Condition", 2);
        }

        moveDir.y = vertvelocity; //applies gravity
        controller.Move(moveDir * Time.deltaTime);


        //inbuilt state machine

        //animation
        if (controller.isGrounded && direction.magnitude != 0f)
        {
            anim.SetInteger("Condition", 1);
        }

        else if (controller.isGrounded && direction.magnitude == 0f)
        {
            anim.SetInteger("Condition", 0);
        }

        else if (controller.isGrounded && horizontal != 0)
        {
            anim.SetInteger("Condition", 1);
        }

        else if (!controller.isGrounded)
        {
            anim.SetInteger("Condition", 2);
        }


    }

    public virtual void charUpdate()
    {

    }

    void stateMachine(float horizontal, float vertical)
    {
        bool isMovingOnGround = controller.isGrounded && (vertical != 0 || horizontal != 0);
        switch (playerState)
        {
            case State.IDLE:
                anim.SetInteger("Condition", 0);
                if (isMovingOnGround)
                {
                    playerState = State.MOVING;
                }
                break;

            case State.MOVING:
                if (isMovingOnGround)
                {
                    anim.SetInteger("Condition", 1);
                }
                else
                {
                    anim.SetInteger("Condition", 2);
                }
                break;

            case State.JUMPING:
                anim.SetInteger("Condition", 2);
                vertvelocity = jumpForce;

                if (controller.isGrounded)
                {
                    anim.SetInteger("Condition", 1);
                }
                break;

            case State.LEDGEGRAB:

                break;
        }
    }
}
