using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MovementScript : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D rb2D;
    [SerializeField] private float speed = 5f;

    private Vector2 moveDirection;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;

    // --- CAMBIO 1: Variables internas, no serializadas ---
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;
    // ---------------------------------------------------

    private bool canDash = true;
    private bool canMove = true;

    private bool isFacingRight = true;

    private Animator animator;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Movement"];
        dashAction = playerInput.actions["Dash"];
    }

    void Update()
    {
        if (canMove)
        {
            moveDirection = moveAction.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            AnimationHandler.MovementAnim(animator, moveDirection);
            rb2D.linearVelocity = new Vector2(moveDirection.x * speed, rb2D.linearVelocity.y);
        }
    }

    private void OnDashing(InputAction.CallbackContext context)
    {
        if (!canDash) return;

        string controlName = context.control.name.ToLower();
        float dashDirection = 0f;

        if (controlName.Contains("right")) dashDirection = 1f;
        else if (controlName.Contains("left")) dashDirection = -1f;

        bool correctRight = (dashDirection > 0 && isFacingRight);

        bool correctLeft = (dashDirection < 0 && !isFacingRight);

        if (correctRight || correctLeft)
        {
            StartCoroutine(DashingRoutine(dashDirection));
        }
    }

    private IEnumerator DashingRoutine(float direction)
    {
        canDash = false;
        canMove = false;

        AnimationHandler.DashAnim(animator);

        rb2D.linearVelocity = new Vector2(direction * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb2D.linearVelocity = Vector2.zero;

        canMove = true;
        canDash = true;
    }

    private void OnEnable()
    {
        dashAction.performed += OnDashing;

        dashAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        dashAction.performed -= OnDashing;

        dashAction.Disable();
        moveAction.Disable();
    }
}