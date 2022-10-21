using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isJumping;

    private Rigidbody2D rb;
    [SerializeField] Transform groundCheck;

    [SerializeField] private float checkRadius = 0.5f;

    [SerializeField] private float speed = 2f;

    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private LayerMask collisionMask; 

    // Jump System
    [SerializeField]
    private int maxJumps = 2;
    private int jumpsJeft;
    private bool inputReleasedButton;
    // End Jump System

    // Dash System
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashTime = 0.5f;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash = true;
    // End Dash System

    // Player moving above the plataform
    private Transform originalParent;
    // End

    // Player death
    [SerializeField] 
    private bool active = true;
    public BoxCollider2D coll;

    private Vector2 respawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        jumpsJeft = maxJumps;

        coll = GetComponent<BoxCollider2D>();

        originalParent = transform.parent;

        SetRespawnPoint(transform.position); // define o ponto de respawn do player
    }


    void Update()
    {
        if (!active) {
            return;
        }

        // Movimentos laterais do player
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Reseta o numero de pulos ao tocar o chão
        isJumping = Input.GetButtonDown("Jump");

        if (isGrounded() && rb.velocity.y < 0 ) {
            jumpsJeft = maxJumps;
        }
        // ======================== //

        // Toda vez que o botao de pulo e pressionado, diminui a quantidade de pulos 
        if (isJumping && jumpsJeft > 0 ){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsJeft -= 1 ;
        }
        // ======================== //
        

        // Quanto mais tempo for pressioando o botao de pulo, maior sera a força
        inputReleasedButton = Input.GetButtonUp("Jump");

        if (inputReleasedButton && rb.velocity.y >0) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        // ======================== //

        // Dash commands
        var dashInput = Input.GetButtonDown("Dash");
        if (dashInput && canDash) {
            isDashing = true;
            canDash = false;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dashDirection == Vector2.zero) {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }

        if (isDashing) {
            rb.velocity = dashDirection.normalized * dashSpeed;
            return;
        }

        if (isGrounded()) {
            canDash = true;
        }
        // End dash commands //

        /*if (horizontal != 0) {
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
        }*/
    }

    private IEnumerator StopDashing() {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    private bool isGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, collisionMask);
    }

    // For collisions, move with platforms
    public void SetParent (Transform newParent) {
        originalParent = transform.parent;
        transform.parent = newParent;
    }

    public void ResetParent () {
        transform.parent = originalParent;
    }
    // ============================= //


    // Dying methods
    private void MiniJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce /2);
    }

    public void Die() {
        active = false;
        GetComponent<BoxCollider2D>().enabled = false;
        MiniJump();
        StartCoroutine(Respawn());
    }

    public void SetRespawnPoint(Vector2 position) {
        respawnPoint = position;
    }

    private IEnumerator Respawn () {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPoint;
        active = true;
        coll.enabled = true;
        MiniJump();
    }

    // End Dying methods //
}
