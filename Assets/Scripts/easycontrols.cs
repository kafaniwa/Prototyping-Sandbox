using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easycontrols : MonoBehaviour
{
    float speed = 6;
    float rotspeed = 120;
    float gravity = 8;
    float rot = 0f;

    Vector3 moveDir = Vector3.zero;

    CharacterController controller;
    Animator anim;

    Rigidbody player;
    private float jumpForce = 100f; //how much force you want when jumping
    private bool onGround; //allows the functions to determine whether player is on the ground or not

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        player = GetComponent<Rigidbody>();
        onGround = true;

        anim.SetInteger("Condition", 0);
    }

    // Update is called once per frame
    void Update()
    {
        rot += Input.GetAxis("Horizontal") * rotspeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);

        void OnCollisionEnter(Collision other)
        {
            //checks if collider is tagged "ground"
            if (other.gameObject.CompareTag("ground"))
            {
                //if the collider is tagged "ground", sets onGround boolean to true
                onGround = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log("Jump pressed");
            //adds force to player on the y axis by using the flaot set for the variable jumpForce. Causes the player to jump
            player.velocity = new Vector3(0f, jumpForce, 0f);
            //says the player is no longer on the ground
            onGround = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("Condition", 1);
            moveDir = new Vector3(0, 0, 1);
            moveDir *= speed;
            moveDir = transform.TransformDirection(moveDir);
        }

       
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("Condition", 0);
            moveDir = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetInteger("Condition", 1);
            moveDir = new Vector3(0, 0, -1);
            moveDir *= speed;
            moveDir = transform.TransformDirection(moveDir);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetInteger("Condition", 0);
            moveDir = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.J))
        {
            anim.SetInteger("Condition", 2);
            
           //moveDir *= speed * 2;
        }

        
        if (Input.GetKeyUp(KeyCode.J) && Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("Condition", 2);
            moveDir *= speed * 2;
        }
        
        else if(Input.GetKeyUp(KeyCode.J))
        {
            anim.SetInteger("Condition", 0);
        }
        
        

    }
}
