using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour {
    enum WormState
    {
        Paused,
        Movement,
        Hit,
        WeaponHandled,
        Dead
    }

    CharacterInstance character;
    Rigidbody rb;
    [SerializeField]
    float moveSpeed = 100.0f;
    [SerializeField]
    float jumpForce = 4.0f;

    [SerializeField]
    float maxVelocityMagnitude = 10.0f;

    [SerializeField]
    float jumpCooldown = 1.0f;

    [SerializeField]
    WormState currentState;

    WormState previousState;

    float jumpTimer = 0.0f;

    Vector3 jumpingForwardConstantForce = Vector3.forward* 1.5f;

    bool isJumping = false;
    bool isGrounded = false;

    private WormState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            ExitState(currentState);
            if(currentState != value)
                previousState = currentState;
            currentState = value;
            EnterState(currentState);
        }
    }

    void Start () {
        character = GetComponent<CharacterInstance>();
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
        if(Input.GetKeyDown(KeyCode.F))
        {
            CurrentState = WormState.Hit;
        }

        switch (currentState)
        {
            case WormState.Paused:
                break;
            case WormState.Movement:
                break;
            case WormState.Hit:
                HandleHitState();
                break;
            case WormState.WeaponHandled:
                break;
            case WormState.Dead:
                break;

        }

    }

    void FixedUpdate () {
        switch(currentState)
        {
            case WormState.Paused:
                break;
            case WormState.Movement:
                HandleMovementStateFixed();
                break;
            case WormState.Hit:
                break;
            case WormState.WeaponHandled:
                break;
            case WormState.Dead:
                break;
        }
	}

    void EnterState(WormState state)
    {
        switch (state)
        {
            case WormState.Paused:
                break;
            case WormState.Movement:
                EnterMovementState();
                break;
            case WormState.Hit:
                EnterHitState();
                break;
            case WormState.WeaponHandled:
                break;
            case WormState.Dead:
                break;
        }
    }

    void ExitState(WormState state)
    {
        switch (state)
        {
            case WormState.Paused:
                break;
            case WormState.Movement:
                break;
            case WormState.Hit:
                ExitHitState();
                break;
            case WormState.WeaponHandled:
                break;
            case WormState.Dead:
                break;
        }
    }

    #region MovementState
    void EnterMovementState()
    {
        transform.eulerAngles = Vector3.up * (transform.forward.x > 0 ? 90 : -90);
    }

    void HandleMovementStateFixed()
    {
        isGrounded = Physics.Raycast(transform.position + transform.forward * 0.5f, Vector3.down, 1.1f, ~LayerMask.GetMask("WormTail")) 
                  || Physics.Raycast(transform.position - transform.forward * 0.5f, Vector3.down, 1.1f, ~LayerMask.GetMask("WormTail"));

        if (isJumping)
        {
            rb.AddRelativeForce(jumpingForwardConstantForce);
        }

        if (isGrounded && !isJumping)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            rb.AddForce(Vector3.right * inputHorizontal * moveSpeed);
            if (Mathf.Abs(inputHorizontal) > 0.1f)
            {
                transform.eulerAngles = Vector3.up * (inputHorizontal > 0.0f ? 90f : -90f);
            }

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocityMagnitude);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Vector3 forceToAdd;

                //Jump with less forward momentum if against a wall
                if (Physics.Raycast(transform.position, transform.forward, 0.6f))
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
    #endregion

    #region HitState
    [SerializeField]
    float maxHitTimeTimer = 3.0f;
    float hitTimeTimer = 0.0f;


    void EnterHitState()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        hitTimeTimer = 0.0f;
    }

    void ExitHitState()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void HandleHitState()
    {
        hitTimeTimer += Time.deltaTime;
        if(hitTimeTimer >= maxHitTimeTimer)
        {
            CurrentState = previousState;
        }
    }
    #endregion
}

