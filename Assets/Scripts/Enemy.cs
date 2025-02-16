using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // enemy movement speed
    public float detectionRange = 10f; // range at which enemy can detect player
    public float attackRange = 2f; // range at which enemy can attack player
    public float attackCooldown = 2f; // time between enemy attacks
    public int attackDamage = 10; // damage dealt by enemy attack

    private Transform player; // reference to player transform
    private bool playerInRange; // flag indicating if player is in detection range
    private bool playerInAttackRange; // flag indicating if player is in attack range
    private float attackTimer; // timer for enemy attack cooldown

    Vector3 moveDir = Vector3.zero;
    private float vertvelocity;
    public float gravity = 14.0f;
    public CharacterController controller;

    Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // find player game object and get transform
        playerInRange = false;
        playerInAttackRange = false;
        attackTimer = 0f;
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 0);
    }

    void Update()
    {
        //**************************************************gravity*********************************************
        moveDir.y = vertvelocity; //applies gravity
        controller.Move(moveDir * Time.deltaTime);

        if (controller.isGrounded)
        {
            vertvelocity = -gravity * Time.deltaTime;
        }

        else
        {
            vertvelocity -= gravity * Time.deltaTime;
        }

        //**************************************************gravity*********************************************
        // calculate distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // check if player is in detection range
        if (distanceToPlayer <= detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        // check if player is in attack range
        if (distanceToPlayer <= attackRange)
        {
            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
        }

        // if player is in attack range and attack timer is ready, attack player
        if (playerInAttackRange && attackTimer <= 0f)
        {
            /*player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);*/
            attackTimer = attackCooldown;
        }

        // update attack timer
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // if player is in detection range, move towards player
        if (playerInRange)
        {
            transform.LookAt(player.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            anim.SetInteger("Condition", 1);
        }

        else
        {
            anim.SetInteger("Condition", 0);
        }
    }
}
