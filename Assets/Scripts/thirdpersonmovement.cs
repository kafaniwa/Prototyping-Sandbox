using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Animator anim;

    Vector3 moveDir = Vector3.zero;

    Rigidbody player;


    public float speed = 6f;

    public float speedb = 0f;

    public float boost = 3f;

    public float gravity = 14.0f;

    private float jumpForce = 20.0f; //how much force you want when jumping

    private float vertvelocity;

    public float height;

    float heightAboveGround;

    RaycastHit hit;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 0);
        controller.minMoveDistance = 0;
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

        if (Input.GetButtonDown("Run"))
        {
            speedb = speed;
            speed = speed * boost;
        }

        if (Input.GetButtonUp("Run"))
        {
            speed = speedb;
            speedb = 0f;
        }

        //**************************************************raycast to floor*********************************************
        Ray ray = new Ray(transform.position, -Vector3.up);

        Debug.DrawRay(transform.position, Vector3.down * height, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "ground")
            {
                heightAboveGround = hit.distance - 1.027f;

                Debug.Log(heightAboveGround);
            }
        }

        //**************************************************gravity*********************************************
        moveDir.y = vertvelocity; //applies gravity
        controller.Move(moveDir * Time.deltaTime);

        //Debug.Log(controller.isGrounded);

        if (controller.isGrounded)
        {
            vertvelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                vertvelocity = jumpForce;
            }
        }
        
        else
        {
            vertvelocity -= gravity * Time.deltaTime;
        }
        

        


        //animation
        if (direction.magnitude >= 0.1f && controller.isGrounded)
        {
            anim.SetInteger("Condition", 1);
        }

        else if (direction.magnitude == 0f)
        {
            anim.SetInteger("Condition", 0);
        }
        
        if (!controller.isGrounded && vertvelocity > 0f)
        {
            anim.SetInteger("Condition", 3);
        }

        if (!controller.isGrounded && vertvelocity < 0f && heightAboveGround > 5f)
        {
            anim.SetInteger("Condition", 4);
        }

        if (Input.GetButton("Run"))
        {
            anim.SetInteger("Condition", 2);

            if (!controller.isGrounded && vertvelocity > 0f)
            {
                anim.SetInteger("Condition", 3);
            }

            if (!controller.isGrounded && vertvelocity < 0f && heightAboveGround > 5f)
        {
            anim.SetInteger("Condition", 4);
        }
        }
    }
}
