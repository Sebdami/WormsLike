using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WormState
{
    Paused,
    Movement,
    Hit,
    WeaponHandled,
    Dead
}

public class WormController : MonoBehaviour {
    public delegate void StateChange(WormState oldState, WormState newState);

    public StateChange OnStateChange;

    RigidbodyConstraints basicConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    RigidbodyConstraints hitConstraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
    RigidbodyConstraints pausedConstraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
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

    private Weapon currentWeapon;

    float jumpTimer = 0.0f;

    Vector3 jumpingForwardConstantForce = Vector3.forward* 1.5f;

    bool isJumping = false;
    [SerializeField]
    bool isGrounded = false;

    CapsuleCollider capsuleCollider;

    public WormState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            WormState old = currentState;
            ExitState(currentState);
            if(currentState != value)
                previousState = currentState;
            currentState = value;
            EnterState(currentState);
            if(OnStateChange != null)
            {
                OnStateChange(old, currentState);
            }
        }
    }

    public Rigidbody Rb
    {
        get
        {
            if (!rb)
                rb = GetComponent<Rigidbody>();
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    public Weapon CurrentWeapon
    {
        get
        {
            if (!currentWeapon)
                currentWeapon = GetComponentInChildren<Weapon>();
            return currentWeapon;
        }

        set
        {
            currentWeapon = value;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }

    void Start () {
        character = GetComponent<CharacterInstance>();
        character.characterData.OnDeath += Die;
        Rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

    }

    void Die()
    {
        CurrentState = WormState.Dead;
        character.characterInfo.SetActive(false);
    }

    void UpdateIsGrounded()
    {
        RaycastHit hit;

        Physics.SphereCast(transform.position, 0.48f, Vector3.down, out hit, 1.1f, ~LayerMask.GetMask("WormTail"));

        isGrounded = (hit.collider != null && hit.collider != capsuleCollider);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + transform.forward * 0.48f, transform.position + transform.forward * 0.48f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position - transform.forward * 0.48f, transform.position - transform.forward * 0.48f + Vector3.down * 1.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.6f);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
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
                HandlePausedStateFixed();
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
                EnterPausedState();
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
                EnterDeadState();
                break;
        }
    }

    void ExitState(WormState state)
    {
        switch (state)
        {
            case WormState.Paused:
                ExitPausedState();
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

    #region PausedState
    void EnterPausedState()
    {
        // Set the mass to a high value so you can't be pushed
        Rb.constraints = pausedConstraints;
        transform.eulerAngles = Vector3.up * (transform.forward.x > 0 ? 90 : -90);
    }

    void ExitPausedState()
    {
        Rb.constraints = basicConstraints;
    }

    void HandlePausedStateFixed()
    {
        UpdateIsGrounded();
    }
    #endregion

    #region MovementState
    void EnterMovementState()
    {
        transform.eulerAngles = Vector3.up * (transform.forward.x > 0 ? 90 : -90);
    }

    void HandleMovementStateFixed()
    {
        UpdateIsGrounded();

        if (isJumping)
        {
            Rb.AddRelativeForce(jumpingForwardConstantForce);
        }

        if (isGrounded && !isJumping)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            Rb.AddForce(Vector3.right * inputHorizontal * moveSpeed);
            if (Mathf.Abs(inputHorizontal) > 0.1f)
            {
                transform.eulerAngles = Vector3.up * (inputHorizontal > 0.0f ? 90f : -90f);
            }


            float oldY = Rb.velocity.y;
            // Clamp Velocity Magnitude
            Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxVelocityMagnitude);
            // Don't clamp on Y
            Rb.velocity = new Vector3(Rb.velocity.x, oldY, Rb.velocity.z);

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

                Rb.velocity = Vector3.zero;
                Rb.AddRelativeForce(forceToAdd * jumpForce, ForceMode.VelocityChange);
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
        Rb.constraints = hitConstraints;
        hitTimeTimer = 0.0f;
    }

    void ExitHitState()
    {
        Rb.constraints = basicConstraints;
    }

    void HandleHitState()
    {
        hitTimeTimer += Time.deltaTime;
        if(hitTimeTimer >= maxHitTimeTimer && Rb.velocity.magnitude < 0.1f)
        {
            if (previousState == WormState.WeaponHandled)
                CurrentState = WormState.Movement;
            else
                CurrentState = previousState;
        }
    }
    #endregion

    #region DeadState
    void EnterDeadState()
    {
        rb.constraints = hitConstraints;
    }

    #endregion
}

