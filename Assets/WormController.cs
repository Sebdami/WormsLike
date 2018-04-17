using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour {
    CharacterData character;
    Rigidbody rb;
    [SerializeField]
    float moveSpeed = 100.0f;
    [SerializeField]
    float jumpForce = 4.0f;

    [SerializeField]
    float maxVelocityMagnitude = 10.0f;

    [SerializeField]
    float jumpCooldown = 1.0f;

    float jumpTimer = 0.0f;

    Vector3 jumpingForwardConstantForce = Vector3.forward* 1.5f;

    bool isJumping = false;
    bool isGrounded = false;

	void Start () {
        character = GetComponent<CharacterData>();
        rb = GetComponent<Rigidbody>();
	}
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + transform.forward * 0.5f, transform.position + transform.forward * 0.5f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position - transform.forward * 0.5f, transform.position - transform.forward * 0.5f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.6f);
    }

    private void Update()
    {
        // Handle jump cooldown
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpCooldown)
            {
                jumpTimer = 0.0f;
                isJumping = false;
            }
        }
    }

    void FixedUpdate () {
        isGrounded = Physics.Raycast(transform.position + transform.forward*0.5f, Vector3.down, 1.1f, ~LayerMask.GetMask("Worm")) || Physics.Raycast(transform.position - transform.forward * 0.5f, Vector3.down, 1.1f, ~LayerMask.GetMask("Worm"));

        if(isJumping)
        {
            rb.AddRelativeForce(jumpingForwardConstantForce);
        }

        if (isGrounded && !isJumping)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            rb.AddForce(Vector3.right * inputHorizontal * moveSpeed);
            if(Mathf.Abs(inputHorizontal) > 0.1f)
            {
                transform.eulerAngles = Vector3.up * (inputHorizontal > 0.0f ? 90f : -90f);
            }

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocityMagnitude);

            if(Input.GetKeyDown(KeyCode.Return))
            {
                Vector3 forceToAdd;

                //Jump with less forward momentum if against a wall
                if(Physics.Raycast(transform.position, transform.forward, 0.6f))
                {
                    forceToAdd = (Vector3.up * 2.2f + Vector3.forward * 0.2f);
                }
                else
                {
                    forceToAdd = (Vector3.up * 2.0f + Vector3.forward);
                }

                rb.velocity = Vector3.zero;
                rb.AddRelativeForce(forceToAdd * jumpForce, ForceMode.VelocityChange);
                isGrounded = false;
                isJumping = true;
            }
        }
	}
}
